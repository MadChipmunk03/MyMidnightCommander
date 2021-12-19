using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMidnightCommander.Components.EditorComponents
{
    public static class EditorStaticticsBar
    {
        public static void Draw(string fileName, int CharNumber, int topRow, int selY, int rowCount)
        {
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.Write($"{fileName}         [----] {CharNumber} L:[ {topRow}+{selY - topRow}  {selY}/{rowCount}]".PadRight(Console.WindowWidth));
        }
    }
}