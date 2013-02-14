// Sphere.cpp: implementation of the Sphere class.
//
//////////////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

class Sphere
{
	public Point center;
	public RGB color;
	public double radius;

	public Sphere()
	{

	}

	public Sphere(double Xc, double Yc, double Zc, double rad, double r, double g, double b)
	{
		center.x = Xc;
		center.y = Yc;
		center.z = Zc;
		radius = rad;
		color.r = r;
		color.g = g;
		color.b = b;
	}

	public Point GetSphereNormal(Point P)
	{
		return new Point(P.x - center.x, P.y - center.y, P.z - center.z);
	}
}