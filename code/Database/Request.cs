using System.Net.Http;
using System.Text.Json;

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
		if( ApiURL == null || ApiURL == "")
		{
			Log.Error( "[RoleplayAPI] You haven't set the api url !" );
			return default;
		}
		
		var url = $"{ApiURL}/{destination}";

		return Http.RequestJsonAsync<T>( url, "GET", headers: GetHeaders() );
	}

	public static Task<T> GetFromMarketplace<T>( string destination )
	{
		Log.Info( destination );

		return Http.RequestJsonAsync<T>( destination, "GET", headers: GetHeaders() );
	}

	public static Task<HttpResponseMessage> Post<T>( string destination, T payload, Dictionary<string, string> Headers )
	{
		if ( ApiURL == null && ApiURL == "" )
		{
			Log.Error( "[RoleplayAPI] You haven't set the api url !" );
			return default;
		}

		var url = $"{ApiURL}/{destination}";
		var content = Http.CreateJsonContent( payload );
		return Http.RequestAsync( url, "POST", content, Headers );
	}

	public static Task<HttpResponseMessage> Post<T>( string destination, T payload )
	{
		if ( ApiURL == null && ApiURL == "" )
		{
			Log.Error( "[RoleplayAPI] You haven't set the api url !" );
			return default;
		}

		var url = $"{ApiURL}/{destination}";
		var content = Http.CreateJsonContent( payload );

		return Http.RequestAsync( url, "POST", content, GetHeaders() );
	}

	public static async Task<T> ReadFromJson<T>( HttpResponseMessage response )
	{
		var json = await response.Content.ReadAsStreamAsync();
		return JsonSerializer.Deserialize<T>( json );
	}
}
