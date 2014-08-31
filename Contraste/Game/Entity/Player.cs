using Otter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contraste {
	public class Player : Actor {
		#region Public Fields
		public float LightSpeed = 400f;
		public float DarkSpeed = 100f;

		public float LightVisionRadius = 500f;
		public float RadkVisionRadius = 150f;

		public enum States {
			Light,
			Dark
		}
		#endregion

		#region Constructor
		public Player()
			: base(0, 0) {
			SetGraphic(Image.CreateRectangle(40, 50, Color.Gray));
			Graphic.CenterOriginZero();

			SetHitbox(40, 50, (int)Tags.Player);

			Heart = new Heart(1);
			Heart.OnDead = RemoveSelf;
			StateMachineCoroutine = new StateMachineCoroutine();
			Movement = new BasicMovement(0, 0, 0);
			Movement.Axis = Global.Session.Controller.AxisLeft;
			Movement.Collider = Collider;
			Movement.AddCollision((int)Tags.Solid);

			AddComponents(Heart, StateMachineCoroutine, Movement);

			StateMachineCoroutine.Populate<States>();
			StateMachineCoroutine.ChangeState(States.Dark);
		}
		#endregion

		#region Otter Override
		override public void Update() {
			//Graphic.Angle = Util.Angle(new Vector2(X + Graphic.HalfWidth, Y + Graphic.HalfHeight), new Vector2(Input.MouseScreenX, Input.MouseScreenY)) - 90;

			if(Overlap(X, Y, (int)Tags.Light))
				StateMachineCoroutine.ChangeState(States.Light);
			else
				StateMachineCoroutine.ChangeState(States.Dark);
		}

		public override void Render() {
			Collider.Render();
		}
		#endregion

		#region States
		public IEnumerator Light() {
			Movement.Speed.Max = Movement.Accel = LightSpeed;
			yield return 0;
		}

		public IEnumerator Dark() {
			Movement.Speed.Max = Movement.Accel = DarkSpeed;
			yield return 0;
		}
		#endregion
	}
}
