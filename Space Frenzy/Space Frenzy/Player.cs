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
    class Player
    {
        private int health, maxHealth, gold, multiplier, laserDamage;
        private Texture2D shipTexture, redLaserTexture;
        private SpriteBatch spriteBatch;
        private Vector2 origin, shipPos;
        private float rotationAngle, drawnAngle, shotTimer, attackSpeed;
        private Rectangle shipRect;
        private List<Laser> laserArray;
        private MouseState currentMouseState;
        private Color shipColor;

        public Player(SpriteBatch spriteBatch, Texture2D shipTexture, Texture2D redLaserTexture)
        {
            this.spriteBatch = spriteBatch;
            this.shipTexture = shipTexture;                                                           
            this.redLaserTexture = redLaserTexture;
            origin.X = shipTexture.Width / 2;
            origin.Y = shipTexture.Height / 2;
            laserArray = new List<Laser>();
            health = 100;
            maxHealth = 100;
            gold = 10;
            attackSpeed = 3;
            shipPos.X = 50;
            shipPos.Y = 50;
            rotationAngle = MathHelper.Pi / 2;
            drawnAngle = rotationAngle;
            shipRect.Height = 100;
            shipRect.Width = 100;
            multiplier = 1;
            laserDamage = 5;
        }

        public void Move(bool titleScreen, ref EnemyManager eManager)
        {
            currentMouseState = Mouse.GetState();
            Vector2 difference = new Vector2(currentMouseState.X - shipPos.X, currentMouseState.Y - shipPos.Y);
            if ((difference.X <= -5 || difference.X >= 5) || (difference.Y <= -5 || difference.Y >= 5))
            {
                rotationAngle = (float)Math.Atan2((double)difference.Y, (double)difference.X);
                if (currentMouseState.LeftButton == ButtonState.Released)
                    drawnAngle = rotationAngle;
            }
            
            if(difference.X <= -5 || difference.X >= 5)
                shipPos.X += (float)Math.Cos(rotationAngle) * Math.Abs(difference.X)/10;
            if(difference.Y <= -5 || difference.Y >= 5)
                shipPos.Y += (float)Math.Sin(rotationAngle) * Math.Abs(difference.Y)/10;
            
            if (shotTimer >= attackSpeed)
            {
                laserArray.Add(new Laser(redLaserTexture, this));
                shotTimer = 0;
            }
            //Move each laser
            for (int i = 0; i < laserArray.Count; i++)
            {
                laserArray[i].move();
            }
            checkLaserCollisions(ref eManager);
            shotTimer += .1f;
        }
        //Draw the ship and each laser
        public void Draw(SpriteFont font)
        {
            spriteBatch.Draw(shipTexture, shipPos, null, Color.White,
                drawnAngle + MathHelper.PiOver2, origin, 1f, SpriteEffects.None, 0f);
            for (int i = 0; i < laserArray.Count; i++)
            {
                laserArray[i].draw(spriteBatch);
            }
            spriteBatch.DrawString(font, gold.ToString(), new Vector2(50, 550), Color.White);
        }
        public Vector2 getPos()
        {
            return shipPos;
        }
        public int getWidth()
        {
            return shipTexture.Width;
        }
        public int getHeight()
        {
            return shipTexture.Height;
        }
        public float getRotationAngle()
        {
            return drawnAngle;
        }
        public Rectangle getRect()
        {
            return shipRect;
        }
        public void hitByLaser()
        {
            health -= 5;
        }
        public List<Laser> getLaserList()
        {
            return laserArray;
        }
        public bool alive()
        {
            if (health <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void restart()
        {
            health = 100;
            shipPos.X = 50;
            shipPos.Y = 50;
            rotationAngle = MathHelper.Pi / 2;
            laserArray.Clear();
        }
        public void drawHealth(SpriteFont myFont)
        {
             spriteBatch.DrawString(myFont, "Health: " + health, new Vector2(50, 20), Color.White);
        }
        public void setColor(Color color)
        {
            shipColor = color;
        }
        public bool intersects(Texture2D otherObject, Vector2 otherObjectPos)
        {
            if (shipPos.X >= otherObjectPos.X && (otherObjectPos.X + otherObject.Width) >= shipPos.X
                && shipPos.Y >= otherObjectPos.Y && (otherObjectPos.Y + otherObject.Height) >= shipPos.Y)
                return true;
            else
                return false;
        }
        private void checkLaserCollisions(ref EnemyManager eManager)
        {
            if (eManager.getEnemyList() != null)
            {
                List<Enemy> enemyList = eManager.getEnemyList();
                for (int i = 0; i < laserArray.Count; i++)
                {
                    for (int x = 0; x < enemyList.Count; x++)
                    {
                        if (laserArray[i].isTouchingEnemy(enemyList[x]))
                        {
                            laserArray.RemoveAt(i);
                            i--;
                            eManager.enemyHitByLaser(x, this.laserDamage);
                            break;
                        }
                    }
                }
            }
        }
        public void addGold(int amount)
        {
            gold += amount;
        }
        public void drawGold(SpriteFont font, Vector2 pos)
        {
            spriteBatch.DrawString(font, gold.ToString(), pos, Color.White);
        }
        public void upgradeShip()
        {
            maxHealth *= 2;
        }
        public void upgradeLaser()
        {
            laserDamage *= 2;
            attackSpeed *= .8f;
        }
        public void upgradeShield()
        {

        }
        public void upgradeMultiplier()
        {
            multiplier++;
        }
        public void removeGold(int amount)
        {
            gold -= amount;
        }
        public bool hasEnoughGold(int amount)
        {
            
                return true;
        }
        public int getLaser()
        {
            return laserDamage;
        }
        public int getMultiplier()
        {
            return multiplier;
        }
        public int getHealth()
        {
            return maxHealth;
        }
        public void resetShip()
        {
            health = maxHealth;
            shipPos.X = 50;
            shipPos.Y = 50;
        }
    }
}
