using System.Text;

namespace RpiProcessHandler.Rtsp
{
    public class RtspHelper
    {
        public static string InjectCredentialsToUrl(string url, string login, string password)
        {
            var protocol = "rtsp://";
            var usePassword = true;

            if (login == "" && password == "")
                usePassword = false;

            string[] splitedUrl = url.Split(protocol);

            var builder = new StringBuilder();
            builder.Append(protocol)
                .Append(login)
                .Append(usePassword ? ":" : string.Empty)
                .Append(password)
                .Append(usePassword ? "@" : string.Empty)
                .Append(splitedUrl[1]);

            return builder.ToString();
        }
    }
}
