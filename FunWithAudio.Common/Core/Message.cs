namespace FunWithAudio.Common.Core;

public struct Message
{
	public readonly uint Data;

	public Message(uint data)
	{
		Data = data;
	}

	public Message(int data)
	{
		Data = (uint)data;
	}

	public override string ToString()
	{
		// check for midi
		if ((Data & 0x01000000) == 0x01000000)
		{
			switch (Data & 0xf00000)
			{
				case 0x800000:
					return $"NoteOff {Data >> 16 & 0xf}:{Maths.GetNoteName(Data >> 8 & 0xff)}:{Data & 0xff}";

				case 0x900000:
					return $"NoteOn  {Data >> 16 & 0xf}:{Maths.GetNoteName(Data >> 8 & 0xff)}:{Data & 0xff}";
			}

			return "Midi";
		}

		return "unknown";
	}


	public static readonly Message Ok = new Message(0x00000000);
	public static readonly Message Error = new Message(0x00000001);
	public static readonly Message Unsupported = new Message(0x00000002);
	public static readonly Message Ping = new Message(0x00000003);
}

public static class MessageMasks
{
	public const uint Ping = 0x00000003;
	public const uint Midi = 0x01000000;
	public const uint NoteOn = 0x01900000;
	public const uint NoteOff = 0x01800000;
}

public static class BasicMessages
{
	public static Message Ping() => new Message(0);

	public static bool IsPing(this Message message)
	{
		return message.Data == 0x0000000;
	}

	public static bool IsMidi(this Message message, out byte status, out byte data1, out byte data2)
	{
		if ((message.Data & 0xff000000) != 0x01000000)
		{
			status = 0;
			data1 = 0;
			data2 = 0;
			return false;
		}

		status = (byte)(message.Data >> 16 & 0xff);
		data1 = (byte)(message.Data >> 8 & 0xff);
		data2 = (byte)(message.Data & 0xff);
		return true;
	}
}

