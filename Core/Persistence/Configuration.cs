using System.Data.Entity.Migrations;

namespace Core.Persistence
{
	internal class Configuration : DbMigrationsConfiguration<Context>
	{
		public Configuration()
		{
			AutomaticMigrationsEnabled = true;
		}
	}
}
