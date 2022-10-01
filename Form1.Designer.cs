namespace LanguageSwitcher
{
    partial class FormMain
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
            this.btnEn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRu = new System.Windows.Forms.Button();
            this.btnUa = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnEn
            // 
            this.btnEn.Location = new System.Drawing.Point(12, 12);
            this.btnEn.Name = "btnEn";
            this.btnEn.Size = new System.Drawing.Size(138, 23);
            this.btnEn.TabIndex = 0;
            this.btnEn.Text = "EN";
            this.btnEn.UseVisualStyleBackColor = true;
            this.btnEn.Click += new System.EventHandler(this.btnEnClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "label1";
            // 
            // btnRu
            // 
            this.btnRu.Location = new System.Drawing.Point(12, 41);
            this.btnRu.Name = "btnRu";
            this.btnRu.Size = new System.Drawing.Size(138, 23);
            this.btnRu.TabIndex = 2;
            this.btnRu.Text = "RU";
            this.btnRu.UseVisualStyleBackColor = true;
            this.btnRu.Click += new System.EventHandler(this.btnRuClick);
            // 
            // btnUa
            // 
            this.btnUa.Location = new System.Drawing.Point(12, 70);
            this.btnUa.Name = "btnUa";
            this.btnUa.Size = new System.Drawing.Size(138, 23);
            this.btnUa.TabIndex = 3;
            this.btnUa.Text = "UA";
            this.btnUa.UseVisualStyleBackColor = true;
            this.btnUa.Click += new System.EventHandler(this.btnUaClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 138);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "label2";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(162, 160);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnUa);
            this.Controls.Add(this.btnRu);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnEn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.formLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRu;
        private System.Windows.Forms.Button btnUa;
        public System.Windows.Forms.Label label2;
    }
}

