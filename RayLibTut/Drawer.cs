using Raylib_cs;
using RayLib2d.Objects;
using System.Collections.Generic;
using System.Numerics;

namespace RayLib2d.Drawing
{
	public class Drawer
	{
		private List<Line> Lines;
		public bool IsDrawing { get; set; } = false;
		private Vector2? firstPoint = null;

		public Drawer()
		{
			Lines = new List<Line>();
		}

		public void Update(char lastKeyPressed)
		{
			Line.DrawAllLines(Lines, IsDrawing) ;
		}
	}
}
