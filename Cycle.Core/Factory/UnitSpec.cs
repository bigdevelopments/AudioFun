using System.Text.Json.Serialization;

namespace Cycle.Core.Factory;

public class UnitSpec
{
	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("elements")]
	public Dictionary<string, string> Elements { get; set; }

	[JsonPropertyName("connections")]
	public Dictionary<string, string> Connections { get; set; }

	[JsonPropertyName("expose")]
	public Dictionary<string, string> Exposures { get; set; }

	public List<string> Validate()
	{
		List<string> errors = new List<string>();

		foreach (var element in Elements)
		{
			var error = Validation.ValidateIdentifier(element.Key);
			if (error != null) errors.Add(error);
		}

		foreach (var connection in Connections)
		{
			var inputData = connection.Key.Split(':');
			var outputData = connection.Value.Split(':');

			if (inputData.Length != 2 || outputData.Length != 2)
			{
				errors.Add($"Connection '{connection.Key}' is not in the correct format. Expected 'element:connection'.");
				continue;
			}

			var inputElement = inputData[0].Trim();
			var inputConnection = inputData[1].Trim();
			var outputElement = outputData[0].Trim();
			var outputConnection = outputData[1].Trim();

			if (!Elements.ContainsKey(inputElement))
			{
				errors.Add($"Connection '{connection.Key} => {connection.Value}' references non-existent element: '{inputElement}'");
			}
			if (!Elements.ContainsKey(outputElement))
			{
				errors.Add($"Connection '{connection.Key} => {connection.Value}' references non-existent element: '{outputElement}'");
			}
		}


		return errors;
	}
}
