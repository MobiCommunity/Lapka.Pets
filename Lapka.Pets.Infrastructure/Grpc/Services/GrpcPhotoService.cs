using System;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.ValueObjects;
using Lapka.Pets.Infrastructure.Documents;

namespace Lapka.Pets.Infrastructure.Grpc.Services
{
    public class GrpcPhotoService : IGrpcPhotoService
    {
        private readonly PhotoProto.PhotoProtoClient _client;

        public GrpcPhotoService(PhotoProto.PhotoProtoClient client)
        {
            _client = client;
        }
        
        public async Task AddAsync(Guid id, string name, Stream photo, BucketName bucket)
        {
            await _client.UploadPhotoAsync(new UploadPhotoRequest
            {
                Id = id.ToString(),
                Name = name,
                Photo = await ByteString.FromStreamAsync(photo),
                BucketName = bucket.AsGrpcUpload()
            });
        }

        public async Task DeleteAsync(Guid photoId, BucketName bucket)
        {
            await _client.DeletePhotoAsync(new DeletePhotoRequest
            {
                Id = photoId.ToString(),
                BucketName = bucket.AsGrpcDelete()
            });
        }
    }
}