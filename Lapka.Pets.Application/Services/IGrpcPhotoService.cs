using System;
using System.IO;
using System.Threading.Tasks;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Services
{
    public interface IGrpcPhotoService
    {
        public Task<string> AddAsync(string name, Guid userId, bool isPublic, Stream photo, BucketName bucket);
    }
}