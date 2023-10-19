using Raylib_cs;
using System.Numerics;

public class Drawer
{
	public List<(Vector2 Start, Vector2 End)> Lines { get; private set; } = new List<(Vector2 Start, Vector2 End)>();
	public List<(Vector2 Center, float Radius)> Circles { get; private set; } = new List<(Vector2 Center, float Radius)>();
	public object SelectedObject { get; private set; }

	public void AddLine(Vector2 start, Vector2 end)
	{
		Lines.Add((start, end));
	}

	public void AddCircle(Vector2 center, float radius)
	{
		Circles.Add((center, radius));
	}

	public void DrawLines()
	{
		foreach (var line in Lines)
		{
			Color color = line.Equals(SelectedObject) ? Color.BLUE : Color.RED;
			if (line.Equals(SelectedObject))
			{
				// Desenhe a linha tracejada quando estiver selecionada
				DrawDashedLine(line.Start, line.End, color);

				float angle = GetAngleBetweenPoints(line.Start, line.End);

				// Desenhe quadrados rotacionados nas extremidades
				DrawRotatedSquare(line.Start, 10, angle, Color.GREEN);
				DrawRotatedSquare(line.End, 10, angle, Color.GREEN);

				// Desenhe um retângulo rotacionado no ponto médio
				Vector2 midPoint = GetMidPoint(line.Start, line.End);
				DrawRotatedSquare(midPoint, 10, angle, Color.GREEN);

			}
			else
			{
				Raylib.DrawLineV(line.Start, line.End, color);
			}
		}
	}


	private void DrawRotatedSquare(Vector2 center, float sideLength, float angle, Color color)
	{
		float halfDiagonal = sideLength * (float)Math.Sqrt(2) / 2;

		Vector2 topRight = center + new Vector2(halfDiagonal * (float)Math.Cos(angle + Math.PI / 4), halfDiagonal * (float)Math.Sin(angle + Math.PI / 4));
		Vector2 topLeft = center + new Vector2(halfDiagonal * (float)Math.Cos(angle + 3 * Math.PI / 4), halfDiagonal * (float)Math.Sin(angle + 3 * Math.PI / 4));
		Vector2 bottomLeft = center + new Vector2(halfDiagonal * (float)Math.Cos(angle + 5 * Math.PI / 4), halfDiagonal * (float)Math.Sin(angle + 5 * Math.PI / 4));
		Vector2 bottomRight = center + new Vector2(halfDiagonal * (float)Math.Cos(angle + 7 * Math.PI / 4), halfDiagonal * (float)Math.Sin(angle + 7 * Math.PI / 4));

		Raylib.DrawTriangle(topLeft, topRight, bottomRight, color);
		Raylib.DrawTriangle(bottomRight, bottomLeft, topLeft, color);
	}

	private float GetAngleBetweenPoints(Vector2 pointA, Vector2 pointB)
	{
		return (float)Math.Atan2(pointB.Y - pointA.Y, pointB.X - pointA.X);
	}


	private void DrawDashedLine(Vector2 start, Vector2 end, Color color)
	{
		float distance = Vector2.Distance(start, end);
		int segments = (int)distance / 10; // Ajuste esse valor conforme desejado

		for (int i = 0; i < segments; i++)
		{
			Vector2 startPoint = Vector2.Lerp(start, end, (float)i / segments);
			Vector2 endPoint = Vector2.Lerp(start, end, (float)(i + 0.5) / segments); // 0.5 define o tamanho do segmento
			Raylib.DrawLineV(startPoint, endPoint, color);
		}
	}

	private Vector2 GetMidPoint(Vector2 pointA, Vector2 pointB)
	{
		return new Vector2((pointA.X + pointB.X) / 2, (pointA.Y + pointB.Y) / 2);
	}


	public void DrawCircles()
	{
		foreach (var circle in Circles)
		{
			Color color = circle.Equals(SelectedObject) ? Color.BLUE : Color.RED;
			Raylib.DrawCircleV(circle.Center, circle.Radius, color);
		}
	}

	public void SelectObject(Vector2 point)
	{
		// Check for circles first
		foreach (var circle in Circles)
		{
			float distance = Vector2.Distance(point, circle.Center);
			if (distance <= circle.Radius)
			{
				SelectedObject = circle;
				return;
			}
		}

		// Check for lines next
		foreach (var line in Lines)
		{
			// This is a simple check, and may not catch all cases where the mouse clicks near the line.
			float distanceToStart = Vector2.Distance(point, line.Start);
			float distanceToEnd = Vector2.Distance(point, line.End);
			float lineLength = Vector2.Distance(line.Start, line.End);

			if (distanceToStart + distanceToEnd - lineLength < 0.1f)
			{
				SelectedObject = line;
				return;
			}
		}

		// Deselect if no object is found
		SelectedObject = null;
	}

	public void DrawTempShapes(char lastKeyPressed, bool isDrawing, Vector2 start, Vector2 mouseWorldPos)
	{
		if (isDrawing)
		{
			if (lastKeyPressed == 'L')
			{
				Raylib.DrawLineV(start, mouseWorldPos, Color.RED);
			}
			else if (lastKeyPressed == 'C')
			{
				float radius = Vector2.Distance(start, mouseWorldPos);
				Raylib.DrawCircleV(start, radius, Color.BLUE);
			}
		}
	}

}
