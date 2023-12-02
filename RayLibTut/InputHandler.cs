using Raylib_cs;
using RayLib2d.Objects;
using System.Numerics;

public class InputHandler
{
	public char LastKeyPressed = ' ';
	private char PreviousKeyPressed = ' ';
	public bool FirstClick = false;
	public Vector2 FirstClickCoordinates;
	private CameraController cameraController;

	// Adiciona uma nova propriedade para armazenar a posição do mouse no mundo
	public Vector2 MouseWorldPosition { get; private set; }

	public InputHandler(Vector2 cameraTarget, Vector2 cameraOffset)
	{
		cameraController = new CameraController(cameraTarget, cameraOffset);
	}

	public void Update()
	{
		// Atualiza o CameraController
		cameraController.Update();
		MouseWorldPosition = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), cameraController.GetCamera());

		// Verifica teclas pressionadas, modificar depois para diferenciar teclas de comandos de unidades
		for (int key = 32; key < 256; key++)
		{
			if (Raylib.IsKeyPressed((KeyboardKey)key))
			{
				LastKeyPressed = (char)key;
				break; // sai do loop assim que encontrar uma tecla
			}
		}

		if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
		{
			ResetClickState();
			LastKeyPressed = ' ';
		}

		if(LastKeyPressed != ' ')
		{
			if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT) && !FirstClick)
			{
				FirstClickCoordinates = MouseWorldPosition;
				FirstClick = true;
			}
		}

		return;
	}

	private void ResetClickState()
	{
		FirstClick = false;
		FirstClickCoordinates = Vector2.Zero;
		PreviousKeyPressed = LastKeyPressed;
	}

	// Retorna a última tecla pressionada
	public char GetLastKeyPressed()
	{
		return LastKeyPressed;
	}
	public bool GetFirstClick()
	{
		return FirstClick;
	}

	// Se você precisar acessar o CameraController de fora, pode adicionar um método getter:
	public Camera2D GetCamera()
	{
		return cameraController.GetCamera();
	}
}
