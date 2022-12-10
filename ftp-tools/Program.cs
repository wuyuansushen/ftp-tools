using System.Net;

namespace ftp_tools
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const int bufferSize = 4096;
            string urlPath = @"", fileName = @"";
            string ftpUser = @"", ftpPasswd = @"";

            FtpDownload(urlPath, fileName, ftpUser, ftpPasswd, bufferSize);
        }
        public static void FtpDownload(string urlPath, string fileName, string ftpUser, string ftpPasswd, int bufferSize = 4096)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(urlPath + fileName);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.UseBinary = true;
            request.Credentials = new NetworkCredential(ftpUser, ftpPasswd);
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responStream = response.GetResponseStream())
                {
                    BinStreamToFile(responStream, fileName, bufferSize);
                }
            }
        }

        public static void BinStreamToFile(Stream binStream, string fileName, int bufferSize)
        {
            using (var outputFileStream = File.Open(fileName, FileMode.Create))
            {
                using (var reader = new BinaryReader(binStream))
                {
                    using (var streamOutput = new BinaryWriter(outputFileStream))
                    {
                        byte[] buffer = new byte[bufferSize];
                        int readBytesCount = 0;
                        while ((readBytesCount = reader.Read(buffer, 0, bufferSize)) != 0)
                        {
                            streamOutput.Write(buffer, 0, readBytesCount);
                        }
                    }
                }
            }

        }
    }
}