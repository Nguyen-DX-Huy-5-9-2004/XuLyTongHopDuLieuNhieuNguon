using DevExpress.Spreadsheet;
using DevExpress.XtraSpreadsheet;
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

namespace connectSqlSever
{
    public partial class InputBox : Form
    {
        public event Action<string> OnMySQLConnected; // Sự kiện gửi connectionString

        public string InputText => txtInput.Text;
        public InputBox(string prompt, string defaultValue = "")
        {
            InitializeComponent();
            lblPrompt.Text = prompt;
            txtInput.Text = defaultValue;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            

        }
    }
}
