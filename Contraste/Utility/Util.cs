using Otter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Utility {
	public class Util {
		static public Spritemap<string> LoadSpritemap<T>(string xmlPath, string spriteName) {
			Spritemap<string> spritemap = null;

			XmlDocument document = new XmlDocument();
			document.Load(xmlPath);

			XmlNodeList sprites = document.GetElementsByTagName("Sprite");
			foreach(XmlElement sprite in sprites) {
				if(sprite.AttributeString("Name") == spriteName) {
					spritemap = new Spritemap<string>(sprite.AttributeString("Texture"), sprite["Size"].AttributeInt("Width"), sprite["Size"].AttributeInt("Height"));
					if(sprite["Origin"].AttributeBool("Center")) {
						spritemap.CenterOrigin();
					} else if(sprite["Origin"].AttributeBool("CenterZero")) {
						spritemap.CenterOriginZero();
					} else {
						spritemap.OriginX = sprite["Origin"].AttributeInt("X");
						spritemap.OriginY = sprite["Origin"].AttributeInt("Y");
					}
					spritemap.Color = new Color(sprite["Color"]);

					foreach(XmlElement a in sprite["Animations"].ChildNodes) {
						Anim animation = new Anim(a.AttributeString("Frames"), a.AttributeString("Delay"), ",");
						if(!a.AttributeBool("Loop")) {
							animation.LoopBackTo(animation.FrameCount - 1);
						}
						spritemap.Add(a.AttributeString("Name"), animation);
					}
				}
			}

			return spritemap;
		}

		static public Tilemap AutoTile(GridCollider grid, string xmlPath, Tilemap tilemap) {
			XmlDocument document = new XmlDocument();
			document.Load(xmlPath);

			SortedDictionary<int, List<TileMaksInfo>> tiles = new SortedDictionary<int, List<TileMaksInfo>>();

			foreach(XmlElement set in document.GetElementsByTagName("Set")) {
				int key = (int)Enum.Parse(typeof(Tile), set.AttributeString("Mask"));
				List<TileMaksInfo> tilesMaskList = new List<TileMaksInfo>();
				foreach(XmlElement tile in set.ChildNodes) {
					tilesMaskList.Add(new TileMaksInfo(tile.AttributeInt("X"), tile.AttributeInt("Y"), tile.AttributeFloat("Weight")));
				}
				tiles.Add(key, tilesMaskList);
			}


			int
				x = -1,
				y = -1;
			while(++x < grid.TileColumns) {
				while(++y < grid.TileRows) {
					if(grid.GetTile(x, y)) {
						int
							filled =
							(y > 0 ? grid.GetTile(x, y - 1) ? (int)Neighbors.Top : 0 : (int)Neighbors.Top) |
							(x < grid.TileColumns - 1 && y > 0 ? grid.GetTile(x + 1, y - 1) ? (int)Neighbors.TopRight : 0 : (int)Neighbors.TopRight) |
							(x < grid.TileColumns - 1 ? grid.GetTile(x + 1, y) ? (int)Neighbors.Right : 0 : (int)Neighbors.Right) |
							(x < grid.TileColumns - 1 && y < grid.TileRows - 1 ? grid.GetTile(x + 1, y + 1) ? (int)Neighbors.BottomRight : 0 : (int)Neighbors.BottomRight) |
							(y < grid.TileRows - 1 ? grid.GetTile(x, y + 1) ? (int)Neighbors.Bottom : 0 : (int)Neighbors.Bottom) |
							(x > 0 && y < grid.TileRows - 1 ? grid.GetTile(x - 1, y + 1) ? (int)Neighbors.BottomLeft : 0 : (int)Neighbors.BottomLeft) |
							(x > 0 ? grid.GetTile(x - 1, y) ? (int)Neighbors.Left : 0 : (int)Neighbors.Left) |
							(x > 0 && y > 0 ? grid.GetTile(x - 1, y - 1) ? (int)Neighbors.TopLeft : 0 : (int)Neighbors.TopLeft);

						int set = (int)Tile.Single;
						if((filled | (int)CasesOptional.Top) == ((int)CasesFilled.Top | (int)CasesOptional.Top)) {
							set = (int)Tile.Top;
						} else if((filled | (int)CasesOptional.TopRight) == ((int)CasesFilled.TopRight | (int)CasesOptional.TopRight)) {
							set = (int)Tile.TopRight;
						} else if((filled | (int)CasesOptional.Right) == ((int)CasesFilled.Right | (int)CasesOptional.Right)) {
							set = (int)Tile.Right;
						} else if((filled | (int)CasesOptional.BottomRight) == ((int)CasesFilled.BottomRight | (int)CasesOptional.BottomRight)) {
							set = (int)Tile.BottomRight;
						} else if((filled | (int)CasesOptional.Bottom) == ((int)CasesFilled.Bottom | (int)CasesOptional.Bottom)) {
							set = (int)Tile.Bottom;
						} else if((filled | (int)CasesOptional.BottomLeft) == ((int)CasesFilled.BottomLeft | (int)CasesOptional.BottomLeft)) {
							set = (int)Tile.BottomLeft;
						} else if((filled | (int)CasesOptional.Left) == ((int)CasesFilled.Left | (int)CasesOptional.Left)) {
							set = (int)Tile.Left;
						} else if((filled | (int)CasesOptional.TopLeft) == ((int)CasesFilled.TopLeft | (int)CasesOptional.TopLeft)) {
							set = (int)Tile.TopLeft;
						} else if((filled | (int)CasesOptional.Center) == ((int)CasesFilled.Center | (int)CasesOptional.Center)) {
							set = (int)Tile.Center;
						} else if((filled | (int)CasesOptional.VerticalTop) == ((int)CasesFilled.VerticalTop | (int)CasesOptional.VerticalTop)) {
							set = (int)Tile.VerticalTop;
						} else if((filled | (int)CasesOptional.VerticalCenter) == ((int)CasesFilled.VerticalCenter | (int)CasesOptional.VerticalCenter)) {
							set = (int)Tile.VerticalCenter;
						} else if((filled | (int)CasesOptional.VerticalBottom) == ((int)CasesFilled.VerticalBottom | (int)CasesOptional.VerticalBottom)) {
							set = (int)Tile.VerticalBottom;
						} else if((filled | (int)CasesOptional.HorizontalRight) == ((int)CasesFilled.HorizontalRight | (int)CasesOptional.HorizontalRight)) {
							set = (int)Tile.HorizontalRight;
						} else if((filled | (int)CasesOptional.HorizontalCenter) == ((int)CasesFilled.HorizontalCenter | (int)CasesOptional.HorizontalCenter)) {
							set = (int)Tile.HorizontalCenter;
						} else if((filled | (int)CasesOptional.HorizontalLeft) == ((int)CasesFilled.HorizontalLeft | (int)CasesOptional.HorizontalLeft)) {
							set = (int)Tile.HorizontalLeft;
						} else if((filled | (int)CasesOptional.InnerCornerTopRight) == ((int)CasesFilled.InnerCornerTopRight | (int)CasesOptional.InnerCornerTopRight)) {
							set = (int)Tile.InnerCornerTopRight;
						} else if((filled | (int)CasesOptional.InnerCornerBottomRight) == ((int)CasesFilled.InnerCornerBottomRight | (int)CasesOptional.InnerCornerBottomRight)) {
							set = (int)Tile.InnerCornerBottomRight;
						} else if((filled | (int)CasesOptional.InnerCornerBottomLeft) == ((int)CasesFilled.InnerCornerBottomLeft | (int)CasesOptional.InnerCornerBottomLeft)) {
							set = (int)Tile.InnerCornerBottomLeft;
						} else if((filled | (int)CasesOptional.InnerCornerTopLeft) == ((int)CasesFilled.InnerCornerTopLeft | (int)CasesOptional.InnerCornerTopLeft)) {
							set = (int)Tile.InnerCornerTopLeft;
						} else if((filled | (int)CasesOptional.Single) == ((int)CasesFilled.Single | (int)CasesOptional.Single)) {
							set = (int)Tile.Single;
						}

						float percent = Rand.Float(1);
						int index = 0;
						foreach(TileMaksInfo info in tiles[set]) {
							if(info.Weight >= percent) {
								index = tiles[set].IndexOf(info);
							}
						}

						tilemap.SetTile(x * tilemap.TileWidth, y * tilemap.TileWidth, tiles[set][index].X * tilemap.TileWidth, tiles[set][index].Y * tilemap.TileHeight);

					} else {
						tilemap.SetTile(x, y, 0, 0);
					}
				}
				y = -1;
			}

			return tilemap;
		}

		[Flags]
		private enum Neighbors {
			Top = 1,
			TopRight = 2,
			Right = 4,
			BottomRight = 8,
			Bottom = 16,
			BottomLeft = 32,
			Left = 64,
			TopLeft = 128
		}

		[Flags]
		private enum CasesFilled {
			Top = (int)Neighbors.Right | (int)Neighbors.Bottom | (int)Neighbors.Left,
			TopRight = (int)Neighbors.Bottom | (int)Neighbors.Left,
			Right = (int)Neighbors.Top | (int)Neighbors.Left | (int)Neighbors.Bottom,
			BottomRight = (int)Neighbors.Top | (int)Neighbors.Left,
			Bottom = (int)Neighbors.Top | (int)Neighbors.Right | (int)Neighbors.Left,
			BottomLeft = (int)Neighbors.Top | (int)Neighbors.Right,
			Left = (int)Neighbors.Top | (int)Neighbors.Right | (int)Neighbors.Bottom,
			TopLeft = (int)Neighbors.Right | (int)Neighbors.Bottom,
			Center = (int)Neighbors.Top | (int)Neighbors.TopRight | (int)Neighbors.Right | (int)Neighbors.BottomRight | (int)Neighbors.Bottom | (int)Neighbors.BottomLeft | (int)Neighbors.Left | (int)Neighbors.TopLeft,
			VerticalTop = (int)Neighbors.Bottom,
			VerticalCenter = (int)Neighbors.Top | (int)Neighbors.Bottom,
			VerticalBottom = (int)Neighbors.Top,
			HorizontalRight = (int)Neighbors.Left,
			HorizontalCenter = (int)Neighbors.Right | (int)Neighbors.Left,
			HorizontalLeft = (int)Neighbors.Right,
			Single = 0,
			InnerCornerTopRight = (int)Neighbors.Right | (int)Neighbors.Left | (int)Neighbors.Bottom,
			InnerCornerBottomRight = (int)Neighbors.Top | (int)Neighbors.Right | (int)Neighbors.Left,
			InnerCornerBottomLeft = (int)Neighbors.Top | (int)Neighbors.Right | (int)Neighbors.Left,
			InnerCornerTopLeft = (int)Neighbors.Right | (int)Neighbors.Left | (int)Neighbors.Bottom
		}

		[Flags]
		private enum CasesOptional {
			Top = (int)Neighbors.TopLeft | (int)Neighbors.TopRight | (int)Neighbors.BottomLeft | (int)Neighbors.BottomRight,
			TopRight = (int)Neighbors.TopRight | (int)Neighbors.BottomRight | (int)Neighbors.BottomLeft,
			Right = (int)Neighbors.TopLeft | (int)Neighbors.TopRight | (int)Neighbors.BottomLeft | (int)Neighbors.BottomRight,
			BottomRight = (int)Neighbors.TopRight | (int)Neighbors.BottomLeft | (int)Neighbors.TopLeft,
			Bottom = (int)Neighbors.TopRight | (int)Neighbors.BottomRight | (int)Neighbors.BottomLeft | (int)Neighbors.TopLeft,
			BottomLeft = (int)Neighbors.TopLeft | (int)Neighbors.BottomRight | (int)Neighbors.TopRight,
			Left = (int)Neighbors.TopRight | (int)Neighbors.BottomRight | (int)Neighbors.BottomLeft | (int)Neighbors.TopLeft,
			TopLeft = (int)Neighbors.TopRight | (int)Neighbors.BottomRight | (int)Neighbors.BottomLeft,
			Center = 0,
			VerticalTop = (int)Neighbors.TopRight | (int)Neighbors.BottomRight | (int)Neighbors.BottomLeft | (int)Neighbors.TopLeft,
			VerticalCenter = (int)Neighbors.TopRight | (int)Neighbors.BottomRight | (int)Neighbors.BottomLeft | (int)Neighbors.TopLeft,
			VerticalBottom = (int)Neighbors.TopRight | (int)Neighbors.BottomRight | (int)Neighbors.BottomLeft | (int)Neighbors.TopLeft,
			HorizontalRight = (int)Neighbors.TopRight | (int)Neighbors.BottomRight | (int)Neighbors.BottomLeft | (int)Neighbors.TopLeft,
			HorizontalCenter = (int)Neighbors.TopRight | (int)Neighbors.BottomRight | (int)Neighbors.BottomLeft | (int)Neighbors.TopLeft,
			HorizontalLeft = (int)Neighbors.TopRight | (int)Neighbors.BottomRight | (int)Neighbors.BottomLeft | (int)Neighbors.TopLeft,
			Single = 0,
			InnerCornerTopRight = (int)Neighbors.Top | (int)Neighbors.TopRight | (int)Neighbors.BottomRight | (int)Neighbors.TopLeft,
			InnerCornerBottomRight = (int)Neighbors.TopRight | (int)Neighbors.BottomRight | (int)Neighbors.Bottom | (int)Neighbors.BottomLeft,
			InnerCornerBottomLeft = (int)Neighbors.BottomRight | (int)Neighbors.Bottom | (int)Neighbors.BottomLeft | (int)Neighbors.TopLeft,
			InnerCornerTopLeft = (int)Neighbors.Top | (int)Neighbors.TopRight | (int)Neighbors.BottomLeft | (int)Neighbors.TopLeft
		}

		private enum Tile {
			Top,
			TopRight,
			Right,
			BottomRight,
			Bottom,
			BottomLeft,
			Left,
			TopLeft,
			Center,
			VerticalTop,
			VerticalCenter,
			VerticalBottom,
			HorizontalRight,
			HorizontalCenter,
			HorizontalLeft,
			Single,
			InnerCornerTopRight,
			InnerCornerBottomRight,
			InnerCornerBottomLeft,
			InnerCornerTopLeft
		}
	}

	class TileMaksInfo {
		public int X, Y;
		public float Weight;
		public TileMaksInfo(int x, int y, float weigth) {
			X = x;
			Y = y;
			Weight = weigth;
		}
	}

	public sealed class ReverseComparer<T> : IComparer<T> {
		private readonly IComparer<T> inner;
		public ReverseComparer() : this(null) { }
		public ReverseComparer(IComparer<T> inner) {
			this.inner = inner ?? Comparer<T>.Default;
		}
		int IComparer<T>.Compare(T x, T y) { return inner.Compare(y, x); }
	}
}
