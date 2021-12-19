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
    }
}
 