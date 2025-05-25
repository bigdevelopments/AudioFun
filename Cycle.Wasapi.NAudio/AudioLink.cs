using Cycle.Core.Audio;
using Cycle.Core.Core;

using NAudio.Wave;

namespace Cycle.Wasapi.NAudio;

public class AudioLink : ISampleProvider
{
	private readonly ComponentSurface _base;
	private readonly AudioOutput _audioOutput;
	private readonly WaveFormat _waveFormat;

	public AudioLink(ComponentSurface bse, AudioOutput audioOutput)
	{
		_base = bse;
		_audioOutput = audioOutput;
		_waveFormat = WaveFormat.CreateIeeeFloatWaveFormat(_base.SampleRate, 2);
	}

	public WaveFormat WaveFormat => _waveFormat;

	public int Read(float[] buffer, int offset, int count)
	{
		Thread.CurrentThread.Priority = ThreadPriority.Highest;

		if (count % 2 != 0) throw new ArgumentException("Buffer size must be even.");
		for (int index = 0; index < count; index += 2)
		{
			_base.Tick();
			buffer[offset + index] = _audioOutput.Left;
			buffer[offset + index + 1] = _audioOutput.Right;
		}
		return count;
	}
}
