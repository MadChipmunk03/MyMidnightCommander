using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyMidnightCommander.Functions
{
    public static class FuncSupportMethods
    {
        public static void GoThroughDirectory(DirectoryInfo dir, int sourceTrimLenght, string destination, string method)
        {
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                if (method == "copy" || method == "move")
                {
                    DirectoryInfo dirToCreateInDest = new DirectoryInfo(destination + subDir.FullName.Substring(sourceTrimLenght, subDir.FullName.Length - sourceTrimLenght));
                    dirToCreateInDest.Create();
                }

                GoThroughDirectory(subDir, sourceTrimLenght, destination, method);

                foreach (FileInfo file in subDir.GetFiles())
                {
                    if (method == "copy")
                        file.CopyTo(destination + file.FullName.Substring(sourceTrimLenght, file.FullName.Length - sourceTrimLenght));
                    else if (method == "move")
                        file.MoveTo(destination + file.FullName.Substring(sourceTrimLenght, file.FullName.Length - sourceTrimLenght));
                    else if (method == "delete")
                    {
                        FileInfo fileToDelete = new FileInfo(file.FullName);
                        fileToDelete.Delete();
                    }
                }

                if (method == "delete" || method == "move")
                    subDir.Delete();
            }
        }

        /*public static int GetSourceTrimLenght(string source)
        {
            string[] sourceParts = source.Split('\\');
            int sourceTrimLenght = 0;
            for (int i = 0; i < sourceParts.Length - 1; i++)
                sourceTrimLenght += sourceParts[i].Length + 1;

            return sourceTrimLenght;
        }*/
    }
}
