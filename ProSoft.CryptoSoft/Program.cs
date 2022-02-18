using System.IO;
using System.Text;
using ProSoft.EasySave.Infrastructure.Helpers;

namespace ProSoft.CryptoSoft
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var key = StringsHelpers.Base64Decode(args[0]);
            var inputFile = StringsHelpers.Base64Decode(args[1]);
            var outputFile = StringsHelpers.Base64Decode(args[2]);
            EncryptFile(inputFile, outputFile, key);
        }

        private static void EncryptFile(string inputFile, string outputFile, string key)
        {
            var fsInput = new FileStream(inputFile, FileMode.Open, FileAccess.Read); // ouvre le fichier d'entré
            var fsOutput = new FileStream(outputFile, FileMode.Create, FileAccess.Write); // créer le fichier de sortie

            var byteArrayKey = Encoding.ASCII.GetBytes(key); // créer un tableau d'octet à partir de la clé
            var byteArrayInput = new byte[fsInput.Length]; // tableau qui va contenir tout les octet du fichier d'entré
            fsInput.Read(byteArrayInput, 0,
                byteArrayInput.Length); // remplie un tableau d'octet avec les octets du fichier d'entré
            var byteArrayOutput =
                new byte[fsInput.Length]; // créer un tableau d'octet à partir de la taille du fichier d'origine

            var indexKey = 0;
            for (var i = 0; i < byteArrayInput.Length; i++)
            {
                byteArrayOutput[i] =
                    (byte)(byteArrayInput[i] ^
                            byteArrayKey
                                [indexKey]); // opération Xor sur un byte de la clé et un byte du fichier d'entré

                if (indexKey ==
                    byteArrayKey.Length -
                    1) // (40 - 43) répête la clé autant de fois que nécessaire pour chiffrer le fichier d'entre
                    indexKey = 0;
                else
                    indexKey++;
            }

            fsOutput.Write(byteArrayOutput); // écrit dans le nouveau fichier

            fsInput.Close(); // ferme le fichier d'entré
            fsOutput.Close(); // ferme le fichier de sortie
        }
    }
}