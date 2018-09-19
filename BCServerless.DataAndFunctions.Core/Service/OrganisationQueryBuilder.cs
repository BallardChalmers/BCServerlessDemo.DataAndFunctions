using BCServerlessDemo.DataAndFunctions.Core.Domain;
using BCServerlessDemo.DataAndFunctions.Core.Model;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using BCServerlessDemo.DataAndFunctions.Core.Service.Search;
using EnumsNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BCServerlessDemo.DataAndFunctions.Core.Service
{
    public class OrganisationQueryBuilder : IQueryBuilder<Organisation, Organisation>
        {
            public IQueryable<Organisation> GetQuery(IQueryable<Organisation> query, GridQuery gridQuery, UserDigest userDigest)
            {
                var expressions = new List<Expression<Func<Organisation, bool>>>();
                expressions.Add((c => c.deleted == false));

                foreach (var filter in gridQuery?.filters.Union(gridQuery.fixedFilters))
                {
                    if (filter.column == "SearchTerm")
                    {
                        expressions.Add(GetSearchClause(filter));
                    }
                    if (filter.column == "ContactEmail")
                    {
                        expressions.Add(GetContactEmailClause(filter));
                    }

                    if (filter.column == "ContactName")
                    {
                        expressions.Add(GetContactNameClause(filter));
                    }

                    if (filter.column == "ContactPhone")
                    {
                        expressions.Add(GetContactPhoneClause(filter));
                    }
                    

                    if (filter.column == "Organisations")
                    {
                        expressions.Add(GetOrganisationsClause(filter));
                    }

                    if (filter.column == "Proprietor")
                    {
                        expressions.Add(GetProprietorClause(filter));
                    }
                }

                expressions.ForEach(fe => query = query.Where(fe));

                if (userDigest.AppRole == Role.OrgAdmin)
                {
                    query = QueryByOrg(query, userDigest.OrganisationId);
                }

                return query;
            }

            public IQueryable<Organisation> GetTotal(IQueryable<Organisation> query, UserDigest userDigest)
            {
                return query;
            }

            private Expression<Func<Organisation, bool>> GetSearchClause(GridQueryFilter filter)
            {
                var lowerSearchTerm = filter.value.ToLower();
                return (t => t.name.ToLower().Contains(lowerSearchTerm));
            }

            private Expression<Func<Organisation, bool>> GetContactNameClause(GridQueryFilter filter)
            {
                var lowerSearchTerm = filter.value.ToLower();
                return (o => o.primaryContact.name.ToLower().Contains(lowerSearchTerm));
            }

            private Expression<Func<Organisation, bool>> GetContactEmailClause(GridQueryFilter filter)
            {
                var lowerSearchTerm = filter.value.ToLower();
                return (o => o.primaryContact.mail.ToLower().Contains(lowerSearchTerm));
            }

            private Expression<Func<Organisation, bool>> GetContactPhoneClause(GridQueryFilter filter)
            {
                var lowerSearchTerm = filter.value.ToLower();
                return (o => o.primaryContact.telephoneNumber.ToLower().Contains(lowerSearchTerm));
            }

            private Expression<Func<Organisation, bool>> GetProprietorClause(GridQueryFilter filter)
            {
                var lowerSearchTerm = filter.value.ToLower();
                return (t => t.proprietor.ToLower().Contains(lowerSearchTerm));
            }

            private static Expression<Func<Organisation, bool>> GetOrganisationsClause(GridQueryFilter filter)
            {
                var organisations = SearchUtilities.GetIdsFromNameValuePairs(filter.value);

                return (o => organisations.Contains(o.id));
            }

            private IQueryable<Organisation> QueryByOrg(IQueryable<Organisation> query, string orgId)
            {
                return query.Where(org => org.id == orgId);
            }
        }
    }