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

        Rectangle ballRec;
        Rectangle board;
        Texture2D ballTex;

        Vector2 perpenPos;
        float radius;
        float angleBetweenPoints;
        float foldingAngle;
        Point ballAfterFolding;

        float timePassed;
        

        public  Ball(Texture2D tex, int posX, int posY,Rectangle innerBoard)
        {
            ballRec = new Rectangle(posX, posY, ballSize, ballSize);
            ballTex = tex;
            board = innerBoard;

            foldingAngle = 0;
            timePassed = 0;

        }

        public void calcBeforeFolding(Vector2 first,Vector2 second) 
        {
            float m = (second.Y - first.Y) / (second.X - first.X);
            float c = first.Y - first.X * m;
            float m1 = -1 / m;
            float c1 = ballRec.Y - ballRec.X * m1;

            perpenPos.X = -(c - c1) / (m - m1);
            perpenPos.Y = m1 * perpenPos.X + c1;

            angleBetweenPoints = (float)Math.Atan2(ballRec.Y - perpenPos.Y, ballRec.X - perpenPos.X);
            radius = Vector2.Distance(new Vector2(ballRec.X, ballRec.Y), new Vector2(perpenPos.X, perpenPos.Y));
        }

        public GameState flipBall(GameTime gameTime)
        {
            if (foldingAngle < MathHelper.Pi)
            {
                timePassed+=(float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timePassed > 0.02f)
                {
                    ballAfterFolding.X = (int)(radius * Math.Cos(angleBetweenPoints + foldingAngle) + perpenPos.X);
                    ballAfterFolding.Y = (int)(radius * Math.Sin(angleBetweenPoints + foldingAngle) + perpenPos.Y);
                    foldingAngle += 0.1f;
                    timePassed = 0;
                }
                return GameState.folding;
            } else 
            {
                if (board.Contains(ballAfterFolding))
                {
                    ballRec.X = ballAfterFolding.X;
                    ballRec.Y = ballAfterFolding.Y;
                }
                foldingAngle = 0;
                return GameState.ballMoved;
            }

        }

    
        public void Draw(SpriteBatch spriteBatch, GameState gamestate)
        {
            spriteBatch.Draw(ballTex, ballRec,null, Color.Blue,0,new Vector2(ballSize/2,ballSize/2),SpriteEffects.None,0);
        }

        public void DrawFolding(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ballTex, new Rectangle(ballAfterFolding.X ,ballAfterFolding.Y, 10, 10), null, Color.Gold, 0, new Vector2(ballSize / 2, ballSize / 2), SpriteEffects.None, 0);
        }

    }
}
