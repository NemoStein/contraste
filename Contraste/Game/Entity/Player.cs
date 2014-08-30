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
		public float Speed = 100f;

		public enum States {
			Free
		}
		#endregion
		public Player()
			: base(0, 0) {
			SetGraphic(Image.CreateRectangle(40, 50, Color.Gray));
			Graphic.CenterOriginZero();

			SetHitbox(40, 50, (int)Tags.Player);

			Heart = new Heart(1);
			StateMachineCoroutine = new StateMachineCoroutine();
			Movement = new BasicMovement(Speed, Speed, Speed);
			Movement.Axis = Global.Session.Controller.AxisLeft;

			AddComponents(Heart, StateMachineCoroutine, Movement);

			StateMachineCoroutine.Populate<States>();
			StateMachineCoroutine.ChangeState(States.Free);
		}

		#region Otter Override
		override public void Update() {
		}
		#endregion

		#region States
		public IEnumerator Free() {
			while(true) {
				Graphic.Angle = Util.Angle(new Vector2(X + Graphic.HalfWidth, Y + Graphic.HalfHeight), new Vector2(Input.MouseScreenX, Input.MouseScreenY)) - 90;
				yield return 0;
			}
		}
		#endregion
	}
}
