using System;
using System.Linq;
using CRM.Data.Database;
using CRM.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace CRM.Data.NumberProvider
{
    public class DealNumberProvider : NumberProviderBase
    {
        private readonly CRMDbContext _dbContext;

        public DealNumberProvider(CRMDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override string Prefix => "DL-";
        
        public override string GetUniqueId()
        {
            return GetUniqueId(Prefix);
        }

        public override string GetUniqueId(string prefix)
        {
            var counter = _dbContext.ClassifierCounter.AsTracking().FirstOrDefault(x => x.CounterOid == (int) Counter.DealNumber);

            if (counter is null)
                throw new ArgumentException($"Counter not found for value : {Counter.DealNumber}");

            var value = counter.NextValue;

            counter.NextValue++;
            _dbContext.SaveChanges();

            return Format(value, prefix);
        }
    }
}