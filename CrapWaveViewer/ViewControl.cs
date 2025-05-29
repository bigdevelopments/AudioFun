
namespace CrapWaveViewer
{
	public class ViewControl : Panel
	{
		private float[] _waveForm;
		

		public ViewControl()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
			_waveForm = Array.Empty<float>();
		}

		public void SetWaveForm(float[] waveForm)
		{
			_waveForm = waveForm;
			Invalidate(); // Trigger a repaint
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.Clear(Color.Black); // Clear the background
			if (_waveForm.Length == 0) return;
			int width = ClientSize.Width;
			int height = ClientSize.Height;
			float scaleX = (float)width / _waveForm.Length;
			float scaleY = height / 2f;
			using (var pen = new Pen(Color.LightBlue, 2))
			{
				for (int i = 0; i < _waveForm.Length - 1; i++)
				{
					float x1 = i * scaleX;
					float y1 = height / 2f - _waveForm[i] * scaleY;
					float x2 = (i + 1) * scaleX;
					float y2 = height / 2f - _waveForm[i + 1] * scaleY;
					e.Graphics.DrawLine(pen, x1, y1, x2, y2);
				}
			}
		}

	}
}
