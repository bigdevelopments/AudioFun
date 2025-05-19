namespace AudioComponents.Audio;

public class HybridComponent : Component
{
	private readonly List<Component> _components;

	public HybridComponent()
	{
		_components = new();
	}

	public T Add<T>(string name, T component) where T : Component
	{
		_components.Add(component);
		Surface.Add(Name + "." + name, component);
		return component;
	}

	public void ExposeInput(string componentName, string inputName, string externalName)
	{
		var component = Surface.GetComponent(Name + "." + componentName);
		if (component == null) throw new ArgumentException($"Component {componentName} not found.");
		var input = component.GetInput(inputName) as Input;
		if (input == null) throw new ArgumentException($"Input {inputName} not found in component {componentName}.");
		AddInput(externalName, input);
	}

	public void ExposeOutput(string component, string outputName, string externalName)
	{
		var comp = Surface.GetComponent(Name + "." + component);
		if (comp == null) throw new ArgumentException($"Component {component} not found.");
		var output = comp.GetOutput(outputName) as Output;
		if (output == null) throw new ArgumentException($"Output {outputName} not found in component {component}.");
		AddOutput(externalName, output);
	}

	public override void Tick()
	{
		// do nothing
	}
}
