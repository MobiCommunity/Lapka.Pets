namespace Lapka.Pets.Application.Services
{
    public interface IJsonSerializer
    {
        string Serialize(object instance);

        TResult Deserialize<TResult>(string value);
    }
}