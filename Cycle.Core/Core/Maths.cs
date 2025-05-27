using System.Runtime.CompilerServices;

namespace Cycle.Core.Core;

public class Maths
{
	// a quarter sine wave table for fast sine calculations
	private const int QuarterSineTableSize = 256;
	private const int FullSineTableSize = QuarterSineTableSize * 4;
	private static readonly float[] _quarterSine = new float[QuarterSineTableSize];

	// notes in the diatonic scale
	private static readonly string[] Notes = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };

	static Maths()
	{
		// precompute the quarter sine wave table
		double divisor = Math.PI / (2d * _quarterSine.Length);
		for (int index = 0; index < _quarterSine.Length; index++) _quarterSine[index] = (float)Math.Sin(index * divisor);
	}

	public static double NoteToFrequency(float note) => MathF.Pow(2, (note - 57f) / 12f) * 440f;


	public static double VelocityToAmplitude(float velocity)
	{
		if (velocity == 0d) return 0d;
		var result = (MathF.Log(velocity / 127f) + 5) / 5;
		//	Console.WriteLine($"{velocity} : {result}");
		return result;
	}

	public static string GetNoteName(float note)
	{
		int index = (int)(note - 9);
		if (index < 0) return "--";
		return $"{Notes[index % 12]}{index / 12}";
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Min(float lhs, float rhs) => lhs <= rhs ? lhs : rhs;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Max(float lhs, float rhs) => lhs > rhs ? lhs : rhs;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float UnitWrap(float value)
	{
		if (value < 0f || value > 1f) return value - MathF.Floor(value);
		return value;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static float GetSine(int index)
	{
		if (index < QuarterSineTableSize) return _quarterSine[index];
		if (index < 2 * QuarterSineTableSize) return _quarterSine[2 * QuarterSineTableSize - index - 1];
		if (index < 3 * QuarterSineTableSize) return -_quarterSine[index - 2 * QuarterSineTableSize];
		return -_quarterSine[FullSineTableSize - index - 1];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Sin(float phase)
	{
		// keep phase in the range 0-1, the if saves the expensive flooring if not needed
		phase = UnitWrap(phase);

		// work out the left and right sample and factor
		float scaledIndex = phase * FullSineTableSize;
		int left = (int)scaledIndex;
		int right = (left + 1) & 1023;
		float frac = scaledIndex - left;

		// get neighbouring sine values
		float from = GetSine(left);
		float to = GetSine(right);

		// linear interpolate between them
		return from + (to - from) * frac;
	}
}