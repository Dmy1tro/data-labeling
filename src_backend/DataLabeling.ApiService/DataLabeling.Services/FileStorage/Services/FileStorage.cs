using DataLabeling.Services.Interfaces.FileStorage.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DataLabeling.Services.FileStorage.Services
{
    public class FileStorage : IFileStorage
    {
        private readonly string _rootPath;

        public FileStorage(string rootPath)
        {
            _rootPath = rootPath;
            Directory.CreateDirectory(rootPath);
        }

        public async Task<(byte[], string)> GetFileAsync(string filePath)
        {
            var fullPath = GetFullFilePath(filePath);
            var fileBytes = await File.ReadAllBytesAsync(fullPath);
            return (fileBytes, "application/octetstream");
        }

        public Func<Stream> GetFileStream(string filePath)
        {
            var fullPath = GetFullFilePath(filePath);
            Func<Stream> stream = () => File.OpenRead(fullPath);

            return stream;
        }

        public string GetFullPath(string filePath)
        {
            return GetFullFilePath(filePath);
        }

        public async Task<string> UploadFileAsync(string fileName, Func<Stream> fileFunc)
        {
            using var file = fileFunc();
            var filePath = $"{Guid.NewGuid()}_{fileName}";
            var fullPath = GetFullFilePath(filePath);

            using var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);
            await file.CopyToAsync(fileStream);

            return filePath;
        }

        public async Task<string> UploadFileAsync(string fileName, byte[] file)
        {
            var filePath = $"{Guid.NewGuid()}_{fileName}";
            var fullPath = GetFullPath(filePath);

            using var fileStream = new FileStream(fullPath, FileMode.OpenOrCreate);
            await fileStream.WriteAsync(file, 0, file.Length);

            return filePath;
        }

        private string GetFullFilePath(string fileName) => $"{_rootPath}{fileName}";
    }
}
