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
        public static bool UpdateWindow { get; set; }
        public static Windows Window { get; set; }
        public static Dialog UsedDialog { get; set; }

        public static void Draw()
        {
            if(!DialogIsOn)
                Window.Draw();
            else
                UsedDialog.Draw();
        }

        public static void HandleKey(ConsoleKeyInfo info)
        {
            if(!DialogIsOn)
                Window.ReadKey(info);
            else
                UsedDialog.ReadKey(info);
        }

        public static void DrawResize()
        {
            Window.Draw();
            if(DialogIsOn)
                UsedDialog.Draw();
        }
    }
}
