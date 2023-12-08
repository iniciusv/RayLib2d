//using Raylib_cs;
//using System.Numerics;

//class LineSelector
//{
//	public int SelectedLineIndex { get; private set; } = -1;

//	public void SelectLine(List<(Vector2 Start, Vector2 End)> lines)
//	{
//		Vector2 mousePos = Raylib.GetMousePosition();
//		for (int i = 0; i < lines.Count; i++)
//		{
//			Rectangle lineRect = new Rectangle(
//				Math.Min(lines[i].Start.X, lines[i].End.X),
//				Math.Min(lines[i].Start.Y, lines[i].End.Y),
//				Math.Abs(lines[i].Start.X - lines[i].End.X),
//				Math.Abs(lines[i].Start.Y - lines[i].End.Y));

//			if (Raylib.CheckCollisionPointRec(mousePos, lineRect))
//			{
//				SelectedLineIndex = i;
//				return;
//			}
//		}

//		SelectedLineIndex = -1;
//	}
//}