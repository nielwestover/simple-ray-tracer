using RTApp;
using System;
// Sphere.cpp: implementation of the Sphere class.
//
//////////////////////////////////////////////////////////////////////


//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

class Sphere : Shape
{
	public Point center = new Point();
	public double radius;

	public Sphere()
	{

	}

	public Sphere(Point c, double rad, RGB diffuse, RGB specular, RGB ambient, double diffCo, double specCo, double ambCo, double phong)
	{
		center = c;
		radius = rad;
		Id = diffuse;
		Is = specular;
		Ia = ambient;
		Kd = diffCo;
		Ks = specCo;
		Ka = ambCo;
		phongExp = phong;

		computeBounds();
	}

	public Point GetSphereNormal(Point P)
	{
		return new Point(P.x - center.x, P.y - center.y, P.z - center.z);
	}

	public override double IntersectDistance(Point Ray)
	{
		double T = double.PositiveInfinity;
		double a = Ray.x * Ray.x + Ray.y * Ray.y + Ray.z * Ray.z;
		double b = 2 * ((PP.E.x - center.x) * Ray.x + (PP.E.y - center.y) * Ray.y + (PP.E.z - center.z) * Ray.z);
		double c = ((PP.E.x - center.x) * (PP.E.x - center.x) + (PP.E.y - center.y) * (PP.E.y - center.y) + (PP.E.z - center.z) * (PP.E.z - center.z) - radius * radius);

		if (b * b > 4 * a * c)
		{
			double squareRt = Math.Sqrt(b * b - 4 * a * c);
			double T1 = (-b + squareRt) / (2 * a);
			double T2 = (-b - squareRt) / (2 * a);

			if (T1 > 0 && T1 <= T2 && T1 < T)
			{
				T = T1;				
			}
			else if (T2 > 0 && T2 <= T1 && T2 < T)
			{
				T = T2;
			}
		}
		return T;
	}

	public override void computeBounds()
	{
		bounds.xmin = center.x - radius;
		bounds.xmax = center.x + radius;
		bounds.ymin = center.y - radius;
		bounds.ymax = center.y + radius;
		bounds.zmin = center.z - radius;
		bounds.zmax = center.z + radius;
	}
}