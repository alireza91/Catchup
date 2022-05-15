using Catchup.Contracts;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Text;

namespace Catchup;
public class Catchup : ICatchup
{
	private readonly string[] _singleDigits = { "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" };
	private readonly string[] _twoDigits = { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };
	private readonly string[] _threeDigits = { "صد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" };

	private readonly short[] _twoDigitsMatch = { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 30, 40, 50, 60, 70, 80, 90 };
	private readonly Random _random = new();
	private readonly short _width = 400;
	private readonly short _height = 100;
	private StringBuilder sb = new StringBuilder();

	public string GetACaptcha()
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
			sb.Append(words[i]);

		return sb.ToString();
	}

	public bool CheckCaptcha(string riddle, string solution) =>
		Analyze(riddle).Equals(solution);

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

	private Bitmap generateCaptchaImage(string riddleString)
	{
		//First declare a bitmap and declare graphic from this bitmap
		Bitmap bitmap = new Bitmap(_width, _height, PixelFormat.Format32bppArgb);
		Graphics g = Graphics.FromImage(bitmap);
		//And create a rectangle to delegete this image graphic 
		Rectangle rect = new Rectangle(0, 0, _width, _height);
		//And create a brush to make some drawings
		HatchBrush hatchBrush = new HatchBrush(HatchStyle.DottedGrid, Color.Aqua, Color.White);
		g.FillRectangle(hatchBrush, rect);

		//here we make the text configurations
		GraphicsPath graphicPath = new GraphicsPath();
		//add this string to image with the rectangle delegate
		graphicPath.AddString(riddleString, FontFamily.GenericMonospace, (int)FontStyle.Bold, 90, rect, null);
		//And the brush that you will write the text
		hatchBrush = new HatchBrush(HatchStyle.Percent20, Color.Black, Color.GreenYellow);
		g.FillPath(hatchBrush, graphicPath);
		//We are adding the dots to the image
		for (int i = 0; i < (int)(rect.Width * rect.Height / 50F); i++)
		{
			int x = _random.Next(_width);
			int y = _random.Next(_height);
			int w = _random.Next(10);
			int h = _random.Next(10);
			g.FillEllipse(hatchBrush, x, y, w, h);
		}
		//Remove all of variables from the memory to save resource
		hatchBrush.Dispose();
		g.Dispose();
		//return the image to the related component
		return bitmap;
	}

}
