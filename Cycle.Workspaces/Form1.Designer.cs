﻿namespace Cycle.Workspaces
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
			viewControl1 = new HostControl();
			SuspendLayout();
			// 
			// viewControl1
			// 
			viewControl1.BackColor = Color.Black;
			viewControl1.Dock = DockStyle.Fill;
			viewControl1.Location = new Point(0, 0);
			viewControl1.Margin = new Padding(4, 3, 4, 3);
			viewControl1.Name = "viewControl1";
			viewControl1.Size = new Size(800, 450);
			viewControl1.TabIndex = 0;
			viewControl1.VSync = true;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(800, 450);
			Controls.Add(viewControl1);
			Name = "Form1";
			Text = "Form1";
			ResumeLayout(false);
		}

		#endregion

		private HostControl viewControl1;
	}
}
