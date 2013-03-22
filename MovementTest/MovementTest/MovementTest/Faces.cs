using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MovementTest
{
    class Faces
    {
        Texture2D pic;
        public Vector2 picPos = new Vector2(100, 100);
        Random rand,rand2;
        float picRot;
        int timer;
        bool gabe;
        Rectangle rec;
        public Faces(Texture2D pic, bool k)
        {
            this.pic = pic;
            if (k)
            {
                picPos = new Vector2(300, 300);
                rand = new Random(5);
                rand2 = new Random(2);
                gabe = true;
                rec = new Rectangle(300, 300, pic.Width, pic.Height);
            }
            else
            {
                rand = new Random();
                gabe = false;
            }

        }
        public void update(MouseState mouse)
        {
            timer++;
            Vector2 mousePos = new Vector2(mouse.X, mouse.Y);
            Vector2 difference = mousePos - picPos;
            picRot += (float)(rand.NextDouble()/4 - 0.125);
            if (mouse.LeftButton == ButtonState.Released)
            {
                //picRot = (float)Math.Atan2((double)difference.Y, (double)difference.X);
                picPos.X += (float)Math.Cos((double)picRot);
                picPos.Y += (float)Math.Sin((double)picRot);
            }
            if(picPos.X <= 0)
                picRot += (MathHelper.Pi);
            if (picPos.X >= 800)
                picRot += (MathHelper.Pi);
            if (picPos.Y <= 0)
                picRot += (MathHelper.Pi);
            if (picPos.Y >= 600)
                picRot += (MathHelper.Pi);
        }
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pic, picPos, null, Color.White, picRot + (float)(0.5 * MathHelper.Pi), new Vector2(pic.Width / 2, pic.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
        }
        public void reset()
        {
            picPos = new Vector2(rand.Next(800), rand2.Next(600));
        }
    }
}
