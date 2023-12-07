using Raylib_cs;
using RayLib2d.Objects;
using System.Collections.Generic;
using System.Numerics;

namespace RayLib2d.Drawing;
public class Drawer
{
	public static List<Line> Lines { get; private set; } = new List<Line>();
	public bool IsDrawing { get; set; } = false;
	private Vector2? firstPoint = null;
	static Vector2? LastProximityPoint = null;
	private TrimLine trimLineTool;

	public Drawer()
	{
		// Inicializando Lines com duas linhas como exemplo
		Lines = new List<Line>
			{
				new Line(new Vector2(0, 0), new Vector2(10, 10), 5, Color.BLUE), // linha do ponto (0,0) para (100,100)
				new Line(new Vector2(10, 0), new Vector2(0, 10), 5, Color.BLUE)  // linha do ponto (100,0) para (0,100)
			};
	}

	public void Update()
	{
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_T)) 
			trimLineTool ??= new TrimLine();
		
		Line.DrawAllLines(Lines);
		trimLineTool?.Update();
		HandleLineSelection();

		if (Raylib.IsKeyPressed(KeyboardKey.KEY_DELETE)) DeleteSelectedLines();

		if (InputHandler.Reset)
		{
			DeselectAllLines();
			InputHandler.Reset = false;
		}
	}
	private void DeleteSelectedLines() => Lines = Lines.Where(line => !line.Selected).ToList();
	public void DeselectAllLines() => Lines.ForEach(line => line.Selected = false);

	private void HandleLineSelection()
	{
		if ( Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
		{
			Vector2 clickPosition = InputHandler.MouseWorldPosition;
			foreach (var line in Lines)
			{
				if (line.IsMouseOver(clickPosition))
				{
					line.Selected = !line.Selected;
					break; // Interrompe o loop uma vez que uma linha é encontrada e selecionada/desselecionada
				}
			}
		}
	}
}
