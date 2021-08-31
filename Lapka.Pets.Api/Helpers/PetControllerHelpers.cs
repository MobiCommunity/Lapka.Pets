using System;
using System.Collections.Generic;
using System.Linq;
using Lapka.Identity.Api.Models;
using Lapka.Pets.Core.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Lapka.Pets.Api.Helpers
{
    public static class PetControllerHelpers
    {
        public static List<PhotoFile> CreatePhotoFiles(List<IFormFile> photos)
        {
            List<PhotoFile> photoFiles = new List<PhotoFile>();

            if (photos == null) return photoFiles;

            photoFiles.AddRange(photos.Select(photo => photo.AsPhotoFile(Guid.NewGuid())));

            return photoFiles;
        }
    }
}