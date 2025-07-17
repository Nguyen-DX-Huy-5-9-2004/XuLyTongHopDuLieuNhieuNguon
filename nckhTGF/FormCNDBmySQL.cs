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

namespace nckhTGF
{
    public partial class FormCNDBmySQL : DevExpress.XtraEditors.XtraForm
    {
        public static string MySQLConnectionString { get; private set; }
        public FormCNDBmySQL()
        {
            InitializeComponent();
        }

        private void CN_Click(object sender, EventArgs e)
        {
            string server = severIn.Text.Trim();
            string user = userIn.Text.Trim();
            string password = passIn.Text.Trim();
            string database = comboBoxDBMySQL.Text.Trim();
            string port = txtPort.Text.Trim();

            if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(port) || string.IsNullOrEmpty(user))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin kết nối!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = $"Server={server};Port={port};User ID={user};Password={password};";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("Kết nối MySQL thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MySQLConnectionString = connectionString;
                    LoadDatabases(connection); // Gọi hàm để tải danh sách database
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kết nối MySQL: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadDatabases(MySqlConnection connection)
        {
            try
            {
                string query = "SHOW DATABASES;";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                comboBoxDBMySQL.Properties.Items.Clear(); // Xóa danh sách cũ
                while (reader.Read())
                {
                    comboBoxDBMySQL.Properties.Items.Add(reader[0].ToString());
                }
                reader.Close();

                if (comboBoxDBMySQL.Properties.Items.Count > 0)
                {
                    comboBoxDBMySQL.SelectedIndex = 0; // Chọn database đầu tiên mặc định
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBoxDBChange(object sender, EventArgs e)
        {
            if (comboBoxDBMySQL.SelectedItem == null)
                return;

            string selectedDatabase = comboBoxDBMySQL.SelectedItem.ToString();

            if (string.IsNullOrEmpty(MySQLConnectionString))
            {
                MessageBox.Show("Chuỗi kết nối không hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Cập nhật lại chuỗi kết nối để chọn đúng database
            string updatedConnectionString = MySQLConnectionString + $"Database={selectedDatabase};";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(updatedConnectionString))
                {
                    connection.Open();
                    LoadTables(connection); // Gọi hàm để cập nhật danh sách bảng
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kết nối đến database: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void LoadTables(MySqlConnection connection)//lấy danh sách các banggr 
        {
            try
            {
                string query = "SHOW TABLES;";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader reader = cmd.ExecuteReader();

                checkedComboBoxTB.Properties.Items.Clear(); // Xóa danh sách cũ

                List<string> tableNames = new List<string>();

                while (reader.Read())
                {
                    string tableName = reader[0].ToString(); // Cột đầu tiên là tên bảng
                    tableNames.Add(tableName);
                    checkedComboBoxTB.Properties.Items.Add(tableName);
                }
                reader.Close();

                if (tableNames.Count > 0)
                {
                    checkedComboBoxTB.SetEditValue(tableNames[0]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách bảng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadDatabaseClick(object sender, EventArgs e)
        {
            string server = severIn.Text.Trim();
            string user = userIn.Text.Trim();
            string password = passIn.Text.Trim();
            string database = comboBoxDBMySQL.Text.Trim();
            string port = txtPort.Text.Trim();

            string connectionString = $"Server={server};Port={port};Database={database};User ID={user};Password={password};SslMode=none;";

            string selectedTables = checkedComboBoxTB.EditValue?.ToString();
            if (string.IsNullOrEmpty(selectedTables))
            {
                MessageBox.Show("Vui lòng chọn ít nhất một bảng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string[] tableList = selectedTables
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToArray();

            // Tìm form chính (getData) và gọi phương thức load dữ liệu
            getData mainForm = Application.OpenForms.OfType<getData>().FirstOrDefault();
            if (mainForm != null)
            {
                try
                {
                    mainForm.LoadMySQLTablesToSpreadsheet(connectionString, tableList);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy form chính!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Hide(); // Ẩn form sau khi xử lý
        }
    }
}