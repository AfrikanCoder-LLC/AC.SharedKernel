using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections;
using System.Collections.Generic;

namespace Kernel.Common.Persistence
{
    public static class DbContextExtension
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool AreAllMigrationsApplied(this DbContext context) 
        {
            var applied = context.GetService<IHistoryRepository>()
                .GetAppliedMigrations()
                .Select(m => m.MigrationId);

            var total = context.GetService<IMigrationsAssembly>()
                .Migrations
                .Select(m => m.Key);

            if (!total.Any())
            {
                // There are no migrations!
                // The application is being run before a db is created or it is being run to actualy create the first migration
                return false;
            }
            else
            {
                return !total.Except(applied).Any();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        public static void SyncroniseEnumeration<T>(this DbContext context) where T : Enumeration
        {
            var allEnumerations = Enumeration.GetAll<T>();
            var totalNumberOfEnumerations = allEnumerations.Count();
            var records = context.Set<T>();
            var totalNumberOfRecords = records.Count();
            foreach (var enumeration in allEnumerations)
            {
                var enumRecord = records.Find(new object[] { enumeration.Id });
                if (enumRecord == null)
                {
                    context.Add(enumeration);
                }
                else
                {
                    if (enumRecord.DisplayName != enumeration.DisplayName)
                    {
                        // If code is inadvertantly changed. We want a specifc change to be made to both code and the DB. 
                        // This only applies to the display name - other setting can be modified
                        throw new Exception(
                            $@"Enumeration in code is not in sync with database. 
                               Enumeration Type : {typeof(T).Name}
                               Enumeration Id : {enumeration.Id}
                               Enumeration / Database Display Value : {enumeration.DisplayName} /// {enumRecord.DisplayName}
                               Please correct this by manualy applying an update script.");
                        
                        //OR just fix as below, according to preference
                        //context.Remove(enumRecord);
                        //context.Add(enumeration);
                    }
                    else
                    {
                        context.Entry(enumRecord).CurrentValues.SetValues(enumeration);
                    }
                }
            }
            if (totalNumberOfEnumerations < totalNumberOfRecords)
            {
                var numberOfExcessRecords = totalNumberOfRecords - totalNumberOfEnumerations;
                for (int i = totalNumberOfRecords; i > totalNumberOfRecords - numberOfExcessRecords; i--)
                {
                    var recordToRemove = records.Find(new object[] { i });
                    context.Remove(recordToRemove);
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="entityItem"></param>
        /// <returns></returns>
        public static async Task LoadAggregate(this DbContext context, Entity entityItem)
        {
            foreach (var collectionEntry in context.Entry(entityItem).Collections)
            {
                await collectionEntry.LoadAsync();
                foreach (var obj in collectionEntry.CurrentValue)
                {
                    if (obj is Entity entity)
                    {
                        await context.LoadAggregate(entity);
                    }
                }
                await context.LoadAggregate(collectionEntry.CurrentValue);
            }

            foreach (var navigationEntry in context.Entry(entityItem).References)
            {
                await navigationEntry.LoadAsync();
            }
        }


        /// <summary>
        /// Ensure the entire aggregate is loaded for the aggregate roots contained in allEntities
        /// </summary>
        /// <param name="context"></param>
        /// <param name="allEntites"></param>
        public static async Task LoadAggregate(this DbContext context, IEnumerable allEntites)
        {
            foreach (var entityItem in allEntites)
            {
                foreach (var collectionEntry in context.Entry(entityItem).Collections)
                {
                    await collectionEntry.LoadAsync();
                    await context.LoadAggregate(collectionEntry.CurrentValue);
                }

                foreach (var navigationEntry in context.Entry(entityItem).References)
                {
                    await navigationEntry.LoadAsync();
                }
            }
        }

    }
}
