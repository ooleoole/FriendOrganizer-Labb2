using System;
using System.Collections.Generic;
using System.Linq;

namespace FriendOrganizer.DataAccess.Migrations
{
    using Model;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<FriendOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FriendOrganizerDbContext context)
        {
            context.Friends.AddOrUpdate(
                f => f.FirstName,
                new Friend { FirstName = "Kalle", LastName = "Anka" },
                new Friend { FirstName = "Kajsa", LastName = "Anka" },
                new Friend { FirstName = "Arne", LastName = "Anka" },
                new Friend { FirstName = "Jocke", LastName = "Anka" }
            );
            context.ProgrammingLanguages.AddOrUpdate(
                pl => pl.Name,
                new ProgrammingLanguage { Name = "C#" },
                new ProgrammingLanguage { Name = "TypeScript" },
                new ProgrammingLanguage { Name = "F#" },
                new ProgrammingLanguage { Name = "Swift" },
                new ProgrammingLanguage { Name = "Java" });

            context.SaveChanges();

            context.FriendPhoneNumbers.AddOrUpdate(pn => pn.Number,
                new FriendPhoneNumber { Number = "+49 12345678", FriendId = context.Friends.First().Id });

            context.Locations.AddOrUpdate(new Location
            {
                Name = "Göteborg",
                Longitude = 11.97,
                Latitude = 57.70

            });

            context.Meetings.AddOrUpdate(
                new Meeting
                {
                    Title = "Watching Soccer",
                    DateFrom = new DateTime(2020, 5, 20),
                    DateTo = new DateTime(2020, 5, 26),
                    LocationName = "Göteborg",

                    Friends = new List<Friend>
                    {
                        context.Friends.Single(f => f.FirstName == "Kalle" && f.LastName == "Anka"),
                        context.Friends.Single(f => f.FirstName == "Kajsa" && f.LastName == "Anka")
                    }
                });
            context.SaveChanges();


        }
    }
}
