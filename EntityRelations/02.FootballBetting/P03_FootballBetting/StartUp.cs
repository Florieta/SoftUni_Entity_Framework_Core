using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data;
using System;

namespace P03_FootballBetting
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            FootballBettingContext dbContext = new FootballBettingContext();
            dbContext.Database.EnsureCreated();

            Console.WriteLine("DB created successfully");

            dbContext.Database.EnsureDeleted();

            //dbContext.Database.Migrate();
        }
    }
}
