using Newtonsoft.Json;
using Lapka.Pets.Application.Services;

namespace Lapka.Pets.Infrastructure.Services
{
    public class CurrentJsonSerializer : IJsonSerializer
    {
        public string Serialize(object instance)
            => JsonConvert.SerializeObject(instance);

        public TResult Deserialize<TResult>(string value)
            => JsonConvert.DeserializeObject<TResult>(value);
    }
}