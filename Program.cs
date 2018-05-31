using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var folderName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var filesToProcess = GetCandidateFilesToProcess(folderName);
            filesToProcess = FilterFilesToProcess(filesToProcess); // Filter on size and guid
            RenameFilesToFolderName(filesToProcess);

        }

        private static void RenameFilesToFolderName(string[] fileNames)
        {
            foreach (string fileName in fileNames)
            {
                var fi = new FileInfo(fileName);
                var newFileName = Path.Combine(Path.GetDirectoryName(fileName), $"{fi.Directory.Name}{fi.Extension}");


                File.Move(fileName, newFileName);
                Console.WriteLine($"Renamed {fileName} to {newFileName}");
            }


        }

        private static string[] FilterFilesToProcess(string[] fileNames)
        {
            var returnValue = new List<string>();

            foreach (string fileName in fileNames)
            {
                // f5ecc3482a9d44888d9268244785600e

                var fi = new FileInfo(fileName);
                if (fi.Length>=300_000_000)
                {
                    Guid testGuid;
                    bool isValid = Guid.TryParse(Path.GetFileNameWithoutExtension(fi.Name), out testGuid);
                    if (isValid)
                    {
                        returnValue.Add(fileName);
                    }
                }
            }

            return returnValue.ToArray();
        }

        private static string[] GetCandidateFilesToProcess(string folderName)
        {
            var returnValue = Directory.GetFiles(folderName, "*.*", SearchOption.AllDirectories);

            return returnValue;
        }
    }
}
