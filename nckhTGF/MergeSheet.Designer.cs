namespace nckhTGF
{
    partial class MergeSheet
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
            checkedListBoxControl1 = new DevExpress.XtraEditors.CheckedListBoxControl();
            listBoxControl1 = new DevExpress.XtraEditors.ListBoxControl();
            simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
            simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)checkedListBoxControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)listBoxControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)comboBoxEdit1.Properties).BeginInit();
            SuspendLayout();
            // 
            // checkedListBoxControl1
            // 
            checkedListBoxControl1.Location = new System.Drawing.Point(57, 33);
            checkedListBoxControl1.Name = "checkedListBoxControl1";
            checkedListBoxControl1.Size = new System.Drawing.Size(206, 297);
            checkedListBoxControl1.TabIndex = 0;
            // 
            // listBoxControl1
            // 
            listBoxControl1.Location = new System.Drawing.Point(281, 33);
            listBoxControl1.Name = "listBoxControl1";
            listBoxControl1.Size = new System.Drawing.Size(216, 242);
            listBoxControl1.TabIndex = 1;
            // 
            // simpleButton1
            // 
            simpleButton1.Location = new System.Drawing.Point(512, 100);
            simpleButton1.Name = "simpleButton1";
            simpleButton1.Size = new System.Drawing.Size(78, 36);
            simpleButton1.TabIndex = 2;
            simpleButton1.Text = "Lên";
            // 
            // comboBoxEdit1
            // 
            comboBoxEdit1.Location = new System.Drawing.Point(281, 308);
            comboBoxEdit1.Name = "comboBoxEdit1";
            comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] { new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo) });
            comboBoxEdit1.Size = new System.Drawing.Size(216, 22);
            comboBoxEdit1.TabIndex = 3;
            // 
            // simpleButton2
            // 
            simpleButton2.Location = new System.Drawing.Point(512, 142);
            simpleButton2.Name = "simpleButton2";
            simpleButton2.Size = new System.Drawing.Size(78, 36);
            simpleButton2.TabIndex = 4;
            simpleButton2.Text = "Xuống";
            // 
            // simpleButton3
            // 
            simpleButton3.Location = new System.Drawing.Point(512, 294);
            simpleButton3.Name = "simpleButton3";
            simpleButton3.Size = new System.Drawing.Size(86, 36);
            simpleButton3.TabIndex = 5;
            simpleButton3.Text = "&GỘP";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(57, 14);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(130, 16);
            label1.TabIndex = 6;
            label1.Text = "&Chọn các shet để gộp";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(281, 14);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(89, 16);
            label2.TabIndex = 7;
            label2.Text = "Sheet đã chọn";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(281, 289);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(123, 16);
            label3.TabIndex = 8;
            label3.Text = "Chọn khóa khả dụng";
            // 
            // MergeSheet
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(629, 367);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(simpleButton3);
            Controls.Add(simpleButton2);
            Controls.Add(comboBoxEdit1);
            Controls.Add(simpleButton1);
            Controls.Add(listBoxControl1);
            Controls.Add(checkedListBoxControl1);
            Name = "MergeSheet";
            Text = "Gộp dữ liệu";
            ((System.ComponentModel.ISupportInitialize)checkedListBoxControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)listBoxControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)comboBoxEdit1.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraEditors.CheckedListBoxControl checkedListBoxControl1;
        private DevExpress.XtraEditors.ListBoxControl listBoxControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}