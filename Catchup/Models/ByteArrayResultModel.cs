using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catchup.Models
{
	public class ByteArrayResultModel
	{
		public byte[]? Image { get; set; }
		public string? GeneratedCaptcha { get; set; }
	}
}
