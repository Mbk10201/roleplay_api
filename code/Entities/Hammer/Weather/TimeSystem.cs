namespace Mbk.RoleplayAPI.Entities.Hammer;

public partial class TimeSystem : Entity
{
	public delegate void SectionChanged( eTimeSection section );
	public static event SectionChanged OnSectionChanged;

	public static TimeSystem Instance { get; private set; }
	public static eTimeSection Section => Instance?.InternalSection ?? eTimeSection.Day;
	public static float TimeOfDay => Instance?.InternalTimeOfDay ?? 12f;

	public static float Temperature
	{
		get => Instance?.InternalTemperature ?? 10f;
		set
		{
			if ( Instance != null )
				Instance.InternalTemperature = value;
		}
	}

	[Net, Change( nameof( OnInternalSectionChanged ) )]
	private eTimeSection InternalSection { get; set; }

	[Net] private float InternalTimeOfDay { get; set; } = 12f;
	[Net] private float InternalTemperature { get; set; } = 10f;

	[ConVar.Server( "time.speed" )]
	public static float Speed { get; set; } = 0.05f;

	public static bool IsTimeBetween( float min, float max )
	{
		if ( min == 0f && max == 0f )
			return true;

		var time = TimeOfDay;

		if ( min <= max )
		{
			if ( time >= min && time <= max )
				return true;
		}
		else
		{
			if ( time >= min || time <= max )
				return true;
		}

		return false;
	}

	public static eTimeSection ToSection( float time )
	{
		if ( time > 5f && time <= 9f )
			return eTimeSection.Dawn;

		if ( time > 9f && time <= 18f )
			return eTimeSection.Day;

		if ( time > 18f && time <= 21f )
			return eTimeSection.Dusk;

		return eTimeSection.Night;
	}

	[GameEvent.Entity.PostSpawn]
	private static void Initialize()
	{
		Game.AssertServer();
		Instance = new TimeSystem();
	}

	public override void ClientSpawn()
	{
		Instance = this;
		base.ClientSpawn();
	}

	public override void Spawn()
	{
		Transmit = TransmitType.Always;
		base.Spawn();
	}

	[GameEvent.Tick.Server]
	private void Tick()
	{
		InternalTimeOfDay += Speed * Sandbox.Time.Delta;

		if ( InternalTimeOfDay >= 24f )
		{
			InternalTimeOfDay = 0f;
		}

		var currentSection = ToSection( TimeOfDay );

		if ( currentSection != Section )
		{
			InternalSection = currentSection;
			OnSectionChanged?.Invoke( currentSection );
		}
	}

	private void OnInternalSectionChanged( eTimeSection section )
	{
		OnSectionChanged?.Invoke( section );
	}
}
