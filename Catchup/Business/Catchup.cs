using Catchup.Contracts;
using Catchup.Models;
using SkiaSharp;
using SkiaSharp.HarfBuzz;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
using System.Text;

namespace Catchup;
public class Catchup : ICatchup
{
	#region [Field(s)]

	private readonly string[] _singleDigits = { "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" };
	private readonly string[] _twoDigits = { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };
	private readonly string[] _threeDigits = { "صد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" };

	private readonly short[] _twoDigitsMatch = { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 30, 40, 50, 60, 70, 80, 90 };

	private readonly Random _random = new();
	private const short _width = 300;
	private const short _height = 50;
	private const double _noisePointPercentage = 0.3;
	private const short _fontSize = 24;

	private readonly StringBuilder _sb = new();

	#endregion

	#region [Public method(s)]

	public string GetACaptcha()
	{
		return GenerateRandomCaptcha();
	}

	//public BitmapResultModel GetAnImageCaptchaInBitmapFormat()
	//{
	//	string generatedCaptchaString = GenerateRandomCaptcha();
	//	return new BitmapResultModel
	//	{
	//		BitmapImage = MakeCaptchaImage(generatedCaptchaString),
	//		GeneratedCaptcha = generatedCaptchaString
	//	};
	//}

	public ByteArrayResultModel GetAnImageCaptchaInByteArray()
	{
		string generatedCaptchaString = GenerateRandomCaptcha();
		var image = MakeCaptchaImage(generatedCaptchaString);
		return new ByteArrayResultModel
		{
			Image = image,
			GeneratedCaptcha = generatedCaptchaString
		};
	}

	public bool CheckCaptcha(string riddle, string solution) =>
		Analyze(riddle).Equals(solution);

	#endregion

	#region [Private method(s)]

	private string GenerateRandomCaptcha()
	{
		int single = _random.Next(_singleDigits.Length);
		int two = _random.Next(_twoDigits.Length);
		int three = _random.Next(_threeDigits.Length);
		int singleForThousand = _random.Next(_singleDigits.Length);

		var words = new string[]
		{
			_singleDigits[singleForThousand],
			" هزار و ",
			_threeDigits[three],
			" و ",
			_twoDigits[two],
			two < 10 ? "" : $" و {_singleDigits[single]}"
		};

		for (short i = 0; i < words.Length; i++)
			_sb.Append(words[i]);

		return _sb.ToString();
	}

	private string Analyze(string riddle)
	{
		riddle = riddle.Replace("هزار", "");
		var riddleArray = riddle.Split(" ");
		riddleArray = riddleArray.Where(x => !string.IsNullOrEmpty(x) && !x.Equals("و")).ToArray();

		int thousandIndex = Array.IndexOf(_singleDigits, riddleArray[0]);
		int hundredIndex = Array.IndexOf(_threeDigits, riddleArray[1]);
		int decimalIndex = Array.IndexOf(_twoDigits, riddleArray[2]);
		int singleIndex = -1;
		if (riddleArray.Length > 3)
			singleIndex = Array.IndexOf(_singleDigits, riddleArray[3]);

		return FindNumberMatch(thousandIndex, hundredIndex, decimalIndex, singleIndex).ToString();
	}

	private int FindNumberMatch(int thousand, int hundred, int dec, int single)
	{
		int forth = int.Parse((thousand + 1).ToString() + "000");
		int third = (hundred + 1) * 100;
		int second = _twoDigitsMatch[dec];
		int first = 0;
		if (single > -1)
			first = single + 1;

		return first + second + third + forth;
	}

	//private Bitmap MakeImageIfOsIsWindows(string riddleString)
	//{
	//	Bitmap bitmap = new Bitmap(_width, _height, PixelFormat.Format32bppArgb);
	//	Graphics g = Graphics.FromImage(bitmap);
	//	Rectangle rect = new Rectangle(0, 0, _width, _height);
	//	HatchBrush hatchBrush = new HatchBrush(HatchStyle.DottedGrid, Color.Aqua, Color.White);
	//	g.FillRectangle(hatchBrush, rect);

	//	GraphicsPath graphicPath = new GraphicsPath();
	//	graphicPath.AddString(riddleString, FontFamily.GenericMonospace, (int)FontStyle.Bold, 80, rect, null);
	//	hatchBrush = new HatchBrush(HatchStyle.Percent20, Color.Black, Color.DarkOliveGreen);
	//	g.FillPath(hatchBrush, graphicPath);
	//	for (int i = 0; i < (int)(rect.Width * rect.Height / 50F); i++)
	//	{
	//		int x = _random.Next(_width);
	//		int y = _random.Next(_height);
	//		int w = _random.Next(10);
	//		int h = _random.Next(10);
	//		g.FillEllipse(hatchBrush, x, y, w, h);
	//	}
	//	hatchBrush.Dispose();
	//	g.Dispose();
	//	return bitmap;
	//}

	private byte[] MakeCaptchaImage(string riddleString)
	{
		SKImageInfo imageInfo = new SKImageInfo(_width, _height, SKColorType.Bgra8888);
		using (var surface = SKSurface.Create(imageInfo))
		{
			var canvas = surface.Canvas;
			canvas.Clear(SKColors.Beige);
			using (SKPaint paint = new() { TextSize = _fontSize })
			{
				paint.Typeface = SKTypeface.FromFamilyName("Arial");
				paint.Color = SKColors.DarkMagenta;
				using (var tf = SKFontManager.Default.MatchCharacter('?'))
				{
					using (var shaper = new SKShaper(tf))
					{
						var x = _width - paint.MeasureText(riddleString);
						var y = ((_height - paint.TextSize) / 3) + paint.TextSize;
						canvas.DrawShapedText(riddleString, new SKPoint(x, y), paint);
					}

					var noisePointMap = NoiseGenerator();
					for (var i = 0; i < noisePointMap.Count(); i++)
					{
						var noisePointPos = noisePointMap.ElementAt(i);
						canvas.DrawPoint(noisePointPos.Item1, noisePointPos.Item2, SKColors.DarkRed);
					}
				}
			}
			SKImage image = surface.Snapshot();
			return image.Encode(SKEncodedImageFormat.Png, 50).ToArray();
		}
	}

	private (int, int)[] NoiseGenerator()
	{
		var noisePointCount = (int)(_width * _height * _noisePointPercentage);
		var noisePointPosList = Enumerable.Range(0, noisePointCount)
			.Select(
				x =>
					(
						_random.Next(_width),
						_random.Next(_height)
					)
			).ToArray();
		return noisePointPosList;
	}

	#endregion
}
