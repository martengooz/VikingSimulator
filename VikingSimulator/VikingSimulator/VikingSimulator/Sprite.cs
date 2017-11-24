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

namespace VikingSimulator
{

    enum dir
    {
        North,
        South,
        West,
        East
    }

    enum type
    {
        cow,
        mjod,
        kottbulle,
        bomb,
        viking,
    }

    class Sprite
    {
        public int direction;
        Texture2D texture;
        Vector2 position;
        public int speed;
        public int health;
        public int strenght;
        public type type;
        public int damage;
        public int speedBoostTimer = 1002;
        public int speedBoost = 0;
        Random crit = new Random();
        public int critSize;
        public int smallCrit = 0;
        public int bigCrit = 0;

        string damageText = "";
        int damageTime = 0;

        public Rectangle hitbox
        {
            get
            {
                return new Rectangle(
                (int)position.X,
                (int)position.Y,
                texture.Width,
                texture.Height);
            }
        }


        public Sprite(Texture2D newTexture, int X, int Y)
        {
            critSize = crit.Next(1, 100);
            texture = newTexture;
            position = new Vector2(X, Y);
            type = type.viking;
            speed = 20;
            health = 1000;
            if (critSize < 100) { smallCrit = crit.Next(0, 4); }
            if (critSize == 100) { bigCrit = crit.Next(20, 25); }
            strenght = 18 + smallCrit + bigCrit;            
            
        }
        public Sprite(Texture2D newTexture, type newType)
        {
            Random value = new Random();
            direction = value.Next(0,3);
            position = new Vector2(value.Next(12, 1140), value.Next(12, 580));
            speed = value.Next(0,0);
            health  = 20;
            texture = newTexture;
            type = newType;

        }

        public void Move(dir direction)
        {
            if (direction == dir.North) { position.Y -= speed + speedBoost;  }
            if (direction == dir.South) { position.Y += speed + speedBoost;  }
            if (direction == dir.West) { position.X -= speed + speedBoost;  }
            if (direction == dir.East) { position.X += speed + speedBoost;  }
            
            if (position.Y < 10) { position.Y = 10; }
            if (position.Y > 582) { position.Y = 582; }
            if (position.X < 10) { position.X = 10; }
            if (position.X > 1142) { position.X = 1142; }
            
            if (speedBoostTimer++ > 1000) { speedBoost = 0; }
        }

        public void MovePowerups(dir direction)
        {
            if (direction == dir.North) { position.Y -= speed; }
            if (direction == dir.South) { position.Y += speed; }
            if (direction == dir.West) { position.X -= speed; }
            if (direction == dir.East) { position.X += speed; }

            if (position.Y < 10) { speed *= -1; }
            if (position.Y > 586) { speed *= -1; }
            if (position.X < 10) { speed *= -1; }
            if (position.X > 1142) { speed *= -1; }
        }

        public void Hurt(int hitStrenght, Texture2D newTexture)
        {
            health = health - hitStrenght;
            texture = newTexture;
            damage = hitStrenght;
        }

        public void Retexture(Texture2D newTexture)
        {
            texture = newTexture;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont damageFont)
        {
            spriteBatch.Draw(texture, position, Color.White);
            if (damageTime++ < 100)
            {
                spriteBatch.DrawString(damageFont, damageText, new Vector2((position.X - (damageFont.MeasureString(damageText).X) / 2) + 64, (position.Y - 50)), Color.Red);
            }
        }

        public void DrawAttack()
        {
            damageText = Convert.ToString(strenght + smallCrit + bigCrit);
            damageTime = 0;
        }


        public void Reset(int X, int Y)
        {
            position = new Vector2(X, Y);
            speed = 20;
            health = 1000;
            strenght = 20;
        }
    }
}
