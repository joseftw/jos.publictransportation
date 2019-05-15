using System;
using System.IO;
using System.Threading.Tasks;
using JOS.TrafikLab.Core;
using Newtonsoft.Json;

namespace JOS.TrafikLab.Functions.Infrastructure
{
    public class NewtonsoftSerializer : ISerializer
    {
        private readonly JsonSerializer _jsonSerializer;

        public NewtonsoftSerializer(JsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
        }

        public Task<T> Deserialize<T>(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    return Task.FromResult(_jsonSerializer.Deserialize<T>(jsonTextReader));
                }
            }
        }

        public string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data);
        }
    }
}
