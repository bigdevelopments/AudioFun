using SkiaSharp;

using System.Numerics;

namespace Cycle.Workspaces;

public abstract class Tile
{
	public int X { get; set; }
	public int Y { get; set; }

	public abstract int SizeAcross { get; }
	public abstract int SizeDown { get; }

	public abstract void Draw(SKCanvas canvas);

	public virtual bool OnMouseDown(Vector2 localPosition)
	{
		return false;
	}

	public virtual void OnMouseUp(Vector2 localPosition)
	{
		// nothing here
	}

	public virtual void OnMouseMove(Vector2 localPosition)
	{
		// nothing here
	}
}
