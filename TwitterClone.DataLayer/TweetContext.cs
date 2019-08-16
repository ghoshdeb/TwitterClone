using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace TwitterClone.DataLayer
{
    public class TweetContext:DbContext
    {
        public TweetContext():base("tweeterCloneConn")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<TweetContext, TwitterClone.DataLayer.Migrations.Configuration>());
        }
       public DbSet<PERSON> persons { get; set; }
       public DbSet<TWEET> tweets { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PERSON>().HasMany(m => m.followers).WithMany(m => m.following).Map(x => x.MapLeftKey("user_id").MapRightKey("follower_Id").ToTable("following"));
        }



    }
}
