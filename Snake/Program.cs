using System;
using System.Collections.Generic;
using System.Threading;
namespace Snake
{
    internal class Program
    {
        // Variables
        public static int mode;
        public static Thread drawthread;
        public static ConsoleKeyInfo input;
        public static Tuple<int, int> xy = new Tuple<int, int>(0, 0);
        public static Tuple<int, int> xyfood = new Tuple<int, int>(0, 0);
        public static List<Tuple<int, int>> pixels = new List<Tuple<int, int>>();
        public static int foodscore = 0;
        public static bool lost = false;
        public static bool re = false;
        public static int time = 0;
        static void Main(string[] args)
        {
            // Tutorial
            Console.WriteLine("Rules: Get the F's! Dont bump into walls! If you do, press any key to try again! Use arrow keys to control the movement.");
            Console.WriteLine("Press any key to start!");
            Console.ReadKey();
            ThreadStart predraw = new ThreadStart(Draw);
            drawthread = new Thread(predraw);
            drawthread.Start();
            Console.CursorVisible = false;

            do
            {
                if (lost == true)
                {
                    // Lose dialouge
                    lost = false;
                    Console.Clear();
                    Console.WriteLine("You lost! Press any key to try again. Score: " + foodscore);
                    Console.ReadKey();
                    Console.Clear();
                    xy = new Tuple<int, int>(0, 0);
                    pixels.Clear();
                    drawthread = new Thread(predraw);
                    drawthread.Start();
                    foodscore = 0;
                }
                input = Console.ReadKey();
                // Inputs
                if (input.Key == ConsoleKey.LeftArrow)
                {
                    mode = 1;
                }
                if (input.Key == ConsoleKey.RightArrow)
                {
                    mode = 2;
                }
                if (input.Key == ConsoleKey.DownArrow)
                {
                    mode = 3;
                }
                if (input.Key == ConsoleKey.UpArrow)
                {
                    mode = 4;
                }
                
            } while (true);
        }
        static void Draw()
        {
            try
            {
                CollectFood();
                while (true)
                {

                    Thread.Sleep(50);
                    time++;
                    Console.Clear();
                    Console.Write("Score: "+foodscore);
                    // Make food
                    Console.SetCursorPosition(xyfood.Item1, xyfood.Item2);
                    Console.Write("F");
                    // Set back to original position
                    Console.SetCursorPosition(xy.Item1, xy.Item2);
                    //Directions on inputs
                    if (mode == 1)
                    {
                        //Left
                        
                            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        
                    }
                    if (mode == 2)
                    {
                        //Right
                        
                            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                    }
                    if (mode == 3)
                    {
                        //Down
                        
                            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop + 1);
                    }
                    if (mode == 4)
                    {
                        
                            Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                    }
                    // Define head
                    xy = new Tuple<int, int>(Console.CursorLeft, Console.CursorTop);
                    // Draw pixels
                    foreach (Tuple<int, int> cord in pixels)
                    {
                        Console.SetCursorPosition(cord.Item1, cord.Item2);
                        Console.Write("O");
                        int i = 0;
                    }
                    // Throw fake exception if bumped into self to trigger the crash dialouge
                    foreach (Tuple<int, int> cord in pixels)
                    {
                        if (xy.Item1 == cord.Item1 && xy.Item2 == cord.Item2)
                        {
                            throw new System.IO.IOException();
                        }
                    }
                    // Add head as pixel
                    pixels.Add(xy);
                    // Snake management
                    if (re)
                    {
                        // Turn on growth in snake and turn it off after
                        re = false;
                        

                    }
                    else
                    {
                        // Turn off growth in snake
                        pixels.RemoveAt(0);
                    }
                    if (xy.Item1 == xyfood.Item1 && xy.Item2 == xyfood.Item2)
                    {
                        // React to food
                        foodscore += 1;
                        CollectFood();
                        // Turn growth switch
                        re = true;
                    }

                }
            }
            // Exception means lost
            // If snake is out of bounds it will throw an exception causing the crash dialouge also.
            catch
            {
                // Crash dialouge (Redirects to lose dialouge)
                Console.SetCursorPosition(0, 0);
                lost = true;
                Console.Beep();
                Console.WriteLine("Oh no! You crashed into yourself, or you bumped into a wall. Press any key to see your high score.");
            }
        }
        static void CollectFood()
        {
            //Generate food position
            Random random = new Random();
            int x = random.Next(1, 40);
            int y = random.Next(1, 20);
            Console.SetCursorPosition(x, y);
            Console.Write("F");
            xyfood = new Tuple<int, int>(x, y);
        }
    }
}
