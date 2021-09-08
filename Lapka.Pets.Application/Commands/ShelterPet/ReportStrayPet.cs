using System;
using System.Collections.Generic;
using Convey.CQRS.Commands;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Lapka.Pets.Application.Commands
{
    public class ReportStrayPet : ICommand
    {
        public Guid userId { get; set; }
        public Location Location { get; }
        public List<PhotoFile> Photos { get; }
        public string Description { get; }
        public string ReporterName { get; }
        public string ReporterPhoneNumber { get; }

        public ReportStrayPet(Guid userId, Location location, List<PhotoFile> photos, string description,
            string reporterName, string reporterPhoneNumber)
        {
            this.userId = userId;
            Location = location;
            Photos = photos;
            Description = description;
            ReporterName = reporterName;
            ReporterPhoneNumber = reporterPhoneNumber;
        }
    }
}