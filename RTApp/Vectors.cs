
using System;

static class V
{
	public static Point sumPV(Point P, Point V)//sum of Point and Vector
	{
		return new Point(P.x + V.xNorm, P.y + V.yNorm, P.z + V.zNorm, 0, 0, 0);
	}

	public static Point sumVV(Point V1, Point V2)//sum of Vector and Vector
	{
		return new Point(0, 0, 0, V1.xNorm + V2.xNorm, V1.yNorm + V2.yNorm, V1.zNorm + V2.zNorm);
	}

	public static Point vsMult(double scalar, Point vec)//vector scalar multiply
	{
		return new Point(vec.x, vec.y, vec.z, scalar * vec.xNorm, scalar * vec.yNorm, scalar * vec.zNorm);
	}

	public static Point PDiff(Point A, Point B)//takes in 2 points, returns vector difference
	{
		return new Point(0, 0, 0, A.x - B.x, A.y - B.y, A.z - B.z);
	}

	public static double VLength(Point P)//takes in a vector and returns its length
	{
		return Math.Sqrt(Math.Pow(P.xNorm, 2) + Math.Pow(P.yNorm, 2) + Math.Pow(P.zNorm, 2));
	}

	public static Point cross(Point first, Point second)//takes 2 vectors, returns cross-product
	{
		Point final = new Point();
		final.xNorm = first.yNorm * second.zNorm - first.zNorm * second.yNorm;
		final.yNorm = -(first.xNorm * second.zNorm - first.zNorm * second.xNorm);
		final.zNorm = first.xNorm * second.yNorm - first.yNorm * second.xNorm;

		return final;
	}

	public static Point normalize(Point norm)
	{
		double length = Math.Sqrt((Math.Pow(norm.xNorm, 2) + Math.Pow(norm.yNorm, 2) + Math.Pow(norm.zNorm, 2)));
		norm.xNorm = norm.xNorm / length;
		norm.yNorm = norm.yNorm / length;
		norm.zNorm = norm.zNorm / length;
		return norm;
	}
	public static double dot(Point A, Point B)
	{
		return (A.xNorm * B.xNorm + A.yNorm * B.yNorm + A.zNorm * B.zNorm);
	}
}