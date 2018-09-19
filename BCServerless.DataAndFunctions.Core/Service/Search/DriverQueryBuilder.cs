using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BCServerlessDemo.DataAndFunctions.Core.Service.Search
{
    public class DriverQueryBuilder : IQueryBuilder<Driver, Driver>
    {
        public IQueryable<Driver> GetQuery(IQueryable<Driver> query, GridQuery gridQuery, UserDigest userDigest)
        {
            var expressions = new List<Expression<Func<Driver, bool>>>();
            expressions.Add((c => c.deleted == false));

            foreach (var filter in gridQuery?.filters.Union(gridQuery.fixedFilters))
            {
                if (filter.column == "SearchTerm")
                {
                    expressions.Add(GetSearchClause(filter));
                }
                if (filter.column == "OrganisationName")
                {
                    expressions.Add(GetOrgNameClause(filter));
                }
                if (filter.column == "Disabled")
                {
                    expressions.Add(GetDisabledClause(filter));
                }
            }

            expressions.ForEach(fe => query = query.Where(fe));

            if (userDigest.AppRole == Role.Driver)
            {
                query = QueryByDriver(query, userDigest.DriverId);
            }

            if (userDigest.AppRole == Role.OrgAdmin || userDigest.AppRole == Role.Driver)
            {
                query = QueryByOrg(query, userDigest.OrganisationId);
            }

            return query;
        }

        public IQueryable<Driver> GetTotal(IQueryable<Driver> query, UserDigest userDigest)
        {
            return query;
        }

        private Expression<Func<Driver, bool>> GetSearchClause(GridQueryFilter filter)
        {
            var lowerSearchTerm = filter.value.ToLower();
            return (t => t.name.ToLower().Contains(lowerSearchTerm));
        }

        private Expression<Func<Driver, bool>> GetDisabledClause(GridQueryFilter filter)
        {
            bool isDisabled = false;
            bool.TryParse(filter.value, out isDisabled);          
            return (t => t.IsDisabled == isDisabled);
        }

        private Expression<Func<Driver, bool>> GetOrgNameClause(GridQueryFilter filter)
        {
            var orgName = filter.value.ToLower();
            return (t => t.Organisation == null ? false : t.Organisation.name.ToLower().Contains(orgName));
        }

        private IQueryable<Driver> QueryByDriver(IQueryable<Driver> query, string DriverId)
        {
            return query.Where(t => t.id == DriverId);
        }

        private IQueryable<Driver> QueryByOrg(IQueryable<Driver> query, string orgId)
        {
            return query.Where(t => t.Organisation.id == orgId);
        }
    }
}