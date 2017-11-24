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

namespace VikingSimulator
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        InputHelper inputHelper;
        List<Sprite> powerups = new List<Sprite>();
        int powerup;
        Random powerupNumber = new Random();


       

        Sprite redbeard;
        Sprite greybeard;


        enum winner
        {
            noWinner,
            redbeard,
            greybeard,
        }

        enum status
        {
            running,
            menu,
            winner,
        }

        enum menuSelected
        {
            play,
            restart,
            exit,

        }

        winner Winner = winner.noWinner;
        status Status = status.menu;
        menuSelected MenuSelected = menuSelected.play;

        public Game1()
        {
            //Enable mouse
            //this.IsMouseVisible = true;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.IsFullScreen = true;

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
            inputHelper = new InputHelper();
            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        // This is a texture we can render.
        Texture2D redbeardEast1;
        Texture2D greybeardEast1;
        Texture2D redbeardWest1;
        Texture2D greybeardWest1;
        Texture2D redWinner;
        Texture2D greyWinner;


        Texture2D hurt;
        Texture2D healthBar;

        Texture2D cowTexture;
        Texture2D mjod;
        Texture2D kottbulle;

        Texture2D menuPlayText;
        Texture2D menuExitText;
        Texture2D menuRestartText;
        Texture2D menuLogoImage;

        public SpriteFont damageFont;

        // Set the coordinates to draw the sprite at.

        // Store some information about the sprite's motion.

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            redbeardEast1 = Content.Load<Texture2D>("redbeardEast1");
            greybeardEast1 = Content.Load<Texture2D>("greybeardEast1");
            redbeardWest1 = Content.Load<Texture2D>("redbeardWest1");
            greybeardWest1 = Content.Load<Texture2D>("greybeardWest1");

            hurt = Content.Load<Texture2D>("Hurt");
            healthBar = Content.Load<Texture2D>("healthBar");
            
            cowTexture = Content.Load<Texture2D>("cow");
            mjod = Content.Load<Texture2D>("mjod");
            kottbulle = Content.Load<Texture2D>("kottbulle");

            redbeard = new Sprite(redbeardWest1, 100, 300);
            greybeard = new Sprite(greybeardEast1, 1052, 300);

            greyWinner = Content.Load<Texture2D>("greyBeardWon");
            redWinner = Content.Load<Texture2D>("redBeardWon");

            menuExitText = Content.Load<Texture2D>("menuExitText");
            menuPlayText = Content.Load<Texture2D>("menuPlayText");
            menuRestartText = Content.Load<Texture2D>("menuRestartText");
            menuLogoImage = Content.Load<Texture2D>("menuLogoImage");

            damageFont = Content.Load<SpriteFont>("Font");
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
            inputHelper.Update();

            //Mouse control
            if (Status == status.menu) { this.IsMouseVisible = true; }
            if (Status == status.running || Status == status.winner) { this.IsMouseVisible = false; }

            if (Status == status.running)
            {
                //THE ACTUAL GAME UPDATE STUFF

                //Redbeard move
                if (inputHelper.IsCurPress(Keys.W)) { redbeard.Move(dir.North); }
                if (inputHelper.IsCurPress(Keys.A)) { redbeard.Move(dir.West); redbeard.Retexture(redbeardWest1); }
                if (inputHelper.IsCurPress(Keys.S)) { redbeard.Move(dir.South); }
                if (inputHelper.IsCurPress(Keys.D)) { redbeard.Move(dir.East); redbeard.Retexture(redbeardEast1); }

                //Greybeard Move
                if (inputHelper.IsCurPress(Keys.Up)) { greybeard.Move(dir.North); }
                if (inputHelper.IsCurPress(Keys.Left)) { greybeard.Move(dir.West); greybeard.Retexture(greybeardWest1); }
                if (inputHelper.IsCurPress(Keys.Down)) { greybeard.Move(dir.South); }
                if (inputHelper.IsCurPress(Keys.Right)) { greybeard.Move(dir.East); greybeard.Retexture(greybeardEast1); }

                //Attack Buttons
                //redbeard attack
                if (inputHelper.IsNewPress(Keys.V) && redbeard.hitbox.Intersects(greybeard.hitbox))
                {
                    greybeard.Hurt(redbeard.strenght, hurt);
                    redbeard.DrawAttack(); 
                }

                //greybeard attack
                if (inputHelper.IsNewPress(Keys.NumPad2) && greybeard.hitbox.Intersects(redbeard.hitbox))
                {
                    redbeard.Hurt(greybeard.strenght, hurt);
                    greybeard.DrawAttack(); 
                }

                //end Attcks

                //Display winner
                if (redbeard.health < 0) { Winner = winner.greybeard; Status = status.winner; }
                if (greybeard.health < 0) { Winner = winner.redbeard; Status = status.winner; }

                // add powerups
                if (powerups == null || powerups.Count < 10)
                {
                    powerup = powerupNumber.Next(0, 5);

                    if (powerup == 0)
                    {
                        powerups.Add(new Sprite(cowTexture, type.cow));                          
                    }
                    if (powerup == 1)
                    {
                        powerups.Add(new Sprite(mjod, type.mjod));
                    }
                    if (powerup == 2)
                    {
                        powerups.Add(new Sprite(kottbulle, type.kottbulle));
                    }
                }

                //pick up powerups
                for (int i = 0; i < powerups.Count; ++i)
                {
                    //greybeard
                    if (inputHelper.IsNewPress(Keys.NumPad2) && greybeard.hitbox.Intersects(powerups[i].hitbox))
                    {
                        //check which powerup and apply effect
                        if (powerups[i].type == type.mjod) 
                        {
                            greybeard.strenght += 60;
                        }
                        if (powerups[i].type == type.cow)
                        {
                            greybeard.speed += 2;
                        }
                        if (powerups[i].type == type.kottbulle)
                        {
                            greybeard.health += 60;
                        }
                        //remove powerup
                        powerups.Remove(powerups[i]); i = 0;               
                    }

                    //redbeard
                    if (inputHelper.IsNewPress(Keys.V) && redbeard.hitbox.Intersects(powerups[i].hitbox))
                    {
                        //check which powerup and apply effect
                        if (powerups[i].type == type.mjod)
                        {
                            redbeard.strenght += 60;
                        }
                        if (powerups[i].type == type.cow)
                        {
                            redbeard.speed += 2;
                        }
                        if (powerups[i].type == type.kottbulle)
                        {
                            redbeard.strenght += 60;
                        }

                        //remove powerup
                        powerups.Remove(powerups[i]); i = 0;    
                    }
                }//end powerups

                // Allows the game to exit
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    this.Exit();

                if (inputHelper.IsNewPress(Keys.Escape)) { Status = status.menu; }
            } //End if running

            //menu
            if (Status == status.menu)
            {

                if (MenuSelected == menuSelected.play)
                {
                    if (inputHelper.IsNewPress(Keys.Enter))
                    {
                        Status = status.running;
                    }
                    if (inputHelper.IsNewPress(Keys.Down))
                    {
                        MenuSelected += 1;
                    }

                    if (inputHelper.IsNewPress(Keys.Up))
                    {
                        MenuSelected += 2;
                    }

                }

                else if (MenuSelected == menuSelected.restart)
                {
                    if (inputHelper.IsNewPress(Keys.Enter))
                    {
                        Status = status.running; greybeard.Reset(1052, 300); redbeard.Reset(100, 300); powerups.Clear();
                    }
                    if (inputHelper.IsNewPress(Keys.Up))
                    {
                        MenuSelected -= 1;
                    }
                    if (inputHelper.IsNewPress(Keys.Down))
                    {
                        MenuSelected += 1;
                    }
                }

                else if (MenuSelected == menuSelected.exit)
                {
                    if (inputHelper.IsNewPress(Keys.Enter))
                    {
                        this.Exit();
                    }
                    if (inputHelper.IsNewPress(Keys.Up))
                    {
                        MenuSelected -= 1;
                    }
                    if (inputHelper.IsNewPress(Keys.Down))
                    {
                        MenuSelected -= 2;
                    }

                }

                //Musgrejor
                MouseState mouseState = Mouse.GetState();
                if (mouseState.LeftButton == ButtonState.Pressed && new Rectangle(mouseState.X, mouseState.Y, 10, 10).Intersects(new Rectangle(490, 300, 300, 100))) { Status = status.running; }
                if (mouseState.LeftButton == ButtonState.Pressed && new Rectangle(mouseState.X, mouseState.Y, 10, 10).Intersects(new Rectangle(490, 400, 300, 100))) { Status = status.running; greybeard.Reset(1052, 300); redbeard.Reset(100, 300); greybeard.Retexture(greybeardWest1); redbeard.Retexture(redbeardEast1); }
                if (mouseState.LeftButton == ButtonState.Pressed && new Rectangle(mouseState.X, mouseState.Y, 10, 10).Intersects(new Rectangle(490, 500, 300, 100))) { this.Exit(); }

            } //End menu

            if (Status == status.winner)
            {
                if (inputHelper.IsNewPress(Keys.Enter))
                {
                    Status = status.menu; greybeard.Reset(1052, 300); redbeard.Reset(100, 300); powerups.Clear();
                }
            }

            // TODO: Add your update logic here
            inputHelper.Update();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // Draw the sprite.
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            ;
            if (Status == status.running)
            {
                //draw healthbar. The "0.2f" is the multiplier of how much pixels each life should take up on screen. 
                spriteBatch.Draw(healthBar, new Rectangle(10, 670, Convert.ToInt32(redbeard.health * 0.2f), 40), Color.White);
                spriteBatch.Draw(healthBar, new Rectangle((1270 - Convert.ToInt32(greybeard.health * 0.2f)), 670, Convert.ToInt32(greybeard.health * 0.2), 40), Color.White);

                redbeard.Draw(spriteBatch, damageFont);
                greybeard.Draw(spriteBatch, damageFont);
                
                //draw powerups
                foreach (Sprite currentSprite in powerups)
                {
                    currentSprite.Draw(spriteBatch, damageFont);
                    if (currentSprite.direction == 0)
                    { currentSprite.MovePowerups(dir.North); }
                    if (currentSprite.direction == 1 )
                    { currentSprite.MovePowerups(dir.South); }
                    if (currentSprite.direction == 2)
                    { currentSprite.MovePowerups(dir.West); }
                    if (currentSprite.direction == 3)
                    { currentSprite.MovePowerups(dir.East); }
                }

                //draw players
                redbeard.Draw(spriteBatch, damageFont);
                greybeard.Draw(spriteBatch, damageFont);
            }// End status running

            if (Status == status.menu)
            {
                spriteBatch.Draw(menuLogoImage, new Rectangle(340, 0, 600, 300), Color.White);
                spriteBatch.Draw(menuPlayText, new Rectangle(490, 300, 300, 100), Color.White);
                spriteBatch.Draw(menuRestartText, new Rectangle(490, 400, 300, 100), Color.White);
                spriteBatch.Draw(menuExitText, new Rectangle(490, 500, 300, 100), Color.White);

                if (MenuSelected == menuSelected.play) { spriteBatch.Draw(redbeardEast1, new Rectangle(440, 325, 50, 50), Color.White); }
                if (MenuSelected == menuSelected.restart) { spriteBatch.Draw(redbeardEast1, new Rectangle(440, 425, 50, 50), Color.White); }
                if (MenuSelected == menuSelected.exit) { spriteBatch.Draw(redbeardEast1, new Rectangle(440, 525, 50, 50), Color.White); }

                }//End status menu

            if (Status == status.winner)
            {
                if (Winner == winner.redbeard) { spriteBatch.Draw(redWinner, new Rectangle(0, 0, 1280, 720), Color.White); }
                if (Winner == winner.greybeard) { spriteBatch.Draw(greyWinner, new Rectangle(0, 0, 1280, 720), Color.White); }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
