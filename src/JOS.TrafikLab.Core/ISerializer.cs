using System.IO;
using System.Threading.Tasks;

namespace JOS.TrafikLab.Core
{
    public interface ISerializer
    {
        Task<T> Deserialize<T>(Stream stream);
        string Serialize(object data);
    }
}
