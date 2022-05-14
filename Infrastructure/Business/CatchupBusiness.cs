using Catchup.Contracts;

namespace Infrastructure
{
	public class CatchupBusiness : ICatchupBusiness
	{
		private readonly string[] _singleDigits = { "یک", "دو", "سه", "چهار", "پنج", "شش", "هفت", "هشت", "نه" };
		private readonly string[] _twoDigits = { "ده", "یازده", "دوازده", "سیزده", "چهارده", "پانزده", "شانزده", "هفده", "هجده", "نوزده", "بیست", "سی", "چهل", "پنجاه", "شصت", "هفتاد", "هشتاد", "نود" };
		private readonly int[] _twoDigitsMatch = { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 30, 40, 50, 60, 70, 80, 90 };
		private readonly string[] _threeDigits = { "صد", "دویست", "سیصد", "چهارصد", "پانصد", "ششصد", "هفتصد", "هشتصد", "نهصد" };
		private readonly Random _random = new();

		public string GetACaptcha()
		{
			int single = _random.Next(_singleDigits.Length);
			int two = _random.Next(_twoDigits.Length);
			int three = _random.Next(_threeDigits.Length);
			int singleForThousand = _random.Next(_singleDigits.Length);
			var result = _singleDigits[singleForThousand] + " هزار و " + _threeDigits[three] + " و " + _twoDigits[two] + (two < 10 ? "" : $" و {_singleDigits[single]}");
			return result;
		}

		public bool CheckCaptcha(string riddle, string solution) =>
			Analyze(riddle).Equals(solution);

		private string Analyze(string riddle)
		{
			try
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
			catch
			{
				return "-1111";
			}

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

	}
}