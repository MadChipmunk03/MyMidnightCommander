using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMidnightCommander.Dialogues;

namespace MyMidnightCommander.Window
{
    public class DirWindow : Windows
    {
        public bool ComponentsAreActive { get; set; } = true;

        private List<IDialog> myDialogues = new List<IDialog>();
        public DirWindow()
        {
            //Dir components
            MyDirectory rightDirectory = new MyDirectory(MyDirectory.RightDirectory);
            components.Add(rightDirectory);

            MyDirectory leftDirectory = new MyDirectory(MyDirectory.LeftDirectory);
            components.Add(leftDirectory);

            FunctionKeysLabels myFKL = new FunctionKeysLabels();
            myFKL.FKLItems.Add("POMOC!");
            myFKL.FKLItems.Add("MkDir");
            myFKL.FKLItems.Add("Rename");
            myFKL.FKLItems.Add("Move");
            myFKL.FKLItems.Add("Copy");
            myFKL.FKLItems.Add("Delete");
            myFKL.FKLItems.Add("Open");
            myFKL.FKLItems.Add("EXIT");
            components.Add(myFKL);

            MenuBar myMenuBar = new MenuBar();
            myMenuBar.MenuBarItems.Add("Left");
            myMenuBar.MenuBarItems.Add("File");
            myMenuBar.MenuBarItems.Add("Command");
            myMenuBar.MenuBarItems.Add("Right");
            components.Add(myMenuBar);

            //dialogues
            /*InfoDialogue myInfoDialogue = new InfoDialogue();
            myDialogues.Add(myInfoDialogue);*/
            ChoiceBox myChoiceBox = new ChoiceBox();
            myDialogues.Add(myChoiceBox);
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void ReadKey(ConsoleKeyInfo info)
        {
             base.ReadKey(info);
        }
    }
}
