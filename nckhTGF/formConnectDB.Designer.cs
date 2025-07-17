namespace nckhTGF
{
    partial class formConnectDB
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formConnectDB));
            simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            comboBoxDB = new DevExpress.XtraEditors.ComboBoxEdit();
            labelControl4 = new DevExpress.XtraEditors.LabelControl();
            severIn = new DevExpress.XtraEditors.TextEdit();
            userIn = new DevExpress.XtraEditors.TextEdit();
            passIn = new DevExpress.XtraEditors.TextEdit();
            labelControl3 = new DevExpress.XtraEditors.LabelControl();
            labelControl2 = new DevExpress.XtraEditors.LabelControl();
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            CN = new DevExpress.XtraEditors.SimpleButton();
            notifyIcon1 = new System.Windows.Forms.NotifyIcon(components);
            btnRunQuery = new DevExpress.XtraEditors.SimpleButton();
            label1 = new System.Windows.Forms.Label();
            txtQuery = new System.Windows.Forms.TextBox();
            labelControl5 = new DevExpress.XtraEditors.LabelControl();
            checkedComboBoxTB = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            btnQuickCn = new DevExpress.XtraEditors.SimpleButton();
            txtMK = new DevExpress.XtraEditors.TextEdit();
            labelControl6 = new DevExpress.XtraEditors.LabelControl();
            labelControl7 = new DevExpress.XtraEditors.LabelControl();
            txtMSVMGV = new DevExpress.XtraEditors.TextEdit();
            btnGetIP = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)comboBoxDB.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)severIn.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)userIn.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)passIn.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)checkedComboBoxTB.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtMK.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtMSVMGV.Properties).BeginInit();
            SuspendLayout();
            // 
            // simpleButton1
            // 
            simpleButton1.Location = new System.Drawing.Point(294, 185);
            simpleButton1.Name = "simpleButton1";
            simpleButton1.Size = new System.Drawing.Size(71, 29);
            simpleButton1.TabIndex = 25;
            simpleButton1.Text = "Load";
            simpleButton1.Click += loadDataClick;
            // 
            // comboBoxDB
            // 
            comboBoxDB.EditValue = "Tên database";
            comboBoxDB.Location = new System.Drawing.Point(89, 170);
            comboBoxDB.Name = "comboBoxDB";
            comboBoxDB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            comboBoxDB.Size = new System.Drawing.Size(199, 22);
            comboBoxDB.TabIndex = 24;
            comboBoxDB.SelectedIndexChanged += autoUpdateTableName;
            // 
            // labelControl4
            // 
            labelControl4.Location = new System.Drawing.Point(28, 173);
            labelControl4.Name = "labelControl4";
            labelControl4.Size = new System.Drawing.Size(55, 16);
            labelControl4.TabIndex = 23;
            labelControl4.Text = "DB. name";
            // 
            // severIn
            // 
            severIn.Location = new System.Drawing.Point(89, 76);
            severIn.Name = "severIn";
            severIn.Size = new System.Drawing.Size(199, 22);
            severIn.TabIndex = 22;
            // 
            // userIn
            // 
            userIn.EditValue = "";
            userIn.Location = new System.Drawing.Point(89, 107);
            userIn.Name = "userIn";
            userIn.Size = new System.Drawing.Size(199, 22);
            userIn.TabIndex = 21;
            // 
            // passIn
            // 
            passIn.EditValue = "";
            passIn.Location = new System.Drawing.Point(89, 139);
            passIn.Name = "passIn";
            passIn.Size = new System.Drawing.Size(199, 22);
            passIn.TabIndex = 20;
            // 
            // labelControl3
            // 
            labelControl3.Location = new System.Drawing.Point(28, 141);
            labelControl3.Name = "labelControl3";
            labelControl3.Size = new System.Drawing.Size(53, 16);
            labelControl3.TabIndex = 19;
            labelControl3.Text = "PassWDB";
            // 
            // labelControl2
            // 
            labelControl2.Location = new System.Drawing.Point(28, 110);
            labelControl2.Name = "labelControl2";
            labelControl2.Size = new System.Drawing.Size(41, 16);
            labelControl2.TabIndex = 18;
            labelControl2.Text = "UserDB";
            // 
            // labelControl1
            // 
            labelControl1.Location = new System.Drawing.Point(28, 82);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new System.Drawing.Size(33, 16);
            labelControl1.TabIndex = 17;
            labelControl1.Text = "Sever";
            // 
            // CN
            // 
            CN.Location = new System.Drawing.Point(294, 125);
            CN.Name = "CN";
            CN.Size = new System.Drawing.Size(71, 47);
            CN.TabIndex = 16;
            CN.Text = "Connect\r\nSever";
            CN.Click += connectSeverClick;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (System.Drawing.Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "ConnectSqlSever";
            notifyIcon1.Visible = true;
            // 
            // btnRunQuery
            // 
            btnRunQuery.Appearance.BorderColor = System.Drawing.Color.Black;
            btnRunQuery.Appearance.Options.UseBorderColor = true;
            btnRunQuery.Appearance.Options.UseImage = true;
            btnRunQuery.Enabled = false;
            btnRunQuery.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.TopCenter;
            btnRunQuery.ImageOptions.SvgImage = (DevExpress.Utils.Svg.SvgImage)resources.GetObject("btnRunQuery.ImageOptions.SvgImage");
            btnRunQuery.Location = new System.Drawing.Point(632, 148);
            btnRunQuery.Name = "btnRunQuery";
            btnRunQuery.Size = new System.Drawing.Size(71, 69);
            btnRunQuery.TabIndex = 30;
            btnRunQuery.Text = "Run Query";
            btnRunQuery.Click += btnRunQuery_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Tahoma", 7.8F, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, 0);
            label1.Location = new System.Drawing.Point(371, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(51, 16);
            label1.TabIndex = 29;
            label1.Text = "Query:";
            // 
            // txtQuery
            // 
            txtQuery.Location = new System.Drawing.Point(371, 19);
            txtQuery.Multiline = true;
            txtQuery.Name = "txtQuery";
            txtQuery.Size = new System.Drawing.Size(335, 201);
            txtQuery.TabIndex = 28;
            // 
            // labelControl5
            // 
            labelControl5.Location = new System.Drawing.Point(29, 198);
            labelControl5.Name = "labelControl5";
            labelControl5.Size = new System.Drawing.Size(32, 16);
            labelControl5.TabIndex = 27;
            labelControl5.Text = "Table";
            // 
            // checkedComboBoxTB
            // 
            checkedComboBoxTB.EditValue = "Tên bảng";
            checkedComboBoxTB.Location = new System.Drawing.Point(89, 198);
            checkedComboBoxTB.Name = "checkedComboBoxTB";
            checkedComboBoxTB.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            checkedComboBoxTB.Size = new System.Drawing.Size(199, 22);
            checkedComboBoxTB.TabIndex = 26;
            // 
            // btnQuickCn
            // 
            btnQuickCn.Location = new System.Drawing.Point(294, 19);
            btnQuickCn.Name = "btnQuickCn";
            btnQuickCn.Size = new System.Drawing.Size(71, 53);
            btnQuickCn.TabIndex = 31;
            btnQuickCn.Text = "Quick\r\nConnect";
            btnQuickCn.Click += quickCnClick;
            // 
            // txtMK
            // 
            txtMK.Location = new System.Drawing.Point(89, 48);
            txtMK.Name = "txtMK";
            txtMK.Size = new System.Drawing.Size(199, 22);
            txtMK.TabIndex = 32;
            // 
            // labelControl6
            // 
            labelControl6.Location = new System.Drawing.Point(29, 51);
            labelControl6.Name = "labelControl6";
            labelControl6.Size = new System.Drawing.Size(17, 16);
            labelControl6.TabIndex = 33;
            labelControl6.Text = "MK";
            // 
            // labelControl7
            // 
            labelControl7.Location = new System.Drawing.Point(29, 22);
            labelControl7.Name = "labelControl7";
            labelControl7.Size = new System.Drawing.Size(57, 16);
            labelControl7.TabIndex = 35;
            labelControl7.Text = "MSV/MGV";
            // 
            // txtMSVMGV
            // 
            txtMSVMGV.Location = new System.Drawing.Point(89, 19);
            txtMSVMGV.Name = "txtMSVMGV";
            txtMSVMGV.Size = new System.Drawing.Size(199, 22);
            txtMSVMGV.TabIndex = 34;
            // 
            // btnGetIP
            // 
            btnGetIP.Location = new System.Drawing.Point(294, 79);
            btnGetIP.Name = "btnGetIP";
            btnGetIP.Size = new System.Drawing.Size(71, 36);
            btnGetIP.TabIndex = 36;
            btnGetIP.Text = "Get IP";
            btnGetIP.Click += getIPClick;
            // 
            // formConnectDB
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(718, 232);
            Controls.Add(btnGetIP);
            Controls.Add(labelControl7);
            Controls.Add(txtMSVMGV);
            Controls.Add(labelControl6);
            Controls.Add(txtMK);
            Controls.Add(btnQuickCn);
            Controls.Add(simpleButton1);
            Controls.Add(comboBoxDB);
            Controls.Add(labelControl4);
            Controls.Add(severIn);
            Controls.Add(userIn);
            Controls.Add(passIn);
            Controls.Add(labelControl3);
            Controls.Add(labelControl2);
            Controls.Add(labelControl1);
            Controls.Add(CN);
            Controls.Add(btnRunQuery);
            Controls.Add(label1);
            Controls.Add(txtQuery);
            Controls.Add(labelControl5);
            Controls.Add(checkedComboBoxTB);
            Name = "formConnectDB";
            Text = "Kết nối cơ sở dữ liệu SQL Sever";
            ((System.ComponentModel.ISupportInitialize)comboBoxDB.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)severIn.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)userIn.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)passIn.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)checkedComboBoxTB.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtMK.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtMSVMGV.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

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
        private DevExpress.XtraEditors.SimpleButton btnRunQuery;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtQuery;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.CheckedComboBoxEdit checkedComboBoxTB;
        private DevExpress.XtraEditors.SimpleButton btnQuickCn;
        private DevExpress.XtraEditors.TextEdit txtMK;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.TextEdit txtMSVMGV;
        private DevExpress.XtraEditors.SimpleButton btnGetIP;
    }
}