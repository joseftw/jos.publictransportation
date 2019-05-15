using System.IO;

namespace JOS.TrafikLab.Client
{
    public interface IJsonDeserializer
    {
        T Deserialize<T>(Stream stream);
    }
}
