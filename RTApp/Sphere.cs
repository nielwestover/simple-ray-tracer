// Sphere.cpp: implementation of the Sphere class.
//
//////////////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

class Sphere : Point
{
	public double radius;

	public Sphere()
	{

	}

	public Sphere(double Xc, double Yc, double Zc, double rad, double r, double g, double b)
	{
		x = Xc;
		y = Yc;
		z = Zc;
		radius = rad;
		color.r = r;
		color.g = g;
		color.b = b;
	}

	public Point GetSphereNormal(Point P)
	{
		return new Point(P.x - x, P.y - y, P.z - z);
	}
}