using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMidnightCommander.Dialogues
{
    public interface IDialog
    {
        void Draw();
        bool ReadKey(ConsoleKeyInfo info);
    }
}
