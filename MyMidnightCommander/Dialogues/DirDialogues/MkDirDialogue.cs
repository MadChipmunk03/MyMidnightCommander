using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MyMidnightCommander.Dialogues.DialogTemplates;
using MyMidnightCommander.Functions;

namespace MyMidnightCommander.Dialogues.DirDialogues
{
    public class MkDirDialogue : Dialog
    {
        private string InputLabel { get; set; } = "Zadejte název složky";
        private string InputVal { get; set; } = "Nová složka";
        private string[] Buttons { get; set; } = { "OK", "Cancel" };
        private string Path { get; set; }
        private int BtnsInRowTotalLenght { get; set; } = 0;
        public int SelectedItem { get; set; } = 0;

        public MkDirDialogue(string path)
        {
            //formatting Buttons + getting their lenght when lined up in row
            BtnsInRowTotalLenght = 0;
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i] = $" [{Buttons[i]}] "; //formatting
                BtnsInRowTotalLenght += Buttons[i].Length; // Buttons lenght in row
            }
            Path = path;
        }

        public override void Draw()
        {
            string[] InputLabels = { InputLabel };
            string[] InputVals = { InputVal };
            InputDialogTemplate.Draw("Vytvořit složku", InputLabels, InputVals, Buttons, BtnsInRowTotalLenght, SelectedItem);
        }

        public override void ReadKey(ConsoleKeyInfo info)
        {
            if (info.Key == ConsoleKey.Enter)
                CreateDir();
            else if (info.Key == ConsoleKey.Backspace)
            {
                if (InputVal.Length > 0)
                    InputVal = InputVal.Substring(0, InputVal.Length - 1);
            }
            else if (info.Key == ConsoleKey.Tab)
            {
                SelectedItem++;
                if (SelectedItem > Buttons.Length)
                    SelectedItem = 0;
            }
            else if (info.Key == ConsoleKey.Escape)
                UI.UsedDialog = new DummyDialogue();
            else if (info.Key != ConsoleKey.F2) ; // filter out
            else
                InputVal += info.KeyChar;
        }

        private void CreateDir()
        {
            if (SelectedItem == 1)
            {
                DirectoryInfo myDirInfo = new DirectoryInfo(Path + "\\" + InputVal);
                myDirInfo.Create();
                UI.UsedDialog = new DummyDialogue();
            }
            else if (SelectedItem == 2)
            {
                UI.UsedDialog = new DummyDialogue();
            }
        }
    }
}
