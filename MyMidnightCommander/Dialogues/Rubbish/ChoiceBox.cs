using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMidnightCommander.Dialogues;
using MyMidnightCommander.Functions;

namespace MyMidnightCommander.Dialogues
{
    public class ChoiceBox
    {
        public static string Method { get; set; }
        public static bool IsActive { get; set; }
        private static string Title { get; set; }
        private static string Text { get; set; }
        private static string[] Buttons { get; set; }
        private static string Path { get; set; }
        private static bool IsFolder { get; set; }
        private static int BtnsInRowTotalLenght { get; set; } = 0;

        public int SelectedItem { get; set; } = 0;

        public static void ChoiceBoxConstructor(string title, string text, string[] buttons, string method, string path, bool isFolder)
        {
            Title = title;
            Text = text;
            Buttons = buttons;

            //formatting Buttons + getting their lenght when lined up in row
            BtnsInRowTotalLenght = 0;
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i] = $" [{Buttons[i]}] "; //formatting
                BtnsInRowTotalLenght += Buttons[i].Length; // Buttons lenght in row
            }

            Method = method;
            Path = path;
            IsFolder = isFolder;
        }

        public void Draw()
        {
            int windowWidth = Text.Length + 6;
            int windowHeight = 6 + 1;
            
            if (Text.Length < BtnsInRowTotalLenght)
                windowWidth = BtnsInRowTotalLenght + 6;

            List<string> textLines = new List<string>();

            if (windowWidth > Console.WindowWidth / 2)
            {
                string[] TextWords = Text.Split(' ');

                int numberOfLines = 0;

                string lineOfText = "";
                foreach (string word in TextWords)
                {
                    if ((lineOfText + word + " ").Length < Console.WindowWidth / 2 - 9)
                        lineOfText += word + " ";
                    else
                    {
                        textLines.Add(lineOfText.Substring(0, lineOfText.Length - 1));
                        numberOfLines += 1;
                        lineOfText = word;
                    }
                }
                textLines.Add(lineOfText);
                windowHeight += numberOfLines;
                windowWidth = Console.WindowWidth / 2 - 6;
            }
            else
                textLines.Add(Text);

            int startX = Console.WindowWidth / 2 - (windowWidth / 2);
            int startY = Console.WindowHeight / 2 - (windowHeight / 2) - 2; //-2 for esthetics

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;

            Console.CursorVisible = false;
            Console.SetCursorPosition(startX, startY);

            for (int dRow = 0; dRow < windowHeight; dRow++)
            {
                Console.SetCursorPosition(startX, startY + dRow);
                if (dRow == 0 || dRow == windowHeight - 1)
                {
                    for (int dChar = 0; dChar < windowWidth; dChar++)
                        Console.Write(' ');
                }
                else if (dRow == windowHeight - 3) // Buttons
                {
                    Console.Write(" │" + "".PadRight((windowWidth - BtnsInRowTotalLenght) / 2 - 2 + windowWidth % 2));
                    for (int i = 0; i < Buttons.Length; i++)
                    {
                        if (i == SelectedItem)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }

                        Console.Write(Buttons[i]);
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write("".PadRight((windowWidth - BtnsInRowTotalLenght) / 2 - 2) + "│ ");
                }
                else if (dRow == 1)
                {
                    Console.Write(" ┌");
                    for (int dChar = 2; dChar < windowWidth / 2 - (Title.Length / 2) - 1 - (Title.Length % 2) + (windowWidth % 2); dChar++)
                        Console.Write('─');
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($" {Title} ");
                    Console.ForegroundColor = ConsoleColor.White;
                    for (int dChar = 2; dChar < windowWidth / 2 - (Title.Length / 2) - 1; dChar++)
                        Console.Write('─');
                    Console.Write("┐ ");
                }
                else if (dRow > 1 && dRow < windowHeight - 4)
                {
                    Console.Write(" │ ");
                    Console.Write(textLines[dRow - 2].PadRight(windowWidth - 6));
                    Console.Write(" │ ");
                }
                else if (dRow == windowHeight - 2) // Bottom
                {
                    Console.Write(" └");
                    for (int dChar = 2; dChar < windowWidth - 2; dChar++)
                        Console.Write('─');
                    Console.Write("┘ ");
                }
                else if (dRow == windowHeight - 4) // Separator between Buttons and inputs
                {
                    Console.Write(" ├");
                    Console.Write("".PadRight(windowWidth - 4, '─'));
                    Console.Write("┤ ");
                }
            }

            Console.SetCursorPosition(0, 0);
            Console.Write(SelectedItem);

            Console.SetCursorPosition(startX, startY);
            Console.CursorVisible = false;
        }

        public bool ReadKey(ConsoleKeyInfo info)
        {
            if (!IsActive)
                return true;

            if (info.Key == ConsoleKey.Tab)
            {
                SelectedItem += 1;
                if (SelectedItem >= Buttons.Length)
                    SelectedItem = 0;
            }
            else if (info.Key == ConsoleKey.Escape)
            {
                IsActive = false;
                return true;
            }
            else if (info.Key == ConsoleKey.K && SelectedItem != Buttons.Length - 1) //last option isn't selected
            {
                if(Method == "Delete")
                {
                    if (SelectedItem == 0)
                        DeleteFunc.HandleDelete(Path, IsFolder);
                    else if (SelectedItem == 1) ;
                }
                    
                if(Method == "EditorSave" && SelectedItem == 1)
                {
                    if (SelectedItem == 0)
                        DeleteFunc.HandleDelete(Path, IsFolder);
                    else if (SelectedItem == 1) ;
                }


                IsActive = false;
                return true;
            }

            return false;
        }
    }
}
