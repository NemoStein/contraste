using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests;

namespace Contraste {
	class Global {

		static public Session Session;
		static public Player Player;
		static public World World;

		static public int TileSide = 40;
	}

	public enum Tags {
		Player,
		Solid,
		Enemy,
		Light
	}
}
