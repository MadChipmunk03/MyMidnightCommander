using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMidnightCommander.Components;

namespace MyMidnightCommander.Window
{
    public abstract class Windows
    {
        protected List<IComponents> components = new List<IComponents>();

        public virtual void ReadKey(ConsoleKeyInfo info)
        {
            foreach (IComponents item in components)
            {
                item.ReadKey(info);
            }
        }

        public virtual void Draw()
        {
            foreach(IComponents item in components)
            {
                item.Draw();
            }
        }
    }
}
