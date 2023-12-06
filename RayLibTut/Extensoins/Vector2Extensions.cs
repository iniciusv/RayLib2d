using RayLib2d.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayLib2d.Extensoins;
public static class Vector2Extensions
{
	public static (Vector2 point, bool snapped) GetPointInProximity(this Vector2 point, List<Line> lines, float snapRadius = 100f)
	{
		Vector2 closestPoint = point;
		float minDistanceSquared = snapRadius * snapRadius;

		foreach (var line in lines.SelectMany(line => line.Vertices))
		{
			float distanceSquared = Vector2.DistanceSquared(point, line);
			if (distanceSquared < minDistanceSquared)
			{
				return (line, true); // Retorna o ponto mais próximo e 'true' para indicar que foi modificado
			}
		}
		return (closestPoint, false); // Retorna o ponto original e 'false' para indicar que não foi modificado
	}
	public static Vector2 GetSnappedAnglePoint(this Vector2 currentPoint, Vector2 firstPoint)
	{
		float angle = (float)Math.Atan2(currentPoint.Y - firstPoint.Y, currentPoint.X - firstPoint.X);
		float angleDegrees = (float)(angle * (180 / Math.PI));
		float snappedAngleDegrees = (float)(Math.Round(angleDegrees / 10) * 10);
		float snappedAngleRadians = (float)(snappedAngleDegrees * (Math.PI / 180));

		float distance = Vector2.Distance(firstPoint, currentPoint);
		float deltaX = (float)(distance * Math.Cos(snappedAngleRadians));
		float deltaY = (float)(distance * Math.Sin(snappedAngleRadians));

		return new Vector2(firstPoint.X + deltaX, firstPoint.Y + deltaY);
	}
}

