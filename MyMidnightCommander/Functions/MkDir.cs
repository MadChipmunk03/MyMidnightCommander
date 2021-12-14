using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyMidnightCommander.Functions
{
    public class MkDir
    {
        public void HandleMkDir(string path)
        {
            string[] inputLabels = { "Zadej název nové složky: " };
            string[] inputVals = { "Nová BUM složka" };
            string[] buttons = { "OK", "Cancel" };

            List<string> MkDirVals = new List<string>();
            MkDirVals = DrawInputDialogue("Vytvořit novou složku", inputLabels, inputVals, buttons);

            if (MkDirVals[0] == "Cancel")
                return;
            else if (MkDirVals[0] == "OK")
            {
                DirectoryInfo myDirInfo = new DirectoryInfo(path + "\\" + MkDirVals[1]);
                myDirInfo.Create();
                return;
            }

            return;
        }
    }
}
