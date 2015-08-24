using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Snake.Core;

namespace Snake
{
    public class Program
    {
        public static List<GameObject> GameObjects = new List<GameObject>();    //Collection of all objects which should be drawn on the screens
        public static Snake player = new Snake();                               //player object

        static void Main(string[] args)
        {
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

            //initial draw
            Draw();

            while (true)
            {
                switch (Console.ReadKey().Key)  //screen updates only when key is pressed, should be replaced with auto-movement
                {
                    case ConsoleKey.UpArrow:
                        {
                            player.MoveDirection.Direction = Direction.Up;
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        {
                            player.MoveDirection.Direction = Direction.Down;
                            break;
                        }
                    case ConsoleKey.LeftArrow:
                        {
                            player.MoveDirection.Direction = Direction.Left;
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        {
                            player.MoveDirection.Direction = Direction.Right;
                            break;
                        }
                    case ConsoleKey.Spacebar:
                        {
                            player.Eat(new Treat() { Position = player.HeadBlock.Position, Type = ObjectType.Treat });
                            break;
                        }
                }
                Tick();
            }
        }

        static void Tick() //Should be invoked once per some time. Must do all the logic
        {

            CheckCollision();
            player.Move();
            Draw();
        }
        
        static void Draw() //redraws screen
        {
            Console.Clear();
            foreach (var GameObject in GameObjects)
            {
                GameObject.Draw();
            }
        }
    }
}
