using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _pixel;
        private int _leftScore;
        private int _rightScore;
        private Vector2 _leftPaddlePos;
        private Vector2 _rightPaddlePos;
        private const int PaddleWidth = 12;
        private const int PaddleHeight = 80;
        private Vector2 _ballPos;
        private Vector2 _ballVelocity;
        private const int BallSize = 12;
        private float _ballTimer = 0f;
        private bool _ballInPlay = false;
        private Random _random = new Random();
        private SpriteFont _scoreFont;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            IsFixedTimeStep = true;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _leftPaddlePos = new Vector2(30, 320);
            _rightPaddlePos = new Vector2(1238, 320);
            _ballPos = new Vector2(640, 360);
            ServeBall();
            _ballInPlay = false;
            _ballTimer = 0f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
            _scoreFont = Content.Load<SpriteFont>("ScoreFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keys = Keyboard.GetState();

            if (keys.IsKeyDown(Keys.W))
            {
                _leftPaddlePos.Y -= 5;
            }
            if (keys.IsKeyDown(Keys.S))
            {
                _leftPaddlePos.Y += 5;
            }
            if (keys.IsKeyDown(Keys.Up))
            {
                _rightPaddlePos.Y -= 300 * dt;
            }
            if (keys.IsKeyDown(Keys.Down))
            {
                _rightPaddlePos.Y += 300 * dt;
            }

            _leftPaddlePos.Y = MathHelper.Clamp(_leftPaddlePos.Y, 0, 720 - PaddleHeight);
            _rightPaddlePos.Y = MathHelper.Clamp(_rightPaddlePos.Y, 0, 720 - PaddleHeight);

            if (_ballInPlay)
            {
                _ballPos += _ballVelocity * dt;

                if (_ballPos.Y <= 0 || _ballPos.Y + BallSize > 720)
                {
                    _ballVelocity.Y = -_ballVelocity.Y;
                }

                Rectangle ballRect = new Rectangle((int)_ballPos.X, (int)_ballPos.Y, BallSize, BallSize);
                Rectangle leftPaddleRect = new Rectangle((int)_leftPaddlePos.X, (int)_leftPaddlePos.Y, PaddleWidth, PaddleHeight);
                Rectangle rightPaddleRect = new Rectangle((int)_rightPaddlePos.X, (int)_rightPaddlePos.Y, PaddleWidth, PaddleHeight);
                if (ballRect.Intersects(leftPaddleRect) || ballRect.Intersects(rightPaddleRect))
                {
                    _ballVelocity.X = -_ballVelocity.X;
                }

                if (_ballPos.X <= 0)
                {
                    _rightScore++;
                    _ballPos = new Vector2(640, 360);
                    ServeBall();
                    _ballInPlay = false;
                    _ballTimer = 0f;
                }

                if (_ballPos.X >= 1280)
                {
                    _leftScore++;
                    _ballPos = new Vector2(640, 360);
                    ServeBall();
                    _ballInPlay = false;
                    _ballTimer = 0f;
                }
            }
            else
            {
                _ballTimer += dt;
                if(_ballTimer >= 2f)
                {
                    _ballInPlay = true;
                    _ballTimer = 0f;
                }
            }


                base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_pixel, new Rectangle((int)_leftPaddlePos.X, (int)_leftPaddlePos.Y,PaddleWidth, PaddleHeight), Color.White);
            _spriteBatch.Draw(_pixel, new Rectangle((int)_rightPaddlePos.X, (int)_rightPaddlePos.Y, PaddleWidth, PaddleHeight), Color.White);
            _spriteBatch.Draw(_pixel, new Rectangle((int)_ballPos.X, (int)_ballPos.Y, BallSize, BallSize), Color.White);
            _spriteBatch.DrawString(_scoreFont, _leftScore.ToString(), new Vector2(400, 30), Color.White);
            _spriteBatch.DrawString(_scoreFont, _rightScore.ToString(), new Vector2(850, 30), Color.White);
            _spriteBatch.End();


            base.Draw(gameTime);
        }

        private void ServeBall()
        {
            float x = _random.Next(0, 2) == 0 ? 200 : -200;
            float y = _random.Next(0, 2) == 0 ? 200 : -200;
            _ballVelocity = new Vector2(x, y);
        }
    }
}
