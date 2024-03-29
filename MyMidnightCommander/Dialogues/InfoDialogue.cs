﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyMidnightCommander.Components;
using MyMidnightCommander.Dialogues;

namespace MyMidnightCommander.Dialogues
{
    public class InfoDialogue : Dialog
    {
        public bool IsActive { get; set; }
        public string Message { get; set; }

        private int DialogueHeight = 10;
        private int DialogueWidth = 50;

        public InfoDialogue(string message)
        {
            Message = message;
        }

        public override void Draw()
        {
            string message = Message;

            if (!IsActive)
                return;

            if (message.Length <= 42)
            {
                DialogueWidth = message.Length + 6;
                DialogueHeight = 7;
            }
            else
            {
                message = message + "\r\n";
                DialogueWidth = 50;
                DialogueHeight = 6 + (message.Length / 42) + 1;
            }

            int startX = Console.WindowWidth / 2 - (DialogueWidth / 2);
            int startY = Console.WindowHeight / 2 - (DialogueHeight / 2) - 4; //-4 z estetiky
            Console.SetCursorPosition(startX, startY);
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            for(int dRow = 0; dRow < DialogueHeight; dRow++)
            {
                Console.SetCursorPosition(startX, startY + dRow);
                if (dRow == 0 || dRow == DialogueHeight - 1)
                    for (int dChar = 0; dChar < DialogueWidth; dChar++)
                        Console.Write(' ');
                else if (dRow == DialogueHeight - 3 || dRow == 2)
                {
                    Console.Write(" │");
                    for (int dChar = 2; dChar < DialogueWidth - 2; dChar++)
                        Console.Write(' ');
                    Console.Write("│ ");
                }
                else if (dRow == 1)
                {
                    Console.Write(" ┌");
                    for (int dChar = 2; dChar < DialogueWidth / 2 - 4 + (DialogueWidth % 2); dChar++)
                        Console.Write('─');
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" Chyba ");
                    Console.ForegroundColor = ConsoleColor.White;
                    for (int dChar = 2; dChar < DialogueWidth / 2 - 3; dChar++)
                        Console.Write('─');
                    Console.Write("┐ ");
                }
                else if (dRow == DialogueHeight - 2)
                {
                    Console.Write(" └");
                    for (int dChar = 2; dChar < DialogueWidth - 2; dChar++)
                        Console.Write('─');
                    Console.Write("┘ ");
                }
                else
                {
                    Console.Write(" │");
                    if (message.Length < 42)
                        Console.Write("  " + message.Substring(0, message.Length % 42 - 2).PadRight(DialogueWidth - 8) + "  ");
                    else
                    {
                        Console.Write("  " + message.Substring(0, 42) + "  ");
                        message = message.Substring(42, message.Length - 42);
                    }
                    Console.Write("│ ");
                }
            }
        }

        public void ReadKey()
        {

        }
    }
}
