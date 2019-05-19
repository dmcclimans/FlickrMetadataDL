namespace FlickrMetadataDL
{
    partial class FormAddLoginAccount
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
            this.Step2GroupBox = new System.Windows.Forms.GroupBox();
            this.labelStep2 = new System.Windows.Forms.Label();
            this.VerifierTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.labelResult = new System.Windows.Forms.Label();
            this.CompleteAuthButton = new System.Windows.Forms.Button();
            this.Step1GroupBox = new System.Windows.Forms.GroupBox();
            this.labelStep1 = new System.Windows.Forms.Label();
            this.AuthenticateButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.Step2GroupBox.SuspendLayout();
            this.Step1GroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Step2GroupBox
            // 
            this.Step2GroupBox.Controls.Add(this.labelStep2);
            this.Step2GroupBox.Controls.Add(this.VerifierTextBox);
            this.Step2GroupBox.Controls.Add(this.label3);
            this.Step2GroupBox.Controls.Add(this.labelResult);
            this.Step2GroupBox.Controls.Add(this.CompleteAuthButton);
            this.Step2GroupBox.Enabled = false;
            this.Step2GroupBox.Location = new System.Drawing.Point(12, 177);
            this.Step2GroupBox.Name = "Step2GroupBox";
            this.Step2GroupBox.Size = new System.Drawing.Size(266, 157);
            this.Step2GroupBox.TabIndex = 2;
            this.Step2GroupBox.TabStop = false;
            this.Step2GroupBox.Text = "Step 2";
            // 
            // labelStep2
            // 
            this.labelStep2.Location = new System.Drawing.Point(16, 23);
            this.labelStep2.Name = "labelStep2";
            this.labelStep2.Size = new System.Drawing.Size(228, 35);
            this.labelStep2.TabIndex = 0;
            this.labelStep2.Text = "Once you have authenticated copy the Verifier code into the text box and click Co" +
    "mplete.";
            // 
            // VerifierTextBox
            // 
            this.VerifierTextBox.Location = new System.Drawing.Point(88, 61);
            this.VerifierTextBox.Name = "VerifierTextBox";
            this.VerifierTextBox.Size = new System.Drawing.Size(100, 20);
            this.VerifierTextBox.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Verifier code:";
            // 
            // labelResult
            // 
            this.labelResult.Location = new System.Drawing.Point(19, 121);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(238, 24);
            this.labelResult.TabIndex = 4;
            this.labelResult.Text = "Successfully authenticated";
            // 
            // CompleteAuthButton
            // 
            this.CompleteAuthButton.Location = new System.Drawing.Point(86, 87);
            this.CompleteAuthButton.Name = "CompleteAuthButton";
            this.CompleteAuthButton.Size = new System.Drawing.Size(75, 23);
            this.CompleteAuthButton.TabIndex = 3;
            this.CompleteAuthButton.Text = "Complete";
            this.CompleteAuthButton.UseVisualStyleBackColor = true;
            this.CompleteAuthButton.Click += new System.EventHandler(this.CompleteAuthButton_Click);
            // 
            // Step1GroupBox
            // 
            this.Step1GroupBox.Controls.Add(this.labelStep1);
            this.Step1GroupBox.Controls.Add(this.AuthenticateButton);
            this.Step1GroupBox.Location = new System.Drawing.Point(12, 36);
            this.Step1GroupBox.Name = "Step1GroupBox";
            this.Step1GroupBox.Size = new System.Drawing.Size(266, 120);
            this.Step1GroupBox.TabIndex = 1;
            this.Step1GroupBox.TabStop = false;
            this.Step1GroupBox.Text = "Step 1";
            // 
            // labelStep1
            // 
            this.labelStep1.Location = new System.Drawing.Point(13, 20);
            this.labelStep1.Name = "labelStep1";
            this.labelStep1.Size = new System.Drawing.Size(244, 58);
            this.labelStep1.TabIndex = 0;
            this.labelStep1.Text = "Click the Authenticate button to start authentication. You will be taken to the F" +
    "lickr web site. Log in if necessary and allow FlickrMetadataDL to access your Fl" +
    "ickr account.";
            // 
            // AuthenticateButton
            // 
            this.AuthenticateButton.Location = new System.Drawing.Point(86, 84);
            this.AuthenticateButton.Name = "AuthenticateButton";
            this.AuthenticateButton.Size = new System.Drawing.Size(75, 23);
            this.AuthenticateButton.TabIndex = 1;
            this.AuthenticateButton.Text = "Authenticate";
            this.AuthenticateButton.UseVisualStyleBackColor = true;
            this.AuthenticateButton.Click += new System.EventHandler(this.AuthenticateButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "To add a new login account:";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(98, 350);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // FormAddLoginAccount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(292, 393);
            this.Controls.Add(this.Step2GroupBox);
            this.Controls.Add(this.Step1GroupBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAddLoginAccount";
            this.Text = "Add Login Account";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormAddLoginAccount_FormClosing);
            this.Load += new System.EventHandler(this.FormAddLoginAccount_Load);
            this.Step2GroupBox.ResumeLayout(false);
            this.Step2GroupBox.PerformLayout();
            this.Step1GroupBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox Step2GroupBox;
        private System.Windows.Forms.Label labelStep2;
        private System.Windows.Forms.TextBox VerifierTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Button CompleteAuthButton;
        private System.Windows.Forms.GroupBox Step1GroupBox;
        private System.Windows.Forms.Label labelStep1;
        private System.Windows.Forms.Button AuthenticateButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
    }
}