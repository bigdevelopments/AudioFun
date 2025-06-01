using Cycle.Core.WaveTables;

namespace Cycle.Core.AudioComponents;

[Primitive("va_osc", "Wavetable based bandlimited virtual analog oscillator")]
public class VAOscillator : Component
{
	// build the band limited wavetables
	private static WaveTableSet WaveSet = VAWavetableFactory.Create();

	// Supported waveforms
	public enum WaveForm { Off, Sine, Square, Saw, Triangle, Pulse, Thing };

	// inputs and outputs
	private readonly SignalInput _frequency;
	private readonly SignalInput _duty;
	private readonly SignalOutput _output;

	// state
	private WaveForm _waveform;
	private Func<Vector2> _waveformHandler;

	private float _phase;

	public VAOscillator()
	{
		_frequency = AddSignalInput("frq");
		_duty = AddSignalInput("dty",0.5f);
		_output = AddSignalOutput("out");
		SelectWaveform(WaveForm.Saw);
	}

	public void SelectWaveform(WaveForm waveform)
	{
		if (waveform == _waveform) return;
		_waveform = waveform;
		switch (waveform)
		{
			case WaveForm.Sine:
				_waveformHandler = TickSine;
				break;
			case WaveForm.Square:
				_waveformHandler = TickSquare;
				break;
			case WaveForm.Saw:
				_waveformHandler = TickSaw;
				break;
		}
	}

	public override void Tick()
	{
		// phase increment is common to all waveshapes
		float phaseIncrement = _frequency.Value.X / SampleRate;
		_phase += phaseIncrement;
		while (_phase >= 1f) _phase -= 1f;

		

		_output.Value = _waveformHandler();
	}

	private Vector2 TickSine()
	{
		return WaveSet.Sample(0, _phase);
	}

	private Vector2 TickSquare()
	{
		return WaveSet.Sample(64, _phase);
	}

	private Vector2 TickSaw()
	{
		return WaveSet.Sample(128, _phase);
	}


}
