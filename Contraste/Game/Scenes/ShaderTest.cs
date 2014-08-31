using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contraste {
	public class ShaderTest : Scene {

		Shader lightsShader;
		Shader pixelateShader;

		public ShaderTest()
			: base(Game.Instance.Width, Game.Instance.Height) {
			Player player = new Player();
			Add(player);
		}

		public override void Begin()
		{
			AddGraphic(new Image("Assets/Ry.png"));

			lightsShader = new Shader("Assets/Lights.frag");
			pixelateShader = new Shader("Assets/Pixelate.frag");

			Game.Surface.AddShader(lightsShader);
			Game.Surface.AddShader(pixelateShader);

			//lightsShader.SetParameter("spot1", 100f, 500f, 50f, 1.0f);
			//lightsShader.SetParameter("spot2", 200f, 200f, 100f, 1.0f);
			//lightsShader.SetParameter("spot3", 300f, 500f, 50f, 1.0f);
			//lightsShader.SetParameter("spot4", 500f, 350f, 150f, 1.0f);
		}

		public override void Update()
		{
			lightsShader.SetParameter("light", (float)Math.Sin(Timer / 100));
			lightsShader.SetParameter("spot" + Rand.Int(1, 32), Rand.Float(800f), Rand.Float(600f), Rand.Float(100f), Rand.Float(0.5f, 1.5f));

			/*
			if (Timer % 30 == 0)
			{
				try
				{
					
				}
				catch (Exception exception)
				{

				}
			}
			*/
		}
	}
}
