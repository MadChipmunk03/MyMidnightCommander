using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MyMidnightCommander.Window;
using MyMidnightCommander.Functions.EditorFunctions;
using MyMidnightCommander.Dialogues.EditorDialogues;
using MyMidnightCommander.Components.EditorComponents;

namespace MyMidnightCommander.Components.EditorComponents
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
            Console.ForegroundColor = ConsoleColor.White;

            XOnLine = SelCharX1;
            if (XOnLine > myLinesOfFileText[SelCharY1 - 1].Length)
                XOnLine = myLinesOfFileText[SelCharY1 - 1].Length;

            int editorHeight = myLinesOfFileText.Count;
            if (editorHeight > Console.WindowHeight - 2)
                editorHeight = Console.WindowHeight - 2;

            Console.BackgroundColor = ConsoleColor.DarkBlue;

            if (SelCharY2 == 0) // if selection isn't selected
            {
                DrawBasic(editorHeight);
            }
            else
            {
                DrawSelection(editorHeight);
            }

            for (int i = editorHeight + TopRow; i < Console.WindowHeight - 2 + TopRow; i++)
            {
                Console.SetCursorPosition(0, i + 1);
                Console.Write("".PadRight(Console.WindowWidth));
            }

            DrawStats();
            EditorStaticticsBar.Draw(FilePath, SelCharX1, TopRow, SelCharY1, myLinesOfFileText.Count);
            Console.BackgroundColor = ConsoleColor.DarkBlue;

            Console.SetCursorPosition(XOnLine - ShiftLenght, SelCharY1 - TopRow);
            Console.CursorVisible = true;
        }
        public void DrawBasic(int editorHeight)
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

        public void DrawPartOfLine(string line, int start, int end)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            if (line == "")
            {
                Console.Write("".PadRight(Console.WindowWidth));
                return;
            }
            else
            {
                start -= ShiftLenght;
                if (start < 0)
                    start = 0;

                end -= ShiftLenght;

                char[] lettersOfLine = line.ToCharArray();
                // before selection
                for (int i = 0; i < start && i < lettersOfLine.Length; i++)
                {
                    Console.Write(lettersOfLine[i]);
                }
                Console.BackgroundColor = ConsoleColor.DarkCyan; // selection
                for (int i = start; i <= end && i < lettersOfLine.Length; i++)
                {
                    Console.Write(lettersOfLine[i]);
                }
                Console.BackgroundColor = ConsoleColor.DarkBlue; // after selection
                for (int i = end + 1; i < lettersOfLine.Length; i++)
                {
                    Console.Write(lettersOfLine[i]);
                }
                Console.Write("".PadRight(Console.WindowWidth - line.Length));
            }
        }

        public void DrawSelection(int editorHeight)
        {
            for (int i = 0; i < editorHeight; i++)
            {
                string selectedItem = "";

                if (myLinesOfFileText[i + TopRow].Length <= ShiftLenght)
                    selectedItem = "";
                else
                    selectedItem = myLinesOfFileText[i + TopRow].Substring(ShiftLenght, myLinesOfFileText[i + TopRow].Length - ShiftLenght);

                if (selectedItem.Length > Console.WindowWidth)
                    selectedItem = selectedItem.Substring(0, Console.WindowWidth);

                if (i + TopRow == SelCharY1 - 1 && SelCharY1 == SelCharY2) //it's on the same line
                {
                    if(SelCharX1 > SelCharX2) // selX1 is further
                        DrawPartOfLine(selectedItem, SelCharX2, XOnLine);
                    else // selX1 is closer
                        DrawPartOfLine(selectedItem, XOnLine, SelCharX2);
                }
                else if(i + TopRow == SelCharY1 - 1)
                {
                    if (SelCharY1 > SelCharY2) // selY1 is bottom
                        DrawPartOfLine(selectedItem, 0, SelCharX1);
                    else //selY1 is top
                        DrawPartOfLine(selectedItem, XOnLine, selectedItem.Length);
                }
                else if(i + TopRow == SelCharY2 - 1)
                {
                    if (SelCharY1 > SelCharY2) // selY2 is top
                        DrawPartOfLine(selectedItem, SelCharX2, selectedItem.Length);
                    else //selY2 is bottom
                        DrawPartOfLine(selectedItem, 0, SelCharX2);
                }
                else if (i + TopRow > SelCharY1 - 1 && i + TopRow < SelCharY2 - 1 || i + TopRow < SelCharY1 - 1 && i + TopRow > SelCharY2 - 1)
                {
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
                        Console.Write("".PadRight(Console.WindowWidth - selectedItem.Length));
                    }
                }
                else
                {
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
            if (info.Key == ConsoleKey.DownArrow) //SelCharY1 == cursorY; TopRow == minusLine
            {
                if (SelCharY1 < myLinesOfFileText.Count)
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

                    if (ShiftLenght > myLinesOfFileText[SelCharY1 - 1].Length)
                    {
                        ShiftLenght = myLinesOfFileText[SelCharY1 - 1].Length;
                        SelCharX1 = myLinesOfFileText[SelCharY1 - 1].Length;
                    }
                        
                }
            }
            else if (info.Key == ConsoleKey.UpArrow)
            {
                if (SelCharY1 != 1)
                {
                    SelCharY1--;
                    if (SelCharY1 - TopRow == 0)
                        TopRow--;

                    if (ShiftLenght > myLinesOfFileText[SelCharY1 - 1].Length)
                    {
                        ShiftLenght = myLinesOfFileText[SelCharY1 - 1].Length;
                        SelCharX1 = myLinesOfFileText[SelCharY1 - 1].Length;
                    }
                        
                }
            }
            else if (info.Key == ConsoleKey.RightArrow)
            {
                //if(SelCharX1 != myLinesOfFileText.Count)
                //{

                if (XOnLine < SelCharX1)
                    SelCharX1 = XOnLine;
                else
                    SelCharX1++;

                if (SelCharX1 - ShiftLenght == Console.WindowWidth)
                    ShiftLenght++;

                //}
            }
            else if (info.Key == ConsoleKey.LeftArrow)
            {
                if (SelCharX1 != 0 && XOnLine != 0)
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
                if (SelCharY2 == 0)
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
                //myLinesOfFileText = Selection.Delete(myLinesOfFileText, SelCharX1, SelCharY1, SelCharX2, SelCharY2);
            } // Smazat
            else if (info.Key == ConsoleKey.F8)
            {

            } // Přesuň
            else if (info.Key == ConsoleKey.F9)
            {

            } // Hl. nabídka
            else if (info.Key == ConsoleKey.F10)
            {
                if (ChangesWereMade)
                {
                    UI.UsedDialog = new EditorSaveDialogue(FilePath, myLinesOfFileText);
                    UI.DialogIsOn = true;
                }
                else
                {
                    UI.Window = new DirWindow();
                }
                
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
            foreach (string lineOfText in myLinesOfFileText)
            {
                writer.WriteLine(lineOfText);
            }
            writer.Close();
            ChangesWereMade = false;
        }
    }
}