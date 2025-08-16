namespace NumPlusFocusNavigator_Winform
{
    partial class Mainform
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
            myTextBox1 = new MyTextBox();
            myTextBox2 = new MyTextBox();
            myTextBox3 = new MyTextBox();
            myTextBox4 = new MyTextBox();
            myTextBox5 = new MyTextBox();
            myTextBox6 = new MyTextBox();
            SuspendLayout();
            // 
            // myTextBox1
            // 
            myTextBox1.Location = new Point(41, 63);
            myTextBox1.Name = "myTextBox1";
            myTextBox1.Size = new Size(200, 45);
            myTextBox1.TabIndex = 6;
            // 
            // myTextBox2
            // 
            myTextBox2.Location = new Point(41, 129);
            myTextBox2.Name = "myTextBox2";
            myTextBox2.Size = new Size(200, 45);
            myTextBox2.TabIndex = 7;
            // 
            // myTextBox3
            // 
            myTextBox3.Location = new Point(41, 240);
            myTextBox3.Name = "myTextBox3";
            myTextBox3.Size = new Size(200, 45);
            myTextBox3.TabIndex = 8;
            // 
            // myTextBox4
            // 
            myTextBox4.Location = new Point(309, 63);
            myTextBox4.Name = "myTextBox4";
            myTextBox4.Size = new Size(200, 45);
            myTextBox4.TabIndex = 9;
            // 
            // myTextBox5
            // 
            myTextBox5.Location = new Point(309, 148);
            myTextBox5.Name = "myTextBox5";
            myTextBox5.Size = new Size(200, 45);
            myTextBox5.TabIndex = 10;
            // 
            // myTextBox6
            // 
            myTextBox6.Location = new Point(309, 240);
            myTextBox6.Name = "myTextBox6";
            myTextBox6.Size = new Size(200, 45);
            myTextBox6.TabIndex = 11;
            // 
            // Mainform
            // 
            AutoScaleDimensions = new SizeF(15F, 38F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(myTextBox6);
            Controls.Add(myTextBox5);
            Controls.Add(myTextBox4);
            Controls.Add(myTextBox3);
            Controls.Add(myTextBox2);
            Controls.Add(myTextBox1);
            Name = "Mainform";
            Text = "form";
            Load += Mainform_Load;
            
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MyTextBox myTextBox1;
        private MyTextBox myTextBox2;
        private MyTextBox myTextBox3;
        private MyTextBox myTextBox4;
        private MyTextBox myTextBox5;
        private MyTextBox myTextBox6;
    }
}