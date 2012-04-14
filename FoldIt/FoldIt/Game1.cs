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

        Board board;
        GameState gamestate;
        Ball ball;
        Goal goal;

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
            //GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width
            
            board = new Board(Content.Load<Texture2D>(@"empty"),Content.Load<Texture2D>(@"edged"),
                new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color),
                graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            ball = new Ball(Content.Load<Texture2D>(@"ball"), new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color), 100, 100, board.getInnerRec());
            goal = new Goal(Content.Load<Texture2D>(@"goal"), 900, 130, 20);

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

            gamestate = board.Update(gamestate,gameTime);
            if (gamestate == GameState.prepreFolding)
            {
                ball.calcBeforeFolding(board.getEdge1(), board.getEdge2());
                gamestate = GameState.folding;
            } 
            if (gamestate == GameState.folding)
                gamestate = ball.flipBall(gameTime);
            if ((gamestate == GameState.ballMoved) && goal.isGoal(ball.getRec()))
                gamestate = GameState.scored;
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
            goal.Draw(spriteBatch);
            if (gamestate == GameState.folding)
                ball.DrawFolding(spriteBatch,board.getEdge1(),board.getEdge2());
            else
                ball.Draw(spriteBatch, gamestate);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
