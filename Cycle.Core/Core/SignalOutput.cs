namespace Cycle.Core.Core;

/// <summary>
/// A SignalOutput is purely a name, and a Vector2 twin 32bit float value.
/// </summary>
/// <remarks>
/// SignalOutputs are referenced by SignalInputs, meaning multiple inputs can read the same output value.
/// They have no idea of what is connected to them, Inputs hold this information.
/// </remarks>
public class SignalOutput : Connection
{
	public Vector2 Value { get; set; }
}
