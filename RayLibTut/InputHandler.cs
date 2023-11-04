using Raylib_cs;
using System.Numerics;

public class InputHandler
{
	public char LastKeyPressed = ' ';
	public bool FirstClick = false;
	public Vector2 FirstClickCoordinates;
	private CameraController cameraController;

	public InputHandler(Vector2 cameraTarget, Vector2 cameraOffset)
	{
		cameraController = new CameraController(cameraTarget, cameraOffset);
	}

	// Atualiza o estado do input
	public void Update()
	{
		// Atualiza o CameraController
		cameraController.Update();

		// Verifica teclas pressionadas, modificar depois para diferenciar teclas de comandos de unidades
		for (int key = 32; key < 256; key++)
		{
			if (Raylib.IsKeyPressed((KeyboardKey)key))
			{
				LastKeyPressed = (char)key;
				break; // sai do loop assim que encontrar uma tecla
			}
		}

		// Verifica se a tecla ESC foi pressionada
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
		{
			LastKeyPressed = ' ';
		}

		if(LastKeyPressed != ' ')
		{
			if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT))
			{
				FirstClickCoordinates = Raylib.GetMousePosition();
				FirstClick = true;
			}
		}

		return;
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
