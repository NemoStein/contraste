using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contraste {
	class Program {
		static void Main(string[] args) {
			Game game = new Game("Contraste", 800, 600, 60);
			game.SetWindow(800, 600);
			game.LockAspectRatio = true;
			game.Color = new Color("252525");
			game.MouseVisible = true;

			Global.Session = Game.Instance.AddSession("p1");
			Global.Session.Controller = Controller.Get360Controller(0);

			Global.Session.Controller.Up.AddKey(Key.W);
			Global.Session.Controller.Right.AddKey(Key.D);
			Global.Session.Controller.Down.AddKey(Key.S);
			Global.Session.Controller.Left.AddKey(Key.A);

			Global.Session.Controller.AxisLeft.AddKeys(Key.W, Key.D, Key.S, Key.A);
			Global.Session.Controller.A.AddKey(Key.Z);

			game.Start(new Home());
		}
	}
}
