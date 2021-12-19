using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyMidnightCommander.Functions.DirFunctions
{
    public static class RenameFunc
    {
        public static void HandleRename(string source, string newName, bool isFolder)
        {

            string[] sourceParts = source.Split('\\');

            string destination = "";
            for (int i = 0; i < sourceParts.Length - 1; i++)
                destination += sourceParts[i] + "\\";
            destination += newName;

            if (isFolder)
            {
                DirectoryInfo dir = new DirectoryInfo(source);
                dir.MoveTo(destination);
            }
            else
            {
                FileInfo myFile = new FileInfo(source);
                myFile.MoveTo(destination);
            }
        }
    }
}
