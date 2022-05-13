using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catchup.Contracts
{
	public interface ICatchupBusiness
	{
		string GetACaptcha();
		bool CheckCaptcha(string words, string number);

	}
}
