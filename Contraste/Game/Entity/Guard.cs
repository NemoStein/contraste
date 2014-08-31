using Otter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Tests;

namespace Contraste {
	public class Guard : Enemy {
		#region Public Fields
		public float PatrollingSpeed = 60f;
		public float SeekingSpeed = 90f;
		public float AlertSpeed = 80f;

		public enum States {
			Patrolling,
			Seeking,
			Alert
		}
		#endregion

		#region Public Properties
		public List<LineCollider> RayCasts { get; private set; }
		public List<Vector2> Positions { get; private set; }
		#endregion

		#region Private Fields
		float angle;
		float precision;
		float range;
		float step;
		float spam;

		float lookAt;

		Vector2 lastSight;
		CircleCollider light;
		#endregion

		#region Constructor
		public Guard(List<Vector2> positions)
			: base(positions[0]) {
			SetGraphic(Image.CreateCircle(20, new Color("CC5555")));
			SetHitbox(20, 20, (int)Tags.Enemy);

			Graphic.CenterOriginZero();
			Collider.OriginX = -Collider.HalfWidth;
			Collider.OriginY = -Collider.HalfHeight;

			Radius = 100;

			light = new CircleCollider((int)Radius, (int)Tags.Light);
			light.OriginX = (Radius / 2) + Graphic.Width - Collider.HalfWidth;
			light.OriginY = (Radius / 2) + Graphic.Height - Collider.HalfHeight;
			AddCollider(light);

			Positions = positions;

			Heart = new Heart(1);
			StateMachineCoroutine = new StateMachineCoroutine();
			Movement = new BasicMovement(0, 0, 0);
			Movement.Axis = new Axis();
			Movement.Collider = Collider;
			Movement.AddCollision((int)Tags.Solid);

			RayCasts = new List<LineCollider>();

			angle = 30f;
			precision = 1f;
			range = 900f;
			step = Util.DEG_TO_RAD / precision;
			spam = angle * precision;

			int i = -1;
			while(++i < spam) {
				RayCasts.Add(new LineCollider(1, 1, 2, 2));
				AddCollider(RayCasts[i]);
			}

			AddComponents(Heart, StateMachineCoroutine, Movement);

			StateMachineCoroutine.Populate<States>();
			StateMachineCoroutine.ChangeState(States.Patrolling);
		}
		#endregion

		#region Overrides
		override public void Update() {
			int i = -1;
			while(++i < spam) {
				RayCasts[i].X = -Collider.OriginX + Collider.HalfWidth;
				RayCasts[i].Y = -Collider.OriginY + Collider.HalfHeight;
				RayCasts[i].X2 = (float)Math.Cos(step * i + lookAt) * range + RayCasts[i].X;
				RayCasts[i].Y2 = (float)Math.Sin(step * i + lookAt) * range + RayCasts[i].Y;

				var c = RayCasts[i].Collide(X, Y, (int)Tags.Solid, (int)Tags.Player);
				if(c != null && c.Entity is Player) {
					if((c.Entity as Player).StateMachineCoroutine.Current == "Light" && StateMachineCoroutine.Current != "Seeking")
						StateMachineCoroutine.ChangeState(States.Seeking);
				}
			}
		}

		public override void Render() {
			foreach(Collider c in Colliders)
				if(c != null)
					c.Render();
		}
		#endregion

		#region Coroutines
		private IEnumerator Rotate() {
			float start = lookAt + spam / 2 * Util.DEG_TO_RAD;
			float target = (lookAt + spam / 2 * Util.DEG_TO_RAD + (float)Math.PI);

			while(true) {
				yield return StateMachineCoroutine.WaitForFrames(80);
				Look((float)Math.Cos(target), (float)Math.Sin(target));
				yield return StateMachineCoroutine.WaitForFrames(80);
				Look((float)Math.Cos(start), (float)Math.Sin(start));
				yield return StateMachineCoroutine.WaitForFrames(80);
				StateMachineCoroutine.ChangeState(States.Patrolling);
			}
		}

		private IEnumerator FollowPath() {
			Vector2 node;
			int index = -1;
			while(++index < Positions.Count) {
				node = Positions[index];
				while(new Vector2(Util.SnapToGrid(X, Global.TileSide), Util.SnapToGrid(Y, Global.TileSide)) != node) {
					Movement.Axis.ForceState(node.X - X, node.Y - Y);
					Look(node.X - X, node.Y - Y, true);
					yield return 0;
				}
			}
		}
		#endregion

		#region States
		public IEnumerator Patrolling() {
			Movement.Speed.Max = Movement.Accel = PatrollingSpeed;
			while(true) {
				yield return FollowPath();
				Positions.Reverse();
				yield return StateMachineCoroutine.WaitForFrames(25);
			}
		}

		public IEnumerator Seeking() {
			Movement.Speed.Max = Movement.Accel = SeekingSpeed;
			while(true) {
				PathNode = FindPath();
				if(Global.Player.StateMachineCoroutine.Current == "Dark" || PathNode == null || PathNode.next == null) {
					lastSight = new Vector2(Util.SnapToGrid(Global.Player.X, Global.TileSide), Util.SnapToGrid(Global.Player.Y, Global.TileSide));
					StateMachineCoroutine.ChangeState(States.Alert);
				} else {
					Movement.Axis.ForceState(PathNode.next.position.X * Global.TileSide - X, PathNode.next.position.Y * Global.TileSide - Y);
					Look(Global.Player.X - X, Global.Player.Y - Y, false);
					var c = Hitbox.Collide(X, Y, (int)Tags.Player);
					if(c != null) {
						(c.Entity as Player).Heart.TakeDamage(1);
					}
				}
				yield return 0;
			}
		}

		public IEnumerator Alert() {
			Movement.Axis.ForceState(0, 0);
			Movement.Speed.Max = Movement.Accel = AlertSpeed;

			do {
				PathNode = FindLastSight();
				Movement.Axis.ForceState(PathNode.next.position.X * Global.TileSide - X, PathNode.next.position.Y * Global.TileSide - Y);
				Look(PathNode.next.position.X * Global.TileSide - X, PathNode.next.position.X * Global.TileSide - Y, false);

				yield return 0;
			}
			while(Movement.Axis.X != 0 || Movement.Axis.Y != 0);

			Movement.Axis.ForceState(0, 0);
			yield return Rotate();
		}
		#endregion

		#region Private Methods
		void Look(float x, float y, bool instant = true) {
			if(instant)
				lookAt = (float)Math.Atan2(y, x) - spam / 2 * Util.DEG_TO_RAD;
			else {
				float diff = (float)Math.Atan2(y, x) - spam / 2 * Util.DEG_TO_RAD - lookAt;
				if(diff > Math.PI)
					diff -= 2 * (float)Math.PI;
				else if(diff < -Math.PI)
					diff += 2 * (float)Math.PI;

				lookAt = Util.Approach(lookAt, lookAt + diff, 1 * Util.DEG_TO_RAD);
			}
		}

		void Look(Vector2 position, bool instant = true) {
			Look(position.X, position.Y, instant);
		}

		SearchNode FindLastSight() {
			return PathFinder.FindPath(Global.World, new Point3D((int)X / Global.TileSide, (int)Y / Global.TileSide, 0), new Point3D((int)(lastSight.X) / Global.TileSide, (int)(lastSight.Y) / Global.TileSide, 0));
		}
		#endregion

		#region Static Methods
		static public void CreateFromXML(Scene scene, XmlElement e) {
			XmlAttributeCollection attributes = e.Attributes;

			List<Vector2> positions = new List<Vector2>();
			positions.Add(new Vector2(float.Parse(attributes["x"].Value), float.Parse(attributes["y"].Value)));
			foreach(XmlNode node in e) {
				positions.Add(new Vector2(float.Parse(node.Attributes["x"].Value), float.Parse(node.Attributes["y"].Value)));
			}

			XmlAttributeCollection att = e.Attributes;
			var self = new Guard(positions);
			scene.Add(self);
		}
		#endregion
	}
}
