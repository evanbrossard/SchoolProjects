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
    class Button
    {
        private Vector2 pos;
        private String text;
        private Texture2D image;
        private Color color;
        private SpriteFont font;
        private Rectangle backgroundRec;
        private int cost;
        public Button(Vector2 pos, String text, Texture2D image, SpriteFont font, int baseCost)
        {
            this.pos = pos;
            this.text = text;
            this.image = image;
            this.font = font;
            this.cost = baseCost;
            backgroundRec = new Rectangle((int)pos.X, (int)pos.Y, (int)font.MeasureString(text).X, (int)font.MeasureString(text).Y);
        }
        public bool isMouseColliding(float x, float y, ButtonState state, ButtonState oldState)
        {
            if (x >= pos.X && (pos.X + image.Width) >= x
                && y >= pos.Y && (pos.Y + image.Height) >= y && state == ButtonState.Pressed && oldState == ButtonState.Released)
            {
                color = Color.Blue;
                return true;
            }
            else
            {
                color = Color.Black;
                return false;
            }
        }
        public void draw(SpriteBatch spriteBatch, Player player)
        {
            font.MeasureString(text);
            spriteBatch.Draw(image, backgroundRec, color);
            if (text == "Upgrade Laser")
            {
                spriteBatch.DrawString(font, text + " $" + cost + "   Damage: " + player.getLaser(), pos, Color.White);
            }
            else if (text == "Upgrade Ship")
            {
                spriteBatch.DrawString(font, text + " $" + cost + "   Health: " + player.getHealth(), pos, Color.White);
            }
            else if (text == "Upgrade Multiplier")
            {
                spriteBatch.DrawString(font, text + " $" + cost + "   x" + player.getMultiplier(), pos, Color.White);
            }
            else
            {
                spriteBatch.DrawString(font, text + " $" + cost, pos, Color.White);
            }
        }
        public void pressed(Player player)
        {
            if (text == "Upgrade Laser")
            {
                player.upgradeLaser();
                player.removeGold(cost);
                cost *= 2;
            }
            else if (text == "Upgrade Ship")
            {
                player.upgradeShip();
                player.removeGold(cost);
                cost *= 2;
            }
            else if (text == "Upgrade Multiplier")
            {
                player.upgradeMultiplier();
                player.removeGold(cost);
                cost *= 2;
            }
            else
            {
            }
        }
        public int getCost()
        {
            return cost;
        }
    }
}
