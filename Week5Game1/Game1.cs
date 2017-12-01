using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using Microsoft.Xna.Framework.Audio;

namespace Week5Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 

        // Jack Gilmartin//Week 5 Game//23 October 2017


    /*
    1.	Add a background to the existing screen. You could do that as a Simple Sprite or just as a texture. 
    2.	Add a new Animated Sprite object called Collectable. It is stationary. 
    3.	When your player Animated Sprite (which moves using ASWD) collides with the Collectable
    a.	Play a sound
    b.	Make the Collectable sprite disappear. 
        I.	Give the Animated Sprite a public bool property called alive.
        II.	Only check for collision if the Collectable is alive.
        III. Only update an Animated Sprite if it is alive
        IV.	Only Draw the an Animated Sprite if it is alive
    4.	Make an array of 5 Collectable Animated Sprites and position them at various positions in the Viewport.
    5.	Collect all the collectables.
    6.	Make a score variable in the game class. Display the score at vector position 20,20
    7.	You get 100 points for every Collectable collected.
    8.	When all the Collectables are collected Display a Game over screen.

    */


    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        AnimatedSprite sonicSprite;
        Texture2D background;
        Texture2D gameOverBackground;

        AnimatedSprite collectable;

        AnimatedSprite[] collectables = new AnimatedSprite[5];

        // Bool for whether or not the game is over.
        bool gameOver = false;

        // Font and Score variable
        SpriteFont sFont;
        int Score = 0;



        // Music and Sound effects
        Song gameOverMusic;
        private SoundEffect effect;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();

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
            // TODO: Add your initialization logic here

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

            // Load Animated player sprite.
            sonicSprite = new AnimatedSprite(
                Content.Load<Texture2D>("runright"), // Image
                new Vector2(300, 300), // Position
                Color.White, // Colour
                6); // Frames

            // Create background.
            background = Content.Load<Texture2D>("background");

            // Create game over background.
            gameOverBackground = Content.Load<Texture2D>("gameOverBackground");

            // Load image for the collectables.
            Texture2D collectableImage = Content.Load<Texture2D>("coin");

            // Create font.
            sFont = Content.Load<SpriteFont>("sfont");

            // Load music and sound effects
            this.gameOverMusic = Content.Load<Song>("robotnik");

            effect = Content.Load<SoundEffect>("collect");

            // Set volume.
            MediaPlayer.Volume = 0.0f;


            // the following for loop allows a number of coins to randomly appear.
            for (int i = 0; i < collectables.Length; i++)
            {

                int xPosition = RandomInt(
                   100,
                   GraphicsDevice.Viewport.Bounds.Width - 100);

                int yPosition = RandomInt(
                   100,
                   GraphicsDevice.Viewport.Bounds.Height - 100);

                collectables[i] = new AnimatedSprite(
                    collectableImage,
                    new Vector2(xPosition, yPosition),
                    Color.White,
                    6);

            }
            // TODO: use this.Content to load your game content here
        }


        Random random = new Random();
        public int RandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            // Update the player sprite if the game isn't over yet.
            if (gameOver == false)
            {
                sonicSprite.UpdateAnimation(gameTime);
            }


            foreach (AnimatedSprite Coin in collectables)
            {
                if (Coin.IsAlive)
                {
                    Coin.UpdateAnimation(gameTime);
                    Coin.CheckCollision(sonicSprite);

                    // If the player collects a coin....
                    if (Coin.CheckCollision(sonicSprite) == true)
                    {
                        effect.Play();
                        Score += 100;
                    }
                }
            }

            // Play music when the game is over and all coins are collected.
            if (Score == 500 && MediaPlayer.Volume != 0.5f)
            {
                gameOver = true;
                MediaPlayer.Volume += 0.5f;
                MediaPlayer.Play(gameOverMusic);
            }

            #region Player Input
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
        
            // The following are for controlling the player.
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                // Left
                sonicSprite.Move(new Vector2(-5, 0));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                // Right
                sonicSprite.Move(new Vector2(5, 0));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                // Up
                sonicSprite.Move(new Vector2(0, -5));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                // Down
                sonicSprite.Move(new Vector2(0, 5));
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

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            // The following will display one of two backgrounds depending on whether or not the game is over.
            if (gameOver == false)
            {
                spriteBatch.Draw(background, GraphicsDevice.Viewport.Bounds, Color.White);

            }

            else
            {
                spriteBatch.Draw(gameOverBackground, GraphicsDevice.Viewport.Bounds, Color.White);
            }

            // Draw Score.
            spriteBatch.DrawString(sFont, "Score: " + Score, new Vector2(20, 20), Color.White);

            // The player sprite is only drawn if the game isn't over yet.
            if (gameOver == false)
            {
                sonicSprite.Draw(spriteBatch);
            }

            // Draw each coin until they are collected.
            foreach (AnimatedSprite Coin in collectables)
            {
                if (Coin.IsAlive == true)
                {
                    Coin.Draw(spriteBatch);
                }
            }

            // Draw the "Game Over" message.
            if (gameOver == true)
            {
                spriteBatch.DrawString(sFont, "Game Over", new Vector2(610, 330), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
