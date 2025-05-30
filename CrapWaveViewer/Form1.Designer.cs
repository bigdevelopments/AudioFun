namespace CrapWaveViewer
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
			viewControl1 = new ViewControl();
			comboBox1 = new ComboBox();
			trackBar1 = new TrackBar();
			trackBar2 = new TrackBar();
			label1 = new Label();
			label2 = new Label();
			label3 = new Label();
			trackBar3 = new TrackBar();
			((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
			((System.ComponentModel.ISupportInitialize)trackBar2).BeginInit();
			((System.ComponentModel.ISupportInitialize)trackBar3).BeginInit();
			SuspendLayout();
			// 
			// viewControl1
			// 
			viewControl1.Location = new Point(12, 12);
			viewControl1.Name = "viewControl1";
			viewControl1.Size = new Size(1115, 325);
			viewControl1.TabIndex = 0;
			// 
			// comboBox1
			// 
			comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
			comboBox1.FormattingEnabled = true;
			comboBox1.Items.AddRange(new object[] { "Sine", "Sawtooth", "Square", "Triangle", "Pulse", "Dunno what the hell this is" });
			comboBox1.Location = new Point(28, 363);
			comboBox1.Name = "comboBox1";
			comboBox1.Size = new Size(365, 28);
			comboBox1.TabIndex = 1;
			comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
			// 
			// trackBar1
			// 
			trackBar1.Location = new Point(28, 445);
			trackBar1.Maximum = 64;
			trackBar1.Minimum = 1;
			trackBar1.Name = "trackBar1";
			trackBar1.Size = new Size(365, 56);
			trackBar1.TabIndex = 2;
			trackBar1.Value = 1;
			trackBar1.Scroll += trackBar1_Scroll;
			// 
			// trackBar2
			// 
			trackBar2.Location = new Point(764, 445);
			trackBar2.Maximum = 1000;
			trackBar2.Name = "trackBar2";
			trackBar2.Size = new Size(365, 56);
			trackBar2.TabIndex = 3;
			trackBar2.Scroll += trackBar2_Scroll;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(28, 422);
			label1.Name = "label1";
			label1.Size = new Size(80, 20);
			label1.TabIndex = 4;
			label1.Text = "Harmonics";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(764, 422);
			label2.Name = "label2";
			label2.Size = new Size(47, 20);
			label2.TabIndex = 5;
			label2.Text = "Phase";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(764, 340);
			label3.Name = "label3";
			label3.Size = new Size(40, 20);
			label3.TabIndex = 7;
			label3.Text = "Duty";
			// 
			// trackBar3
			// 
			trackBar3.Location = new Point(764, 363);
			trackBar3.Maximum = 999;
			trackBar3.Name = "trackBar3";
			trackBar3.Size = new Size(365, 56);
			trackBar3.TabIndex = 6;
			trackBar3.Value = 500;
			trackBar3.Scroll += trackBar3_Scroll;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(8F, 20F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(1139, 514);
			Controls.Add(label3);
			Controls.Add(trackBar3);
			Controls.Add(label2);
			Controls.Add(label1);
			Controls.Add(trackBar2);
			Controls.Add(trackBar1);
			Controls.Add(comboBox1);
			Controls.Add(viewControl1);
			FormBorderStyle = FormBorderStyle.FixedDialog;
			Name = "Form1";
			Text = "Form1";
			((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
			((System.ComponentModel.ISupportInitialize)trackBar2).EndInit();
			((System.ComponentModel.ISupportInitialize)trackBar3).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private ViewControl viewControl1;
		private ComboBox comboBox1;
		private TrackBar trackBar1;
		private TrackBar trackBar2;
		private Label label1;
		private Label label2;
		private Label label3;
		private TrackBar trackBar3;
	}
}
