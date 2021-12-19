using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MyMidnightCommander.Window;

namespace MyMidnightCommander.Functions.EditorFunctions
{
    public class EditorSave
    {
        public static void Save(string filePath, List<string> myLinesOfFileText)
        {
            StreamWriter writer = new StreamWriter(filePath);
            foreach (string lineOfText in myLinesOfFileText)
            {
                writer.WriteLine(lineOfText);
            }
            writer.Close();
            UI.Window = new DirWindow();
        }
    }
}
