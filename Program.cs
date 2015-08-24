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
        public static List<GameObject> GameObjects = new List<GameObject>();    //Collection of all objects which should be drawn on the screens
        public static Snake player = new Snake();                               //player object

        static void Main(string[] args)
        {
            var StartPos = new Position() { X = 40, Y = 12 };             //Center of the screen
            player = new Snake()
            {
                Type = ObjectType.Player,
                MoveDirection = new MoveDirection() { Direction = Direction.Up },
                Blocks = new List<SnakeBlock>()
                {   //Starting snake length is 3 blocks.
                    new SnakeBlock() { IsHead = true, Symbol = "#", Position = StartPos },
                    new SnakeBlock() { IsHead = false, Symbol = "#", Position = new Position(){ X = StartPos.X - 1, Y = StartPos.Y } },
                    new SnakeBlock() { IsHead = false, Symbol = "#", Position = new Position(){ X = StartPos.X - 2, Y = StartPos.Y } }
                }
            };
            player.Blocks.ElementAt(1).Parent = player.Blocks.ElementAt(0); //we actually need to hardcode parents for existing blocks. 
            player.Blocks.ElementAt(2).Parent = player.Blocks.ElementAt(1); //todo: override player.blocks.add() to automaticaly setup parents, or make parents auto-assigning
                             
            //Adding objects to GameObjects or they won't be drawn.   
            GameObjects.Add(player);
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
        
        static void CheckCollision() //collision checking. TODO: Check all the collisions, get it types and invoke other operations
        {
            var Treats = GameObjects.Where(o => o.Type == ObjectType.Treat);
            foreach (var t in Treats)
            {
                if (t.Position.X == player.HeadBlock.Position.X && t.Position.Y == player.HeadBlock.Position.Y)
                {
                    player.Eat(t);
                    break;
                }
            }
        }

        static void Draw() //redraws screen
        {
            Console.Clear();
            foreach (var GameObject in GameObjects)
            {
                GameObject.Draw();
            }
        }

        public enum ObjectType
        {
            Player, Treat
        }

        public class GameObject //probably must be an interface.
        {
            public ObjectType Type { get; set; }
            public string Symbol { get; set; }
            public Position Position { get; set; }
            public virtual void Draw()
            {
                Console.SetCursorPosition(Position.X, Position.Y);
                Console.Write(Symbol);
            }
        }

        public class Treat : GameObject
        {
            //prop Points
        }

        public class SnakeBlock : GameObject
        {
            public bool IsHead { get; set; }
            public SnakeBlock Parent { get; set; }
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
                    if (!Blocks.ElementAt(i).IsHead)  //Every move each block take it's parent position, except for head, which takes new position according to snake movement direction
                    {
                        Blocks.ElementAt(i).Position = new Position()
                        {  //we MUST instantiate new position object, and I can't explain why. Try setting it this way: Blocks.ElementAt(i).Position = Blocks.ElementAt(i).Parent.Position; and it doesn't work
                            X = Blocks.ElementAt(i).Parent.Position.X,
                            Y = Blocks.ElementAt(i).Parent.Position.Y, };
                    }
                }
                HeadBlock.Position.Y += MoveDirection.Vertical;  //this approach saves long-ass switch(direction) train
                HeadBlock.Position.X += MoveDirection.Horizontal;
            }
            public void Eat(GameObject t) 
            {
                Position EatLocation = t.Position;
                Blocks.Insert(0, new SnakeBlock() { IsHead = true, Position = EatLocation, Symbol = "#" });
                Blocks.ElementAt(1).IsHead = false;
                Blocks.ElementAt(1).Parent = Blocks.First();
                GameObjects.Remove(t);
            }
            public override void Draw()  //overrides default draw method to draw each block. 
            {
                foreach (var b in Blocks)
                {
                    Console.SetCursorPosition(b.Position.X, b.Position.Y);
                    Console.Write(b.Symbol);
                }
            }

        }
        public enum Direction
        {
            Up, Down, Right, Left
        }
        public class MoveDirection //pretty complicated approach, but saves a lot of code
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



        public class Position
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
