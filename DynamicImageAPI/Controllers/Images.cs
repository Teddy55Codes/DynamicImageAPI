using DynamicImageAPI.Models;
using SkiaSharp;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace DynamicImageAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class Images : ControllerBase
{
    [HttpGet("Counter")]
    public IActionResult Get(string counterId = "0", int fontSize = 12, string textColorHEX = "000000")
    {
        fontSize = fontSize > 100 ? 100 : fontSize;
        long counter = DataManager.GetNewCounterValue(counterId);

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
        fontSize = fontSize > 100 ? 100 : fontSize;
        string cityLocation = LocationManager.GetClientLocation(Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty);
        
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
}