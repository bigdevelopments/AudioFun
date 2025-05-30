using Cycle.Core.Core;

namespace CrapWaveViewer
{
	public partial class Form1 : Form
	{
		private readonly WaveFormGenerator waveFormGenerator = new WaveFormGenerator(1024);

		public Form1()
		{
			InitializeComponent();
			comboBox1.SelectedIndex = 0; // Set default selection to the first item
			trackBar1.Value = 1; // Set default value for the trackbar
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			Draw();
		}

		private void trackBar1_Scroll(object sender, EventArgs e)
		{
			Draw();
		}

		private void trackBar2_Scroll(object sender, EventArgs e)
		{
			Draw();
		}

		private void Draw()
		{
			int harmonics = trackBar1.Value;
			float phase = trackBar2.Value / 1000f;
			float duty = trackBar3.Value / 1000f;

			Text = $"Harmonics: {trackBar1.Value} Duty: {(int)(trackBar3.Value/10)}%";
			switch (comboBox1.SelectedIndex)
			{
				case 0: // Sine Wave
					viewControl1.SetWaveForm(waveFormGenerator.GenerateSineWave(phase));
					break;
				case 1: // Saw Wave
					viewControl1.SetWaveForm(waveFormGenerator.GenerateSawWave(trackBar1.Value, phase));
					break;
				case 2:
					viewControl1.SetWaveForm(waveFormGenerator.GenerateSquareWave(trackBar1.Value, phase));
					break;
				case 3:
					viewControl1.SetWaveForm(waveFormGenerator.GenerateTriangleWave(trackBar1.Value, phase));
					break;
				case 4:
					float[] one = waveFormGenerator.GenerateSawWave(trackBar1.Value, phase);
					float[] two = waveFormGenerator.GenerateSawWave(trackBar1.Value, phase+duty);
					for (int index = 0; index < 1024; index++)
					{
						one[index] -= two[index];
						one[index] /= 2;
					}

					viewControl1.SetWaveForm(one);
					break;

				case 5:
					float[] one1 = waveFormGenerator.GenerateSawWave(trackBar1.Value, phase);
					float[] two1 = waveFormGenerator.GenerateSawWave(trackBar1.Value, phase + duty);
					for (int index = 0; index < 1024; index++)
					{
						one1[index] += two1[index] + (duty / 30); // 30 is trial and error - I don't know the significance of this value
						one1[index] /= 2;
					}

					viewControl1.SetWaveForm(one1);
					break;

				default:
					viewControl1.SetWaveForm(Array.Empty<float>());
					break;
			}
		}

		private void trackBar3_Scroll(object sender, EventArgs e)
		{
			Draw();
		}
	}
}
