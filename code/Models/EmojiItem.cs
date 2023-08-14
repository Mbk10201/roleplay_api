namespace Mbk.RoleplayAPI.Models;

/// <summary>
/// Represents a emoji
/// </summary>
public class EmojiItem
{
	public string Emoji { get; set; }
	public string Category { get; set; }
	public List<string> Tags { get; set; }
}
