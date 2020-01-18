using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace Duck_Hunt
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D mainMenuBackground; //Background for the main menu
        Texture2D startGameUI;
        Texture2D optionsUI;
        Texture2D quitUI;
        Texture2D gameBackground; //Background for the main game
        Texture2D duckFlyingLeft; //Spritesheet for the flying duck[Left]
        Texture2D crosshair;
        Texture2D explosion; //The explosion when the duck is hit
        Texture2D[] shotguns = new Texture2D[2]; //The shotguns in the game
        Texture2D doubleShotgun; //The default double barrel shotgun
        Texture2D[] doubleBarrelShells = new Texture2D[2]; //The ammo indicator for double barrel shotgun
        Texture2D duckRoast;
        
        SpriteFont gameTitle; //Font for the game's title
        SpriteFont debug;//Font for debugging
        SpriteFont inGameUI; //Spritefont for in game purposes
        SpriteFont lostScreenUi; //The font to display the reason of losing
        SpriteFont scoreUI; //The font to display the score, highscore, etc

        Rectangle[] uiRectangles = new Rectangle[3];
        Rectangle crosshairRect; //Rectangle for the crosshair
        Rectangle[] shotgunRectangles = new Rectangle[2]; //Rectangles for the shotguns

        Color[] uiColors = new Color[3]; //The color of the UI Elements
        Color[] shellColor = new Color[2]; //Color to help in differentiating between spent and unspent shells

        string debugText;
        string reasonOfLosing;

        static public bool isMainiMenu = true;
        bool isGame;
        static public bool isOptions;
        static public bool isCredits;
        bool loaded = true; //Checks if the player's gun is loaded
        bool startMouseTimer; //Tells whether to start the timer
        bool lost; //Checks if the player has lost the game
        static public bool startEscapeTimer; //Tells whether to start the escape key timer
        bool soundIsOn = true; //Checks whether the player wants the game sound on
        static bool isA; //Checks whether the game window is active

        Song mainMenuMusic; //Background song for the main menu

        float duckDelay = 2; //Delay for instantiating a new duck
        float duckElapsedTime; //Time elapsed since the last duck
        float mousePressElapsed = 1; //Time elapsed since the last elapsed time
        float escElapsedTime; //Time elapsed since the last escape key press

        int duckValue = 0; //The duck to release
        int doubleBarrelAmmoCounter = 2; //The ammo counter for the double barrel shotgun
        int playerAmmo; //The ammo that the player has
        int doubleBarrelMaxCapacity = 2; //The max ammo that the double barrel can hold
        int score = 0; //The score of the player
        int playerChances; //The chances that the player have
        int animFrames; //Animation Frames

        List<DuckFlight> ducksToDisplay = new List<DuckFlight>(); //A list of the ducks to display on the screen

        Custom_Random<int> rand;
        Options optionsMenu = new Options(); //A instance of the options class
        Credits credits = new Credits(); //A instance of the credits class for displaying the credits of the game

        SoundEffect doubleBarrelBlast;  //Sound to play when the double barrel shotgun is fired
        SoundEffectInstance doubleBarrelBlastInst;
        SoundEffect duckQuack;
        SoundEffectInstance duckQuackInst;

        float guix;
        float guiy;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Setting the size of the window
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

        }

        protected override void Initialize()
        {
                base.Initialize();

                //Initializing the color of the ui elements
                for(int a = 0;a < 3;a++)
                {
                    uiColors[a] = Color.LightGoldenrodYellow;
                }

                //Making the mouse visible
                IsMouseVisible = true;

                //Initializing the color of the shells
                shellColor[0] = new Color(255, 255, 255);
                shellColor[1] = new Color(255, 255, 255);
        }


        protected override void LoadContent()
        {
            
            spriteBatch = new SpriteBatch(GraphicsDevice);

            if (this.IsActive)
            {
                if (isMainiMenu)
                {
                    mainMenuBackground = Content.Load<Texture2D>("Main Menu Content\\Main Menu Background 1280x720");
                    startGameUI = Content.Load<Texture2D>("Main Menu Content\\Start Game Egg");
                    optionsUI = Content.Load<Texture2D>("Main Menu Content\\Options Egg");
                    quitUI = Content.Load<Texture2D>("Main Menu Content\\Quit Egg");
                    gameTitle = Content.Load<SpriteFont>("Main Menu Content\\Game Title");
                    debug = Content.Load<SpriteFont>("Debug Font");
                    mainMenuMusic = Content.Load<Song>("Main Menu Content\\Main Menu Music");

                    //Initializing UI Rectangles
                    uiRectangles[0] = new Rectangle(540,200 ,startGameUI.Width - 50, startGameUI.Height - 50);
                    uiRectangles[1] = new Rectangle(540, 400, optionsUI.Width - 50, optionsUI.Height - 50);
                    uiRectangles[2] = new Rectangle(540, 575, quitUI.Width - 50, quitUI.Height - 50);

                    //Playing the main menu music
                    MediaPlayer.Play(mainMenuMusic);
                    MediaPlayer.IsRepeating = true;

                    //Initializing the options selected by the player
                    if (optionsMenu.options.ValueOf("Mute"))
                    {
                        MediaPlayer.Pause();
                        soundIsOn = false;
                    }
                    else
                    {
                        MediaPlayer.Resume();
                        soundIsOn = true;

                    }
                }

                else if (isOptions)
                {
                    optionsMenu.LoadContent(Content);
                    credits.LoadContent(Content);
                }

                else if (isGame)
                {
                    MediaPlayer.Stop();

                    gameBackground = Content.Load<Texture2D>("Game Content\\Game Background");
                    duckFlyingLeft = Content.Load<Texture2D>("Game Content\\Duck Flying Animation");
                    crosshair = Content.Load<Texture2D>("Game Content\\Crosshair 1");
                    explosion = Content.Load<Texture2D>("Game Content\\Explosion");
                    doubleShotgun = Content.Load<Texture2D>("Game Content\\Double Barrel Shotgun");
                    doubleBarrelBlast = Content.Load<SoundEffect>("Game Content\\10 Guage Shotgun v2");
                    duckQuack = Content.Load<SoundEffect>("Game Content\\Quack Quack-SoundBible.com-620056916");
                    inGameUI = Content.Load<SpriteFont>("Game Content\\InGame UI");
                    duckRoast = Content.Load<Texture2D>("Game Content\\Duck Roast");
                    debug = Content.Load<SpriteFont>("Debug Font");
                    lostScreenUi = Content.Load<SpriteFont>("Lost Font");
                    scoreUI = Content.Load<SpriteFont>("Lost Screen UI");
                    credits.LoadContent(Content);

                    for (int a = 0; a < 2; a++)
                    {
                        doubleBarrelShells[a] = Content.Load<Texture2D>("Game Content//Shell");
                    }

                    //Creating a sound effect for the double barrel shotgun
                    doubleBarrelBlastInst = doubleBarrelBlast.CreateInstance();

                    //Creating a instance of the quack sound effect
                    duckQuackInst = duckQuack.CreateInstance();

                    //Initializing the rectangle for the crosshair
                    crosshairRect = new Rectangle(0, 0, crosshair.Width - 150, crosshair.Height - 150);

                    //Adding shotguns
                    shotguns[0] = doubleShotgun;

                    //Initializing the rectangles for the shotguns
                    shotgunRectangles[0] = new Rectangle(((graphics.GraphicsDevice.Viewport.Width / 2) + (shotguns[0].Width)) - shotguns[0].Width, graphics.GraphicsDevice.Viewport.Height - 200, 100, 200);
                }
            }
        }


        protected override void UnloadContent()
        {
            Content.Unload();
        }


        protected override void Update(GameTime gameTime)
        {

         if(this.IsActive)
          {
            
            //Checking whether the game window is active
            isA = this.IsActive;
           
            if (!lost)
            {
                if (isMainiMenu)
                {               
                    if(optionsMenu.justReturned)
                   {
                       LoadContent();
                       optionsMenu.justReturned = false;
                   }

                   //Checking if the game window is active to play music
                   if (!optionsMenu.options.ValueOf("Mute"))
                   {
                       MediaPlayer.Resume();
                   }
                    
                    //Making the mouse pointer visible
                    IsMouseVisible = true;

                    //Checking if mouse is hovering above on of the buttons
                    //or if one of the has been clicked
                    for (int a = 0; a < 3; a++)
                    {
                        if (uiRectangles[a].Contains(Mouse.GetState().Position))
                        {

                            uiColors[a] = Color.Red;

                            //Checking if Quit button has been pressed
                            if (a == 2 && Mouse.GetState().LeftButton == ButtonState.Pressed && this.IsActive)
                            {
                                this.Exit();
                            }

                            //Checking if 'Start Game' Button has been pressed
                            if (a == 0 && Mouse.GetState().LeftButton == ButtonState.Pressed && this.IsActive)
                            {
                                isMainiMenu = false;
                                isGame = true;

                                //Transitioning from main menu to the game by Unloading and Loading content
                                UnloadContent();
                                LoadContent();

                                startMouseTimer = true;
                                //Resetting the mouse press elapsed time to 0
                                mousePressElapsed = 0;

                                //Resetting everything to default
                                playerAmmo = 100;
                                score = 0;
                                playerChances = 20;
                                doubleBarrelAmmoCounter = 2;
                            }

                            //Checking if 'Options' has been selected
                            if (a == 1 && Mouse.GetState().LeftButton == ButtonState.Pressed && this.IsActive)
                            {
                                isMainiMenu = false;
                                isOptions = true;
                                
                                //Transitioning from main menu to the options screen by Loading content
                                LoadContent();

                                startMouseTimer = true;
                                //Resetting the mouse press elapsed time to 0
                                mousePressElapsed = 0;
                            }
                        }
                        else
                        {
                            uiColors[a] = Color.LightGoldenrodYellow;
                        }
                    }
                }

                //Switching control to options screen
                else if (isOptions)
                {
                    optionsMenu.Update(gameTime);
                }

                //Switching control to the main game
                if (isGame)
                {
                    
                    //Making the mouse pointer invisible so that it does not interfere with the pointer
                    IsMouseVisible = false;

                    //Returning to the Main Menu
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                    {
                        isGame = false;
                        isMainiMenu = true;

                        //Transitioning from the game to the main menu by loading and unloading content
                        UnloadContent();
                        LoadContent();
                    }

                    //Moving the crosshair
                    crosshairRect.X = Mouse.GetState().X - (crosshairRect.Width / 2);
                    crosshairRect.Y = Mouse.GetState().Y - (crosshairRect.Height / 2);

                    //Creating a new duck
                    InstantiateDuck(gameTime);

                    //Calling the update function of each individual duck
                    for (int a = 0; a < ducksToDisplay.Count; a++)
                    {
                        if (ducksToDisplay[a] != null)
                        {
                            ducksToDisplay[a].Update();
                            if (ducksToDisplay[a].duckDestRect.Contains(new Point(Mouse.GetState().X, Mouse.GetState().Y)) && Mouse.GetState().LeftButton == ButtonState.Pressed && mousePressElapsed >= doubleBarrelBlast.Duration.TotalSeconds && loaded && this.IsActive)
                            {
                                ducksToDisplay[a].hit = true; //Sending the message that the duck has been hit

                                if (soundIsOn)
                                {
                                    doubleBarrelBlastInst.Play(); //Playing the sound of the shotgun

                                    duckQuackInst.Play(); //Playing the sound of the duck
                                }

                                doubleBarrelAmmoCounter--; //Reducing ammo from the gun

                                startMouseTimer = true; //Starting the mouse timer

                                mousePressElapsed = 0; //Resetting the timer to zero

                                score++; //Incrementing the score as a duck has been hit
                            }

                            //Checking if the duck has crossed the screen
                            if (ducksToDisplay[a].duckDestRect.X <= 0)
                            {
                                playerChances--;
                                ducksToDisplay[a] = null;
                            }
                        }
                    }

                    //Checking if the player's shotgun is empty
                    if (doubleBarrelAmmoCounter == 0)
                    {
                        loaded = false;
                    }
                    else
                    {
                        loaded = true;
                    }

                    //Checking if the player wants to reload
                    if (playerAmmo >= 0 && Keyboard.GetState().IsKeyDown(Keys.R) && this.IsActive)
                    {
                        playerAmmo -= (doubleBarrelMaxCapacity - doubleBarrelAmmoCounter);
                        doubleBarrelAmmoCounter += (doubleBarrelMaxCapacity - doubleBarrelAmmoCounter);
                    }
                    else if(playerAmmo < 0 && Keyboard.GetState().IsKeyDown(Keys.R))
                    {
                        reasonOfLosing = "You lost all your ammo";
                        IsMouseVisible = true;
                        lost = true;
                    }

                    //Checking if the player has shot the gun
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && mousePressElapsed >= doubleBarrelBlast.Duration.TotalSeconds && loaded && this.IsActive)
                    {
                        if (soundIsOn)
                        {
                            doubleBarrelBlastInst.Play();
                        }
                        doubleBarrelAmmoCounter--;
                        startMouseTimer = true;
                        mousePressElapsed = 0;
                    }

                    //Starting the mouse timer
                    if (startMouseTimer)
                    {
                        mousePressElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    if (mousePressElapsed >= doubleBarrelBlast.Duration.TotalSeconds)
                    {
                        startMouseTimer = false;
                    }

                    //Displaying the ammo counter according to the ammo loaded
                    if (doubleBarrelAmmoCounter == 1)
                    {
                        shellColor[1] = new Color(0, 0, 0);
                    }
                    else if (doubleBarrelAmmoCounter == 0)
                    {
                        shellColor[0] = new Color(0, 0, 0);
                    }
                    else
                    {
                        shellColor[0] = Color.White;
                        shellColor[1] = Color.White;
                    }

                    //Checking if the player has lost all his ammo
                    if(playerAmmo <= 0 && doubleBarrelAmmoCounter == 0)
                    {
                        reasonOfLosing = "You lost all your ammo";
                        IsMouseVisible = true;
                        lost = true;
                    }

                    //Checking if the player has lost all his chances
                    if(playerChances == 0)
                    {
                        reasonOfLosing = "You lost all your chances";
                        IsMouseVisible = true;
                        lost = true;
                    }

                }
            
            }
            else
            {
                if(Keyboard.GetState().IsKeyDown(Keys.Escape) && this.IsActive)
                {
                    lost = false;
                    isGame = false;
                    isCredits = true;
                    startEscapeTimer = true;
                }
            }

            if(startEscapeTimer)
            {
                escElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if(escElapsedTime >= 1.0f)
            {
                startEscapeTimer = false;
            }

            if(isCredits)
            {
                //Making the mouse visible
                IsMouseVisible = true;
               
                
                if(Keyboard.GetState().IsKeyDown(Keys.Escape) && escElapsedTime >= 1.0f && this.IsActive)
                {
                    isCredits = false;
                    isMainiMenu = true;
                    escElapsedTime = 0;

                    //Transitioning from Credits to Main Menu by loading content
                    LoadContent();
                }
            }
         }
          else
          {
              MediaPlayer.Pause();
          }

                base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {

            if (!lost)
            {
                if (isMainiMenu)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(mainMenuBackground, new Rectangle(0,0,1280,720), Color.White);
                    spriteBatch.DrawString(gameTitle, "Duck Hunt 2D", new Vector2(500, 50), Color.Gold);

                    //Drawing the main Menu UI
                    spriteBatch.Draw(startGameUI, uiRectangles[0], uiColors[0]);
                    spriteBatch.Draw(optionsUI, uiRectangles[1], uiColors[1]);
                    spriteBatch.Draw(quitUI, uiRectangles[2], uiColors[2]);

                    spriteBatch.End();
                }

                else if (isOptions)
                {
                    GraphicsDevice.Clear(Color.DeepSkyBlue);
                    optionsMenu.Draw(spriteBatch);
                }

                else if (isGame)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(gameBackground, new Rectangle(0, 0, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);

                    //Drawing the duck
                    if (duckValue > 0)
                    {
                        for (int a = 0; a < ducksToDisplay.Count; a++)
                        {
                            if (ducksToDisplay[a] != null)
                            {
                                if (!ducksToDisplay[a].hit) //Does not draw the duck if it has been hit
                                {
                                    ducksToDisplay[a].Draw(spriteBatch);
                                }
                                else
                                {
                                    ducksToDisplay[a] = null;
                                }
                            }
                        }
                    }

                    //Drawing the ammo indicator for the double barrel shotgun
                    for (int ctr = 0; ctr < 2; ctr++)
                    {
                        if (ctr == 0)
                        {
                            spriteBatch.Draw(doubleBarrelShells[ctr], new Vector2(graphics.GraphicsDevice.Viewport.Width - 200, graphics.GraphicsDevice.Viewport.Height - 110), shellColor[0]);
                        }
                        else
                        {
                            spriteBatch.Draw(doubleBarrelShells[ctr], new Vector2(graphics.GraphicsDevice.Viewport.Width - 250, graphics.GraphicsDevice.Viewport.Height - 110), shellColor[1]);
                        }
                    }
                  

                    //Showing the ammo owned by the player
                    spriteBatch.DrawString(inGameUI, "Ammo Left: " + playerAmmo, new Vector2(graphics.GraphicsDevice.Viewport.Width - 300, graphics.GraphicsDevice.Viewport.Height - 150), Color.Thistle);

                    spriteBatch.Draw(duckRoast, new Vector2(50, graphics.GraphicsDevice.Viewport.Height - 125), Color.White);

                    //Showing the score of the player
                    spriteBatch.DrawString(inGameUI, "Ducks Hit: ", new Vector2(50, graphics.GraphicsDevice.Viewport.Height - 150), Color.Tomato);
                    spriteBatch.DrawString(inGameUI, score.ToString(), new Vector2(100, graphics.GraphicsDevice.Viewport.Height - 90), Color.Tomato);

                    //Showing the chances left with the player
                    spriteBatch.DrawString(inGameUI, "Chances Left:", new Vector2((graphics.GraphicsDevice.Viewport.Width / 2) - 150, graphics.GraphicsDevice.Viewport.Height - 140), Color.Bisque);
                    spriteBatch.DrawString(inGameUI, playerChances.ToString(), new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 100, graphics.GraphicsDevice.Viewport.Height - 100), Color.Bisque);

                    //Drawing the crosshair
                    spriteBatch.Draw(crosshair, crosshairRect, Color.White);

                    spriteBatch.End();
                }
            }
            else
            {
                GraphicsDevice.Clear(Color.DeepSkyBlue);
                spriteBatch.Begin();
                spriteBatch.DrawString(lostScreenUi,reasonOfLosing,new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 600,graphics.GraphicsDevice.Viewport.Height / 2 - 300),Color.IndianRed);
                spriteBatch.DrawString(scoreUI, "Score: " + score.ToString(), new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 110, graphics.GraphicsDevice.Viewport.Height / 2 - 100), Color.Maroon);
                spriteBatch.DrawString(scoreUI, "Your Highscore: " + ReturnPlayerHighScore(), new Vector2(graphics.GraphicsDevice.Viewport.Width / 2 - 200, graphics.GraphicsDevice.Viewport.Height / 2), Color.Maroon);
                spriteBatch.End();
            }
            
            if(isCredits)
            {
                GraphicsDevice.Clear(Color.GhostWhite);
                credits.Draw(spriteBatch);
            }

            base.Draw(gameTime);
        }

        void InstantiateDuck(GameTime gameTime)
        {
            //Decreasing the delay as the player's score increases to make the game more difficulty
            if(score >= 10)
            {
                duckDelay = 1.8f;
            }
            if(score >= 20)
            {
                duckDelay = 1.6f;
            }
            if(score >= 30)
            {
                duckDelay = 1.4f;
            }
            if (score >= 40)
            {
                duckDelay = 1.2f;
            }
            if (score >= 50)
            {
                duckDelay = 1f;
            }
            if (score >= 60)
            {
                duckDelay = 0.8f;
            }
            if (score >= 100)
            {
                duckDelay = 0.6f;
            }

            Debug.WriteLine(duckDelay);

            duckElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(duckElapsedTime >= duckDelay)
            {
                ducksToDisplay.Add(new DuckFlight(true, duckFlyingLeft)); //Adding a new duck object to the list
                
                ducksToDisplay[duckValue].GameTime = gameTime; //Providing gameTime to the object
                
                ducksToDisplay[duckValue].duckDestRect.X = (graphics.GraphicsDevice.Viewport.Width - 400); //Initializing the x coordinate of the duck

                //Increasing the speed of the duck if the player has obtained a particular score
                if (score >= 20)
                {
                    ducksToDisplay[duckValue].duckSpeed = 8f;
                    ducksToDisplay[duckValue].delay = 0.9f; //Decreasing the delay of the animation to match the speed of the duck
                }
                if(score >= 40)
                {
                    ducksToDisplay[duckValue].duckSpeed = 10;
                    ducksToDisplay[duckValue].delay = 0.7f; //Decreasing the delay of the animation to match the speed of the duck
                }
                if(score >= 60)
                {
                    ducksToDisplay[duckValue].duckSpeed = 12;
                    ducksToDisplay[duckValue].delay = 0.5f; //Decreasing the delay of the animation to match the speed of the duck
                }
                if(score >= 100)
                {
                    ducksToDisplay[duckValue].duckSpeed = 14;
                    ducksToDisplay[duckValue].delay = 0.3f; //Decreasing the delay of the animation to match the speed of the duck
                }

                //Giving values to the random class
                rand = new Custom_Random<int>(150,300,75);

                ducksToDisplay[duckValue].duckDestRect.Y = (graphics.GraphicsDevice.Viewport.Height / 2) - rand.ReturnRandom(); //Initializing the y coordinate of the duck

                duckValue++; //Incrementing the duck in order to add a new duck to the list and ascess it
                
                duckElapsedTime = 0; //S\Resetting time to 0
                
            }
        }
   
        int ReturnPlayerHighScore()
        {
            int playerHighScore = 0;
            
            if(!File.Exists("PlayerData.txt"))
            {
                StreamWriter write = new StreamWriter("PlayerData.txt");
                write.WriteLine(0);
                write.Close();
            }
            else
            {
                StreamReader read = new StreamReader("PlayerData.txt");
                playerHighScore = int.Parse(read.ReadLine());

                read.Close();

                if (playerHighScore < score)
                {
                    StreamWriter write = new StreamWriter("PlayerData.txt");
                    write.WriteLine(score);
                    write.Close();
                }
            }

            return playerHighScore;
        }

        public static bool isActive
        {
            get {return isA; }
            set { isA = value; }
        }
     }
 }


