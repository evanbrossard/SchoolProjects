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
    class Enemy
    {
        private int attackPattern, fireRate, attackDamage, health, rotation, timer, baseTime, deathTimer;
        private Rectangle rect;
        private Texture2D shipPic, explosPic;
        private Vector2 origin, movement;
        private bool dead = false;
        public Enemy(int attackPattern, int fireRate, int attackDamage, int health, Texture2D shipPic, Texture2D explosPic, Rectangle startingRect, int rotation)
        {
            this.attackDamage = attackDamage;
            this.attackPattern = attackPattern;
            this.fireRate = fireRate;
            this.health = health;
            this.shipPic = shipPic;
            this.rotation = rotation;
            this.explosPic = explosPic;
            rect = startingRect;
            origin.X = rect.Width / 2;
            origin.Y = rect.Height / 2;
            deathTimer = 0;
            movement = Vector2.Zero;
        }
        public void move()
        {
            if (dead && deathTimer == 0)
            {
                shipPic = explosPic;
            }
            if (dead)
            {
                rect.Width = (int)((float)rect.Width + 1);
                rect.Height = (int)((float)rect.Height + 1);
                rect.X = (int)((float)rect.X - 1);
                rect.Y = (int)((float)rect.Y - 1);
                deathTimer++;
            }
            else
            {
                switch (attackPattern)
                {
                    case (1):
                        rect.Y += 1;
                        rotation += 5;
                        rect.X += (int)(Math.Cos((double)MathHelper.ToRadians(rotation)) * 3);
                        break;
                    case (2):
                        rect.X -= 2;
                        rect.Y += 2;
                        break;
                    case (3):
                        rect.X += 2;
                        rect.Y += 2;
                        break;
                    case (4):
                        if (timer >= baseTime && (timer <= (baseTime + 90)))
                        {
                            movement.X = 1;
                            movement.Y = -1;
                        }
                        if ((timer > baseTime + 90) && (timer <= baseTime + 180))
                        {
                            movement.X = 1;
                            movement.Y = 1;
                        }
                        if ((timer > baseTime + 180) && (timer <= baseTime + 270))
                        {
                            movement.X = -1;
                            movement.Y = 1;
                        }
                        if ((timer > baseTime + 270) && (timer <= baseTime + 360))
                        {
                            movement.X = -1;
                            movement.Y = -1;
                        }

                        if (timer % 360 == 0 && timer != 0)
                        {
                            baseTime += 360;
                        }
                        rect.X += (int)movement.X;
                        rect.Y += (int)movement.Y;
                        timer++;
                        break;
                }
            }
            
        }
        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(shipPic, rect, null, Color.White, 0, origin, SpriteEffects.None, 0.0f);
        }
        public void hitByLaser(int damage)
        {
            health -= damage;
        }
        public Rectangle getRect()
        {
            return rect;
        }
        public int getHealth()
        {
            return health;
        }
        public int getDeathTimer()
        {
            return deathTimer;
        }
        public void destroyed()
        {
            dead = true;
        }
    }
}
