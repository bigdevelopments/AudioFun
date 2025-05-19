namespace FunWithAudio.Common;

public class Maths
{
	private static readonly string[] Notes = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };
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

	public static float Min(float lhs, float rhs) => lhs <= rhs ? lhs : rhs;
	public static float Max(float lhs, float rhs) => lhs > rhs ? lhs : rhs;
}

