using Raylib_cs;
using System.Numerics;

public class InputController
{
	public Vector2 Start { get; private set; }
	public KeyboardKey LastKeyPressed { get; private set; }


	public KeyboardKey? Update(KeyboardKey? lastKeyPressed)
	{
		if (Raylib.IsKeyPressed(KeyboardKey.KEY_L))
		{
			LastKeyPressed = KeyboardKey.KEY_L;
			Start = Raylib.GetMousePosition();
		}

		if (Raylib.IsKeyPressed(KeyboardKey.KEY_S))
		{
			LastKeyPressed = KeyboardKey.KEY_S;
		}

		return lastKeyPressed;
	}
}