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
    class EnemyManager
    {
        private List<Enemy> enemyList;
        public EnemyManager()
        {
            enemyList = new List<Enemy>();
        }

        public void add(int attackPattern, int fireRate, int attackDamage, int health, Texture2D shipPic, Texture2D explosPic, Rectangle startingRect, int rotation)
        {
            enemyList.Add(new Enemy(attackPattern, fireRate, attackDamage, health, shipPic, explosPic, startingRect, rotation));
        }

        public void update(Player player)
        {
            for(int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].move();
                if (enemyList[i].getHealth() <= 0 && enemyList[i].getDeathTimer() == 0)
                {
                    enemyList[i].destroyed();
                    player.addGold(5);
                    break;
                }
                if (enemyList[i].getDeathTimer() >= 30)
                {
                    enemyList.RemoveAt(i);
                    break;
                }
                if ((enemyList[i].getRect().X <= -enemyList[i].getRect().Width) || (enemyList[i].getRect().X >= 800) || (enemyList[i].getRect().Y >= 600))
                {
                    enemyList.RemoveAt(i);
                }
            }
            
        }
        public void draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy e in enemyList)
            {
                e.draw(spriteBatch);
            }
        }
        public List<Enemy> getEnemyList()
        {
            return enemyList;
        }
        public bool isLevelOver(int timer)
        {
            if (enemyList.Count == 0 && timer >= 50)
                return true;
            else
                return false;
        }
        public void enemyHitByLaser(int index, int damage)
        {
            enemyList[index].hitByLaser(damage);
        }
    }
}
