using Editor;
using Mbk.RoleplayAPI.Player;

namespace Mbk.RoleplayAPI.Entities.Hammer;

[Library( "rp_zone" )]
[Display( Name = "rp_zone" ), Category( "Triggers" ), Icon( "activity_zone" )]
[HammerEntity]
public partial class Zone : TriggerMultiple
{
	[Property( Title = "Area Name" )]
	public string AreaName { get; set; }

	[Property( Title = "Zone JobID" )]
	public int Zone_Job { get; set; }

	[Property( Title = "Zone Type" )]
	public ZoneType Zone_Type { get; set; }

	[Property( Title = "Zone Identifier" )]
	public int Zone_ID { get; set; }

	[Property( Title = "Zone (Damage/Health/Armory) gived by type" )]
	public float Amount { get; set; }

	private TimeSince LastTrigger;

	public string AreaNameTranslated { get; set; }

	public enum ZoneType
	{
		Normal = 0,
		Fire,
		Kill,
		Heal,
		Armory
	}

	public Zone()
	{
		ActivationTags.Add( "rp_zones" );
	}

	public override void Spawn()
	{
		base.Spawn();

		// Have to finish translation API
		AreaNameTranslated = $"#{AreaName}";
	}

	public override void Touch( Entity other )
	{
		base.Touch( other );

		switch ( Zone_Type )
		{
			case ZoneType.Normal:
				{
					if ( other is RoleplayPlayer player )
					{
						player.CurrentZone = AreaNameTranslated;
					}
					else if ( other is RoleplayEntity entity )
						entity.CurrentZone = AreaNameTranslated;

					break;
				}

			case ZoneType.Fire:
				{
					if ( LastTrigger > +5f )
					{
						LastTrigger = 0;
						var damage = new DamageInfo
						{
							Attacker = this,
							Damage = Amount,
						};

						other.TakeDamage( damage );
					}

					break;
				}

			case ZoneType.Kill:
				{
					var damage = new DamageInfo
					{
						Attacker = this,
						Damage = (float)other.Health,
					};

					other.TakeDamage( damage );

					break;
				}

			case ZoneType.Heal:
				{
					if ( LastTrigger > +5f )
					{
						LastTrigger = 0;

						if ( other.Health + (int)Amount >= 100 )
							other.Health = 100;
						else
							other.Health = other.Health + (int)Amount;
					}

					break;
				}

			case ZoneType.Armory:
				{
					if ( LastTrigger > +5f )
					{
						LastTrigger = 0;

						if ( other is RoleplayPlayer player )
						{
							if ( player.Armor + (int)Amount >= player.MaxArmor )
								player.Armor = player.MaxArmor;
							else
								player.Armor = player.Armor + (int)Amount;
						}
					}

					break;
				}
		}
	}

	public override void EndTouch( Entity other )
	{
		base.EndTouch( other );

		switch ( Zone_Type )
		{
			case ZoneType.Normal:
				{
					if ( other is RoleplayPlayer player )
						player.CurrentZone = "#Zone.Default";
					else if ( other is RoleplayEntity entity )
						entity.CurrentZone = "#Zone.Default";
					break;
				}
		}
	}
}
