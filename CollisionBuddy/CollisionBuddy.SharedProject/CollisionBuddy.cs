using Microsoft.Xna.Framework;

namespace CollisionBuddy
{
	public static class CollisionCheck
	{
		/// <summary>
		/// Check for collision between two circles
		/// This also checks for tunneling, so that if the circles are going to pass bewteen each other
		/// </summary>
		/// <param name="A">teh first circle</param>
		/// <param name="B">the second circle</param>
		/// <returns>These cricles are either colliding, or going to collide</returns>
		public static bool CircleCircleCollision(Circle A, Circle B)
		{
			// calculate relative velocity and position
			float dvx = (B.Pos.X - B.OldPos.X) - (A.Pos.X - A.OldPos.X);
			float dvy = (B.Pos.Y - B.OldPos.Y) - (A.Pos.Y - A.OldPos.Y);
			float dx = B.OldPos.X - A.OldPos.X;
			float dy = B.OldPos.Y - A.OldPos.Y;

			// check if circles are already colliding
			float sqRadiiSum = A.Radius + B.Radius;
			sqRadiiSum *= sqRadiiSum;
			float pp = dx * dx + dy * dy - sqRadiiSum;
			if (pp < 0)
			{
				return true;
			}

			// check if the circles are moving away from each other and hence can’t collide
			float pv = dx * dvx + dy * dvy;
			if (pv >= 0)
			{
				return false;
			}

			// check if the circles can reach each other between the frames
			float vv = dvx * dvx + dvy * dvy;
			if ((pv + vv) <= 0 && (vv + 2 * pv + pp) >= 0)
			{
				return false;
			}

			// if we've gotten this far then it’s possible for intersection if the distance between
			// the circles is less than the radii sum when it’s at a minimum. Therefore find the time
			// when the distance is at a minimum and test this
			float tmin = -pv / vv;
			return (pp + pv * tmin < 0);
		}

		/// <summary>
		/// given two circles, find the closest points between them.
		/// </summary>
		/// <param name="A">teh first circle</param>
		/// <param name="B">the second circle</param>
		/// <param name="closestPointA">the closet point of circle A to circle B</param>
		/// <param name="closestPointB">the closet point of circle B to circle A</param>
		public static void ClosestPoints(Circle A, Circle B, ref Vector2 closestPointA, ref Vector2 closestPointB)
		{
			//get the direction of the vector between the circles
			Vector2 lineDirection2 = A.Pos - B.Pos;
			lineDirection2.Normalize();
			Vector2 lineDirection1 = new Vector2(-lineDirection2.X, -lineDirection2.Y);

			//get the vector from the circle centers to the collision point
			Vector2 center2collision1 = lineDirection1 * A.Radius;
			Vector2 center2collision2 = lineDirection2 * B.Radius;

			//get the first collision point
			closestPointA = A.Pos + center2collision1;

			//get the second collision point
			closestPointB = B.Pos + center2collision2;
		}

		/// <summary>
		/// given a circle and a line, check if they are colliding and where they are colliding at!
		/// NOTE: this does not check for tunneling!  If the circles velocity is greater than it's radius, it can pass right through the line
		/// </summary>
		/// <param name="A">the circle to check</param>
		/// <param name="B">the line to check</param>
		/// <param name="closestPointA">if a collision is occuring, the closet point on the circle to the line</param>
		/// <param name="closestPointB">if a collision is occuring, the closest point on the line to the circle</param>
		/// <returns>true if the circle and line are colliding, false if not</returns>
		public static bool CircleLineCollision(Circle A, Line B, ref Vector2 closestPointA, ref Vector2 closestPointB)
		{
			//get the vector from the circle center to the start of the line segment
			Vector2 CV = A.Pos - B.Start;

			//project the CV onto the line segment
			float projL = Vector2.Dot(B.Direction, CV);

			if (projL < 0)
			{
				//the closest point on the line is the start point
				float circleDot = Vector2.Dot(CV, CV);
				if (circleDot > A.RadiusSquared)
				{
					//There is no collision
					return false;
				}
				else
				{
					closestPointB = B.Start;

					//get the collision point on the circle
					CV.Normalize();
					Vector2 CenterToEdge = CV * A.Radius;
					closestPointA = A.Pos - CenterToEdge;

					return true;
				}
			}
			else if (projL > B.Length)
			{
				//get the vector of the line segment
				Vector2 lineVect = B.End - B.Start;

				//The second point of the line segment is the closest
				Vector2 CV2 = CV - lineVect;
				float cv2Dot = Vector2.Dot(CV2, CV2);
				if (cv2Dot > A.RadiusSquared)
				{
					//There is no collision
					return false;
				}
				else
				{
					closestPointB = B.End;

					//get the collision point on the circle
					CV = A.Pos - B.End;
					CV.Normalize();
					Vector2 CenterToEdge = CV * A.Radius;
					closestPointA = A.Pos - CenterToEdge;

					return true;
				}
			}
			else
			{
				//The closest point is a midpoint on the line segemnt
				Vector2 VProj = B.Direction * projL;
				Vector2 ClosePoint = B.Start + VProj;

				//if the dot product of the vector from the closest point and itself is less than the radius squared, there is a collision
				float fClosestPoint = Vector2.Dot((ClosePoint - A.Pos), (ClosePoint - A.Pos));
				if (fClosestPoint > A.RadiusSquared)
				{
					return false;
				}
				else
				{
					closestPointB = ClosePoint;

					//get the collision point on the circle
					CV = A.Pos - ClosePoint;
					CV.Normalize();
					Vector2 CenterToEdge = CV * A.Radius;
					closestPointA = A.Pos - CenterToEdge;
					return true;
				}
			}
		}

		/// <summary>
		/// Given a rect and a circle that we assume is in that rect... 
		/// Find if the circle is hitting the wall, where it is hitting the wall, and where to move it to put it back in the box
		/// This function also prevents tunneling, if the circle totally leaves the rect it will pop back in
		/// </summary>
		/// <param name="circle">The circle we are checking</param>
		/// <param name="rectangle">THe rectangle we are putting the circle in</param>
		/// <param name="collisionPoint">If the circle is touching the wall, the point where they are touching</param>
		/// <param name="overlap">A vector that you can add to the circle's position to put it back in the rect</param>
		/// <returns>true if the circle is outsied the edge of the rect, false if it is still all inside</returns>
		public static bool CircleRectCollision(Circle circle, Rectangle rectangle, ref Vector2 collisionPoint, ref Vector2 overlap)
		{
			//Get the delta from last frame to this one
			Vector2 velocity = circle.OldPos - circle.Pos;

			//get the velocity direction if it isn't zero
			var velocityLength = 0.0f;
			if (velocityLength > 0.0f)
			{
				velocity.Normalize();
			}

			//get the bottom of the circle
			var bottom = circle.Pos.Y + circle.Radius;

			//check for fast ground hits
			var fastBottom = bottom + (velocity.Y * velocityLength);
			if (fastBottom > rectangle.Bottom)
			{
				//a floor hit occured

				//get the delta between the current pos and the ground
				overlap.X = 0.0f;
				overlap.Y = (rectangle.Bottom - bottom);

				//get the point where they are colliding
				collisionPoint.X = circle.Pos.X;
				collisionPoint.Y = rectangle.Bottom;
				return true;
			}
			else
			{
				//get the top of the polygon (will be fast bottom plus 2 * radius
				var fastTop = fastBottom - (2.0f * circle.Radius);
				if (fastTop < rectangle.Top)
				{
					//a ceiling hit occured

					//get the top of the polygon
					var top = circle.Pos.Y - circle.Radius;

					//get the delta between the current pos and the ceiling
					overlap.X = 0.0f;
					overlap.Y = -1.0f * (top - rectangle.Top);

					//get the point where they are colliding
					collisionPoint.X = circle.Pos.X;
					collisionPoint.Y = rectangle.Top;
					return true;
				}
			}

			//get the right edge of the polygon
			var right = circle.Pos.X + circle.Radius;

			//check for fast right wall hits
			var fastRight = right + (velocity.X * velocityLength);
			if (fastRight > rectangle.Right)
			{
				//a right wall hit occured

				//get the delta between the current pos and the right wall
				overlap.X = (rectangle.Right - fastRight);
				overlap.Y = 0.0f;

				//get the collision poitn
				collisionPoint.X = rectangle.Right;
				collisionPoint.Y = circle.Pos.Y;
				return true;
			}
			else
			{
				//get the left of the polygon (will be fast right plus 2 * radius
				var fastLeft = fastRight - (2.0f * circle.Radius);
				if (fastLeft < rectangle.Left)
				{
					//a left wall hit occured

					//get the left edge of the polygon
					var left = circle.Pos.X - circle.Radius;
					overlap.X = -1.0f * (left - rectangle.Left);
					overlap.Y = 0.0f;

					//get the collision poitn
					collisionPoint.X = rectangle.Left;
					collisionPoint.Y = circle.Pos.Y;
					return true;
				}
			}

			//no collision occured!
			return false;
		}
	}
}
