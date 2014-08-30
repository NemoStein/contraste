using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contraste {
	public class Home : Scene {
		public Home()
			: base(Game.Instance.Width, Game.Instance.Height) {
			Player player = new Player();
			Add(player);

			Surface.AddShader(new Shader("Assets/Test.frag"));
		}
	}
}
