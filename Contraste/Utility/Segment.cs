using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
	public class Segment
	{
		public Vector2 pointA { get; private set; }
		public Vector2 pointB { get; private set; }
		private float slope;
		private float intercept;

		public Segment(Vector2 pointA, Vector2 pointB)
		{
			this.pointA = pointA;
			this.pointB = pointB;

			update();
		}

		public void update()
		{
			slope = (pointA.Y - pointB.Y) / (pointA.X - pointB.X);
			intercept = pointA.Y - slope * pointA.X;
		}

		static public bool intersection(Segment segment1, Segment segment2, Vector2 point)
		{
			if (segment1.pointA.X == segment1.pointB.X)
			{
				point.X = segment1.pointA.X;
				point.Y = segment2.slope * segment1.pointA.X + segment2.intercept;
			}
			else if (segment2.pointA.X == segment2.pointB.X)
			{
				point.X = segment2.pointA.X;
				point.Y = segment1.slope * segment2.pointA.X + segment1.intercept;
			}
			else
			{
				point.X = (segment2.intercept - segment1.intercept) / (segment1.slope - segment2.slope);
				point.Y = segment1.slope * point.X + segment1.intercept;
			}

			if ((segment1.pointA.X < segment1.pointB.X && (point.X < segment1.pointA.X || point.X > segment1.pointB.X)) ||
				(segment1.pointA.X > segment1.pointB.X && (point.X > segment1.pointA.X || point.X < segment1.pointB.X)) ||
				(segment2.pointA.X < segment2.pointB.X && (point.X < segment2.pointA.X || point.X > segment2.pointB.X)) ||
				(segment2.pointA.X > segment2.pointB.X && (point.X > segment2.pointA.X || point.X < segment2.pointB.X)) ||
				(segment1.pointA.Y < segment1.pointB.Y && (point.Y < segment1.pointA.Y || point.Y > segment1.pointB.Y)) ||
				(segment1.pointA.Y > segment1.pointB.Y && (point.Y > segment1.pointA.Y || point.Y < segment1.pointB.Y)) ||
				(segment2.pointA.Y < segment2.pointB.Y && (point.Y < segment2.pointA.Y || point.Y > segment2.pointB.Y)) ||
				(segment2.pointA.Y > segment2.pointB.Y && (point.Y > segment2.pointA.Y || point.Y < segment2.pointB.Y)))
			{
				return false;
			}

			return true;
		}
	}
}
