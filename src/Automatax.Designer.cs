namespace Automatax
{
    partial class Automatax
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.message1 = new System.Windows.Forms.Label();
            this.readButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.inputTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.message2 = new System.Windows.Forms.Label();
            this.resultsGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.isDfaTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.showCfgFileButton = new System.Windows.Forms.Button();
            this.showPdaFileButton = new System.Windows.Forms.Button();
            this.cfgTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.pdaTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.showInputFileButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsGridView)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.showInputFileButton);
            this.groupBox1.Controls.Add(this.clearButton);
            this.groupBox1.Controls.Add(this.message1);
            this.groupBox1.Controls.Add(this.readButton);
            this.groupBox1.Controls.Add(this.loadButton);
            this.groupBox1.Controls.Add(this.inputTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(565, 100);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input";
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(489, 45);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(70, 23);
            this.clearButton.TabIndex = 7;
            this.clearButton.Text = "Clear";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // message1
            // 
            this.message1.ForeColor = System.Drawing.Color.Red;
            this.message1.Location = new System.Drawing.Point(40, 71);
            this.message1.Name = "message1";
            this.message1.Size = new System.Drawing.Size(519, 26);
            this.message1.TabIndex = 6;
            // 
            // readButton
            // 
            this.readButton.Location = new System.Drawing.Point(199, 45);
            this.readButton.Name = "readButton";
            this.readButton.Size = new System.Drawing.Size(150, 23);
            this.readButton.TabIndex = 2;
            this.readButton.Text = "Read";
            this.readButton.UseVisualStyleBackColor = true;
            this.readButton.Click += new System.EventHandler(this.readButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(43, 45);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(150, 23);
            this.loadButton.TabIndex = 2;
            this.loadButton.Text = "Browse File";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // inputTextBox
            // 
            this.inputTextBox.Location = new System.Drawing.Point(43, 19);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.Size = new System.Drawing.Size(516, 20);
            this.inputTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Input";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.message2);
            this.groupBox2.Controls.Add(this.resultsGridView);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.isDfaTextBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(12, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(565, 303);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Test Vectors";
            // 
            // message2
            // 
            this.message2.AutoSize = true;
            this.message2.ForeColor = System.Drawing.Color.Red;
            this.message2.Location = new System.Drawing.Point(163, 22);
            this.message2.Name = "message2";
            this.message2.Size = new System.Drawing.Size(0, 13);
            this.message2.TabIndex = 5;
            // 
            // resultsGridView
            // 
            this.resultsGridView.AllowUserToResizeRows = false;
            this.resultsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.resultsGridView.Location = new System.Drawing.Point(9, 60);
            this.resultsGridView.Name = "resultsGridView";
            this.resultsGridView.ReadOnly = true;
            this.resultsGridView.RowHeadersVisible = false;
            this.resultsGridView.Size = new System.Drawing.Size(550, 237);
            this.resultsGridView.TabIndex = 4;
            this.resultsGridView.SelectionChanged += new System.EventHandler(this.resultsGridView_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Word";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Is accepted?";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Red;
            this.Column3.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column3.HeaderText = "";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Acceptance Tests";
            // 
            // isDfaTextBox
            // 
            this.isDfaTextBox.Location = new System.Drawing.Point(57, 19);
            this.isDfaTextBox.Name = "isDfaTextBox";
            this.isDfaTextBox.Size = new System.Drawing.Size(100, 20);
            this.isDfaTextBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Is DFA?";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.showCfgFileButton);
            this.groupBox3.Controls.Add(this.showPdaFileButton);
            this.groupBox3.Controls.Add(this.cfgTextBox);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.pdaTextBox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(12, 427);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(565, 83);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PDA <-> CFG";
            // 
            // showCfgFileButton
            // 
            this.showCfgFileButton.Location = new System.Drawing.Point(349, 44);
            this.showCfgFileButton.Name = "showCfgFileButton";
            this.showCfgFileButton.Size = new System.Drawing.Size(210, 23);
            this.showCfgFileButton.TabIndex = 5;
            this.showCfgFileButton.Text = "Show File";
            this.showCfgFileButton.UseVisualStyleBackColor = true;
            this.showCfgFileButton.Click += new System.EventHandler(this.showCfgFileButton_Click);
            // 
            // showPdaFileButton
            // 
            this.showPdaFileButton.Location = new System.Drawing.Point(349, 18);
            this.showPdaFileButton.Name = "showPdaFileButton";
            this.showPdaFileButton.Size = new System.Drawing.Size(210, 23);
            this.showPdaFileButton.TabIndex = 4;
            this.showPdaFileButton.Text = "Show File";
            this.showPdaFileButton.UseVisualStyleBackColor = true;
            this.showPdaFileButton.Click += new System.EventHandler(this.showPdaFileButton_Click);
            // 
            // cfgTextBox
            // 
            this.cfgTextBox.Location = new System.Drawing.Point(43, 46);
            this.cfgTextBox.Name = "cfgTextBox";
            this.cfgTextBox.ReadOnly = true;
            this.cfgTextBox.Size = new System.Drawing.Size(300, 20);
            this.cfgTextBox.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "CFG";
            // 
            // pdaTextBox
            // 
            this.pdaTextBox.Location = new System.Drawing.Point(43, 20);
            this.pdaTextBox.Name = "pdaTextBox";
            this.pdaTextBox.ReadOnly = true;
            this.pdaTextBox.Size = new System.Drawing.Size(300, 20);
            this.pdaTextBox.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "PDA";
            // 
            // showInputFileButton
            // 
            this.showInputFileButton.Location = new System.Drawing.Point(355, 45);
            this.showInputFileButton.Name = "showInputFileButton";
            this.showInputFileButton.Size = new System.Drawing.Size(128, 23);
            this.showInputFileButton.TabIndex = 6;
            this.showInputFileButton.Text = "Show File";
            this.showInputFileButton.UseVisualStyleBackColor = true;
            this.showInputFileButton.Click += new System.EventHandler(this.showInputFileButton_Click);
            // 
            // Automatax
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(589, 521);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Automatax";
            this.Text = "Automatax";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.resultsGridView)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button readButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.TextBox inputTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox isDfaTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView resultsGridView;
        private System.Windows.Forms.Label message2;
        private System.Windows.Forms.Label message1;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox pdaTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button showCfgFileButton;
        private System.Windows.Forms.Button showPdaFileButton;
        private System.Windows.Forms.TextBox cfgTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button showInputFileButton;
    }
}

