using System;
using System.Collections.Generic;
using System.Linq;
using GeoCoordinatePortable;
using Lapka.Identity.Application.Dto;
using Lapka.Pets.Core.Entities;
using Lapka.Pets.Core.ValueObjects;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Application.Dto
{
    public static class Extensions
    {
        public static List<Guid> IdsAsGuidList(this List<PhotoFile> photos)
        {
            List<Guid> guids = new List<Guid>();
            photos.ForEach((p) => guids.Add(p.Id));
            return guids;
        }
        
        public static List<Guid> IdsAsGuidList(this IEnumerable<PhotoFile> photos)
        {
            List<Guid> guids = new List<Guid>();
            photos.ToList().ForEach((p) => guids.Add(p.Id));
            return guids;
        }


    }
}