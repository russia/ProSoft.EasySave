using System.Security.Cryptography;

namespace ProSoft.EasySave.Application.Extensions;

public static class FileInfoExtensions
{
    public static string GetHash(this FileInfo fileInfo)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(fileInfo.FullName);
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash)
            .Replace("-", "")
            .ToLowerInvariant();
    }
}