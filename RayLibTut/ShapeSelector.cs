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
	public void ManageRectangleSelection()
	{
		if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_BUTTON_LEFT))
		{
			if (!IsSelecting)
			{
				StartRectangleSelection(InputHandler.MouseWorldPosition);
			}
			else
			{
				UpdateRectangleSelection(InputHandler.MouseWorldPosition);
			}
		}
		else if (IsSelecting)
		{
			EndRectangleSelection();
		}
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
	public void StartRectangleSelection(Vector2 startPoint)
	{
		selectionStartPoint = startPoint;
		isDragging = true;
	}

	public void UpdateRectangleSelection(Vector2 currentPoint)
	{
		if (isDragging && selectionStartPoint.HasValue)
		{
			selectionRectangle = new Rectangle(
				Math.Min(selectionStartPoint.Value.X, currentPoint.X),
				Math.Min(selectionStartPoint.Value.Y, currentPoint.Y),
				Math.Abs(currentPoint.X - selectionStartPoint.Value.X),
				Math.Abs(currentPoint.Y - selectionStartPoint.Value.Y)
			);

			Raylib.DrawRectangleRec(selectionRectangle, new Color(255, 255, 255, 125)); // Semi-transparent rectangle
		}
	}

	public void EndRectangleSelection()
	{
		if (isDragging)
		{
			foreach (var line in lines)
			{
				if (IsLineInsideRectangle(line, selectionRectangle))
				{
					line.Selected = true;
				}
			}
			isDragging = false;
			selectionStartPoint = null;
		}
	}
	private bool IsLineInsideRectangle(Line line, Rectangle selectionRectangle)
	{
		return IsPointInsideRectangle(line.Vertices[0], selectionRectangle) &&
			   IsPointInsideRectangle(line.Vertices[1], selectionRectangle);
	}

	private bool IsPointInsideRectangle(Vector2 point, Rectangle rectangle)
	{
		return point.X >= rectangle.x &&
			   point.X <= rectangle.x + rectangle.width &&
			   point.Y >= rectangle.y &&
			   point.Y <= rectangle.y + rectangle.height;
	}
}