using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace FoldIt
{
    class Goal
    {
        Texture2D goalTex;
        Rectangle goalRec;

        public Goal(Texture2D tex, int posX, int posY,int goalSize)
        {
            goalTex = tex;
            goalRec.X = posX;
            goalRec.Y = posY;
            goalRec.Width = goalRec.Height = goalSize;
        }

        public bool isGoal(Rectangle ball)
        {
            return goalRec.Contains(ball.Center);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(goalTex, goalRec, null, Color.LightSeaGreen , 0, new Vector2(goalRec.Width / 2, goalRec.Height / 2), SpriteEffects.None, 0);
        }
    }
}
