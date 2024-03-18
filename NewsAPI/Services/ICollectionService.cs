using NewsAPI.Models;
//doar declari functiile cu tipul lor 
namespace NewsAPI.Services
{
    public interface ICollectionService<T> where T : new()

    {
        Task <List<Announcement>> GetAll();

        Task <Announcement> Get(Guid id);

       Task< bool> Create(T model);

       Task< bool> Update(Guid id, T model);

       Task< bool> Delete(Guid id);

    }
}
