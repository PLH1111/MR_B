namespace Monitor.View
{
    partial class UcReadWriteBox
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.BtnAfeWrite = new System.Windows.Forms.Button();
            this.TbxRegId = new System.Windows.Forms.TextBox();
            this.BtnAfeRead = new System.Windows.Forms.Button();
            this.TbxRegValue = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // BtnAfeWrite
            // 
            this.BtnAfeWrite.Location = new System.Drawing.Point(322, 9);
            this.BtnAfeWrite.Name = "BtnAfeWrite";
            this.BtnAfeWrite.Size = new System.Drawing.Size(75, 23);
            this.BtnAfeWrite.TabIndex = 5;
            this.BtnAfeWrite.Text = "Write";
            this.BtnAfeWrite.UseVisualStyleBackColor = true;
            this.BtnAfeWrite.Click += new System.EventHandler(this.BtnAfeWrite_Click);
            // 
            // TbxRegId
            // 
            this.TbxRegId.Location = new System.Drawing.Point(7, 8);
            this.TbxRegId.MaxLength = 2;
            this.TbxRegId.Name = "TbxRegId";
            this.TbxRegId.Size = new System.Drawing.Size(100, 28);
            this.TbxRegId.TabIndex = 3;
            // 
            // BtnAfeRead
            // 
            this.BtnAfeRead.Location = new System.Drawing.Point(241, 10);
            this.BtnAfeRead.Name = "BtnAfeRead";
            this.BtnAfeRead.Size = new System.Drawing.Size(75, 23);
            this.BtnAfeRead.TabIndex = 6;
            this.BtnAfeRead.Text = "Read";
            this.BtnAfeRead.UseVisualStyleBackColor = true;
            this.BtnAfeRead.Click += new System.EventHandler(this.BtnAfeRead_Click);
            // 
            // TbxRegValue
            // 
            this.TbxRegValue.Location = new System.Drawing.Point(122, 8);
            this.TbxRegValue.MaxLength = 4;
            this.TbxRegValue.Name = "TbxRegValue";
            this.TbxRegValue.Size = new System.Drawing.Size(100, 28);
            this.TbxRegValue.TabIndex = 4;
            // 
            // UcReadWriteBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.BtnAfeWrite);
            this.Controls.Add(this.TbxRegId);
            this.Controls.Add(this.BtnAfeRead);
            this.Controls.Add(this.TbxRegValue);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UcReadWriteBox";
            this.Size = new System.Drawing.Size(405, 37);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnAfeWrite;
        private System.Windows.Forms.TextBox TbxRegId;
        private System.Windows.Forms.Button BtnAfeRead;
        private System.Windows.Forms.TextBox TbxRegValue;
    }
}
