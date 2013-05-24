
using System;

static class V
{
	public static Point sumPV(Point P, Point V)//sum of Point and Vector
	{
		return new Point(P.x + V.x, P.y + V.y, P.z + V.z);
	}

	public static Point sumVV(Point V1, Point V2)//sum of Vector and Vector
	{
		return new Point(V1.x + V2.x, V1.y + V2.y, V1.z + V2.z);
	}

	public static Point vsMult(double scalar, Point vec)//vector scalar multiply
	{
		return new Point(scalar * vec.x, scalar * vec.y, scalar * vec.z);
	}

	public static  Point PDiff(Point A, Point B)//takes in 2 points, returns vector difference
	{
		return new Point(A.x - B.x, A.y - B.y, A.z - B.z);
	}

	public static double VLength(Point P)//takes in a vector and returns its length
	{
		return Math.Sqrt(Math.Pow(P.x, 2) + Math.Pow(P.y, 2) + Math.Pow(P.z, 2));
	}

	public static Point cross(Point first, Point second)//takes 2 vectors, returns cross-product
	{
		Point final = new Point();
		final.x = first.y * second.z - first.z * second.y;
		final.y = -(first.x * second.z - first.z * second.x);
		final.z = first.x * second.y - first.y * second.x;

		return final;
	}

	public static Point normalize(Point norm)
	{
		double length = Math.Sqrt((Math.Pow(norm.x, 2) + Math.Pow(norm.y, 2) + Math.Pow(norm.z, 2)));
		norm.x = norm.x / length;
		norm.y = norm.y / length;
		norm.z = norm.z / length;
		return norm;
	}
	public static double dot(Point A, Point B)
	{
		return (A.x * B.x + A.y * B.y + A.z * B.z);
	}
}