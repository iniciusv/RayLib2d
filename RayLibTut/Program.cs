using Raylib_cs;
using RayLib2d;
using RayLib2d.Drawing;
using System.Numerics;

static class Program
{
	static void Main(string[] args)
	{
		const int screenWidth = 1900;
		const int screenHeight = 800;

		Raylib.InitWindow(screenWidth, screenHeight, "2D Space with Camera Control");

		Vector2 cameraTarget = new Vector2(screenWidth / 2.0f, screenHeight / 2.0f);
		Vector2 cameraOffset = new Vector2(screenWidth / 2.0f, screenHeight / 2.0f);
		CameraController.Initialize(cameraTarget, cameraOffset);

		char lastKeyPressed = ' ';
		bool FirstClick = false;
		Vector2 FirstClickCoordinates;

		Drawer drawer = new Drawer();

		while (!Raylib.WindowShouldClose()) // Main game loop
		{
			// Update
			InputHandler.Update();

			// Draw

			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.BLACK);

			Raylib.BeginMode2D(InputHandler.GetCamera());
			drawer.Update();
			Draw2DSpace();
			Raylib.EndMode2D();

			Raylib.DrawText($"Last key pressed: {InputHandler.GetLastKeyPressed()}, FirstClick: {InputHandler.GetFirstClick()}", 10, 10, 20, Color.WHITE);
			Raylib.DrawText($"Mouse position: {Raylib.GetMousePosition()}, Transformed Mouse position: ", 10, 25, 20, Color.WHITE);

			Raylib.EndDrawing();
		}

		Raylib.CloseWindow(); // Close window and OpenGL context
	}

	// Draws some shapes in 2D space for visual reference
	static void Draw2DSpace()
	{
		Raylib.DrawRectangle(100, 100, 200, 200, Color.RED);
		Raylib.DrawCircle(500, 500, 100, Color.BLUE);
		//Raylib.DrawLine(0, 0, 1000, 1000, Color.GREEN);
	}
}
