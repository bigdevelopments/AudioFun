namespace Cycle.Core.WaveTables;

/// <summary>
/// Used to create the virtual analog wavetable sets
/// </summary>
public static class VAWavetableFactory
{
	public static WaveTableSet Create(int width = 1024, int harmonics = 64)
	{
		// number of wave tables sine is 1, all the others (saw, square, triangle) are the number of harmonics
		int waveTables = 1 + harmonics * 3;

		// create the set
		var wavetableSet = new WaveTableSet(width, waveTables);

		WaveFormGenerator generator = new WaveFormGenerator(width);
		
		// the sine wave goes into zero
		wavetableSet.SetTable(0, generator.GenerateSineWave());

		for (int harmonic = 0; harmonic < harmonics; harmonic++)
		{
			// square wave
			wavetableSet.SetTable(1 + harmonic, generator.GenerateSquareWave(harmonic + 1));
		
			// sawtooth wave
			wavetableSet.SetTable(1 + harmonics + harmonic, generator.GenerateSawWave(harmonic + 1));

			// triangle wave
			wavetableSet.SetTable(1 + harmonics * 2 + harmonic, generator.GenerateTriangleWave(harmonic + 1));
		}

		// return the set
		return wavetableSet;
	}
}
