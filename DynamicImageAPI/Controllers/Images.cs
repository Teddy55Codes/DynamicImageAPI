using SkiaSharp;
using Microsoft.AspNetCore.Mvc;

namespace DynamicImageAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class Images : ControllerBase
{
    [HttpGet("Counter")]
    public IActionResult Get(string counterId = "0", int fontSize = 12, string color = "#000000") => 
        GetImageFileWithText(DataManager.GetNewCounterValue(counterId).ToString(), fontSize, color); // File(skData.ToArray(), "image/png");

    [HttpGet("Location")]
    public IActionResult GetLocation(int fontSize = 12, string color = "#000000")
    {
        string cityLocation = LocationManager.GetClientLocation(Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty);
        return GetImageFileWithText(cityLocation, fontSize, color);
    }

    private FileContentResult GetImageFileWithText(string text, int fontSize, string color) 
    {
        fontSize = fontSize > 100 ? 100 : fontSize;
        
        var bounds = new SKRect();
        
        var textPaint = new SKPaint
        {
            TextSize = fontSize, 
            Color = SKColor.Parse(color)
        };
        textPaint.MeasureText(text, ref bounds);
        
        using var bitmap = new SKBitmap((int)bounds.Right+2, (int)bounds.Height);
        using var bitmapCanvas = new SKCanvas(bitmap);
        bitmapCanvas.Clear();
        bitmapCanvas.DrawText(text, 0, -bounds.Top, textPaint);
        
        var skData = bitmap.Encode(SKEncodedImageFormat.Png, 50);
        return File(skData.ToArray(), "image/png");
    }
}