namespace MDORM.Tools
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtMingWen = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnJiami = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtMiWeng = new System.Windows.Forms.TextBox();
            this.btnJiemi = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtMingWen
            // 
            this.txtMingWen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMingWen.Location = new System.Drawing.Point(3, 17);
            this.txtMingWen.Multiline = true;
            this.txtMingWen.Name = "txtMingWen";
            this.txtMingWen.Size = new System.Drawing.Size(556, 72);
            this.txtMingWen.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtMingWen);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(562, 92);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "加密前的内容（明文）";
            // 
            // btnJiami
            // 
            this.btnJiami.Location = new System.Drawing.Point(15, 110);
            this.btnJiami.Name = "btnJiami";
            this.btnJiami.Size = new System.Drawing.Size(164, 38);
            this.btnJiami.TabIndex = 3;
            this.btnJiami.Text = "加密并复制到系统剪切板";
            this.btnJiami.UseVisualStyleBackColor = true;
            this.btnJiami.Click += new System.EventHandler(this.btnJiami_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtMiWeng);
            this.groupBox2.Location = new System.Drawing.Point(15, 156);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(559, 110);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "加密后的内容（密文）";
            // 
            // txtMiWeng
            // 
            this.txtMiWeng.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMiWeng.Location = new System.Drawing.Point(3, 17);
            this.txtMiWeng.Multiline = true;
            this.txtMiWeng.Name = "txtMiWeng";
            this.txtMiWeng.Size = new System.Drawing.Size(553, 90);
            this.txtMiWeng.TabIndex = 1;
            // 
            // btnJiemi
            // 
            this.btnJiemi.Location = new System.Drawing.Point(425, 112);
            this.btnJiemi.Name = "btnJiemi";
            this.btnJiemi.Size = new System.Drawing.Size(149, 38);
            this.btnJiemi.TabIndex = 5;
            this.btnJiemi.Text = "密文解密";
            this.btnJiemi.UseVisualStyleBackColor = true;
            this.btnJiemi.Click += new System.EventHandler(this.btnJiemi_Click);
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnJiami;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 283);
            this.Controls.Add(this.btnJiemi);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnJiami);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "字符串加密";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtMingWen;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnJiami;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtMiWeng;
        private System.Windows.Forms.Button btnJiemi;
    }
}

