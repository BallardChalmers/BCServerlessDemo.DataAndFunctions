using BCServerlessDemo.DataAndFunctions.Core.Model;
using BCServerlessDemo.DataAndFunctions.Core.Model.Account;
using System.Linq;

namespace BCServerlessDemo.DataAndFunctions.Core.Service.Search
{
    public interface IQueryBuilder<T, U>
    {
        IQueryable<U> GetQuery(IQueryable<T> query, GridQuery gridQuery, UserDigest userDigest);

        IQueryable<U> GetTotal(IQueryable<T> query, UserDigest userDigest);
    }
}
