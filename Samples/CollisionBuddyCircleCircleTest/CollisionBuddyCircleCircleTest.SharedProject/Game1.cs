using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PrimitiveBuddy;
using CollisionBuddy;
using GameTimer;
using HadoukInput;

namespace CollisionBuddyCircleCircleTest
{
	/// <summary>
	/// This "game" puts two circles on teh screen and tests the collision detection.
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		Circle _circle1;
		Circle _circle2;

		GameClock _clock;

		InputState _inputState;
		InputWrapper _inputWrapper;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			_circle1 = new Circle();
			_circle2 = new Circle();

			_clock = new GameClock();
			_inputState = new InputState();
			_inputWrapper = new InputWrapper(new ControllerWrapper(PlayerIndex.One, true), _clock.GetCurrentTime);
			_inputWrapper.Controller.UseKeyboard = true;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			Setup();

			base.Initialize();
		}

		private void Setup()
		{
			//init the blue circle so it will be on the left of the screen
			_circle1.Initialize(new Vector2(graphics.GraphicsDevice.Viewport.TitleSafeArea.Center.X - 300,
				graphics.GraphicsDevice.Viewport.TitleSafeArea.Center.Y), 80.0f);

			//put the red circle on the right of the screen
			_circle2.Initialize(graphics.GraphicsDevice.Viewport.TitleSafeArea.Center, 80.0f);

			_clock.Start();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) || 
				Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
#if !__IOS__
				this.Exit();
#endif
			}

			//update the timer
			_clock.Update(gameTime);

			//update the input
			_inputState.Update();
			_inputWrapper.Update(_inputState, false);

			//move the circle
			float movespeed = 3200.0f;
			if (_inputWrapper.Controller.CheckKeystroke(EKeystroke.Up, false, Vector2.UnitX))
			{
				_circle1.Translate(0.0f, -movespeed * _clock.TimeDelta);
			}
			else if (_inputWrapper.Controller.CheckKeystroke(EKeystroke.Down, false, Vector2.UnitX))
			{
				_circle1.Translate(0.0f, movespeed * _clock.TimeDelta);
			}
			else if (_inputWrapper.Controller.CheckKeystroke(EKeystroke.Forward, false, Vector2.UnitX))
			{
				_circle1.Translate(movespeed * _clock.TimeDelta, 0.0f);
			}
			else if (_inputWrapper.Controller.CheckKeystroke(EKeystroke.Back, false, Vector2.UnitX))
			{
				_circle1.Translate(-movespeed * _clock.TimeDelta, 0.0f);
			}

			if (_inputWrapper.Controller.CheckKeystroke(EKeystroke.A))
			{
				Setup();
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			//draw the circles green...
			Color circleColor = Color.Green;

			//...unless a collision is occuring, in which case draw the circles in red.
			if (CollisionCheck.CircleCircleCollision(_circle2, _circle1))
			{
				circleColor = Color.Red;
			}

			spriteBatch.Begin();

			//draw the circles
			var circlePrim = new Primitive(graphics.GraphicsDevice, spriteBatch);
			circlePrim.Circle(_circle1.Pos, _circle1.Radius, circleColor);
			circlePrim.Circle(_circle2.Pos, _circle2.Radius, circleColor);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
