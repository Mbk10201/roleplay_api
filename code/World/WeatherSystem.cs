namespace Mbk.RoleplayAPI.World;

[Library]
[Display( Name = "Weather system" ), Category( "Roleplay" ), Icon( "sunny_snowing" )]
public partial class WeatherSystem : Entity
{
	public const int OneDayInSecond = 1440;
	public static WeatherSystem Instance { get; private set; }

	/// <summary>
	/// The year numbers
	/// </summary>
	[Net, Change]
	public ushort Year { get; set; } = 2023;

	/// <summary>
	/// The month number
	/// </summary>
	[Net, Change]
	public ushort Month { get; set; } = 1;

	/// <summary>
	/// The day number
	/// </summary>
	[Net, Change]
	public ushort Day { get; set; } = 1;

	/// <summary>
	/// The first digits of the hours
	/// </summary>
	[Net]
	public ushort Hour1 { get; set; }

	/// <summary>
	/// The second digits of the hours
	/// </summary>
	[Net, Change]
	public ushort Hour2 { get; set; }

	/// <summary>
	/// The first digits of the minutes
	/// </summary>
	[Net]
	public ushort Minute1 { get; set; }

	/// <summary>
	/// The second digits of the minutes
	/// </summary>
	[Net]
	public ushort Minute2 { get; set; }

	/// <summary>
	/// The first digits of the seconds
	/// </summary>
	[Net]
	public ushort Seconds { get; set; }

	public static WeatherType weatherType { get; set; }
	private static TimeSince refreshRate { get; set; }

	private const string OnNewYear = "onnewyear";
	private const string OnNewMonth = "onnewmonth";
	private const string OnNewDay = "onnewday";
	private const string OnNewHour = "onnewhour";

	[MethodArguments( new Type[] { typeof( ushort ) } )]
	public class OnNewYearAttribute : EventAttribute
	{
		public OnNewYearAttribute() : base( OnNewYear ) { }
	}

	[MethodArguments( new Type[] { typeof( ushort ) } )]
	public class OnNewMonthAttribute : EventAttribute
	{
		public OnNewMonthAttribute() : base( OnNewMonth ) { }
	}

	[MethodArguments( new Type[] { typeof( ushort ) } )]
	public class OnNewDayAttribute : EventAttribute
	{
		public OnNewDayAttribute() : base( OnNewDay ) { }
	}

	[MethodArguments( new Type[] { typeof( ushort ) } )]
	public class OnNewHourAttribute : EventAttribute
	{
		public OnNewHourAttribute() : base( OnNewHour ) { }
	}

	public enum WeatherType
	{
		SUNNY,
		CLOUDY,
		RAINY,
		WINDY,
		SNOWY
	};

	public enum DayType
	{
		MONDAY,
		TUESDAY,
		WEDNESDAY,
		THURSDAY,
		FRIDAY,
		SATURDAY,
		SUNDAY
	}

	[GameEvent.Entity.PostSpawn]
	public static void OnPostSpawn()
	{
		Game.AssertServer();
		_ = new WeatherSystem();
	}

	public WeatherSystem()
	{
		Instance = this;
		Transmit = TransmitType.Always;
		weatherType = WeatherType.SUNNY;
	}

	public void OnYearChanged( ushort oldValue, ushort newValue )
	{
		Event.Run( OnNewYear, oldValue, newValue );
	}

	public void OnMonthChanged( ushort oldValue, ushort newValue )
	{
		Event.Run( OnNewMonth, oldValue, newValue );
	}

	public void OnDayChanged( ushort oldValue, ushort newValue )
	{
		Event.Run( OnNewDay, oldValue, newValue );
	}

	public void OnHour2Changed( ushort oldValue, ushort newValue )
	{
		Event.Run( OnNewHour, oldValue, newValue );
	}

	public static string GetTime()
	{
		return $"{Instance.Hour1}{Instance.Hour2}:{Instance.Minute1}{Instance.Minute2}";
	}

	public static string GetDate()
	{
		return $"{Instance.Day} {Instance.GetMonthName()} {Instance.Year}";
	}

	public string GetTimeFormatted()
	{
		string monthname = GetMonthName();
		string dayname = GetDayName();

		return $"{dayname}, {Day} {monthname}";
	}

	public string GetMonthName()
	{
		switch ( (eMonth)Month )
		{
			case eMonth.JANUARY: return "Janvier";
			case eMonth.FEBRUARY: return "Février";
			case eMonth.MARCH: return "Mars";
			case eMonth.APRIL: return "Avril";
			case eMonth.MAY: return "May";
			case eMonth.JUNE: return "Juin";
			case eMonth.JULY: return "Juillet";
			case eMonth.AUGUST: return "Août";
			case eMonth.SEPTEMBER: return "Septembre";
			case eMonth.OCTOBER: return "Octobre";
			case eMonth.NOVEMBER: return "Novembre";
			case eMonth.DECEMBER: return "Décembre";
		}

		return "N/A";
	}

	public string GetDayName()
	{
		DayType dayType = DayType.MONDAY;

		switch ( (eMonth)Month )
		{
			case eMonth.JANUARY:
			{
				switch ( Day )
				{
					case 3:
					case 10:
					case 17:
					case 24:
					case 31:
						dayType = DayType.MONDAY;
						break;

					case 4:
					case 11:
					case 18:
					case 25:
						dayType = DayType.TUESDAY;
						break;

					case 5:
					case 12:
					case 19:
					case 26:
						dayType = DayType.WEDNESDAY;
						break;

					case 6:
					case 13:
					case 20:
					case 27:
						dayType = DayType.THURSDAY;
						break;

					case 7:
					case 14:
					case 21:
					case 28:
						dayType = DayType.FRIDAY;
						break;

					case 8:
					case 15:
					case 22:
					case 29:
						dayType = DayType.SATURDAY;
						break;

					case 9:
					case 16:
					case 23:
					case 30:
						dayType = DayType.SUNDAY;
						break;
				}
				break;
			}

			case eMonth.FEBRUARY:
			{
				switch ( Day )
				{
					case 7:
					case 14:
					case 21:
					case 28:
						dayType = DayType.MONDAY;
						break;
					case 1:
					case 8:
					case 15:
					case 22:
						dayType = DayType.TUESDAY;
						break;
					case 2:
					case 9:
					case 16:
					case 23:
						dayType = DayType.WEDNESDAY;
						break;
					case 3:
					case 10:
					case 17:
					case 24:
						dayType = DayType.THURSDAY;
						break;

					case 4:
					case 11:
					case 18:
					case 25:
						dayType = DayType.FRIDAY;
						break;

					case 5:
					case 12:
					case 19:
					case 26:
						dayType = DayType.SATURDAY;
						break;

					case 6:
					case 13:
					case 20:
					case 27:
						dayType = DayType.SUNDAY;
						break;

				}

				break;
			}
		}


		switch ( dayType )
		{
			case DayType.MONDAY: return "Lundi";
			case DayType.TUESDAY: return "Mardi";
			case DayType.WEDNESDAY: return "Mercredi";
			case DayType.THURSDAY: return "Jeudi";
			case DayType.FRIDAY: return "Vendredi";
			case DayType.SATURDAY: return "Samedi";
			case DayType.SUNDAY: return "Dimanche";
		}

		return "N/A";

	}

	[GameEvent.Tick.Server]
	public void Update()
	{
		if ( refreshRate >= 0.052 )
		{
			Seconds++;

			if(Seconds > 59)
			{
				Seconds = 0;
				Minute2++;
			}

			if ( Minute2 > 9 )
			{
				Minute2 = 0;
				Minute1++;

				if ( Minute1 > 5 && Minute2 >= 0 )
				{
					Minute1 = 0;
					Hour2++;
				}
			}

			if ( Hour2 > 9 )
			{
				Hour2 = 0;
				Hour1 += 1;
			}

			if ( Hour1 >= 2 && Hour2 >= 4)
			{
				Hour1 = 0;
				Hour2 = 0;
				Day++;
			}

			if ( Month == 4 || Month == 6 || Month == 9 || Month == 11 )
			{
				if ( Day >= 30 )
				{
					Month += 1;
					Day = 1;
				}
			}
			else
			{
				if ( Day >= 31 )
				{
					Month += 1;
					Day = 1;
				}
			}

			if ( Month >= 12 )
			{
				Month = 1;
				Year += 1;
			}

			refreshRate = 0;
		}
	}

	public static int GetSecondsFromDays(int days = 1) => OneDayInSecond * days;
}
