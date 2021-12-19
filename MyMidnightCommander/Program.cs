using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using MyMidnightCommander.Window;
using MyMidnightCommander.Components.EditorComponents;
using MyMidnightCommander.Dialogues;

namespace MyMidnightCommander
{
    class Program
    {
        public static bool ProgramIsOn { get; set; } = true;

        static void Main(string[] args)
        {
            Editor.FilePath = @"C:\Users\Péťa\Desktop\Readme.txt";
            UI.Window = new EditorWindow();

            //UI.Window = new DirWindow();
            UI.UsedDialog = new DummyDialogue();

            /*Thread t = new Thread(HandleResize);
            t.Priority = ThreadPriority.Lowest;
            t.Start();*/

            Console.CursorVisible = false;
            while (ProgramIsOn)
            {
                UI.Draw();

                ConsoleKeyInfo info = Console.ReadKey();
                UI.HandleKey(info);
            }

            //t.Abort();
        }

        private static void HandleResize()
        {
            const int minWidth = 60;

            int consoleWidth = Console.WindowWidth;
            int consoleHeight = Console.WindowHeight;

            while (true)
            {
                if(consoleWidth != Console.WindowWidth || consoleHeight != Console.WindowHeight)
                {
                    if (Console.WindowWidth < minWidth)
                        Console.WindowWidth = minWidth;

                    consoleWidth = Console.WindowWidth;
                    consoleHeight = Console.WindowHeight;

                    if (consoleWidth >= minWidth)
                        UI.DrawResize();
                }
            }
        }
    }
}
