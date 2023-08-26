using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;
using System;

static class Program
{
	static void Main(string[] args)
	{
		const int screenWidth = 800;
		const int screenHeight = 450;

		Raylib.InitWindow(screenWidth, screenHeight, "raylib example");

		Vector2 start = new Vector2();
		List<(Vector2 Start, Vector2 End)> lines = new List<(Vector2 Start, Vector2 End)>();

		bool isDrawing = false;
		bool previousMouseState = false;

		while (!Raylib.WindowShouldClose())
		{
			bool currentMouseState = Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON);

			if (currentMouseState && !previousMouseState)
			{
				if (isDrawing)
				{
					// Second click: end the current line
					lines.Add((start, Raylib.GetMousePosition()));
					isDrawing = false;
				}
				else
				{
					// First click: start a new line
					start = Raylib.GetMousePosition();
					isDrawing = true;
				}
			}

			previousMouseState = currentMouseState;

			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.BLACK);

			// Draw all lines
			foreach (var line in lines)
			{
				Raylib.DrawLineV(line.Start, line.End, Color.RED);
			}

			// Draw current line
			if (isDrawing)
			{
				Raylib.DrawLineV(start, Raylib.GetMousePosition(), Color.RED);
			}

			Raylib.EndDrawing();
		}

		Raylib.CloseWindow();
	}
}
