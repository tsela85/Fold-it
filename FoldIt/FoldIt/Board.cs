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
        int outX, outY, outH, outW;
        int inX, inY, inX1, inY1;
        EdgePosition edge1, edge2;
        MouseState ms;

        public Board(Texture2D outT,Texture2D inT,int screenW,int screenH)
        {
            outX = outY = DISTANCEfromSCREEN;
            outH = screenH - 2*DISTANCEfromSCREEN;
            outW = screenW - 2*DISTANCEfromSCREEN;

            inX = inY = DISTANCEfromSCREEN+DISTANCEfromOUTER;
            inY1 = screenH - DISTANCEfromSCREEN - DISTANCEfromOUTER;
            inX1 = screenW - DISTANCEfromSCREEN - DISTANCEfromOUTER;

            outerTex = outT;
            innerTex = inT;

            edge1 = new EdgePosition();
            edge2 = new EdgePosition();

            
        }

        public GameState Update(GameState gamestate, GameTime gameTime) 
        {
            Edge currentEdge = onEdge();
            ms = Mouse.GetState();

            if (gamestate == GameState.chooseEdge1)
            {
                if (currentEdge != Edge.None)
                {
                    edge1.theEdge = currentEdge;
                    switch (currentEdge)
                    {
                        case (Edge.Top): edge1.x = ms.X; edge1.y = inY-3; break;
                        case (Edge.Bottom): edge1.x = ms.X; edge1.y = inY1-6; break;
                        case (Edge.Left): edge1.x = inX-3; edge1.y = ms.Y; break;
                        case (Edge.Right): edge1.x = inX1-6; edge1.y = ms.Y; break;
                        default: edge1.x = ms.X; edge1.y = ms.Y; break;
                    }
                    return GameState.onEdge1; //((ms.LeftButton == ButtonState.Pressed) ? GameState.onEdge1 : GameState.chooseEdge1);
                }
            } 
            return GameState.chooseEdge1;
        }

        public void Draw(SpriteBatch spriteBatch,GameState gamestate)
        {
            spriteBatch.Draw(outerTex, new Rectangle(outX, outY, outW, outH), Color.RosyBrown);
            spriteBatch.Draw(innerTex, new Rectangle(inX, inY, inX1 - inX, inY1 - inY), Color.Salmon);
            if (gamestate == GameState.onEdge1)
                spriteBatch.Draw(outerTex, new Rectangle(edge1.x, edge1.y, 8, 8), Color.Yellow);

        }

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
    }
}
