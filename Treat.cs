using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Snake.Core;

namespace Snake
{
    public class Treat : GameObject
    {
        //prop Points
        public override void OnCollide(GameObject obj2)
        {
            if (obj2.Type == ObjectType.Player)
            {
                Program.player.Eat(this);
            }
        }
    }
}
