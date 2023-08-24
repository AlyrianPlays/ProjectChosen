using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;


namespace Game1
{
    public class Game1 : Game
    {
        private SaveState gameData;
        private GraphicsDeviceManager _graphics;
        private Level currentLevel;
        private Dictionary<(int, int), Character> currentCharacters;
        private Dictionary<(int, int), Character> currentProjectiles;

        List<Sprite> platforms;
        Vector2 scale;

        Vector2 position = new Vector2(0, 0);
        Vector2 velocity = new Vector2(1024, 1024);
        int worldHeight = 1080;
        int worldWidth = 3840;

        //Character
        Sprite testSprite;
        //List to build right walk cycle
        List<Sprite> testList;
        //List to build left walk cycle
        List<Sprite> testList2;
        //List to build shrine animation
        List<Sprite> testList3;
        //Right walk cycle animation
        Animation testAni;
        //Left walk cycle animation
        Animation testAni2;

        Sprite shrineTest;
        Animation shrineAni;
        int swap;
        Camera Camera;

        Character mainChar;
        Sprite testScale;
        int gameState = 0;
        int bagState = 0;
        int stateLatchB = 0;

        Sprite bag;
        Sprite worldChunk;
        Sprite testGem;

        //Testing World Builder
        private bool lastLeftClickState;
        private Vector2 drawCoordinate;
        private Vector2 lastDrawCoordinate;
        private String currentDrawChunk;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            //FHD
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            //HD, wonky with fullscreen on FHD monitor
            //_graphics.PreferredBackBufferWidth = 1280;
            //_graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            Camera = new Camera(GraphicsDevice.Viewport, worldWidth, worldHeight, 1.0f);
            scale = new Vector2(_graphics.PreferredBackBufferWidth/(60), _graphics.PreferredBackBufferHeight/(33.75f));

            float aniTime = (float)0.075;
            float aniTime2 = (float)0.25;

            platforms = new List<Sprite>();
            //platforms.Add(new Sprite(new Vector2(320, GraphicsDevice.Viewport.Height - 32), scale, velocity, "Dirt1", aniTime, this.Content, 1));
            //platforms.Add(new Sprite(new Vector2(320, GraphicsDevice.Viewport.Height - 64), scale, velocity, "Dirt1", aniTime, this.Content, 1));
            //platforms.Add(new Sprite(new Vector2(320, GraphicsDevice.Viewport.Height - 256), scale, velocity, "Dirt1", aniTime, this.Content, 1));
            //platforms.Add(new Sprite(new Vector2(180, GraphicsDevice.Viewport.Height - 128), scale, velocity, "Dirt1", aniTime, this.Content, 1));
            //platforms.Add(new Sprite(new Vector2(1500, GraphicsDevice.Viewport.Height - 32), scale, velocity, "Dirt1", aniTime, this.Content, 1));

            for (int i = 0; i < worldWidth / 32; i++)
            {
                platforms.Add(new Sprite(new Vector2(i * 32, GraphicsDevice.Viewport.Height - 24), scale, velocity, "Dirt1", aniTime, this.Content, 1));
                platforms.Add(new Sprite(new Vector2(i * 32, GraphicsDevice.Viewport.Height - 56), scale, velocity, "Dirt1", aniTime, this.Content, 1));
            }

            

            //Testing Level class load
            currentLevel = new Level();
            currentLevel.LoadLevel(@"C:\Users\music\source\repos\Game1\Game1\Content\SaveLevelTest.json", this.Content);
/*
            //Testing Level class save
            int testHeight = 1080;
            int testWidth = 3840;
            Dictionary<(int, int), Sprite> testPlatforms = new Dictionary<(int, int), Sprite>();
            foreach (Sprite s in platforms)
            {
                testPlatforms.Add(((int)s.position.X, (int)s.position.Y), s);
            }


            List<Character> testNPCs = new List<Character>();
            //testNPCs.Add(mainChar);
            currentLevel = new Level(testHeight, testWidth, testPlatforms, testNPCs);
            Level.SaveLevel(currentLevel, @"C:\Users\music\source\repos\Game1\Game1\Content\SaveLevelTest.json");
*/
            //Testing loading the game from a previous save
            gameData = LoadGame();
            if(gameData != null)
            {
                position.X = (float) Convert.ToDouble(gameData.positionX);
                position.Y = (float) Convert.ToDouble(gameData.positionY);
            }
            else
            {
                position = new Vector2(0, worldHeight - this.Content.Load<Texture2D>("FemaleConfidentWholeNoArm").Height);
            }

            //Testing color changing
            testGem = new Sprite(position, scale, velocity, "Whitestone1_large", 1, this.Content, 4);
            
            shrineTest = new Sprite(new Vector2(640, 0), scale, velocity, "FlameShrine1", aniTime2, this.Content, 2);
            swap = 1;

            //Testing Sprite and Animation classes
            testSprite = new Sprite(position, scale, velocity, "FemaleConfidentWholeNoArm", 1, this.Content, 2);

            testList = new List<Sprite>();
            
            //float aniTime = (float)0.25;
            testList.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholeUpRight", aniTime, this.Content, 2));
            testList.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholeContactRight", aniTime, this.Content, 2));
            testList.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholeDownRight", aniTime, this.Content, 2));
            testList.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholePassingRight", aniTime, this.Content, 2));
            testList.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholeUpLeft", aniTime, this.Content, 2));
            testList.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholeContactLeft", aniTime, this.Content, 2));
            testList.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholeDownLeft", aniTime, this.Content, 2));
            testList.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholePassingLeft", aniTime, this.Content, 2));
            testAni = new Animation(testList);
            
            testList2 = new List<Sprite>();
            testList2.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholeUpRight", aniTime, this.Content, 2));
            testList2.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholeContactRight", aniTime, this.Content, 2));
            testList2.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholeDownRight", aniTime, this.Content, 2));
            testList2.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholePassingRight", aniTime, this.Content, 2));
            testList2.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholeUpLeft", aniTime, this.Content, 2));
            testList2.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholeContactLeft", aniTime, this.Content, 2));
            testList2.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholeDownLeft", aniTime, this.Content, 2));
            testList2.Add(new Sprite(position, scale, velocity, "FemaleConfidentWholePassingLeft", aniTime, this.Content, 2));
            testAni2 = new Animation(testList2);

            testList3 = new List<Sprite>();
            testList3.Add(new Sprite(new Vector2(640, 0), scale, velocity, "FlameShrine3", aniTime2, this.Content, 2));
            testList3.Add(new Sprite(new Vector2(640, 0), scale, velocity, "FlameShrine2", aniTime2, this.Content, 2));
            testList3.Add(new Sprite(new Vector2(640, 0), scale, velocity, "FlameShrine1", aniTime2, this.Content, 2));
            testList3.Add(new Sprite(new Vector2(640, 0), scale, velocity, "FlameShrine4", aniTime2, this.Content, 2));

            shrineAni = new Animation(testList3);


            mainChar = new Character(position, scale, new Vector2(15, 1080), "FemaleConfidentWholeNoArm", 0.075f,
                Content, currentLevel.platforms, testAni, testAni2, new Vector2(37, 107), "FemaleConfidentArm", true, 2);


            //Use this when switching resolutions for testing, will save new position VVV
            //mainChar.position = new Vector2(0, worldHeight - (mainChar.visual.Height * mainChar.scale.X));
            // TODO: use this.Content to load your game content here

            //Testing new scaling 12/21/2021
            testScale = new Sprite(position, scale, velocity, "FemaleConfidentWholeNoArm", 1, this.Content, 2);
            testScale.position = new Vector2(0, worldHeight - (testScale.visual.Height*testScale.scale.X));

            //Testing inventory and paper doll screens
            bag = new Sprite(position, scale, velocity, "Bag", 1, this.Content, 12);
            worldChunk = new Sprite(position, scale, velocity, "Dirt1", 1, this.Content, 1);

            lastLeftClickState = false;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            MouseState mouse = Mouse.GetState(Window);
            switch (gameState)
            {
                case 0:
                    if (state.IsKeyDown(Keys.P))
                    {
                        _graphics.ToggleFullScreen();
                    }

                    if (state.IsKeyDown(Keys.Escape))
                    {
                        //Testing saving the game on exit
                        SaveGame();
                        Exit();
                    }

                    if (state.IsKeyDown(Keys.B) && stateLatchB == 0)
                    {
                        if(bagState == 0)
                        {
                            bagState = 1;
                        }
                        else
                        {
                            bagState = 0;
                        }
                        stateLatchB = 1;
                    }
                    if (state.IsKeyUp(Keys.B))
                    {
                        stateLatchB = 0;
                    }

                    //Updating mainChar
                    mainChar.Update(gameTime, state, mouse, Camera, worldWidth, worldHeight, currentLevel.platforms);

                    //World builder code here, place blocks when left button pressed
                    currentDrawChunk = "Dirt1";
                    Matrix inverse = Matrix.Invert(Camera.GetTransformation());
                    Vector2 mousePos = Vector2.Transform(new Vector2(mouse.X, mouse.Y), inverse);
                    lastDrawCoordinate = drawCoordinate;
                    drawCoordinate = new Vector2(scale.X * ((int)mousePos.X / (int)scale.X), scale.X * ((int)mousePos.Y / (int)scale.X));
                    System.Diagnostics.Debug.WriteLine(drawCoordinate.X + " " + drawCoordinate.Y);
                    if (mouse.LeftButton == ButtonState.Pressed && lastLeftClickState == false)
                    {
                        lastLeftClickState = true;
                        if (!currentLevel.platforms.ContainsKey(((int)drawCoordinate.X, (int)drawCoordinate.Y)))
                        {
                            currentLevel.platforms.Add((Convert.ToInt32(drawCoordinate.X), Convert.ToInt32(drawCoordinate.Y)), new Sprite(new Vector2((int)drawCoordinate.X, (int)drawCoordinate.Y), scale, velocity, currentDrawChunk, 0, this.Content, 1));
                        }
                    }
                    else if(mouse.LeftButton == ButtonState.Pressed && lastLeftClickState == true)
                    {
                        if (!lastDrawCoordinate.Equals(drawCoordinate) && !currentLevel.platforms.ContainsKey(((int)drawCoordinate.X, (int)drawCoordinate.Y)))
                        {
                            currentLevel.platforms.Add((Convert.ToInt32(drawCoordinate.X), Convert.ToInt32(drawCoordinate.Y)), new Sprite(new Vector2((int)drawCoordinate.X, (int)drawCoordinate.Y), scale, velocity, currentDrawChunk, 0, this.Content, 1));
                        }
                    }
                    else
                    {
                        lastLeftClickState = false;
                    }

                    //Animation test
                    if ((shrineAni.currTime >= shrineTest.aniTime) && (shrineAni.currSprite == 3))
                    {
                        if (swap == 1)
                        {
                            swap = 0;
                        }
                        else
                        {
                            swap = 1;
                        }
                    }

                    //System.Diagnostics.Debug.WriteLine(Collision(new Sprite(new Vector2(position.X + 10, position.Y), scale, velocity, "FemaleConfidentWholeNoArm", 1, this.Content), platforms) == 0);
                    //System.Diagnostics.Debug.WriteLine(jump);

                    shrineAni.Play(shrineTest, gameTime);
                    Camera.Pos = mainChar.position;
                    bag.position = new Vector2(Camera.Pos.X + _graphics.PreferredBackBufferWidth/2 - bag.visual.Width*bag.scale.X, Camera.Pos.Y + _graphics.PreferredBackBufferHeight/2 - bag.visual.Width * bag.scale.X);

                    base.Update(gameTime);
                    break;
                default:
                    break;
            }
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            // TODO: Add your update logic here
        }

        protected override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);
            switch (gameState)
            {
                case 0:
                    GraphicsDevice.Clear(Color.CornflowerBlue);

                    // TODO: Add your drawing code here
                    spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Camera.GetTransformation());

                    foreach (KeyValuePair<(int, int), Sprite> kvp in currentLevel.platforms)
                    {
                        //System.Diagnostics.Debug.WriteLine(kvp.Value.scale.X);
                        spriteBatch.Draw(kvp.Value.visual, new Vector2(kvp.Key.Item1, kvp.Key.Item2), null, Color.White, 0, Vector2.Zero, kvp.Value.scale.X, SpriteEffects.None, 0);
                    }

                    //Worldbuilder highlight
                    spriteBatch.Draw(Content.Load<Texture2D>("Highlight"), drawCoordinate, null, Color.White, 0, Vector2.Zero, worldChunk.scale.X, SpriteEffects.None, 0);

                    //spriteBatch.Draw(character, position: Vector2.Zero, Color.White);
                    if (mainChar.charDirection == 0)
                    {
                        spriteBatch.Draw(mainChar.visual, mainChar.position, null, Color.White, 0, Vector2.Zero, mainChar.scale.X, SpriteEffects.None, 0);
                        if (!mainChar.walkCycle.playing && !mainChar.walkCycle2.playing)
                        {
                            spriteBatch.Draw(Content.Load<Texture2D>(mainChar.armContent), new Vector2(mainChar.position.X + (37 * mainChar.scale.X), mainChar.position.Y + (107 * mainChar.scale.X)), null, Color.White, mainChar.armRotation, new Vector2(mainChar.armOrigin.X, mainChar.armOrigin.Y), mainChar.scale.X, SpriteEffects.None, 0);
                        }
                    }
                    else if (mainChar.charDirection == 1)
                    {
                        spriteBatch.Draw(mainChar.visual, mainChar.position, null, Color.White, 0, Vector2.Zero, mainChar.scale.X, SpriteEffects.FlipHorizontally, 0);
                        if (!mainChar.walkCycle.playing && !mainChar.walkCycle2.playing)
                        {
                            spriteBatch.Draw(Content.Load<Texture2D>(mainChar.armContent), new Vector2(mainChar.position.X + (90 * mainChar.scale.X), mainChar.position.Y + (107 * mainChar.scale.X)), null, Color.White, mainChar.armRotation, new Vector2(mainChar.armOrigin.X + 53, mainChar.armOrigin.Y), mainChar.scale.X, SpriteEffects.FlipHorizontally, 0);
                        }
                    }

                    //Animation Test (fire shrine)
/*                    if (swap == 1)
                    {
                        spriteBatch.Draw(shrineTest.visual, shrineTest.position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
                    }
                    else
                    {
                        spriteBatch.Draw(shrineTest.visual, shrineTest.position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
                    }*/
                    
                    //Bag
                    if(bagState == 1)
                    {
                        spriteBatch.Draw(bag.visual, bag.position, null, Color.White, 0, Vector2.Zero, bag.scale.X, SpriteEffects.None, 0);
                        spriteBatch.Draw(Content.Load<Texture2D>("Dirt1"), new Vector2(bag.position.X + (16 * bag.scale.X), bag.position.Y + (64 * bag.scale.X)), null, Color.White, 0, Vector2.Zero, worldChunk.scale.X, SpriteEffects.None, 0);
                    }

                    //Gems
                    //spriteBatch.Draw(testGem.visual, new Vector2(mainChar.position.X, mainChar.position.Y - 200), null, Color.Red, 0, Vector2.Zero, testGem.scale.X, SpriteEffects.None, 0);
                    //spriteBatch.Draw(testGem.visual, new Vector2(mainChar.position.X, mainChar.position.Y - 400), null, Color.White, 0, Vector2.Zero, testGem.scale.X, SpriteEffects.None, 0);

                    spriteBatch.End();

                    //System.Diagnostics.Debug.WriteLine(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width.ToString());
                    //System.Diagnostics.Debug.WriteLine(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height.ToString());
                    base.Draw(gameTime);
                    break;
                default:
                    break;
            }
        }

        public static Sprite Collision(Sprite comparison, Dictionary<(int, int), Sprite> list)
        {
            List<Vector2> compList = CompList(comparison);
            foreach (Vector2 element in compList)
            {
                if(!list.ContainsKey(((int)element.X, (int)element.Y)))
                {
                    continue;
                }
                Sprite compListElement = list[((int)element.X, (int)element.Y)];
                float elementX = (compListElement.position.X + (compListElement.visual.Width * compListElement.scale.X));
                float elementY = (compListElement.position.Y + (compListElement.visual.Height * compListElement.scale.X));
                float comparisonX = (comparison.position.X + (comparison.visual.Width * comparison.scale.X));
                float comparisonY = (comparison.position.Y + (comparison.visual.Height * comparison.scale.X));

                if (doOverlap(compListElement.position, 
                    new Vector2((int)elementX, (int)elementY), 
                    comparison.position, 
                    new Vector2((int)comparisonX, (int)comparisonY)))
                {
                    return compListElement;
                }
            } 
            return null;
        }

        //backup of collision
/*        public static Sprite Collision(Sprite comparison, Dictionary<(int, int), Sprite> list)
        {
            //TODO: Logic for checking around comparison, not the whole list
            List<Vector2> compList = CompList(comparison);
            foreach (KeyValuePair<(int, int), Sprite> element in list)
            {
                float elementX = (element.Value.position.X + (element.Value.visual.Width * element.Value.scale.X));
                float elementY = (element.Value.position.Y + (element.Value.visual.Height * element.Value.scale.X));
                float comparisonX = (comparison.position.X + (comparison.visual.Width * comparison.scale.X));
                float comparisonY = (comparison.position.Y + (comparison.visual.Height * comparison.scale.X));

                if (doOverlap(element.Value.position,
                    new Vector2((int)elementX, (int)elementY),
                    comparison.position,
                    new Vector2((int)comparisonX, (int)comparisonY)))
                {
                    return element.Value;
                }
            }
            return null;
        }*/

        public static List<Vector2> CompList(Sprite sprite)
        {
            List<Vector2> retList = new List<Vector2>();
            for (int i = (int)sprite.position.X; i < (sprite.position.X + (sprite.visual.Width * sprite.scale.X)); i += (int)sprite.gameScaleFactor.X)
            {
                for (int k = (int)sprite.position.Y; k < (sprite.position.Y + (sprite.visual.Height * sprite.scale.X)); k += (int)sprite.gameScaleFactor.X)
                {
                    Vector2 elementTopLeft = new Vector2(sprite.gameScaleFactor.X * (i / (int)sprite.gameScaleFactor.X), sprite.gameScaleFactor.X * (k / (int)sprite.gameScaleFactor.X));
                    Vector2 elementTopRight = new Vector2((sprite.gameScaleFactor.X * (i / (int)sprite.gameScaleFactor.X))+sprite.gameScaleFactor.X, sprite.gameScaleFactor.X * (k / (int)sprite.gameScaleFactor.X));
                    Vector2 elementBotLeft = new Vector2(sprite.gameScaleFactor.X * (i / (int)sprite.gameScaleFactor.X), (sprite.gameScaleFactor.X * (k / (int)sprite.gameScaleFactor.X)) + sprite.gameScaleFactor.X);
                    Vector2 elementBotRight = new Vector2((sprite.gameScaleFactor.X * (i / (int)sprite.gameScaleFactor.X)) + sprite.gameScaleFactor.X, (sprite.gameScaleFactor.X * (k / (int)sprite.gameScaleFactor.X)) + sprite.gameScaleFactor.X);
                    if (!retList.Contains(elementTopLeft))
                    {
                        retList.Add(elementTopLeft);
                    }
                    if (!retList.Contains(elementTopRight))
                    {
                        retList.Add(elementTopRight);
                    }
                    if (!retList.Contains(elementBotLeft))
                    {
                        retList.Add(elementBotLeft);
                    }
                    if (!retList.Contains(elementBotRight))
                    {
                        retList.Add(elementBotRight);
                    }
                }
            }
            return retList;
        }

        public static bool doOverlap(Vector2 l1, Vector2 r1, Vector2 l2, Vector2 r2)
        {
            // If one rectangle is on left side of other
            if (l1.X >= r2.X)
                return false;

            if (l2.X >= r1.X)
                return false;

            // If one rectangle is above other
            if (r1.Y <= l2.Y)
                return false;

            if (r2.Y <= l1.Y)
                return false;

            return true;
        }

        public void SaveGame()
        {
            using (StreamWriter file = File.CreateText(@"C:\Users\music\source\repos\Game1\Game1\Content\path.json"))
            {
                List<String> testInventory = new List<String>() {"Hi","There","it","worked" };
                gameData = new SaveState(mainChar.position, testInventory);
                var serializer = new JsonSerializer();
                serializer.Serialize(file, gameData);
            }
        }
        public SaveState LoadGame()
        {
            try
            {
                using (var file = File.OpenText(@"C:\Users\music\source\repos\Game1\Game1\Content\path.json"))
                {
                    var serializer = new JsonSerializer();
                    return (SaveState)serializer.Deserialize(file, typeof(SaveState));
                }
            }
            catch(FileNotFoundException e)
            {
                return null;
            }
        }
    }
}
