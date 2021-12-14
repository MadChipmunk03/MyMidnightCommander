using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using MyMidnightCommander.Components;
using MyMidnightCommander.Window;
using MyMidnightCommander.Dialogues;

namespace MyMidnightCommander
{
    public class UI
    {
        public static bool DialogIsOn {get; set;}
        public static Windows Window { get; set; }
        public static Dialog UsedDialog { get; set; }

        public static void Draw()
        {
            Window.Draw();
            UsedDialog.Draw();
        }

        public static void HandleKey(ConsoleKeyInfo info)
        {
            if(!DialogIsOn)
                Window.ReadKey(info);
            UsedDialog.ReadKey(info);
        }
    }
}
