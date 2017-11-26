using System.Threading.Tasks;

namespace FriendOrganizer.UI.Data.Services
{
    public interface ILocationService
    {
        Task<Model.Location> ResolveLocation(string locationName);
    }
}