using System;
using System.IO;

namespace Lapka.Pets.Core.ValueObjects
{
    public class PhotoFile : File
    {
        public Guid Id { get; }

        public PhotoFile(Guid id, string name, Stream content, string contentType) : base(name, content,
            contentType)
        {
            Id = id;
        }
    }
}