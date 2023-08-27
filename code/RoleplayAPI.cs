using Mbk.RoleplayAPI.Inventory;
using Mbk.RoleplayAPI.UI.RootPanels;
using Mbk.RoleplayAPI.Setup;
using Mbk.RoleplayAPI.Player;
using Mbk.RoleplayAPI.Inventory.Items;
using Mbk.RoleplayAPI.Jobs;
//using Editor.NodeEditor;

namespace Mbk.RoleplayAPI;

[Display( Name = "Roleplay API" ), Category( "MBK" ), Icon( "webhook" )]
public partial class RoleplayAPI : Entity
{
	public static RoleplayAPI Instance { get; private set; }

	[Net]
	public Configuration Configuration { get; set; }

	public List<EmojiItem> ListEmoji = new();

	public RoleplayAPI()
	{
		Instance = this;
		Configuration = new Configuration();

		bool Exists = FileSystem.Data.FileExists( "configuration.json" );

		if ( Game.IsServer )
		{
			Event.Run( OnServerInit );

			_ = new JobSystem();
			InventorySystem.Initialize();
			Database.Database.Initialize();

			ItemTag.Register( "deployable", "Deployable", ItemColors.Deployable );
			ItemTag.Register( "consumable", "Consumable", ItemColors.Consumable );
			ItemTag.Register( "tool", "Tool", ItemColors.Tool );

			if ( Exists )
			{
				Configuration = FileSystem.Data.ReadJson<Configuration>( "configuration.json" );
			}
		}

		if ( Game.IsClient )
		{
			Event.Run( OnClientInit );

			/*if ( !Exists )
			{
				// Show the setup installation
				_ = new SetupUI();
			}
			else
			{
				Game.RootPanel?.Delete();
				_ = new MainHud();
				Event.Run( OnAfterRender );
			}*/

			Game.RootPanel?.Delete();
			_ = new MainHud();
			Event.Run( OnAfterRender );
		}

		ListEmoji = FileSystem.Mounted.ReadJson<List<EmojiItem>>( "data/configs/EmojiList.json" );
	}

	public static bool Debug() => Instance.Configuration.DebugMode;

	public override void Spawn()
	{
		Transmit = TransmitType.Always;
		base.Spawn();
	}

	[GameEvent.Entity.PostSpawn]
	public static void Initialize()
	{
		Game.AssertServer();
		_ = new RoleplayAPI();
	}

	[ConCmd.Server]
	public static void SetToken( string value )
	{
		Instance.Configuration.Token = value;
	}

	[ConCmd.Server]
	public static void SetAPI( string value )
	{
		Instance.Configuration.ApiUrl = value;
	}

	[ConCmd.Server]
	public static void SaveConfiguration()
	{
		FileSystem.Data.WriteJson( "configuration.json", Instance.Configuration);
	}
}
