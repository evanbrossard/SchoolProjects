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
    class FacesMoveTo
    {
        Texture2D pic;
        public Vector2 picPos = new Vector2(100, 100);
        float picRot;
        public FacesMoveTo(Texture2D pic, bool k)
        {
            this.pic = pic;
            if(k)
                picPos = new Vector2(350, 200);

        }
        public void update(MouseState mouse)
        {
            Vector2 personToFollowPos = new Vector2(mouse.X, mouse.Y);
            Vector2 difference = personToFollowPos - picPos;
            if (difference.X > 3 || difference.X < -3 || difference.Y > 3 || difference.Y < -3)
            {
                picRot = (float)Math.Atan2((double)difference.Y, (double)difference.X);
                picPos.X += (float)Math.Cos((double)picRot) * 2;
                picPos.Y += (float)Math.Sin((double)picRot) * 2;
            }
        }
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pic, picPos, null, Color.White, picRot + (float)(0.5 * MathHelper.Pi), new Vector2(pic.Width / 2, pic.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
