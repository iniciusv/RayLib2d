using Raylib_cs;

public class GridDrawer
{
	public void DrawGrid(int gridSize = 10000, int lineSpacing = 100 )
	{
		int halfGrid = gridSize / 2;

		for (int i = -halfGrid; i < halfGrid; i += lineSpacing)
		{
			// Draw horizontal lines
			Raylib.DrawLine(i, -halfGrid, i, halfGrid, Color.DARKGRAY);
			// Draw vertical lines
			Raylib.DrawLine(-halfGrid, i, halfGrid, i, Color.DARKGRAY);
		}
	}
}
