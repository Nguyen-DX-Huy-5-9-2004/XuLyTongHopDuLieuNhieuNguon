namespace connectSqlSever
{
    partial class InputBox
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
            lblPrompt = new System.Windows.Forms.Label();
            btnOK = new System.Windows.Forms.Button();
            button2 = new System.Windows.Forms.Button();
            txtInput = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // lblPrompt
            // 
            lblPrompt.AutoSize = true;
            lblPrompt.Location = new System.Drawing.Point(53, 30);
            lblPrompt.Name = "lblPrompt";
            lblPrompt.Size = new System.Drawing.Size(50, 20);
            lblPrompt.TabIndex = 0;
            lblPrompt.Text = "label1";
            // 
            // btnOK
            // 
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(63, 103);
            btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(75, 29);
            btnOK.TabIndex = 1;
            btnOK.Text = "Oke";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // button2
            // 
            button2.Location = new System.Drawing.Point(193, 103);
            button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(75, 29);
            button2.TabIndex = 2;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            // 
            // txtInput
            // 
            txtInput.Location = new System.Drawing.Point(63, 54);
            txtInput.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            txtInput.Name = "txtInput";
            txtInput.Size = new System.Drawing.Size(205, 27);
            txtInput.TabIndex = 3;
            // 
            // InputBox
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(320, 162);
            Controls.Add(txtInput);
            Controls.Add(button2);
            Controls.Add(btnOK);
            Controls.Add(lblPrompt);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "InputBox";
            Text = "Nhập tên mới";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtInput;
    }
}