using Microsoft.EntityFrameworkCore;
using Vista.App.Data;       // Import the database classes 

var trainerDbContext = new TrainersDbContext();    // Create database context

ListCateogies(trainerDbContext);
ListTrainers(trainerDbContext);

// Method to List Categories from the database
static void ListCateogies(TrainersDbContext dbContext)
{
    // Load list of categories from the database
    var catogoryList = dbContext.Categories.ToList();

    Console.WriteLine("Category List\n");

    // Display list of categories from the memory object (categoryList)
    foreach (var cat in catogoryList)
    {
        Console.WriteLine($"{cat.CategoryCode} {cat.CategoryName}");
    }
    Console.WriteLine();
}


// Method to List trainers from the database
static void ListTrainers(TrainersDbContext dbContext)
{
    // Load list of categories from the database
    var trainerList = dbContext.Trainers.ToList();

    Console.WriteLine("Trainer List\n");

    // Display list of categories from the memory object (trainerList)
    foreach (var t in trainerList)
    {
        Console.WriteLine($"{t.TrainerId} {t.Name}");
    }
    Console.WriteLine();
}



