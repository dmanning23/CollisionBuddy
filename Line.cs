using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollisionBuddy
{
	public class Line
	{
		Vector2 Start
		{
			get;
			set;
		}

		Vector2 End
		{
			get;
			set;
		}

		Vector2 OldStart
		{
			get;
			set;
		}

		Vector2 OldEnd
		{
			get;
			set;
		}
	}
}
