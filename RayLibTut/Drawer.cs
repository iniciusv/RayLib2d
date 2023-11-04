using Raylib_cs;
using RayLib2d.Objects;
using System.Collections.Generic;
using System.Numerics;

namespace RayLib2d.Drawing
{
	public class Drawer
	{
		public static List<Line> Lines { get; private set; } = new List<Line>();
		public bool IsDrawing { get; set; } = false;
		private Vector2? firstPoint = null;

		public Drawer()
		{
			// Inicializando Lines com duas linhas como exemplo
			Lines = new List<Line>
			{
				new Line(new Vector2(0, 0), new Vector2(10, 10), 5, Color.BLUE), // linha do ponto (0,0) para (100,100)
				new Line(new Vector2(10, 0), new Vector2(0, 10), 5, Color.BLUE)  // linha do ponto (100,0) para (0,100)
			};
		}

		public void Update(InputHandler inputHandler)
		{
			Line.DrawAllLines(Lines, inputHandler);
		}

	}
}
