namespace Monitor.View
{
    partial class FormRegisterRw
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonReadRdb = new System.Windows.Forms.Button();
            this.buttonWriteRdb = new System.Windows.Forms.Button();
            this.textBoxRdb = new System.Windows.Forms.TextBox();
            this.comboBoxRdb = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BtnRemove = new System.Windows.Forms.Button();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonReadRdb);
            this.groupBox1.Controls.Add(this.buttonWriteRdb);
            this.groupBox1.Controls.Add(this.textBoxRdb);
            this.groupBox1.Controls.Add(this.comboBoxRdb);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(422, 79);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RDB Write";
            // 
            // buttonReadRdb
            // 
            this.buttonReadRdb.Location = new System.Drawing.Point(250, 37);
            this.buttonReadRdb.Name = "buttonReadRdb";
            this.buttonReadRdb.Size = new System.Drawing.Size(75, 23);
            this.buttonReadRdb.TabIndex = 7;
            this.buttonReadRdb.Text = "Read";
            this.buttonReadRdb.UseVisualStyleBackColor = true;
            this.buttonReadRdb.Click += new System.EventHandler(this.buttonReadRdb_Click);
            // 
            // buttonWriteRdb
            // 
            this.buttonWriteRdb.Location = new System.Drawing.Point(331, 37);
            this.buttonWriteRdb.Name = "buttonWriteRdb";
            this.buttonWriteRdb.Size = new System.Drawing.Size(75, 23);
            this.buttonWriteRdb.TabIndex = 6;
            this.buttonWriteRdb.Text = "Write";
            this.buttonWriteRdb.UseVisualStyleBackColor = true;
            this.buttonWriteRdb.Click += new System.EventHandler(this.buttonWriteRdb_Click);
            // 
            // textBoxRdb
            // 
            this.textBoxRdb.Location = new System.Drawing.Point(136, 37);
            this.textBoxRdb.Name = "textBoxRdb";
            this.textBoxRdb.Size = new System.Drawing.Size(95, 32);
            this.textBoxRdb.TabIndex = 5;
            // 
            // comboBoxRdb
            // 
            this.comboBoxRdb.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxRdb.DropDownWidth = 150;
            this.comboBoxRdb.FormattingEnabled = true;
            this.comboBoxRdb.IntegralHeight = false;
            this.comboBoxRdb.Location = new System.Drawing.Point(16, 35);
            this.comboBoxRdb.Name = "comboBoxRdb";
            this.comboBoxRdb.Size = new System.Drawing.Size(100, 32);
            this.comboBoxRdb.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Location = new System.Drawing.Point(13, 97);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(421, 391);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "AfeAccess";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 71);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(415, 317);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BtnRemove);
            this.panel2.Controls.Add(this.BtnAdd);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(415, 43);
            this.panel2.TabIndex = 3;
            // 
            // BtnRemove
            // 
            this.BtnRemove.Location = new System.Drawing.Point(324, 9);
            this.BtnRemove.Name = "BtnRemove";
            this.BtnRemove.Size = new System.Drawing.Size(75, 23);
            this.BtnRemove.TabIndex = 2;
            this.BtnRemove.Text = "Remove";
            this.BtnRemove.UseVisualStyleBackColor = true;
            this.BtnRemove.Click += new System.EventHandler(this.BtnRemove_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Location = new System.Drawing.Point(243, 9);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(75, 23);
            this.BtnAdd.TabIndex = 0;
            this.BtnAdd.Text = "Add";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 24);
            this.label1.TabIndex = 1;
            this.label1.Text = "RegId";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(119, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "RegValue";
            // 
            // FormRegisterRw
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(446, 500);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormRegisterRw";
            this.ShowIcon = false;
            this.Text = "RegisterRw";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonReadRdb;
        private System.Windows.Forms.Button buttonWriteRdb;
        private System.Windows.Forms.TextBox textBoxRdb;
        private System.Windows.Forms.ComboBox comboBoxRdb;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BtnRemove;
        private System.Windows.Forms.Button BtnAdd;
    }
}