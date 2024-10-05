using RestaurantManagement.SharedLibrary.Responses;
using RestaurantManagement.SharedLibrary.Responses;
using System.Linq.Expressions;
namespace RestaurantManagement.SharedLibrary.Interface
{
    public interface IGenericInterface<T> where T: class
    {
        Task<Response<T>> CreateAsync(T entity);

        Task<Response<T>> UpdateAsync(T entity);

        Task<Response<T>> DeleteAsync(T entity);

        Task<IEnumerable<T>> GetAllAsync(T entity);

        Task<T> FindByIdAsync(int id);

        // where = lambda expression
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
