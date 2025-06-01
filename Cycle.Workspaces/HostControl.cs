using SkiaSharp;
using SkiaSharp.Views.Desktop;

using System.ComponentModel;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Cycle.Workspaces;

[DesignerCategory("Code")]
public class HostControl : SKGLControl
{
	public const int TileSize = 20;
	private readonly List<Tile> _tiles;
	private Vector2 DragOrigin;
	private Vector2 Origin;
	private bool _dragging;
	private Tile _captured;

	public HostControl()
	{
		_captured = null;
		Variable v = new Variable();
		v.MinValue = 0;
		v.MaxValue = 1;
		v.Value = 0.5f;

		_tiles = new List<Tile>();
		Rectangle r = new Rectangle();
		r.X = 10;
		r.Y = 10;
		Slider slider = new();
		slider.Variable = v;
		slider.X = 15;
		slider.Y = 15;
		slider.Width = 1;
		slider.Height = 5;
		_tiles.Add(slider);
		slider = new();
		slider.Variable = v;
		slider.X = 20;
		slider.Y = 15;
		slider.Width = 2;
		slider.Height = 15;
		_tiles.Add(slider);

		_tiles.Add(r);
	}

	protected override void OnResize(EventArgs e)
	{
		base.OnResize(e);
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		//		base.OnMouseDown(e);
		var tile = FindTile(new Vector2(e.X, e.Y) + Origin);

		if (tile == null)
		{
			_dragging = true;
			DragOrigin = new Vector2(e.X, e.Y);
		}
		else
		{
			var localPosition = new Vector2(e.X, e.Y) + Origin - new Vector2(tile.X * TileSize, tile.Y * TileSize);
			if (tile.OnMouseDown(localPosition))
			{
				_captured = tile;
				NativeMethods.SetCapture(Handle);
			}
		}

	}

	protected override void OnMouseUp(MouseEventArgs e)
	{
		//	base.OnMouseUp(e);
		if (_dragging)
		{
			_dragging = false;
		}
		else
		{
			if (_captured != null)
			{
				var localPosition = new Vector2(e.X, e.Y) + Origin - new Vector2(_captured.X * TileSize, _captured.Y * TileSize);
				_captured.OnMouseUp(localPosition);
				return;
			}


			var tile = FindTile(new Vector2(e.X, e.Y) + Origin);
			if (tile != null)
			{
				var localPosition = new Vector2(e.X, e.Y) + Origin - new Vector2(tile.X * TileSize, tile.Y * TileSize);
				tile.OnMouseUp(localPosition);
			}
			_captured = null;
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		//base.OnMouseMove(e);
		if (_dragging)
		{
			Origin -= (new Vector2(e.X, e.Y) - DragOrigin);
			DragOrigin = new Vector2(e.X, e.Y);
			Invalidate();
		}
		else
		{
			if (_captured != null)
			{
				var localPosition = new Vector2(e.X, e.Y) + Origin - new Vector2(_captured.X * TileSize, _captured.Y * TileSize);
				_captured.OnMouseMove(localPosition);
				return;
			}

			var tile = FindTile(new Vector2(e.X, e.Y) + Origin);
			if (tile == null)
			{
				Cursor = Cursors.Hand;
			}
			else
			{
				Cursor = Cursors.Default;
				var localPosition = new Vector2(e.X, e.Y) + Origin - new Vector2(tile.X * TileSize, tile.Y * TileSize);
				tile.OnMouseMove(localPosition);
			}
		}
	}

	protected override void OnPaintSurface(SKPaintGLSurfaceEventArgs e)
	{
		e.Surface.Canvas.Clear(new SKColor(0x30, 0x30, 0x30));

		//if (View != null)
		//{
		//	View.Render(e.Surface.Canvas);
		//}
		//else
		//{
		//	e.Surface.Canvas.Clear(SKColors.Black);
		//}

		var color = new SKColor(0x20, 0x20, 0x20);

		for (int x = 0; x < e.Info.Width; x += 20)
		{
			e.Surface.Canvas.DrawLine(x + 2 - Origin.X % TileSize, 0, x + 2 - Origin.X % TileSize, e.Info.Height, new SKPaint { Color = color, StrokeWidth = 2 });
		}

		for (int y = 0; y < e.Info.Height; y += 20)
		{
			e.Surface.Canvas.DrawLine(0, y + 2 - Origin.Y % TileSize, e.Info.Width, y + 2 - Origin.Y % TileSize, new SKPaint { Color = color, StrokeWidth = 2 });
		}

		//	e.Surface.Canvas.SetMatrix(SKMatrix.CreateScale(20, 20));
		foreach (var tile in _tiles)
		{
			e.Surface.Canvas.SetMatrix(SKMatrix.CreateTranslation(2 + tile.X * TileSize - Origin.X, 2 + tile.Y * TileSize - Origin.Y));

			tile.Draw(e.Surface.Canvas);
			e.Surface.Canvas.ResetMatrix();

		}

		//	e.Surface.Canvas.ResetMatrix();

		Invalidate();
	}

	private Tile FindTile(Vector2 position)
	{
		foreach (var tile in _tiles)
		{
			if (tile.X * TileSize <= position.X && position.X < (tile.X + tile.SizeAcross) * TileSize &&
				tile.Y * TileSize <= position.Y && position.Y < (tile.Y + tile.SizeDown) * TileSize)
			{
				return tile;
			}
		}
		return null;
	}
}
