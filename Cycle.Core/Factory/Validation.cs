namespace Cycle.Core.Factory;

public static class Validation
{
	public static string ValidateIdentifier(string identifier)
	{
		if (string.IsNullOrWhiteSpace(identifier)) return "Identifier not supplied or empty";
		
		if (identifier.Length > 32)	return "Identifier cannot exceed 32 characters";
		if (identifier.Any(c => !char.IsAsciiLetterLower(c) && !char.IsDigit(c) && c != '_'))
		{
			return "Identifier can only contain lower case letters, digits, and underscores";
		}
		return null;
	}
}
