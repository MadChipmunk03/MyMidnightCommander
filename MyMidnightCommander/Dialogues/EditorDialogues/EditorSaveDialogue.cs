using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMidnightCommander.Dialogues.DialogTemplates;
using MyMidnightCommander.Functions.EditorFunctions;
using MyMidnightCommander.Window;

namespace MyMidnightCommander.Dialogues.EditorDialogues
{
    public class EditorSaveDialogue : Dialog
    {
        private string Title { get; set; } = "Uložit?";
        private string Text { get; set; } = "Soubor není uložený. Přejete si ho uložit?";
        private string[] Buttons { get; set; } = { "Uložit", "Zahodit", "Zpět" };
        private string Path { get; set; }
        private int BtnsInRowTotalLenght { get; set; } = 0;
        private int SelectedItem { get; set; } = 0;
        private List<string> LinesOfText = new List<string>();

        public EditorSaveDialogue(string path, List<string> linesOfText)
        {
            Path = path;
            LinesOfText = linesOfText;

            //formatting Buttons + getting their lenght when lined up in row
            BtnsInRowTotalLenght = 0;
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i] = $" [{Buttons[i]}] "; //formatting
                BtnsInRowTotalLenght += Buttons[i].Length; // Buttons lenght in row
            }
        }

        public override void Draw()
        {
            ChoiceBoxTemplate.Draw(Title, Text, Buttons, false, BtnsInRowTotalLenght, SelectedItem);
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
            }
            else if (info.Key == ConsoleKey.Enter) //last option isn't selected
            {
                if (SelectedItem == 0)
                {
                    EditorSave.Save(Path, LinesOfText);
                }
                else if (SelectedItem == 1)
                {
                    UI.Window = new DirWindow();
                }
                else if (SelectedItem == 2);

                UI.UsedDialog = new DummyDialogue();
            }
        }
    }
}
