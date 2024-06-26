﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Media;



namespace Pacman.GameClasses
{


    class PacMan
    {

        private Position pacManPos;
        private int score;
        private int lives;
        private int level;

        private string symbol = ((char)9786).ToString();
        private ConsoleColor color = ConsoleColor.Yellow;
        public string Direction = "right";
        public string NextDirection = "right";



        private GameBoard gameBoard;

        public int GetScore()
        {
            return this.score;




        }

        public void IncreaseScore(int points) => score += points;

        public int Score => score;



        public int Lives()
        {
            return this.lives;
        }

        public int GetLevel()
        {
            return this.level;
        }

        public PacMan(GameBoard gameBoard)
        {
            this.pacManPos = new Position(17, 20);
            this.score = 0;
            this.lives = 3;
            this.level = 1;
            this.gameBoard = gameBoard;
        }

        public void ResetPacMan()
        {
            this.pacManPos.X = 17;
            this.pacManPos.Y = 20;
            this.Direction = "right";
            this.NextDirection = "right";
        }

        public void LoseLife()
        {
            this.lives--;
            DeathPlayerMusic();
            Thread.Sleep(600);
        }

        public void EarnPoint()
        {
            this.score++;
        }

        public void EarnStar()
        {
            this.score += 100;
        }

        public void LevelUp()
        {
            this.level++;
            this.score = 0;
        }

        public string GetSymbol()
        {
            return this.symbol;
        }

        public int GetPosX()
        {
            return this.pacManPos.X;
        }

        public int GetPosY()
        {
            return this.pacManPos.Y;
        }

        public ConsoleColor GetColor()
        {
            return this.color;
        }

        public void ResetScore()
        {
            this.score = 0;



        }

        public void ResetLives()
        {
            this.lives = 3;
        }

        public void ResetLevel()
        {
            this.level = 1;
        }


        public BoardElements CheckCell(string[,] border, string direction, Monster[] monsterList)
        {
            switch (direction)
            {
                case "up":
                    switch (gameBoard.GetBoard[this.pacManPos.Y - 1, this.pacManPos.X])
                    {
                        case "#":
                            return BoardElements.Wall;
                        case ".":
                            return BoardElements.Dot;
                        case "*":
                            return BoardElements.Star;
                        //case "☻":
                        //  return BoardElements.Monster;
                        default:
                            if (checkIfMonsterAppears(monsterList, this.pacManPos.Y - 1, this.pacManPos.X))
                            {
                                return BoardElements.Monster;
                            }
                            else
                            {
                                return BoardElements.Empty;
                            }
                    }

                //return BoardElements.Empty;
                case "right":
                    switch (gameBoard.GetBoard[this.pacManPos.Y, this.pacManPos.X + 1])
                    {
                        case "#":
                            return BoardElements.Wall;
                        case ".":
                            return BoardElements.Dot;
                        case "*":
                            return BoardElements.Star;
                        default:
                            if (checkIfMonsterAppears(monsterList, this.pacManPos.Y, this.pacManPos.X + 1))
                            {
                                return BoardElements.Monster;
                            }
                            else
                            {
                                return BoardElements.Empty;
                            }
                    }

                //return BoardElements.Empty;
                case "down":
                    switch (gameBoard.GetBoard[this.pacManPos.Y + 1, this.pacManPos.X])
                    {
                        case "#":
                            return BoardElements.Wall;
                        case ".":
                            return BoardElements.Dot;
                        case "*":
                            return BoardElements.Star;
                        default:
                            if (checkIfMonsterAppears(monsterList, this.pacManPos.Y + 1, this.pacManPos.X))
                            {
                                return BoardElements.Monster;
                            }
                            else
                            {
                                return BoardElements.Empty;
                            }
                    }

                //return BoardElements.Empty;
                case "left":
                    switch (gameBoard.GetBoard[this.pacManPos.Y, this.pacManPos.X - 1])
                    {
                        case "#":
                            return BoardElements.Wall;
                        case ".":
                            return BoardElements.Dot;
                        case "*":
                            return BoardElements.Star;
                        default:
                            if (checkIfMonsterAppears(monsterList, this.pacManPos.Y, this.pacManPos.X - 1))
                            {
                                return BoardElements.Monster;
                            }
                            else
                            {
                                return BoardElements.Empty;
                            }
                    }

                //return BoardElements.Empty;
                default:
                    if (checkIfMonsterAppears(monsterList, pacManPos.Y, pacManPos.X))
                    {
                        return BoardElements.Monster;
                    }
                    else
                    {
                        return BoardElements.Empty;
                    }
            }
            //return BoardElements.Empty;
        }
        public void MoveUp()
        {
            if (gameBoard.GetBoard[this.pacManPos.Y - 1, this.pacManPos.X] != "#")
            {
                this.pacManPos.Y -= 1;
            }
        }
        public void MoveDown()
        {
            if (gameBoard.GetBoard[this.pacManPos.Y + 1, this.pacManPos.X] != "#")
            {
                this.pacManPos.Y += 1;
            }
        }
        public void MoveLeft()
        {
            if (gameBoard.GetBoard[this.pacManPos.Y, this.pacManPos.X - 1] != "#")
            {
                this.pacManPos.X -= 1;
            }
        }
        public void MoveRight()
        {
            if (gameBoard.GetBoard[this.pacManPos.Y, this.pacManPos.X + 1] != "#")

            {
                this.pacManPos.X += 1;

            }
        }

        public bool checkIfMonsterAppears(Monster[] monsterList, int pacManPosY, int pacManPosX)
        {
            foreach (var monster in monsterList)
            {
                if (monster.GetPosX() == pacManPosX && monster.GetPosY() == pacManPosY)
                {
                    return true;
                }


            }

            return false;
        }


        public void DeathPlayerMusic()
        {
            SoundPlayer death = new SoundPlayer(Pacman.PacManMusic.pacman_death);
            death.Play();
        }

    }
}