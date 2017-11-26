using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FriendOrganizer.Model
{
    public class Location
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public ICollection<Meeting> Meetings { get; set; }

    }
}
