using System;
using System.IO;
using System.Text;

namespace HomeSensorServerAPI.Utils
{
    public class FileUploadHelper
    {
        public string GetUniqueFileName(string fileName)
        {
            var builder = new StringBuilder();
            fileName = Path.GetFileName(fileName);

            builder.Append(Path.GetFileNameWithoutExtension(fileName))
                    .Append("_")
                    .Append(Guid.NewGuid().ToString().Substring(0, 4))
                    .Append(Path.GetExtension(fileName));

            return builder.ToString();
        }

        public void EnsureFolderCreation(string path)
        {
            if ((path.Length > 0) && (!Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
