using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catchup.Models
{
	public class BitmapResultModel
	{
		public Bitmap? BitmapImage { get; set; }
		public string? GeneratedCaptcha { get; set; }
	}
}
