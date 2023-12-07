using Raylib_cs;
using RayLib2d.Drawing;
using RayLib2d.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayLib2d;
public class TrimLine
{
	private Line referenceLine;
	private Line lineToTrim;
	private bool isWaitingForReferenceLine = true;

	public void Update()
	{

		if (isWaitingForReferenceLine)
		{
			// Espera o usuário selecionar a linha de referência
			if (LineSelected())
			{
				referenceLine = Drawer.Lines.FirstOrDefault(line => line.Selected);
				isWaitingForReferenceLine = false;

				// Chama DeselectAllLines para desmarcar todas as linhas
				DeselectAllLines();
			}
		}

		if (!isWaitingForReferenceLine)
		{
			// Espera o usuário selecionar a segunda linha e a corta
			if (LineSelected())
			{
				lineToTrim = Drawer.Lines.FirstOrDefault(line => line.Selected);
				TrimLineAtIntersection();
				UpdateLinesList();
				EndTrimOperation();
			}
			DrawLinesIfExist();
		}
	}

	private bool LineSelected() => Drawer.Lines.Count(line => line.Selected) == 1;
	private void DrawLinesIfExist()
	{
		if (referenceLine != null)
		{
			Line.DrawDashedLine(referenceLine.Vertices[0], referenceLine.Vertices[1], 10, 5, Color.GRAY);
		}

		if (lineToTrim != null)
		{
			Line.DrawDashedLine(lineToTrim.Vertices[0], lineToTrim.Vertices[1], 10, 5, Color.GRAY);
		}
	}


	private void TrimLineAtIntersection()
	{
		Vector2? intersectionPoint = CalculateIntersectionPoint(referenceLine, lineToTrim);

		if (intersectionPoint.HasValue)
		{
			TrimLineClosestToMouse(lineToTrim, intersectionPoint.Value);
		}
	}

	private Vector2? CalculateIntersectionPoint(Line line1, Line line2)
	{
		Vector2 a = line1.Vertices[0];
		Vector2 b = line1.Vertices[1];
		Vector2 c = line2.Vertices[0];
		Vector2 d = line2.Vertices[1];

		Vector2 r = b - a;
		Vector2 s = d - c;

		float rCrossS = r.X * s.Y - r.Y * s.X;

		if (rCrossS == 0) return null;

		Vector2 cMinusA = c - a;

		float t = (cMinusA.X * s.Y - cMinusA.Y * s.X) / rCrossS;
		float u = (cMinusA.X * r.Y - cMinusA.Y * r.X) / rCrossS;

		if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
		{
			return a + t * r;
		}
		return null;
	}


	private void TrimLineClosestToMouse(Line line, Vector2 intersectionPoint)
	{
		Vector2 mousePosition = InputHandler.MouseWorldPosition;

		float distanceToStart = Vector2.Distance(mousePosition, line.Vertices[0]);
		float distanceToEnd = Vector2.Distance(mousePosition, line.Vertices[1]);

		if (distanceToStart < distanceToEnd)
		{
			line.Vertices[0] = intersectionPoint;// Corta a linha do início até o ponto de interseção
		}
		else
		{	
			line.Vertices[1] = intersectionPoint;// Corta a linha do fim até o ponto de interseção
		}
	}
	private void UpdateLinesList() => Drawer.Lines.RemoveAll(line => IsLineInvalid(line));
	private bool IsLineInvalid(Line line)
	{
		float minLength = 1.0f; // Define um comprimento mínimo para as linhas
		float length = Vector2.Distance(line.Vertices[0], line.Vertices[1]);
		return length < minLength;
	}

	private void EndTrimOperation()
	{
		referenceLine = null;
		lineToTrim = null;
		isWaitingForReferenceLine = true;
		DeselectAllLines();
	}
	public void DeselectAllLines() => Drawer.Lines.ForEach(line => line.Selected = false);
}
