using System;
using System.IO;
using System.Threading.Tasks;

namespace DataLabeling.Services.Interfaces.FileStorage.Services
{
    public interface IFileStorage
    {
        Task<string> UploadFileAsync(string fileName, Func<Stream> data);

        Task<string> UploadFileAsync(string fileName, byte[] file);

        string GetFullPath(string filePath);

        Task<(byte[], string)> GetFileAsync(string filePath);

        Func<Stream> GetFileStream(string filePath);
    }
}
