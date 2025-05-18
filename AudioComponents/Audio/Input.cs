using System.Numerics;

namespace AudioComponents.Audio;

/// <summary>
/// An Input is pretty much just a named reference to an Output
/// </summary>
/// <remarks>
/// An Input is either connected to an output or unconnected. If it is connected, it will echo the value from the output.
/// If it is not connected, it will return a default value of zero. Values are Vector2s, dual 32bit floats.
/// </remarks>
public class Input : IInput, IInputReader
{
	public string Name { get; init; }
	public Output SourceOutput { get; set; }

	public Input(string name)
	{
		Name = name;
	}

	public void ConnectTo(IOutput output)
	{
		if (!(output is Output typedOutput)) throw new ArgumentException($"Output {output.Name} is not a valid output.");
		SourceOutput = typedOutput;
	}

	public Vector2 Value
	{
		// value is the value of the output if connected, or zero if not connected
		get => SourceOutput?.Value ?? Vector2.Zero;
	}
}
