using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMidnightCommander.Components;

namespace MyMidnightCommander.Window
{
    public class EditorWindow : Windows
    {
        public EditorWindow()
        {
            

            MenuBar myMenuBar = new MenuBar();
            //myMenuBar.MenuBarItems.Add("Soubor");
            //myMenuBar.MenuBarItems.Add("Nastavení");
            //myMenuBar.MenuBarItems.Add("neco dalsiho");
            //myMenuBar.MenuBarItems.Add("neco2");
            components.Add(myMenuBar);

            FunctionKeysLabels myFKL = new FunctionKeysLabels();
            myFKL.FKLItems.Add("Nápověda");
            myFKL.FKLItems.Add("Uložit");
            myFKL.FKLItems.Add("Označ");
            myFKL.FKLItems.Add("Nahraď");
            myFKL.FKLItems.Add("Kopie");
            myFKL.FKLItems.Add("Přesun");
            myFKL.FKLItems.Add("Hledat");
            myFKL.FKLItems.Add("Smazat");
            myFKL.FKLItems.Add("Hl. nabídka");
            myFKL.FKLItems.Add("Konec");
            components.Add(myFKL);

            Editor myEditor = new Editor();
            components.Add(myEditor);
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
