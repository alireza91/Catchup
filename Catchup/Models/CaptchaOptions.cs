namespace Catchup.Models;
public class CaptchaOptions
{
	public int Width { get; set; } = 200;
	public int Height { get; set; } = 60;
	public int FontSize { get; set; } = 48;
	public int CaptchaLength { get; set; } = 6;
	public int LineNoiseCount { get; set; } = 12;
	public int DotNoiseCount { get; set; } = 220;
}
