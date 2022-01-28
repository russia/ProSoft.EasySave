using System.Security.Cryptography;

namespace ProSoft.EasySave.Application.Extensions;

public static class FileInfoExtensions
{
    public static string GetSha256Hash(this FileInfo fileInfo)
    {
        using var stream = new BufferedStream(File.OpenRead(fileInfo.FullName), 4096);
        SHA256Managed sha = new SHA256Managed();
        byte[] checksum = sha.ComputeHash(stream);
        return BitConverter.ToString(checksum).Replace("-", String.Empty).ToLower();
    }

    public static string GetMd5Hash(this FileInfo fileInfo)
    {
        using var md5 = MD5.Create();
        using var stream = File.OpenRead(fileInfo.FullName);
        var hash = md5.ComputeHash(stream);
        return BitConverter.ToString(hash)
            .Replace("-", "")
            .ToLowerInvariant();
    }

    public static bool Compare(this FileInfo fileInfo1, FileInfo fileInfo2)
    {
        var fileInfo1Path = fileInfo1.FullName;
        var fileInfo2Path = fileInfo2.FullName;

        int file1byte;
        int file2byte;
        FileStream fs1;
        FileStream fs2;

        // Determine if the same file was referenced two times.
        if (fileInfo1Path == fileInfo2Path)
            // Return true to indicate that the files are the same.
            return true;

        // Open the two files.
        fs1 = new FileStream(fileInfo1Path, FileMode.Open);
        fs2 = new FileStream(fileInfo2Path, FileMode.Open);

        // Check the file sizes. If they are not the same, the files
        // are not the same.
        if (fs1.Length != fs2.Length)
        {
            // Close the file
            fs1.Close();
            fs2.Close();

            // Return false to indicate files are different
            return false;
        }

        // Read and compare a byte from each file until either a
        // non-matching set of bytes is found or until the end of
        // file1 is reached.
        do
        {
            // Read one byte from each file.
            file1byte = fs1.ReadByte();
            file2byte = fs2.ReadByte();
        } while (file1byte == file2byte && file1byte != -1);

        // Close the files.
        fs1.Close();
        fs2.Close();

        // Return the success of the comparison. "file1byte" is
        // equal to "file2byte" at this point only if the files are
        // the same.
        return file1byte - file2byte == 0;
    }
}