using System.Threading.Tasks;

namespace RpiProcesses
{
    public interface IRpiProcessHandler
    {
        Task<string> ExecuteShellCommand(string command);
    }
}