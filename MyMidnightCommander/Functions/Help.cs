using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMidnightCommander.Functions
{
    public class Help
    {
        public void HandleHelp()
        {
            InfoDialogue showError = new InfoDialogue();
            showError.Draw("Tato funkce ještě nebyla přidána!  ");
            Console.ReadKey();
        }
    }
}
