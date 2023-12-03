using Raylib_cs;
using System.Numerics;

static public class CameraController
{
	static private Camera2D camera;

	static public void Initialize(Vector2 target, Vector2 offset)
	{
		camera = new Camera2D
		{
			target = target,
			offset = offset,
			zoom = 1.0f,
			rotation = 0.0f
		};
	}

	static public Camera2D GetCamera() => camera;

	static public void Update()
	{
		// Camera control
		if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
		{
			Vector2 delta = Raylib.GetMouseDelta();
			camera.target = Vector2.Add(camera.target, Vector2.Divide(delta, -camera.zoom));
		}

		float mouseWheelMove = Raylib.GetMouseWheelMove();
		if (mouseWheelMove != 0)
		{
			Vector2 mouseWorldPos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera);
			camera.offset = Raylib.GetMousePosition();
			camera.target = mouseWorldPos;

			const float zoomIncrement = 0.125f;
			camera.zoom += mouseWheelMove * zoomIncrement;

			if (camera.zoom < zoomIncrement) camera.zoom = zoomIncrement;
		}
	}
}
