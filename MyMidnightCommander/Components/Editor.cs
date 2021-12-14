using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MyMidnightCommander.Window;
using MyMidnightCommander.EditorFolder.Functions;
using MyMidnightCommander.Dialogues;

namespace MyMidnightCommander.Components
{
    public class Editor : IComponents
    {
        public string[] Args { get; set; }
        public bool IsActive { get; set; }
        public static string FilePath { get; set; }
        private List<string> myLinesOfFileText = new List<string>();

        private int SelCharX1 { get; set; } = 0;
        private int XOnLine { get; set; }
        private int SelCharY1 { get; set; } = 1;
        private int SelCharX2 { get; set; }
        private int SelCharY2 { get; set; }
        private int TopRow { get; set; } = 0;
        private int ShiftLenght { get; set; } = 0;
        private string SelectedText { get; set; }
        private bool ChangesWereMade { get; set; } = false;

        public Editor()
        {
            GetLinesOfFile();
        }

        private void GetLinesOfFile()
        {
            StreamReader reader = new StreamReader(FilePath);
            while (!reader.EndOfStream)
            {
                myLinesOfFileText.Add(reader.ReadLine());
            }
            reader.Close();
        }

        public void Draw()
        {
            XOnLine = SelCharX1;

            int editorHeight = myLinesOfFileText.Count;
            if (editorHeight > Console.WindowHeight - 2)
                editorHeight = Console.WindowHeight - 2;

            Console.BackgroundColor = ConsoleColor.DarkBlue;

            if (SelCharY2 == 0) // if selection isn't selected
            {
                for (int i = 0; i < editorHeight; i++)
                {
                    string selectedItem = "";
                    if (myLinesOfFileText[i + TopRow].Length <= ShiftLenght)
                    {
                        selectedItem = "";
                    }
                    else
                        selectedItem = myLinesOfFileText[i + TopRow].Substring(ShiftLenght, myLinesOfFileText[i + TopRow].Length - ShiftLenght);

                    if (i == SelCharY1 - 1 && SelCharX1 > selectedItem.Length)
                    {
                        XOnLine = selectedItem.Length;
                    }

                    Console.SetCursorPosition(0, i + 1);
                    if (selectedItem.Length > Console.WindowWidth)
                        Console.Write(selectedItem.Substring(0, Console.WindowWidth));
                    else
                        Console.Write(selectedItem.PadRight(Console.WindowWidth));
                }
            }
            else
            {
                DrawSelection(editorHeight);
            }

            for(int i = editorHeight + TopRow; i < Console.WindowHeight - 2 + TopRow; i++)
            {
                Console.SetCursorPosition(0, i + 1);
                Console.Write("".PadRight(Console.WindowWidth));
            }

            DrawStats();

            Console.SetCursorPosition(XOnLine - ShiftLenght, SelCharY1 - TopRow);
            Console.CursorVisible = true;
        }

        public void DrawSelection(int editorHeight)
        {
            for (int i = 0; i < editorHeight; i++)
            {
                string selectedItem = "";

                if (i + TopRow >= SelCharY1 - 1 && i + TopRow <= SelCharY2 - 1 || i + TopRow <= SelCharY1 - 1 && i + TopRow >= SelCharY2 - 1)
                    Console.BackgroundColor = ConsoleColor.DarkCyan;

                if (myLinesOfFileText[i + TopRow].Length <= ShiftLenght)
                    selectedItem = "";
                else
                    selectedItem = myLinesOfFileText[i + TopRow].Substring(ShiftLenght, myLinesOfFileText[i + TopRow].Length - ShiftLenght);

                if (i == SelCharY1 - 1 && SelCharX1 > selectedItem.Length)
                    XOnLine = selectedItem.Length;

                Console.SetCursorPosition(0, i + 1);

                if (selectedItem.Length > Console.WindowWidth)
                    Console.Write(selectedItem.Substring(0, Console.WindowWidth));
                else
                {
                    Console.Write(selectedItem);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.Write("".PadRight(Console.WindowWidth));
                }

                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
        }

        public void DrawStats() // remove
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 0);
            Console.Write("SelX:" + SelCharX1);
            Console.SetCursorPosition(9, 0);
            Console.Write("SelY:" + SelCharY1);
            Console.SetCursorPosition(18, 0);
            Console.Write("XonL:" + XOnLine);
            Console.SetCursorPosition(27, 0);
            Console.Write("TopR:" + TopRow);
            Console.SetCursorPosition(36, 0);
            Console.Write("SlX2:" + SelCharX2);
            Console.SetCursorPosition(45, 0);
            Console.Write("SlY2:" + SelCharY2);
            Console.SetCursorPosition(54, 0);
            Console.Write("ShLe:" + ShiftLenght);
            Console.SetCursorPosition(63, 0);
            Console.Write("CoWh:" + Console.WindowWidth);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
        }

        public void ReadKey(ConsoleKeyInfo info)
        {
            if(info.Key == ConsoleKey.DownArrow)
            {
                if(SelCharY1 < myLinesOfFileText.Count)
                {
                    if (SelCharY1 - TopRow < Console.WindowHeight - 2)
                    {
                        SelCharY1++;
                    }
                    else
                    {
                        SelCharY1++;
                        TopRow++;
                    }
                }
            }
            else if (info.Key == ConsoleKey.UpArrow)
            {
                if (SelCharY1 != 1)
                {
                    SelCharY1--;
                    if(SelCharY1 - TopRow == 0)
                        TopRow--;
                }
            }
            else if (info.Key == ConsoleKey.RightArrow)
            {
                //if(SelCharX1 != myLinesOfFileText.Count)
                //{

                if (XOnLine < SelCharX1)
                        SelCharX1 = XOnLine + 1;
                else
                    SelCharX1++;

                if (SelCharX1 - ShiftLenght == Console.WindowWidth)
                    ShiftLenght++;

                //}
            }
            else if (info.Key == ConsoleKey.LeftArrow)
            {
                if(SelCharX1 != 0 && XOnLine != 0)
                {
                    if (XOnLine < SelCharX1)
                        SelCharX1 = XOnLine - 1;
                    else
                        SelCharX1--;

                    if (SelCharX1 - ShiftLenght + 1 == 0)
                        ShiftLenght--;
                }
                /*else
                {
                    myLinesOfFileText.RemoveAt(SelCharY1 - 1);
                }*/
            }
            else if (info.Key == ConsoleKey.Backspace)
            {
                if (XOnLine != 0)
                {
                    myLinesOfFileText[SelCharY1 - 1] = myLinesOfFileText[SelCharY1 - 1].Substring(0, XOnLine - 1) + myLinesOfFileText[SelCharY1 - 1].Substring(XOnLine, myLinesOfFileText[SelCharY1 - 1].Length - XOnLine);
                    SelCharX1 = XOnLine - 1;
                    if (SelCharX1 - ShiftLenght == 1 && ShiftLenght != 0)
                        ShiftLenght--;
                    ChangesWereMade = true;
                }
            }
            else if (info.Key == ConsoleKey.F1)
            {

            } //Nápověda
            else if (info.Key == ConsoleKey.F2)
            {
                Save();
            } // Uložit
            else if (info.Key == ConsoleKey.F3)
            {
                if(SelCharY2 == 0)
                {
                    SelCharX2 = XOnLine;
                    SelCharY2 = SelCharY1;
                }
                else
                {
                    SelCharX2 = 0;
                    SelCharY2 = 0;
                }
                
            } // označ
            else if (info.Key == ConsoleKey.F4)
            {

            } // Nahraď
            else if (info.Key == ConsoleKey.F5)
            {
                
            } // Přesuň
            else if (info.Key == ConsoleKey.F6)
            {

            } // Hledat
            else if (info.Key == ConsoleKey.F7)
            {
                myLinesOfFileText = Selection.Delete(myLinesOfFileText, SelCharX1, SelCharY1, SelCharX2, SelCharY2);
            } // Smazat
            else if (info.Key == ConsoleKey.F8)
            {

            } // Přesuň
            else if (info.Key == ConsoleKey.F9)
            {
                
            } // Hl. nabídka
            else if (info.Key == ConsoleKey.F10)
            {
                UI.Window = new DirWindow();
                
            } // odejít
            else if (info.Key == ConsoleKey.Enter)
            {
                string nextLineString = myLinesOfFileText[SelCharY1 - 1].Substring(SelCharX1, myLinesOfFileText[SelCharY1 - 1].Length - SelCharX1);
                myLinesOfFileText[SelCharY1 - 1] = myLinesOfFileText[SelCharY1 - 1].Substring(0, SelCharX1);
                myLinesOfFileText.Insert(SelCharY1, nextLineString);
                SelCharY1++;
                SelCharX1 = 0;
            }
            else //(info.KeyChar <= 90 && info.KeyChar >= 48) || (info.KeyChar <= 111 && info.KeyChar >= 106)
            {
                myLinesOfFileText[SelCharY1 - 1] = myLinesOfFileText[SelCharY1 - 1].Substring(0, XOnLine) + info.KeyChar + myLinesOfFileText[SelCharY1 - 1].Substring(XOnLine, myLinesOfFileText[SelCharY1 - 1].Length - XOnLine);
                SelCharX1++;
                if (SelCharX1 - ShiftLenght == Console.WindowWidth)
                    ShiftLenght++;
                ChangesWereMade = true;
            }
        }

        public void Save()
        {
            StreamWriter writer = new StreamWriter(FilePath);
            foreach(string lineOfText in myLinesOfFileText)
            {
                writer.WriteLine(lineOfText);
            }
            writer.Close();
            UI.Window = new DirWindow();
        }
    }
}
