using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMidnightCommander.Dialogues;

namespace MyMidnightCommander.Dialogues.DialogTemplates
{
    public class InputDialogTemplate
    {
        /*public static bool IsActive { get; set; }
        private static string Title { get; set; }
        public static List<string> InputLabels = new List<string>();
        public static List<string> InputVals = new List<string>();
        private static string[] Buttons { get; set; }
        private static string Method { get; set; }

        private int SelectedItem { get; set; } = 0;*/
        
        public static void Draw(string title, string[] inputLabels, string[] inputVals, string[] buttons, int btnsInRowTotalLenght, int selectedItem)
        {
            int windowWidth = 75;
            int windowHeight = 6 + (2 * inputLabels.Length);

            int startX = Console.WindowWidth / 2 - (windowWidth / 2);
            int startY = Console.WindowHeight / 2 - (windowHeight / 2) - 2; //-2 for esthetics

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;

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
                        if (i == selectedItem - inputVals.Length)
                            Console.BackgroundColor = ConsoleColor.DarkCyan;

                        Console.Write(buttons[i]);
                        Console.BackgroundColor = ConsoleColor.Gray;
                    }
                    Console.Write("".PadRight((windowWidth - btnsInRowTotalLenght) / 2 - 2) + "│ ");
                }
                else if (dRow == 1)
                {
                    Console.Write(" ┌");
                    for (int dChar = 2; dChar < windowWidth / 2 - (title.Length / 2) - 1 - (title.Length % 2) + (windowWidth % 2); dChar++)
                        Console.Write('─');
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write($" {title} ");
                    Console.ForegroundColor = ConsoleColor.Black;
                    for (int dChar = 2; dChar < windowWidth / 2 - (title.Length / 2) - 1; dChar++)
                        Console.Write('─');
                    Console.Write("┐ ");
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
                else if (dRow % 2 == 0) // Lables
                {
                    Console.Write(" │ ");
                    Console.Write(inputLabels[dRow / 2 - 1].PadRight(windowWidth - 6));
                    Console.Write(" │ ");
                }
                else if (dRow % 2 == 1) // Inputs
                {
                    Console.Write(" │ ");
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    if (selectedItem == dRow / 2 - 1)
                        Console.ForegroundColor = ConsoleColor.Black;
                    else
                        Console.ForegroundColor = ConsoleColor.DarkGray;

                    if (inputVals[dRow / 2 - 1].Length > windowWidth - 6)
                    {
                        string usedInputVal = inputVals[dRow / 2 - 1];
                        Console.Write("..." + usedInputVal.Substring(usedInputVal.Length - (windowWidth - 10), windowWidth - 10) + " ");
                    }
                    else
                        Console.Write(inputVals[dRow / 2 - 1].PadRight(windowWidth - 6));

                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" │ ");
                }
            }

            //hide cursor, or show it in Input
            Console.SetCursorPosition(startX, startY);
            Console.CursorVisible = false;
            for (int i = 0; i < inputVals.Length; i++)
            {
                if (selectedItem == i)
                {
                    Console.CursorVisible = true;
                    if (inputVals[i].Length >= windowWidth - 6)
                    {
                        Console.SetCursorPosition(startX + windowWidth - 4, startY + ((selectedItem + 1) * 2) + 1);
                    }
                    else
                        Console.SetCursorPosition(startX + 3 + inputVals[i].Length, startY + ((selectedItem + 1) * 2) + 1);
                }
            }
        }
        /*public bool ReadKey(ConsoleKeyInfo info)
        {
            return true;
            if (info.Key == ConsoleKey.Tab)
            {
                SelectedItem += 1;
                if (SelectedItem >= inputVals.lenght + Buttons.Length)
                    SelectedItem = 0;
            }
            else if (info.Key == ConsoleKey.Escape)
            {
                List<string> returnVals = new List<string>();
                returnVals.Add(Buttons[Buttons.Length - 1]);



                return returnVals;
            }
            else if (info.Key == ConsoleKey.Backspace && SelectedItem < inputVals.lenght)
            {
                if (InputVals[SelectedItem].Length < 1)
                    return;
                InputVals[SelectedItem] = InputVals[SelectedItem].Substring(0, InputVals[SelectedItem].Length - 1);
            }
            else if (SelectedItem < inputVals.lenght)
            {
                //remove unwanted keys
                if (info.Key == ConsoleKey.Enter || info.Key == ConsoleKey.LeftWindows || info.Key == ConsoleKey.RightWindows)
                    return;

                InputVals[SelectedItem] += info.KeyChar;
            }
            else if (SelectedItem >= inputVals.lenght)
            {
                List<string> returnVals = new List<string>();
                string actualButtonValue = Buttons[SelectedItem - inputVals.lenght];

                actualButtonValue = actualButtonValue.Substring(2, actualButtonValue.Length - 2);
                actualButtonValue = actualButtonValue.Substring(0, actualButtonValue.Length - 2);

                returnVals.Add(actualButtonValue);
                foreach (string returnInput in InputVals)
                    returnVals.Add(returnInput);

                Console.CursorVisible = false;
                return returnVals;
            }
        }*/
    }
}
