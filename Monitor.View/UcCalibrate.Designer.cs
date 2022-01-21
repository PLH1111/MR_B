namespace Monitor.View
{
    partial class UcCalibrate
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxOut2 = new System.Windows.Forms.TextBox();
            this.textBoxGain = new System.Windows.Forms.TextBox();
            this.textBoxIn2 = new System.Windows.Forms.TextBox();
            this.textBoxOut1 = new System.Windows.Forms.TextBox();
            this.textBoxIn1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxOffset = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "Gain:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "输入:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 15);
            this.label1.TabIndex = 12;
            this.label1.Text = "输入:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(156, 68);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "第二步";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(156, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "第一步";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBoxOut2
            // 
            this.textBoxOut2.Location = new System.Drawing.Point(252, 70);
            this.textBoxOut2.Name = "textBoxOut2";
            this.textBoxOut2.Size = new System.Drawing.Size(80, 21);
            this.textBoxOut2.TabIndex = 5;
            // 
            // textBoxGain
            // 
            this.textBoxGain.Location = new System.Drawing.Point(51, 98);
            this.textBoxGain.Name = "textBoxGain";
            this.textBoxGain.Size = new System.Drawing.Size(80, 21);
            this.textBoxGain.TabIndex = 6;
            // 
            // textBoxIn2
            // 
            this.textBoxIn2.Location = new System.Drawing.Point(51, 68);
            this.textBoxIn2.Name = "textBoxIn2";
            this.textBoxIn2.Size = new System.Drawing.Size(80, 21);
            this.textBoxIn2.TabIndex = 7;
            // 
            // textBoxOut1
            // 
            this.textBoxOut1.Location = new System.Drawing.Point(252, 41);
            this.textBoxOut1.Name = "textBoxOut1";
            this.textBoxOut1.Size = new System.Drawing.Size(80, 21);
            this.textBoxOut1.TabIndex = 8;
            // 
            // textBoxIn1
            // 
            this.textBoxIn1.Location = new System.Drawing.Point(51, 39);
            this.textBoxIn1.Name = "textBoxIn1";
            this.textBoxIn1.Size = new System.Drawing.Size(80, 21);
            this.textBoxIn1.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(48, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 15);
            this.label4.TabIndex = 15;
            this.label4.Text = "Name";
            // 
            // textBoxOffset
            // 
            this.textBoxOffset.Location = new System.Drawing.Point(252, 98);
            this.textBoxOffset.Name = "textBoxOffset";
            this.textBoxOffset.Size = new System.Drawing.Size(80, 21);
            this.textBoxOffset.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(162, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 15);
            this.label5.TabIndex = 17;
            this.label5.Text = "Offset:";
            // 
            // UcCalibrate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxOffset);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxOut2);
            this.Controls.Add(this.textBoxGain);
            this.Controls.Add(this.textBoxIn2);
            this.Controls.Add(this.textBoxOut1);
            this.Controls.Add(this.textBoxIn1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UcCalibrate";
            this.Size = new System.Drawing.Size(357, 132);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxOut2;
        private System.Windows.Forms.TextBox textBoxGain;
        private System.Windows.Forms.TextBox textBoxIn2;
        private System.Windows.Forms.TextBox textBoxOut1;
        private System.Windows.Forms.TextBox textBoxIn1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxOffset;
        private System.Windows.Forms.Label label5;
    }
}
