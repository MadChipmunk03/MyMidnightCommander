using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MyMidnightCommander.Components;
using MyMidnightCommander.Dialogues.DirDialogues;
using MyMidnightCommander.Dialogues;
using MyMidnightCommander.Window;

namespace MyMidnightCommander
{
    public class MyDirectory : IComponents
    {
        public string[] Args { get; set; }
        public bool IsActive { get; set; }
        public int RowNumber { get; set; }

        private bool IgnoreReadKey { get; set; }

        public bool IsRightDirecory { get; set; }
        public const bool RightDirectory = true;
        public const bool LeftDirectory = false;

        public bool IsSelectedDir { get; set; }
        public int SelectedRow { get; set; }
        public int TopRow { get; set; }
        public int MaxRow { get; set; }

        public string DirPath { get; set; }
        public List<string> DirItemNames = new List<string>();
        public List<string> DirItemSizes = new List<string>();
        public List<string> DirItemDates = new List<string>();

        public int DirWidth { get; set; }
        public int DirHeight { get; set; }
        public int StartAtX { get; set; }

        public MyDirectory(bool isRightDirecory)
        {
            DirWidth = Console.WindowWidth / 2;
            DirHeight = Console.WindowHeight - 2;

            SelectedRow = 1;
            TopRow = 0;

            DirPath = @"C:\Users\Péťa\Documents\Git\Vs\MultiUSB";//Directory.GetCurrentDirectory()

            IsRightDirecory = isRightDirecory;

            if (IsRightDirecory)
            {
                StartAtX = DirWidth;
                DirWidth += Console.WindowWidth % 2;
                DirPath = @"C:\Users\Péťa\Desktop";
                IsSelectedDir = false;
            }
            else
            {
                StartAtX = 0;
                IsSelectedDir = true;
            }
            GetFilesAndFolders();
        }

        public void Draw()
        {
            DirWidth = Console.WindowWidth / 2;
            DirHeight = Console.WindowHeight - 2;
            if (IsRightDirecory)
            {
                StartAtX = DirWidth;
                DirWidth += Console.WindowWidth % 2;
            }

            int startX = StartAtX;
            int startY = 1;

            Console.SetCursorPosition(startX, startY);

            Console.BackgroundColor = ConsoleColor.DarkBlue;

            for (RowNumber = 0; RowNumber < DirHeight; RowNumber++)
            {
                Console.SetCursorPosition(startX, startY + RowNumber);
                WriteDirectoryLine();
            }
            Console.ResetColor();
        }

        public void WriteDirectoryLine()
        {
            if(RowNumber == 0)
            {
                Console.Write('┌' + "<" + '─'); //top of Dir
                if (IsSelectedDir)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                if (DirPath.Length <= DirWidth - 5) {
                    Console.Write(DirPath);
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.White;
                    for (int i = 0; i < DirWidth - 5 - DirPath.Length; i++)
                    {
                        
                        Console.Write('─');
                    }
                }
                else
                    Console.Write("..." + DirPath.Substring(DirPath.Length - (DirWidth - 8), DirWidth - 8));

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write(">" + '┐');
            }
            else if(RowNumber == DirHeight - 3)
            {
                Console.Write('├');
                for (int i = 0; i < DirWidth - 2; i++)
                    Console.Write('─');
                Console.Write('┤');
            }
            else if (RowNumber == DirHeight - 2)
            {
                Console.Write('│');
                Console.Write(DirItemNames[SelectedRow].PadRight(DirWidth-2));
                Console.Write('│');
            }
            else if (RowNumber == DirHeight - 1) //bottom of Dir
            {
                Console.Write('└');
                for (int i = 0; i < DirWidth - 2; i++)
                    Console.Write('─');
                Console.Write('┘');
            }
            else //content of Dir
            {
                HandleDirContent();
            }
        }

        public void HandleDirContent()
        {
            Console.Write('│');

            int availableSpace = DirWidth - 23;
            int selectedItem = RowNumber - 1;

            if(RowNumber > 1) //not includeing header
                selectedItem += TopRow;

            if ((DirItemNames.Count - TopRow >= RowNumber || RowNumber == 0 || RowNumber == 1) && Console.WindowHeight - RowNumber > 5) 
            {
                DirContentColorChooser(selectedItem);

                string row = "";
                if(DirItemNames[selectedItem].Length < (availableSpace))
                    row += DirItemNames[selectedItem].PadRight(availableSpace);
                else
                {
                    row += DirItemNames[selectedItem].Substring(0, (availableSpace) / 2) 
                        + "~" 
                        + DirItemNames[selectedItem].Substring(DirItemNames[selectedItem].Length - (availableSpace) / 2 + DirWidth % 2, (availableSpace) / 2 - DirWidth % 2);
                }
                    row += '│';
                    row += DirItemSizes[selectedItem].PadLeft(8);
                    row += '│';
                    row += DirItemDates[selectedItem].PadLeft(11);

                
                Console.Write(row);
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            else
                for (int i = 0; i < DirWidth - 2; i++)
                    Console.Write(' ');

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write('│');
        }

        public void DirContentColorChooser(int selectedItem)
        {
            if (selectedItem == SelectedRow && IsSelectedDir)
            {
                Console.BackgroundColor = ConsoleColor.DarkCyan;
                Console.ForegroundColor = ConsoleColor.Black;
                return;
            }

            string[] ItemParts = DirItemNames[selectedItem].Split('.');
            string fileType = ItemParts[ItemParts.Length - 1];

            if(DirItemNames[selectedItem] == "" || DirItemNames[selectedItem].Substring(0,1) == "\\") //DirItemNames[selectedItem] == "" for empty Drive row
                Console.ForegroundColor = ConsoleColor.White;
            else if(fileType == "exe")
                Console.ForegroundColor = ConsoleColor.Green;
            else if (fileType == "txt")
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            else if (fileType == "dat")
                Console.ForegroundColor = ConsoleColor.Magenta;
            else
                Console.ForegroundColor = ConsoleColor.Gray;
        }

        public string GetFilesAndFolders()
        {
            DirectoryInfo dir = new DirectoryInfo(DirPath);

            DirItemNames.Clear();
            DirItemSizes.Clear();
            DirItemDates.Clear();

            //add header
            DirItemNames.Add(".n");
            DirItemSizes.Add("Velikost");
            DirItemDates.Add("datum změny");

            //add upperDirectory
            DirItemNames.Add("\\..");
            DirItemSizes.Add("VÝŠ-ADR");
            DirItemDates.Add("-");

            try
            {
                foreach (DirectoryInfo myDir in dir.GetDirectories())
                {
                    if (myDir.Name == "D:\\")
                        continue;
                    DirItemNames.Add('\\' + myDir.Name);
                    DirItemSizes.Add("");
                    DirItemDates.Add(Convert.ToString(myDir.LastWriteTime.Day).PadLeft(2, '0')
                        + "." + Convert.ToString(myDir.LastWriteTime.Month).PadLeft(2, '0')
                        + "." + myDir.LastWriteTime.Year);
                }
                foreach (FileInfo myFile in dir.GetFiles())
                {
                    DirItemNames.Add(' ' + Convert.ToString(myFile));

                    string lenght = "";
                    if (myFile.Length < 9999)
                        lenght = Convert.ToString(myFile.Length) + " B ";
                    else if (myFile.Length < 9999999)
                    {
                        int number = Convert.ToInt32(myFile.Length) / 1024;
                        lenght = Convert.ToString(number) + " kB";
                    }
                    else if (myFile.Length < 9999999999)
                    {
                        long number = Convert.ToInt64(myFile.Length) / 1024 / 1024;
                        lenght = Convert.ToString(number) + " mB";
                    }
                    DirItemSizes.Add(lenght);

                    DirItemDates.Add(Convert.ToString(myFile.LastWriteTime.Day).PadLeft(2, '0')
                        + "."
                        + Convert.ToString(myFile.LastWriteTime.Month).PadLeft(2, '0')
                        + "."
                        + myFile.LastWriteTime.Year);
                }
            }
            catch (System.IO.IOException e)
            {
                if(DirPath.Length < 4)
                {
                    DirPath = "Drives";
                    GetDisks();
                    return e.Message;
                }
            }
            catch (System.UnauthorizedAccessException e)
            {
                return e.Message;
            }

            return "";
        }

        public void GetDisks()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            DirItemNames.Clear();
            DirItemSizes.Clear();
            DirItemDates.Clear();

            //add header
            DirItemNames.Add("Drive");
            DirItemSizes.Add("Velikost");
            DirItemDates.Add("datum změny");

            //add upperDirectory
            DirItemNames.Add("");
            DirItemSizes.Add("");
            DirItemDates.Add("");

            foreach (DriveInfo Drive in allDrives)
            {
                DirItemNames.Add(Drive.Name);
                DirItemSizes.Add("");
                DirItemDates.Add("");
            }
        }

        public void ReadKey(ConsoleKeyInfo info)
        {
            if (IgnoreReadKey)
                return;

            if (info.Key == ConsoleKey.Tab) // zkopírováno od šibravy (protože to je mega hustý)
            {
                IsSelectedDir = !IsSelectedDir;
            }
            else if (info.Key == ConsoleKey.F5) //Copy
            {
                if (IsSelectedDir)
                {
                    string selectedItem = DirItemNames[SelectedRow];
                    if (selectedItem == "\\..") // if parent dir is selected
                        CopyDialogue.SourcePath = DirPath;
                    else if (selectedItem.Substring(0, 1) == "\\") // if folder is selected
                        CopyDialogue.SourcePath = DirPath + selectedItem;
                    else // file is selected
                        CopyDialogue.SourcePath = DirPath + '\\' + selectedItem.Substring(1, selectedItem.Length - 1);
                    UI.UsedDialog = new CopyDialogue();
                    UI.DialogIsOn = true;
                }
                else
                {
                    string selectedItem = DirItemNames[SelectedRow];
                    if (selectedItem == "\\..") // if parent dir is selected
                        CopyDialogue.DestinationPath = DirPath;
                    else if (selectedItem.Substring(0, 1) == "\\") // if folder is selected
                        CopyDialogue.DestinationPath = DirPath + selectedItem;
                    else // file is selected
                        CopyDialogue.DestinationPath = DirPath;
                }
            }
            else if (info.Key == ConsoleKey.F4) //Move
            {
                if (IsSelectedDir)
                {
                    string selectedItem = DirItemNames[SelectedRow];
                    if (selectedItem == "\\..") // if parent dir is selected
                        MoveDialogue.SourcePath = DirPath;
                    else if (selectedItem.Substring(0, 1) == "\\") // if folder is selected
                        MoveDialogue.SourcePath = DirPath + selectedItem;
                    else // file is selected
                        MoveDialogue.SourcePath = DirPath + '\\' + selectedItem.Substring(1, selectedItem.Length - 1);
                    UI.UsedDialog = new MoveDialogue();
                    UI.DialogIsOn = true;
                }
                else
                {
                    string selectedItem = DirItemNames[SelectedRow];
                    if (selectedItem == "\\..") // if parent dir is selected
                        MoveDialogue.DestinationPath = DirPath;
                    else if (selectedItem.Substring(0, 1) == "\\") // if folder is selected
                        MoveDialogue.DestinationPath = DirPath + selectedItem;
                    else // file is selected
                        MoveDialogue.DestinationPath = DirPath;
                }
            }
            else if (IsSelectedDir)
            {
                if (info.Key == ConsoleKey.Enter)
                {
                    if (SelectedRow == 1) // upper dir "/.."
                    {
                        string[] parts = DirPath.Split('\\');

                        if (parts.Length > 2)
                        {
                            string lowerPath = parts[0];
                            for (int i = 1; i < parts.Length - 1; i++)
                            {
                                lowerPath += "\\" + parts[i];
                            }
                            DirPath = lowerPath;
                            GetFilesAndFolders();
                        }
                        else if (parts.Length > 1 && parts[1] != "")
                        {
                            DirPath = parts[0] + "\\";
                            GetFilesAndFolders();
                        }
                        else if (DirPath != "Drives")
                        {
                            GetDisks();
                            DirPath = "Drives";
                        }
                    }
                    else if (DirPath == "Drives")
                    {
                        DirPath = DirItemNames[SelectedRow];
                        SelectedRow = 1;
                        TopRow = 0;
                        string errorMessage = GetFilesAndFolders();
                        if (errorMessage == "")
                            return;
                        else
                        {
                            UI.UsedDialog = new InfoDialogue(errorMessage);
                            UI.DialogIsOn = true;
                        }
                    }
                    else if (DirItemNames[SelectedRow].Substring(0, 1) == "\\")
                    {
                        if (DirPath.Length < 4) //if It's a Disk "C:\"
                        {
                            DirPath = DirPath.Substring(0, 2);
                        }
                        DirPath += DirItemNames[SelectedRow];
                        SelectedRow = 1;
                        TopRow = 0;
                        string errorMessage = GetFilesAndFolders();
                        if (errorMessage == "")
                            return;
                        else
                        {
                            UI.UsedDialog = new InfoDialogue(errorMessage);
                            UI.DialogIsOn = true;
                        }
                    }
                    else
                    {
                        GetFilesAndFolders();
                    }

                    SelectedRow = 1;
                    TopRow = 0;
                }
                else if (info.Key == ConsoleKey.UpArrow)
                {
                    if (SelectedRow > 1)
                        SelectedRow -= 1;

                    if (SelectedRow == TopRow)
                        TopRow -= 1;
                }
                else if (info.Key == ConsoleKey.DownArrow)
                {
                    if (IsSelectedDir && SelectedRow + 1 < DirItemNames.Count)
                    {
                        SelectedRow += 1;
                        if (SelectedRow > Console.WindowHeight - 7)
                            TopRow += 1;
                    }
                }
                else if (info.Key == ConsoleKey.F2) //MkDir
                {
                    UI.UsedDialog = new MkDirDialogue(DirPath);
                    UI.DialogIsOn = true;
                }
                else if (info.Key == ConsoleKey.F3) // Rename
                {
                    string selectedItem = DirItemNames[SelectedRow];

                    if (selectedItem == "\\..") // if parent dir is selected
                        UI.UsedDialog = new RenameDialogue(DirPath);
                    else if (selectedItem.Substring(0, 1) == "\\") // if folder is selected
                        UI.UsedDialog = new RenameDialogue(DirPath + selectedItem);
                    else // file is selected
                        UI.UsedDialog = new RenameDialogue(DirPath + '\\' + selectedItem.Substring(1, selectedItem.Length - 1));

                    UI.DialogIsOn = true;
                }
                else if (info.Key == ConsoleKey.F6) //Delete
                {
                    string selectedItem = DirItemNames[SelectedRow];
                    if (selectedItem == "\\..") // if parent dir is selected
                        UI.UsedDialog = new DeleteDialogue($"opravdu si přejete smazat složku: {selectedItem}",  DirPath, true);
                    else if (selectedItem.Substring(0, 1) == "\\") // if folder is selected
                        UI.UsedDialog = new DeleteDialogue($"opravdu si přejete smazat složku: {selectedItem}",  DirPath + '\\' + selectedItem, true);
                    else // file is selected
                        UI.UsedDialog = new DeleteDialogue($"opravdu si přejete smazat soubor: {selectedItem}",  DirPath + '\\' + selectedItem.Substring(1, selectedItem.Length - 1), false);

                    UI.DialogIsOn = true;
                }
                else if (info.Key == ConsoleKey.F7) // Open
                {
                    string selectedItem = DirItemNames[SelectedRow];
                    if (selectedItem.Substring(0, 1) == " ")
                    {
                        Editor.FilePath = DirPath + '\\' + selectedItem.Substring(1, selectedItem.Length - 1);
                        UI.Window = new EditorWindow();
                    }
                }
            }

            return;
        }
    }
}
