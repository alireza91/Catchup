namespace Catchup.Contracts;
public interface ICatchup
{
	string GetACaptcha();
	bool CheckCaptcha(string words, string number);
}
