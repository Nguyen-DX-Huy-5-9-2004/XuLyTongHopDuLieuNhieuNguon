namespace nckhTGF
{
    partial class loginForm
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
            btnLogin = new System.Windows.Forms.Button();
            typ = new DevExpress.XtraEditors.ComboBoxEdit();
            toggleSwitch1 = new DevExpress.XtraEditors.ToggleSwitch();
            passIn = new DevExpress.XtraEditors.TextEdit();
            userIn = new DevExpress.XtraEditors.TextEdit();
            labelControl2 = new DevExpress.XtraEditors.LabelControl();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)typ.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)toggleSwitch1.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)passIn.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)userIn.Properties).BeginInit();
            SuspendLayout();
            // 
            // btnLogin
            // 
            btnLogin.Location = new System.Drawing.Point(229, 125);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new System.Drawing.Size(94, 29);
            btnLogin.TabIndex = 14;
            btnLogin.Text = "&Đăng nhập";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // typ
            // 
            typ.EditValue = "Đối tượng";
            typ.Location = new System.Drawing.Point(163, 87);
            typ.Name = "typ";
            typ.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            typ.Properties.Items.AddRange(new object[] { "Sinh viên", "Giảng viên" });
            typ.Size = new System.Drawing.Size(106, 22);
            typ.TabIndex = 13;
            // 
            // toggleSwitch1
            // 
            toggleSwitch1.Location = new System.Drawing.Point(287, 87);
            toggleSwitch1.Name = "toggleSwitch1";
            toggleSwitch1.Properties.OffText = "Duy trì đăng nhập";
            toggleSwitch1.Properties.OnText = "Duy trì đăng nhập";
            toggleSwitch1.Size = new System.Drawing.Size(164, 24);
            toggleSwitch1.TabIndex = 12;
            // 
            // passIn
            // 
            passIn.Location = new System.Drawing.Point(163, 59);
            passIn.Name = "passIn";
            passIn.Size = new System.Drawing.Size(288, 22);
            passIn.TabIndex = 11;
            // 
            // userIn
            // 
            userIn.Location = new System.Drawing.Point(163, 31);
            userIn.Name = "userIn";
            userIn.Size = new System.Drawing.Size(288, 22);
            userIn.TabIndex = 10;
            userIn.ToolTip = "Mã sinh viên/Mã giảng viên";
            // 
            // labelControl2
            // 
            labelControl2.LineVisible = true;
            labelControl2.Location = new System.Drawing.Point(105, 62);
            labelControl2.Name = "labelControl2";
            labelControl2.Size = new System.Drawing.Size(52, 16);
            labelControl2.TabIndex = 9;
            labelControl2.Text = "Mật khẩu";
            // 
            // labelControl1
            // 
            labelControl1.LineVisible = true;
            labelControl1.Location = new System.Drawing.Point(72, 34);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new System.Drawing.Size(86, 16);
            labelControl1.TabIndex = 8;
            labelControl1.Text = "Tên đăng nhập";
            // 
            // simpleButton1
            // 
            simpleButton1.Location = new System.Drawing.Point(388, 125);
            simpleButton1.Name = "simpleButton1";
            simpleButton1.Size = new System.Drawing.Size(63, 29);
            simpleButton1.TabIndex = 15;
            simpleButton1.Text = "Khách";
            simpleButton1.Click += LoginClientClick;
            // 
            // loginForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(549, 209);
            Controls.Add(simpleButton1);
            Controls.Add(btnLogin);
            Controls.Add(typ);
            Controls.Add(toggleSwitch1);
            Controls.Add(passIn);
            Controls.Add(userIn);
            Controls.Add(labelControl2);
            Controls.Add(labelControl1);
            Name = "loginForm";
            Text = "Đăng nhập";
            Load += loginForm_Load;
            ((System.ComponentModel.ISupportInitialize)typ.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)toggleSwitch1.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)passIn.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)userIn.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnLogin;
        private DevExpress.XtraEditors.ComboBoxEdit typ;
        private DevExpress.XtraEditors.ToggleSwitch toggleSwitch1;
        private DevExpress.XtraEditors.TextEdit passIn;
        private DevExpress.XtraEditors.TextEdit userIn;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}