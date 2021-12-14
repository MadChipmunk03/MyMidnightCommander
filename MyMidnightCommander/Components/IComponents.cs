using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMidnightCommander.Components
{
    public interface IComponents
    {
        string[] Args { get; set; }
        bool IsActive { get; set; }

        void ReadKey(ConsoleKeyInfo info);
        void Draw();
    }
}
