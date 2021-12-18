using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMidnightCommander.Components;

namespace MyMidnightCommander
{
    public class MenuBar : IComponents
    {
        public string[] Args { get; set; }
        public bool IsActive { get; set; }
        public List<string> MenuBarItems = new List<string>();

        public MenuBar(string[] menuBarItems)
        {
            foreach (string item in menuBarItems)
                MenuBarItems.Add(item);
        }

        public void Draw()
        {
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;
            string menuBarText = "";

            for(int i = 0; i < MenuBarItems.Count; i++)
            {
                string item = "   " + MenuBarItems[i] + "   ";
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

        public void ReadKey(ConsoleKeyInfo info)
        {

        }
    }
}
