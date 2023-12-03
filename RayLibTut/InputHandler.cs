using Raylib_cs;
using RayLib2d.Objects;
using System.Numerics;

public static class InputHandler
{
	public static char LastKeyPressed = ' ';
	private static char PreviousKeyPressed = ' ';
	public static bool FirstClick = false;
	public static bool SnapAngle { get; private set; } = true;
	public static bool IsAltKeyPressed { get; private set; } = true;
	public static bool Reset = false;
	public static Vector2 FirstClickCoordinates;
	//private static CameraController cameraController;

	// Adiciona uma nova propriedade para armazenar a posição do mouse no mundo
	public static Vector2 MouseWorldPosition { get; private set; }

	public static void Update()
	{
		// Atualiza o CameraController
		CameraController.Update();
		MouseWorldPosition = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), CameraController.GetCamera());
		//IsAltKeyPressed = Raylib.IsKeyDown(KeyboardKey.KEY_LEFT_ALT) || Raylib.IsKeyDown(KeyboardKey.KEY_RIGHT_ALT);
		// Verifica teclas pressionadas, modificar depois para diferenciar teclas de comandos de unidades
		for (int key = 32; key < 256; key++)
		{
			if (Raylib.IsKeyPressed((KeyboardKey)key))
			{
				LastKeyPressed = (char)key;
				break; // sai do loop assim que encontrar uma tecla
			}
		}

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
		Reset = true;
	}

	public static char GetLastKeyPressed() => LastKeyPressed;// Retorna a última tecla pressionada
	public static bool GetFirstClick() => FirstClick;
	public static Camera2D GetCamera() => CameraController.GetCamera();// Se você precisar acessar o CameraController de fora, pode adicionar um método getter:

}
