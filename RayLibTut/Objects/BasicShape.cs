using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace RayLib2d.Objects;
public interface IBasicShape
{
	public List<Vector2> Vertices { get; set; } 
	public bool Selected { get; set; }
	public int Thickness { get; set; }
	public Color ShapeColor { get; set; }
}
