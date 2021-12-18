using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MyMidnightCommander.Components;
using MyMidnightCommander.Dialogues;

namespace MyMidnightCommander
{
    public class FunctionKeysLabels : IComponents
    {
        public string[] Args { get; set; }
        public bool IsActive { get; set; } = true;

        public List<string> FKLItems = new List<string>();
        public FunctionKeysLabels(string[] fklItem)
        {
            foreach (string item in fklItem)
                FKLItems.Add(item);
        }

        public void Draw()
        {
            Console.SetCursorPosition(0, Console.WindowHeight-1);
            Console.ResetColor();

            for(int i = 0; i < FKLItems.Count; i++)
            {
                Console.Write($" {i + 1}");
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(FKLItems[i].PadRight(Console.WindowWidth / (FKLItems.Count + 1)));
                Console.ResetColor();
            }
            for(int i = Console.CursorLeft; i < Console.WindowWidth; i++)
            {
                Console.Write(' ');
            }

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(0, 0);
        }

        public void ReadKey(ConsoleKeyInfo info)
        {
            if (info.Key == ConsoleKey.F1) //Help
            {
                UI.UsedDialog = new InfoDialogue("Tato funkce jeětě nebyla přidána  ");
                UI.DialogIsOn = true;
            }
            else if (info.Key == ConsoleKey.F8) //EXIT
            {
                Program.ProgramIsOn = false;
            }
        }

        /*public void HandleHelp()
        {
            DisplayInfoDialogue showError = new DisplayInfoDialogue("Tato funkce ještě nebyla přidána!  ");
            showError.DrawDialogue();
            Console.ReadKey();
        }*/

        /*
        public void HandleRename(string path, string selectedItem)
        {
            //parameters for dialogue
            string[] inputLabels = { "Zadej nový název: "};
            string[] inputVals = { selectedItem.Substring(1, selectedItem.Length - 1) };
            string[] buttons = { "Ok", "Cancel" };

            if (selectedItem == "\\..") // if parent dir is selected
            {
                DisplayInfoDialogue showError = new DisplayInfoDialogue("Nemůžeš přejmenovat adresář \\..  ");
                showError.DrawDialogue();
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

        public void HandleMove(string[] path, string[] selectedItem)
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
            CopyVals = DrawInputDialogue("Vyjmout", inputLabels, inputVals, buttons);

            //handle dialogue output
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
                    DisplayInfoDialogue myDialogue = new DisplayInfoDialogue($"položka {destinationDir.FullName} already exists!");
                    myDialogue.DrawDialogue();
                    Console.ReadKey();
                    return;
                }

                //handle the operation
                GoThroughDirectory(dir, sourceTrimLenght, destination, "move");

                DirectoryInfo destinationDir2 = new DirectoryInfo(destination + sourceParts[sourceParts.Length - 1]);
                if(!destinationDir2.Exists)
                    destinationDir2.Create();

                foreach (FileInfo file in dir.GetFiles())
                {
                    file.MoveTo(destination + file.FullName.Substring(sourceTrimLenght, file.FullName.Length - sourceTrimLenght));
                }
                dir.Delete();

                return;
            }
            return;
        }

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
            CopyVals = DrawInputDialogue("Kopírování", inputLabels, inputVals, buttons);

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
                    DisplayInfoDialogue myDialogue = new DisplayInfoDialogue($"položka {destinationDir.FullName} already exists!");
                    myDialogue.DrawDialogue();
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

        public void GoThroughDirectory(DirectoryInfo dir, int sourceTrimLenght, string destination, string method)
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
        }*/
    }
}
 