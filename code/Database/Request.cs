using System.Net.Http;

namespace Mbk.RoleplayAPI.Database;

public static class Request
{
	static string ApiURL => RoleplayAPI.Instance.Configuration.ApiUrl;
	
	public static Dictionary<string, string> GetHeaders() => new()
	{
		{ "Content-Type", "application/json" },
		//{ "Authorization", $"Bearer {Instance.Token}" }
	};

	public static Task<T> Get<T>( string destination )
	{
		var url = $"{ApiURL}/{destination}";

		Log.Info( url );

		return Http.RequestJsonAsync<T>( url, "GET", headers: GetHeaders() );
	}

	public static Task<T> GetFromMarketplace<T>( string destination )
	{
		Log.Info( destination );

		return Http.RequestJsonAsync<T>( destination, "GET", headers: GetHeaders() );
	}

	public static Task<HttpResponseMessage> Post<T>( string destination, T payload, Dictionary<string, string> Headers )
	{
		var url = $"{ApiURL}/{destination}";
		var content = Http.CreateJsonContent( payload );
		return Http.RequestAsync( "POST", url, content, Headers );
	}

	public static Task<HttpResponseMessage> Post<T>( string destination, T payload )
	{
		var url = $"{ApiURL}/{destination}";
		
		Log.Info( url );

		var content = Http.CreateJsonContent( payload );
		return Http.RequestAsync( "POST", url, content, GetHeaders() );
	}

	public static Task<HttpResponseMessage> Post<T>( string destination )
	{
		var url = $"{ApiURL}/{destination}";

		return Http.RequestAsync( "POST", url, headers: GetHeaders() );
	}
}
