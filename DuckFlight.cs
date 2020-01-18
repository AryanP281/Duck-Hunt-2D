using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Duck_Hunt
{
        //The class for the object duck
        class DuckFlight
        {
            bool flyingLeft;//Determines whether the animation is for left flight or for right flight
            public bool hit = false; //Checks whether the duck has been hit

            Texture2D duck;

            GameTime gameTime;

            int animFrame; //The animation Frame
            public float duckSpeed; //The speed at which the duck flies

            float animElapsedTime; //The elapsed Time
            public float delay = 1; //The delay for each frame in milliseconds

            Rectangle duckSourceRect;
            public Rectangle duckDestRect;

            public DuckFlight(bool isFlyingLeft, Texture2D duckTex)
            {
                flyingLeft = isFlyingLeft;
                duck = duckTex;

                //Initializing the destination rectangle of the class
                duckDestRect = new Rectangle(0, 200, duck.Width - 100, duck.Height - 100);

                if (flyingLeft)
                {
                    animFrame = 0;
                }
                else
                {
                    animFrame = 1;
                }

                //Initializing the speed of the duck
                duckSpeed = 5;
            }

            public GameTime GameTime
            {
                set { this.gameTime = value; }
            }

            public void Update()
            {
                duckDestRect.X -= (int)duckSpeed;
            }

            public void AnimateDuck()
            {

                animElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (animElapsedTime >= delay)
                {
                    if (animFrame == 0)
                    {
                        animFrame = 1;
                    }
                    else if(animFrame == 1)
                    {
                        animFrame = 0;
                    }

                    animElapsedTime = 0;
                }


                duckSourceRect = new Rectangle(animFrame * 200, 0, 200, 150);

            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(duck, duckDestRect, duckSourceRect, Color.White);

                AnimateDuck();
            }
        }
   }


