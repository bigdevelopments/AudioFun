namespace Cycle.Core.Core;

/// <summary>
/// A SignalInput is pretty much just a named reference to a SignalOutput
/// </summary>
/// <remarks>
/// A SignalInput is either connected to an SignalOutput or unconnected. If it is connected, it will echo the value from the output.
/// If it is not connected, it will return a default value of zero. Values are Vector2s, dual 32bit floats.
/// </remarks>
public class SignalInput : Connection
{
	private SignalOutput _connectedTo;

	public void ConnectTo(Connection connection)
	{
		if (!(connection is SignalOutput typedOutput)) throw new ArgumentException("Supplied connection is not a valid SignalOutput.");
		_connectedTo = typedOutput;
	}

	public void Disconnect()
	{
		_connectedTo = null;
	}

	public Vector2 Value
	{
		// value is the value of the output if connected, or zero if not connected
		get => _connectedTo?.Value ?? Vector2.Zero;
	}
}
