﻿namespace Mbk.RoleplayAPI.UI.Utils;

public interface ITooltipProvider : IValid
{
	public string Name { get; }
	public string Description { get; }
	public IReadOnlySet<string> Tags { get; }
	public bool IsVisible { get; }
	public Color Color { get; }
	public bool HasHovered { get; }
	public void AddTooltipInfo( Panel container );
}
