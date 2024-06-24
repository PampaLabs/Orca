using ContosoUniversity.EntityFrameworkCore.Store.Infrastructure.Data.Seeders;
using Balea.Store.EntityFrameworkCore;

namespace ContosoUniversity.EntityFrameworkCore.Store
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDbContext<BaleaDbContext>((db, acc) => BaleaSeeder.Seed(db, acc).Wait())
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
