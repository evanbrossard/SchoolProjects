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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Space_Frenzy
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum GameState
        {
            titleScreen, level1, level2, level3, level4, endBoss, endScreen, options, upgrades
        }
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Background lvl1background;
        MouseState mouse, oldMouse;
        List<Button> buttonList = new List<Button>();
        GameState state = GameState.titleScreen;
        EnemyManager enemyManager = new EnemyManager();
        Texture2D shipTexture, enemyTexture, explosPic, laserTexture, level1Pic, level2Pic, level3Pic, level4Pic, optionsPic, blankPic, upgradePic, backPic;
        Vector2 level1 = new Vector2(0, 375);
        Vector2 level2 = new Vector2(0, 125);
        Vector2 level3 = new Vector2(125, 0);
        Vector2 level4 = new Vector2(450, 0);
        Vector2 upgrades = new Vector2(600, 500);
        Rectangle backRec = new Rectangle(450, 400, 104, 43);
        int timer = 0;
        int notifTimer = 0;
        Player ship;
        bool drawFundsNotification = false;
        SpriteFont titleFont, buttonFont, goldFont;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
            this.IsFixedTimeStep = true;
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
            goldFont = Content.Load<SpriteFont>("goldFont");
            shipTexture = Content.Load<Texture2D>("ship");
            laserTexture = Content.Load<Texture2D>("redLaserRay");
            ship = new Player(spriteBatch, shipTexture, laserTexture);
            titleFont = Content.Load<SpriteFont>("titleFont");
            level1Pic = Content.Load<Texture2D>("level1");
            level2Pic = Content.Load<Texture2D>("level2");
            level3Pic = Content.Load<Texture2D>("level3");
            level4Pic = Content.Load<Texture2D>("level4");
            backPic = Content.Load<Texture2D>("BackButtton");
            optionsPic = Content.Load<Texture2D>("options");
            enemyTexture = Content.Load<Texture2D>("1027217_med");
            buttonFont = Content.Load<SpriteFont>("buttonFont");
            blankPic = Content.Load<Texture2D>("blank");
            upgradePic = Content.Load<Texture2D>("upgrades");
            explosPic = Content.Load<Texture2D>("explosion");
            lvl1background = new Background(Content.Load<Texture2D>("backgroundlvl1"));
            buttonList.Add(new Button(new Vector2(150, 150), "Upgrade Ship", blankPic, buttonFont, 10));
            buttonList.Add(new Button(new Vector2(150, 200), "Upgrade Laser", blankPic, buttonFont, 10));
            buttonList.Add(new Button(new Vector2(150, 250), "Upgrade Multiplier", blankPic, buttonFont, 10));
            // TODO: use this.Content to load your game content here
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
            switch (state)
            {
                case GameState.titleScreen:
                    ship.Move(true, ref enemyManager);
                    checkposforLevel();
                    break;
                case GameState.level1:
                    updateEnemies();
                    ship.Move(false, ref enemyManager);
                    lvl1background.update();
                    break;
                case GameState.level2:
                    updateEnemies();
                    ship.Move(false, ref enemyManager);
                    break;
                case GameState.level3:
                    updateEnemies();
                    ship.Move(false, ref enemyManager);
                    break;
                case GameState.level4:
                    updateEnemies();
                    ship.Move(false, ref enemyManager);
                    break;
                case GameState.endBoss:
                    updateBoss();
                    ship.Move(false, ref enemyManager);
                    break;
                case GameState.options:
                    updateOptionScreen();
                    break;
                case GameState.upgrades:
                    updateUpgradeScreen();
                    break;
            }
            timer++;
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            switch (state)
            {
                case GameState.titleScreen:
                    drawTitleScreen();
                    ship.Draw(goldFont);
                    break;
                case GameState.level1:
                    lvl1background.draw(spriteBatch);
                    ship.Draw(goldFont);
                    drawEnemies();
                    break;
                case GameState.level2:
                    drawEnemies();
                    ship.Draw(goldFont);
                    break;
                case GameState.level3:
                    drawEnemies();
                    ship.Draw(goldFont);
                    break;
                case GameState.level4:
                    drawEnemies();
                    ship.Draw(goldFont);
                    break;
                case GameState.endBoss:
                    updateBoss();
                    ship.Draw(goldFont);
                    break;
                case GameState.options:
                    drawOptionScreen();
                    break;
                case GameState.upgrades:
                    drawUpgradeScreen();
                    break;

            }
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        private void checkposforLevel()
        {
            if (ship.intersects(level1Pic, level1))
            {
                state = GameState.level1;
                timer = 0;
            }
            if (ship.intersects(level2Pic, level2))
            {
                state = GameState.level2;
                timer = 0;
            }
            if (ship.intersects(level3Pic, level3))            
            {
                state = GameState.level3;
                timer = 0;
            }
            if (ship.intersects(level4Pic, level4))
            {
                state = GameState.level4;
                timer = 0;
            }
            if (ship.intersects(upgradePic, upgrades))
            {
                state = GameState.upgrades;
                this.IsMouseVisible = true;
                timer = 0;
            }
        }
        //Update Methods
        private void updateEnemies()
        {
            if (state == GameState.level1 && (timer % 90 == 0 || timer <= 1) && timer <= 360)
            {
                enemyManager.add(1, 1, 1, 10, enemyTexture, explosPic, new Rectangle(150, 0, 40, 40), 0);
                enemyManager.add(1, 1, 1, 10, enemyTexture, explosPic, new Rectangle(450, 0, 40, 40), 0);
            }
            if (state == GameState.level2 && (timer % 60 == 0 || timer <= 1) && timer <= 360)
            {
                enemyManager.add(3, 1, 2, 10, enemyTexture, explosPic, new Rectangle(20, 0, 40, 40), -45);
                enemyManager.add(2, 1, 2, 10, enemyTexture, explosPic, new Rectangle(780, 0, 40, 40), 45);
            }
            if (state == GameState.level3 && (timer % 60 == 0 || timer <= 1) && timer <= 360)
            {
                enemyManager.add(4, 1, 2, 10, enemyTexture, explosPic, new Rectangle(0, 60, 40, 40), -45);
                enemyManager.add(4, 1, 2, 10, enemyTexture, explosPic, new Rectangle(600, 60, 40, 40), 45);
            }
            enemyManager.update(ship);
            if (enemyManager.isLevelOver(timer))
            {
                state = GameState.titleScreen;
                timer = 0;
            }
        }
        private void updateUpgradeScreen()
        {
            mouse = Mouse.GetState();
            foreach (Button i in buttonList)
            {
                if (i.isMouseColliding(mouse.X, mouse.Y, mouse.LeftButton, oldMouse.LeftButton))
                {
                    if (ship.hasEnoughGold(i.getCost()))
                        i.pressed(ship);
                    else
                    {
                        drawFundsNotification = true;
                        notifTimer = 150;
                    }
                }

            }
            oldMouse = mouse;
        }
        private void updateBoss()
        {
            
        }
        private void updateOptionScreen()
        {
        }
        //Draw Methods
        private void drawEnemies()
        {
            enemyManager.draw(spriteBatch);
        }

        private void drawOptionScreen()
        {
        }
        private void drawUpgradeScreen()
        {
            foreach (Button i in buttonList)
            {
                i.draw(spriteBatch, ship);
                spriteBatch.Draw(backPic, backRec, Color.White);
                mouse = Mouse.GetState();
                if (mouse.X >= backRec.X && (backRec.X + backRec.Width) >= mouse.X
                && mouse.Y >= backRec.Y && (backRec.Y + backRec.Height) >= mouse.Y && mouse.LeftButton == ButtonState.Pressed)
                {
                    state = GameState.titleScreen;
                    ship.resetShip();
                }
            }
            if (drawFundsNotification)
            {
                spriteBatch.DrawString(buttonFont, "Insufficient Funds", new Vector2(330, 400), Color.Red);
                if (notifTimer == 0)
                    drawFundsNotification = false;
                else
                    notifTimer--;
            }
            ship.drawGold(buttonFont, new Vector2(200, 500));
        }
        private void drawTitleScreen()
        {
            spriteBatch.DrawString(titleFont, "Space Frenzy", new Vector2(250, 200), Color.Red);
            spriteBatch.Draw(level1Pic, level1, Color.White);
            spriteBatch.Draw(level2Pic, level2, Color.White);
            spriteBatch.Draw(level3Pic, level3, Color.White);
            spriteBatch.Draw(level4Pic, level4, Color.White);
            spriteBatch.Draw(upgradePic, upgrades, Color.White);
            
        }
        
    }
}
