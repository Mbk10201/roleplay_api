namespace Mbk.RoleplayAPI.UI.Shared.AlertSystem;

public partial class Alert
{
	public static IAlertPanel AlertPanel { get; set; }

	[RoleplayAPI.OnAfterRender]
	public static void Initialize()
	{
		if(RoleplayAPI.Debug())
			Log.Info( "Alert - Initialize" );

		Game.AssertClient();

		foreach ( var type in TypeLibrary.GetTypes().Where( x => x.IsClass ) )
		{
			int count = type.Interfaces.Count( x => x.Name == "IAlertPanel" );
			if ( count > 0 )
			{
				AlertPanel = (IAlertPanel)TypeLibrary.Create( type.TargetType.Name, type.TargetType );
				Game.RootPanel?.AddChild( (RoleplayPanel)AlertPanel );
				return;
			}
		}
	}

	[ConCmd.Client("rp_add_alert", CanBeCalledFromServer = true)]
	public static void Add( AlertBuilder item )
	{
		AlertPanel.Add( item );
		if( RoleplayAPI.Instance.Configuration.AlertSound != "")
			Sound.FromScreen( RoleplayAPI.Instance.Configuration.AlertSound );
	}

	[GameEvent.Client.Frame]
	public void Frame()
	{
		/*if ( Game.RootPanel.Children.OfType<NotificationList>().Count() == 0 )
			Game.RootPanel.AddChild<NotificationList>();*/

		if ( !Game.RootPanel.Children.Contains( (RoleplayPanel)AlertPanel ) )
			Game.RootPanel.AddChild( (RoleplayPanel)AlertPanel );
	}

	/*[GameEvent.Client.BuildInput] 
	public static void BuildInput()
	{
		if ( Input.Pressed( "Inventory" ) )
		{
			Add( new()
			{
				Title = "Info",
				Message = "Hello this is a info alert test",
				Type = Models.Enums.eAlertType.Info
			} );

			Add( new()
			{
				Title = "Success",
				Message = "Hello this is a success alert test",
				Type = Models.Enums.eAlertType.Success
			} );

			Add( new()
			{
				Title = "Error",
				Message = "Hello this is a error alert test",
				Type = Models.Enums.eAlertType.Error
			} );

			Add( new()
			{
				Title = "Warning",
				Message = "Hello this is a warning alert test",
				Type = Models.Enums.eAlertType.Warning
			} );

			Add( new()
			{
				Title = "Custom",
				Message = "Hello this is a custom alert test",
				Type = Models.Enums.eAlertType.Custom
			} );
		}
	}*/
}
