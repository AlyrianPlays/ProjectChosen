using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Text.Json.Serialization;

namespace Game1
{
    public class Character : Sprite
    {
        public int charDirection { get; set; }
        public Animation walkCycle { get; set; }
        public Animation walkCycle2 { get; set; }
        public int jump { get; set; }
        public int firstJump { get; set; }
        public Microsoft.Xna.Framework.Content.ContentManager contentManager { get; set; }
        public Dictionary<(int, int), Sprite> platforms { get; set; }
        public Vector2 distance { get; set; }
        public Vector2 armOrigin { get; set; }
        public float armRotation { get; set; }
        public string armContent { get; set; }
        public bool MC { get; set; }
        public float jumpHeight { get; set; }
        public float jumpMax { get; set; }
        public bool lastLeftClickState { get; set; }

        public Character(Vector2 position, Vector2 scale, Vector2 velocity, string content, float aniTime, 
            Microsoft.Xna.Framework.Content.ContentManager contentManager, Dictionary<(int, int), Sprite> platforms,
            Animation walkCycle, Animation walkCycle2, Vector2 armOrigin, string armContent, bool MC, int numCows)
        {
            this.numCows = numCows;
            this.position = position;
            this.velocity = velocity;
            this.content = content;
            this.armContent = armContent;
            this.aniTime = aniTime;
            this.contentManager = contentManager;
            visual = this.contentManager.Load<Texture2D>(content);
            origVisual = this.contentManager.Load<Texture2D>(content);
            this.scale = new Vector2((scale.X / 32 / (this.visual.Width / 32)) * numCows, 0);
            this.gameScaleFactor = scale;
            this.platforms = platforms;
            this.walkCycle = walkCycle;
            this.walkCycle2 = walkCycle2;
            this.armOrigin = armOrigin;
            this.MC = MC;
            jumpHeight = 500;
            jumpMax = 0;
        }

        public void Update(GameTime gameTime, KeyboardState keyboard, MouseState mouse, Camera camera, int worldWidth, int worldHeight, Dictionary<(int, int), Sprite> platforms)
        {
            this.platforms = platforms;
            Matrix inverse = Matrix.Invert(camera.GetTransformation());
            Vector2 mousePos = Vector2.Transform(new Vector2(mouse.X, mouse.Y), inverse);

            if (keyboard.IsKeyDown(Keys.D) && position.X <= 
                worldWidth - (visual.Width*this.scale.X) && Game1.Collision(new Sprite(new Vector2(position.X + velocity.X, position.Y), 
                gameScaleFactor, velocity, content, 1, contentManager, numCows), platforms) == null)
            {
                position = new Vector2(position.X + velocity.X, position.Y);
                charDirection = 0;
                walkCycle.Play(this, gameTime);
            }
            else
            {
                Sprite tester = Game1.Collision(new Sprite(new Vector2(position.X + velocity.X, position.Y),
                    gameScaleFactor, velocity, content, 1, contentManager, numCows), platforms);
                if(tester != null)
                {
                    position = new Vector2(tester.position.X - (visual.Width*scale.X), position.Y);
                    walkCycle.Stop(this);
                }
                else
                {
                    walkCycle.Stop(this);
                }
            }
            if (keyboard.IsKeyDown(Keys.A) && position.X >= 
                0 && Game1.Collision(new Sprite(new Vector2(position.X - velocity.X, position.Y), 
                gameScaleFactor, velocity, content, 1, contentManager, numCows), platforms) == null)
            {
                position = new Vector2(position.X - velocity.X, position.Y);
                charDirection = 1;
                walkCycle2.Play(this, gameTime);
            }
            else
            {
                Sprite tester = Game1.Collision(new Sprite(new Vector2(position.X - 10, position.Y),
                    gameScaleFactor, velocity, content, 1, contentManager, numCows), platforms);
                if (tester != null)
                {
                    position = new Vector2(tester.position.X + (tester.visual.Width*tester.scale.X), position.Y);
                    walkCycle2.Stop(this);
                }
                else
                {
                    walkCycle2.Stop(this);
                }
            }
            if (keyboard.IsKeyDown(Keys.A) && keyboard.IsKeyDown(Keys.D))
            {
                walkCycle.Stop(this);
                walkCycle2.Stop(this);
            }
            if (keyboard.IsKeyDown(Keys.Space))
            {
                if (jump != 1)
                {
                    jump = 1;
                    firstJump = 1;
                    jumpMax = position.Y - jumpHeight;
                }
            }

            //State Checks
            //Jump
            if (jump == 1)
            {
                Sprite tester = Game1.Collision(new Sprite(new Vector2(position.X, position.Y - (velocity.Y) *
                    (float)gameTime.ElapsedGameTime.TotalSeconds), gameScaleFactor, velocity, content, 1, contentManager, numCows), platforms);
                if (position.Y < jumpMax)
                {
                    if (velocity.Y > 0)
                    {
                        velocity = new Vector2(velocity.X, velocity.Y * -1);
                        position = new Vector2(position.X, position.Y - (velocity.Y) * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    }
                }
                else if (position.Y >= worldHeight - (int)(this.visual.Height*scale.X))
                {
                    if (firstJump == 0) // we are at the end of our jump
                    {
                        velocity = new Vector2(velocity.X, velocity.Y * -1);
                        position = new Vector2(position.X, worldHeight - (int)(this.visual.Height*scale.X));
                        jump = 0;
                    }
                    else // we pressed jump but aren't jumping yet
                    {
                        firstJump = 0;
                        position = new Vector2(position.X, position.Y - (velocity.Y) * (float)gameTime.ElapsedGameTime.TotalSeconds);
                    }
                }
                else if (tester != null) // colliding with something while going upward
                {
                    if (velocity.Y < 0)
                    {
                        velocity = new Vector2(velocity.X, velocity.Y * -1);
                        position = new Vector2(position.X, tester.position.Y - (int)(visual.Height*scale.X));
                        jump = 0;
                    }
                    else
                    {
                        velocity = new Vector2(velocity.X, velocity.Y * -1);
                    }
                }
                else
                {
                    position = new Vector2(position.X, position.Y - (velocity.Y) * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
            }

            //Gravity
            if ((position.Y + velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds < worldHeight - (int)(this.visual.Height*scale.X)) && jump != 1)
            {
                Sprite tester = Game1.Collision(new Sprite(new Vector2(position.X, position.Y + (velocity.Y) *
                    (float)gameTime.ElapsedGameTime.TotalSeconds), gameScaleFactor, velocity, content, 1, contentManager, numCows), platforms);
                if (tester == null)
                {
                    position = new Vector2(position.X, position.Y + (velocity.Y) * (float)gameTime.ElapsedGameTime.TotalSeconds);
                }
                else
                {
                    position = new Vector2(position.X, tester.position.Y - (int)(visual.Height*scale.X));
                }
            }
            else if (position.Y + velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds >= worldHeight - (int)(this.visual.Height*scale.X) && jump != 1)
            {
                position = new Vector2(position.X, worldHeight - (int)(this.visual.Height*scale.X));
            }

            //Direction based on mouse position
            if (mousePos.X >= position.X + (int)(this.visual.Width*scale.X) / 2)
            {
                charDirection = 0;
            }
            else
            {
                charDirection = 1;
            }

            if (charDirection == 0)
            {
                distance = new Vector2(mousePos.X - (position.X + (armOrigin.X * scale.X)), mousePos.Y - (position.Y + (armOrigin.Y * scale.X)));
                armRotation = (float)Math.Atan(distance.Y / distance.X) - (float)Math.PI * 0.5f;
            }
            else
            {
                distance = new Vector2(mousePos.X - (position.X + ((armOrigin.X+53) * scale.X)), mousePos.Y - (position.Y + (armOrigin.Y * scale.X)));
                armRotation = (float)Math.Atan(distance.Y / distance.X) - (float)Math.PI * 1.5f;
            }
        }
    }
}
