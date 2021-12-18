﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMidnightCommander.Dialogues.DialogTemplates;
using MyMidnightCommander.Functions;

namespace MyMidnightCommander.Dialogues.DirDialogues
{
    public class CopyDialogue : Dialog
    {
        private string InputLabel { get; set; }
        private string[] Buttons { get; set; } = { "OK", "Cancel" };
        private string Path { get; set; }
        private int BtnsInRowTotalLenght { get; set; } = 0;
        private bool IsFolder { get; set; }
        public int SelectedItem { get; set; } = 0;
        public static string SourcePath { get; set; }
        public static string DestinationPath { get; set; }

        public CopyDialogue()
        {
            string[] SPParts = SourcePath.Split('\\');
            if (SPParts[SPParts.Length - 1].Contains('.'))
            {
                InputLabel = $"Kopírovat soubor \"{SPParts[SPParts.Length - 1].Substring(1, SPParts[SPParts.Length - 1].Length - 1)}\" na:";
                IsFolder = false;
            }
            else
            {
                InputLabel = $"Kopírovat složku \"{SPParts[SPParts.Length - 1]}\" na:";
                IsFolder = true;
            }

            //formatting Buttons + getting their lenght when lined up in row
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i] = $" [{Buttons[i]}] "; //formatting
                BtnsInRowTotalLenght += Buttons[i].Length; // Buttons lenght in row
            }
        }

        public override void Draw()
        {
            string[] InputLabels = { InputLabel };
            string[] InputVals = { DestinationPath };
            InputDialogTemplate.Draw("Kopírovat položku", InputLabels, InputVals, Buttons, BtnsInRowTotalLenght, SelectedItem);
        }

        public override void ReadKey(ConsoleKeyInfo info)
        {
            if (info.Key == ConsoleKey.Enter)
                Copy();
            else if (info.Key == ConsoleKey.Backspace)
            {
                if (DestinationPath.Length > 0)
                    DestinationPath = DestinationPath.Substring(0, DestinationPath.Length - 1);
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
                DestinationPath += info.KeyChar;
        }

        private void Copy()
        {
            if (SelectedItem == 1)
            {
                string[] SPParts = SourcePath.Split('\\');
                CopyFunc.HandleCopy(SourcePath, DestinationPath + '\\' + SPParts[SPParts.Length - 1], IsFolder);
                UI.UsedDialog = new DummyDialogue();
            }
            else if (SelectedItem == 2)
            {
                UI.UsedDialog = new DummyDialogue();
            }
        }
    }
}
