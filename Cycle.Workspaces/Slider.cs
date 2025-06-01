using SkiaSharp;

using System.Numerics;

namespace Cycle.Workspaces;

public class Slider : Tile
{
	// model
	public Variable Variable { get; set; }

	private int _width;
	private int _height;

	// properties
	public int Width
	{
		get => _width;
		set
		{
			_width = value;
			Calculate();

		}
	}
	public int Height
	{
		get => _height;
		set
		{
			_height = value;
			Calculate();
		}
	}

	public SKColor BackgroundColour { get; set; } = new SKColor(0x40, 0x40, 0x40);
	public SKColor SliderColour { get; set; } = SKColors.Black;
	public SKColor KnobColour { get; set; } = SKColors.LightGray;

	public override int SizeAcross => _width;
	public override int SizeDown => _height;

	private SKRect _knobArea;
	private SKRect _knobHitbox;
	private bool _dragging;
	private SKRect _sliderArea;
	private SKRect _tileArea;

	private void Calculate()
	{
		_tileArea = new SKRect(2, 2, _width * 20 - 2, _height * 20 - 2);

		_sliderArea = new SKRect(_width * 20 / 2f - 3, 10, _width * 20 / 2f + 3, _height*20 - 10);
	}


	public override void Draw(SKCanvas canvas)
	{
		var area = new SKRect(2, 2, _width * 20 - 2, _height * 20 - 2);

		var position = (Variable.Value - Variable.MinValue) / (Variable.MaxValue - Variable.MinValue);
		var height = 8f + (_height - 1) * 20 * position;
		_knobArea = new SKRect(5, height, _width * 20 - 5, height + 4);
		_knobHitbox = new SKRect(5, height - 2, _width * 20 - 5, height + 6);
		float cornerRadius = 2f;

		using (var paint = new SKPaint())
		{
			// Draw filled rounded rectangle
			paint.Color = BackgroundColour;
			paint.Style = SKPaintStyle.Fill;
			canvas.DrawRect(area, paint);

			paint.Color = SliderColour;
			canvas.DrawRect(_sliderArea, paint);

			paint.Color = KnobColour;

			canvas.DrawRoundRect(_knobArea, cornerRadius, cornerRadius, paint);
		}
	}

	public override bool OnMouseDown(Vector2 localPosition)
	{
	//	if (_knobHitbox.Contains(localPosition.X, localPosition.Y))
		{
			_dragging = true;
			return true;
		}

		return false;
	}

	public override void OnMouseUp(Vector2 localPosition)
	{
		_dragging = false;
	}

	public override void OnMouseMove(Vector2 localPosition)
	{
		if (_dragging)
		{
			var position = (localPosition.Y - _sliderArea.Top) / _sliderArea.Height;

			position = Math.Clamp(position, 0, 1);
			Variable.Value = Variable.MinValue + position * (Variable.MaxValue - Variable.MinValue);
		}
	}
}
