﻿using Raylib_cs;
using RayLib2d;
using RayLib2d.Drawing;
using System.Numerics;

static class Program
{
	static void Main(string[] args)
	{
		const int screenWidth = 800;
		const int screenHeight = 450;

		Raylib.InitWindow(screenWidth, screenHeight, "2D Space with Camera Control");

		Vector2 cameraTarget = new Vector2(screenWidth / 2.0f, screenHeight / 2.0f);
		Vector2 cameraOffset = new Vector2(screenWidth / 2.0f, screenHeight / 2.0f);
		InputHandler inputHandler = new InputHandler(cameraTarget, cameraOffset);

		Drawer drawer = new Drawer();

		while (!Raylib.WindowShouldClose()) // Main game loop
		{
			// Update
			var LastKeyPressed = inputHandler.Update();

			// Draw
			drawer.Update(LastKeyPressed);

			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.RAYWHITE);

			Raylib.BeginMode2D(inputHandler.GetCamera());
			Draw2DSpace();
			Raylib.EndMode2D();

			Raylib.DrawText($"Last key pressed: {inputHandler.GetLastKeyPressed()}", 10, 10, 20, Color.BLACK);

			Raylib.EndDrawing();
		}

		Raylib.CloseWindow(); // Close window and OpenGL context
	}

	// Draws some shapes in 2D space for visual reference
	static void Draw2DSpace()
	{
		Raylib.DrawRectangle(100, 100, 200, 200, Color.RED);
		Raylib.DrawCircle(500, 500, 100, Color.BLUE);
		Raylib.DrawLine(0, 0, 1000, 1000, Color.GREEN);
	}
}
