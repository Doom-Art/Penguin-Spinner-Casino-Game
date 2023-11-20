using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Penguin_Spinner_Casino_Game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Random rand;
        private int spun, bet, roll1, roll2, coinFlip, payout, profit;
        SoundEffectInstance kahootMusic;
        List<Texture2D> textures;
        Button spinButton;
        MouseState mouse, prevMouse;
        float rotation;
        Vector2 origin;
        Screen screen;
        int counter;
        Button[] betButtons;
        enum Screen
        {
            menu, 
            lose,
            dice,
            coin,
            jackpot
        }
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1900;
            _graphics.PreferredBackBufferHeight = 999;
            _graphics.ApplyChanges();
            rand = new();
            screen = Screen.menu;
            profit = 0;
            bet = 5;
            base.Initialize();
            origin = new Vector2(textures[0].Width/2, textures[0].Height/2);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            kahootMusic = Content.Load<SoundEffect>("lobby-classic-game").CreateInstance();
            textures = new() 
            {
                Content.Load<Texture2D>("Spinner"),
                Content.Load<Texture2D>("Jackpot"),
                Content.Load<Texture2D>("SadPrivate"),
                Content.Load<Texture2D>("PenguinDice"),
            };
            SpriteFont font = Content.Load<SpriteFont>("Font");
            Texture2D rectTex= Content.Load<Texture2D>("Rectangle");

            int counter2 = 1;
            betButtons = new Button[25];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j<5; j++)
                {
                    betButtons[counter2 - 1] = new Button(rectTex, font, counter2, new Rectangle(1100 + (j * 140), 70 + (i * 140), 120, 120));
                    counter2++;
                }
            }
            spinButton = new(rectTex, font, "Spin", new Rectangle(1350, 800, 200, 110));
            betButtons[4].SwitchColor();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            prevMouse = mouse;
            mouse = Mouse.GetState();
            kahootMusic.Play();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            rotation += 0.12f;
            switch (screen)
            {
                case Screen.menu:
                    if ((mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released))
                    {
                        if (spinButton.Rectangle.Contains(mouse.X, mouse.Y))
                        {
                            spun = rand.Next(1, 51);
                            profit -= bet;
                            switch (spun)
                            {
                                case <= 5:
                                    screen = Screen.jackpot;
                                    break;
                                case <= 10:
                                    screen = Screen.lose;
                                    break;
                                case <= 30:
                                    screen = Screen.dice;
                                    break;
                                case <= 50:
                                    screen = Screen.coin;
                                    break;
                            }
                        }
                        else
                        {
                            for (int i = 0; i< 25; i++)
                            {
                                if (betButtons[i].Rectangle.Contains(mouse.X, mouse.Y))
                                {
                                    if (i+1 != bet)
                                    {
                                        betButtons[bet - 1].SwitchColor(false);
                                        betButtons[i].SwitchColor(true);
                                        bet = i + 1;
                                    }
                                }
                            }
                        }
                    }
                    break;
                case Screen.lose:
                    payout = 0;
                    screen = Screen.menu;
                    profit += payout;
                    this.Window.Title = $"the Payout is: {payout}, bet is {bet} in Lose, total Profit {profit}";
                    break;
                case Screen.dice:
                    roll1 = rand.Next(1, 7);
                    roll2 = rand.Next(1, 7);
                    if (roll1 == roll2)
                        payout = bet * 2;
                    else if (roll1 + roll2 == 7)
                        payout = (int)(1.5 * bet);
                    else
                        payout = 0;
                    screen = Screen.menu;
                    profit += payout;
                    this.Window.Title = $"the Payout is: {payout}, bet is {bet} in Dice, total Profit {profit}";
                    break;
                case Screen.coin:
                    coinFlip = rand.Next(0, 2);
                    if (coinFlip == 0)
                        payout = bet * 2;
                    else
                        payout = 0;
                    screen = Screen.menu;
                    profit += payout;
                    this.Window.Title = $"the Payout is: {payout}, bet is {bet} in Coin, total Profit {profit}";
                    break;
                case Screen.jackpot:
                    payout = bet * 3;
                    screen = Screen.menu;
                    profit += payout;
                    this.Window.Title = $"the Payout is: {payout}, bet is {bet} in Jackpot, total Profit {profit}";
                    break;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Purple);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw(textures[0], new Rectangle(400, 500, 800, 800), null, Color.White, rotation, origin, SpriteEffects.None, 0f);
            foreach (Button b in betButtons)
            {
                b.Draw(_spriteBatch);
            }
            spinButton.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}