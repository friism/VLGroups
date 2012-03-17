using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Model;
using FileHelpers;
using HtmlAgilityPack;

namespace Scraper
{
	public class VLGroupScraper
	{
		private static IEnumerable<string> malenames = 
			GetListFromCVS("data/drenge.csv").Select(_ => _.Trim());
		private static IEnumerable<string> femalenames =
			GetListFromCVS("data/piger.csv").Select(_ => _.Trim());

		public static IEnumerable<Member> GetMembers()
		{
			var relevantAreas = new List<string>() {
				"København",
				"Århus",
				"Aarhus",
				"Aalborg",
				"Ålborg",
				"Vestsjælland",
				"Kolding",
				"Midt- og Nordvestjylland",
				"Esbjerg",
				"Haderslev-Kolding",
				"Østjylland",
				"Vendsyssel",
				"Fyn",
				"Fredericia",
				"Helsingør",
				"Vejle",
				"Viborg",
				"Horsens",
				"Sydjylland",
				"Midt-Østjylland",
				"Lillebælt",
				"Frederiksborg",
				"Trekantområdet",
				"Sønderjylland",
				"Sydvest Jylland",
			};

			var doc = Util.LoadDoc(
				Util.GetHtml("http://iframe.vl.dk/gruppeoversigt.php", Encoding.UTF8));
			var rows = doc.DocumentNode.
				SelectNodes("//table[@id='table-1']/tbody/tr/td[@width='40%']/a");

			var allgroups = rows.OfType<HtmlNode>().
				Select(_ => new
				{
					Name = _.InnerText.Trim(),
					Url = _.Attributes["href"].Value,
				});

			var nondanish = allgroups.Where(_ => !relevantAreas.Contains(_.Name));

			Console.WriteLine("Ignoring {0}", string.Join(", ", nondanish.Select(x => x.Name)));

			var danishGroups = allgroups.Where(_ => relevantAreas.Contains(_.Name));

			var members = danishGroups.AsParallel().WithDegreeOfParallelism(10).SelectMany(_ =>
				GetGroup(_.Url, _.Name));

			return members;
		}

		private static bool GetGender(string firstname)
		{
			string testname = firstname.Split(' ')[0].Trim();
			var forcedMaleNames = new[] { "Bo", "Kim", "Johnny", "Benny", "Tonny" };
			var forcedFemaleNames = new[] { "Mai", "Joan", "Kelly" };

			if (forcedMaleNames.Contains(testname))
				return true;

			if (forcedFemaleNames.Contains(testname))
				return false;

			if (malenames.Contains(testname))
				return true;

			if (femalenames.Contains(testname))
				return false;

			// this is an expedient measure to get groups with no women
			return true;
		}

		private static IEnumerable<string> GetListFromCVS(string filename)
		{
			var eng = new FileHelperEngine<Name>();
			return eng.ReadFile(filename).Select(_ => _.TheName);
		}

		private static IEnumerable<Member> GetGroup(string url, string area)
		{
			var doc = Util.LoadDoc(Util.GetHtml("http://iframe.vl.dk/" + url, Encoding.UTF8));
			var rows = doc.DocumentNode.
				SelectNodes("//table[@id='table-1']/tbody/tr");

			foreach (var row in rows)
			{
				var cells = row.SelectNodes("./td");
				yield return new Member
				{
					Title = cells[0].InnerText.Trim(),
					Firstname = cells[1].InnerText.Trim(),
					Lastname = cells[2].InnerText.Trim(),
					Company = cells[3].InnerText.Trim(),
					Group = int.Parse(url.Split(new string[] { "id=" }, StringSplitOptions.RemoveEmptyEntries)[1]),
					ProbablyGender = GetGender(cells[1].InnerText.Trim()),
				};
			}
		}
	}

	[DelimitedRecord(",")]
	class Name
	{
		public string TheName { get; set; }
	}
}
