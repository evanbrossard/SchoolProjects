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
        Rectangle rec;
        public FacesMoveTo(Texture2D pic)
        {
            this.pic = pic;
            picPos = new Vector2(350, 200);
            rec = new Rectangle(350, 200, 24, 31);

        }
        public void update(Vector2 personToFollow)
        {
            Vector2 personToFollowPos = new Vector2(personToFollow.X, personToFollow.Y);
            Vector2 difference = personToFollowPos - picPos;
            if (difference.X > 3 || difference.X < -3 || difference.Y > 3 || difference.Y < -3)
            {
                picRot = (float)Math.Atan2((double)difference.Y, (double)difference.X);
                picPos.X += (float)Math.Cos((double)picRot)/4;
                picPos.Y += (float)Math.Sin((double)picRot)/4;
            }
        }
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(pic, picPos, null, Color.White, picRot + (float)(0.5 * MathHelper.Pi), new Vector2(pic.Width / 2, pic.Height / 2), 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
