using System;
using Microsoft.Xna.Framework;

namespace CollisionBuddy
{
	/// <summary>
	/// This is a simple circle class... it has a center and a radius, that's about it
	/// </summary>
	public class Circle
	{
		#region Members

		/// <summary>
		/// the radius of the circle
		/// </summary>
		private float _fRadius = 0.0f;

		/// <summary>
		/// the radius squared of the circle
		/// </summary>
		private float _fRadiusSquared = 0.0f;

		/// <summary>
		/// The center point of this circle
		/// </summary>
		private Vector2 _Position = Vector2.Zero;

		#endregion //Members

		#region Properties

		/// <summary>
		/// Update or get the position of this circle
		/// </summary>
		public Vector2 Pos
		{
			get
			{
				return _Position;
			}
			set
			{
				OldPos = _Position;
				_Position = value;
			}
		}

		/// <summary>
		/// The previous position of this circle
		/// </summary>
		public Vector2 OldPos { get; set; }

		/// <summary>
		/// update or get the radius of this circle
		/// </summary>
		public float Radius
		{
			get
			{
				return _fRadius;
			}
			set
			{
				_fRadius = value;
				_fRadiusSquared = value * value;
			}
		}

		/// <summary>
		/// Get the radius squared of this circle
		/// </summary>
		public float RadiusSquared
		{
			get
			{
				return _fRadiusSquared;
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
		/// Set teh initial values of this circle
		/// </summary>
		/// <param name="position"></param>
		/// <param name="radius"></param>
		public void Initialize(Vector2 position, float radius)
		{
			_Position = position;
			OldPos = position;
			Radius = radius;
		}

		/// <summary>
		/// Get the minimum x distance from this dude to a point
		/// </summary>
		/// <param name="position">position to get the x distance to</param>
		/// <returns>float: the minumum distance from this dudes current circles to the point</returns>
		public float GetXDistance(Vector2 position)
		{
			//subtract point x location from my x location
			float fDistance = Pos.X - position.X;

			//get abs value of that distance
			fDistance = Math.Abs(fDistance);

			//subtract radius
			return fDistance - Radius;
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
			_Position.X += x;
			_Position.Y += y;
		}

		/// <summary>
		/// move teh circle
		/// </summary>
		/// <param name="delta">the amount to move</param>
		public void Translate(Vector2 delta)
		{
			_Position += delta;
		}

		#endregion //Methods
	}
}
