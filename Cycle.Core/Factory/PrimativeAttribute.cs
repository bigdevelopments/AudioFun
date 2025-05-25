namespace Cycle.Core.Factory;

[AttributeUsage(AttributeTargets.Class)]
public class PrimitiveAttribute : Attribute
{
	public string Name { get; }
	public string Description { get; }

	public PrimitiveAttribute(string name, string description = null)
	{
		var error = Validation.ValidateIdentifier(name);
		if (error != null) Console.WriteLine($"Invalid primitive name defined in code: [{name}]");

		Name = name;
		Description = description ?? string.Empty;
	}
}
