using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;
using RayLib2d.Objects;

namespace RayLib2d.Drawing;
public class ShapeSelector
{
	private Vector2? selectionStartPoint = null;
	private bool isDragging = false;
	private Rectangle selectionRectangle;
	private List<Line> lines;
	public bool IsSelecting => isDragging;

	public ShapeSelector(List<Line> lines)
	{
		this.lines = lines;
	}

	public void HandleLineSelection()
	{
		if ((InputHandler.LastKeyPressed == ' ' || InputHandler.LastKeyPressed == 'T') && Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
		{
			Vector2 clickPosition = InputHandler.MouseWorldPosition;
			foreach (var line in lines)
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
