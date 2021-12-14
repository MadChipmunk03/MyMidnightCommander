using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMidnightCommander.Dialogues;
using MyMidnightCommander.Functions;

namespace MyMidnightCommander.Dialogues
{
    public static class ChoiceBoxTemplate
    {
        

        public static void Draw(string title, string text, string[] buttons, int btnsInRowTotalLenght, int selectedItem)
        {
            int windowWidth = text.Length + 6;
            int windowHeight = 6 + 1;
            
            if (text.Length < btnsInRowTotalLenght)
                windowWidth = btnsInRowTotalLenght + 6;

            List<string> textLines = new List<string>();

            if (windowWidth > Console.WindowWidth / 2)
            {
                string[] textWords = text.Split(' ');

                int numberOfLines = 0;

                string lineOftext = "";
                foreach (string word in textWords)
                {
                    if ((lineOftext + word + " ").Length < Console.WindowWidth / 2 - 9)
                        lineOftext += word + " ";
                    else
                    {
                        textLines.Add(lineOftext.Substring(0, lineOftext.Length - 1));
                        numberOfLines += 1;
                        lineOftext = word;
                    }
                }
                textLines.Add(lineOftext);
                windowHeight += numberOfLines;
                windowWidth = Console.WindowWidth / 2 - 6;
            }
            else
                textLines.Add(text);

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
                else if (dRow == windowHeight - 3) // buttons
                {
                    Console.Write(" │" + "".PadRight((windowWidth - btnsInRowTotalLenght) / 2 - 2 + windowWidth % 2));
                    for (int i = 0; i < buttons.Length; i++)
                    {
                        if (i == selectedItem)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }

                        Console.Write(buttons[i]);
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write("".PadRight((windowWidth - btnsInRowTotalLenght) / 2 - 2) + "│ ");
                }
                else if (dRow == 1)
                {
                    Console.Write(" ┌");
                    for (int dChar = 2; dChar < windowWidth / 2 - (title.Length / 2) - 1 - (title.Length % 2) + (windowWidth % 2); dChar++)
                        Console.Write('─');
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($" {title} ");
                    Console.ForegroundColor = ConsoleColor.White;
                    for (int dChar = 2; dChar < windowWidth / 2 - (title.Length / 2) - 1; dChar++)
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
                else if (dRow == windowHeight - 4) // Separator between buttons and inputs
                {
                    Console.Write(" ├");
                    Console.Write("".PadRight(windowWidth - 4, '─'));
                    Console.Write("┤ ");
                }
            }

            Console.SetCursorPosition(0, 0);
            Console.Write(selectedItem);

            Console.SetCursorPosition(startX, startY);
            Console.CursorVisible = false;
        }
    }
}
