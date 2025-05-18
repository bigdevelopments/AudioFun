namespace AudioComponents;

public struct Message
{
	public uint Data;
}

public static class BasicMessages
{
	public static Message Ping() => new Message { Data = 0x0000000 };

	public static Message Midi(byte status, byte data1, byte data2)
		=> new Message { Data = (uint)1<< 24 | (uint)((status << 16) | (data1 << 8) | data2) };

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

		status = (byte)((message.Data >> 16) & 0xff);
		data1 = (byte)((message.Data >> 8) & 0xff);
		data2 = (byte)(message.Data & 0xff);
		return true;
	}
}

