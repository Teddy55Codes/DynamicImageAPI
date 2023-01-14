using DynamicImageAPI.Models;
using SkiaSharp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Newtonsoft.Json.Linq;

namespace DynamicImageAPI.Controllers;

[ApiController]
[EnableRateLimiting("fixed")]
[Route("[controller]")]
public class Images : ControllerBase
{
    private string _locationFallback = "Earth";
    
    [HttpGet("Counter")]
    public IActionResult Get(string counterId = "0", int fontSize = 12, string textColorHEX = "000000")
    {
        fontSize = fontSize > 100 ? 100 : fontSize;
        long counter = CounterManager.GetNewCounterValue(counterId);

        SKRect bounds = new SKRect();
        
        var textPaint = new SKPaint { TextSize = fontSize, Color = SKColor.Parse("#"+textColorHEX) };
        textPaint.MeasureText(counter.ToString(), ref bounds);
        
        var bitmap = new SKBitmap(
            (int)bounds.Right+2, 
            (int)bounds.Height);

        SKCanvas bitmapCanvas = new SKCanvas(bitmap);

        bitmapCanvas.Clear();
        bitmapCanvas.DrawText(counter.ToString(), 0, -bounds.Top, textPaint);

        var skData = bitmap.Encode(SKEncodedImageFormat.Png, 0);

        return File(skData.ToArray(), "image/png");
    }
    
    [HttpGet("Location")]
    public IActionResult GetLocation(int fontSize = 12, string textColorHEX = "000000")
    {
        string cityLocation = GetCityFromIp(Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty);
        
        SKRect bounds = new SKRect();
        
        var textPaint = new SKPaint { TextSize = fontSize, Color = SKColor.Parse("#"+textColorHEX) };
        textPaint.MeasureText(cityLocation, ref bounds);
        
        var bitmap = new SKBitmap(
            (int)bounds.Right+2, 
            (int)bounds.Height);

        SKCanvas bitmapCanvas = new SKCanvas(bitmap);

        bitmapCanvas.Clear();
        bitmapCanvas.DrawText(cityLocation, 0, -bounds.Top, textPaint);

        var skData = bitmap.Encode(SKEncodedImageFormat.Png, 0);

        return File(skData.ToArray(), "image/png");
    }

    private string GetCityFromIp(string ipAddress)
    {
        HttpClient client = new HttpClient();
        
        client.DefaultRequestHeaders.Add("User-Agent", "DynamicImageAPI");
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        if (string.IsNullOrEmpty(ipAddress)) return _locationFallback;
        var response = client.GetAsync($"http://ip-api.com/json/{ipAddress}").Result;
        if (!response.IsSuccessStatusCode) return _locationFallback;
        IpAPIRsoponse? ipApiRsoponse = JObject.Parse(response.Content.ReadAsStringAsync().Result).ToObject<IpAPIRsoponse>();
        if (ipApiRsoponse?.City == null) return _locationFallback;
        return ipApiRsoponse.City;
    }
}