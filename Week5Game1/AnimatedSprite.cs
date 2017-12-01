using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//monogame classes are in these namepspaces
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Week5Game1
{
    public class AnimatedSprite
    {
        //variables
        public Texture2D Image;
        public Vector2 Position;
        public Rectangle Bounds;
        public Color Tint;



        //used to determine where inside the spritesheet we are drawing
        public Rectangle SourceRectangle;

        //variables for animation
        int currentFrame = 0;
        int numberOfFrames = 0;
        int millisecondsBetweenFrames = 100;
        float elaspedTime = 0;

        // "Alive" variable
        public bool IsAlive = true;

        //constructor
        public AnimatedSprite(Texture2D image, Vector2 position, Color tint, int frameCount)
        {
            numberOfFrames = frameCount;

            Image = image;
            Position = position;
            Tint = tint;

            // width is now total width /  number of frames
            Bounds = new Rectangle((int)position.X, (int)position.Y,
                image.Width / frameCount,
                image.Height);
        }

        public void UpdateAnimation(GameTime gameTime)
        {
            //track how much time has passed
            elaspedTime += gameTime.ElapsedGameTime.Milliseconds;

            //if it's greater than the frame time the move to the next frame
            if (elaspedTime >= millisecondsBetweenFrames)
            {
                currentFrame++;

                if (currentFrame > numberOfFrames - 1)
                {
                    currentFrame = 0;
                }

                elaspedTime = 0;
            }

            //update our source rectangle
            SourceRectangle = new Rectangle(
                currentFrame * (Image.Width / numberOfFrames), //sprite width
                0,
                Image.Width / numberOfFrames,
                Image.Height);
        }

        // caller has a SpriteBatch ready and has already called Begin
        public void Draw(SpriteBatch sp)
        {
            sp.Draw(Image, Position, SourceRectangle, Tint);
        }

        // overload. Same name, different arguments
        public void Draw(SpriteBatch sp, SpriteFont sfont)
        {
            Draw(sp);//call the other Draw method above
            sp.DrawString(sfont, Position.ToString(), Position, Color.White);
        }

        // move the sprite by the given amount
        public void Move(Vector2 delta)
        {
            Position += delta;
            Bounds.X = (int)Position.X;
            Bounds.Y = (int)Position.Y;
        }

        // check for collision
        public bool CheckCollision(AnimatedSprite other)
        {
            if (Bounds.Intersects(other.Bounds))
            {
                
                Tint = Color.Black;
                IsAlive = false;
                return true;
            }
            else
            {
                Tint = Color.White;
                return false;
            }
        }
    }
}
