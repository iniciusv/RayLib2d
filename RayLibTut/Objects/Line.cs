using Raylib_cs;
using RayLib2d.Drawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayLib2d.Objects;
public class Line : IBasicShape
{
	public List<Vector2> Vertices { get; set; }
	public bool Selected { get; set; }
	public int Thickness { get; set; }
	public Color ShapeColor { get; set; }
	public enum Endpoint { None, Start, Middle, End }
	public Endpoint SelectedEndpoint { get; private set; } = Endpoint.None;
	public Line(Vector2 start, Vector2 end, int thickness, Color color)
	{
		Vertices = new List<Vector2> { start, end };
		Thickness = thickness;
		ShapeColor = color;
	}

	public void Draw()
	{
		if (Vertices == null || Vertices.Count != 2) return; // Saia do método se a lista de vértices estiver nula ou não tiver exatamente dois pontos

		Raylib.DrawLineV(Vertices[0], Vertices[1], ShapeColor);
	}

	public static void DrawAllLines(List<Line> lines, InputHandler inputHandler)
	{
		var lastKeyPressed = inputHandler.LastKeyPressed;
		var firstClickCoordinates = inputHandler.FirstClickCoordinates;
		lines.Where(line => !line.Selected).ToList().ForEach(line => line.Draw());
		lines.Where(line => line.Selected).ToList().ForEach(line => line.DrawSelectedLines());


		if (inputHandler.LastKeyPressed == 'L')
		{
			if (lastKeyPressed == 'L' && inputHandler.FirstClick)
			{
				Vector2 secondClickCoordinates = inputHandler.MouseWorldPosition;


				// Se o snap de ângulo estiver ativado, ajusta o segundo ponto
				if (inputHandler.SnapAngle)
				{
					secondClickCoordinates = GetSnappedPoint(firstClickCoordinates, secondClickCoordinates);
				}
				// Aplica o snap de proximidade se a tecla Alt estiver pressionada
				if (inputHandler.IsAltKeyPressed)
				{
					secondClickCoordinates = GetSnappedProximity(secondClickCoordinates, lines);
				}

				Raylib.DrawLineV(firstClickCoordinates, secondClickCoordinates, Color.RED);

				if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
				{
					Line newLine = new Line(firstClickCoordinates, secondClickCoordinates, 5, Color.BLUE);
					lines.Add(newLine);

					// Atualiza FirstClickCoordinates para a posição do segundo clique
					inputHandler.FirstClickCoordinates = secondClickCoordinates;
				}
			}
		}
	}
	private static Vector2 GetSnappedProximity(Vector2 mousePosition, List<Line> lines, float snapRadius = 100f)
	{
		Vector2 closestPoint = mousePosition;
		float minDistanceSquared = snapRadius * snapRadius; // Usa o quadrado da distância para evitar cálculos de raiz quadrada

		foreach (var line in lines)
		{
			foreach (var vertex in line.Vertices)
			{
				float distanceSquared = Vector2.DistanceSquared(mousePosition, vertex);
				if (distanceSquared < minDistanceSquared)
				{
					closestPoint = vertex;
					minDistanceSquared = distanceSquared;
				}
			}
		}
		return closestPoint;
	}

	private static Vector2 GetSnappedPoint(Vector2 firstPoint, Vector2 currentPoint)
	{
		float angle = (float)Math.Atan2(currentPoint.Y - firstPoint.Y, currentPoint.X - firstPoint.X);
		float angleDegrees = (float)(angle * (180 / Math.PI));
		float snappedAngleDegrees = (float)(Math.Round(angleDegrees / 10) * 10);
		float snappedAngleRadians = (float)(snappedAngleDegrees * (Math.PI / 180));

		float distance = Vector2.Distance(firstPoint, currentPoint);
		float deltaX = (float)(distance * Math.Cos(snappedAngleRadians));
		float deltaY = (float)(distance * Math.Sin(snappedAngleRadians));

		return new Vector2(firstPoint.X + deltaX, firstPoint.Y + deltaY);
	}
	public void DrawSelectedLines()
	{
		Color lineColor = Selected ? Color.BLUE : Color.RED;

		if (Selected)
		{
			// Desenhe a linha tracejada quando estiver selecionada
			DrawDashedLine(Vertices[0], Vertices[1], lineColor);

			float angle = GetAngleBetweenPoints(Vertices[0], Vertices[1]);

			// Aqui estou presumindo que "SelectedEndpoint" e "Endpoint" são definidos em algum lugar
			// no seu código original ou em alguma classe externa, eles precisam ser corretamente referenciados.
			Color startSquareColor = (SelectedEndpoint == Endpoint.Start) ? Color.BLUE : Color.GREEN;
			Color endSquareColor = (SelectedEndpoint == Endpoint.End) ? Color.BLUE : Color.GREEN;
			Color midSquareColor = (SelectedEndpoint == Endpoint.Middle) ? Color.BLUE : Color.GREEN;

			DrawRotatedSquare(Vertices[0], 10, angle, startSquareColor);
			DrawRotatedSquare(Vertices[1], 10, angle, endSquareColor);
			Vector2 midPoint = GetMidPoint(Vertices[0], Vertices[1]);
			DrawRotatedSquare(midPoint, 10, angle, midSquareColor);
		}
		else
		{
			Raylib.DrawLineV(Vertices[0], Vertices[1], lineColor);
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
	public bool IsMouseOver(Vector2 mousePosition, float threshold = 10f)
	{
		// Converte threshold para int antes de passá-lo para a função CheckCollisionPointLine
		return Raylib.CheckCollisionPointLine(mousePosition, Vertices[0], Vertices[1], (int)threshold);
	}

}
