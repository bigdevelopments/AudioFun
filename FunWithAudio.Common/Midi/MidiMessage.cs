namespace AudioComponents.Midi;

public static class MidiMessage
{
	public static Message Midi(byte status, byte data1, byte data2)
	{
		if (data1 < 0 || data1 > 127) throw new ArgumentOutOfRangeException(nameof(data1), "Data1 must be between 0 and 127.");
		if (data2 < 0 || data2 > 127) throw new ArgumentOutOfRangeException(nameof(data2), "Data2 must be between 0 and 127.");
		return new Message(0x01000000 | status << 16 | data1 << 8 | data2);
	}

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

	public static bool IsNoteOn(Message message, ushort channelMask, out int note, out int velocity)
	{
		// if note not on or channel not in mask, it's not for us
		if ((message.Data & 0xfff00000) != 0x01900000 || (channelMask & (ushort)(message.Data >> 16)) == 0)
		{
			note = 0;
			velocity = 0;
			return false;
		}

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

	public static bool IsNoteOff(Message message, ushort channelMask, out int note, out int velocity)
	{
		if ((message.Data & 0xfff00000) != 0x01800000 || (channelMask & (ushort)(message.Data >> 16)) == 0)
		{
			note = 0;
			velocity = 0;
			return false;
		}
		note = (int)(message.Data >> 8 & 0x7f);
		velocity = (int)(message.Data & 0x7f);
		return true;
	}

	public static bool IsPitchBend(Message message, ushort channelMask, out int bend)
	{
		if ((message.Data & 0xfff00000) != 0x01e00000 || (channelMask & (ushort)(message.Data >> 16)) == 0)
		{
			bend = 0;
			return false;
		}

		// the two data bytes are 7 bits only, so we need to shift them around
		var msb = (byte)(message.Data >> 8 & 0x7f);
		var lsb = (byte)(message.Data & 0x7f);

		// bend is 14 bits, and goes from -8192 to 8191
		bend = (lsb | msb << 7) - 0x2000;
		return true;
	}

}
