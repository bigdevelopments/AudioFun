using System.Numerics;

namespace Cycle.Core.Core;

public class WaveFormGenerator
{
	private readonly int _length;
	private readonly double _scaling;

	public WaveFormGenerator(int length)
	{
		_length = length;
		_scaling = 0.54d;
	}

	public float[] GenerateSineWave(float phase = 0)
	{
		phase = phase % 1f;
		float[] samples = new float[_length];
		for (int index = 0; index < _length; index++) samples[index] = (float)(Math.Sin(Math.PI * 2 * phase + Math.PI * 2 * index / _length) * _scaling);
		return samples;
	}

	public float[] GenerateSawWave(int harmonics, float phase = 0)
	{
		phase = phase % 1f;
		float[] samples = new float[_length];
		for (int index = 0; index < _length; index++)
		{
			double sample = 0f;
			for (int harmonic = 1; harmonic <= harmonics; harmonic+=1)
			{
				if (harmonic % 2 == 1)
				{
					sample -= Math.Sin(Math.PI * 2 * harmonic * (phase +  index / (double)_length)) / harmonic;
				}
				else
				{
					sample += Math.Sin(Math.PI * 2 * harmonic * (phase + index / (double)_length)) / harmonic;
				}
			}
			samples[index] = (float)(-sample * _scaling);
		//	if (MathF.Abs(samples[index]) > 1f) throw new Exception();
		}

		return samples;
	}

	public float[] GenerateSquareWave(int harmonics, float phase = 0)
	{
		phase = phase % 1f;
		float[] samples = new float[_length];
		for (int index = 0; index < _length; index++)
		{
			double sample = 0f;
			for (int harmonic = 1; harmonic <= harmonics; harmonic += 2)
			{
				sample += Math.Sin(Math.PI * 2 * harmonic * (phase + index / (double)_length)) / harmonic;
			}
			samples[index] = (float)(sample * _scaling);
			if (MathF.Abs(samples[index]) > 1f) throw new Exception();
		}

		return samples;
	}
}
