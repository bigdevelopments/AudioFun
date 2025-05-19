using System.Runtime.InteropServices;

namespace MidiInterop;

public class MidiDevices
{
	private MidiInput[] _inputDevices;

	public MidiDevices()
	{
		Refresh();
	}

	public MidiInput[] Inputs => _inputDevices;

	public void Refresh()
	{
		int inputCount = NativeMethods.midiInGetNumDevs();
		_inputDevices = new MidiInput[inputCount];

		for (int index = 0; index < inputCount; index++)
		{
			MIDIINCAPS caps = new MIDIINCAPS();
			NativeMethods.midiInGetDevCaps(0, ref caps, (uint)Marshal.SizeOf(caps.GetType()));
			_inputDevices[index] = new MidiInput(caps.szPname);
		}
	}
}
