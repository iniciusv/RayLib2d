using Raylib_cs;
using RayLib2d.Drawing;
using RayLib2d.Extensoins;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Program;

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
		if (Vertices == null || Vertices.Count != 2) return;

		Raylib.DrawLineV(Vertices[0], Vertices[1], ShapeColor);
	}
	public static void DrawAllLines(List<Line> lines)
	{
		lines.ForEach(line =>
		{
			if (line.Selected)
				line.DrawSelectedLines();
			else
				line.Draw();
		});

		if (InputHandler.LastKeyPressed == 'L' && InputHandler.FirstClick)
		{
			var secondClickCoordinates = InputHandler.MouseWorldPosition;
			var secondClickModified = secondClickCoordinates.GetSnappedAnglePoint(InputHandler.FirstClickCoordinates);
			var proximityResult = secondClickCoordinates.GetPointInProximity(lines);

			if (proximityResult.snapped)
			{
				secondClickModified = proximityResult.point;
				GlobalState.LastModifiedSecondClick = secondClickModified;
			}

			if (float.TryParse(InputHandler.LineExtension, out float lineExtensionValue) && lineExtensionValue > 0)
			{
				var direction = Vector2.Normalize(secondClickModified - InputHandler.FirstClickCoordinates);
				secondClickModified = InputHandler.FirstClickCoordinates + direction * lineExtensionValue;
			}

			if (GlobalState.LastModifiedSecondClick.HasValue)
			{
				secondClickModified = secondClickModified.GetSnappedAnglePoint(GlobalState.LastModifiedSecondClick.Value);
				DrawDashedLine(GlobalState.LastModifiedSecondClick.Value, secondClickModified, 3, 3, Color.WHITE);
			}

			DrawTemporaryLine(InputHandler.FirstClickCoordinates, secondClickModified, Color.RED);

			if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
			{
				lines.Add(new Line(InputHandler.FirstClickCoordinates, secondClickModified, 5, Color.BLUE));
				InputHandler.FirstClickCoordinates = secondClickModified;
				GlobalState.LastModifiedSecondClick = null; // Reset após a utilização
				InputHandler.LineExtension = "";
			}
		}
	}

	private static void DrawTemporaryLine(Vector2 start, Vector2 end, Color color) => Raylib.DrawLineV(start, end, color);

	public void DrawSelectedLines()
	{
		Color lineColor = Selected ? Color.BLUE : Color.RED;

		if (Selected)
		{
			// Desenhe a linha tracejada quando estiver selecionada
			DrawDashedLine(Vertices[0], Vertices[1], 10, 5 , lineColor);

			float angle = GetAngleBetweenPoints(Vertices[0], Vertices[1]);

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
	public static void DrawDashedLine(Vector2 start, Vector2 end, int dashLength, int spaceLength, Color color)
	{
		float totalLength = Vector2.Distance(start, end);
		Vector2 direction = Vector2.Normalize(end - start);

		float drawnLength = 0.0f;
		while (drawnLength < totalLength)
		{
			Vector2 dashStart = start + direction * drawnLength;
			drawnLength += dashLength;
			if (drawnLength > totalLength) drawnLength = totalLength;
			Vector2 dashEnd = start + direction * drawnLength;
			Raylib.DrawLineV(dashStart, dashEnd, color);

			drawnLength += spaceLength;
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
	private float GetAngleBetweenPoints(Vector2 pointA, Vector2 pointB) => (float)Math.Atan2(pointB.Y - pointA.Y, pointB.X - pointA.X);
	private Vector2 GetMidPoint(Vector2 pointA, Vector2 pointB) => new Vector2((pointA.X + pointB.X) / 2, (pointA.Y + pointB.Y) / 2);
	public bool IsMouseOver(Vector2 mousePosition, float threshold = 10f) => Raylib.CheckCollisionPointLine(mousePosition, Vertices[0], Vertices[1], (int)threshold);
}


