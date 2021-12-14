using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyMidnightCommander.Functions
{
    public class Rename
    {
        public void HandleRename(string path, string selectedItem)
        {
            //parameters for dialogue
            string[] inputLabels = { "Zadej nový název: " };
            string[] inputVals = { selectedItem.Substring(1, selectedItem.Length - 1) };
            string[] buttons = { "Ok", "Cancel" };

            if (selectedItem == "\\..") // if parent dir is selected
            {
                InfoDialogue showError = new InfoDialogue();
                showError.Draw("Nemůžeš přejmenovat adresář \\..  ");
                Console.ReadKey();
                return;
            }

            //dialogue
            List<string> RenameVals = new List<string>();
            RenameVals = DrawInputDialogue("Přejmenovat", inputLabels, inputVals, buttons);

            if (selectedItem.Substring(0, 1) == "\\" && RenameVals[0] == "Ok") // if folder is selected
            {
                DirectoryInfo myDir = new DirectoryInfo(path + selectedItem);
                myDir.MoveTo(path + '\\' + RenameVals[1]);
            }
            else if (RenameVals[0] == "Ok") // file is selected
            {
                FileInfo myFile = new FileInfo(path + '\\' + selectedItem.Substring(1, selectedItem.Length - 1));
                myFile.MoveTo(path + '\\' + RenameVals[1]);
            }

            return;

        }
    }
}
