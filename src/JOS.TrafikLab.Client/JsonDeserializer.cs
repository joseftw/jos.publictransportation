using System;
using System.IO;
using Newtonsoft.Json;

namespace JOS.TrafikLab.Client
{
    public class JsonDeserializer : IJsonDeserializer
    {
        private readonly JsonSerializer _jsonSerializer;

        public JsonDeserializer(JsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
        }

        public T Deserialize<T>(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                return _jsonSerializer.Deserialize<T>(jsonTextReader);
            }
        }
    }
}
