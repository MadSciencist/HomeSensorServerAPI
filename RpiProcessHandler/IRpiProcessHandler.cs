using System.Threading.Tasks;

namespace RpiProcesses
{
    public interface IRpiProcessHandler
    {
        string ExecuteShellCommand(string command);
    }
}