using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MyMidnightCommander.Dialogues;

namespace MyMidnightCommander.Functions
{
    public class Copy
    {
        public void HandleCopy(string[] path, string[] selectedItem)
        {
            //change paths accordint to selected item in dir
            for (int i = 0; i < 2; i++)
            {
                if (selectedItem[i] == "\\..") // path[i] stays the same
                    ;
                else if (selectedItem[i].Substring(0, 1) == "\\")
                    path[i] += selectedItem[i];
                else if (selectedItem[i].Substring(0, 1) == " ")
                    path[i] += '\\' + selectedItem[i].Substring(1, selectedItem[i].Length - 1);
            }

            //parameters for dialogue
            string[] inputLabels = { "Zdrojová složka: ", "Cílová složka" };
            string[] inputVals = { path[0], path[1] };
            string[] buttons = { "OK", "Cancel" };

            //dialogue
            List<string> CopyVals = new List<string>();
            CopyVals = DrawInputDialogue("kopírování", inputLabels, inputVals, buttons);

            if (CopyVals[0] == "Cancel")
                return;
            else if (CopyVals[0] == "OK")
            {
                string source = CopyVals[1];
                string destination = CopyVals[2] + '\\';

                DirectoryInfo dir = new DirectoryInfo(source);

                string[] sourceParts = source.Split('\\');
                int sourceTrimLenght = 0;
                for (int i = 0; i < sourceParts.Length - 1; i++)
                    sourceTrimLenght += sourceParts[i].Length + 1;

                //check if destination exists
                DirectoryInfo destinationDir = new DirectoryInfo(destination + sourceParts[sourceParts.Length - 1]);
                if (destinationDir.Exists)
                {
                    InfoDialogue myDialogue = new InfoDialogue();
                    myDialogue.Draw($"položka {destinationDir.FullName} already exists!");
                    Console.ReadKey();
                    return;
                }

                //handle the operation
                GoThroughDirectory(dir, sourceTrimLenght, destination, "copy");

                DirectoryInfo destinationDir2 = new DirectoryInfo(destination + sourceParts[sourceParts.Length - 1]);
                if (!destinationDir2.Exists)
                    destinationDir2.Create();

                foreach (FileInfo file in dir.GetFiles())
                    file.CopyTo(destination + file.FullName.Substring(sourceTrimLenght, file.FullName.Length - sourceTrimLenght));
                return;
            }
            return;
        }
    }
}
