using Raylib_cs;
using System.Numerics;

namespace RayLib2d
{
	public class InputHandler
	{
		private bool previousMouseState = false;
		private Vector2 start = new Vector2();
		private bool isDrawing = false;
		private CameraController cameraController;
		private Drawer drawer;

		private bool isDraggingEndpoint = false;

		// Propriedades públicas para expor os valores necessários:
		public char LastKeyPressed { get; private set; } = 'L';
		public bool IsDrawing { get => isDrawing; }
		public Vector2 Start { get => start; }
		public Vector2 MouseWorldPos
		{
			get
			{
				return Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), cameraController.GetCamera());
			}
		}

		public InputHandler(CameraController camController, Drawer drawerInstance)
		{
			cameraController = camController;
			drawer = drawerInstance;
		}

		public void Update()
		{
			bool currentMouseState = Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON);
			Vector2 mouseWorldPos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), cameraController.GetCamera());

			if (Raylib.IsKeyPressed(KeyboardKey.KEY_L)) LastKeyPressed = 'L';
			if (Raylib.IsKeyPressed(KeyboardKey.KEY_C)) LastKeyPressed = 'C';
			if (Raylib.IsKeyPressed(KeyboardKey.KEY_S)) LastKeyPressed = 'S';

			if (currentMouseState && !previousMouseState && LastKeyPressed == 'S')
			{
				drawer.SelectObject(mouseWorldPos);
			}

			// Verificar se o botão esquerdo do mouse está pressionado e se algum quadrado de um vértice da linha foi selecionado.
			if (Raylib.IsMouseButtonDown(MouseButton.MOUSE_LEFT_BUTTON) && drawer.SelectedEndpoint != Drawer.Endpoint.None)
			{
				isDraggingEndpoint = true;
			}
			else if (Raylib.IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
			{
				isDraggingEndpoint = false;
			}

			if (isDraggingEndpoint)
			{
				drawer.MoveSelectedEndpoint(mouseWorldPos);
			}

			if (currentMouseState && !previousMouseState)
			{
				if (LastKeyPressed == 'L')
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
				else if (LastKeyPressed == 'C')
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
		}

	}

}
