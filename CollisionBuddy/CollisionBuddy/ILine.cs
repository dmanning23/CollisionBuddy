using Microsoft.Xna.Framework;

namespace CollisionBuddy
{
	public interface ILine : ICollidable
	{
		Vector2 Start { get; set; }

		Vector2 End { get; set; }

		float Length { get; }

		Vector2 Direction { get; }

		Vector2 Normal { get; }
    }
}
