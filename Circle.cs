using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollisionBuddy
{
	public class Circle
	{
		#region Members

		/// <summary>
		/// the radius of the circle
		/// </summary>
		private float _fRadius;

		/// <summary>
		/// the radius squared of the circle
		/// </summary>
		private float _fRadiusSquared;

		Vector2 Pos { get; set; }

		Vector2 OldPos { get; set; }

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

		public float RadiusSquared
		{
			get
			{
				return _fRadiusSquared;
			}
		}

		#endregion //Members

	}
}
