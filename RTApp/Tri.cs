// Tri.cpp: implementation of the Tri class.
//
//////////////////////////////////////////////////////////////////////

using System;
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
		////outputs the normal vector of these 3
		//Point normal;
		//Point P1 = pts[0];
		//Point P2 = pts[1];
		//Point P3 = pts[2];
		//normal = V.cross(V.PDiff(P2, P1), V.PDiff(P3, P1));

		////double cos = V.dot(P, normal);
		////if (cos < 0)
		////{
		////    normal.x *= -1;
		////    normal.y *= -1;
		////    normal.z *= -1;
		////}
		//return V.normalize(normal);

		Point normal;
		Point P1 = pts[0];
		Point P2 = pts[1];
		Point P3 = pts[2];
		if (P == P1 || P == P2 || P == P3)
		{
			normal = V.cross(V.PDiff(P2, P1), V.PDiff(P3, P1));
			return normal;
		}
		double t1 = Math.Abs(V.VLength(V.cross(V.PDiff(P2, P), V.PDiff(P3, P))));
		double t2 = Math.Abs(V.VLength(V.cross(V.PDiff(P3, P), V.PDiff(P1, P))));
		double t3 = Math.Abs(V.VLength(V.cross(V.PDiff(P1, P), V.PDiff(P2, P))));
		double total = t1 + t2 + t3;
		double p1 = t1 / total;
		double p2 = t2 / total;
		double p3 = t3 / total;

		Point N1 = V.vsMult(p1, new Point(P1.nx, P1.ny, P1.nz));
		Point N2 = V.vsMult(p2, new Point(P2.nx, P2.ny, P2.nz));
		Point N3 = V.vsMult(p3, new Point(P3.nx, P3.ny, P3.nz));
		normal = V.sumVV(N3, V.sumVV(N1, N2));
		//double cos = V.dot(P, normal);
		//if (cos < 0)
		//{
		//    normal.x *= -1;
		//    normal.y *= -1;
		//    normal.z *= -1;
		//}
		return V.normalize(normal);
	}
// 	public Point triNormal2(Point P)
// 	{
// 		//outputs the normal vector of these 3
// // 		Point normal;
// // 		Point P1 = pts[0];
// // 		Point P2 = pts[1];
// // 		Point P3 = pts[2];
// // 		normal = V.cross(V.PDiff(P2, P1), V.PDiff(P3, P1));
// // 
// // 		//double cos = V.dot(P, normal);
// // 		//if (cos < 0)
// // 		//{
// // 		//    normal.x *= -1;
// // 		//    normal.y *= -1;
// // 		//    normal.z *= -1;
// // 		//}
// // 		return V.normalize(normal);
// 
// 		Point normal;
// 		Point P1 = pts[0];
// 		Point P2 = pts[1];
// 		Point P3 = pts[2];
// 		double t1 = Math.Abs(V.VLength(V.cross(V.PDiff(P2, P), V.PDiff(P3, P))));
// 		double t2 = Math.Abs(V.VLength(V.cross(V.PDiff(P3, P), V.PDiff(P2, P))));
// 		double t3 = Math.Abs(V.VLength(V.cross(V.PDiff(P2, P), V.PDiff(P1, P))));
// 		double total = t1 + t2 + t3;
// 		double p1 = t1 / total;
// 		double p2 = t2 / total;
// 		double p3 = t3 / total;
// 
// 		Point N1 = V.vsMult(p1, new Point(P1.nx, P1.ny, P1.nz));
// 		Point N2 = V.vsMult(p2, new Point(P2.nx, P2.ny, P2.nz));
// 		Point N3 = V.vsMult(p3, new Point(P3.nx, P3.ny, P3.nz));
// 		normal = V.sumVV(N3, V.sumVV(N1, N2));
// 		double cos = V.dot(P, normal);
// 		if (cos < 0)
// 		{
// 			normal.x *= -1;
// 			normal.y *= -1;
// 			normal.z *= -1;
// 		}
// 		return V.normalize(normal);
// 	}
}


