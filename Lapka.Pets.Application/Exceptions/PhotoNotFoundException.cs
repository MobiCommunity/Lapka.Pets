namespace Lapka.Pets.Application.Exceptions
{
    public class PhotoNotFoundException : AppException
    {
        public string Path { get; }
        public PhotoNotFoundException(string path) : base($"Photo does not exist: {path}")
        {
            Path = path;
        }

        public override string Code => "photo_not_found";
    }
}