namespace BesysGUI
{
    partial class Form1
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.hueUpDown = new System.Windows.Forms.NumericUpDown();
            this.satUpDown = new System.Windows.Forms.NumericUpDown();
            this.valUpDown = new System.Windows.Forms.NumericUpDown();
            this.valMaxUpDown = new System.Windows.Forms.NumericUpDown();
            this.satMaxUpDown = new System.Windows.Forms.NumericUpDown();
            this.hueMaxUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hueUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.satUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMaxUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.satMaxUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hueMaxUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(13, 13);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(615, 430);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(677, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // hueUpDown
            // 
            this.hueUpDown.Location = new System.Drawing.Point(677, 66);
            this.hueUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.hueUpDown.Name = "hueUpDown";
            this.hueUpDown.Size = new System.Drawing.Size(120, 20);
            this.hueUpDown.TabIndex = 2;
            this.hueUpDown.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.hueUpDown.ValueChanged += new System.EventHandler(this.hueUpDown_ValueChanged);
            // 
            // satUpDown
            // 
            this.satUpDown.Location = new System.Drawing.Point(677, 92);
            this.satUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.satUpDown.Name = "satUpDown";
            this.satUpDown.Size = new System.Drawing.Size(120, 20);
            this.satUpDown.TabIndex = 3;
            this.satUpDown.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // valUpDown
            // 
            this.valUpDown.Location = new System.Drawing.Point(677, 118);
            this.valUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.valUpDown.Name = "valUpDown";
            this.valUpDown.Size = new System.Drawing.Size(120, 20);
            this.valUpDown.TabIndex = 4;
            // 
            // valMaxUpDown
            // 
            this.valMaxUpDown.Location = new System.Drawing.Point(677, 205);
            this.valMaxUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.valMaxUpDown.Name = "valMaxUpDown";
            this.valMaxUpDown.Size = new System.Drawing.Size(120, 20);
            this.valMaxUpDown.TabIndex = 7;
            this.valMaxUpDown.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // satMaxUpDown
            // 
            this.satMaxUpDown.Location = new System.Drawing.Point(677, 179);
            this.satMaxUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.satMaxUpDown.Name = "satMaxUpDown";
            this.satMaxUpDown.Size = new System.Drawing.Size(120, 20);
            this.satMaxUpDown.TabIndex = 6;
            this.satMaxUpDown.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            // 
            // hueMaxUpDown
            // 
            this.hueMaxUpDown.Location = new System.Drawing.Point(677, 153);
            this.hueMaxUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.hueMaxUpDown.Name = "hueMaxUpDown";
            this.hueMaxUpDown.Size = new System.Drawing.Size(120, 20);
            this.hueMaxUpDown.TabIndex = 5;
            this.hueMaxUpDown.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 455);
            this.Controls.Add(this.valMaxUpDown);
            this.Controls.Add(this.satMaxUpDown);
            this.Controls.Add(this.hueMaxUpDown);
            this.Controls.Add(this.valUpDown);
            this.Controls.Add(this.satUpDown);
            this.Controls.Add(this.hueUpDown);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hueUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.satUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMaxUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.satMaxUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hueMaxUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.NumericUpDown hueUpDown;
        private System.Windows.Forms.NumericUpDown satUpDown;
        private System.Windows.Forms.NumericUpDown valUpDown;
        private System.Windows.Forms.NumericUpDown valMaxUpDown;
        private System.Windows.Forms.NumericUpDown satMaxUpDown;
        private System.Windows.Forms.NumericUpDown hueMaxUpDown;
    }
}

