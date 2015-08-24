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

        public class Position
        {
            public int X { get; set; }
            public int Y { get; set; }
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

    }
}
