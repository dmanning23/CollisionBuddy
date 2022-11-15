using CollisionBuddy;
using GameTimer;
using HadoukInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PrimitiveBuddy;

namespace CollisionBuddySample
{
	/// <summary>
	/// This is a quick sample game to show how to use the circle-rect collision functionality of the CollisionBuddy
	/// There is a circle in a box, and it will stay in the box no matter how fast it moves.
	/// This shows the tunneling prevention of teh collision
	/// </summary>
	public class Game1 : Microsoft.Xna.Framework.Game
	{
		#region Properties

		#region boilerplate stuff

		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		GameClock _clock;
		InputState _inputState;
		InputWrapper _inputWrapper;

		#endregion //boilerplate stuff

		/// <summary>
		/// This is the circle that will be used to check for 
		/// </summary>
		Circle _circle;
		Rectangle _box;

		#endregion //Properties

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			_circle = new Circle();
			_box = new Rectangle();

			_clock = new GameClock();
			_inputState = new InputState();
			_inputWrapper = new InputWrapper(new ControllerWrapper(0), _clock.GetCurrentTime);
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			//init our box somewhere we can see it
			_box = graphics.GraphicsDevice.Viewport.TitleSafeArea;
			_box.X += 100;
			_box.Width -= 200;
			_box.Y += 100;
			_box.Height -= 200;

			//init the circle so it will be in the middle of the box
			_circle.Initialize(graphics.GraphicsDevice.Viewport.TitleSafeArea.Center.ToVector2(), 80.0f);

			_clock.Start();

			base.Initialize();
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
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
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

			//MOVE THE CIRCLE

			//This is VERY FAST movement to demonstrate that the circle will not tunnel out of the box's walls
			//Set it to like 200 to see the thing actually move
			float movespeed = 20000.0f;

			if (_inputWrapper.Controller.CheckKeystroke(EKeystroke.Up, false, Vector2.UnitX))
			{
				_circle.Translate(0.0f, -movespeed * _clock.TimeDelta);
			}
			else if (_inputWrapper.Controller.CheckKeystroke(EKeystroke.Down, false, Vector2.UnitX))
			{
				_circle.Translate(0.0f, movespeed * _clock.TimeDelta);
			}
			else if (_inputWrapper.Controller.CheckKeystroke(EKeystroke.Forward, false, Vector2.UnitX))
			{
				_circle.Translate(movespeed * _clock.TimeDelta, 0.0f);
			}
			else if (_inputWrapper.Controller.CheckKeystroke(EKeystroke.Back, false, Vector2.UnitX))
			{
				_circle.Translate(-movespeed * _clock.TimeDelta, 0.0f);
			}

			//COLLISION REACTION

			//put the circle back in the box?
			Vector2 overlap = Vector2.Zero;
			Vector2 collisionPoint = Vector2.Zero;
			if (CollisionCheck.CircleRectCollision(_circle, _box, ref collisionPoint, ref overlap))
			{
				//move the circle by the overlap
				_circle.Translate(overlap);
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

			spriteBatch.Begin();

			//draw the circle
			var circlePrim = new Primitive(graphics.GraphicsDevice, spriteBatch);
			circlePrim.Circle(_circle.Pos, _circle.Radius, Color.Red);

			//darw the rectangle
			var rectPrim = new Primitive(graphics.GraphicsDevice, spriteBatch);
			rectPrim.Rectangle(_box, Color.White);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
