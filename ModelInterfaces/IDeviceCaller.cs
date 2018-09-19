using System;
using System.Threading.Tasks;

namespace ModelInterfaces
{
    public interface IDeviceCaller
    {
        Task<string> SendRequestAsync(INode node, Uri deviceUri, string device, string state);
    }

    public class DeviceCallerFactory
    {
        public static IDeviceCaller GetCaller()
        {
            return new SetDevice();
        }
    }
}
