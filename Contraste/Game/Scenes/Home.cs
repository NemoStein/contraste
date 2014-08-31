using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests;

namespace Contraste {
	public class Home : Scene {

		Shader lightsShader;
		Shader pixelateShader;

		List<LightEmitter> spots;

		public Home()
			: base(Game.Instance.Width, Game.Instance.Height) {
			Global.Player = new Player();

			OgmoProject ogmoProject;
			ogmoProject = new OgmoProject("Assets/Ogmo/Contraste.oep", "Assets/Ogmo/");
			ogmoProject.UseCameraBounds = false;
			ogmoProject.DisplayGrids = true;
			ogmoProject.RegisterTag((int)Tags.Solid, "Solids");
			ogmoProject.LoadLevel("Assets/Ogmo/0.oel", this);

			Global.World = new World(Game.Instance.Width, Game.Instance.Height);
			Entity SolidsMap = ogmoProject.Entities["Solids"];

			lightsShader = new Shader("Assets/Lights.frag");
			pixelateShader = new Shader("Assets/Pixelate.frag");

			int
				x = -1,
				y = -1;
			while(++x < Game.Instance.Width / Global.TileSide) {
				while(++y < Game.Instance.Height / Global.TileSide) {
					Point3D p = new Point3D(x, y, 0);
					Global.World.MarkPosition(p, (SolidsMap.Collider as GridCollider).GetTile(x, y));
				}
				y = -1;
			}

			Add(Global.Player);
		}

		public override void Begin() {


		}

		private bool ready;

		public override void Update() {

			if(!ready) {
				ready = true;

				AddGraphic(new Image("Assets/Ry.png"));

				Game.Surface.AddShader(lightsShader);
				Game.Surface.AddShader(pixelateShader);

				spots = GetEntities<LightEmitter>();
			}

			for(var i = 0; i < spots.Count; ++i) {
				var spot = spots[i];
				lightsShader.SetParameter("spot" + i, spot.X, 600 - spot.Y, spot.Radius, spot.Intensity);
			}
		}
	}
}
