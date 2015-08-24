using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Snake.Core;

namespace Snake
{
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

        public int Length
        {
            get { return Blocks.Count; }
        }
        public SnakeBlock HeadBlock { get { return Blocks.First(); } }
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
                        Y = Blocks.ElementAt(i).Parent.Position.Y,
                    };
                }
            }
            HeadBlock.Position.Y += MoveDirection.Vertical;  //this approach saves long-ass switch(direction) train
            HeadBlock.Position.X += MoveDirection.Horizontal;
        }
        public void Eat(GameObject t)
        {
            Add();
            Program.GameObjects.Remove(t);
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
            Program.GameObjects.Add(Blocks.First());
            Blocks.ElementAt(1).IsHead = false;
            Blocks.ElementAt(1).Parent = Blocks.First();
        }

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
        public int Horizontal
        {
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
            }
        }
    }
}
