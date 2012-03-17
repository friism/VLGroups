using System.Data.Entity;
using Core.Model;

namespace Core.Persistence
{
	public class Context : DbContext
	{
		public DbSet<Member> Members { get; set; }
		public DbSet<Batch> Batches { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Configuration>());
		}
	}
}
