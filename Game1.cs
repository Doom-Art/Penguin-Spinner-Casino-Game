﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Penguin_Spinner_Casino_Game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Random rand;
        private int spin, bet, roll1, roll2, coinFlip, payout, profit;
        SoundEffectInstance kahootMusic;
        List<Texture2D> textures;
        SpriteFont font;
        Button spinButton, rollButton, flipButton;
        MouseState mouse, prevMouse;
        List<Records> records;
        float rotation, rotationIncrease, timer;
        Vector2 origin;
        Screen screen;
        bool spun, rolled, flipped;
        Button[] betButtons;
        Rectangle[] diceSourceRects; 
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
            _graphics.PreferredBackBufferWidth = 1900;
            _graphics.PreferredBackBufferHeight = 999;
            _graphics.ApplyChanges();
            rand = new();
            records = new();
            screen = Screen.menu;
            profit = 0;
            bet = 5;
            rotationIncrease = 0.12f;
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
                Content.Load<Texture2D>("arrow"),
                Content.Load<Texture2D>("Jackpot"),
                Content.Load<Texture2D>("SadPrivate"),
                Content.Load<Texture2D>("PenguinDice"),
                Content.Load<Texture2D>("Dice"),
                Content.Load<Texture2D>("penguinCoin"),
            };
            diceSourceRects = new Rectangle[6]
            {
                new(0,0,textures[5].Width/2,textures[5].Height/3),
                new(textures[5].Width/2,0,textures[5].Width/2,textures[5].Height/3),
                new(0,textures[5].Height/3,textures[5].Width/2,textures[5].Height/3),
                new(textures[5].Width/2,textures[5].Height/3,textures[5].Width/2,textures[5].Height/3),
                new(0,(2*textures[5].Height/3),textures[5].Width/2,textures[5].Height/3),
                new(textures[5].Width/2,(2*textures[5].Height/3),textures[5].Width/2,textures[5].Height/3),
            };
            font = Content.Load<SpriteFont>("Font");
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
            rollButton = new(rectTex, font, "Roll", new Rectangle(880, 800, 200, 110));
            flipButton = new(rectTex, font, "Flip", new Rectangle(880, 800, 200, 110));
            betButtons[4].SwitchColor();
        }

        protected override void Update(GameTime gameTime)
        {
            prevMouse = mouse;
            mouse = Mouse.GetState();
            kahootMusic.Play();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                StreamWriter writer = new($"SaveFile#{rand.Next()}.txt");
                foreach (Records r in records)
                {
                    writer.WriteLine(r);
                }
                writer.WriteLine($"\nOur Profit: {profit}");
                writer.Close();
                Exit();
            }
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            switch (screen)
            {
                case Screen.menu:
                    rotation += rotationIncrease;
                    if ((mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released) && !spun)
                    {
                        if (spinButton.Rectangle.Contains(mouse.X, mouse.Y))
                        {
                            spin = rand.Next(1, 51);
                            spun = true;
                            profit += bet;
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
                    else if (rotationIncrease != 0 && spun)
                    {
                        rotationIncrease = 0.07f;
                        var spinnerPos = rotation % 6.28;
                        timer = 0;
                        switch (spin)
                        {
                            case <= 5:
                                if (spinnerPos > 6 && spinnerPos < 6.28)
                                {
                                    rotationIncrease = 0;
                                }
                                break;
                            case <= 10:
                                if (spinnerPos > 2.9 && spinnerPos < 3.14)
                                {
                                    rotationIncrease = 0;
                                }
                                break;
                            case <= 30:
                                if (spinnerPos > 4.3 && spinnerPos < 4.5)
                                {
                                    rotationIncrease = 0;
                                }
                                break;
                            case <= 50:
                                if (spinnerPos > 1.5 && spinnerPos < 1.7)
                                {
                                    rotationIncrease = 0;
                                }
                                break;
                        }
                    }
                    else if (rotationIncrease == 0 && timer > 1)
                    {
                        switch (spin)
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
                        rotationIncrease = 0.12f;
                        rotation = 0;
                        spun = false;
                        timer = 0;
                    }
                    break;
                case Screen.jackpot:
                    payout = bet * 3;
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
                    {
                        screen = Screen.menu;
                        profit -= payout;
                        records.Add(new Records(bet, payout));
                    }
                    break;
                case Screen.lose:
                    payout = 0;
                    if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
                    {
                        screen = Screen.menu;
                        profit -= payout;
                        records.Add(new Records(bet, payout));
                    }
                    break;
                case Screen.dice:
                    if (!rolled && (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released))
                    {
                        if (rollButton.Rectangle.Contains(mouse.X, mouse.Y))
                        {
                            roll1 = rand.Next(1, 7);
                            roll2 = rand.Next(1, 7);
                            rolled = true;
                            timer = 0;
                            if (roll1 == roll2)
                                payout = bet * 2;
                            else if (roll1 + roll2 == 7)
                                payout = (int)(1.5 * bet);
                            else
                                payout = 0;
                            profit -= payout;
                        }
                    }
                    else if (rolled && mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
                    {
                        screen = Screen.menu;
                        records.Add(new Records(bet, payout, true, roll1, roll2));
                        rolled = false;
                    }
                    break;
                case Screen.coin:
                    if (!flipped && (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released))
                    {
                        if (flipButton.Rectangle.Contains(mouse.X, mouse.Y))
                        {
                            coinFlip = rand.Next(0, 2);
                            if (coinFlip == 0)
                                payout = bet * 2;
                            else
                                payout = 0;
                            flipped = true;
                        }
                    }
                    else if (flipped && mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
                    {
                        screen = Screen.menu;
                        profit -= payout;
                        records.Add(new Records(bet, payout, true, coinFlip == 0));
                        flipped = false;
                    }
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Purple);

            _spriteBatch.Begin();
            if (screen == Screen.menu)
            {
                _spriteBatch.Draw(textures[0], new Rectangle(400, 500, 800, 800), null, Color.White, rotation, origin, SpriteEffects.None, 0f);
                _spriteBatch.Draw(textures[1], new Rectangle(400, 20, 40, 100), Color.White);
                foreach (Button b in betButtons)
                {
                    b.Draw(_spriteBatch);
                }
                spinButton.Draw(_spriteBatch);
            }
            else if (screen == Screen.jackpot)
            {
                _spriteBatch.Draw(textures[2], new Rectangle(600, 220, 700, 700), Color.White);
                _spriteBatch.DrawString(font, $"You Won {payout} from the Jackpot\n                 Congrats!!", new Vector2(390, 30), Color.Black);
            }
            else if (screen == Screen.lose)
            {
                _spriteBatch.Draw(textures[3], new Rectangle(600,220,700,700), Color.White);
                _spriteBatch.DrawString(font, "You Lost from the instant Lose \n         better luck next time", new Vector2(390,30), Color.Black);
            }
            else if (screen == Screen.dice)
            {
                if (!rolled)
                {
                    _spriteBatch.Draw(textures[4], new Rectangle(600, 80, 700, 700), Color.White);
                    rollButton.Draw(_spriteBatch);
                }
                else
                {
                    _spriteBatch.Draw(textures[5], new Rectangle(500, 300, 350, 350), diceSourceRects[roll1 - 1], Color.White);
                    _spriteBatch.Draw(textures[5], new Rectangle(1000, 300, 350, 350), diceSourceRects[roll2 - 1], Color.White);
                    if (payout != 0)
                    {
                        _spriteBatch.DrawString(font, $"You Won {payout} from the Dice", new Vector2(440, 50), Color.Black);
                    }
                    else
                    {
                        _spriteBatch.DrawString(font, "You Lost from the Dice\n  better luck next time", new Vector2(480, 30), Color.Black);
                    }
                }
            }
            else if (screen == Screen.coin)
            {
                if (!flipped)
                {
                    _spriteBatch.Draw(textures[6], new Rectangle(600, 80, 700, 700), Color.White);
                    flipButton.Draw(_spriteBatch);
                }
                else
                {
                    if (payout != 0)
                    {
                        _spriteBatch.DrawString(font, $"You Won {payout} from the Coin Flip", new Vector2(440, 30), Color.Black);
                        _spriteBatch.Draw(textures[2], new Rectangle(600, 220, 700, 700), Color.White);
                    }
                    else
                    {
                        _spriteBatch.Draw(textures[3], new Rectangle(600, 220, 700, 700), Color.White);
                        _spriteBatch.DrawString(font, "You Lost from the Coin Flip\n  better luck next time", new Vector2(480, 30), Color.Black);
                    }
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}