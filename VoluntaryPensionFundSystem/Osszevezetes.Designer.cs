namespace VoluntaryPensionFundSystem
{
    partial class Osszevezetes
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
            this.bIndit = new System.Windows.Forms.Button();
            this.rtbMessage = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.bMegszakit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bIndit
            // 
            this.bIndit.AutoSize = true;
            this.bIndit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bIndit.Location = new System.Drawing.Point(30, 115);
            this.bIndit.Name = "bIndit";
            this.bIndit.Size = new System.Drawing.Size(162, 29);
            this.bIndit.TabIndex = 3;
            this.bIndit.Text = "Összevezetés indítása";
            this.bIndit.UseVisualStyleBackColor = true;
            this.bIndit.Click += new System.EventHandler(this.bIndit_Click);
            // 
            // rtbMessage
            // 
            this.rtbMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rtbMessage.Location = new System.Drawing.Point(30, 160);
            this.rtbMessage.Name = "rtbMessage";
            this.rtbMessage.ReadOnly = true;
            this.rtbMessage.Size = new System.Drawing.Size(947, 325);
            this.rtbMessage.TabIndex = 5;
            this.rtbMessage.Text = "";
            this.rtbMessage.WordWrap = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(423, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 29);
            this.label2.TabIndex = 176;
            this.label2.Text = "Összevezetés";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(405, 121);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 17);
            this.label1.TabIndex = 177;
            this.label1.Text = "label1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(30, 501);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(947, 23);
            this.progressBar1.Step = 2;
            this.progressBar1.TabIndex = 178;
            // 
            // bMegszakit
            // 
            this.bMegszakit.AutoSize = true;
            this.bMegszakit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bMegszakit.Location = new System.Drawing.Point(198, 115);
            this.bMegszakit.Name = "bMegszakit";
            this.bMegszakit.Size = new System.Drawing.Size(201, 29);
            this.bMegszakit.TabIndex = 179;
            this.bMegszakit.Text = "Összevezetés megszakítása";
            this.bMegszakit.UseVisualStyleBackColor = false;
            this.bMegszakit.Click += new System.EventHandler(this.bMegszakit_Click);
            // 
            // Osszevezetes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(1008, 545);
            this.Controls.Add(this.bMegszakit);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rtbMessage);
            this.Controls.Add(this.bIndit);
            this.Name = "Osszevezetes";
            this.Text = "Összevezetés";
            this.Controls.SetChildIndex(this.bIndit, 0);
            this.Controls.SetChildIndex(this.rtbMessage, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.progressBar1, 0);
            this.Controls.SetChildIndex(this.bMegszakit, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bIndit;
        private System.Windows.Forms.RichTextBox rtbMessage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button bMegszakit;
    }
}
