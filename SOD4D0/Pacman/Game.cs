﻿using Pacman.GameClasses;
using System;
using System.Threading.Tasks;
using System.Media;
using System.Threading;

namespace Pacman
{
    class Game
    {
        // Global Declarations

        static Random random = new Random();
        static bool gamePaused = false;
        static bool pausedTextIsShown = false;
        static bool continueLoop = true;

        // Game Board
        static GameBoard board = new GameBoard();
        static string[,] border = board.GetBoard;

        // Player
        static PacMan pacman = new PacMan(board);

        // Monsters
        static Monster[] monsterList =
        {
            new Monster(ConsoleColor.Red,15,8),
            new Monster(ConsoleColor.Cyan,16,12),
            new Monster(ConsoleColor.Magenta,17,12),
            new Monster(ConsoleColor.DarkCyan,18,12),

        };

        // Console Settings
        const int GameWidth = 70;
        const int GameHeight = 29;

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Console.Title = "Pacman by Janos";
            Console.WindowWidth = GameWidth;
            Console.BufferWidth = GameWidth;
            Console.WindowHeight = GameHeight;
            Console.BufferHeight = GameHeight;
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            ShowWelcomeMenu();

            RedrawBoard();
            LoadGUI();

            LoadPlayer();

            LoadMonsters();

            while (continueLoop)
            {

                ReadUserKey();

                // Check if paused
                if (gamePaused)
                {
                    BlinkPausedText();
                    continue;
                }


                PlayerMovement();

                CheckIfNoLives();

                CheckScore();

                Thread.Sleep(200);
            }
        }

        static void LoadGUI()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(40, 2);
            Console.Write("Level: {0}", pacman.GetLevel());
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(40, 4);
            Console.Write("Score: {0}", pacman.GetScore());
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(40, 6);
            Console.Write("Lives: {0}", pacman.Lives());
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(40, GameHeight - 8);
            Console.Write("{0}", new string('-', 22));
            Console.SetCursorPosition(40, GameHeight - 7);
            Console.Write("|  PRESS P TO PAUSE  |");
            Console.SetCursorPosition(40, GameHeight - 6);
            Console.Write("|  PRESS ESC TO EXIT |");
            Console.SetCursorPosition(40, GameHeight - 4);
            Console.Write("{0}", new string('-', 22));
        }

        static void LoadPlayer()
        {
            Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY());
            Console.ForegroundColor = pacman.GetColor();
            Console.Write(pacman.GetSymbol());
        }

        static void LoadMonsters()
        {
            foreach (var monster in monsterList)
            {
                Console.ForegroundColor = monster.GetColor();
                Console.SetCursorPosition(monster.GetPosX(), monster.GetPosY());
                Console.Write(monster.GetSymbol());
            }

        }

        static void ReadUserKey()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.Escape:
                        continueLoop = false;
                        break;
                    case ConsoleKey.P:
                        SetGamePaused();
                        break;
                    case ConsoleKey.UpArrow:
                        pacman.NextDirection = "up";
                        break;
                    case ConsoleKey.DownArrow:
                        pacman.NextDirection = "down";
                        break;
                    case ConsoleKey.LeftArrow:
                        pacman.NextDirection = "left";
                        break;
                    case ConsoleKey.RightArrow:
                        pacman.NextDirection = "right";
                        break;

                }
            }
        }


        static void SetGamePaused()
        {
            switch (gamePaused)
            {
                case false:
                    ShowPausedText(true);
                    break;
                case true:
                    ShowPausedText(false);
                    break;
            }

            gamePaused = gamePaused ? false : true;
        }

        static void BlinkPausedText()
        {
            switch (pausedTextIsShown)
            {
                case true:
                    Thread.Sleep(100);
                    ShowPausedText(false);
                    break;
                case false:
                    Thread.Sleep(100);
                    ShowPausedText(true);
                    break;
            }
        }

        static void ShowPausedText(bool showText)
        {
            switch (showText)
            {
                case true:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.SetCursorPosition(47, GameHeight - 2);
                    Console.Write("PAUSED");
                    pausedTextIsShown = true;
                    break;
                case false:
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(47, GameHeight - 2);
                    Console.Write("      ");
                    pausedTextIsShown = false;
                    break;
            }
        }

        static void PlayerMovement()
        {
            switch (pacman.CheckCell(border, pacman.NextDirection, monsterList))
            {
                case BoardElements.Dot:
                    MovePlayer(pacman.NextDirection);
                    pacman.EarnPoint();
                    pacman.Direction = pacman.NextDirection;
                    LoadGUI();
                    break;
                case BoardElements.Star:
                    MovePlayer(pacman.NextDirection);
                    pacman.EarnStar();
                    pacman.Direction = pacman.NextDirection;
                    LoadGUI();
                    break;
                case BoardElements.Empty:
                    MovePlayer(pacman.NextDirection);
                    pacman.Direction = pacman.NextDirection;
                    break;
                case BoardElements.Monster:
                    pacman.LoseLife();
                    MovePlayer("reset");
                    LoadGUI();
                    break;
                case BoardElements.Wall:
                    switch (pacman.CheckCell(border, pacman.Direction, monsterList))
                    {
                        case BoardElements.Dot:
                            MovePlayer(pacman.Direction);
                            pacman.EarnPoint();
                            LoadGUI();
                            break;
                        case BoardElements.Star:
                            MovePlayer(pacman.Direction);
                            pacman.EarnStar();
                            LoadGUI();
                            break;
                        case BoardElements.Empty:
                            MovePlayer(pacman.Direction);
                            break;
                        case BoardElements.Monster:
                            pacman.LoseLife();
                            MovePlayer("reset");
                            LoadGUI();
                            break;
                        case BoardElements.Wall:
                            break;
                    }
                    break;
            }
        }
        static void MovePlayer(string direction)
        {
            switch (direction)
            {
                case "up":
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY());
                    Console.Write(" ");
                    ChangeBoard();
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY() - 1);
                    Console.ForegroundColor = pacman.GetColor();
                    Console.Write(pacman.GetSymbol());
                    pacman.MoveUp();
                    break;
                case "right":
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY());
                    Console.Write(" ");
                    ChangeBoard();
                    Console.SetCursorPosition(pacman.GetPosX() + 1, pacman.GetPosY());
                    Console.ForegroundColor = pacman.GetColor();
                    Console.Write(pacman.GetSymbol());
                    pacman.MoveRight();
                    break;
                case "down":
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY());
                    Console.Write(" ");
                    ChangeBoard();
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY() + 1);
                    Console.ForegroundColor = pacman.GetColor();
                    Console.Write(pacman.GetSymbol());
                    pacman.MoveDown();
                    break;
                case "left":
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY());
                    Console.Write(" ");
                    ChangeBoard();
                    Console.SetCursorPosition(pacman.GetPosX() - 1, pacman.GetPosY());
                    Console.ForegroundColor = pacman.GetColor();
                    Console.Write(pacman.GetSymbol());
                    pacman.MoveLeft();
                    break;
                case "reset":
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY());
                    Console.Write(" ");
                    ChangeBoard();
                    pacman.ResetPacMan();
                    Console.SetCursorPosition(pacman.GetPosX(), pacman.GetPosY());
                    Console.ForegroundColor = pacman.GetColor();
                    Console.Write(pacman.GetSymbol());
                    break;
            }
        }

        static void CheckScore()
        {
            if (pacman.GetScore() == 684)
            {
                continueLoop = false;
                WinGame();
            }
        }

        static void CheckIfNoLives()
        {
            if (pacman.Lives() < 0)
            {
                continueLoop = false;
            }

        }

        static void RedrawBoard()
        {
            // Előző játékmezők törlése
            Console.Clear();

            for (int i = 0; i < board.GetBoard.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetBoard.GetLength(1); j++)
                {
                    Console.Write("{0}", board.GetBoard[i, j]);
                }
                Console.WriteLine();
            }
        }
        static void ChangeBoard()
        {
            border[pacman.GetPosY(), pacman.GetPosX()] = " ";
        }

        static void ShowWelcomeMenu()
        {

            RedrawBoard();

            int horizontalPos = GameHeight / 2 - 2;
            int verticalPos = GameWidth / 2 - 15;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(verticalPos, horizontalPos);
            Console.Write("|{0}|", new string('-', 28));
            Console.SetCursorPosition(verticalPos, horizontalPos + 1);
            Console.Write("||     PRESS X TO START     ||");
            Console.SetCursorPosition(verticalPos, horizontalPos + 2);
            Console.Write("||     PRESS ESC TO EXIT    ||");
            Console.SetCursorPosition(verticalPos, horizontalPos + 3);
            Console.Write("|{0}|", new string('-', 28));
            Console.ForegroundColor = ConsoleColor.White;

            ConsoleKeyInfo keyPressed = Console.ReadKey(true);
            while (true)
            {
                if (keyPressed.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
                else if (keyPressed.Key == ConsoleKey.X)
                {
                    Console.Clear();
                    break;
                }

                keyPressed = Console.ReadKey(true);
            }
        }

        static void WinGame()
        {
            Console.Clear();
            RedrawBoard();

            int horizontalPos = GameHeight / 2 - 2;
            int verticalPos = GameWidth / 2 - 15;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(verticalPos, horizontalPos);
            Console.Write("|{0}|", new string('-', 27));
            Console.SetCursorPosition(verticalPos, horizontalPos + 1);
            Console.Write("||        YOU WON!         ||");
            Console.SetCursorPosition(verticalPos, horizontalPos + 2);
            Console.Write("||                         ||");
            Console.SetCursorPosition(verticalPos, horizontalPos + 3);
            int score = pacman.GetScore();
            Console.Write("||       SCORE: {0}{1}  ||", score, new string(' ', 9 - score.ToString().Length));
            Console.SetCursorPosition(verticalPos, horizontalPos + 4);
            Console.Write("||                         ||");
            Console.SetCursorPosition(verticalPos, horizontalPos + 5);
            Console.Write("||    PRESS ESC TO EXIT    ||");
            Console.SetCursorPosition(verticalPos, horizontalPos + 6);
            Console.Write("|{0}|", new string('-', 27));
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(0, GameHeight - 1);

            ConsoleKeyInfo keyPressed = Console.ReadKey(true);
            while (true)
            {
                if (keyPressed.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }

                keyPressed = Console.ReadKey(true);
            }
        }

    }
}