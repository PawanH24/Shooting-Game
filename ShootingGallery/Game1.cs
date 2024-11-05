using System; //for random
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ShootingGallery
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D targetSprite;
        Texture2D crossHairsSprite;
        Texture2D backgroundSprite;
        Texture2D heartSprite;
        SpriteFont gameFont;


        Vector2 targetPosition = new Vector2(300, 300);
        const int targetRadius = 45;

        MouseState mState;
        bool mReleased = true;
        int score = 0;
        float timer = 10f;
        int life = 3;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            targetSprite = Content.Load<Texture2D>("target");
            crossHairsSprite = Content.Load <Texture2D>("crosshairs");
            backgroundSprite = Content.Load<Texture2D>("sky");
            gameFont = Content.Load<SpriteFont>("galleryFont");
            heartSprite = Content.Load<Texture2D>("heart");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (timer >0) 
            { 
                timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (timer < 0)
                    timer = 0;
            }
            

            mState = Mouse.GetState();

            if (mState.LeftButton == ButtonState.Pressed && mReleased == true) 
            {
                float mouseTargetDist = Vector2.Distance(targetPosition,mState.Position.ToVector2());
                if (mouseTargetDist<targetRadius && timer>0)
                {
                    score++;

                    Random rand = new Random();
                    targetPosition.X = rand.Next(targetRadius,_graphics.PreferredBackBufferWidth-targetRadius+1);
                    targetPosition.Y = rand.Next(targetRadius,_graphics.PreferredBackBufferHeight-targetRadius+1);
                }
                else if(mouseTargetDist>targetRadius && timer>0) 
                {
                    life--;

                    if (life == 0)
                    {
                        timer = 0;
                    }

                }
                mReleased = false;
            }
            if(mState.LeftButton==ButtonState.Released)
            {
                mReleased= true;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            
            _spriteBatch.Draw(backgroundSprite, new Vector2(0, 0), Color.White);

            for (int i = 0; i < 3; i++)
            {
                Color heartColor = i < life ? Color.White : Color.Black;

                // Draw each heart with a slight offset for spacing, in a fixed position
                _spriteBatch.Draw(heartSprite, new Vector2(_graphics.PreferredBackBufferWidth - 5
                    - (heartSprite.Width+25*i), 0), heartColor);
            }
            _spriteBatch.DrawString(gameFont,"Score: "+ score.ToString(), new Vector2(3,3), Color.White);
            if (timer > 0)
            {
                _spriteBatch.Draw(targetSprite, new Vector2(targetPosition.X - targetRadius, targetPosition.Y - targetRadius), Color.White);
            }
            _spriteBatch.DrawString(gameFont, "Time: "+Math.Ceiling(timer).ToString(), new Vector2(3, 40),Color.White);
            _spriteBatch.Draw(crossHairsSprite, new Vector2(mState.X-25, mState.Y-25), Color.White) ;
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
