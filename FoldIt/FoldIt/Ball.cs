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
        Texture2D blankTex;

        Vector2 foldLine;
        Vector2 perpendicular;
        float foldingAngle;

        float timePassed;

        // relevent to ball
        Vector2 center;
        float radius;
        float angleBetweenPoints;
        Point ballAfterFolding;
        ////relevent to Left Top
        //Vector2 centerLT;
        //float radiusLT;
        //float angleBetweenLT;
        //Vector2 AfterFoldingLT;
        ////relevent to Left Bottom
        //Vector2 centerLB;
        //float radiusLB;
        //float angleBetweenLB;
        //Vector2 AfterFoldingLB;
        ////relevent to Right Top
        //Vector2 centerRT;
        //float radiusRT;
        //float angleBetweenRT;
        //Vector2 AfterFoldingRT;
        ////relevent to Right Bottom
        //Vector2 centerRB;
        //float radiusRB;
        //float angleBetweenRB;
        //Vector2 AfterFoldingRB;


        

        public  Ball(Texture2D tex,Texture2D blank, int posX, int posY,Rectangle innerBoard)
        {
            ballRec = new Rectangle(posX, posY, ballSize, ballSize);
            ballTex = tex;
            blankTex = blank;
            blankTex.SetData(new[] { Color.White });
            board = innerBoard;

            foldingAngle = 0;
            timePassed = 0;

        }

        public void calcBeforeFolding(EdgePosition first, EdgePosition second) 
        {

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
            //calcLT();
            //calcLB();
            //calcRT();
            //calcRB();
        }

        //private void calcLT()
        //{
        //    perpendicular.Y = board.Y - board.X * perpendicular.X;

        //    centerLT.X = -(foldLine.Y - perpendicular.Y) / (foldLine.X - perpendicular.X);
        //    centerLT.Y = perpendicular.X * centerLT.X + perpendicular.Y;
        //    angleBetweenLT = (float)Math.Atan2(board.Y - centerLT.Y, board.X - centerLT.X);
        //    radiusLT = Vector2.Distance(new Vector2(board.X, board.Y), new Vector2(centerLT.X, centerLT.Y));
        //}

        //private void calcLB()
        //{
        //    perpendicular.Y = board.Bottom - board.X * perpendicular.X;

        //    centerLB.X = -(foldLine.Y - perpendicular.Y) / (foldLine.X - perpendicular.X);
        //    centerLB.Y = perpendicular.X * centerLB.X + perpendicular.Y;
        //    angleBetweenLB = (float)Math.Atan2(board.Bottom - centerLB.Y, board.X - centerLB.X);
        //    radiusLB = Vector2.Distance(new Vector2(board.X, board.Bottom), new Vector2(centerLB.X, centerLB.Y));
        //}

        //private void calcRT()
        //{
        //    perpendicular.Y = board.Y - board.Right * perpendicular.X;

        //    centerRT.X = -(foldLine.Y - perpendicular.Y) / (foldLine.X - perpendicular.X);
        //    centerRT.Y = perpendicular.X * centerRT.X + perpendicular.Y;
        //    angleBetweenRT = (float)Math.Atan2(board.Y - centerRT.Y,board.Right - centerRT.X);
        //    radiusRT = Vector2.Distance(new Vector2(board.X, board.Y), new Vector2(centerRT.X, centerRT.Y));
        //}

        //private void calcRB()
        //{
        //    perpendicular.Y = board.Bottom - board.Right * perpendicular.X;

        //    centerRB.X = -(foldLine.Y - perpendicular.Y) / (foldLine.X - perpendicular.X);
        //    centerRB.Y = perpendicular.X * centerRB.X + perpendicular.Y;
        //    angleBetweenRB = (float)Math.Atan2(board.Bottom - centerRB.Y, board.Right - centerRB.X);
        //    radiusRB = Vector2.Distance(new Vector2(board.Right, board.Bottom), new Vector2(centerRB.X, centerRB.Y));
        //}

        public GameState flipBall(GameTime gameTime)
        {
            if (foldingAngle < MathHelper.Pi)
            {
                timePassed+=(float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timePassed > 0.02f)
                {
                    ballAfterFolding.X = (int)(radius * Math.Cos(angleBetweenPoints + foldingAngle) + center.X);
                    ballAfterFolding.Y = (int)(radius * Math.Sin(angleBetweenPoints + foldingAngle) + center.Y);
                    //// Left Top
                    //AfterFoldingLT.X = (int)(radiusLT * Math.Cos(angleBetweenLT + foldingAngle) + centerLT.X);
                    //AfterFoldingLT.Y = (int)(radiusLT * Math.Sin(angleBetweenLT + foldingAngle) + centerLT.Y);
                    //// Left Bottom
                    //AfterFoldingLB.X = (int)(radiusLB * Math.Cos(angleBetweenLB + foldingAngle) + centerLB.X);
                    //AfterFoldingLB.Y = (int)(radiusLB * Math.Sin(angleBetweenLB + foldingAngle) + centerLB.Y);
                    //// Right Top
                    //AfterFoldingRT.X = (int)(radiusRT * Math.Cos(angleBetweenRT + foldingAngle) + centerRT.X);
                    //AfterFoldingRT.Y = (int)(radiusRT * Math.Sin(angleBetweenRT + foldingAngle) + centerRT.Y);
                    //// Right Buttom
                    //AfterFoldingRB.X = (int)(radiusRB * Math.Cos(angleBetweenRB + foldingAngle) + centerRB.X);
                    //AfterFoldingRB.Y = (int)(radiusRB * Math.Sin(angleBetweenRB + foldingAngle) + centerRB.Y);

                    foldingAngle += 0.05f;
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

        public void DrawFolding(SpriteBatch spriteBatch, EdgePosition first, EdgePosition second)
        {
            spriteBatch.Draw(ballTex, new Rectangle(ballAfterFolding.X, ballAfterFolding.Y, (int)(10 + 10 * Math.Sin(foldingAngle)), (int)(10 + 10 * Math.Sin(foldingAngle))), null, Color.Gold, 0, new Vector2(ballSize / 2, ballSize / 2), SpriteEffects.None, 0);

            #region flipping the page
            //// top -> left / left -> top
            //if (((first.theEdge == Edge.Top) && (second.theEdge == Edge.Left) && ((ballRec.X - ballAfterFolding.X) < 0))
            //    || ((second.theEdge == Edge.Top) && (first.theEdge == Edge.Left) && ((ballRec.X - ballAfterFolding.X) < 0)))
            //{
            //    DrawLine(spriteBatch, 3, Color.Black, AfterFoldingLT, new Vector2(first.x + 4, first.y + 4));
            //    DrawLine(spriteBatch, 3, Color.Black, AfterFoldingLT, new Vector2(second.x + 4, second.y + 4));
            //}
            //else
            //{
            //    // top <- left
            //    if ((first.theEdge == Edge.Top) && (second.theEdge == Edge.Left) && ((ballRec.X - ballAfterFolding.X) > 0))
            //    {
            //        DrawLine(spriteBatch, 3, Color.Black, AfterFoldingRT, new Vector2(first.x + 4, first.y + 4));
            //        DrawLine(spriteBatch, 3, Color.Black, AfterFoldingLB, new Vector2(second.x + 4, second.y + 4));
            //        DrawLine(spriteBatch, 3, Color.Black, AfterFoldingLB, AfterFoldingRB);
            //        DrawLine(spriteBatch, 3, Color.Black, AfterFoldingRT, AfterFoldingRB);
            //    }
            //    else
            //    { // left <- top
            //        if ((second.theEdge == Edge.Top) && (first.theEdge == Edge.Left) && ((ballRec.X - ballAfterFolding.X) > 0))
            //        {
            //            DrawLine(spriteBatch, 3, Color.Black, AfterFoldingLB, new Vector2(first.x + 4, first.y + 4));
            //            DrawLine(spriteBatch, 3, Color.Black, AfterFoldingRT, new Vector2(second.x + 4, second.y + 4));
            //            DrawLine(spriteBatch, 3, Color.Black, AfterFoldingLB, AfterFoldingRB);
            //            DrawLine(spriteBatch, 3, Color.Black, AfterFoldingRT, AfterFoldingRB);
            //        }
            //        else
            //        {   // top <- right / right <- top
            //            if (((first.theEdge == Edge.Top) && (second.theEdge == Edge.Right) && ((ballAfterFolding.X - ballRec.X) < 0))
            //            || ((second.theEdge == Edge.Top) && (first.theEdge == Edge.Right) && ((ballAfterFolding.X - ballRec.X) < 0)))
            //            {
            //                DrawLine(spriteBatch, 3, Color.Black, AfterFoldingRT, new Vector2(first.x + 4, first.y + 4));
            //                DrawLine(spriteBatch, 3, Color.Black, AfterFoldingRT, new Vector2(second.x + 4, second.y + 4));
            //            }
            //            else
            //            {   // top -> right
            //                if ((first.theEdge == Edge.Top) && (second.theEdge == Edge.Right) && ((ballAfterFolding.X - ballRec.X) > 0))
            //                {
            //                    DrawLine(spriteBatch, 3, Color.Black, AfterFoldingLT, new Vector2(first.x + 4, first.y + 4));
            //                    DrawLine(spriteBatch, 3, Color.Black, AfterFoldingRB, new Vector2(second.x + 4, second.y + 4));
            //                    DrawLine(spriteBatch, 3, Color.Black, AfterFoldingRB, AfterFoldingRB);
            //                    DrawLine(spriteBatch, 3, Color.Black, AfterFoldingLT, AfterFoldingRB);
            //                }
            //                else
            //                { // right <- top
            //                    if ((second.theEdge == Edge.Top) && (first.theEdge == Edge.Right) && ((ballAfterFolding.X - ballRec.X) > 0))
            //                    {
            //                        DrawLine(spriteBatch, 3, Color.Black, AfterFoldingRB, new Vector2(first.x + 4, first.y + 4));
            //                        DrawLine(spriteBatch, 3, Color.Black, AfterFoldingLT, new Vector2(second.x + 4, second.y + 4));
            //                        DrawLine(spriteBatch, 3, Color.Black, AfterFoldingRB, AfterFoldingRB);
            //                        DrawLine(spriteBatch, 3, Color.Black, AfterFoldingLT, AfterFoldingRB);

            //                    }
            //                }
            //            }
            //        }
            //    }
            //}



            #endregion                        
        }

        //  enables to draw a line
        void DrawLine(SpriteBatch spriteBatch, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(blankTex, point1, null, color,
                  angle, Vector2.Zero, new Vector2(length, width),
                  SpriteEffects.None, 0);
        }

        public Rectangle getRec()
        {
            return ballRec;
        }


    }
}
