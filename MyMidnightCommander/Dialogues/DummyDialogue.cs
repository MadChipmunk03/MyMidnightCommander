using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMidnightCommander.Dialogues
{
    public class DummyDialogue : Dialog
    {
        public DummyDialogue()
        {
            UI.DialogIsOn = false;
            UI.Window.Draw();
        }
    }
}
