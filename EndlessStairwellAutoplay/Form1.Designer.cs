namespace EndlessStairwellAutoplay
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			button1 = new Button();
			button2 = new Button();
			button4 = new Button();
			timeLabel = new Label();
			listBox1 = new ListBox();
			manualTaskslistBox = new ListBox();
			textBox1 = new TextBox();
			button5 = new Button();
			infoTextBox = new TextBox();
			statusLabel = new Label();
			SuspendLayout();
			// 
			// button1
			// 
			button1.Location = new Point(12, 12);
			button1.Name = "button1";
			button1.Size = new Size(112, 34);
			button1.TabIndex = 0;
			button1.Text = "Go";
			button1.UseVisualStyleBackColor = true;
			button1.Click += OnGo;
			// 
			// button2
			// 
			button2.Location = new Point(12, 92);
			button2.Name = "button2";
			button2.Size = new Size(112, 34);
			button2.TabIndex = 3;
			button2.Text = "Step";
			button2.UseVisualStyleBackColor = true;
			button2.Click += OnStep;
			// 
			// button4
			// 
			button4.Location = new Point(12, 52);
			button4.Name = "button4";
			button4.Size = new Size(112, 34);
			button4.TabIndex = 5;
			button4.Text = "Stop";
			button4.UseVisualStyleBackColor = true;
			button4.Click += OnStop;
			// 
			// timeLabel
			// 
			timeLabel.AutoSize = true;
			timeLabel.Location = new Point(12, 436);
			timeLabel.Name = "timeLabel";
			timeLabel.Size = new Size(59, 25);
			timeLabel.TabIndex = 7;
			timeLabel.Text = "label2";
			// 
			// listBox1
			// 
			listBox1.FormattingEnabled = true;
			listBox1.ItemHeight = 25;
			listBox1.Location = new Point(12, 498);
			listBox1.Name = "listBox1";
			listBox1.Size = new Size(347, 679);
			listBox1.TabIndex = 8;
			listBox1.DoubleClick += OnMilestoneDoubleClick;
			// 
			// manualTaskslistBox
			// 
			manualTaskslistBox.FormattingEnabled = true;
			manualTaskslistBox.ItemHeight = 25;
			manualTaskslistBox.Location = new Point(365, 498);
			manualTaskslistBox.Name = "manualTaskslistBox";
			manualTaskslistBox.Size = new Size(363, 679);
			manualTaskslistBox.TabIndex = 10;
			manualTaskslistBox.DoubleClick += OnManualTaskRun;
			// 
			// textBox1
			// 
			textBox1.Location = new Point(12, 463);
			textBox1.Name = "textBox1";
			textBox1.Size = new Size(228, 31);
			textBox1.TabIndex = 11;
			// 
			// button5
			// 
			button5.Location = new Point(246, 461);
			button5.Name = "button5";
			button5.Size = new Size(112, 34);
			button5.TabIndex = 12;
			button5.Text = "Save";
			button5.UseVisualStyleBackColor = true;
			// 
			// infoTextBox
			// 
			infoTextBox.Location = new Point(365, 12);
			infoTextBox.Multiline = true;
			infoTextBox.Name = "infoTextBox";
			infoTextBox.ReadOnly = true;
			infoTextBox.Size = new Size(363, 480);
			infoTextBox.TabIndex = 13;
			// 
			// statusLabel
			// 
			statusLabel.AutoSize = true;
			statusLabel.Location = new Point(130, 17);
			statusLabel.Name = "statusLabel";
			statusLabel.Size = new Size(59, 25);
			statusLabel.TabIndex = 14;
			statusLabel.Text = "status";
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(10F, 25F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1104, 1189);
			Controls.Add(statusLabel);
			Controls.Add(infoTextBox);
			Controls.Add(button5);
			Controls.Add(textBox1);
			Controls.Add(manualTaskslistBox);
			Controls.Add(listBox1);
			Controls.Add(timeLabel);
			Controls.Add(button4);
			Controls.Add(button2);
			Controls.Add(button1);
			Name = "Form1";
			Text = "Form1";
			FormClosed += OnFormClosed;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button button1;
		private Button button2;
		private Button button4;
		private Label timeLabel;
		private ListBox listBox1;
		private ListBox manualTaskslistBox;
		private TextBox textBox1;
		private Button button5;
		private TextBox infoTextBox;
		private Label statusLabel;
	}
}