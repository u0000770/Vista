using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Emit;
using VistaApi.Domain;

namespace VistaApi.Data
{
    public class TrainersDbContext : DbContext
    {
        // Notes
        // - DbSet defines the  database table.
        // - the class name is defined as part of the data model
        // - the instance/object name is normally plural
        // - by default, the instance name will become the table name
        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<Trainer> Trainers { get; set; } = null!;

        public DbSet<TrainerCategory> TrainerCategories { get; set; } = null!;

        public DbSet<Session> Sessions { get; set; } = null!;

        private string DbPath { get; set; } = string.Empty;

        // Constructor to set-up the database path & name
        public TrainersDbContext()
        {
            var folder = Environment.SpecialFolder.MyDocuments;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "vista.trainers.db");
        }

        // OnConfiguring to specify that the SQLite database engine will be used
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite($"Data Source={DbPath}");

            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=VistaLocalDB;Trusted_Connection=True;");
            //optionsBuilder.UseSqlServer("Server=TU42687\\SQLEXPRESS;Database=CarsLocalDb;Trusted_Connection=True;");

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Composite  (Compound) Key
            builder.Entity<TrainerCategory>()
                .HasKey(ts => new { ts.TrainerId, ts.CategoryCode });


           // builder.Entity<TrainerCategory>()
           //.HasOne(M => M.)
           //.WithMany(M => M.)
           //.HasForeignKey(MenuFoodItem => MenuFoodItem.MenuId);

           // builder.Entity<MenuFoodItems>()
           // .HasOne(MenuFoodItem => MenuFoodItem.foodItem)
           // .WithMany(MenuFoodItem => MenuFoodItem.Menus)
           // .HasForeignKey(MenuFoodItem => MenuFoodItem.FoodItemId);

            // EF Core can work out most relationships from the navigation properties 
            // One Category to many TrainerCategories needs to be defined because it does 
            // NOT have a convensional primary column name (i.e. CategoryId) and is based
            // on a Composite key 
            builder.Entity<Category>()
                .HasMany(c => c.TrainerCategories)
                .WithOne(tr => tr.Category)
                .HasForeignKey(tr => tr.CategoryCode)
                .OnDelete(DeleteBehavior.Restrict);
                // The OnDelete prevents categories from been deleted if there are 
                // corresponding entities within the TrainerCategories


            // Manually define the Trainer to TrainerCategories relationship to
            // enforce the delete rule.
            builder.Entity<Trainer>()
                .HasMany(c => c.TrainerCategories)
                .WithOne(tr => tr.Trainer)
                .HasForeignKey(tr => tr.TrainerId)
            .OnDelete(DeleteBehavior.Restrict);





            // Insert Seed/Test Data
            builder.Entity<Category>().HasData(
                new Category { CategoryCode = "IT", CategoryName = "Information Technology" },
                new Category { CategoryCode = "HS", CategoryName = "Health and Safety" },
                new Category { CategoryCode = "LM", CategoryName = "Leadership and Management" },
                new Category { CategoryCode = "ED", CategoryName = "Equality and Diversity" },
                new Category { CategoryCode = "MAR", CategoryName = "Marketing" }
            );

            builder.Entity<Trainer>().HasData(
                new Trainer { TrainerId = 1, Name = "Palmer Patel", Location = "Middlesbrough" },
                new Trainer { TrainerId = 2, Name = "Cater Moon", Location = "Stockton-on-Tees" },
                new Trainer { TrainerId = 3, Name = "Alex Dickerson", Location = "Middlesbrough" },
                new Trainer { TrainerId = 4, Name = "Sally Johnson", Location = "Stockton-on-Tees" }
            );

            builder.Entity<TrainerCategory>().HasData(
                new TrainerCategory { TrainerId = 1, CategoryCode = "IT" },
                new TrainerCategory { TrainerId = 1, CategoryCode = "ED" },
                new TrainerCategory { TrainerId = 2, CategoryCode = "LM" },
                new TrainerCategory { TrainerId = 2, CategoryCode = "HS" },
                new TrainerCategory { TrainerId = 2, CategoryCode = "ED" },
                new TrainerCategory { TrainerId = 3, CategoryCode = "IT" },
                new TrainerCategory { TrainerId = 3, CategoryCode = "LM" }
            );

            builder.Entity<Session>().HasData(
                new Session { SessionId = 1, TrainerId = 1, SessionDate = new DateTime(2023, 1, 10) },
                new Session { SessionId = 2, TrainerId = 1, SessionDate = new DateTime(2023, 1, 11) },
                new Session { SessionId = 3, TrainerId = 1, SessionDate = new DateTime(2023, 1, 12) },
                new Session { SessionId = 4, TrainerId = 1, SessionDate = new DateTime(2023, 1, 13) },
                new Session { SessionId = 5, TrainerId = 1, SessionDate = new DateTime(2023, 1, 14), BookingReference = "TST-99" },
                new Session { SessionId = 6, TrainerId = 1, SessionDate = new DateTime(2023, 1, 17) },
                new Session { SessionId = 7, TrainerId = 1, SessionDate = new DateTime(2023, 1, 18) },
                new Session { SessionId = 8, TrainerId = 1, SessionDate = new DateTime(2023, 1, 19) },
                new Session { SessionId = 9, TrainerId = 1, SessionDate = new DateTime(2023, 1, 20) },
                new Session { SessionId = 10, TrainerId = 1, SessionDate = new DateTime(2023, 1, 21) },

                new Session { SessionId = 11, TrainerId = 2, SessionDate = new DateTime(2023, 1, 10) },
                new Session { SessionId = 12, TrainerId = 2, SessionDate = new DateTime(2023, 1, 11) },
                new Session { SessionId = 13, TrainerId = 2, SessionDate = new DateTime(2023, 1, 12) },
                new Session { SessionId = 14, TrainerId = 2, SessionDate = new DateTime(2023, 1, 13) },
                new Session { SessionId = 15, TrainerId = 2, SessionDate = new DateTime(2023, 1, 14) },
                new Session { SessionId = 16, TrainerId = 2, SessionDate = new DateTime(2023, 1, 17), BookingReference = "TST-98" },
                new Session { SessionId = 17, TrainerId = 2, SessionDate = new DateTime(2023, 1, 18), BookingReference = "TST-97" },
                new Session { SessionId = 18, TrainerId = 2, SessionDate = new DateTime(2023, 1, 19) },
                new Session { SessionId = 19, TrainerId = 2, SessionDate = new DateTime(2023, 1, 20) },
                new Session { SessionId = 20, TrainerId = 2, SessionDate = new DateTime(2023, 1, 21) },

                new Session { SessionId = 21, TrainerId = 3, SessionDate = new DateTime(2023, 1, 10) },
                new Session { SessionId = 22, TrainerId = 3, SessionDate = new DateTime(2023, 1, 11) },
                new Session { SessionId = 23, TrainerId = 3, SessionDate = new DateTime(2023, 1, 12) },
                new Session { SessionId = 24, TrainerId = 3, SessionDate = new DateTime(2023, 1, 13), BookingReference = "TST-96" },
                new Session { SessionId = 25, TrainerId = 3, SessionDate = new DateTime(2023, 1, 14) },
                new Session { SessionId = 26, TrainerId = 3, SessionDate = new DateTime(2023, 1, 17) },
                new Session { SessionId = 27, TrainerId = 3, SessionDate = new DateTime(2023, 1, 18) },
                new Session { SessionId = 28, TrainerId = 3, SessionDate = new DateTime(2023, 1, 19) },
                new Session { SessionId = 29, TrainerId = 3, SessionDate = new DateTime(2023, 1, 20) },
                new Session { SessionId = 30, TrainerId = 3, SessionDate = new DateTime(2023, 1, 21), BookingReference = "TST-94" },

                new Session { SessionId = 31, TrainerId = 4, SessionDate = new DateTime(2023, 1, 11) },
                new Session { SessionId = 32, TrainerId = 4, SessionDate = new DateTime(2023, 1, 12) },
                new Session { SessionId = 33, TrainerId = 4, SessionDate = new DateTime(2023, 1, 13), BookingReference = "TST-95" },
                new Session { SessionId = 34, TrainerId = 4, SessionDate = new DateTime(2023, 1, 18) },
                new Session { SessionId = 35, TrainerId = 4, SessionDate = new DateTime(2023, 1, 19) },
                new Session { SessionId = 36, TrainerId = 4, SessionDate = new DateTime(2023, 1, 20) }

            );

        }

    }
}
