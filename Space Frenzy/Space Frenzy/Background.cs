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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Space_Frenzy
{
    class Background
    {
        Texture2D pic;
        Vector2 pos, pos2;
        bool secondPic;
        public Background(Texture2D pic)
        {
            this.pic = pic;
            pos = Vector2.Zero;
        }
        public void update()
        {
            pos += new Vector2(0, -1);
        }
        public void draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(pic, pos, Color.White);
        }
    }
}
