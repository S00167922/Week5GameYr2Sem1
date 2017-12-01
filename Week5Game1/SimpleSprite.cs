using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Monogame classes are in these naemspaces.

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Week5Game1
{
    class SimpleSprite
    {
        // Variables
        public Texture2D Image;
        public Vector2 Position;
        public Rectangle Bounds;
        public Color Tint;

        // Used to determine where inside the spritesheet we are drawing
        public Rectangle SourceRectangle;

        // Variables for animation
        int currentFrame = 0;
        int numberOfFrames = 0;
        int millisecondsBetweenFrames = 100;
        float elapsedTime = 0;

        // Properties.... later

        // Constructor
        public SimpleSprite(Texture2D image, Vector2 position, Color tint, int frameCount)
        {
            numberOfFrames = frameCount;

            Image = image;
            Position = position;
            Tint = tint;

            // Width is now width/number of frames
            Bounds = new Rectangle((int)position.X, (int)position.Y, image.Width / frameCount, image.Height);

        }

        public void UpdateAnimation(GameTime gameTime)
        {
            // Track how much time has passed
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            // If it's greater than the frame time then move to the next frame
            if (elapsedTime >= millisecondsBetweenFrames)
            {
                currentFrame++;

                if (currentFrame > numberOfFrames - 1)
                {
                    currentFrame = 0;
                }

                elapsedTime = 0;
            }

            // Update our source rectangle
            SourceRectangle = new Rectangle(
                currentFrame * Image.Width / numberOfFrames, // Sprite width
                0,
                Image.Width / numberOfFrames,
                Image.Height);

        }

        // Caller has a spritebatch ready and has already called Begin
        public void Draw(SpriteBatch sp)
        {
            sp.Draw(Image, Position, SourceRectangle, Tint);
            // sp.Draw(Image, Position, Tint);
        }

        public void Draw(SpriteBatch sp, SpriteFont sfont)
        {
            sp.Draw(Image, Position, Tint);
            sp.DrawString(sfont, Position.ToString(), Position, Color.White);
        }

        public void Move(Vector2 delta)
        {
            Position += delta;
            Bounds.X = (int)Position.X;
            Bounds.Y = (int)Position.Y;
        }

        // Check for collision
        public void CheckCollision(SimpleSprite other)
        {
            // Rectangle intersects

            // If there's a collision change the tint to red
            // Otherwise set tint to white
            if ((Bounds.Intersects(other.Bounds)))
            {
                Tint = Color.Red;
            }

            else
            {
                Tint = Color.White;
            }

        }


    }
}
