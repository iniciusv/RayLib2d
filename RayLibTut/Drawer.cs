using Raylib_cs;
using RayLib2d.Objects;
using System.Collections.Generic;
using System.Numerics;

namespace RayLib2d.Drawing;
public class Drawer
{
	public static List<Line> Lines { get; private set; } = new List<Line>();
	private TrimLine trimLineTool;

	private ShapeSelector shapeSelector; // Usando a nova classe ShapeSelector


	public Drawer()
	{
		// Inicializando Lines com duas linhas como exemplo
		Lines = new List<Line>
			{
				new Line(new Vector2(0, 0), new Vector2(10, 10), 5, Color.BLUE), // linha do ponto (0,0) para (100,100)
				new Line(new Vector2(10, 0), new Vector2(0, 10), 5, Color.BLUE)  // linha do ponto (100,0) para (0,100)
			};
		shapeSelector = new ShapeSelector(Lines);
	}

	public void Update()
	{
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_T))
		{
			if (trimLineTool == null)
			{
				trimLineTool = new TrimLine();
				trimLineTool.TrimOperationCompleted += OnTrimOperationCompleted;
			}
		}

			Line.DrawAllLines(Lines);
		trimLineTool?.Update();

		if (Raylib.IsKeyPressed(KeyboardKey.KEY_DELETE)) DeleteSelectedLines();

		if (InputHandler.Reset)
		{
			DeselectAllLines();
			InputHandler.Reset = false;
		}
		
		shapeSelector.HandleLineSelection();
		shapeSelector.ManageRectangleSelection();
	}

	private void DeleteSelectedLines() => Lines = Lines.Where(line => !line.Selected).ToList();
	public void DeselectAllLines() => Lines.ForEach(line => line.Selected = false);

	private void OnTrimOperationCompleted() => trimLineTool = null;
}
