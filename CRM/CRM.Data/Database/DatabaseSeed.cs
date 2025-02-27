using System.Collections.Generic;
using CRM.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace CRM.Data.Database
{
    public static class DatabaseSeed
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.ClassifierCounterSeed();
        }

        /// <summary>
        /// Seed for <see cref="ClassifierCounter"/>
        /// </summary>
        /// <param name="modelBuilder"></param>
        private static void ClassifierCounterSeed(this ModelBuilder modelBuilder)
        {
            var data = new List<ClassifierCounter>
            {
                new ClassifierCounter {CounterOid = (int)Counter.DealNumber, NextValue = 1, Usage = "Counter value for deal number generator"}
            };

            modelBuilder.Entity<ClassifierCounter>().HasData(data);
        }
    }
}