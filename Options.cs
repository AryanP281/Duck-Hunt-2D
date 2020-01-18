using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Duck_Hunt
{
    class Options
    {
        SpriteFont UI; //The UI text for displaying the options
        SpriteFont debug;
        SpriteFont buttonsUI; //Text for the GUI buttons

        string[] keys = { "Mute"};
        bool[] values = { false};

        public CustomDictionary<string, bool> options = new CustomDictionary<string, bool>();

        Texture2D[] tickboxes = new Texture2D[1];
        Texture2D tick;
        Texture2D cross;
        Texture2D eggUI; //The egg texture as UI buttons

        Rectangle[] tickboxesDestRects = new Rectangle[1]; //An array for storing the destination rectangles of the tickboxes
        Rectangle[] buttonRects = new Rectangle[2];

        Color[] GUIColors = new Color[2]; //Color tints of the GUI buttons

        int[] frames = new int[2]; //An array storing the frames of the tickboxes

        bool startMouseTimer = true;
        public bool justReturned = false; //Checks whether the player has returned to the main menu
        bool isOptions = true; //Detremines whethter the player wants to see options page
        bool isControlsPage; //Determines whether the player wants to see Controls
        bool startEscapeTimer;

        float mousePressElapsedTime = 0.0f;
        float escapePressedElapsedTime = 0.0f;

        string debugText = "";

       public Options()
       {
           options.Add(keys, values);

           //Initializing the GUI color tints
           for(int a = 0;a < 2;a++)
           {
               GUIColors[a] = Color.White;
           }
       }

        public void LoadContent(ContentManager Content)
       {
           UI = Content.Load<SpriteFont>("Options Content\\Options GUI");
           debug = Content.Load<SpriteFont>("Debug Font");
           tick = Content.Load<Texture2D>("Options Content\\Tickbox ticked");
           cross = Content.Load<Texture2D>("Options Content\\Tickbox cross");
           eggUI = Content.Load<Texture2D>("Options Content\\Blank Egg");
           buttonsUI = Content.Load<SpriteFont>("Options Content\\Buttons UI");

            //Initializing the destination rectangles of the tickboxes
            tickboxesDestRects[0] = new Rectangle(650, 310, 100, 100);

            //Initializing the rectangles for the UI buttons
            buttonRects[0] = new Rectangle(0, 570, eggUI.Width - 50, eggUI.Height - 50);
            buttonRects[1] = new Rectangle(1130, 570, eggUI.Width - 50, eggUI.Height - 50);

            //Loading the default tickboxes
            if (frames[0] == 0)
            {
                tickboxes[0] = Content.Load<Texture2D>("Options Content\\Tickbox cross");
            }
            else
            {
                tickboxes[0] = Content.Load<Texture2D>("Options Content\\Tickbox ticked");
            }
            
       }
       
        public void Update (GameTime gameTime)
        {
            if (isOptions)
            {
                //Switching just returned to false
                justReturned = false;

                //Returning to Main Menu is 'Escape' is pressed
                if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !startEscapeTimer)
                {
                    Game1.isOptions = false;
                    Game1.isMainiMenu = true;
                    justReturned = true;

                    startEscapeTimer = true;
                    escapePressedElapsedTime = 0;

                }

                //Checking if the options have been changed
                for (int a = 0; a < tickboxesDestRects.Length; a++)
                {
                    if (tickboxesDestRects[a].Contains(Mouse.GetState().Position) && Mouse.GetState().LeftButton == ButtonState.Pressed && !startMouseTimer)
                    {
                        if (frames[a] == 0)
                        {
                            frames[a] = 1;

                            //Muting sound if interacted with the button
                            if (options.KeyAt(a) == "Mute")
                            {
                                options.ChangeValueTo(a, true);

                                //Setting tickbox to tick
                                tickboxes[a] = tick;
                            }

                        }
                        else
                        {
                            frames[a] = 0;

                            //Playing sound if interacted with the button
                            if (options.KeyAt(a) == "Mute")
                            {
                                options.ChangeValueTo(a, false);

                                //Setting tickbox to cross
                                tickboxes[a] = cross;
                            }
                        }

                        mousePressElapsedTime = 0;
                        startMouseTimer = true;
                    }

                }

                //Checking if the player has interacted with any GUI buttons
                for (int a = 0; a < 2; a++)
                {
                    if (buttonRects[a].Contains(Mouse.GetState().Position))
                    {
                        GUIColors[a] = Color.Red;

                        //Checking if the player wants to go to the credits screen
                        if (a == 1 && Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            Game1.isOptions = false;
                            Game1.isCredits = true;
                            Game1.startEscapeTimer = true;

                            MediaPlayer.Stop();
                        }

                        //Checking if the player wants to see the controls
                        if(a == 0 && Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            isOptions = false;
                            isControlsPage = true;

                            MediaPlayer.Stop();
                        }
                    }
                    else
                    {
                        GUIColors[a] = Color.White;
                    }

                }
            }

            else if(isControlsPage)
            {
                if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    isOptions = true;
                    isControlsPage = false;
                    startEscapeTimer = true;
                    escapePressedElapsedTime = 0;
                }
            }

            if(startEscapeTimer)
            {
                escapePressedElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if(escapePressedElapsedTime >= 1)
            {
                startEscapeTimer = false;
            }

            //Starting the mouse press timer
            if(startMouseTimer)
            {
                mousePressElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            if(mousePressElapsedTime >= 500)
            {
                startMouseTimer = false;
            }

                //Checking if the game window is active. If it is inactive than stooping the music
                if (!Game1.isActive)
                {
                    MediaPlayer.Pause();
                }
            
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Begin();

            if (isOptions)
            {
                spritebatch.DrawString(UI, "Mute", new Vector2(500, 325), Color.Gold);

                //Drawing the tickboxes
                 spritebatch.Draw(tickboxes[0], tickboxesDestRects[0], Color.White);
                

                //Drawing the GUI buttons
                spritebatch.Draw(eggUI, buttonRects[0], GUIColors[0]);
                spritebatch.Draw(eggUI, buttonRects[1], GUIColors[1]);

                //UI for the GUI
                spritebatch.DrawString(buttonsUI, "Controls", new Vector2(35, 630), new Color(239, 212, 136));
                spritebatch.DrawString(buttonsUI, "Credits", new Vector2(1170, 630), new Color(239, 212, 136));

                spritebatch.DrawString(debug, debugText, new Vector2(100, 100), Color.Goldenrod);
            }

            else if(isControlsPage)
            {
                spritebatch.DrawString(UI, "SHOOT:", new Vector2(400, 100), Color.Gold);
                spritebatch.DrawString(UI, "LMB", new Vector2(800, 100), Color.Gold);

                spritebatch.DrawString(UI, "RELOAD:", new Vector2(400, 300), Color.Gold);
                spritebatch.DrawString(UI, "R Key", new Vector2(800, 300), Color.Gold);

                spritebatch.DrawString(UI,"BACK:",new Vector2(400,500),Color.Gold);
                spritebatch.DrawString(UI, "Escape Key", new Vector2(800, 500), Color.Gold);
            }

           spritebatch.End();
        }
    }

    class CustomDictionary<K,V>
    {
        List<K> keys = new List<K>();
        List<V> values = new List<V>();

        public CustomDictionary()
        {

        }

        public CustomDictionary(K[] keys2, V[] values2)
        {
            foreach(K key in keys2)
            {
                keys.Add(key);
            }

            foreach(V value in values2)
            {
                values.Add(value);
            }
        }

        public void Add(K[] keys3, V[] values3)
        {
            foreach(K key in keys3)
            {
                keys.Add(key);
            }

            foreach(V value in values3)
            {
                values.Add(value);
            }
        }

        public int Count
        {
            get { return keys.Count; }
        }

        public K KeyAt(int index)
        {
            try
            {
                return keys[index];
            }
            catch (ArgumentNullException)
            {
            }
            catch (ArgumentOutOfRangeException)
            {
            }

            return keys[index];
        }

        public V ValueAt(int index)
        {
            try
            {
                return values[index];
            }
            catch (ArgumentException)
            {
            }
            return values[index];
        }

        public void ChangeValueTo(int index, V newValue)
        {
            values[index] = newValue;
        }

        public V ValueOf(K key)
        {
            int index = 0;

            for(int a = 0;a < keys.Count;a++)
            {
                if(object.Equals(keys[a], key))
                {
                    index = a;
                }
            }

            if(!object.Equals(keys[index], key))
            {
                return default(V);
            }

            return values[index];
        }
    }
}

