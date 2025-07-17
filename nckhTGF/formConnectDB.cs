using DevExpress.XtraEditors;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using connectSqlSever;
using System.Net;
using System.Net.Sockets;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using DevExpress.PivotGrid.CriteriaVisitors;


namespace nckhTGF
{
    public partial class formConnectDB : DevExpress.XtraEditors.XtraForm
    {
        public string ConnectionString { get; private set; }
        public string SelectedDatabase { get; private set; }
        public static string ConnectionStringDB { get; set; }
        public static string nameTB { get; set; }
        public formConnectDB()
        {
            InitializeComponent();
            this.Resize += FormConnectDB_Resize;
            //MessageBox.Show(loginForm.TypeUser);
            resetInfor();
            if (loginForm.TypeUser == "Client")
            {
                btnQuickCn.Enabled = false;
                txtMSVMGV.Enabled = false;
                txtMK.Enabled = false;
            }
            else
            {
                txtMSVMGV.Text = loginForm.CodeUser;
            }
        }

        private void FormConnectDB_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                //notifyIcon1.Visible = true; // Hiện NotifyIcon trong system tray
                //notifyIcon1.ShowBalloonTip(1000, "Ứng dụng chạy nền", "Click vào icon để mở lại", ToolTipIcon.Info);
                this.Hide();
            }
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            notifyIcon1.Visible = false; 
        }
        public void resetInfor()
        {
            severIn.Reset();
            userIn.Reset();
            passIn.Reset();
            comboBoxDB.Clear();
            checkedComboBoxTB.Clear();
            txtQuery.ResetText();
            comboBoxDB.Text= "Tên database";
            checkedComboBoxTB.Text = "Tên bảng";
        }
        private void connectSeverClick(object sender, EventArgs e)
        {
            string server = severIn.Text.Trim();
            string user = userIn.Text.Trim();
            string password = passIn.Text.Trim();
            if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
            {
                XtraMessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            String ConnectionString = $"Server={server};User Id={user};Password={password};";
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    DataTable dt = conn.GetSchema("Databases");
                    btnRunQuery.Enabled = true;
                    comboBoxDB.Properties.Items.Clear();
                    foreach (DataRow row in dt.Rows)
                    {
                        comboBoxDB.Properties.Items.Add(row["database_name"].ToString());
                    }

                    if (comboBoxDB.Properties.Items.Count > 0)
                        comboBoxDB.SelectedIndex = 0; // Chọn database đầu tiên

                    XtraMessageBox.Show("Kết nối thành công! Chọn database để tiếp tục.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi kết nối: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void autoUpdateTableName(object sender, EventArgs e)
        {
            string server = severIn.Text.Trim();
            string user = userIn.Text.Trim();
            string password = passIn.Text.Trim();
            String dtbn = comboBoxDB.Text.Trim();
            //String tablname = TBname.Text;

            //string connectionString = "Data Source=TUGEND-FAKAS\\TGFSQLSEVER ;Initial Catalog=test ;User ID="+username+";Password="+password;
            string connectionString = "Data Source=" + server + " ;Initial Catalog=" + dtbn + ";User ID=" + user + ";Password=" + password;
            //string connectionString = "Server="+ID+";Integrated Security=true;";
            //string connectionString = "Server=" + ID + ";User Id=" + username + ";Password=" + password;
            //MessageBox.Show(connectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT name FROM sys.tables", connection);
                    SqlDataReader reader = command.ExecuteReader();

                    checkedComboBoxTB.Properties.Items.Clear(); // Xóa danh sách cũ
                    List<string> tableNames = new List<string>();

                    while (reader.Read())
                    {
                        string tableName = reader["name"].ToString();
                        checkedComboBoxTB.Properties.Items.Add(tableName);
                        tableNames.Add(tableName); // Lưu danh sách tên bảng
                    }

                    if (tableNames.Count > 0)
                    {
                        checkedComboBoxTB.SetEditValue(tableNames[0]); // Hiển thị bảng đầu tiên
                    }

                    // MessageBox.Show("Kết nối thành công và đã tải danh sách các bảng!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi kết nối server: " + ex.Message);
                }
            }
        }

        private void loadDataClick(object sender, EventArgs e)
        {
            string server = severIn.Text.Trim();
            string user = userIn.Text.Trim();
            string password = passIn.Text.Trim();
            String dtbn = comboBoxDB.Text.Trim();
            formConnectDB.ConnectionStringDB = "Server= " + server + ";Database=" + dtbn + ";User ID=" + user + ";Password=" + password + "; ";
            //string connectionString = $"Server={sever.};Database={txtDatabase.Text};User Id={txtUser.Text};Password={txtPassword.Text};";
            string connectionString = "Data Source=  " + server + ";Initial Catalog=" + dtbn + ";User ID=" + user + ";Password=" + password;
            // Lấy danh sách bảng đã chọn từ CheckedComboBoxEdit
            string selectedTables = checkedComboBoxTB.EditValue?.ToString();
            if (string.IsNullOrEmpty(selectedTables))
            {
                MessageBox.Show("Vui lòng chọn ít nhất một bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string[] tableList = selectedTables.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                               .Select(t => t.Trim()).ToArray();
            //XtraMessageBox.Show("tìm form chính");
            // Tìm form chính (MainForm) và gọi phương thức load dữ liệu
            getData mainForm = Application.OpenForms.OfType<getData>().FirstOrDefault();
            if (mainForm != null)
            {
                //XtraMessageBox.Show("tìm form chính xong");
                //XtraMessageBox.Show(connectionString);
                mainForm.LoadDataToSpreadsheet(connectionString, tableList);

            }
            else
            {
                MessageBox.Show("Không tìm thấy MainForm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Hide();
        }

        private void btnRunQuery_Click(object sender, EventArgs e)
        {
            string server = severIn.Text.Trim();
            string user = userIn.Text.Trim();
            string password = passIn.Text.Trim();
            string dtbn = comboBoxDB.Text.Trim();
            string query = txtQuery.Text.Trim();

            string connectionString = $"Server={server};Database={dtbn};User ID={user};Password={password};";

            if (string.IsNullOrWhiteSpace(query))
            {
                MessageBox.Show("Vui lòng nhập câu truy vấn SQL!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(query, conn);
                    if (query.Trim().StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        MessageBox.Show("Truy vấn SELECT thực thi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        var mainForm = Application.OpenForms.OfType<getData>().FirstOrDefault();
                        if (mainForm != null)
                        {
                            mainForm.LoadDataToSpreadsheet(dt, "QueryResult");
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy form chính!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();
                        MessageBox.Show($"Đã thực thi truy vấn thành công. Số dòng bị ảnh hưởng: {rowsAffected}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thực hiện truy vấn:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void quickCnClick(object sender, EventArgs e)
        {
            if (txtMK.Text == loginForm.PassUser)
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        severIn.Text = ip.ToString();
                        break;
                    }
                }
                userIn.Text = "sa";
                passIn.Text = "Huy@0865567532";
                passIn.Properties.UseSystemPasswordChar = true;
                connectSeverClick(sender, e);
                //comboBoxDB.SelectedText = ;
            }
            else
            {
                MessageBox.Show("Sai mật khẩu, vui lòng nhập lại!", "Load data fail!");
            }
        }

        private void getIPClick(object sender, EventArgs e)
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    severIn.Text = ip.ToString();
                    break;
                }
            }
        }
    }
}

//string server = severIn.Text.Trim();
//string user = userIn.Text.Trim();
//string password = passIn.Text.Trim();
//String dtbn = comboBoxDB.Text.Trim();

//string connectionString = "Server=" + server + ";Database=" + dtbn + ";User ID=" + user + ";Password=" + password + ";";

//string query = txtQuery.Text.Trim();

//if (string.IsNullOrWhiteSpace(query))
//{
//    MessageBox.Show("Vui lòng nhập câu truy vấn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//    return;
//}

//try
//{
//    using (SqlConnection conn = new SqlConnection(connectionString))
//    {
//        conn.Open();
//        SqlCommand cmd = new SqlCommand(query, conn);

//        if (query.ToLower().StartsWith("select"))
//        {
//            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
//            DataTable result = new DataTable();
//            adapter.Fill(result);

//            if (result.Rows.Count == 0)
//            {
//                MessageBox.Show("Truy vấn thành công nhưng không có dữ liệu.", "Thông báo");
//                return;
//            }

//            // Gửi dữ liệu về Form chính
//            getData mainForm = Application.OpenForms.OfType<getData>().FirstOrDefault();
//            if (mainForm != null)
//            {
//                mainForm.LoadQueryResultToSpreadsheet(result, "SheetQuery");
//            }
//            else
//            {
//                MessageBox.Show("Không tìm thấy form chính!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//            }
//        }
//        else
//        {
//            int rows = cmd.ExecuteNonQuery();
//            MessageBox.Show($"Truy vấn thực thi thành công. {rows} dòng bị ảnh hưởng.", "Thành công");
//        }
//    }
//}
//catch (Exception ex)
//{
//    MessageBox.Show("Lỗi khi thực thi truy vấn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//}