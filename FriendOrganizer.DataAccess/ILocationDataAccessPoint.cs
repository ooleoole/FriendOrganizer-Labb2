using System.Threading.Tasks;
using FriendOrganizer.DataAccess.Dtos;

namespace FriendOrganizer.DataAccess
{
    public interface ILocationDataAccessPoint
    {
        Task<LocationDto> GetLocation(string locationName);
    }
}