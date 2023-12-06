using Raylib_cs;
using RayLib2d.Objects;
using System.Numerics;
using static Program;

public static class InputHandler
{
	public static char LastKeyPressed = ' ';
	private static char PreviousKeyPressed = ' ';
	public static bool FirstClick = false;
	public static bool SnapAngle { get; private set; } = true;
	public static bool IsAltKeyPressed { get; private set; } = true;
	public static bool Reset = false;
	public static Vector2 FirstClickCoordinates;
	public static Vector2 MouseWorldPosition { get; private set; }
	public static string LineExtension = "";

	public static void Update()
	{
		CameraController.Update();
		MouseWorldPosition = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), CameraController.GetCamera());
		for (int key = 32; key < 256; key++)
		{
			if (Raylib.IsKeyPressed((KeyboardKey)key))
			{
				if (char.IsLetter((char)key))
				{
					LastKeyPressed = (char)key;
					break; // Sai do loop se uma letra for pressionada
				}
			}
		}
		HandleNumberInput();

		if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE)) ResetClickState();

		if (LastKeyPressed != ' ')
		{
			if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_LEFT) && !FirstClick)
			{
				FirstClickCoordinates = MouseWorldPosition;
				FirstClick = true;
			}
		}
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_F7)) SnapAngle = !SnapAngle; // Alterna o estado de SnapAngle

		return;
	}
	private static void ResetClickState()
	{
		FirstClick = false;
		FirstClickCoordinates = Vector2.Zero;
		PreviousKeyPressed = LastKeyPressed;
		LastKeyPressed = ' ';
		LineExtension = "";
		Reset = true;
		GlobalState.LastModifiedSecondClick = null;
	}
	private static void HandleNumberInput()
	{
		// Captura a entrada do usuário para números
		for (int key = 48; key <= 57; key++) // Dígitos de 0 a 9
		{
			if (Raylib.IsKeyPressed((KeyboardKey)key))
			{
				LineExtension += (char)key;
				break;
			}
		}

		// Captura o ponto decimal
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_PERIOD))
		{
			if (!LineExtension.Contains('.'))
			{
				LineExtension += ".";
			}
		}

		if (Raylib.IsKeyPressed(KeyboardKey.KEY_COMMA))
		{
			if (!LineExtension.Contains(','))
			{
				LineExtension += ",";
			}
		}


		// Exemplo de como converter LineExtension em um float e resetar para a próxima entrada
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER))
		{
			if (float.TryParse(LineExtension, out float lineExtensionValue))
			{
				// Faça algo com lineExtensionValue, que é o valor flutuante
			}
			LineExtension = ""; // Resetar para a próxima entrada
		}
	}
	public static char GetLastKeyPressed() => LastKeyPressed;// Retorna a última tecla pressionada
	public static bool GetFirstClick() => FirstClick;
	public static Camera2D GetCamera() => CameraController.GetCamera();// Se você precisar acessar o CameraController de fora, pode adicionar um método getter:

}
