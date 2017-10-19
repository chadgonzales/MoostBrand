namespace DTR
{
    partial class frmLogList
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
            this.btnEnrollEmployee = new System.Windows.Forms.Button();
            this.lvwLogs = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnEnrollEmployee
            // 
            this.btnEnrollEmployee.Location = new System.Drawing.Point(416, 38);
            this.btnEnrollEmployee.Name = "btnEnrollEmployee";
            this.btnEnrollEmployee.Size = new System.Drawing.Size(107, 23);
            this.btnEnrollEmployee.TabIndex = 2;
            this.btnEnrollEmployee.Text = "Enroll employee";
            this.btnEnrollEmployee.UseVisualStyleBackColor = true;
            this.btnEnrollEmployee.Click += new System.EventHandler(this.button1_Click);
            // 
            // lvwLogs
            // 
            this.lvwLogs.FullRowSelect = true;
            this.lvwLogs.GridLines = true;
            this.lvwLogs.Location = new System.Drawing.Point(25, 67);
            this.lvwLogs.Name = "lvwLogs";
            this.lvwLogs.Size = new System.Drawing.Size(498, 422);
            this.lvwLogs.TabIndex = 39;
            this.lvwLogs.UseCompatibleStateImageBehavior = false;
            this.lvwLogs.View = System.Windows.Forms.View.Details;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(154, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 40;
            this.label1.Text = "Search:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(205, 39);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(127, 20);
            this.textBox1.TabIndex = 41;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(162, 497);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(44, 23);
            this.button2.TabIndex = 42;
            this.button2.Text = "<<";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(212, 496);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(44, 23);
            this.button3.TabIndex = 43;
            this.button3.Text = "<";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(268, 496);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(44, 23);
            this.button4.TabIndex = 44;
            this.button4.Text = ">";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(318, 496);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(44, 23);
            this.button5.TabIndex = 45;
            this.button5.Text = ">>";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 46;
            this.label2.Text = "Show:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(69, 38);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(57, 21);
            this.comboBox1.TabIndex = 47;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(338, 39);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(51, 23);
            this.button6.TabIndex = 48;
            this.button6.Text = "Submit";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 501);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 13);
            this.label3.TabIndex = 49;
            this.label3.Text = "Showing page {0} of {1}";
            // 
            // frmLogList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 550);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvwLogs);
            this.Controls.Add(this.btnEnrollEmployee);
            this.Name = "frmLogList";
            this.Text = "frmLogList";
            this.Load += new System.EventHandler(this.frmLogList_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEnrollEmployee;
        private System.Windows.Forms.ListView lvwLogs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Label label3;
    }
}