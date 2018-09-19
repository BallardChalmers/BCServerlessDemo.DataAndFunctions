using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace BCServerlessDemo.DataAndFunctions.Core.Service.Search
{
    public class ChangeRecordQueryBuilder : IQueryBuilder<ChangeRecord, ChangeRecord>
    {
        public IQueryable<ChangeRecord> GetTotal(IQueryable<ChangeRecord> query, UserDigest userDigest)
        {
            return query;
        }

        public IQueryable<ChangeRecord> GetQuery(IQueryable<ChangeRecord> query, GridQuery gridQuery, UserDigest userDigest)
        { 
            foreach (var filter in gridQuery.filters)
            {
                if (filter.column == "SearchTerm")
                {
                    query = query.Where(GetSearchClause(filter));
                }

                if (filter.column == "FromChangedOn")
                {
                    query = query.Where(GetFromChangedOnClause(filter));
                }

                if (filter.column == "ToChangedOn")
                {
                    var dateValue = DateTime.Parse(filter.value);

                    query = query.Where(GetToChangedOnClause(filter));
                }

                if (filter.column == "Action")
                {
                    query = query.Where(GetActionClause(filter));
                }

                if (filter.column == "SubjectType")
                {
                    query = query.Where(GetSubjectTypeClause(filter));
                }

                if (filter.column == "ChangedBy")
                {
                    query = query.Where(GetChangedByClause(filter));
                }
            }

            query = ApplySort(query, gridQuery);

            return query;
        }

        private Expression<Func<ChangeRecord, bool>> GetSearchClause(GridQueryFilter filter)
        {
            var lowerSearchTerm = filter.value.ToLower();           
            return (a => a.name.ToLower().Contains(lowerSearchTerm));
        }

        private Expression<Func<ChangeRecord, bool>> GetFromChangedOnClause(GridQueryFilter filter)
        {
            var date = DateTime.Parse(filter.value);

            return (a => a.changedOn >= date);
        }

        private Expression<Func<ChangeRecord, bool>> GetToChangedOnClause(GridQueryFilter filter)
        {
            var date = DateTime.Parse(filter.value);

            return (a => a.changedOn <= date);
        }

        private Expression<Func<ChangeRecord, bool>> GetActionClause(GridQueryFilter filter)
        {
            return (a => a.action == filter.value);
        }

        private Expression<Func<ChangeRecord, bool>> GetSubjectTypeClause(GridQueryFilter filter)
        {
            return (a => a.subjectType == filter.value);
        }

        private Expression<Func<ChangeRecord, bool>> GetChangedByClause(GridQueryFilter filter)
        {
            var ids = SearchUtilities.GetIdsFromNameValuePairs(filter.value);
            var id = ids.Any() ? ids[0] : "";
            return (a => a.changedById == id);
        }

        private IQueryable<ChangeRecord> ApplySort(IQueryable<ChangeRecord> query, GridQuery gridQuery)
        {
            if (!string.IsNullOrEmpty(gridQuery.sort))
            {
                if (gridQuery.sort == "SubjectType")
                {
                    query = gridQuery.sortAscending ? query.OrderBy(ob => ob.subjectType) : query.OrderByDescending(obd => obd.subjectType);
                }
                else if (gridQuery.sort == "Action")
                {
                    query = gridQuery.sortAscending ? query.OrderBy(ob => ob.action) : query.OrderByDescending(obd => obd.action);
                }
                else if (gridQuery.sort == "Name")
                {
                    query = gridQuery.sortAscending ? query.OrderBy(ob => ob.name) : query.OrderByDescending(obd => obd.name);
                }
                else if (gridQuery.sort == "ChangedByName")
                {
                    query = gridQuery.sortAscending ? query.OrderBy(ob => ob.changedByName) : query.OrderByDescending(obd => obd.changedByName);
                }
                else if (gridQuery.sort == "ChangedOn")
                {
                    query = gridQuery.sortAscending ? query.OrderBy(ob => ob.changedOn) : query.OrderByDescending(obd => obd.changedOn);
                }
            }

            return query;
        }
    }
}
