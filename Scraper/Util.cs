using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace Scraper
{
	public class Util
	{
		public static string GetHtml(string url, Encoding enc)
		{
			WebClient wc = new WebClient();
			return ExtractString(wc.DownloadData(url), enc);
		}

		static string ExtractString(byte[] webResult, Encoding enc)
		{
			string s = enc.GetString(webResult);
			return s;
		}

		public static HtmlDocument LoadDoc(string html)
		{
			var doc = new HtmlDocument();
			doc.LoadHtml(html);
			return doc;
		}
	}
}
