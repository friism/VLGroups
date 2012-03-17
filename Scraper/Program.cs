using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Threading;
using Core.Model;
using Core.Persistence;

namespace Scraper
{
	public class Program
	{
		static void Main(string[] args)
		{
			while (true)
			{
				using (var context = new Context())
				{
					var now = DateTime.UtcNow;
					if (context.Batches.Any(x => 
						x.FetchedAt.Year == now.Year && x.FetchedAt.Month == now.Month))
					{
						Thread.Sleep(24 * 60 * 60 * 1000);
						continue;
					}
				}

				try
				{
					var batch = new Batch
					{
						FetchedAt = DateTime.UtcNow,
					};

					var members = VLGroupScraper.GetMembers();

					batch.Members = members.ToList();

					using (var context = new Context())
					{
						context.Batches.Add(batch);
						context.SaveChanges();
					}
				}
				catch (Exception exception)
				{
					using (var smtpClient = new SmtpClient())
					{
						var emailAddress = ConfigurationManager.AppSettings.Get("logemailaddress");
						smtpClient.Send(new MailMessage(
							emailAddress, emailAddress, "Problem scraping", exception.ToString()));
					}
				}
			}
		}
	}
}
