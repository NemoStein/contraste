using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Contraste {
	public class Light : Entity {
		#region Public Properties
		public int Radius { get; private set; }
		#endregion

		#region Constructor
		public Light(Vector2 position, int radius)
			: base(position.X, position.Y) {
			Radius = radius;

			SetGraphic(Image.CreateRectangle(40, new Color("C0C055")));
			SetCollider(new CircleCollider(Radius, (int)Tags.Light));
			Graphic.CenterOriginZero();

			Collider.OriginX += Radius /2 + Graphic.Width;
			Collider.OriginY += Radius /2 + Graphic.Height;
		}
		#endregion

		#region Otter Overrides
		public override void Render() {
			Collider.Render();
		}
		#endregion

		#region Static Methods
		static public void CreateFromXML(Scene scene, XmlElement e) {
			XmlAttributeCollection attributes = e.Attributes;

			var self = new Light(new Vector2(float.Parse(attributes["x"].Value), float.Parse(attributes["y"].Value)), int.Parse(attributes["Radius"].Value));
			scene.Add(self);
		}
		#endregion
	}
}
