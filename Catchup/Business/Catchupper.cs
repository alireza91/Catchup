using Catchup.Contracts;
using Catchup.Models;
using SkiaSharp;
using System.Text;

namespace Catchup.Business;

public class Catchupper : ICatchup
{
	#region [Field(s)]

	private readonly Random _random = new();
	private const string _captchaChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

	#endregion

	#region [Public method(s)]

	/// <summary>
	/// Generates a captcha image along with its corresponding text.
	/// </summary>
	/// <param name="options">
	/// Optional configuration for captcha generation (length, colors, noise, etc.). 
	/// If null, default options will be used.
	/// </param>
	/// <returns>
	/// A <see cref="ByteArrayResultModel"/> containing the generated captcha image (as a byte array) 
	/// and the captcha text.
	/// </returns>
	public ByteArrayResultModel GetAnImageCaptcha(CaptchaOptions? options = null)
	{
		var effectiveOptions = options ?? new CaptchaOptions();

		string captcha = GenerateAlphanumericCaptcha(effectiveOptions.CaptchaLength);
		byte[] image = MakeCaptchaImage(captcha, effectiveOptions);

		return new ByteArrayResultModel
		{
			Image = image,
			GeneratedCaptcha = captcha
		};
	}

	/// <summary>
	/// Validates whether the provided solution matches the given captcha riddle.
	/// </summary>
	/// <param name="riddle">The original captcha text that was generated.</param>
	/// <param name="solution">The user-provided input to validate against the captcha.</param>
	/// <returns>
	/// True if the solution matches the riddle (case-insensitive, trimmed); otherwise, false.
	/// </returns>
	public bool CheckCaptcha(string riddle, string solution)
	{
		if (string.IsNullOrWhiteSpace(riddle) || string.IsNullOrWhiteSpace(solution))
			return false;

		return string.Equals(riddle.Trim(), solution.Trim(), StringComparison.OrdinalIgnoreCase);
	}

	#endregion

	#region [Private method(s)]

	private string GenerateAlphanumericCaptcha(int length)
	{
		var captcha = new StringBuilder();
		for (int i = 0; i < length; i++)
		{
			int index = _random.Next(_captchaChars.Length);
			captcha.Append(_captchaChars[index]);
		}
		return captcha.ToString();
	}

	private SKColor GetRandomColor()
	{
		return new SKColor(
			(byte)_random.Next(256),
			(byte)_random.Next(256),
			(byte)_random.Next(256)
		);
	}

	private void DrawLineNoise(SKCanvas canvas, CaptchaOptions options)
	{
		using var paint = new SKPaint
		{
			StrokeWidth = 2,
			IsAntialias = true
		};

		for (int i = 0; i < options.LineNoiseCount; i++)
		{
			paint.Color = GetRandomColor();
			var x0 = _random.Next(options.Width);
			var y0 = _random.Next(options.Height);
			var x1 = _random.Next(options.Width);
			var y1 = _random.Next(options.Height);
			canvas.DrawLine(x0, y0, x1, y1, paint);
		}
	}

	private void DrawDotNoise(SKCanvas canvas, CaptchaOptions options)
	{
		using var paint = new SKPaint
		{
			IsAntialias = true
		};

		for (int i = 0; i < options.DotNoiseCount; i++)
		{
			paint.Color = GetRandomColor();
			var x = _random.Next(options.Width);
			var y = _random.Next(options.Height);
			canvas.DrawCircle(x, y, 2, paint);
		}
	}

	private byte[] MakeCaptchaImage(string captcha, CaptchaOptions options)
	{
		var imageInfo = new SKImageInfo(options.Width, options.Height);
		using var surface = SKSurface.Create(imageInfo);
		var canvas = surface.Canvas;

		canvas.Clear(SKColors.White);

		using var paint = new SKPaint
		{
			TextSize = options.FontSize,
			IsAntialias = true,
			Color = GetRandomColor(),
			Typeface = SKTypeface.FromFamilyName("Arial", SKFontStyle.Italic)
		};

		float textWidth = paint.MeasureText(captcha);
		float x = (options.Width - textWidth) / 2;
		float y = (options.Height + paint.TextSize) / 2 - 5;
		canvas.DrawText(captcha, x, y, paint);

		DrawLineNoise(canvas, options);
		DrawDotNoise(canvas, options);

		using var image = surface.Snapshot();
		return image.Encode(SKEncodedImageFormat.Png, 70).ToArray();
	}

	#endregion
}
