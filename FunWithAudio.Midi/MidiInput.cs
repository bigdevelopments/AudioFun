using AudioComponents.Core;
using AudioComponents.Midi;

namespace MidiInterop;

public class MidiInput
{
	// where to send the midi messages that arrive
	private Component _target;

	// native handle
	private nint _handle;

	internal MidiInput(string name)
	{ 
		_handle = nint.Zero;
	}

	public void Start(Component target)
	{
		if (_handle != nint.Zero) throw new InvalidOperationException("Input already started");
		_target = target;

		// start things off
		int result = NativeMethods.InOpen(out _handle, 0, HandleMessage, 123, 0x30000);
		NativeMethods.midiInStart(_handle);
	}

	public void Stop()
	{
		if (_handle == nint.Zero) throw new InvalidOperationException("Input not yet started");
		NativeMethods.midiInStop(_handle);
		NativeMethods.midiInClose(_handle);
		_handle = nint.Zero;
	}

	private void HandleMessage(int hnd, InputMessages msg, int instance, int param1, int param2)
	{
		if (msg == InputMessages.Data)
		{
			byte b1 = (byte)param1;
			byte b2 = (byte)(param1 >> 8);
			byte b3 = (byte)(param1 >> 16);
			Message message = MidiMessage.Midi(b1, b2, b3);
			_target?.OnNotify(message);
			Console.WriteLine(message);
		}
	}
}