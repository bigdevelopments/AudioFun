namespace Cycle.Core.Midi;

/// <summary>
/// A polyphonic driver assigns incoming MIDI information into polyphonic lanes
/// </summary>
/// <remarks>
/// The input is midi, which is checked against a channel Mask. The output is lane outputs, one for each polyphonic note,
/// with each lane providing a trigger, frequency, amplitude and modulation outputs.
/// </remarks>
[Primitive("poly_bridge", "MIDI Polyphony Driver")]
public class PolyBridge : Component
{
	// an array of lanes and what note is assigned no them
	private readonly int[] _notesInUse;

	// three outputs per lane, frequency, amplitude and modulation
	private readonly MidiInput _midiInput;
	private readonly SignalOutput[] _triggerOutputs;
	private readonly SignalOutput[] _frequencyOutputs;
	private readonly SignalOutput[] _amplitudeOutputs;
	private readonly SignalOutput[] _modulationOutputs;

	// channel mask (0-15) for channels to respond to
	private ushort _channelMask;

	// cache of pitch bend
	private float _pitchBend = 0f;

	private float _pitchBendScale = 12f;

	public PolyBridge(params string[] parameters)
	{
		if (parameters?.Length < 1 || !int.TryParse(parameters[0], out var polyphony))
		{
			throw new ArgumentException("Polyphony driver requires at least one parameter: the polyphony level", nameof(parameters));
		}

		if (polyphony < 1 || polyphony > 16)
		{
			throw new ArgumentOutOfRangeException(nameof(polyphony), "Polyphony driver supports between 2 and 64 polyphony channels");
		}

		var channelMask = 0xffff; // default to all channels	

		if (polyphony < 0 || polyphony > 63) throw new Exception("Polyphony must be between 0 and 63.");
		if (channelMask < 0 || channelMask > 0xffff) throw new Exception("Channel mask must be between 0 and 65535.");

		_midiInput = AddMidiInput("midi_in", MidiProc);

		_notesInUse = new int[polyphony];

		_triggerOutputs = new SignalOutput[polyphony];
		_frequencyOutputs = new SignalOutput[polyphony];
		_amplitudeOutputs = new SignalOutput[polyphony];
		_modulationOutputs = new SignalOutput[polyphony];

		for (int index = 0; index < polyphony; index++)
		{
			_notesInUse[index] = 0; // completeness :)
			_triggerOutputs[index] = AddSignalOutput($"trg_{index + 1}");
			_frequencyOutputs[index] = AddSignalOutput($"frq_{index + 1}");
			_amplitudeOutputs[index] = AddSignalOutput($"amp_{index + 1}");
			_modulationOutputs[index] = AddSignalOutput($"mod_{index + 1}");
		}

		_channelMask = (ushort)channelMask;
	}


	private void MidiProc(Message message)
	{
		if (MidiMessage.IsNoteOn(message, _channelMask, out var onNote, out var onVelocity))
		{
			int lane = FindBestLane(onNote);
			_notesInUse[lane] = onNote;
			var frequency = MidiScales.NoteToFrequency(onNote + _pitchBend* _pitchBendScale);
			var amplitude = onVelocity / 127f;
			_triggerOutputs[lane].Value = Vector2.One; // trigger on note on
			_frequencyOutputs[lane].Value = new Vector2(frequency, frequency);
			_amplitudeOutputs[lane].Value = new Vector2(amplitude, amplitude);
		}

		if (MidiMessage.IsNoteOff(message, _channelMask, out var offNote, out var offVelocity))
		{
			// find lane for this note
			int lane = Array.IndexOf(_notesInUse, offNote);

			// if we can't find it, its probably been sacrificed, just ignore it
			if (lane == -1) return;

			_triggerOutputs[lane].Value = Vector2.Zero;
			//_frequencyOutputs[lane].Value = Vector2.Zero;
			//_amplitudeOutputs[lane].Value = Vector2.Zero;
			//_modulationOutputs[lane].Value = Vector2.Zero;

			// free up the lane
			_notesInUse[lane] = 0;
		}

		if (MidiMessage.IsPitchBend(message, _channelMask, out var bend))
		{
			if (bend == _pitchBend) return;
			_pitchBend = bend;

			// update all lanes with the new pitch bend
			for (int index = 0; index < _notesInUse.Length; index++)
			{
				if (_notesInUse[index] == 0) continue; // skip empty lanes
				var frequency = MidiScales.NoteToFrequency(_notesInUse[index] + _pitchBend*_pitchBendScale);
				_frequencyOutputs[index].Value = new Vector2(frequency, frequency);
			}
		}
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
}
