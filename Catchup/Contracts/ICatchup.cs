using System.Drawing;

namespace Catchup.Contracts;
public interface ICatchup
{
	string GetACaptcha();
	Bitmap GetAnImageCaptchaInBitmapFormat();
	byte[] GetAnImageCaptchaInByteArray();
	bool CheckCaptcha(string words, string number);
}
