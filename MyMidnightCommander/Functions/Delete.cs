using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyMidnightCommander.Functions
{
    public class Delete
    {
        public void HandleDelete(string path, string selectedItem)
        {
            string[] buttons = { "Ok", "Cancel" };

            string chosenOption = "";
            if (selectedItem == "\\..") // if parent dir is selected
            {
                DisplayInfoDialogue showError = new DisplayInfoDialogue("Nemůžeš odstranit adresář \\..  ");
                showError.DrawDialogue();
                Console.ReadKey();
            }
            else if (selectedItem.Substring(0, 1) == "\\") // if folder is selected
            {
                chosenOption = DrawChoiceDialogue("Smazat", $"smazat složku „{selectedItem}“?", buttons);
                if (chosenOption == "Ok")
                {
                    DirectoryInfo dir = new DirectoryInfo(path + selectedItem);
                    GoThroughDirectory(dir, 0, "", "delete");
                    foreach (FileInfo file in dir.GetFiles())
                        file.Delete();
                    dir.Delete();
                }
            }
            else // file is selected
            {
                chosenOption = DrawChoiceDialogue("Smazat", $"smazat soubor „{selectedItem.Substring(1, selectedItem.Length - 1)}“?", buttons);
                if (chosenOption == "Ok")
                {
                    FileInfo myFile = new FileInfo(path + '\\' + selectedItem.Substring(1, selectedItem.Length - 1));
                    myFile.Delete();
                }

            }
            return;
        }
    }
}
