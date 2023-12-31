﻿namespace Mbk.RoleplayAPI.UI.Shared.Carriable;

[StyleSheet]
public class CarriableIcon : Panel
{
	public Entity TargetEnt;
	public Label Label;
	public Label Number;

	public CarriableIcon( int i, Panel parent )
	{
		Parent = parent;
		Label = Add.Label( "empty", "item-name" );
		Number = Add.Label( $"{i}", "slot-number" );
	}

	public void Clear()
	{
		Label.Text = "";
		SetClass( "active", false );
	}
}
