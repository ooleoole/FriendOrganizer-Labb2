using System.Threading.Tasks;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    public interface ILocationRepostitory: IGenericRepository<Location>
    {
        Task<Location> GetByName(string name);
    }
}