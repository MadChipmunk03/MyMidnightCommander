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

        

        
        
        

        
        }*/
    }
}
 