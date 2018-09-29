using System.Threading.Tasks;

namespace NSDeviceCaller
{
    public interface IDeviceCaller
    {
        Task<string> SetStateAsync(string state);
    }
}