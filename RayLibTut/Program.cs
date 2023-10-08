using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;

static class Program
{
	static void Main(string[] args)
	{
		const int screenWidth = 800;
		const int screenHeight = 450;

		Raylib.InitWindow(screenWidth, screenHeight, "raylib example with camera control");

		Vector2 start = new Vector2();
		List<(Vector2 Start, Vector2 End)> lines = new List<(Vector2 Start, Vector2 End)>();

		bool isDrawing = false;
		bool previousMouseState = false;

		Camera2D camera = new Camera2D
		{
			target = new Vector2(screenWidth / 2, screenHeight / 2),
			offset = new Vector2(screenWidth / 2, screenHeight / 2),
			zoom = 1.0f,
			rotation = 0.0f
		};

		while (!Raylib.WindowShouldClose())
		{
			bool currentMouseState = Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON);

			Vector2 mouseWorldPos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera);

			if (currentMouseState && !previousMouseState)
			{
				if (isDrawing)
				{
					lines.Add((start, mouseWorldPos));
					isDrawing = false;
				}
				else
				{
					start = mouseWorldPos;
					isDrawing = true;
				}
			}

			previousMouseState = currentMouseState;

			// Camera control
			if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
			{
				Vector2 delta = Raylib.GetMouseDelta();
				camera.target = Vector2.Add(camera.target, Vector2.Divide(delta, -camera.zoom));
			}

			float mouseWheelMove = Raylib.GetMouseWheelMove();
			if (mouseWheelMove != 0)
			{
				camera.offset = Raylib.GetMousePosition();
				camera.target = mouseWorldPos;

				const float zoomIncrement = 0.125f;
				camera.zoom += mouseWheelMove * zoomIncrement;

				if (camera.zoom < zoomIncrement) camera.zoom = zoomIncrement;
			}

			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.BLACK);

			Raylib.BeginMode2D(camera);

			foreach (var line in lines)
			{
				Raylib.DrawLineV(line.Start, line.End, Color.RED);
			}

			if (isDrawing)
			{
				Raylib.DrawLineV(start, mouseWorldPos, Color.RED);
			}

			Raylib.EndMode2D();

			Raylib.DrawText("Mouse right button drag to move, mouse wheel to zoom", 10, 10, 20, Color.WHITE);

			Raylib.EndDrawing();
		}

		Raylib.CloseWindow();
	}
}
