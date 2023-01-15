using System.Reflection;
using DynamicImageAPI.Models;
using Newtonsoft.Json.Linq;

namespace DynamicImageAPI;

public static class LocationManager
{
    private static string _locationFallback = "Earth";

    static LocationManager()
    {
        var thread = new Thread(ClearDataBase);
        thread.IsBackground = true;
        thread.Start();
    }
    
    public static string GetClientLocation(string ipAddress)
    {
        var locationDb = SQLiteDBAccess.SQLiteDBAccess.Instance("Images", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        
        if (!locationDb.CheckForExistingElementByAttribute("Locations", "Query", $"'{ipAddress}'"))
        {
            var ipLocation = GetCityFromIp(ipAddress);
            if (ipLocation.City.Equals(_locationFallback)) return _locationFallback;
            var isMobile = ipLocation.Mobile ? 1 : 0;
            var isProxy = ipLocation.Proxy ? 1 : 0;
            locationDb.Insert("Locations", 
                "Query, City, IsMobile, IsProxy", 
                $"'{ipLocation.Query}', '{ipLocation.City}', {isMobile}, {isProxy}");
            return ipLocation.City;
        }
        string clientCity = _locationFallback;
        var reader = locationDb.GetByAttribute("Locations", "Query", $"'{ipAddress}'");
        if (reader.Read())
        {
            clientCity = reader.GetString(1);
        }
        locationDb.CloseDBFile(reader);
        return clientCity;
    }
    
    private static IpLocation GetCityFromIp(string ipAddress)
    {
        HttpClient client = new HttpClient();
        
        client.DefaultRequestHeaders.Add("User-Agent", "DynamicImageAPI");
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        if (string.IsNullOrEmpty(ipAddress)) return new IpLocation() { City = _locationFallback};
        var response = client.GetAsync($"http://ip-api.com/json/{ipAddress}?fields=query,status,city,mobile,proxy").Result;
        if (!response.IsSuccessStatusCode) return new IpLocation() { City = _locationFallback};
        IpLocation? ipLocation = JObject.Parse(response.Content.ReadAsStringAsync().Result).ToObject<IpLocation>();
        if (ipLocation == null) return new IpLocation() { City = _locationFallback};
        return ipLocation;
    }

    private static void ClearDataBase()
    {
        Task.Delay(new TimeSpan(1, 0, 0, 0));
        var counterDb = SQLiteDBAccess.SQLiteDBAccess.Instance("Images", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        counterDb.CreateTable("Locations", "Query TEXT PRIMARY KEY, City TEXT, IsMobile INTEGER, IsProxy INTEGER");

    }
}