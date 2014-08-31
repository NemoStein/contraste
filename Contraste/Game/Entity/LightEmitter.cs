using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contraste {
	public class LightEmitter : Actor {
		public float Radius;
		public float Intensity = 1f;

		public LightEmitter(float x, float y, Graphic graphic = null, Collider collider = null)
			: base(x, y, graphic, collider) {
		}

		public LightEmitter(Vector2 position, Graphic graphic = null, Collider collider = null)
			: this(position.X, position.Y, graphic, collider) {
		}
	}
}
