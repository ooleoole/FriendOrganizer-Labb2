using System.Data.Entity;
using System.Threading.Tasks;
using FriendOrganizer.DataAccess;
using FriendOrganizer.Model;

namespace FriendOrganizer.UI.Data.Repositories
{
    public class LocationRepostitory : GenericRepository<Model.Location, FriendOrganizerDbContext>, ILocationRepostitory
    {

        public LocationRepostitory(FriendOrganizerDbContext context) : base(context)
        {
        }

        public async Task<Location> GetByName(string name)
        {
            return await Context.Locations.SingleOrDefaultAsync(l => l.Name == name);
        }

    }
}