using Raylib_cs;
using System.Numerics;

public class InputHandler
{
	private char lastKeyPressed = ' ';
	private bool MouseDirButton = false;
	private CameraController cameraController;

	public InputHandler(Vector2 cameraTarget, Vector2 cameraOffset)
	{
		cameraController = new CameraController(cameraTarget, cameraOffset);
	}

	// Atualiza o estado do input
	public char Update()
	{
		// Atualiza o CameraController
		cameraController.Update();

		// Verifica teclas pressionadas, modificar depois para diferenciar teclas de comandos de unidades
		for (int key = 32; key < 256; key++)
		{
			if (Raylib.IsKeyPressed((KeyboardKey)key))
			{
				lastKeyPressed = (char)key;
				break; // sai do loop assim que encontrar uma tecla
			}
		}

		// Verifica se a tecla ESC foi pressionada
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_ESCAPE))
		{
			lastKeyPressed = ' ';
		}

		return lastKeyPressed;
	}

	// Retorna a última tecla pressionada
	public char GetLastKeyPressed()
	{
		return lastKeyPressed;
	}

	// Se você precisar acessar o CameraController de fora, pode adicionar um método getter:
	public Camera2D GetCamera()
	{
		return cameraController.GetCamera();
	}
}
