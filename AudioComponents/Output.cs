using System.Numerics;

namespace AudioComponents;

public class Output : IOutput, IOutputWriter
{
	private readonly string _name;
	
	internal Output(string name)
	{
		_name = name;	
	}

	public string Name => _name;

	public Vector2 Value { get; set; }
}
