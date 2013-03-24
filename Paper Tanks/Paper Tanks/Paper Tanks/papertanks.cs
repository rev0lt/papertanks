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
    public struct PlayerData
    {
        public Vector2 Position;
        public bool IsAlive;
        public Color Color;
        public float Angle;
        public float Power;
        public int hp;
        public int mode;
        public int moves;
    }

    public struct ParticleData
    {
        public float BirthTime;
        public float MaxAge;
        public Vector2 OrginalPosition;
        public Vector2 Accelaration;
        public Vector2 Direction;
        public Vector2 Position;
        public float Scaling;
        public Color ModColor;
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class papertanks : GameScreen
    {
        float sval;
         Texture2D sliderBackground; //Texture of the slider background
         Vector2 posBackground; //position of the slider background
         Rectangle sliderBackgroundRectangle;
         Texture2D slider;
         Vector2 posSlider;
         Rectangle sliderRectangle;
         Rectangle touchRectangle;
         private bool touched = false;

         //devider that we get a value between 0 - 100 
         private float divider;     
        //SoundEffect rfly;
        SoundEffect bclick;
        SoundEffect cclick;
        SoundEffect hit;
        SoundEffect launch;
        Texture2D smokeTexture;
        Texture2D rocketTexture;
        GraphicsDevice device;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D backgroundTexture;
        Texture2D foregroundTexture;
        Texture2D carriageTexture;
        Texture2D cannonTexture;
        Texture2D groundTexture;
        Texture2D increaseTexture;
        Texture2D decreaseTexture;
        Texture2D fireTexture;
        Texture2D switchTexture;
        Texture2D explosionTexture;
        Vector2 decPosition1;
        Vector2 incPosition1;
        Vector2 decPosition2;
        Vector2 incPosition2;
        Vector2 firePosition;
        Vector2 switchPosition;
        Vector2 cannonOrigin;
        int screenWidth;
        int screenHeight;
        PlayerData[] players;
        int numberOfPlayers = 2;
        float playerScaling;
        float playerscal2;
        List<Vector2> smokeList = new List<Vector2>(); Random randomizer = new Random();
        int[] terrainContour;
        bool touching;
        int currentPlayer = 0;
        Rectangle box;
        Rectangle box1;
        Rectangle box2;
        Rectangle box3;
        Rectangle box4;
        Rectangle box5;
        SpriteFont font;
        bool rocketFlying = false;
        Vector2 rocketPosition;
        Vector2 rocketDirection;
        float rocketAngle;
        float rocketScaling = 0.1f;
        Color[,] rocketColorArray;
        Color[,] foregroundColorArray;
        Color[,] carriageColorArray;
        Color[,] cannonColorArray;
        string[] modes = new string[3];
        

        List<ParticleData> particleList = new List<ParticleData>();
        Color[,] explosionColorArray;
        public papertanks()
        {
            

            // Frame rate is 30 fps by default for Windows Phone.
        
           // graphics.IsFullScreen = true;


        }
        private void SetUpPlayers()
        {
            Color[] playerColors = new Color[10];
            playerColors[0] = Color.Red;
            playerColors[1] = Color.Green;
            //playerColors[2] = Color.Blue;
            //playerColors[3] = Color.Purple;
            //playerColors[4] = Color.Orange;
            //playerColors[5] = Color.Indigo;
            //playerColors[6] = Color.Yellow;
            //playerColors[7] = Color.SaddleBrown;
            //playerColors[8] = Color.Tomato;
            //playerColors[9] = Color.Turquoise;

            players = new PlayerData[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players[i].IsAlive = true;
                players[i].Color = playerColors[i];
                players[i].Angle = MathHelper.ToRadians(90);
                players[i].Power = 100;
                players[i].hp = 0;
                players[i].mode = 0;
                players[i].moves = 3;
                players[i].Position = new Vector2();
                if (i == 0)
                    players[i].Position.X = (screenWidth / (numberOfPlayers + 1) * (i + 1)) - 100;
                else
                    players[i].Position.X = (screenWidth / (numberOfPlayers + 1) * (i + 1)) + 100;
                players[i].Position.Y = terrainContour[(int)players[i].Position.X];
            }


        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
      //  public  void Initialize()
       // {
            // TODO: Add your initialization logic here
            
            //base.Initialize();
      // }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent()
        {
           

            

            //later we want to get values between 0 and 100 from our slider
            
            bclick = Load<SoundEffect>("sounds/bclick"); 
            cclick = Load<SoundEffect>("sounds/cclick");
           // rfly = Load<SoundEffect>("sounds/rfly");
            launch = Load<SoundEffect>("sounds/launch");
            hit = Load<SoundEffect>("sounds/hit");
            device = ScreenManager.Game.GraphicsDevice;
            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;
            smokeTexture = Load<Texture2D>("Textures/Backgrounds/smoke");
            backgroundTexture = Load<Texture2D>("Textures/Backgrounds/Background");
            foregroundTexture = Load<Texture2D>("Textures/Backgrounds/Foreground");
            carriageTexture = Load<Texture2D>("Textures/Backgrounds/Tank2-1");
            cannonTexture = Load<Texture2D>("Textures/Backgrounds/barrel3");
            groundTexture = Load<Texture2D>("Textures/Backgrounds/texture");
            increaseTexture = Load<Texture2D>("Textures/Buttons/increase");
            decreaseTexture = Load<Texture2D>("Textures/Buttons/decrease");
            fireTexture = Load<Texture2D>("Textures/Buttons/FireSmall");
            switchTexture = Load<Texture2D>("Textures/Buttons/Switchsmall");
            font = Load<SpriteFont>("myFont");
            rocketTexture = Load<Texture2D>("Textures/Backgrounds/rocket");
            explosionTexture = Load<Texture2D>("Textures/Explosion/explosion");
            box = new Rectangle(20, 350, decreaseTexture.Width + 40, 410);
            box1 = new Rectangle(130, 350, increaseTexture.Width + 50, 410);
            //box2 = new Rectangle(screenWidth - 220, 350, decreaseTexture.Width + 40, 410);
            //box3 = new Rectangle(screenWidth - 90, 350, increaseTexture.Width + 50, 410);
            box4 = new Rectangle((screenWidth / 2) - 90, 350, fireTexture.Width + 90, 410);
            box5 = new Rectangle(10,110,switchTexture.Width+20,switchTexture.Height+20);
            modes[0] = "Rocket";
            modes[1] = "Move Left";
            modes[2] = "Move Right"; decPosition1 = new Vector2(50, 375);
            incPosition1 = new Vector2(150, 375);
            decPosition2 = new Vector2(screenWidth - 200, 395);
            incPosition2 = new Vector2(screenWidth - 90, 375);
            sliderBackground = Load<Texture2D>("Textures/Buttons/slider");
            posBackground = decPosition2;

            slider = Load<Texture2D>("Textures/Buttons/sbutton");
            posSlider = new Vector2(posBackground.X + (sliderBackground.Width) / 2,
            posBackground.Y);

            sliderRectangle = new Rectangle((int)posSlider.X,
            (int)posSlider.Y, slider.Width, slider.Height); divider = (sliderBackground.Width - slider.Width) / 100f;
            touchRectangle = new Rectangle(0, 0, 30, 30);
            sliderBackgroundRectangle = new Rectangle((int)posBackground.X - 40,
            (int)posBackground.Y - 10, sliderBackground.Width + 80,
            sliderBackground.Height + 20);
            rocketColorArray = TextureTo2DArray(rocketTexture);
            carriageColorArray = TextureTo2DArray(carriageTexture);
            cannonColorArray = TextureTo2DArray(cannonTexture);


            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);
            playerScaling = 50.0f / (float)carriageTexture.Width;

            GenerateTerrainContour();
            SetUpPlayers();
            FlattenTerrainBelowPlayers();
            CreateForeground();
            explosionColorArray = TextureTo2DArray(explosionTexture);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        private void FlattenTerrainBelowPlayers()
        {
            foreach (PlayerData player in players)
                if (player.IsAlive)
                    for (int x = 0; x < 57; x++)
                        terrainContour[(int)player.Position.X + x] = terrainContour[(int)player.Position.X];
        }
        private void GenerateTerrainContour()
        {
            terrainContour = new int[screenWidth];

            double rand1 = randomizer.NextDouble() + 1;
            double rand2 = randomizer.NextDouble() + 2;
            double rand3 = randomizer.NextDouble() + 3;

            float offset = screenHeight / 2;
            float peakheight = 100;
            float flatness = 70;

            for (int x = 0; x < screenWidth; x++)
            {
                double height = peakheight / rand1 * Math.Sin((float)x / flatness * rand1 + rand1);
                height += peakheight / rand2 * Math.Sin((float)x / flatness * rand2 + rand2);
                height += peakheight / rand3 * Math.Sin((float)x / flatness * rand3 + rand3);
                height += offset;
                terrainContour[x] = (int)height;
            }
        }
        private void CreateForeground()
        {
            Color[,] groundColors = TextureTo2DArray(groundTexture);
            Color[] foregroundColors = new Color[screenWidth * screenHeight];

            for (int x = 0; x < screenWidth; x++)
            {
                for (int y = 0; y < screenHeight; y++)
                {
                    if (y > terrainContour[x])
                        foregroundColors[x + y * screenWidth] = groundColors[x % groundTexture.Width, y % groundTexture.Height];
                    else
                        foregroundColors[x + y * screenWidth] = Color.Transparent;
                }
            }

            foregroundTexture = new Texture2D(device, screenWidth, screenHeight, false, SurfaceFormat.Color);
            foregroundTexture.SetData(foregroundColors);
            foregroundColorArray = TextureTo2DArray(foregroundTexture);
        }
        private Color[,] TextureTo2DArray(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);

            Color[,] colors2D = new Color[texture.Width, texture.Height];
            for (int x = 0; x < texture.Width; x++)
                for (int y = 0; y < texture.Height; y++)
                    colors2D[x, y] = colors1D[x + y * texture.Width];

            return colors2D;
        }

        public override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
               // this.Exit();

            // TODO: Add your update logic here

            TouchCollection touches = TouchPanel.GetState();
            if (!touching && touches.Count > 0)
            {
                //spriteBatch.DrawString(font, "Touch: "+ touches.First().Position.X , new Vector2(20, 65),Color.Red);
                touching = true;
                TouchLocation touch = touches.First();

                if (box.Contains((int)touch.Position.X, (int)touch.Position.Y))
                {
                    cclick.Play();
                    players[currentPlayer].Angle -= 0.1f;
                }
                else if (box1.Contains((int)touch.Position.X, (int)touch.Position.Y))
                {
                    cclick.Play();
                    players[currentPlayer].Angle += 0.1f;
                }

                if (players[currentPlayer].Angle > MathHelper.PiOver2)
                    players[currentPlayer].Angle = -MathHelper.PiOver2;
                if (players[currentPlayer].Angle < -MathHelper.PiOver2)
                    players[currentPlayer].Angle = MathHelper.PiOver2;

                sinput(gameTime);
                //if (box2.Contains((int)touch.Position.X, (int)touch.Position.Y))
                //{
                //    cclick.Play();
                //    players[currentPlayer].Power -= 50;

                //}
                //else if (box3.Contains((int)touch.Position.X, (int)touch.Position.Y))
                //{
                //    cclick.Play();
                //    players[currentPlayer].Power += 50;

                //}
                players[currentPlayer].Power = 10*((int)sval);

                if (players[currentPlayer].Power > 1000)
                    players[currentPlayer].Power = 1000;
                if (players[currentPlayer].Power < 0)
                    players[currentPlayer].Power = 0;
                if (box4.Contains((int)touch.Position.X, (int)touch.Position.Y))
                {
                    if (players[currentPlayer].moves <= 0)
                    {
                        players[currentPlayer].mode = 0;
                    }
                    if (players[currentPlayer].mode == 0)
                    {
                        rocketFlying = true;
                        launch.Play();
                        rocketPosition = players[currentPlayer].Position;
                        rocketPosition.X += 20;
                        rocketPosition.Y -= 20;
                        rocketAngle = players[currentPlayer].Angle;
                        Vector2 up = new Vector2(0, -1);
                        Matrix rotMatrix = Matrix.CreateRotationZ(rocketAngle);
                        rocketDirection = Vector2.Transform(up, rotMatrix);
                        rocketDirection *= players[currentPlayer].Power / 50.0f;
                    }
                    else if (players[currentPlayer].mode == 1)
                    {
                        players[currentPlayer].moves -= 1;
                        players[currentPlayer].Position.X -= 15;
                        players[currentPlayer].Position.Y = terrainContour[(int)players[currentPlayer].Position.X];                        
                        FlattenTerrainBelowPlayers();
                        CreateForeground();
                        NextPlayer();
                    }
                    else if (players[currentPlayer].mode == 2)
                    {
                        players[currentPlayer].moves -= 1;
                        players[currentPlayer].Position.X += 15;
                                                
                        players[currentPlayer].Position.Y = terrainContour[(int)players[currentPlayer].Position.X];                        
                        FlattenTerrainBelowPlayers();
                        //CreateForeground();
                        NextPlayer();
                    
                    }

                    ///FIRE ACTION


                }
                if (box5.Contains((int)touch.Position.X, (int)touch.Position.Y))
                {
                    cclick.Play();
                    if (players[currentPlayer].moves <= 0)
                    {
                        players[currentPlayer].mode = 0;
                    }
                    else
                    {
                        players[currentPlayer].mode += 1;
                        if (players[currentPlayer].mode == 3)
                        {
                            players[currentPlayer].mode = 0;
                        }
                    }
                    
                }


            }
            else if (touches.Count == 0)
            {
                touching = false;
            }
            if (rocketFlying)
            {
                UpdateRocket();
                CheckCollisions(gameTime);
                foreach (PlayerData player in players)
                    if (player.hp >= 2)
                        ScreenManager.AddScreen(new Class1(currentPlayer), null);
            }

            if (particleList.Count > 0)
                UpdateParticles(gameTime);

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        private void AddCrater(Color[,] tex, Matrix mat)
        {
            int width = tex.GetLength(0);
            int height = tex.GetLength(1);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (tex[x, y].R > 10)
                    {
                        Vector2 imagePos = new Vector2(x, y);
                        Vector2 screenPos = Vector2.Transform(imagePos, mat);

                        int screenX = (int)screenPos.X;
                        int screenY = (int)screenPos.Y;

                        if ((screenX) > 0 && (screenX < screenWidth))
                            if (terrainContour[screenX] < screenY)
                                terrainContour[screenX] = screenY;

                    }
                }
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
           ScreenManager.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            DrawScenery();
            DrawPlayers();
            DrawText();
            DrawRocket();
            DrawSmoke();
            
            spriteBatch.End();
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            DrawExplosion();
            spriteBatch.End();

            base.Draw(gameTime);
        }
        private void DrawScenery()
        {
            Rectangle screenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            //Rectangle buttonRectangle = new Rectangle(75, screenHeight - 250, 125, screenHeight - 50);
            decPosition1 = new Vector2(50, 375);
            incPosition1 = new Vector2(150, 375);
            decPosition2 = new Vector2(screenWidth - 200, 375);
            incPosition2 = new Vector2(screenWidth - 90, 375);
            firePosition = new Vector2((screenWidth / 2) - 90, 375);
            switchPosition = new Vector2(25, 125);
            spriteBatch.Draw(backgroundTexture, screenRectangle, Color.White);
            spriteBatch.Draw(foregroundTexture, screenRectangle, Color.White);
            spriteBatch.Draw(decreaseTexture, decPosition1, Color.White);
            spriteBatch.Draw(increaseTexture, incPosition1, Color.White);
            //posBackground.X += 50;
            //posBackground.Y += 120;
            spriteBatch.Draw(sliderBackground, posBackground, Color.Black);
            spriteBatch.Draw(slider, posSlider, Color.White); //spriteBatch.Draw(decreaseTexture, decPosition2, Color.White);
            //spriteBatch.Draw(increaseTexture, incPosition2, Color.White);
            spriteBatch.Draw(fireTexture, firePosition, Color.White);
            spriteBatch.Draw(switchTexture, switchPosition, Color.White);

        }
        private void DrawPlayers()
        {
            foreach (PlayerData player in players)
            {
                if (player.IsAlive)
                {
                    int xPos = (int)player.Position.X;
                    int yPos = (int)player.Position.Y;
                    cannonOrigin = new Vector2(32, cannonTexture.Height - 5);
                    playerscal2 = ((1.5f) * playerScaling) / 10;
                    spriteBatch.Draw(cannonTexture, new Vector2(xPos + 20, yPos - 20), null, player.Color, player.Angle, cannonOrigin, playerscal2, SpriteEffects.None, 1);
                    spriteBatch.Draw(carriageTexture, player.Position, null, player.Color, 0, new Vector2(0, carriageTexture.Height), playerScaling, SpriteEffects.None, 0);

                }
            }
        }
        private void DrawText()
        {
            PlayerData player = players[currentPlayer];
            int currentAngle = (int)MathHelper.ToDegrees(player.Angle);
            spriteBatch.DrawString(font, "Cannon angle: " + currentAngle.ToString(), new Vector2(20, 20), player.Color);
            spriteBatch.DrawString(font, "Cannon power: " + player.Power.ToString(), new Vector2(20, 45), player.Color);
            spriteBatch.DrawString(font, "Mode:" + modes[player.mode], new Vector2(20, 70), player.Color);
            spriteBatch.DrawString(font, "Moves Left:" + player.moves, new Vector2(20, 90), player.Color);
            spriteBatch.DrawString(font, "points:" + player.hp.ToString(), new Vector2(600, 20), player.Color);
        }

        private void DrawRocket()
        {
            if (rocketFlying)
             spriteBatch.Draw(rocketTexture, rocketPosition, null, Color.Black, rocketAngle, new Vector2(42, 240), 0.1f, SpriteEffects.None, 1);
        }      

        private void UpdateRocket()
        {
            if (rocketFlying)
            {
                Vector2 gravity = new Vector2(0, 1);
                rocketDirection += gravity / 10.0f;
                rocketPosition += rocketDirection;
                rocketAngle = (float)Math.Atan2(rocketDirection.X, -rocketDirection.Y);
                Vector2 smokePos = rocketPosition;
                smokePos.X += randomizer.Next(10) - 5;
                smokePos.Y += randomizer.Next(10) - 5;
                smokeList.Add(smokePos);
            }
        }
        private void DrawSmoke()
        {
            foreach (Vector2 smokePos in smokeList)
                spriteBatch.Draw(smokeTexture, smokePos, null, Color.Black, 0, new Vector2(40, 35), 0.2f, SpriteEffects.None, 1);
        }

        private Vector2 TexturesCollide(Color[,] tex1, Matrix mat1, Color[,] tex2, Matrix mat2)
        {
            Matrix mat1to2 = mat1 * Matrix.Invert(mat2);
            int width1 = tex1.GetLength(0);
            int height1 = tex1.GetLength(1);
            int width2 = tex2.GetLength(0);
            int height2 = tex2.GetLength(1);

            for (int x1 = 0; x1 < width1; x1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    Vector2 pos1 = new Vector2(x1, y1);
                    Vector2 pos2 = Vector2.Transform(pos1, mat1to2);

                    int x2 = (int)pos2.X;
                    int y2 = (int)pos2.Y;
                    if ((x2 >= 0) && (x2 < width2))
                    {
                        if ((y2 >= 0) && (y2 < height2))
                        {
                            if (tex1[x1, y1].A > 0)
                            {
                                if (tex2[x2, y2].A > 0)
                                {
                                    Vector2 screenPos = Vector2.Transform(pos1, mat1);
                                    return screenPos;
                                }
                            }
                        }
                    }
                }
            }

            return new Vector2(-1, -1);
        }

        private Vector2 CheckTerrainCollision()
        {
            Matrix rocketMat = Matrix.CreateTranslation(-42, -240, 0) * Matrix.CreateRotationZ(rocketAngle) * Matrix.CreateScale(rocketScaling) * Matrix.CreateTranslation(rocketPosition.X, rocketPosition.Y, 0);
            Matrix terrainMat = Matrix.Identity;
            Vector2 terrainCollisionPoint = TexturesCollide(rocketColorArray, rocketMat, foregroundColorArray, terrainMat);
            return terrainCollisionPoint;
        }
        private Vector2 CheckPlayersCollision()
        {
            Matrix rocketMat = Matrix.CreateTranslation(-42, -240, 0) * Matrix.CreateRotationZ(rocketAngle) * Matrix.CreateScale(rocketScaling) * Matrix.CreateTranslation(rocketPosition.X, rocketPosition.Y, 0);
            for (int i = 0; i < numberOfPlayers; i++)
            {
                PlayerData player = players[i];
                if (player.IsAlive)
                {
                    if (i != currentPlayer)
                    {
                        int xPos = (int)player.Position.X;
                        int yPos = (int)player.Position.Y;

                        Matrix carriageMat = Matrix.CreateTranslation(0, -carriageTexture.Height, 0) * Matrix.CreateScale(playerScaling) * Matrix.CreateTranslation(xPos, yPos, 0);
                        Vector2 carriageCollisionPoint = TexturesCollide(carriageColorArray, carriageMat, rocketColorArray, rocketMat);

                        if (carriageCollisionPoint.X > -1)
                        {
                            
                            players[currentPlayer].hp += 1;//players[i].IsAlive = false;
                            return carriageCollisionPoint;
                        }

                        Matrix cannonMat = Matrix.CreateTranslation(-cannonOrigin.X, -cannonOrigin.Y, 0) * Matrix.CreateRotationZ(player.Angle) * Matrix.CreateScale(playerscal2) * Matrix.CreateTranslation(xPos + 20, yPos - 20, 0);
                        Vector2 cannonCollisionPoint = TexturesCollide(cannonColorArray, cannonMat, rocketColorArray, rocketMat);
                        if (cannonCollisionPoint.X > -1)
                        {
                            
                            players[currentPlayer].hp += 1;//players[i].IsAlive = false;
                            return cannonCollisionPoint;
                        }
                    }
                }
            }
            return new Vector2(-1, -1);
        }
        private bool CheckOutOfScreen()
        {
            bool rocketOutOfScreen = rocketPosition.Y > screenHeight;
            rocketOutOfScreen |= rocketPosition.X < 0;
            rocketOutOfScreen |= rocketPosition.X > screenWidth;

            return rocketOutOfScreen;
        }
        private void CheckCollisions(GameTime gameTime)
        {
            Vector2 terrainCollisionPoint = CheckTerrainCollision();
            Vector2 playerCollisionPoint = CheckPlayersCollision();
            bool rocketOutOfScreen = CheckOutOfScreen();

            if (playerCollisionPoint.X > -1)
            {
                rocketFlying = false;
                hit.Play();
                smokeList = new List<Vector2>();
                AddExplosion(playerCollisionPoint, 10, 80.0f, 2000.0f, gameTime);

                NextPlayer();
            }

            if (terrainCollisionPoint.X > -1)
            {
                rocketFlying = false;
                hit.Play();
                smokeList = new List<Vector2>();
                AddExplosion(terrainCollisionPoint, 4, 30.0f, 1000.0f, gameTime);

                NextPlayer();
            }

            if (rocketOutOfScreen)
            {
                rocketFlying = false;

                smokeList = new List<Vector2>();
                NextPlayer();
            }
        }
        private void NextPlayer()
        {

            currentPlayer = currentPlayer + 1;
            currentPlayer = currentPlayer % numberOfPlayers;
            while (!players[currentPlayer].IsAlive)
                currentPlayer = ++currentPlayer % numberOfPlayers;
        }

        private void AddExplosion(Vector2 explosionPos, int numberOfParticles, float size, float maxAge, GameTime gameTime)
        {
            for (int i = 0; i < numberOfParticles; i++)
                AddExplosionParticle(explosionPos, size, maxAge, gameTime);
            float rotation = (float)randomizer.Next(10);
            Matrix mat = Matrix.CreateTranslation(-explosionTexture.Width / 2, -explosionTexture.Height / 2, 0) * Matrix.CreateRotationZ(rotation) * Matrix.CreateScale(size / (float)explosionTexture.Width * 2.0f) * Matrix.CreateTranslation(explosionPos.X, explosionPos.Y, 0);
            //original last paramater Matrix.CreateTranslation(explosionPos.X, explosionPos.Y, 0); 
            AddCrater(explosionColorArray, mat);

            for (int i = 0; i < players.Length; i++)
                players[i].Position.Y = terrainContour[(int)players[i].Position.X];
            FlattenTerrainBelowPlayers();
            CreateForeground();

        }
        private void AddExplosionParticle(Vector2 explosionPos, float explosionSize, float maxAge, GameTime gameTime)
        {
            ParticleData particle = new ParticleData();

            particle.OrginalPosition = explosionPos;
            particle.Position = particle.OrginalPosition;

            particle.BirthTime = (float)gameTime.TotalGameTime.TotalMilliseconds;
            particle.MaxAge = maxAge;
            particle.Scaling = 0.25f;
            particle.ModColor = Color.Red;

            float particleDistance = (float)randomizer.NextDouble() * explosionSize;
            Vector2 displacement = new Vector2(particleDistance, 0);
            float angle = MathHelper.ToRadians(randomizer.Next(360));
            displacement = Vector2.Transform(displacement, Matrix.CreateRotationZ(angle));

            particle.Direction = displacement * 2.0f;
            particle.Accelaration = -particle.Direction;
            particleList.Add(particle);
        }

        private void DrawExplosion()
        {
            for (int i = 0; i < particleList.Count; i++)
            {
                ParticleData particle = particleList[i];
                spriteBatch.Draw(explosionTexture, particle.Position, null, particle.ModColor, i, new Vector2(explosionTexture.Width / 2, explosionTexture.Height / 2), particle.Scaling, SpriteEffects.None, 1);
            }
        }

        private void UpdateParticles(GameTime gameTime)
        {
            float now = (float)gameTime.TotalGameTime.TotalMilliseconds;
            for (int i = particleList.Count - 1; i >= 0; i--)
            {
                ParticleData particle = particleList[i];
                float timeAlive = now - particle.BirthTime;

                if (timeAlive > particle.MaxAge)
                {
                    particleList.RemoveAt(i);
                }
                else
                {
                    float relAge = timeAlive / particle.MaxAge;
                    particle.Position = 0.5f * particle.Accelaration * relAge * relAge + particle.Direction * relAge + particle.OrginalPosition;

                    float invAge = 1.0f - relAge;
                    particle.ModColor = new Color(new Vector4(invAge, invAge, invAge, invAge));

                    Vector2 positionFromCenter = particle.Position - particle.OrginalPosition;
                    float distance = positionFromCenter.Length();
                    particle.Scaling = (50.0f + distance) / 200.0f;

                    particleList[i] = particle;

                }
            }
        }

private void sinput(GameTime gameTime)
{
    float distance = 0f;
    touched = false;

    TouchCollection touches = TouchPanel.GetState();

    foreach (var touch in touches)
    {
        touchRectangle.X = (int)touch.Position.X - 15; // -15, because the finger is 30x30 pixel
        touchRectangle.Y = (int)touch.Position.Y - 15;
        
        //finger touched on the sliders background
        if (touchRectangle.Intersects(sliderBackgroundRectangle))
        {
            //Distance between finger and slider button
            distance = posSlider.X - (touchRectangle.X);

            //move the button to position of the finger 
            if (distance < -5f)
            {
                posSlider.X += 5; //posSlider.X += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.40f;
            }
            if (distance > 5f)
            {
               posSlider.X -=5;  //posSlider.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.40f;
            }

            if (posSlider.X <= posBackground.X)
            {
                posSlider.X = posBackground.X;
            }
            if (posSlider.X >= posBackground.X + sliderBackground.Width - slider.Width)
            {
                posSlider.X = posBackground.X + sliderBackground.Width - slider.Width;
            }

            sliderRectangle.X = (int)posSlider.X;

            //player touched the slider
            touched = true;
        }
    }

    if (touched == false)
    {
       
    //    //move back to the middle of the slider
    //    distance = posSlider.X - (posBackground.X + (sliderBackground.Width
    //    - slider.Width) / 2);
    //    if (distance < -5f)
    //    {
    //        posSlider.X += 5; //posSlider.X += (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.40f;
    //    }
    //    else if (distance > 5f)
    //    {
    //        posSlider.X -= 5; //posSlider.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.40f;
    //    }
    //    else
    //    {
    //        posSlider.X = posBackground.X + (sliderBackground.Width - slider.Width) / 2;
    //    }

    //    sliderRectangle.X = (int)posSlider.X;
    }

    sval = (posSlider.X - posBackground.X) / divider;
    //posSlider.X = posBackground.X + (sliderBackground.Width / 2);
}

    }

}