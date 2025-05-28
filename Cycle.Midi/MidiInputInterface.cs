using Cycle.Core.Core;
using Cycle.Core.Midi;

namespace Cycle.Midi;

public class MidiInputInterface : Component
{
	// the output
	private readonly MidiOutput _output;

	// we need to remember the name, as it's provided by hardware and not user supplied
	private readonly string _name;

	// we need to maintain this, otherwise the garbage collector will remove it
	// and we will get a crash when the callback is called
	private readonly MidiInProc _handler;

	// native handle
	private nint _handle;

	internal MidiInputInterface(string name)
	{
		_name = name;
		_output = AddMidiOutput("out");
		_handler = HandleMessage;

		_handle = nint.Zero;
	}

	public override string ToString() => _name;


	public string Name => _name;

	public void Start()
	{
		if (_handle != nint.Zero) throw new InvalidOperationException("Input already started");

		// start things off
		int result = NativeMethods.InOpen(out _handle, 0, _handler, 123, 0x30000);
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
			_output.Send(message);
			Console.WriteLine($"{Name}: {message}");
		}
	}

	public override void Tick()
	{
	}
}