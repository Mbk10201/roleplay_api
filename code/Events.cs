namespace Mbk.RoleplayAPI;

public partial class RoleplayAPI
{
	public const string OnServerInit = "onserverinit";
	public class OnServerInitttribute : EventAttribute
	{
		public OnServerInitttribute() : base( OnServerInit ) { }
	}

	public const string OnClientInit = "onclientinit";
	public class OnClientInitAttribute : EventAttribute
	{
		public OnClientInitAttribute() : base( OnClientInit ) { }
	}

	public const string OnClientJoined = "onclientjoined";
	[MethodArguments( new Type[] { typeof( IClient ) } )]
	public class OnClientJoinedAttribute : EventAttribute
	{
		public OnClientJoinedAttribute() : base( OnClientJoined ) { }
	}

	public const string OnClientDisconnect = "onclientdisconnect";
	[MethodArguments( new Type[] { typeof( IClient ) } )]
	public class OnClientDisconnectAttribute : EventAttribute
	{
		public OnClientDisconnectAttribute() : base( OnClientDisconnect ) { }
	}

	public const string OnAfterRender = "onafterrender";
	public class OnAfterRenderAttribute : EventAttribute
	{
		public OnAfterRenderAttribute() : base( OnAfterRender ) { }
	}

	public const string OnExecuteCommand = "onexecutecommand";
	public class OnExecuteCommandAttribute : EventAttribute
	{
		public OnExecuteCommandAttribute() : base( OnExecuteCommand ) { }
	}

	public const string OnNewChatMessage = "onnewchatmessage";
	public class OnNewChatMessageAttribute : EventAttribute
	{
		public OnNewChatMessageAttribute() : base( OnNewChatMessage ) { }
	}
}
