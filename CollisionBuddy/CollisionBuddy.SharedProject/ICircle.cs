using Microsoft.Xna.Framework;

namespace CollisionBuddy
{
	public interface ICircle : ICollidable
	{
		Vector2 Pos { get; set; }

		float Radius { get; set; }

		float RadiusSquared { get; }
	}
}
