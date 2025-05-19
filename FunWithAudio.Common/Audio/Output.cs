namespace FunWithAudio.Common.Audio;

/// <summary>
/// An output is purely a name, and a Vector2 twin 32bit float value.
/// </summary>
/// <remarks>
/// Outputs are referenced by inputs, meaning multiple inputs can read the same output value.
/// They have no idea of what is connected to them, Inputs hold this information.
/// </remarks><
public class Output : IOutput, IOutputWriter
{
	private readonly string _name;
	
	internal Output(string name)
	{
		_name = name;	
	}

	public string Name => _name;

	// abstraction not quite right, only the owning component should be able to set the value
	public Vector2 Value { get; set; }
}
