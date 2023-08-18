using Editor;
using Mbk.RoleplayAPI.Player;
using Mbk.RoleplayAPI.UI.Shared.AlertSystem;

namespace Mbk.RoleplayAPI.Entities;

public enum eMoneyType
{
	Stack,
	Roll,
	Five,
	Ten,
	Twenty,
	Fifty,
	Hundred,
	TwoHundred,
	FiveHundred
}

[Library( "rp_money" )]
public partial class MoneyEntity : AnimatedEntity, IUse
{
	[Net]
	public long Amount { get; set; }

	public override void Spawn()
	{
		Transmit = TransmitType.Always;
		EnableTouch = true;

		SetModel( "models/atm01/atm01.vmdl" );
		SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );

		base.Spawn();
	}

	public bool OnUse( Entity user )
	{
		var client = user.Client;

		if ( client == null )
			return false;

		if ( Owner != null && Owner != client)
		{
			Alert.Add( new()
			{
				Title = "Money",
				Message = "You cannot pickup this stack of money.",
				Type = eAlertType.Warning
			} );
			return false;
		}

		var pawn = client.Pawn as RoleplayPlayer;

		if ( pawn != null )
		{
			pawn.Data.Money += Amount;

			Alert.Add( new()
			{
				Title = "Money",
				Message = $"You have picked {Amount}€.",
				Type = eAlertType.Success
			} );

			Delete();
		}

		return false;
	}

	public bool IsUsable( Entity user )
	{
		return true;
	}

	public void SetType( eMoneyType type )
	{
		Game.AssertServer();

		string path = "models/money/";

		string finalPath = "";

		switch(type)
		{
			case eMoneyType.Stack:
			{
				finalPath = $"{path}stack.vmdl";
				break;
			}
			case eMoneyType.Roll:
			{
				finalPath = $"{path}roll.vmdl";
				break;
			}
			case eMoneyType.Five:
			{
				finalPath = $"{path}5_euro.vmdl";
				break;
			}
			case eMoneyType.Ten:
			{
				finalPath = $"{path}10_euro.vmdl";
				break;
			}
			case eMoneyType.Twenty:
			{
				finalPath = $"{path}20_euro.vmdl";
				break;
			}
			case eMoneyType.Fifty:
			{
				finalPath = $"{path}50_euro.vmdl";
				break;
			}
			case eMoneyType.Hundred:
			{
				finalPath = $"{path}100_euro.vmdl";
				break;
			}
			case eMoneyType.TwoHundred:
			{
				finalPath = $"{path}200_euro.vmdl";
				break;
			}
		}

		if ( finalPath != "" )
		{
			SetModel( finalPath );
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
		}
	}

	[ConCmd.Server]
	public static void CreateMoney( long amount, Transform transform = new Transform() )
	{
		var money = new MoneyEntity()
		{
			Amount = amount,
			Transform = (transform)
		};

		eMoneyType type = eMoneyType.Stack;

		if ( amount < 5 )
			type = eMoneyType.Stack;
		else if ( amount >= 5 && amount < 10 )
			type = eMoneyType.Five;
		else if ( amount >= 10 && amount < 20 )
			type = eMoneyType.Ten;
		else if ( amount >= 20 && amount < 50 )
			type = eMoneyType.Twenty;
		else if ( amount >= 50 && amount < 100 )
			type = eMoneyType.Fifty;
		else if ( amount >= 100 && amount < 200 )
			type = eMoneyType.Hundred;
		else if ( amount >= 200 )
			type = eMoneyType.TwoHundred;

		money.SetType( type );
	}
}
