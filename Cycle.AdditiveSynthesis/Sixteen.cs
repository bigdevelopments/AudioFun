using Cycle.Core.Core;

using System.Numerics;
using System.Runtime.CompilerServices;

namespace Cycle.AdditiveSynthesis;

/// <summary>
/// We move in 16s for additive synthesis, so this represents sixteen 32 bit floating point values
/// </summary>
internal struct Sixteen
{
	public Vector4 Band1;
	public Vector4 Band2;
	public Vector4 Band3;
	public Vector4 Band4;

	public Sixteen(float value)
	{
		var asVector = new Vector4(value);
		Band1 = asVector;
		Band2 = asVector;
		Band3 = asVector;
		Band4 = asVector;
	}

	public Sixteen()
	{
		Band1 = Vector4.Zero;
		Band2 = Vector4.Zero;
		Band3 = Vector4.Zero;
		Band4 = Vector4.Zero;
	}

	public static Sixteen Harmonics
	{
		get
		{
			return new Sixteen
			{
				Band1 = new Vector4(1, 2, 3, 4),
				Band2 = new Vector4(5, 6, 7, 8),
				Band3 = new Vector4(9, 10, 11, 12),
				Band4 = new Vector4(13, 14, 15, 16)
			};
		}
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Sixteen operator +(Sixteen a, Sixteen b)
	{
		return new Sixteen
		{
			Band1 = a.Band1 + b.Band1,
			Band2 = a.Band2 + b.Band2,
			Band3 = a.Band3 + b.Band3,
			Band4 = a.Band4 + b.Band4
		};
	}

	public float Sum()
	{
		return Band1.X + Band1.Y + Band1.Z + Band1.W +
			   Band2.X + Band2.Y + Band2.Z + Band2.W +
			   Band3.X + Band3.Y + Band3.Z + Band3.W +
			   Band4.X + Band4.Y + Band4.Z + Band4.W;
	}



	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Sixteen operator +(float a, Sixteen b)
	{
		var asVector = new Vector4(a);
		return new Sixteen
		{
			Band1 = asVector + b.Band1,
			Band2 = asVector + b.Band2,
			Band3 = asVector + b.Band3,
			Band4 = asVector + b.Band4
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Sixteen operator -(float a, Sixteen b)
	{
		var asVector = new Vector4(a);
		return new Sixteen
		{
			Band1 = asVector - b.Band1,
			Band2 = asVector - b.Band2,
			Band3 = asVector - b.Band3,
			Band4 = asVector - b.Band4
		};
	}


	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Sixteen operator *(Sixteen a, Sixteen b)
	{
		return new Sixteen
		{
			Band1 = a.Band1 * b.Band1,
			Band2 = a.Band2 * b.Band2,
			Band3 = a.Band3 * b.Band3,
			Band4 = a.Band4 * b.Band4
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Sixteen operator *(Sixteen a, float b)
	{
		return new Sixteen
		{
			Band1 = a.Band1 * b,
			Band2 = a.Band2 * b,
			Band3 = a.Band3 * b,
			Band4 = a.Band4 * b
		};
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Sixteen Sin(Sixteen phases)
	{
		return new Sixteen
		{
			// TODO: need to review this, might be better to use the Core lookup
			Band1 = Vector4.Sin(phases.Band1 * MathF.PI * 2),
			Band2 = Vector4.Sin(phases.Band2 * MathF.PI * 2),
			Band3 = Vector4.Sin(phases.Band3 * MathF.PI * 2),
			Band4 = Vector4.Sin(phases.Band4 * MathF.PI * 2)

		};
	}
}