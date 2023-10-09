using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

public class Drawer
{
	public List<(Vector2 Start, Vector2 End)> Lines { get; private set; } = new List<(Vector2 Start, Vector2 End)>();
	public List<(Vector2 Center, float Radius)> Circles { get; private set; } = new List<(Vector2 Center, float Radius)>();

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
			Raylib.DrawLineV(line.Start, line.End, Color.RED);
		}
	}

	public void DrawCircles()
	{
		foreach (var circle in Circles)
		{
			Raylib.DrawCircleV(circle.Center, circle.Radius, Color.BLUE);
		}
	}
}
