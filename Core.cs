using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public static class Core
    {
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

        public enum Direction
        {
            Up, Down, Right, Left
        }

        public static Position NextPos(this Direction dir, int steps)
        {
            int x = 0,
                y = 0;
            switch (dir)
            {
                case Direction.Up: y = 1; break;
                case Direction.Down: y = -1; break;
                case Direction.Left: x = -1; break;
                case Direction.Right: x = 1; break;
            }
            x *= steps;
            y *= steps;
            return new Position() { X = x, Y = y };
        }

        public class Position
        {
            public int X { get; set; }
            public int Y { get; set; }

            public static Position operator +(Position p1, Position p2)
            {
                return new Position() { X = p1.X + p2.X, Y = p1.Y + p2.Y };
            }
       }

        public static void CheckCollision() //collision checking. TODO: Check all the collisions, get it types and invoke other operations
        {
            List<GameObject> tmp_GameObjects = Program.GameObjects.ToList(); //some weird shitty code to make a new instance of gameobjects, so that if object is deleted on collision - don't break an app

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

        public static List<GameObject> Add(this List<GameObject> list, Snake snk)
        {
            foreach(var block in snk.Blocks)
            {
                list.Add(block);
            }
            return list;
        }

    }
}
