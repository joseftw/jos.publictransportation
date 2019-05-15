using System.IO;
using System.Threading.Tasks;
using JOS.Core;

namespace JOS.TrafikLab.Core
{
    public interface IGetAllStopPointsBlobQuery
    {
        Task<Result<Stream>> Execute();
    }
}