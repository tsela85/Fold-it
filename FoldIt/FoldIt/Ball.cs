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

        Vector2 center;
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

        public void calcBeforeFolding(EdgePosition first, EdgePosition second) 
        {
            Vector2 foldLine;
            Vector2 perpendicular;

            foldLine.X = (float)((float)(second.y - (float)first.y) / ((float)second.x - (float)first.x));
            perpendicular.X = -1 / foldLine.X;

            if (foldLine.X == 0) //fold line is horizontal
            {
                center.X = ballRec.X;
                center.Y = first.y;
            }
            else
            {
                if (perpendicular.X == 0) //fold line is vertical
                {
                    center.X = first.x;
                    center.Y = ballRec.Y;
                }
                else
                {
                    foldLine.Y = (float)((float)first.y - (float)first.x * foldLine.X);
                    perpendicular.Y = ballRec.Y - ballRec.X * perpendicular.X;

                    center.X = -(foldLine.Y - perpendicular.Y) / (foldLine.X - perpendicular.X);
                    center.Y = perpendicular.X * center.X + perpendicular.Y;
                }
            }
            angleBetweenPoints = (float)Math.Atan2(ballRec.Y - center.Y, ballRec.X - center.X);
            radius = Vector2.Distance(new Vector2(ballRec.X, ballRec.Y), new Vector2(center.X, center.Y));
        }

        public GameState flipBall(GameTime gameTime)
        {
            if (foldingAngle < MathHelper.Pi)
            {
                timePassed+=(float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timePassed > 0.02f)
                {
                    ballAfterFolding.X = (int)(radius * Math.Cos(angleBetweenPoints + foldingAngle) + center.X);
                    ballAfterFolding.Y = (int)(radius * Math.Sin(angleBetweenPoints + foldingAngle) + center.Y);
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

                // enables to draw a line
        //void DrawLine(SpriteBatch spriteBatch, float width, Color color, Vector2 point1, Vector2 point2)
        //{
        //    float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
        //    float length = Vector2.Distance(point1, point2);

        //    spriteBatch.Draw(blankTex, point1, null, color,
        //          angle, Vector2.Zero, new Vector2(length, width),
        //          SpriteEffects.None, 0);
        //}

    }
}
