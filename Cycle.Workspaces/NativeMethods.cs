using System.Runtime.InteropServices;

namespace Cycle.Workspaces;
public static class NativeMethods
{
	[DllImport("user32.dll")]
	public static extern int SendMessage(nint hWnd, int msg, int wParam, int lParam);

	[DllImport("user32.dll")]
	public static extern nint SetCapture(nint hWnd);

	[DllImport("user32.dll")]
	public static extern bool ReleaseCapture();

	[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
	public static extern nint CreateRoundRectRgn(int left, int top, int right, int bottom, int cornerWidth, int cornerHeight);

	[DllImport("dwmapi.dll")]
	public static extern int DwmExtendFrameIntoClientArea(nint hWnd, ref MARGINS pMarInset);

	[DllImport("dwmapi.dll")]
	public static extern int DwmSetWindowAttribute(nint hwnd, int attr, ref int attrValue, int attrSize);

	public struct MARGINS                           // struct for box shadow
	{
		public int leftWidth;
		public int rightWidth;
		public int topHeight;
		public int bottomHeight;
	}

	public const int WM_NCHITTEST = 0x84;          // variables for dragging the form
	public const int HTCLIENT = 0x1;
	public const int HTCAPTION = 0x2;
	public const int WM_NCPAINT = 0x0085;
	public const int WM_NCLBUTTONDOWN = 0xA1;
	public const int HT_CAPTION = 0x2;

	public const uint WM_NCCALCSIZE = 0x83;
	//public const uint WM_NCHITTEST = 0x84;

	//RECT Structure
	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int left, top, right, bottom;
	}

	//WINDOWPOS Structure
	[StructLayout(LayoutKind.Sequential)]
	public struct WINDOWPOS
	{
		public IntPtr hwnd;
		public IntPtr hwndinsertafter;
		public int x, y, cx, cy;
		public int flags;
	}

	//NCCALCSIZE_PARAMS Structure
	[StructLayout(LayoutKind.Sequential)]
	public struct NCCALCSIZE_PARAMS
	{
		public RECT rgrc0, rgrc1, rgrc2;
		public WINDOWPOS lppos;
	}
}
