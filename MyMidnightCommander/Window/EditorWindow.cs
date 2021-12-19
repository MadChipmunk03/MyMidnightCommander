using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMidnightCommander.Components.EditorComponents;

namespace MyMidnightCommander.Window
{
    public class EditorWindow : Windows
    {
        public EditorWindow()
        {
            string[] fklItems = { "Nápověda" , "Uložit" , "Označ" , "Nahraď" , "Kopie" , "Přesun" , "Hledat" , "Smazat" , "Hl. nabídka" , "Konec" };
            FunctionKeysLabels myFKL = new FunctionKeysLabels(fklItems);
            components.Add(myFKL);

            Editor myEditor = new Editor();
            components.Add(myEditor);
        }
    }
}
