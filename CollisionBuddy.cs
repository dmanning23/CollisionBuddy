using System;
using System.Collections.Generic;
using System.Text;

namespace CollisionBuddy
{
	static class CollisionBuddy
	{
		public static bool CircleCircleCollision(Circle A, Circle B)
		{
			// calculate relative velocity and position
			float dvx = (B.Pos.X - B.OldPos.X) - (A.Pos.X - A.OldPos.X);
			float dvy = (B.Pos.Y - B.OldPos.Y) - (A.Pos.Y - A.OldPos.Y);
			float dx = B.OldPos.X - A.OldPos.X;
			float dy = B.OldPos.Y - A.OldPos.Y;

			// check if circles are already colliding
			float sqRadiiSum = A.RadiusSquared + B.RadiusSquared;
			float pp = dx*dx + dy*dy - sqRadiiSum;
			if(pp < 0) 
			{
				return true; 
			}

			// check if the circles are moving away from each other and hence can’t collide
			float pv = dx*dvx + dy*dvy;
			if(pv >= 0) 
			{
				return false; 
			}

			// check if the circles can reach each other between the frames
			float vv = dvx*dvx + dvy*dvy;
			if((pv + vv) <= 0 && (vv + 2*pv + pp) >= 0) 
			{
				return false; 
			}

			// if we've gotten this far then it’s possible for intersection if the distance between
			// the circles is less than the radii sum when it’s at a minimum. Therefore find the time
			// when the distance is at a minimum and test this
			float tmin = -pv / vv;
			return (pp + pv*tmin > 0);
		}

		/// <summary>
		/// check if a circle is colliding and get the collision points between circles
		/// </summary>
		//given two circles, find the closest points between them.
		public void ClosestPoints(Circle A, Circle B, ref Vector2 ClosestPointA, ref Vector2 ClosestPointB)
		{

				//get the direction of the vector between the circles
				Vector2 lineDirection2 = m_WorldPosition - B.m_WorldPosition;
				lineDirection2.Normalize();
				Vector2 lineDirection1 = new Vector2(-lineDirection2.X, -lineDirection2.Y);

				//get the vector from the circle centers to the collision point
				Vector2 center2collision1 = lineDirection1 * WorldRadius;
				Vector2 center2collision2 = lineDirection2 * B.WorldRadius;

				//get the first collision point
				MyPoint = m_WorldPosition + center2collision1;

				//get the second collision point
				HisPoint = B.m_WorldPosition + center2collision2;

				return true;
			}
		}

		//given a circle and a line, check if they are colliding and where they are colliding at!
	//NOTE: this does not check for tunneling!  If the circles velocity is greater than it's radius, it can pass right through the line
		public bool IsColliding(Circle A, Line bool, ref Vector2 ClosestPointA, ref Vector2 ClosestPointB)
		{
			//get the vector of the line segment
			Vector2 lineVect = End - Start;

			//get the vector from the circle center to the start of the line segment
			Vector2 CV = rCircle.WorldPosition - Start;

			//project the CV onto the line segment
			float ProjL = Vector2.Dot(Direction, CV);

			if (ProjL < 0)
			{
				//the closest point on the line is the start point
				float fCirlclDot = Vector2.Dot(CV, CV);
				if (fCirlclDot > rCircle.WorldRadiusSquared)
				{
					//There is no collision
					return false;
				}
				else
				{
					MyPoint = Start;

					//get the collision point on the circle
					CV.Normalize();
					Vector2 CenterToEdge = CV * rCircle.WorldRadius;
					HisPoint = rCircle.WorldPosition - CenterToEdge;

					return true;
				}
			}
			else if (ProjL > Length)
			{
				//The second point of the line segment is the closest
				Vector2 CV2 = CV - lineVect;
				float cv2Dot = Vector2.Dot(CV2, CV2);
				if (cv2Dot > rCircle.WorldRadiusSquared)
				{
					//There is no collision
					return false;
				}
				else
				{
					MyPoint = End;

					//get the collision point on the circle
					CV = rCircle.WorldPosition - End;
					CV.Normalize();
					Vector2 CenterToEdge = CV * rCircle.WorldRadius;
					HisPoint = rCircle.WorldPosition - CenterToEdge;

					return true;
				}
			}
			else
			{
				//The closest point is a midpoint on the line segemnt
				Vector2 VProj = Direction * ProjL;
				Vector2 ClosePoint = Start + VProj;

				//if the dot product of the vector from the closest point and itself is less than the radius squared, there is a collision
				float fClosestPoint = Vector2.Dot((ClosePoint - rCircle.WorldPosition), (ClosePoint - rCircle.WorldPosition));
				if (fClosestPoint > rCircle.WorldRadiusSquared)
				{
					return false;
				}
				else
				{
					MyPoint = ClosePoint;

					//get the collision point on the circle
					CV = rCircle.WorldPosition - ClosePoint;
					CV.Normalize();
					Vector2 CenterToEdge = CV * rCircle.WorldRadius;
					HisPoint = rCircle.WorldPosition - CenterToEdge;
					return true;
				}
			}
		}

	//TODO: put the cirlce in a box and check if it tries to leave!
	}
}
