using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;


public class Drawer
{
	public List<(Vector2 Start, Vector2 End)> Lines { get; } = new List<(Vector2 Start, Vector2 End)>();

	public void Draw(string tool, Vector2 start, Vector2 end)
	{
		if (tool == "Line")
		{
			Lines.Add((start, end));
			Raylib.DrawText("Drawing == true", 10, 10, 20, Color.WHITE);
		}

		foreach (var line in Lines)
		{
			DrawLine(start, end);
		}
		DrawLine(start, end);

	}
	public void DrawLine(Vector2 start, Vector2 end)
	{
			Raylib.DrawLineV(start, end, Color.RED);
	}

	public void AddLine(Vector2 start, Vector2 end)
	{
		Lines.Add((start, end));
	}
}