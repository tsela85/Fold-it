using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FoldIt
{
    class Ball
    {
        public const int ballSize = 10;

        //int x;
        //int y;
        Rectangle ballRec;
        Texture2D ballTex;

        float centerX, centerY;
        float tempX, tempY;

        public Ball(Texture2D tex, int posX, int posY)
        {
            ballRec = new Rectangle(posX, posY, ballSize, ballSize);
            ballTex = tex;

            centerX = centerX = 0;
            tempX = tempY = 0;
        }

        public void flipBall(Vector2 first,Vector2 second)
        {

            float x1 = first.X;
            float y1 = first.Y;
            float x2 = second.X;
            float y2 = second.Y;

            float m = (y2 - y1) / (x2 - x1);
            float c = y1 - x1*m;
            float m1 = -1 / m;
            float c1 = ballRec.Y - ballRec.X * m1;

            float radius = Vector2.Distance(new Vector2(ballRec.X, ballRec.Y), new Vector2(centerX, centerY));

            centerX = - (c - c1) / (m - m1);
            centerY = m1 * centerX + c1;


            float angle = MathHelper.Pi;

            float deltaX = (float)(Math.Cos(angle) * ballRec.X - Math.Sin(angle) * ballRec.Y);
            float deltaY = (float)(Math.Cos(angle) * ballRec.Y + Math.Sin(angle) * ballRec.X);

            tempX = centerX + deltaX;
            tempX = centerY + deltaY;
        }

        public void Draw(SpriteBatch spriteBatch, GameState gamestate)
        {
            spriteBatch.Draw(ballTex, ballRec,null, Color.Blue,0,new Vector2(ballSize/2,ballSize/2),SpriteEffects.None,0);
            spriteBatch.Draw(ballTex, new Rectangle((int)centerX,(int)centerY,30,30), null, Color.Blue, 0, new Vector2(ballSize / 2, ballSize / 2), SpriteEffects.None, 0);
            spriteBatch.Draw(ballTex, new Rectangle((int)tempX, (int)tempY, 30, 30), null, Color.DarkRed, 0, new Vector2(ballSize / 2, ballSize / 2), SpriteEffects.None, 0);
        }

    }
}
