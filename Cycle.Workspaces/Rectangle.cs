using SkiaSharp;

namespace Cycle.Workspaces;

public class Rectangle : Tile
{
	// properties
	public int Width { get; set; } = 17;
	public int Height { get; set; } = 3;
	public SKColor BackgroundColour { get; set; } = new SKColor(0x60, 0x10, 0x00);
	public SKColor BorderColour { get; set; } = new SKColor(0xc0, 0x80, 0x30);	

	public override int SizeAcross => Width;
	public override int SizeDown => Height;

	public override void Draw(SKCanvas canvas)
	{
		var rect = new SKRect(2, 2, Width * 20 - 2, Height * 20 - 2);
		float cornerRadius = 12f;

		using (var paint = new SKPaint())
		{
			// Draw filled rounded rectangle
			paint.Color = BackgroundColour;
			paint.Style = SKPaintStyle.Fill;
			canvas.DrawRoundRect(rect, cornerRadius, cornerRadius, paint);

			// Draw border rounded rectangle
			paint.Color = BorderColour;
			paint.Style = SKPaintStyle.Stroke;
			paint.StrokeWidth = 2;
			canvas.DrawRoundRect(rect, cornerRadius, cornerRadius, paint);
		}
	}
}
