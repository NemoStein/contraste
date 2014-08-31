using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contraste {
	public class Actor : Entity {
		#region Public Properties
		public Heart Heart { get; protected set; }
		public StateMachineCoroutine StateMachineCoroutine { get; protected set; }
		public BasicMovement Movement { get; protected set; }
		#endregion

		#region Public Fields
		public bool AutoLayer = true;
		#endregion

		#region Constructor
		public Actor(float x, float y, Graphic graphic = null, Collider collider = null)
			: base(x, y, graphic, collider) {
		}

		public Actor(Vector2 position, Graphic graphic = null, Collider collider = null)
			: this(position.X, position.Y, graphic, collider) {
		}
		#endregion

		#region Otter Overrides
		override public void UpdateLast() {
			if(AutoLayer)
				Layer = -(int)Y;
		}
		#endregion
	}
}
