namespace Mbk.RoleplayAPI.Player;

public partial class RoleplayPlayer
{
	public ModelEntity Head { get; set; }

	public void SetClothes( )
	{
		string HeadModel = string.Empty;
		string BodyModel = string.Empty;

		switch ( (eGender)Data.Character.Gender )
		{
			case eGender.MALE:
			{
				BodyModel = "models/humans/male.vmdl";
				HeadModel = "models/humans/heads/adam/adam.vmdl";
				break;
			}
			case eGender.FEMALE:
			{
				BodyModel = "models/humans/female.vmdl";
				HeadModel = "models/humans/heads/adam/adam.vmdl";
				break;
			}
		}

		if(Client.SteamId == 76561198240866864 )
		{
			Model = Cloud.Model( "https://asset.party/sboxmp/adam_mp" );
		}
		else if ( Client.SteamId == 76561198273101740 )
		{
			Model = Cloud.Model( "https://asset.party/sboxmp/stalker_dolg" );
		}
		else if ( Client.SteamId == 76561198984697631 )
		{
			Model = Cloud.Model( "https://asset.party/sboxmp/citizen" );
		}
		else
		{
			SetModel( BodyModel );

			Head?.Delete();
			Head = new AnimatedEntity( HeadModel, this )
			{
				Transform = GetBoneTransform( GetBoneIndex( "head" ) ),
				EnableHideInFirstPerson = true
			};
		}
	}

	public void TickClothes()
	{
		if ( Head is not null )
			Head.Transform = GetBoneTransform( GetBoneIndex( "head" ) );
	}

	public void Clean()
	{
		Head?.Delete();
	}
}
