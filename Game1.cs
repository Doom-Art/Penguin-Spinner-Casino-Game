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
        Rectangle buttonRect;
        MouseState mouse, prevMouse;
        Screen screen;
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
            rand = new();
            textures = new();
            screen = Screen.menu;
            profit = 0;
            buttonRect = new Rectangle(0,0,1000,1000);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            kahootMusic = Content.Load<SoundEffect>("lobby-classic-game").CreateInstance();
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            prevMouse = mouse;
            mouse = Mouse.GetState();
            kahootMusic.Play();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            switch (screen)
            {
                case Screen.menu:
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released && buttonRect.Contains(mouse.X, mouse.Y))
                    {
                        spun = rand.Next(1, 51);
                        bet = rand.Next(1,26);
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}