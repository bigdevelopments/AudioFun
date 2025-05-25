namespace Cycle.Core.Midi;

public static class MidiScales
{
	public static float NoteToFrequency(float noteNumber)
	{
		return 440f * (float)Math.Pow(2, (noteNumber - 69f) / 12f);
	}
}
