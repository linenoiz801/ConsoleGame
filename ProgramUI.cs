using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame
{
    class ProgramUI
    {
        int arenaWidth = 80;
        int arenaHeight = 40;
        int snakeLength = 1;
        char[,] MainGrid;
        public void Run()
        {
            DrawMenu();
            PlayGame();
        }
        void DrawMenu()
        {
            int selection = 0;

            while (selection != 4)
            {
                Console.Clear();
                Console.WriteLine("------------------------------------------------");
                Console.WriteLine("|                                              |");
                Console.WriteLine("|           S N A K E    G A M E               |");
                Console.WriteLine("|            By Paul and Olivia                |");
                Console.WriteLine("|                                              |");
                Console.WriteLine($"|       1) Snake Length: {snakeLength}                     |");
                Console.WriteLine($"|       2) Arena Width: {arenaWidth}                     |");
                Console.WriteLine($"|       3) Arena Height: {arenaHeight}                    |");
                Console.WriteLine("|       4) Start Game                          |");
                Console.WriteLine("|                                              |");
                Console.WriteLine("------------------------------------------------");
                Console.Write("Enter selection: ");
                int.TryParse(Console.ReadLine(), out selection);

                switch (selection)
                {
                    case (1):
                        {
                            Console.Write("Enter starting snake length (1-9): ");
                            int.TryParse(Console.ReadLine(), out snakeLength);
                            if (snakeLength <= 0)
                            {
                                snakeLength = 1;
                            }
                            else if (snakeLength >= 9)
                            {
                                snakeLength = 9;
                            }
                            break;
                        }
                    case (2):
                        {
                            Console.Write("Enter desired arena width: ");
                            int.TryParse(Console.ReadLine(), out arenaWidth);
                            if (arenaWidth <= 10)
                            {
                                arenaWidth = 10;
                            }
                            break;
                        }
                    case (3):
                        {
                            Console.Write("Enter desired arena Height: ");
                            int.TryParse(Console.ReadLine(), out arenaHeight);
                            if (arenaHeight <= 10)
                            {
                                arenaHeight = 10;
                            }
                            break;
                        }
                    case (4):
                        {
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
        }
        void PlayGame()
        {
            MainGrid = new char[arenaWidth, arenaHeight];
            InitializeGrid();
            bool GameOver = false;
            Snake snake = new Snake(GetRandomPoint());
            snake.ArenaSize.X = arenaWidth;
            snake.ArenaSize.Y = arenaHeight;
            for (int x = 1; x < snakeLength; x++)
            {
                snake.AddSegment();
            }

            Console.CursorVisible = false;
            Console.SetWindowSize(arenaWidth, arenaHeight);
            Console.Clear();

            DrawScreen();

            while (!GameOver)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Escape)
                    {
                        GameOver = true;
                        break;
                    }
                    else if (key == ConsoleKey.UpArrow)
                    {
                        snake.ChangeDirection(Direction.Up);
                    }
                    else if (key == ConsoleKey.DownArrow)
                    {
                        snake.ChangeDirection(Direction.Down);
                    }
                    else if (key == ConsoleKey.LeftArrow)
                    {
                        snake.ChangeDirection(Direction.Left);
                    }
                    else if (key == ConsoleKey.RightArrow)
                    {
                        snake.ChangeDirection(Direction.Right);
                    }
                }
                if (snake.Move())
                {
                    if (MainGrid[snake.HeadPos.X, snake.HeadPos.Y] == 'π')
                    {
                        snake.AddSegment();
                        AddNibble();
                    }
                    UpdateGrid(snake);
                    DrawScreen();
                }
                else
                {
                    GameOver = true;
                }
                System.Threading.Thread.Sleep(10);
            }
            System.Threading.Thread.Sleep(1000);
            while (Console.KeyAvailable)
                Console.ReadKey();

            Console.SetCursorPosition(1, arenaHeight + 1);
            Console.CursorVisible = true;
            Console.WriteLine("You have died. Press any key to acknowledge that you are a loser.");
            Console.ReadKey();
        }

        void AddNibble()
        {
            Point point = GetRandomPoint();
            MainGrid[point.X, point.Y] = 'π';
        }
        Point GetRandomPoint()
        {
            Point point = new Point();
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            do
            {
                point.X = rand.Next(2, arenaWidth - 1);
                point.Y = rand.Next(2, arenaHeight - 1);
            } while (MainGrid[point.X, point.Y] != '.');

            return point;
        }

        void InitializeGrid()
        {
            for (int x = 0; x < arenaWidth; x++)
                for (int y = 0; y < arenaHeight; y++)
                    MainGrid[x, y] = '.';

            for (int x = 0; x < arenaWidth; x++)
            {
                MainGrid[x, 0] = '▀';
                MainGrid[x, arenaHeight - 1] = '▄';
            }
            for (int y = 0; y < arenaHeight; y++)
            {
                MainGrid[0, y] = '▐';
                MainGrid[arenaWidth - 1, y] = '▌';
            }
            AddNibble();
        }

        void DrawScreen()
        {
            for (int x = 1; x <= arenaWidth; x++)
            {
                for (int y = 1; y <= arenaHeight; y++)
                {
                    if (MainGrid[x - 1, y - 1] != '.')
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write(MainGrid[x - 1, y - 1]);
                    }
                    if (MainGrid[x - 1, y - 1] == ' ')
                        MainGrid[x - 1, y - 1] = '.';
                }
            }
        }

        void UpdateGrid(Snake snake)
        {
            foreach (Point point in snake.OccupiedCells)
            {
                MainGrid[point.X, point.Y] = '▓';
            }

            foreach (Point point in snake.ErasableCells)
            {
                MainGrid[point.X, point.Y] = ' ';
            }
            snake.ErasableCells.Clear();
        }

    }
}