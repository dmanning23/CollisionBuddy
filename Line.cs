using System;
using Microsoft.Xna.Framework;

namespace CollisionBuddy
{
	/// <summary>
	/// A line class... it has a start and end point, and all the properties that entails.
	/// </summary>
	public class Line
	{
		#region Members

		/// <summary>
		/// The start point of this vector
		/// </summary>
		private Vector2 _Start = Vector2.Zero;

		/// <summary>
		/// The end point of this vector
		/// </summary>
		private Vector2 _End = Vector2.Zero;

		#endregion //Members

		#region Properties

		/// <summary>
		/// update or get the start point... 
		/// If you are going to update both the start and end points at the same time, use the Set() method!!!
		/// </summary>
		public Vector2 Start
		{
			get
			{
				return _Start;
			}
			set
			{
				OldStart = _Start;
				_Start = value;
				Updated(); //set the length and direction
			}
		}

		/// <summary>
		/// update or get the start point... 
		/// If you are going to update both the start and end points at the same time, use the Set() method!!!
		/// </summary>
		public Vector2 End
		{
			get
			{
				return _End;
			}
			set
			{
				OldEnd = _End;
				_End = value;
				Updated(); //set the length and direction
			}
		}

		/// <summary>
		/// The last position of the start point
		/// </summary>
		public Vector2 OldStart
		{
			get;
			set;
		}

		/// <summary>
		/// The last position of the end point
		/// </summary>
		public Vector2 OldEnd
		{
			get;
			set;
		}

		/// <summary>
		/// the length of the line, world units
		/// </summary>
		public float Length { get; private set; }

		/// <summary>
		/// unit vector, the direction of the line
		/// </summary>
		public Vector2 Direction { get; private set; }

		#endregion //Properties

		#region Methods

		/// <summary>
		/// Constructor
		/// </summary>
		public Line()
		{
			OldStart = Vector2.Zero;
			OldEnd = Vector2.Zero;
			Length = 0.0f;
			Direction = Vector2.Zero;
		}

		/// <summary>
		/// Set all the values of this line
		/// </summary>
		/// <param name="start">the start point</param>
		/// <param name="end">the end point</param>
		public void Initialize(Vector2 start, Vector2 end)
		{
			OldStart = start;
			_Start = start;
			OldEnd = end;
			_End = end;
			Updated();
		}

		/// <summary>
		/// When the end points get updated, the length and direction need to be updated as well
		/// </summary>
		private void Updated()
		{
			//set teh length and direction
			Direction = End - Start;
			Length = Direction.Length();
			Direction.Normalize();
		}

		#endregion //Methods
	}
}
