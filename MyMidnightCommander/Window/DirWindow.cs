using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMidnightCommander.Dialogues;
using MyMidnightCommander.Components.DirComponents;

namespace MyMidnightCommander.Window
{
    public class DirWindow : Windows
    {
        public DirWindow()
        {
            MyDirectory rightDirectory = new MyDirectory(MyDirectory.RightDirectory);
            components.Add(rightDirectory);

            MyDirectory leftDirectory = new MyDirectory(MyDirectory.LeftDirectory);
            components.Add(leftDirectory);

            string[] fklLabels = { "POMOC!", "Make", "Rename" , "Move" , "Copy" , "Delete" , "Open" , "EXIT" };
            FunctionKeysLabels myFKL = new FunctionKeysLabels(fklLabels);
            components.Add(myFKL);

            string[] menuBarItems = { "Left", "File", "Command", "Right" };
            MenuBar myMenuBar = new MenuBar(menuBarItems);
            components.Add(myMenuBar);
        }
    }
}
