using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace FoldIt
{
    enum GameState {chooseEdge1,onEdge1, chooseEdge2,onEdge2,prepreFolding, folding , ballMoved, scored};
   
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont font,scoreFont;
        Board board;
        GameState gamestate;
        Ball ball,ball2;
        Goal goal,goal2;

        int level;
        int folds;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            #region screenInit
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 600;
            //graphics.IsFullScreen = false;
            graphics.ApplyChanges();
           // Window.AllowUserResizing = true; 
            Window.Title = "Fold It";
            #endregion

            gamestate = GameState.chooseEdge1;
            this.IsMouseVisible = true;
            folds = 0;
            level = 1;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>(@"font");
            scoreFont = Content.Load<SpriteFont>(@"scoreFont");
            
            board = new Board(Content.Load<Texture2D>(@"empty"),Content.Load<Texture2D>(@"edged"),
                new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color),
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            ball = new Ball(Content.Load<Texture2D>(@"ball"), new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color), 100, 100, board.getInnerRec());
            goal = new Goal(Content.Load<Texture2D>(@"goal"), 1000, 180, 20);

            ball2 = new Ball(Content.Load<Texture2D>(@"ball"), new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color), 100, 100, board.getInnerRec());
            goal2 = new Goal(Content.Load<Texture2D>(@"goal"), 1000, 180, 20);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            #region Level 1
            if (level == 1)
            {
                  gamestate = GameState.scored;
                if (Keyboard.GetState().IsKeyDown(Keys.R))
                {
                    folds = 0;
                    gamestate = GameState.chooseEdge1;
                    ball.initializeBall(100, 100);
                    goal.initializeGoal(1000, 180);
                }
                if ((gamestate == GameState.scored) && (Mouse.GetState().LeftButton == ButtonState.Pressed))
                {
                    folds = 0;
                    ball.initializeBall(300, 100);
                    ball2.initializeBall(400, 100);

                    goal.initializeGoal(1000, 350);
                    goal2.initializeGoal(1000, 450);
                    level = 2;
                    gamestate = GameState.chooseEdge1;
                }
                gamestate = board.Update(gamestate, gameTime);
                if (gamestate == GameState.prepreFolding)
                {
                    ball.calcBeforeFolding(board.getEdge1(), board.getEdge2());
                    gamestate = GameState.folding;
                    folds++;
                }
                if (gamestate == GameState.folding)
                    gamestate = ball.flipBall(gameTime);
                if ((gamestate == GameState.ballMoved) && goal.isGoal(ball.getRec()))
                    gamestate = GameState.scored;
            } 
            #endregion
            else
                #region Level 2
                if (level == 2)
                {
                    gamestate = GameState.scored;
                    if (Keyboard.GetState().IsKeyDown(Keys.R))
                    {
                        folds = 0;
                        gamestate = GameState.chooseEdge1;
                        ball.initializeBall(350, 200);
                        ball2.initializeBall(900, 200);

                        goal.initializeGoal(400, 400);
                        goal2.initializeGoal(950, 400);
                    }
                    if ((gamestate == GameState.scored) && (Mouse.GetState().LeftButton == ButtonState.Pressed))
                    {
                        folds = 0;
                        ball.initializeBall(200, 200);
                        ball2.initializeBall(1000, 200);

                        goal.initializeGoal(300, 400);
                        goal2.initializeGoal(900, 400);
                        level = 3;
                        gamestate = GameState.chooseEdge1;
                    }
                    gamestate = board.Update(gamestate, gameTime);
                    if (gamestate == GameState.prepreFolding)
                    {
                        ball.calcBeforeFolding(board.getEdge1(), board.getEdge2());
                        ball2.calcBeforeFolding(board.getEdge1(), board.getEdge2());
                        gamestate = GameState.folding;
                        folds++;
                    }
                    if (gamestate == GameState.folding)
                    {
                        ball2.flipBall(gameTime);
                        gamestate = ball.flipBall(gameTime);
                    }
                    if ((gamestate == GameState.ballMoved) && (goal.isGoal(ball.getRec()) || goal2.isGoal(ball.getRec()))
                            && (goal.isGoal(ball2.getRec()) || goal2.isGoal(ball2.getRec())))
                        gamestate = GameState.scored;
                }
                #endregion
                else
                    #region Level 3
                    if (level == 3)
                    {

                        if (Keyboard.GetState().IsKeyDown(Keys.R))
                        {
                            folds = 0;
                            gamestate = GameState.chooseEdge1;
                            ball.initializeBall(350, 200);
                            ball2.initializeBall(900, 200);

                            goal.initializeGoal(400, 400);
                            goal2.initializeGoal(950, 400);
                        }
                        if ((gamestate == GameState.scored) && (Mouse.GetState().LeftButton == ButtonState.Pressed))
                        {
                            folds = 0;
                            ball.initializeBall(100, 100);
                            goal.initializeGoal(1000, 180);
                            level = 1;
                            gamestate = GameState.chooseEdge1;
                        }
                        gamestate = board.Update(gamestate, gameTime);
                        if (gamestate == GameState.prepreFolding)
                        {
                            ball.calcBeforeFolding(board.getEdge1(), board.getEdge2());
                            ball2.calcBeforeFolding(board.getEdge1(), board.getEdge2());
                            gamestate = GameState.folding;
                            folds++;
                        }
                        if (gamestate == GameState.folding)
                        {
                            ball2.flipBall(gameTime);
                            gamestate = ball.flipBall(gameTime);
                        }
                        if ((gamestate == GameState.ballMoved) && (goal.isGoal(ball.getRec()) || goal2.isGoal(ball.getRec()))
                                && (goal.isGoal(ball2.getRec()) || goal2.isGoal(ball2.getRec())))
                            gamestate = GameState.scored;
                    } 
                    #endregion
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            board.Draw(spriteBatch,gamestate);
            if (level == 1)
            {
                goal.Draw(spriteBatch);
                if (gamestate == GameState.folding)
                    ball.DrawFolding(spriteBatch, board.getEdge1(), board.getEdge2());
                else
                    ball.Draw(spriteBatch, gamestate);
            } // level 2 or 3
            else
            {
                goal.Draw(spriteBatch);
                goal2.Draw(spriteBatch);
                if (gamestate == GameState.folding)
                {
                    ball.DrawFolding(spriteBatch, board.getEdge1(), board.getEdge2());
                    ball2.DrawFolding(spriteBatch, board.getEdge1(), board.getEdge2());
                }
                else
                {
                    ball.Draw(spriteBatch, gamestate);
                    ball2.Draw(spriteBatch, gamestate);

                }
            }
            spriteBatch.DrawString(font, "Fold the page, till the ink-stain is in the hole", new Vector2(50, 15), Color.Black);
            spriteBatch.DrawString(font, "Mouse Left Button - choose, Mouse Right Button - cancel", new Vector2(50, graphics.PreferredBackBufferHeight - 50), Color.Black);
            spriteBatch.DrawString(font, "folds: " + folds, new Vector2(graphics.PreferredBackBufferWidth - 150, 15), Color.Black);
            spriteBatch.DrawString(font, "level: " + level, new Vector2(graphics.PreferredBackBufferWidth - 150, graphics.PreferredBackBufferHeight - 50), Color.Black);
            spriteBatch.DrawString(font,"press R to restart level", new Vector2(50,150), Color.Black
                    ,(MathHelper.Pi/2)+ 0.02f, new Vector2(0,0), 1, SpriteEffects.None, 0);
            spriteBatch.DrawString(font,"Click on the page edges to fold it" , new Vector2(1185, 100), Color.Black
                    , (MathHelper.Pi / 2), new Vector2(0, 0), 1, SpriteEffects.None, 0);
            if (gamestate == GameState.scored)
            {
                string output = "    WINNER!! \n only " + folds + " folds";
                Vector2 FontOrigin = scoreFont.MeasureString(output) / 2;
                spriteBatch.DrawString(scoreFont, output, new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2), Color.Black
                    , 0, FontOrigin, 1.0f, SpriteEffects.None, 0);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
