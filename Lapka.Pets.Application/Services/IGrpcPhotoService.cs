using System;
using System.IO;
using System.Threading.Tasks;

namespace Lapka.Pets.Application.Services
{
    public interface IGrpcPhotoService
    {
        public Task AddAsync(string photoPath, Stream photo);
        public Task DeleteAsync(string photoPath);
    }
}