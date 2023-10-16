using Raylib_cs;
using RayLib2d;
using System.Numerics;

static class Program
{
	static void Main(string[] args)
	{
		const int screenWidth = 800;
		const int screenHeight = 450;

		Raylib.InitWindow(screenWidth, screenHeight, "raylib example with camera control");

		CameraController cameraController = new CameraController(
			new Vector2(screenWidth / 2, screenHeight / 2),
			new Vector2(screenWidth / 2, screenHeight / 2)
		);

		Drawer drawer = new Drawer();
		InputHandler inputHandler = new InputHandler(cameraController, drawer); // Esta linha estava faltando

		while (!Raylib.WindowShouldClose())
		{
			inputHandler.Update(); // Atualize o manipulador de entrada a cada loop

			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.BLACK);

			Raylib.BeginMode2D(cameraController.GetCamera());

			drawer.DrawLines();
			drawer.DrawCircles();

			drawer.DrawTempShapes(inputHandler.LastKeyPressed, inputHandler.IsDrawing, inputHandler.Start, inputHandler.MouseWorldPos);

			Raylib.EndMode2D();
			Raylib.DrawText("Press L for lines, C for circles. Mouse click to draw.", 10, 10, 20, Color.WHITE);
			Raylib.EndDrawing();
		}

		Raylib.CloseWindow();
	}
}