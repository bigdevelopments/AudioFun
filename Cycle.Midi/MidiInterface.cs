using System.Runtime.InteropServices;

namespace Cycle.Midi;

public class MidiInterface
{
	private MidiInputInterface[] _inputDevices;

	public MidiInterface()
	{
		Refresh();
	}

	public MidiInputInterface[] Inputs => _inputDevices;

	public void Refresh()
	{
		int inputCount = NativeMethods.midiInGetNumDevs();
		_inputDevices = new MidiInputInterface[inputCount];

		for (int index = 0; index < inputCount; index++)
		{
			MIDIINCAPS caps = new MIDIINCAPS();
			NativeMethods.midiInGetDevCaps(0, ref caps, (uint)Marshal.SizeOf(caps.GetType()));
			_inputDevices[index] = new MidiInputInterface(caps.szPname);
			_inputDevices[index].Start();
		}
	}
}
