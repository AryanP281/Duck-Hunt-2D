using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Duck_Hunt
{    
    
    class Credits
    {
        SpriteFont credits;
        SpriteFont names;

        string webAddress = @"http://firereign.newgrounds.com/";

        public Credits()
        {

        }

        public void LoadContent(ContentManager content)
        {
            credits = content.Load<SpriteFont>("Credits title");
            names = content.Load<SpriteFont>("Credits Names");
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            
            //Showing the titles
            spriteBatch.DrawString(credits, "PROGRAMMING", new Vector2(20, 100), Color.Black);
            spriteBatch.DrawString(credits, "ART", new Vector2(100, 300), Color.Black);
            spriteBatch.DrawString(credits, "DESIGN", new Vector2(1000, 100), Color.Black);
            spriteBatch.DrawString(credits, "MUSIC", new Vector2(1000, 300), Color.Black);

            spriteBatch.DrawString(names, "Aryan[Gameologist]", new Vector2(50, 175), Color.Black);
            spriteBatch.DrawString(names, "Aryan[Gameologist]", new Vector2(50, 375), Color.Black);
            spriteBatch.DrawString(names, "Aryan[Gameologist]", new Vector2(975, 175), Color.Black);
            spriteBatch.DrawString(names, "FireReign", new Vector2(1000, 375), Color.Black);
            spriteBatch.DrawString(names, "[" + webAddress + "]", new Vector2(900,425),Color.Black);
            spriteBatch.End();
        }
    }
}

