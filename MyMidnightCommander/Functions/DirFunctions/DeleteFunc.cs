using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyMidnightCommander.Functions.DirFunctions
{
    public static class DeleteFunc
    {
        public static void HandleDelete(string path, bool isFolder)
        {
            if (isFolder)
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                FuncSupportMethods.GoThroughDirectory(dir, 0, "", "delete");
                foreach (FileInfo file in dir.GetFiles())
                    file.Delete();
                dir.Delete();
            }
            else
            {
                FileInfo myFile = new FileInfo(path);
                myFile.Delete();
            }
        }
    }
}
