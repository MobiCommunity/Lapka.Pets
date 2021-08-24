using System;
using System.IO;
using System.Threading.Tasks;
using Lapka.Pets.Core.ValueObjects;

namespace Lapka.Pets.Application.Services
{
    public interface IGrpcPhotoService
    {
        public Task AddAsync(string photoPath, Stream photo, BucketName bucket);
        public Task DeleteAsync(string photoPath, BucketName bucket);
    }
}