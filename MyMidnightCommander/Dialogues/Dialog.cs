using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMidnightCommander.Dialogues
{
    public abstract class Dialog
    {
        public virtual void ReadKey(ConsoleKeyInfo info)
        {
            
        }

        public virtual void Draw()
        {
            
        }
    }
}
