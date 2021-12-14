using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MyMidnightCommander
{
    public class FunctionKeysLabels
    {
        public List<string> FKLItems = new List<string>();
        public FunctionKeysLabels()
        {
            FKLItems.Add("POMOC!");
            FKLItems.Add("MkDir");
            FKLItems.Add("Rename");
            FKLItems.Add("Move");
            FKLItems.Add("Copy");
            FKLItems.Add("Delete");
            FKLItems.Add("EXIT"); 
        }


        public void DrawFKL()
        {
            Console.SetCursorPosition(0, Console.WindowHeight-1);

            for(int i = 0; i < FKLItems.Count; i++)
            {
                Console.Write($" {i + 1}");
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.Write(FKLItems[i].PadRight(Console.WindowWidth / (FKLItems.Count + 1)));
                Console.ResetColor();
            }
            for(int i = Console.CursorLeft; i < Console.WindowWidth; i++)
            {
                Console.Write(' ');
            }

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(0, 0);
        }

        public bool HandleFunctionKeys(ConsoleKeyInfo info, MyDirectory rightDir, MyDirectory leftDir)
        {
            bool keyWasPressed = true;

            if (info.Key == ConsoleKey.F1) //Help
            {
                HandleHelp();
            }
            else if (info.Key == ConsoleKey.F2) //MkDir
            {
                if (rightDir.IsSelectedDir)
                    HandleMkDir(rightDir.DirPath);
                else
                    HandleMkDir(leftDir.DirPath);
            }
            else if (info.Key == ConsoleKey.F3) //Rename
            {
                if (rightDir.IsSelectedDir)
                    HandleRename(rightDir.DirPath, rightDir.DirItemNames[rightDir.SelectedRow]);
                else
                    HandleRename(leftDir.DirPath, leftDir.DirItemNames[leftDir.SelectedRow]);
            }
            else if (info.Key == ConsoleKey.F4) //Move
            {
                string[] dirPaths = {leftDir.DirPath, rightDir.DirPath };
                string[] dirSelItems = { leftDir.DirItemNames[leftDir.SelectedRow], rightDir.DirItemNames[rightDir.SelectedRow]};
                HandleMove(dirPaths, dirSelItems);
            }
            else if (info.Key == ConsoleKey.F5) //Copy
            {
                string[] dirPaths = { leftDir.DirPath, rightDir.DirPath };
                string[] dirSelItems = { leftDir.DirItemNames[leftDir.SelectedRow], rightDir.DirItemNames[rightDir.SelectedRow] };
                HandleCopy(dirPaths, dirSelItems);
            }
            else if (info.Key == ConsoleKey.F6) //Delete
            {
                if (rightDir.IsSelectedDir)
                    HandleDelete(rightDir.DirPath, rightDir.DirItemNames[rightDir.SelectedRow]);
                else
                    HandleDelete(leftDir.DirPath, leftDir.DirItemNames[leftDir.SelectedRow]);
            }
            //F7 (EXIT) is already defined in UI.HandleKey (bottom - FK section)
            else
                keyWasPressed = false;

            return keyWasPressed;
        }

        public void HandleHelp()
        {
            InfoDialogue.Draw("Tato funkce ještě nebyla přidána!  ");
            Console.ReadKey();
        }

        public void HandleMkDir(string path)
        {
            string[] inputLabels = { "Zadej název nové složky: " };
            string[] inputVals = { "Nová BUM složka"};
            string[] buttons = { "OK", "Cancel"};

            List<string> MkDirVals = new List<string>();
            MkDirVals = DrawInputDialogue("Vytvořit novou složku", inputLabels, inputVals, buttons);

            if (MkDirVals[0] == "Cancel")
                return;
            else if (MkDirVals[0] == "OK")
            {
                DirectoryInfo myDirInfo = new DirectoryInfo(path + "\\" + MkDirVals[1]);
                myDirInfo.Create();
                return;
            }

            return;
        }

        public void HandleRename(string path, string selectedItem)
        {
            //parameters for dialogue
            string[] inputLabels = { "Zadej nový název: "};
            string[] inputVals = { selectedItem.Substring(1, selectedItem.Length - 1) };
            string[] buttons = { "Ok", "Cancel" };

            if (selectedItem == "\\..") // if parent dir is selected
            {
                InfoDialogue.Draw("Nemůžeš přejmenovat adresář \\..  ");
                Console.ReadKey();
                return;
            }

            //dialogue
            List<string> RenameVals = new List<string>();
            RenameVals = DrawInputDialogue("Přejmenovat", inputLabels, inputVals, buttons);
            
            if (selectedItem.Substring(0, 1) == "\\" && RenameVals[0] == "Ok") // if folder is selected
            {
                DirectoryInfo myDir = new DirectoryInfo(path + selectedItem);
                myDir.MoveTo(path + '\\' + RenameVals[1]);
            }
            else if (RenameVals[0] == "Ok") // file is selected
            {
                FileInfo myFile = new FileInfo(path + '\\' + selectedItem.Substring(1, selectedItem.Length - 1));
                myFile.MoveTo(path + '\\' + RenameVals[1]);
            }

            return;

        }

        public void HandleMove(string[] path, string[] selectedItem)
        {
            //change paths accordint to selected item in dir
            for (int i = 0; i < 2; i++)
            {
                if (selectedItem[i] == "\\..") // path[i] stays the same
                    ;
                else if (selectedItem[i].Substring(0, 1) == "\\")
                    path[i] += selectedItem[i];
                else if (selectedItem[i].Substring(0, 1) == " ")
                    path[i] += '\\' + selectedItem[i].Substring(1, selectedItem[i].Length - 1);
            }

            //parameters for dialogue
            string[] inputLabels = { "Zdrojová složka: ", "Cílová složka" };
            string[] inputVals = { path[0], path[1] };
            string[] buttons = { "OK", "Cancel" };

            //dialogue
            List<string> CopyVals = new List<string>();
            CopyVals = DrawInputDialogue("Vyjmout", inputLabels, inputVals, buttons);

            //handle dialogue output
            if (CopyVals[0] == "Cancel")
                return;
            else if (CopyVals[0] == "OK")
            {
                string source = CopyVals[1];
                string destination = CopyVals[2] + '\\';

                DirectoryInfo dir = new DirectoryInfo(source);

                string[] sourceParts = source.Split('\\');
                int sourceTrimLenght = 0;
                for (int i = 0; i < sourceParts.Length - 1; i++)
                    sourceTrimLenght += sourceParts[i].Length + 1;

                //check if destination exists
                DirectoryInfo destinationDir = new DirectoryInfo(destination + sourceParts[sourceParts.Length - 1]);
                if (destinationDir.Exists)
                {
                    InfoDialogue.Draw($"položka {destinationDir.FullName} already exists!");
                    Console.ReadKey();
                    return;
                }

                //handle the operation
                GoThroughDirectory(dir, sourceTrimLenght, destination, "move");

                DirectoryInfo destinationDir2 = new DirectoryInfo(destination + sourceParts[sourceParts.Length - 1]);
                if(!destinationDir2.Exists)
                    destinationDir2.Create();

                foreach (FileInfo file in dir.GetFiles())
                {
                    file.MoveTo(destination + file.FullName.Substring(sourceTrimLenght, file.FullName.Length - sourceTrimLenght));
                }
                dir.Delete();

                return;
            }
            return;
        }

        public void HandleCopy(string[] path, string[] selectedItem)
        {
            //change paths accordint to selected item in dir
            for (int i = 0; i < 2; i++)
            {
                if (selectedItem[i] == "\\..") // path[i] stays the same
                    ;
                else if (selectedItem[i].Substring(0, 1) == "\\")
                    path[i] += selectedItem[i];
                else if (selectedItem[i].Substring(0, 1) == " ")
                    path[i] += '\\' + selectedItem[i].Substring(1, selectedItem[i].Length - 1);
            }

            //parameters for dialogue
            string[] inputLabels = { "Zdrojová složka: ", "Cílová složka" };
            string[] inputVals = { path[0], path[1] };
            string[] buttons = { "OK", "Cancel" };

            //dialogue
            List<string> CopyVals = new List<string>();
            CopyVals = DrawInputDialogue("Kopírování", inputLabels, inputVals, buttons);

            if (CopyVals[0] == "Cancel")
                return;
            else if (CopyVals[0] == "OK")
            {
                string source = CopyVals[1];
                string destination = CopyVals[2] + '\\';

                DirectoryInfo dir = new DirectoryInfo(source);
                
                string[] sourceParts = source.Split('\\');
                int sourceTrimLenght = 0;
                for (int i = 0; i < sourceParts.Length - 1; i++)
                    sourceTrimLenght += sourceParts[i].Length + 1;

                //check if destination exists
                DirectoryInfo destinationDir = new DirectoryInfo(destination + sourceParts[sourceParts.Length - 1]);
                if (destinationDir.Exists)
                {
                    InfoDialogue.Draw($"položka {destinationDir.FullName} already exists!");
                    Console.ReadKey();
                    return;
                }

                //handle the operation
                GoThroughDirectory(dir, sourceTrimLenght, destination, "copy");

                DirectoryInfo destinationDir2 = new DirectoryInfo(destination + sourceParts[sourceParts.Length - 1]);
                if (!destinationDir2.Exists)
                    destinationDir2.Create();

                foreach (FileInfo file in dir.GetFiles())
                    file.CopyTo(destination + file.FullName.Substring(sourceTrimLenght, file.FullName.Length - sourceTrimLenght));
                return;
            }
            return;
        }

        public void HandleDelete(string path, string selectedItem)
        {
            string[] buttons = { "Ok", "Cancel" };

            string chosenOption = "";
            if(selectedItem == "\\..") // if parent dir is selected
            {
                InfoDialogue.Draw("Nemůžeš odstranit adresář \\..  ");
                Console.ReadKey();
            }
            else if(selectedItem.Substring(0, 1) == "\\") // if folder is selected
            {
                chosenOption = DrawChoiceDialogue("Smazat", $"smazat složku „{selectedItem}“?", buttons);
                if(chosenOption == "Ok")
                {
                    DirectoryInfo dir = new DirectoryInfo(path + selectedItem);
                    GoThroughDirectory(dir, 0, "", "delete");
                    foreach (FileInfo file in dir.GetFiles())
                        file.Delete();
                    dir.Delete();
                }
            }
            else // file is selected
            {
                chosenOption = DrawChoiceDialogue("Smazat", $"smazat soubor „{selectedItem.Substring(1, selectedItem.Length - 1)}“?", buttons);
                if(chosenOption == "Ok")
                {
                    FileInfo myFile = new FileInfo(path + '\\' + selectedItem.Substring(1, selectedItem.Length - 1));
                    myFile.Delete();
                }
                
            }
            return;
        }

        public void GoThroughDirectory(DirectoryInfo dir, int sourceTrimLenght, string destination, string method)
        {
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                if (method == "copy" || method == "move")
                {
                    DirectoryInfo dirToCreateInDest = new DirectoryInfo(destination + subDir.FullName.Substring(sourceTrimLenght, subDir.FullName.Length - sourceTrimLenght));
                    dirToCreateInDest.Create();
                }

                GoThroughDirectory(subDir, sourceTrimLenght, destination, method);

                foreach (FileInfo file in subDir.GetFiles())
                {
                    if (method == "copy")
                        file.CopyTo(destination + file.FullName.Substring(sourceTrimLenght, file.FullName.Length - sourceTrimLenght));
                    else if (method == "move")
                        file.MoveTo(destination + file.FullName.Substring(sourceTrimLenght, file.FullName.Length - sourceTrimLenght));
                    else if (method == "delete")
                    {
                        FileInfo fileToDelete = new FileInfo(file.FullName);
                        fileToDelete.Delete();
                    }
                }
                
                if (method == "delete" || method == "move")
                    subDir.Delete();
            }
        }

        public List<string> DrawInputDialogue(string title, string[] inputLabels, string[] inputVals, string[] buttons)
        {
            //just to be sure...
            if (inputLabels.Length != inputVals.Length)
            {
                List<string> returnVals = new List<string>();
                returnVals.Add("Incorrect Syntax");
                returnVals.Add("count of inputLabels doesn't match inputVals");
                return returnVals;
            }

            int windowWidth = 75;
            int windowHeight = 6 + (2 * inputLabels.Length);

            int SelectedItem = 0;

            //formatting buttons + getting their lenght when lined up in row
            int btnsInRowTotalLenght = 0;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = $" [{buttons[i]}] "; //formatting
                btnsInRowTotalLenght += buttons[i].Length; // buttons lenght in row
            }

            int startX = Console.WindowWidth / 2 - (windowWidth / 2);
            int startY = Console.WindowHeight / 2 - (windowHeight / 2) - 2; //-2 for esthetics

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.CursorVisible = false;
            Console.SetCursorPosition(startX, startY);

            while (true)
            {
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
                        Console.Write(" │" + "".PadRight((windowWidth - btnsInRowTotalLenght) / 2 - 2 + windowWidth % 2));
                        for (int i = 0; i < buttons.Length; i++)
                        {
                            if (i == SelectedItem - inputVals.Length)
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
                        if (SelectedItem == dRow / 2 - 1)
                            Console.ForegroundColor = ConsoleColor.Black;
                        else
                            Console.ForegroundColor = ConsoleColor.DarkGray;

                        if(inputVals[dRow / 2 - 1].Length > windowWidth - 6)
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
                    if (SelectedItem == i)
                    {
                        Console.CursorVisible = true;
                        if(inputVals[i].Length >= windowWidth - 6)
                        {
                            Console.SetCursorPosition(startX + windowWidth - 4, startY + ((SelectedItem + 1) * 2) + 1);
                        }
                        else
                            Console.SetCursorPosition(startX + 3 + inputVals[i].Length, startY + ((SelectedItem + 1) * 2) + 1);
                    }
                }

                //HandleKey
                ConsoleKeyInfo info = Console.ReadKey();
                if (info.Key == ConsoleKey.Tab)
                {
                    SelectedItem += 1;
                    if (SelectedItem >= inputVals.Length + buttons.Length)
                        SelectedItem = 0;
                }
                else if (info.Key == ConsoleKey.Escape)
                {
                    List<string> returnVals = new List<string>();
                    returnVals.Add(buttons[buttons.Length - 1]);
                    return returnVals;
                }
                else if (info.Key == ConsoleKey.Backspace && SelectedItem < inputVals.Length)
                {
                    if (inputVals[SelectedItem].Length < 1)
                        continue;
                    inputVals[SelectedItem] = inputVals[SelectedItem].Substring(0, inputVals[SelectedItem].Length - 1);
                }
                else if (SelectedItem < inputVals.Length)
                {
                    //remove unwanted keys
                    if (info.Key == ConsoleKey.Enter || info.Key == ConsoleKey.LeftWindows || info.Key == ConsoleKey.RightWindows)
                        continue;

                    inputVals[SelectedItem] += info.KeyChar;
                }
                else if (SelectedItem >= inputVals.Length)
                {
                    List<string> returnVals = new List<string>();
                    string actualButtonValue = buttons[SelectedItem - inputVals.Length];

                    actualButtonValue = actualButtonValue.Substring(2, actualButtonValue.Length - 2);
                    actualButtonValue = actualButtonValue.Substring(0, actualButtonValue.Length - 2);

                    returnVals.Add(actualButtonValue);
                    foreach (string returnInput in inputVals)
                        returnVals.Add(returnInput);

                    Console.CursorVisible = false;
                    return returnVals;
                }
            }
        }

        public string DrawChoiceDialogue(string title, string text, string[] buttons)
        {
            int windowWidth = text.Length + 6;
            int windowHeight = 6 + 1;

            //formatting buttons + getting their lenght when lined up in row
            int btnsInRowTotalLenght = 0;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i] = $" [{buttons[i]}] "; //formatting
                btnsInRowTotalLenght += buttons[i].Length; // buttons lenght in row
            }

            if (text.Length < btnsInRowTotalLenght)
                windowWidth = btnsInRowTotalLenght + 6;

            List<string> textLines = new List<string>();

            if (windowWidth > Console.WindowWidth / 2)
            {
                string[] textWords = text.Split(' ');

                int numberOfLines = 0;

                string lineOfText = "";
                foreach (string word in textWords)
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
                textLines.Add(text);

            int SelectedItem = 0;

            int startX = Console.WindowWidth / 2 - (windowWidth / 2);
            int startY = Console.WindowHeight / 2 - (windowHeight / 2) - 2; //-2 for esthetics

            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;

            Console.CursorVisible = false;
            Console.SetCursorPosition(startX, startY);

            while (true)
            {

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
                        Console.Write(" │" + "".PadRight((windowWidth - btnsInRowTotalLenght) / 2 - 2 + windowWidth % 2));
                        for (int i = 0; i < buttons.Length; i++)
                        {
                            if (i == SelectedItem)
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

                Console.SetCursorPosition(startX, startY);
                Console.CursorVisible = false;

                //HandleKey
                ConsoleKeyInfo info = Console.ReadKey();
                if (info.Key == ConsoleKey.Tab)
                {
                    SelectedItem += 1;
                    if (SelectedItem >= buttons.Length)
                        SelectedItem = 0;
                }
                else if (info.Key == ConsoleKey.Escape)
                {
                    return buttons[buttons.Length - 1];
                }
                else if (info.Key == ConsoleKey.Enter)
                {
                    string actualButtonValue = buttons[SelectedItem];

                    actualButtonValue = actualButtonValue.Substring(2, actualButtonValue.Length - 2);
                    actualButtonValue = actualButtonValue.Substring(0, actualButtonValue.Length - 2);

                    return actualButtonValue;
                }
            }
        }
    }
}
 