using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;
using RayLib2d.Objects;

namespace RayLib2d.Drawing;
public class RectangleSelector
{
	private Vector2? selectionStartPoint = null;
	private bool isDragging = false;
	private Rectangle selectionRectangle;
	private List<Line> lines;

	public RectangleSelector(List<Line> lines)
	{
		this.lines = lines;
	}

	public bool IsSelecting => isDragging;

	public void StartSelection(Vector2 startPoint)
	{
		selectionStartPoint = startPoint;
		isDragging = true;
	}

	public void UpdateSelection(Vector2 currentPoint)
	{
		if (selectionStartPoint.HasValue)
		{
			selectionRectangle = new Rectangle(
				Math.Min(selectionStartPoint.Value.X, currentPoint.X),
				Math.Min(selectionStartPoint.Value.Y, currentPoint.Y),
				Math.Abs(currentPoint.X - selectionStartPoint.Value.X),
				Math.Abs(currentPoint.Y - selectionStartPoint.Value.Y)
			);
		}
	}

	public void EndSelection()
	{
		if (isDragging)
		{
			foreach (var line in lines)
			{
				if (LineIsInRectangle(line, selectionRectangle))
				{
					line.Selected = true;
				}
			}
		}

		isDragging = false;
		selectionStartPoint = null;
	}

	public void DrawSelection()
	{
		if (isDragging)
		{
			Raylib.DrawRectangleRec(selectionRectangle, new Color(100, 100, 255, 100)); // Cor semi-transparente
		}
	}

	private bool LineIsInRectangle(Line line, Rectangle rect)
	{
		return Raylib.CheckCollisionPointRec(line.Vertices[0], rect) || Raylib.CheckCollisionPointRec(line.Vertices[1], rect);
	}
}
