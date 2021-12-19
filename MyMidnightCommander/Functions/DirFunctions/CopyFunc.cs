using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyMidnightCommander.Functions.DirFunctions
{
    public static class CopyFunc
    {
        public static void HandleCopy(string source, string destination, bool isFolder)
        {
            string[] sourceParts = source.Split('\\');
            int sourceTrimLenght = 0;
            for (int i = 0; i < sourceParts.Length - 1; i++)
                sourceTrimLenght += sourceParts[i].Length + 1;

            if (isFolder)
            {
                DirectoryInfo dir = new DirectoryInfo(source);

                FuncSupportMethods.GoThroughDirectory(dir, sourceTrimLenght, destination, "copy");

                DirectoryInfo destinationDir2 = new DirectoryInfo(destination + sourceParts[sourceParts.Length - 1]);
                if (!destinationDir2.Exists)
                    destinationDir2.Create();

                foreach (FileInfo file in dir.GetFiles())
                    file.CopyTo(destination + file.FullName.Substring(sourceTrimLenght, file.FullName.Length - sourceTrimLenght));
            }
            else
            {
                FileInfo myFile = new FileInfo(source);
                myFile.CopyTo(destination);
            }
        }
    }
}
