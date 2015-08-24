using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class Program
    {
        public static List<GameObject> GameObjects = new List<GameObject>();
        public static Snake player = new Snake();

        static void Main(string[] args)
        {
            var StartPos = new Position() { PosX = 40, PosY = 12 };
            player = new Snake()
            {
                Type = ObjectType.Player,
                MoveDirection = new MoveDirection() { Direction = Direction.Up },
                Blocks = new List<SnakeBlock>()
                {
                    new SnakeBlock() { IsHead = true, Symbol = "#", Position = StartPos },
                    new SnakeBlock() { IsHead = false, Symbol = "#", Position = new Position(){ PosX = StartPos.PosX - 1, PosY = StartPos.PosY } },
                    new SnakeBlock() { IsHead = false, Symbol = "#", Position = new Position(){ PosX = StartPos.PosX - 2, PosY = StartPos.PosY } }
                }
            };

            player.Blocks.ElementAt(1).Parent = player.Blocks.ElementAt(0);
            player.Blocks.ElementAt(2).Parent = player.Blocks.ElementAt(1);
            GameObjects.Add(new Treat() { Position = new Position() { PosX = 10, PosY = 10 }, Symbol = "@", Type = ObjectType.Treat });

            GameObjects.Add(player);

            Draw();

            while (true)
            {
                switch (Console.ReadKey().Key)
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
                            player.Eat();
                            break;
                        }
                }
                Tick();
            }
        }

        static void Tick()
        {
            CheckCollision();
            player.Move();
            Draw();
        }
        
        static void CheckCollision()
        {
            var Treats = GameObjects.Where(o => o.Type == ObjectType.Treat);
            foreach (var t in Treats)
            {
                if (t.Position.PosX == player.HeadBlock.Position.PosX && t.Position.PosY == player.HeadBlock.Position.PosY)
                {
                    player.Eat();
                    GameObjects.Remove(t);
                    break;
                }
            }
        }

        static void Draw()
        {
            Console.Clear();
            foreach (var GameObject in GameObjects)
            {
                GameObject.Draw();
            }
        }


        public class GameObject
        {
            public ObjectType Type { get; set; }
            public string Symbol { get; set; }
            public Position Position { get; set; }
            public virtual void Draw()
            {
                Console.SetCursorPosition(Position.PosX, Position.PosY);
                Console.Write(Symbol);
            }
        }

        public class SnakeBlock : GameObject
        {
            public bool IsHead { get; set; }
            public SnakeBlock Parent { get; set; }
        }

        public class Treat : GameObject
        {
        }

        public class Snake : GameObject
        {
            public int Length {
                get{return Blocks.Count;}
            }
            public SnakeBlock HeadBlock { get{return Blocks.First();}}
            public List<SnakeBlock> Blocks { get; set; }
            public MoveDirection MoveDirection { get; set; }
            public void Move()
            {
                for (int i = Blocks.Count - 1; i > 0; i--)
                {
                    if (!Blocks.ElementAt(i).IsHead)
                    {
                        Blocks.ElementAt(i).Position = new Position() {
                            PosX = Blocks.ElementAt(i).Parent.Position.PosX,
                            PosY = Blocks.ElementAt(i).Parent.Position.PosY, };
                    }
                }
                HeadBlock.Position.PosY += MoveDirection.Vertical;
                HeadBlock.Position.PosX += MoveDirection.Horizontal;
            }
            public void Eat() //todo: add food object
            {
                Position PosToAdd = HeadBlock.Position;
                Blocks.Insert(0, new SnakeBlock() { IsHead = true, Position = PosToAdd, Symbol = "#" });
                Blocks.ElementAt(1).IsHead = false;
                Blocks.ElementAt(1).Parent = Blocks.First();
            }
            public override void Draw()
            {
                foreach (var b in Blocks)
                {
                    Console.SetCursorPosition(b.Position.PosX, b.Position.PosY);
                    Console.Write(b.Symbol);
                }
            }

        }
        public class MoveDirection
        {
            public Direction Direction { get; set; }
            public int Vertical
            {
                get
                {
                    switch (Direction)
                    {
                        case Direction.Up:
                            {
                                return -1;
                            }
                        case Direction.Down:
                            {
                                return +1;
                            }
                        default: return 0;
                    }
                }
            }
            public int Horizontal {
                get
                {
                    switch (Direction)
                    {
                        case Direction.Right:
                            {
                                return +1;
                            }
                        case Direction.Left:
                            {
                                return -1;
                            }
                        default: return 0;
                    }
                } }
        }

        public enum Direction
        {
            Up, Down, Right, Left
        }

        public enum ObjectType
        {
            Player, Treat
        }
        public class Position
        {
            public int PosX { get; set; }
            public int PosY { get; set; }
        }
    }
}
