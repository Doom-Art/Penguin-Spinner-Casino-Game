using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Penguin_Spinner_Casino_Game
{
    internal class Button
    {
        Texture2D _backgroundTex;
        int _number;
        string _text;
        Color _color;
        Rectangle _rect;
        SpriteFont _font;
        Vector2 _position;
        public Button(Texture2D backgroundTex, SpriteFont font, int number, Rectangle rect)
        {
            _backgroundTex = backgroundTex;
            _number = number;
            _color = Color.White;
            _rect = rect;
            _text = null;
            _position = new Vector2(_rect.X + 10, _rect.Y + 5);
            _font = font;
        }
        public Button(Texture2D backgroundTex, SpriteFont font, string text, Rectangle rect)
        {
            _backgroundTex = backgroundTex;
            _text = text;
            _color = Color.White;
            _rect = rect;
            _position = new Vector2(_rect.X + 10, _rect.Y + 5);
            _font = font;
        }
        public int Number
        {
            get { return _number; }
        }
        public Rectangle Rectangle { get { return _rect; } }
        public void SwitchColor()
        {
            if (Color.White == _color)
            {
                _color = Color.SkyBlue;
            }
            else
            {
                _color = Color.White;
            }
        }
        public void SwitchColor(bool secondColor)
        {
            if (secondColor)
            {
                _color = Color.SkyBlue;
            }
            else
            {
                _color = Color.White;
            }
        }
        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(_backgroundTex, _rect, _color);
            if (_text == null)
                sprite.DrawString(_font, _number.ToString(), _position, Color.Black);
            else
                sprite.DrawString(_font, _text, _position, Color.Black);

        }
    }
}
