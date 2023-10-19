using Raylib_cs;
using System.Numerics;

public class Drawer
{
	public List<(Vector2 Start, Vector2 End)> Lines { get; private set; } = new List<(Vector2 Start, Vector2 End)>();
	public List<(Vector2 Center, float Radius)> Circles { get; private set; } = new List<(Vector2 Center, float Radius)>();
	public enum Endpoint { None, Start, Middle, End }
	public Endpoint SelectedEndpoint { get; private set; } = Endpoint.None;

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
			Color lineColor = line.Equals(SelectedObject) ? Color.BLUE : Color.RED;
			if (line.Equals(SelectedObject))
			{
				// Desenhe a linha tracejada quando estiver selecionada
				DrawDashedLine(line.Start, line.End, lineColor);

				float angle = GetAngleBetweenPoints(line.Start, line.End);

				// Desenhe quadrados rotacionados nas extremidades
				Color startSquareColor = (SelectedEndpoint == Endpoint.Start) ? Color.BLUE : Color.GREEN;
				Color endSquareColor = (SelectedEndpoint == Endpoint.End) ? Color.BLUE : Color.GREEN;
				Color midSquareColor = (SelectedEndpoint == Endpoint.Middle) ? Color.BLUE : Color.GREEN;

				DrawRotatedSquare(line.Start, 10, angle, startSquareColor);
				DrawRotatedSquare(line.End, 10, angle, endSquareColor);
				Vector2 midPoint = GetMidPoint(line.Start, line.End);
				DrawRotatedSquare(midPoint, 10, angle, midSquareColor);
			}
			else
			{
				Raylib.DrawLineV(line.Start, line.End, lineColor);
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
		SelectedEndpoint = Endpoint.None; // Reset para nenhum ponto de extremidade selecionado

		// Se já temos uma linha selecionada, verifique se um dos quadrados foi clicado
		if (SelectedObject is ValueTuple<Vector2, Vector2> selectedLine)
		{
			float squareSize = 10; // O tamanho dos quadrados
			if (Vector2.Distance(point, selectedLine.Item1) <= squareSize)
			{
				SelectedEndpoint = Endpoint.Start;
				return;
			}
			else if (Vector2.Distance(point, selectedLine.Item2) <= squareSize)
			{
				SelectedEndpoint = Endpoint.End;
				return;
			}
			else
			{
				Vector2 midPoint = GetMidPoint(selectedLine.Item1, selectedLine.Item2);
				if (Vector2.Distance(point, midPoint) <= squareSize)
				{
					SelectedEndpoint = Endpoint.Middle;
					return;
				}
			}
		}

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

	public void MoveLineEndpoint(Vector2 newPoint)
	{
		if (SelectedObject is ValueTuple<Vector2, Vector2> selectedLineTuple && SelectedEndpoint != Endpoint.None)
		{
			var (Start, End) = selectedLineTuple;

			switch (SelectedEndpoint)
			{
				case Endpoint.Start:
					Start = newPoint;
					break;
				case Endpoint.End:
					End = newPoint;
					break;
				case Endpoint.Middle:
					Vector2 midPoint = GetMidPoint(Start, End);
					Vector2 delta = newPoint - midPoint;
					Start += delta;
					End += delta;
					break;
			}

			// Atualizar a linha na lista
			Lines.Remove((Start, End));  // Remova a antiga
			Lines.Add((Start, End));     // Adicione a atualizada

			SelectedObject = (Start, End); // Atualize o objeto selecionado
		}
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

	public void MoveSelectedEndpoint(Vector2 newPosition)
	{
		if (SelectedObject is ValueTuple<Vector2, Vector2> selectedLine && SelectedEndpoint != Endpoint.None)
		{
			var updatedLine = selectedLine;

			switch (SelectedEndpoint)
			{
				case Endpoint.Start:
					updatedLine.Item1 = newPosition;
					break;
				case Endpoint.Middle:
					Vector2 middle = GetMidPoint(selectedLine.Item1, selectedLine.Item2);
					Vector2 delta = newPosition - middle;
					updatedLine.Item1 += delta;
					updatedLine.Item2 += delta;
					break;
				case Endpoint.End:
					updatedLine.Item2 = newPosition;
					break;
			}

			// Atualize a linha na lista
			Lines.Remove(selectedLine);
			Lines.Add(updatedLine);
			SelectedObject = updatedLine;  // Atualize o objeto selecionado para a nova linha
		}
	}


}
