namespace SimpleTranslator
{
    partial class SimpleTranslator
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
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.ResultTxtBox = new System.Windows.Forms.TextBox();
            this.SrcTxtBox = new System.Windows.Forms.TextBox();
            this.LoadNetworkButton = new System.Windows.Forms.Button();
            this.StopTrainingButton = new System.Windows.Forms.Button();
            this.PredictButton = new System.Windows.Forms.Button();
            this.TrainButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CreateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(194, 163);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 13);
            this.label12.TabIndex = 35;
            this.label12.Text = "Result";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(194, 69);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "Text to translate :";
            // 
            // ResultTxtBox
            // 
            this.ResultTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultTxtBox.Enabled = false;
            this.ResultTxtBox.Location = new System.Drawing.Point(197, 179);
            this.ResultTxtBox.Multiline = true;
            this.ResultTxtBox.Name = "ResultTxtBox";
            this.ResultTxtBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ResultTxtBox.Size = new System.Drawing.Size(423, 56);
            this.ResultTxtBox.TabIndex = 33;
            this.ResultTxtBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // SrcTxtBox
            // 
            this.SrcTxtBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SrcTxtBox.Enabled = false;
            this.SrcTxtBox.Location = new System.Drawing.Point(197, 85);
            this.SrcTxtBox.Multiline = true;
            this.SrcTxtBox.Name = "SrcTxtBox";
            this.SrcTxtBox.Size = new System.Drawing.Size(423, 59);
            this.SrcTxtBox.TabIndex = 32;
            this.SrcTxtBox.Text = "Recognizing the role of the General Assembly in addressing issues of peace and se" +
    "curity , in accordance with the Charter";
            // 
            // LoadNetworkButton
            // 
            this.LoadNetworkButton.Location = new System.Drawing.Point(31, 52);
            this.LoadNetworkButton.Name = "LoadNetworkButton";
            this.LoadNetworkButton.Size = new System.Drawing.Size(95, 23);
            this.LoadNetworkButton.TabIndex = 40;
            this.LoadNetworkButton.Text = "Load Network";
            this.LoadNetworkButton.UseVisualStyleBackColor = true;
            this.LoadNetworkButton.Click += new System.EventHandler(this.button5_Click);
            // 
            // StopTrainingButton
            // 
            this.StopTrainingButton.Enabled = false;
            this.StopTrainingButton.Location = new System.Drawing.Point(31, 110);
            this.StopTrainingButton.Name = "StopTrainingButton";
            this.StopTrainingButton.Size = new System.Drawing.Size(95, 23);
            this.StopTrainingButton.TabIndex = 38;
            this.StopTrainingButton.Text = "Stop";
            this.StopTrainingButton.UseVisualStyleBackColor = true;
            this.StopTrainingButton.Click += new System.EventHandler(this.Stop_Click);
            // 
            // PredictButton
            // 
            this.PredictButton.Enabled = false;
            this.PredictButton.Location = new System.Drawing.Point(307, 59);
            this.PredictButton.Name = "PredictButton";
            this.PredictButton.Size = new System.Drawing.Size(75, 23);
            this.PredictButton.TabIndex = 37;
            this.PredictButton.Text = "Predict";
            this.PredictButton.UseVisualStyleBackColor = true;
            this.PredictButton.Click += new System.EventHandler(this.Predict_Click);
            // 
            // TrainButton
            // 
            this.TrainButton.Enabled = false;
            this.TrainButton.Location = new System.Drawing.Point(31, 81);
            this.TrainButton.Name = "TrainButton";
            this.TrainButton.Size = new System.Drawing.Size(95, 23);
            this.TrainButton.TabIndex = 36;
            this.TrainButton.Text = "Train";
            this.TrainButton.UseVisualStyleBackColor = true;
            this.TrainButton.Click += new System.EventHandler(this.Train_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(28, 216);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(16, 13);
            this.label10.TabIndex = 50;
            this.label10.Tag = "";
            this.label10.Text = "It.";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(28, 194);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 48;
            this.label8.Tag = "";
            this.label8.Text = "Cost";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(28, 170);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 47;
            this.label7.Tag = "";
            this.label7.Text = "End";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(28, 141);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 46;
            this.label6.Tag = "";
            this.label6.Text = "Start";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(63, 216);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 45;
            this.label5.Text = "label5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 194);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 43;
            this.label3.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 42;
            this.label2.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(63, 141);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 41;
            this.label1.Text = "label1";
            // 
            // CreateButton
            // 
            this.CreateButton.Location = new System.Drawing.Point(31, 23);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(95, 23);
            this.CreateButton.TabIndex = 51;
            this.CreateButton.Text = "Create Network";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // SimpleTranslator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 256);
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LoadNetworkButton);
            this.Controls.Add(this.StopTrainingButton);
            this.Controls.Add(this.PredictButton);
            this.Controls.Add(this.TrainButton);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.ResultTxtBox);
            this.Controls.Add(this.SrcTxtBox);
            this.Name = "SimpleTranslator";
            this.Text = "SimpleTranslator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox ResultTxtBox;
        private System.Windows.Forms.TextBox SrcTxtBox;
        private System.Windows.Forms.Button LoadNetworkButton;
        private System.Windows.Forms.Button StopTrainingButton;
        private System.Windows.Forms.Button PredictButton;
        private System.Windows.Forms.Button TrainButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button CreateButton;
    }
}

