using AudioComponents.Audio;
namespace AudioComponents.Midi;

/// <summary>
/// A polyphonic driver assigns incoming MIDI information into polyphonic lanes
/// </summary>
/// <remarks>
/// The input is midi, which is checked against a channel Mask. The output is lane output, one for each polyphonic note
/// The X value is the frequency (from note and pitch bend), the Y value is the velocity (0-1, >0 = on, 0 = off), such
/// that the frequency can be fed into an oscillator or other ramp/lfo etc., and the velocity can trigger ADSRs etc.
/// </remarks>
public class PolyphonyDriver : Component
{
	// an array of lanes and what note is assigned no them
	private readonly int[] _notesInUse;

	// three outputs per lane, frequency, amplitude and modulation
	private readonly IOutputWriter[] _frequencyOutputs;
	private readonly IOutputWriter[] _amplitudeOutputs;
	private readonly IOutputWriter[] _modulationOutputs;

	// channel mask (0-15) for channels to respond to
	private ushort _channelMask;

	// cache of pitch bend
	private float _pitchBend = 0f;

	public PolyphonyDriver(int polyphony, int channelMask = 0xffff)
	{
		if (polyphony < 0 || polyphony > 63) throw new Exception("Polyphony must be between 0 and 63.");
		if (channelMask < 0 || channelMask > 0xffff) throw new Exception("Channel mask must be between 0 and 65535.");
		
		_notesInUse = new int[polyphony];

		_frequencyOutputs = new IOutputWriter[polyphony];
		_amplitudeOutputs = new IOutputWriter[polyphony];
		_modulationOutputs = new IOutputWriter[polyphony];

		for (int index = 0; index < polyphony; index++)
		{
			_notesInUse[index] = 0; // completeness :)
			_frequencyOutputs[index] = AddOutput($"frequency-{index}");
			_amplitudeOutputs[index] = AddOutput($"amplitude-{index}");
			_modulationOutputs[index] = AddOutput($"modulation-{index}");
		}

		_channelMask = (ushort)channelMask;
	}

	public override Message OnNotify(Message message)
	{
		if (MidiMessage.IsNoteOn(message, _channelMask, out var onNote, out var onVelocity))
		{
			int lane = FindBestLane(onNote);
			_notesInUse[lane] = onNote;
			var frequency = MidiScales.NoteToFrequency(onNote + _pitchBend);
			var amplitude = onVelocity / 127f;
			_frequencyOutputs[lane].Value = new Vector2(frequency, frequency);
			_amplitudeOutputs[lane].Value = new Vector2(amplitude, amplitude);
			// no modulation yet
			return Message.Ok;
		}

		if (MidiMessage.IsNoteOff(message, _channelMask, out var offNote, out var offVelocity))
		{
			// find lane for this note
			int lane = Array.IndexOf(_notesInUse, offNote);

			// if we can't find it, its probably been sacrificed, just ignore it
			if (lane == -1) return Message.Ok;

			_frequencyOutputs[lane].Value = Vector2.Zero;
			_amplitudeOutputs[lane].Value = Vector2.Zero;
			_modulationOutputs[lane].Value = Vector2.Zero;

			// free up the lane
			_notesInUse[lane] = 0;
			return Message.Ok;
		}

		if (MidiMessage.IsPitchBend(message, _channelMask, out var bend))
		{
			var pitchBend = bend / 8192f; // 0-16384 to -1 to 1
			if (pitchBend == _pitchBend) return Message.Ok; // no change

			_pitchBend = pitchBend;
			// update all lanes with the new pitch bend
			for (int index = 0; index < _notesInUse.Length; index++)
			{
				if (_notesInUse[index] == 0) continue; // skip empty lanes
				var frequency = MidiScales.NoteToFrequency(_notesInUse[index] + _pitchBend);
				_frequencyOutputs[index].Value = new Vector2(frequency, frequency);
			}
		}

		return Message.Ok;
	}

	private int FindBestLane(int note)
	{
		int nearest = 0;
		int nearestIndex = 0;

		// going to iterate over the drivers to find the best one
		for (int index = 0; index < _notesInUse.Length; index++)
		{
			// if one is completely free, use that
			if (_notesInUse[index] == 0) return index;

			// meanwhile, as we go through, find the nearest one
			var distance = _notesInUse[index] - note;
			if (distance < 0) distance = -distance;

			// if no free ones, we'll use nearest (for now, perhaps duration, or level would be better sacrificial measure)
			if (distance < nearest)
			{
				nearest = distance;
				nearestIndex = index;
			}
		}

		return nearestIndex;
	}

	public override void Tick()
	{
		// nothing to do here
	}
}
