using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMidnightCommander.Dialogues;
using MyMidnightCommander.Functions;

namespace MyMidnightCommander.Dialogues
{
    public class DeleteChoiceBox : Dialog
    {
        private string Title { get; set; }
        private string Text { get; set; }
        private string[] Buttons { get; set; }
        private string Path { get; set; }
        private bool IsFolder { get; set; }
        private int BtnsInRowTotalLenght { get; set; } = 0;
        public int SelectedItem { get; set; } = 0;

        public DeleteChoiceBox(string title, string text, string[] buttons, string method, string path, bool isFolder)
        {
            Title = title;
            Text = text;
            Buttons = buttons;

            //formatting Buttons + getting their lenght when lined up in row
            BtnsInRowTotalLenght = 0;
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i] = $" [{Buttons[i]}] "; //formatting
                BtnsInRowTotalLenght += Buttons[i].Length; // Buttons lenght in row
            }

            Path = path;
            IsFolder = isFolder;
        }

        public override void Draw()
        {
            ChoiceBoxTemplate.Draw(Title, Text, Buttons, BtnsInRowTotalLenght, SelectedItem);
        }

        public override void ReadKey(ConsoleKeyInfo info)
        {
            if (info.Key == ConsoleKey.Tab)
            {
                SelectedItem += 1;
                if (SelectedItem >= Buttons.Length)
                    SelectedItem = 0;
            }
            else if (info.Key == ConsoleKey.Escape)
            {
                UI.UsedDialog = new DummyDialogue();
                UI.DialogIsOn = false;
            }
            else if (info.Key == ConsoleKey.Enter) //last option isn't selected ok!!
            {
                if (SelectedItem == 0)
                    DeleteFunc.HandleDelete(Path, IsFolder);
                else if (SelectedItem == 1);

                UI.UsedDialog = new DummyDialogue();
                UI.DialogIsOn = false;
            }
        }
    }
}
