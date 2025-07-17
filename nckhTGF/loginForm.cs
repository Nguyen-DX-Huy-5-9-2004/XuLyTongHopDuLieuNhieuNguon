using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace nckhTGF
{
    public partial class loginForm : DevExpress.XtraEditors.XtraForm
    {
        public static string TypeUser { get; private set; }
        public static string CodeUser { get; private set; }
        public static string PassUser { get; private set; }
        public static string RMBCode { get; private set; }
        public static string RMBPass { get; private set; }
        public loginForm()
        {
            InitializeComponent();
            typ.SelectedIndex = 0;
        }

        private string connectionString = "Server=TUGEND-FAKAS\\TGF_SQLSEVER;Database=dataNCKH2;User Id=sa;Password=Huy@0865567532;";

        private bool AuthenticateUser(string username, string password)
        {
            bool isAuthenticated = false;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "";
                SqlCommand cmd = null;

                if (typ.SelectedItem != null)
                {
                    string userType = typ.SelectedItem.ToString();
                    if (userType == "Sinh viên")
                    {
                        loginForm.TypeUser = "SV";
                        query = "SELECT COUNT(*) FROM dbo.accSV WHERE [Mã sinh viên] = @username AND MK = @password";
                        cmd = new SqlCommand(query, conn);
                    }
                    else if (userType == "Giảng viên")
                    {
                        loginForm.TypeUser = "GV";
                        query = "SELECT COUNT(*) FROM dbo.accGV WHERE [Mã giảng viên] = @username AND MK = @password";
                        cmd = new SqlCommand(query, conn);
                    }

                    if (cmd != null)
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", password);
                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();
                        conn.Close();

                        if (count > 0)
                        {
                            isAuthenticated = true;
                            loginForm.CodeUser = username;
                            loginForm.PassUser = password;
                        }
                        //MessageBox.Show(query);
                    }
                }
                else
                {
                    MessageBox.Show("Vui lòng chọn loại tài khoản.");
                }
            }

            return isAuthenticated;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string username = userIn.Text;
            string password = passIn.Text;

            if (AuthenticateUser(username, password))
            {
                loaddin loadingForm = new loaddin();
                loadingForm.StartPosition = FormStartPosition.CenterScreen;
                loadingForm.Show();

                await Task.Delay(3000);

                loadingForm.Close();
                var result = MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (toggleSwitch1.IsOn)
                {
                    loginForm.RMBCode = username;
                    loginForm.RMBPass = password;
                }
                else
                {
                    loginForm.RMBCode = null;
                    loginForm.RMBPass = null;
                }

                if (result == DialogResult.OK)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private  void LoginClientClick(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Đăng nhập với tư cách khách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            loginForm.TypeUser = "Client";
            loginForm.CodeUser = "";
            loginForm.PassUser = "";
            loginForm.RMBCode = null;
            loginForm.RMBPass = null;



            //loaddin loadingForm = new loaddin();
            //loadingForm.StartPosition = FormStartPosition.CenterScreen;
            //loadingForm.Show();

            //await Task.Delay(2000);

            //loadingForm.Close();


            if (result == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        public void loginForm_Load(object sender, EventArgs e)
        {
            if(loginForm.RMBCode != null && loginForm.RMBPass != null)
            {
                userIn.Text = loginForm.RMBCode;
                passIn.Text = loginForm.RMBPass;
                passIn.Properties.PasswordChar = '*';
            }
        }
    }
}