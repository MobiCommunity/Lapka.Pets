using System;
using System.IO;
using System.Threading.Tasks;
using Google.Protobuf;
using Lapka.Pets.Application.Services;
using Lapka.Pets.Core.ValueObjects;
using Lapka.Pets.Infrastructure.Documents;
using Lapka.Pets.Infrastructure.Mongo.Documents;

namespace Lapka.Pets.Infrastructure.Grpc.Services
{
    public class GrpcPhotoService : IGrpcPhotoService
    {
        private readonly PhotoProto.PhotoProtoClient _client;

        public GrpcPhotoService(PhotoProto.PhotoProtoClient client)
        {
            _client = client;
        }

        public async Task<string> AddAsync(string name, Guid userId, bool isPublic, Stream photo, BucketName bucket)
        {
            UploadPhotoReply response = await _client.UploadPhotoAsync(new UploadPhotoRequest
            {
                IsPublic = isPublic,
                Name = name,
                UserId = userId.ToString(),
                Photo = await ByteString.FromStreamAsync(photo),
                BucketName = bucket.AsGrpcUpload()
            });

            return response.Path;
        }
    }
}