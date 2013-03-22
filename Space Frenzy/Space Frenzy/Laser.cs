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

namespace Space_Frenzy
{
    class Laser
    {
        private Vector2 pos, origin;
        private Texture2D laserTexture;
        private float rotationAngle;
        public Laser(Texture2D laserTexture, Player player)
        {
            origin.X = laserTexture.Width / 2;
            origin.Y = laserTexture.Height / 2;
            rotationAngle = player.getRotationAngle();
            pos = player.getPos();
            this.laserTexture = laserTexture;
        }
        public void move()
        {
            pos += new Vector2((float)Math.Cos(rotationAngle) * 11, (float)Math.Sin(rotationAngle) * 11);
        }
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(laserTexture, pos, null, Color.White,
                rotationAngle + MathHelper.PiOver2, origin, .1f, SpriteEffects.None, 0f);
        }
        public Boolean isOffScreen()
        {
            if (pos.X <= -laserTexture.Width || pos.X >= 1280 || pos.Y >= 1024 || pos.Y <= -laserTexture.Height)
                return true;
            else
                return false;
        }
        public Boolean isTouchingEnemy(Enemy enemy)
        {
            Vector2 enemyPos = new Vector2(enemy.getRect().X, enemy.getRect().Y);
            if ((enemyPos.X < pos.X) && (enemyPos.X + enemy.getRect().Width > pos.X)
                && (enemyPos.Y < pos.Y) && (enemyPos.Y + enemy.getRect().Height > pos.Y) && enemy.getDeathTimer() == 0)
                return true;
            else
                return false;
        }

    }



}

