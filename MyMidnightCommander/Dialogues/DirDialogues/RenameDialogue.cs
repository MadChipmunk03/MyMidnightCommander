using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMidnightCommander.Dialogues.DialogTemplates;
using MyMidnightCommander.Functions.DirFunctions;

namespace MyMidnightCommander.Dialogues.DirDialogues
{
    public class RenameDialogue : Dialog
    {
        private string InputLabel { get; set; }
        private string[] Buttons { get; set; } = { "OK", "Cancel" };
        private string Path { get; set; }
        private int BtnsInRowTotalLenght { get; set; } = 0;
        private bool IsFolder { get; set; }
        public int SelectedItem { get; set; } = 0;
        public string SourcePath { get; set; }
        public static string NewName { get; set; }

        public RenameDialogue(string sourcePath)
        {
            SourcePath = sourcePath;

            string[] SPParts = SourcePath.Split('\\');
            if (SPParts[SPParts.Length - 1].Contains('.'))
            {
                InputLabel = $"Přejmenovat soubor \"{SPParts[SPParts.Length - 1]}\" na:";
                IsFolder = false;
            }
            else
            {
                InputLabel = $"Přejmenovat složku \"{SPParts[SPParts.Length - 1]}\" na:";
                IsFolder = true;
            }

            //formatting Buttons + getting their lenght when lined up in row
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i] = $" [{Buttons[i]}] "; //formatting
                BtnsInRowTotalLenght += Buttons[i].Length; // Buttons lenght in row
            }

            NewName = SPParts[SPParts.Length - 1];
        }

        public override void Draw()
        {
            string[] InputLabels = { InputLabel };
            string[] InputVals = { NewName };
            InputDialogTemplate.Draw("Přesunout položku", InputLabels, InputVals, Buttons, BtnsInRowTotalLenght, SelectedItem);
        }

        public override void ReadKey(ConsoleKeyInfo info)
        {
            if (info.Key == ConsoleKey.Enter)
                Rename();
            else if (info.Key == ConsoleKey.Backspace)
            {
                if (NewName.Length > 0)
                    NewName = NewName.Substring(0, NewName.Length - 1);
            }
            else if (info.Key == ConsoleKey.Tab)
            {
                SelectedItem++;
                if (SelectedItem > Buttons.Length)
                    SelectedItem = 0;
            }
            else if (info.Key == ConsoleKey.Escape)
                UI.UsedDialog = new DummyDialogue();
            else if (info.Key == ConsoleKey.F3) ; // filter out
            else
                NewName += info.KeyChar;
        }

        private void Rename()
        {
            if (SelectedItem == 1)
            {
                string destination = "";
                string[] SPParts = SourcePath.Split('\\');
                for (int i = 0; i < SPParts.Length - 1; i++)
                    destination += SPParts[i];

                if(IsFolder)
                    RenameFunc.HandleRename(SourcePath, NewName, IsFolder);
                else
                    RenameFunc.HandleRename(SourcePath, NewName, IsFolder);
                UI.UsedDialog = new DummyDialogue();
            }
            else if (SelectedItem == 2)
            {
                UI.UsedDialog = new DummyDialogue();
            }
        }
    }
}
