using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Paper_Tanks;
using GameStateManagement;

namespace Paper_Tanks
{
    public class Class1 : GameScreen
    {
        int i = 0;
        SoundEffect gameover;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        GraphicsDevice device;
        Texture2D gameovr;
        int screenWidth;
        int screenHeight;
        String s;
        SpriteFont font;
        public Class1(int a)
        {
            if (a == 1)
                s = "player 1 wins";
            else
                s = "player 2 wins";

        }
        public override void LoadContent()
        {
            gameover = Load<SoundEffect>("sounds/gameovr");
            device = ScreenManager.Game.GraphicsDevice;
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
            gameovr = Load<Texture2D>("Textures/Backgrounds/gameover");
            font = Load<SpriteFont>("myFont");
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {


            spriteBatch.Begin();
            drawscreen();
            if (i==0)
                gameover.Play();
            i++;
            if (i > 10000)
                i = 1;
            DrawText();
            spriteBatch.End();

            base.Draw(gameTime);

        }

        private void drawscreen()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            spriteBatch.Draw(gameovr, screenRectangle, Color.White);

        }
        public override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
 
        private void DrawText()
        {
              spriteBatch.DrawString(font, s, new Vector2(600, 20), Color.Black);
        }
    
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
