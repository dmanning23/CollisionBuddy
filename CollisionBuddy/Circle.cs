using PrimitiveBuddy;
using Microsoft.Xna.Framework;
using System;

namespace CollisionBuddy
{
	/// <summary>
	/// This is a simple circle class... it has a center and a radius, that's about it
	/// </summary>
	public class Circle : ICircle
	{
		#region Properties

		protected Vector2 _position = Vector2.Zero;

		/// <summary>
		/// Update or get the center position of this circle
		/// </summary>
		public Vector2 Pos
		{
			get
			{
				return _position;
			}
			set
			{
				OldPos = _position;
				_position = value;
			}
		}

		/// <summary>
		/// The previous position of this circle
		/// </summary>
		public Vector2 OldPos { get; set; }

		private float _radius = 0.0f;
		private float _radiusSquared = 0.0f;

		/// <summary>
		/// update or get the radius of this circle
		/// </summary>
		public float Radius
		{
			get
			{
				return _radius;
			}
			set
			{
				_radius = value;
				_radiusSquared = value * value;
			}
		}

		/// <summary>
		/// Get the radius squared of this circle
		/// </summary>
		public float RadiusSquared
		{
			get
			{
				return _radiusSquared;
			}
		}

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public Circle()
		{
			OldPos = Vector2.Zero;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="position"></param>
		/// <param name="radius"></param>
		public Circle(Vector2 position, float radius)
		{
			Initialize(position, radius);
		}

		/// <summary>
		/// Set teh initial values of this circle
		/// It's SUPER important that this method is used first, because it sets up both the old and current position
		/// </summary>
		/// <param name="position">the initial position</param>
		/// <param name="radius">the initial radius</param>
		public void Initialize(Vector2 position, float radius)
		{
			_position = position;
			OldPos = position;
			Radius = radius;
		}

		/// <summary>
		/// Set teh initial values of this circle
		/// It's SUPER important that this method is used first, because it sets up both the old and current position
		/// </summary>
		/// <param name="position">the initial position as a point</param>
		/// <param name="radius">the initial radius</param>
		public void Initialize(Point position, float radius)
		{
			Initialize(new Vector2(position.X, position.Y), radius);
		}

		/// <summary>
		/// Get the minimum x distance from this dude to a point
		/// </summary>
		/// <param name="position">position to get the x distance to</param>
		/// <returns>float: the minumum distance from this dudes current circles to the point</returns>
		public float GetXDistance(Vector2 position)
		{
			//subtract point x location from my x location
			float distance = Pos.X - position.X;

			//get abs value of that distance
			distance = (float)Math.Abs(distance);

			//subtract radius
			return distance - Radius;
		}

		/// <summary>
		/// Get the distance from the center of the circle to a point
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public float DistanceToPoint(Vector2 position)
		{
			//get a vector from the point to the center
			Vector2 myVect = position - Pos;

			//compensate for the graphical scaling
			return myVect.Length();
		}

		/// <summary>
		/// move teh circle
		/// </summary>
		/// <param name="x">the amount to move on the x plane</param>
		/// <param name="y">amount to move on the y plane</param>
		public void Translate(float x, float y)
		{
			//update the old position
			OldPos = _position;

			//set teh new postiionh
			_position.X += x;
			_position.Y += y;
		}

		/// <summary>
		/// move teh circle
		/// </summary>
		/// <param name="delta">the amount to move</param>
		public void Translate(Vector2 delta)
		{
			//update the old position
			OldPos = _position;

			//add the change to the position
			_position += delta;
		}

		/// <summary>
		/// draw the circle
		/// </summary>
		/// <param name="prim"></param>
		/// <param name="color"></param>
		public void Draw(IPrimitive prim, Color color)
		{
			prim.Circle(Pos, Radius, color);
		}

		#endregion //Methods
	}
}
