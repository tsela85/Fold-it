using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FoldIt
{
   public enum Edge {Top,Bottom,Left,Right,None};

    public struct EdgePosition
    {
        public Edge theEdge;
        public int x,y;
    }

    class Board
    {
        public const int DISTANCEfromSCREEN = 50;
        public const int DISTANCEfromOUTER = 30;

        Texture2D outerTex;
        Texture2D innerTex;
        Texture2D blankTex;
        int outX, outY, outH, outW;
        int inX, inY, inX1, inY1;
        EdgePosition edge1, edge2;
        MouseState ms;

        public Board(Texture2D outT,Texture2D inT,Texture2D blank,int screenW,int screenH)
        {
            outX = outY = DISTANCEfromSCREEN;
            outH = screenH - 2*DISTANCEfromSCREEN;
            outW = screenW - 2*DISTANCEfromSCREEN;

            inX = inY = DISTANCEfromSCREEN+DISTANCEfromOUTER;
            inY1 = screenH - DISTANCEfromSCREEN - DISTANCEfromOUTER;
            inX1 = screenW - DISTANCEfromSCREEN - DISTANCEfromOUTER;

            outerTex = outT;
            innerTex = inT;
            blankTex = blank;
            blankTex.SetData(new[] { Color.White });

            edge1 = new EdgePosition();
            edge2 = new EdgePosition();

            
        }

        public GameState Update(GameState gamestate, GameTime gameTime) 
        {
            Edge currentEdge = onEdge();
            ms = Mouse.GetState();


            #region choosing first edge
            if ((gamestate == GameState.chooseEdge1) || ((gamestate == GameState.onEdge1)))
            {
                if (currentEdge != Edge.None)
                {

                    edge1.theEdge = currentEdge;
                    switch (currentEdge)
                    {
                        case (Edge.Top): edge1.x = ms.X; edge1.y = inY - 3; break;
                        case (Edge.Bottom): edge1.x = ms.X; edge1.y = inY1 - 6; break;
                        case (Edge.Left): edge1.x = inX - 3; edge1.y = ms.Y; break;
                        case (Edge.Right): edge1.x = inX1 - 6; edge1.y = ms.Y; break;
                        default: edge1.x = ms.X; edge1.y = ms.Y; break;
                    }
                    return ((ms.LeftButton != ButtonState.Pressed) ? GameState.onEdge1 : GameState.chooseEdge2);
                }
                return GameState.chooseEdge1;
            } 
            #endregion

            #region choosing second edge
            if ((gamestate == GameState.chooseEdge2) || ((gamestate == GameState.onEdge2)))
            {
                if (ms.RightButton == ButtonState.Pressed)
                    return GameState.chooseEdge1; // cancel firs selection
                if ((currentEdge != Edge.None) && (currentEdge != edge1.theEdge))
                {

                    edge2.theEdge = currentEdge;
                    switch (currentEdge)
                    {
                        case (Edge.Top): edge2.x = ms.X; edge2.y = inY - 3; break;
                        case (Edge.Bottom): edge2.x = ms.X; edge2.y = inY1 - 6; break;
                        case (Edge.Left): edge2.x = inX - 3; edge2.y = ms.Y; break;
                        case (Edge.Right): edge2.x = inX1 - 6; edge2.y = ms.Y; break;
                        default: edge1.x = ms.X; edge1.y = ms.Y; break;
                    }
                    return ((ms.LeftButton != ButtonState.Pressed) ? GameState.onEdge2 : GameState.prepreFolding);
                }
                return GameState.chooseEdge2;
            }
            
            #endregion

            //if ((gamestate == GameState.folding) && (ms.RightButton == ButtonState.Pressed))
            //    return GameState.chooseEdge1;
            if ((gamestate == GameState.ballMoved) && (ms.LeftButton == ButtonState.Released))
                return GameState.chooseEdge1;

            return gamestate;
        }

        public void Draw(SpriteBatch spriteBatch,GameState gamestate)
        {
            spriteBatch.Draw(outerTex, new Rectangle(outX, outY, outW, outH), Color.Teal);
            spriteBatch.Draw(innerTex, new Rectangle(inX, inY, inX1 - inX, inY1 - inY), Color.Silver);
            if (gamestate == GameState.onEdge1)
                spriteBatch.Draw(outerTex, new Rectangle(edge1.x, edge1.y, 9, 9), Color.Yellow);
            if (gamestate == GameState.chooseEdge2)
            {
                spriteBatch.Draw(outerTex, new Rectangle(edge1.x, edge1.y, 9, 9), Color.Orange);
                DrawLine(spriteBatch,1,Color.Red,new Vector2(edge1.x+4,edge1.y+4), new Vector2(ms.X,ms.Y));     
            }
            if (gamestate == GameState.onEdge2)
            {
                spriteBatch.Draw(outerTex, new Rectangle(edge1.x, edge1.y, 9, 9), Color.Orange);
                spriteBatch.Draw(outerTex, new Rectangle(edge2.x, edge2.y, 9, 9), Color.Yellow);
                DrawLine(spriteBatch,2,Color.Blue,new Vector2(edge1.x+4,edge1.y+4), new Vector2(edge2.x+4,edge2.y+4));
            }
            if (gamestate == GameState.prepreFolding)
            {
                spriteBatch.Draw(outerTex, new Rectangle(edge1.x, edge1.y, 9, 9), Color.Orange);
                spriteBatch.Draw(outerTex, new Rectangle(edge2.x, edge2.y, 9, 9), Color.Orange);
                DrawLine(spriteBatch, 2, Color.DarkBlue, new Vector2(edge1.x + 4, edge1.y + 4), new Vector2(edge2.x + 4, edge2.y + 4));
            }

        }

        // checks if the mouse cursor is on one of the edges
        private Edge onEdge()
        {
            if ((ms.X >= inX) && (ms.X <= inX1) && (ms.Y <= inY +3) && (ms.Y >= inY -3))
                return Edge.Top;
            if ((ms.X >= inX) && (ms.X <= inX1) && (ms.Y <= inY1 +3) && (ms.Y >= inY1 -3))
                return Edge.Bottom;
            if ((ms.Y >= inY ) && (ms.Y <= inY1) && (ms.X <= inX +3) && (ms.X >= inX -3))
                return Edge.Left;
            if ((ms.Y >= inY) && (ms.Y <= inY1) && (ms.X <= inX1 +3) && (ms.X >= inX1 -3))
                return Edge.Right;
            return Edge.None;
        }
        
        // enables to draw a line
        void DrawLine(SpriteBatch batch, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blankTex, point1, null, color,
                  angle, Vector2.Zero, new Vector2(length, width),
                  SpriteEffects.None, 0);
        }

        public Vector2 getEdge1Position()
        {
            return new Vector2(edge1.x, edge1.y);
        }

        public Vector2 getEdge2Position()
        {
            return new Vector2(edge2.x, edge2.y);
        }

        public Rectangle getInnerRec()
        {
            return new Rectangle(inX, inY, inX1 - inX, inY1 - inY);
        }
    }
}
