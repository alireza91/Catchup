using Catchup.Models;
using System.Drawing;

namespace Catchup.Contracts;
public interface ICatchup
{
	//string GetACaptcha();
	////BitmapResultModel GetAnImageCaptchaInBitmapFormat();
	//ByteArrayResultModel GetAnImageCaptchaInByteArray();
	//bool CheckCaptcha(string words, string number);

	public ByteArrayResultModel GetAnImageCaptcha(CaptchaOptions? options = null);

	//bool CheckCaptcha(string riddle, string solution);
}
