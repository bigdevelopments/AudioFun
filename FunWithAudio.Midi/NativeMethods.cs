using System.Runtime.InteropServices;
using System.Text;

namespace MidiInterop;

public class NativeMethods
{
	[DllImport("winmm.dll")]
	public static extern int midiConnect(int handleA, int handleB, int reserved);

	[DllImport("winmm.dll", EntryPoint="midiInOpen")]
	public static extern int InOpen(out nint handle, int deviceID, MidiInProc proc, int instance, int flags);

	[DllImport("winmm.dll")]
	public static extern int midiDisconnect(int handleA, int handleB, int reserved);

	[DllImport("winmm.dll")]
	public static extern long mciSendString(string command, StringBuilder returnValue, int returnLength, nint winHandle);

	[DllImport("winmm.dll")]
	public static extern int midiInGetNumDevs();

	[DllImport("winmm.dll")]
	public static extern int midiOutGetNumDevs();

	[DllImport("winmm.dll")]
	public static extern int midiInGetDevCaps(int uDeviceID, ref MIDIINCAPS lpMidiInCaps, uint cbMidiInCaps);

	[DllImport("winmm.dll")]
	public static extern int midiOutGetDevCaps(int uDeviceID, ref MidiOutCaps lpMidiOutCaps, uint cbMidiOutCaps);

	//[DllImport("winmm.dll")]
	//public static extern int midiOutOpen(ref int handle, int deviceID, MidiInProc proc, int instance, int flags);

	[DllImport("winmm.dll")]
	public static extern int midiOutShortMsg(int handle, int message);

	[DllImport("winmm.dll")]
	public static extern int midiOutClose(int handle);

	[DllImport("winmm.dll")]
	public static extern int midiInClose(nint handle);

	[DllImport("winmm.dll")]
	public static extern int midiInStart(nint handle);

	[DllImport("winmm.dll")]
	public static extern int midiInStop(nint handle);

	[DllImport("winmm.dll")]
	public static extern int midiInReset(nint handle);

	[DllImport("winmm.dll")]
	public static extern int midiInPrepareHeader(nint handle,
		nint headerPtr, int sizeOfMidiHeader);

	[DllImport("winmm.dll")]
	public static extern int midiInUnprepareHeader(nint handle,
		nint headerPtr, int sizeOfMidiHeader);

	[DllImport("winmm.dll")]
	public static extern int midiInAddBuffer(nint handle, nint headerPtr, int sizeOfMidiHeader);

	public const int MIM_OPEN = 0x3C1;
	public const int MIM_CLOSE = 0x3C2;
	public const int MIM_DATA = 0x3C3;
	public const int MIM_LONGDATA = 0x3C4;
	public const int MIM_ERROR = 0x3C5;
	public const int MIM_LONGERROR = 0x3C6;
	public const int MIM_MOREDATA = 0x3CC;
	public const int MHDR_DONE = 0x00000001;
}

public enum InputMessages
{
	Open = 0x3c1,
	Close = 0x3c2,
	Data = 0x3c3,
	LongData = 0x3c4,
	Error = 0x3c5,
	LongError = 0x3c6,
	MoreData = 0x3cc
}

[StructLayout(LayoutKind.Sequential)]
public struct MidiOutCaps
{
	public ushort wMid;
	public ushort wPid;
	public ushort vDriverVersion;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string szPname;

	public ushort wTechnology;
	public ushort wVoices;
	public ushort wNotes;
	public ushort wChannelMask;
	public ushort dwSupport;
}

[StructLayout(LayoutKind.Sequential)]
public struct MidiInCaps
{
	public ushort wMid;
	public ushort wPid;
	public ushort vDriverVersion;

	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string szPname;
	public ushort dwSupport;
}

[StructLayout(LayoutKind.Sequential)]
public struct MIDIINCAPS
{
	public ushort wMid;
	public ushort wPid;
	public uint vDriverVersion;     // MMVERSION
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string szPname;
	public uint dwSupport;
}

public delegate void MidiInProc(int handle, InputMessages message, int instance, int param1, int param2);
