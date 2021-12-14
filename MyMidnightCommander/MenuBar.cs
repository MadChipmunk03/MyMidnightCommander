using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMidnightCommander
{
    public static class MenuBar
    {
        public static void Draw(string[] tabs)
        {
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;
            string menuBarText = "";

            for(int i = 0; i < tabs.Length; i++)
            {
                string item = "   " + tabs[i] + "   ";
                if((menuBarText + item).Length > Console.WindowWidth)
                {
                    break;
                }
                menuBarText += item;
            }
            menuBarText = menuBarText.PadRight(Console.WindowWidth, ' ');
            Console.Write(menuBarText);
            Console.ResetColor();

        }
    }
}
