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
	private static BaseFileSystem fs => FileSystem.Data;
	const string CONFFILE = "roleplay_cfg.json";

	[Net]
	public Configuration Configuration { get; set; }

	public List<EmojiItem> ListEmoji = new();

	public RoleplayAPI()
	{
		Instance = this;

		if ( Game.IsServer )
		{
			Event.Run( OnServerInit );

			Configuration = new()
			{
				DebugMode = true
			};

			if ( fs.FileExists(CONFFILE) )
			{
				Configuration = fs.ReadJsonOrDefault<Configuration>( CONFFILE, new Configuration() );
			}
			else
				fs.WriteJson( CONFFILE, Configuration );

			_ = new JobSystem();
			Database.Database.Initialize();
			InventorySystem.Initialize();

			ItemTag.Register( "deployable", "Deployable", ItemColors.Deployable );
			ItemTag.Register( "consumable", "Consumable", ItemColors.Consumable );
			ItemTag.Register( "tool", "Tool", ItemColors.Tool );

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
			_ = new WelcomeHud();
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
		fs.WriteJson( CONFFILE, Instance.Configuration);
	}

	[ConCmd.Server]
	public static void InitializeRespawn()
	{
		var pawn = ConsoleSystem.Caller.Pawn as RoleplayPlayer;
		pawn.TimeUntilRespawn = Instance.Configuration.RespawnTime;

		RoleplayPlayer.InitializeRespawn( To.Single( ConsoleSystem.Caller ) );
	}
}
