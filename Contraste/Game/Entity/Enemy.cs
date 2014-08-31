using Otter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests;

namespace Contraste {
	public class Enemy : Actor {
		
		#region Public Properties
		public SearchNode PathNode { get; protected set; }
		#endregion

		#region Constructor
		public Enemy(Vector2 position)
			: base(position) {

		}
		#endregion

		#region Protected Methods
		virtual protected SearchNode FindPath() {
			return PathFinder.FindPath(Global.World, new Point3D((int)X / Global.TileSide, (int)Y / Global.TileSide, 0), new Point3D((int)(Global.Player.Collider.CenterX) / Global.TileSide, (int)(Global.Player.Collider.CenterY) / Global.TileSide, 0));
		}

		virtual protected float DistanceFromPlayer(float range) {
			float
				playerCenterX = Global.Player.Collider.CenterX,
				playerCenterY = Global.Player.Collider.CenterY,
				centerX = Collider.CenterX,
				centerY = Collider.CenterY;

				return Util.Distance(centerX, centerY, playerCenterX, playerCenterY);
		}
		#endregion
	}
}
