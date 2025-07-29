namespace Ultimate_Control
{
    partial class FormFeedback
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormFeedback));
            this.labelN = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonFBCancel = new System.Windows.Forms.Button();
            this.linkWebsite = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.linkGit = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // labelN
            // 
            this.labelN.AutoSize = true;
            this.labelN.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(51)))), ((int)(((byte)(153)))));
            this.labelN.Location = new System.Drawing.Point(12, 15);
            this.labelN.Name = "labelN";
            this.labelN.Size = new System.Drawing.Size(328, 21);
            this.labelN.TabIndex = 26;
            this.labelN.Text = "Tell Jack Pomi Software about your experience";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 45);
            this.label1.MaximumSize = new System.Drawing.Size(432, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(427, 26);
            this.label1.TabIndex = 27;
            this.label1.Text = "Thank you for downloading Ultimate Control. We would be really glad if you leave " +
    "a short review or report a bug if you found one. Here is where you can do it:";
            // 
            // buttonFBCancel
            // 
            this.buttonFBCancel.Location = new System.Drawing.Point(376, 155);
            this.buttonFBCancel.Name = "buttonFBCancel";
            this.buttonFBCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonFBCancel.TabIndex = 28;
            this.buttonFBCancel.Text = "Cancel";
            this.buttonFBCancel.UseVisualStyleBackColor = true;
            this.buttonFBCancel.Click += new System.EventHandler(this.buttonFBCancel_Click);
            // 
            // linkWebsite
            // 
            this.linkWebsite.AutoSize = true;
            this.linkWebsite.LinkColor = System.Drawing.SystemColors.Highlight;
            this.linkWebsite.Location = new System.Drawing.Point(13, 85);
            this.linkWebsite.Name = "linkWebsite";
            this.linkWebsite.Size = new System.Drawing.Size(120, 13);
            this.linkWebsite.TabIndex = 29;
            this.linkWebsite.TabStop = true;
            this.linkWebsite.Text = "Ultimate Control website";
            this.linkWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkWebsite_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(128, 85);
            this.label2.MaximumSize = new System.Drawing.Size(325, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(321, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "— you can use e-mail to leave your review. Also, we would be really";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 98);
            this.label3.MaximumSize = new System.Drawing.Size(320, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(145, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "grateful if you leave us a tip :)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 130);
            this.label4.MaximumSize = new System.Drawing.Size(320, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(193, 13);
            this.label4.TabIndex = 34;
            this.label4.Text = "please leave an issue on that repository";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(94, 117);
            this.label5.MaximumSize = new System.Drawing.Size(345, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(344, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "— here you can check the source code and find bugs. If you found one,";
            // 
            // linkGit
            // 
            this.linkGit.AutoSize = true;
            this.linkGit.LinkColor = System.Drawing.SystemColors.Highlight;
            this.linkGit.Location = new System.Drawing.Point(13, 117);
            this.linkGit.Name = "linkGit";
            this.linkGit.Size = new System.Drawing.Size(86, 13);
            this.linkGit.TabIndex = 32;
            this.linkGit.TabStop = true;
            this.linkGit.Text = "Github repository";
            this.linkGit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkGit_LinkClicked);
            // 
            // FormFeedback
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(463, 187);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.linkGit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linkWebsite);
            this.Controls.Add(this.buttonFBCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormFeedback";
            this.Text = "How to send feedback";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelN;
        private System.Windows.Forms.Button buttonFBCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkWebsite;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel linkGit;
    }
}