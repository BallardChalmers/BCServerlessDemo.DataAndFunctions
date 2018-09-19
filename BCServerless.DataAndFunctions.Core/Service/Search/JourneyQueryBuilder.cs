using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BCServerlessDemo.DataAndFunctions.Core.Service.Search
{
    public class JourneyQueryBuilder : IQueryBuilder<Journey, Journey>
    {
        public IQueryable<Journey> GetQuery(IQueryable<Journey> query, GridQuery gridQuery, UserDigest userDigest)
        {
            var expressions = new List<Expression<Func<Journey, bool>>>();
            expressions.Add((c => c.deleted == false));

            foreach (var filter in gridQuery?.filters.Union(gridQuery.fixedFilters))
            {
                if (filter.column == "DriverId")
                {
                    expressions.Add(GetDriverClause(filter));
                }

                if (filter.column == "SearchTerm")
                {
                    expressions.Add(GetSearchClause(filter));
                }

                if (filter.column == "JourneyDateEquals")
                {
                    var date = DateTime.Parse(filter.value);
                    

                    expressions.Add(GetJourneyDateEqualsClause(DateTime.SpecifyKind(date, DateTimeKind.Utc)));
                }

                if (filter.column == "JourneyDateFrom")
                {
                    var date = DateTime.Parse(filter.value);

                    expressions.Add(GetJourneyDateFromClause(date));
                }
            }

            expressions.ForEach(fe => query = query.Where(fe));
            

            return query;
        }

        public IQueryable<Journey> GetTotal(IQueryable<Journey> query, UserDigest userDigest)
        {
            return query;
        }

        private Expression<Func<Journey, bool>> GetSearchClause(GridQueryFilter filter)
        {
            var lowerSearchTerm = filter.value.ToLower();
            return (c => c.name.ToLower().Contains(lowerSearchTerm.ToLower()) || c.Driver.name.ToLower().Contains(lowerSearchTerm.ToLower()));
        }

        private Expression<Func<Journey, bool>> GetJourneyDateEqualsClause(DateTime date)
        {
            return (w => w.JourneyDate == date);
        }

        private Expression<Func<Journey, bool>> GetJourneyDateFromClause(DateTime date)
        {
            return (w => w.JourneyDate >= date);
        }

        private Expression<Func<Journey, bool>> GetDriverClause(GridQueryFilter filter)
        {
            var driverId = filter.value;

            return (c => c.Driver.id == driverId);
        }
    }
}