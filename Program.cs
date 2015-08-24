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
        
        static void CheckCollision() //collision checking. TODO: Check all the collisions, get it types and invoke other operations
        {
            List<GameObject> tmp_GameObjects = GameObjects.ToList(); //some weird shitty code to make a new instance of gameobjects, so that if object is deleted on collision - don't break an app

            foreach (var g in tmp_GameObjects)
            {
                foreach (var g1 in tmp_GameObjects.Where(d => d != g))                  //for each gameobject check all the other objects if it's position is same. 
                {
                    if (g.Position.X == g1.Position.X && g.Position.Y == g1.Position.Y)
                    {
                        g.OnCollide(g1);                                                 
                    }
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

            public virtual void OnCollide(GameObject obj2) { }
        }

        public class Treat : GameObject
        {
            //prop Points
            public override void OnCollide(GameObject obj2)
            {
                if (obj2.Type == ObjectType.Player)
                {
                    player.Eat(this);
                }
            }
        }



        public class Snake
        {
            public class SnakeBlock : GameObject
            {
                public bool IsHead { get; set; }
                public SnakeBlock Parent { get; set; }
                public override void OnCollide(GameObject obj2)
                {
                    if (IsHead && obj2.Type == ObjectType.Player)
                    {
                        Debug.Print("LOST");
                    }
                }
            }

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
                Add();
                GameObjects.Remove(t);
            }
            public void Draw()  //overrides default draw method to draw each block. 
            {
                foreach (var b in Blocks)
                {
                    Console.SetCursorPosition(b.Position.X, b.Position.Y);
                    Console.Write(b.Symbol);
                }
            }
            public void Add()
            {
                Position AddLocation = HeadBlock.Position;
                Blocks.Insert(0, new SnakeBlock() { IsHead = true, Position = AddLocation, Symbol = "#" });
                GameObjects.Add(Blocks.First());
                Blocks.ElementAt(1).IsHead = false;
                Blocks.ElementAt(1).Parent = Blocks.First();
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
