namespace nckhTGF
{
    partial class FormCNDBmySQL
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCNDBmySQL));
            txtPort = new DevExpress.XtraEditors.TextEdit();
            simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            labelControl4 = new DevExpress.XtraEditors.LabelControl();
            severIn = new DevExpress.XtraEditors.TextEdit();
            userIn = new DevExpress.XtraEditors.TextEdit();
            passIn = new DevExpress.XtraEditors.TextEdit();
            labelControl3 = new DevExpress.XtraEditors.LabelControl();
            labelControl2 = new DevExpress.XtraEditors.LabelControl();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            CN = new DevExpress.XtraEditors.SimpleButton();
            notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);
            labelControl6 = new DevExpress.XtraEditors.LabelControl();
            labelControl5 = new DevExpress.XtraEditors.LabelControl();
            checkedComboBoxTB = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            comboBoxDBMySQL = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)txtPort.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)severIn.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)userIn.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)passIn.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)checkedComboBoxTB.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)comboBoxDBMySQL.Properties).BeginInit();
            SuspendLayout();
            // 
            // txtPort
            // 
            txtPort.EditValue = "3306";
            txtPort.Location = new System.Drawing.Point(99, 137);
            txtPort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtPort.Name = "txtPort";
            txtPort.Size = new System.Drawing.Size(199, 22);
            txtPort.TabIndex = 39;
            // 
            // simpleButton1
            // 
            simpleButton1.Location = new System.Drawing.Point(304, 208);
            simpleButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            simpleButton1.Name = "simpleButton1";
            simpleButton1.Size = new System.Drawing.Size(61, 36);
            simpleButton1.TabIndex = 35;
            simpleButton1.Text = "Load";
            simpleButton1.Click += LoadDatabaseClick;
            // 
            // labelControl4
            // 
            labelControl4.Location = new System.Drawing.Point(38, 184);
            labelControl4.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            labelControl4.Name = "labelControl4";
            labelControl4.Size = new System.Drawing.Size(55, 16);
            labelControl4.TabIndex = 33;
            labelControl4.Text = "DB. name";
            // 
            // severIn
            // 
            severIn.Location = new System.Drawing.Point(99, 24);
            severIn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            severIn.Name = "severIn";
            severIn.Size = new System.Drawing.Size(199, 22);
            severIn.TabIndex = 32;
            // 
            // userIn
            // 
            userIn.EditValue = "tgf5";
            userIn.Location = new System.Drawing.Point(99, 63);
            userIn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            userIn.Name = "userIn";
            userIn.Size = new System.Drawing.Size(199, 22);
            userIn.TabIndex = 31;
            // 
            // passIn
            // 
            passIn.EditValue = "Huy@0865567532";
            passIn.Location = new System.Drawing.Point(99, 102);
            passIn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            passIn.Name = "passIn";
            passIn.Size = new System.Drawing.Size(199, 22);
            passIn.TabIndex = 30;
            // 
            // labelControl3
            // 
            labelControl3.Location = new System.Drawing.Point(38, 109);
            labelControl3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            labelControl3.Name = "labelControl3";
            labelControl3.Size = new System.Drawing.Size(55, 16);
            labelControl3.TabIndex = 29;
            labelControl3.Text = "Password";
            // 
            // labelControl2
            // 
            labelControl2.Location = new System.Drawing.Point(38, 70);
            labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            labelControl2.Name = "labelControl2";
            labelControl2.Size = new System.Drawing.Size(26, 16);
            labelControl2.TabIndex = 28;
            labelControl2.Text = "User";
            // 
            // labelControl1
            // 
            labelControl1.Location = new System.Drawing.Point(38, 32);
            labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new System.Drawing.Size(33, 16);
            labelControl1.TabIndex = 27;
            labelControl1.Text = "Sever";
            // 
            // CN
            // 
            CN.Location = new System.Drawing.Point(304, 67);
            CN.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            CN.Name = "CN";
            CN.Size = new System.Drawing.Size(61, 59);
            CN.TabIndex = 26;
            CN.Text = "Connect\r\nSever";
            CN.Click += CN_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (System.Drawing.Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "ConnectSqlSever";
            notifyIcon1.Visible = true;
            // 
            // labelControl6
            // 
            labelControl6.Location = new System.Drawing.Point(38, 144);
            labelControl6.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            labelControl6.Name = "labelControl6";
            labelControl6.Size = new System.Drawing.Size(23, 16);
            labelControl6.TabIndex = 38;
            labelControl6.Text = "Port";
            // 
            // labelControl5
            // 
            labelControl5.Location = new System.Drawing.Point(39, 220);
            labelControl5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            labelControl5.Name = "labelControl5";
            labelControl5.Size = new System.Drawing.Size(32, 16);
            labelControl5.TabIndex = 37;
            labelControl5.Text = "Table";
            // 
            // checkedComboBoxTB
            // 
            checkedComboBoxTB.EditValue = "Tên bảng";
            checkedComboBoxTB.Location = new System.Drawing.Point(99, 217);
            checkedComboBoxTB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            checkedComboBoxTB.Name = "checkedComboBoxTB";
            checkedComboBoxTB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            checkedComboBoxTB.Size = new System.Drawing.Size(199, 22);
            checkedComboBoxTB.TabIndex = 36;
            // 
            // comboBoxDBMySQL
            // 
            comboBoxDBMySQL.Location = new System.Drawing.Point(101, 179);
            comboBoxDBMySQL.Name = "comboBoxDBMySQL";
            comboBoxDBMySQL.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            comboBoxDBMySQL.Size = new System.Drawing.Size(197, 22);
            comboBoxDBMySQL.TabIndex = 40;
            comboBoxDBMySQL.SelectedIndexChanged += comboBoxDBChange;
            // 
            // FormCNDBmySQL
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(387, 268);
            Controls.Add(comboBoxDBMySQL);
            Controls.Add(txtPort);
            Controls.Add(simpleButton1);
            Controls.Add(labelControl4);
            Controls.Add(severIn);
            Controls.Add(userIn);
            Controls.Add(passIn);
            Controls.Add(labelControl3);
            Controls.Add(labelControl2);
            Controls.Add(labelControl1);
            Controls.Add(CN);
            Controls.Add(labelControl6);
            Controls.Add(labelControl5);
            Controls.Add(checkedComboBoxTB);
            Name = "FormCNDBmySQL";
            Text = "Kết nối cơ sở dữ liệu MySQL";
            ((System.ComponentModel.ISupportInitialize)txtPort.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)severIn.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)userIn.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)passIn.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)checkedComboBoxTB.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)comboBoxDBMySQL.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraEditors.TextEdit txtPort;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxDB;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.TextEdit severIn;
        private DevExpress.XtraEditors.TextEdit userIn;
        private DevExpress.XtraEditors.TextEdit passIn;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton CN;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.CheckedComboBoxEdit checkedComboBoxTB;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxDBMySQL;
    }
}