using Raylib_cs;
using System.Numerics;

class CameraController
{
	private Camera2D _camera;

	public CameraController(Camera2D camera)
	{
		_camera = camera;
	}

	public void HandleInput()
	{
		// Translate based on mouse right click
		if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_RIGHT_BUTTON))
		{
			Vector2 delta = Raylib.GetMouseDelta();
			delta = Vector2.Multiply(delta, -1.0f / _camera.zoom);

			_camera.target = Vector2.Add(_camera.target, delta);
		}

		// Zoom based on mouse wheel
		float wheel = Raylib.GetMouseWheelMove();
		if (wheel != 0)
		{
			// Get the world point that is under the mouse
			Vector2 mouseWorldPos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), _camera);

			// Set the offset to where the mouse is
			_camera.offset = Raylib.GetMousePosition();

			// Set the target to match, so that the camera maps the world space point 
			// under the cursor to the screen space point under the cursor at any zoom
			_camera.target = mouseWorldPos;

			// Zoom increment
			const float zoomIncrement = 0.125f;

			_camera.zoom += (wheel * zoomIncrement);
			if (_camera.zoom < zoomIncrement) _camera.zoom = zoomIncrement;
		}
	}

	public Camera2D GetCamera()
	{
		return _camera;
	}
}
