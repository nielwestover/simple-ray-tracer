// Tri.cpp: implementation of the Tri class.
//
//////////////////////////////////////////////////////////////////////

class Tri
{

	public int numPoints;
	public Point[] pts = new Point[3];
	public RGB color;

	public Tri()
	{
		numPoints = 0;
	}
	public Tri(Point P1, Point P2, Point P3, RGB c)
	{
		numPoints = 0;
		add(P1);
		add(P2);
		add(P3);
		color = c;
	}

	public void add(Point P)
	{
		pts[numPoints++] = P;
	}

	public void SetColor(RGB c)
	{
		for (int i = 0; i < numPoints; i++)
		{
			pts[i].color = c;
		}
	}

	void Print()
	{
		System.Console.WriteLine("numPoints: " + numPoints);
		for (int i = 0; i < numPoints; i++)
		{
			pts[i].Print();
			pts[i].color.Print();
		}
	}

	public Point triNormal(Point P)
	{
		//outputs the normal vector of these 3
		Point normal;
		Point P1 = pts[0];
		Point P2 = pts[1];
		Point P3 = pts[2];
		normal = V.cross(V.PDiff(P3, P1), V.PDiff(P2, P1));

		double cos = V.dot(P, normal);
		if (cos < 0)
		{
			normal.x *= -1;
			normal.y *= -1;
			normal.z *= -1;
		}
		return V.normalize(normal);
	}

}


