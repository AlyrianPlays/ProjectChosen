using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace Game1
{
    public class Animation
    {
        public List<Sprite> sprites { get; set; }
        public bool playing { get; set; }
        public float currTime { get; set; }
        public int currSprite { get; set; }

        public Animation(List<Sprite> sprites)
        {
            this.sprites = sprites;
        }

        public void Play(Sprite sprite, GameTime gameTime)
        {
            if (!this.playing)
            {
                this.playing = true;
                this.currSprite = 0;
                sprite.visual = this.sprites[this.currSprite].visual;
                this.currTime = 0;
            }
            else
            {
                if(this.currTime >= this.sprites[this.currSprite].aniTime)
                {
                    this.currTime = 0;
                    if(this.currSprite+1 == this.sprites.Count)
                    {
                        this.currSprite = 0;
                    }
                    else
                    {
                        this.currSprite++;
                    }
                    sprite.visual = this.sprites[this.currSprite].visual;
                }
                else
                {
                    this.currTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }
        }

        public void Stop(Sprite sprite)
        {
            if (this.playing)
            {
                this.playing = false;
                sprite.visual = sprite.origVisual;
                this.currSprite = 0;
                this.currTime = 0;
            }
        }
    }
}
