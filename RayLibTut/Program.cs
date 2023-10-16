using Raylib_cs;
using System.Numerics;

static class Program
{
	static void Main(string[] args)
	{
		const int screenWidth = 800;
		const int screenHeight = 450;

		Raylib.InitWindow(screenWidth, screenHeight, "raylib example with camera control");

		Vector2 start = new Vector2();
		bool isDrawing = false;
		bool previousMouseState = false;
		char lastKeyPressed = 'L'; // Default to drawing lines

		CameraController cameraController = new CameraController(
			new Vector2(screenWidth / 2, screenHeight / 2),
			new Vector2(screenWidth / 2, screenHeight / 2)
		);

		Drawer drawer = new Drawer();

		while (!Raylib.WindowShouldClose())
		{
			bool currentMouseState = Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON);

			Vector2 mouseWorldPos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), cameraController.GetCamera());

			if (Raylib.IsKeyPressed(KeyboardKey.KEY_L)) lastKeyPressed = 'L';
			if (Raylib.IsKeyPressed(KeyboardKey.KEY_C)) lastKeyPressed = 'C';
			if (Raylib.IsKeyPressed(KeyboardKey.KEY_S)) lastKeyPressed = 'S';

			if (currentMouseState && !previousMouseState && lastKeyPressed == 'S')
			{
				drawer.SelectObject(mouseWorldPos);
			}


			if (currentMouseState && !previousMouseState)
			{
				if (lastKeyPressed == 'L')
				{
					if (isDrawing)
					{
						drawer.AddLine(start, mouseWorldPos);
						isDrawing = false;
					}
					else
					{
						start = mouseWorldPos;
						isDrawing = true;
					}
				}
				else if (lastKeyPressed == 'C')
				{
					if (isDrawing)
					{
						float radius = Vector2.Distance(start, mouseWorldPos);
						drawer.AddCircle(start, radius);
						isDrawing = false;
					}
					else
					{
						start = mouseWorldPos;
						isDrawing = true;
					}
				}
			}

			previousMouseState = currentMouseState;

			cameraController.Update();

			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.BLACK);

			Raylib.BeginMode2D(cameraController.GetCamera());

			drawer.DrawLines();
			drawer.DrawCircles();

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

			Raylib.EndMode2D();

			Raylib.DrawText("Press L for lines, C for circles. Mouse click to draw.", 10, 10, 20, Color.WHITE);

			Raylib.EndDrawing();
		}

		Raylib.CloseWindow();
	}
}
