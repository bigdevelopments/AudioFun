using AudioComponents.Core;

namespace AudioComponents.Midi;

public static class MidiMessage
{
	public static Message NoteOn(int channel, int note, int velocity)
	{
		if (channel < 0 || channel > 15) throw new ArgumentOutOfRangeException(nameof(channel), "Channel must be between 0 and 15.");
		if (note < 0 || note > 127) throw new ArgumentOutOfRangeException(nameof(note), "Note must be between 0 and 127.");
		if (velocity < 0 || velocity > 127) throw new ArgumentOutOfRangeException(nameof(velocity), "Velocity must be between 0 and 127.");
		return new Message(0x01900000 | channel << 16 | note << 8 | velocity);
	}

	public static Message NoteOff(int channel, int note, int velocity)
	{
		if (channel < 0 || channel > 15) throw new ArgumentOutOfRangeException(nameof(channel), "Channel must be between 0 and 15.");
		if (note < 0 || note > 127) throw new ArgumentOutOfRangeException(nameof(note), "Note must be between 0 and 127.");
		if (velocity < 0 || velocity > 127) throw new ArgumentOutOfRangeException(nameof(velocity), "Velocity must be between 0 and 127.");
		return new Message(0x01800000 | channel << 16 | note << 8 | velocity);
	}

	public static bool IsNoteOn(Message message, out int channel, out int note, out int velocity)
	{
		if ((message.Data & 0xfff00000) != 0x01900000)
		{
			channel = 0;
			note = 0;
			velocity = 0;
			return false;
		}
		channel = (int)(message.Data >> 16 & 0x0f);
		note = (int)(message.Data >> 8 & 0x7f);
		velocity = (int)(message.Data & 0x7f);
		return true;
	}

	public static bool IsNoteOff(Message message, out int channel, out int note, out int velocity)
	{
		if ((message.Data & 0xfff00000) != 0x01800000)
		{
			channel = 0;
			note = 0;
			velocity = 0;
			return false;
		}
		channel = (int)(message.Data >> 16 & 0x0f);
		note = (int)(message.Data >> 8 & 0x7f);
		velocity = (int)(message.Data & 0x7f);
		return true;
	}
}
