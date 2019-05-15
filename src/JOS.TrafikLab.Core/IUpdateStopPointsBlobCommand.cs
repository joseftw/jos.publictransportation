using System.Threading.Tasks;

namespace JOS.TrafikLab.Core
{
    public interface IUpdateStopPointsBlobCommand
    {
        Task Execute();
    }
}
