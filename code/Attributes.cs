namespace Mbk.RoleplayAPI;

[AttributeUsage( AttributeTargets.Class )]
public class HudElementAttribute : LibraryAttribute, ITypeAttribute
{
	public Type TargetType { get; set; }
}
