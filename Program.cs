using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Snake.Core;

namespace Snake
{
    public class Program
    {
        public static List<GameObject> GameObjects = new List<GameObject>();    //Collection of all objects which should be drawn on the screens
        public static Snake player = new Snake();                               //player object
        public static bool lost = false;

        static void Main(string[] args)
        {
            int boost;
            double sleepTime = 200;
            var StartPos = new Position() { X = 40, Y = 12 };             //Center of the screen
            player = new Snake()
            {
                MoveDirection = new MoveDirection() { Direction = Direction.Up },
                Blocks = new List<Snake.SnakeBlock>()
                {   //Starting snake length is 3 blocks.
                    new Snake.SnakeBlock() { IsHead = true, Symbol = "#", Position = StartPos, Type = ObjectType.Player}
                }
            };
            GameObjects.Add(player.Blocks.First());
            player.Add();    //let snake be 3 blocks long
            player.Add();
            player.Move();
            player.Move();
            //Adding objects to GameObjects or they won't be drawn.  
            GameObjects.Add(new Treat() { Position = new Position() { X = 10, Y = 10 }, Symbol = "@", Type = ObjectType.Treat });

            Console.WriteLine("Press any key to start the game!");

            while (!lost)
            {
                var tickNow = DateTime.Now;
                if (Console.KeyAvailable)
                {
                    switch (Console.ReadKey().Key)  //screen updates only when key is pressed, should be replaced with auto-movement
                    {
                        case ConsoleKey.UpArrow:
                            {
                                if (player.MoveDirection.Direction != Direction.Down)
                                {
                                    player.MoveDirection.Direction = Direction.Up;
                                }
                                break;
                            }
                        case ConsoleKey.DownArrow:
                            {
                                if (player.MoveDirection.Direction != Direction.Up)
                                {
                                    player.MoveDirection.Direction = Direction.Down;
                                }
                                break;
                            }
                        case ConsoleKey.LeftArrow:
                            {
                                if (player.MoveDirection.Direction != Direction.Right)
                                {
                                    player.MoveDirection.Direction = Direction.Left;
                                }
                                break;
                            }
                        case ConsoleKey.RightArrow:
                            {
                                if (player.MoveDirection.Direction != Direction.Left)
                                {
                                    player.MoveDirection.Direction = Direction.Right;
                                }
                                break;
                            }
                    }
                }
                Tick();
                sleepTime -= 0.01;
                Thread.Sleep((int)(sleepTime));
            }

            Console.SetCursorPosition(0,0);
            Console.Write("You lost!");
            Console.ReadLine();

        }

        static void Tick() //Does all the logic
        {
            CheckCollision();
            player.Move();
            Draw();
        }
        
        static void Draw() //redraws screen
        {
            Console.Clear();
            Console.Write("Score:" + player.Score);
            foreach (var GameObject in GameObjects)
            {
                GameObject.Draw();
            }
        }
    }
}
