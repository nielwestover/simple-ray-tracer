// Tri.cpp: implementation of the Tri class.
//
//////////////////////////////////////////////////////////////////////

using System;
using RTApp;
using System.Collections.Generic;
class Tri : Shape
{

	const double eps = .0000001;

	public int numPoints;
	public List<Point> pts = new List<Point>();
	public int ID;
	//cache these - the same every time we do barycentric coords
	Point v0;
	Point v1;
	double d00;
	double d01;
	double d11;
	double invDenom;

	Point faceNormal = null;

	public Tri()
	{
		numPoints = 0;
	}
	public Tri(Point P1, Point P2, Point P3, RGB c, int id = 0)
	{
		ID = id;
		numPoints = 0;
		pts.Add(P1);
		pts.Add(P2);
		pts.Add(P3);
		color = c;

		v0 = V.PDiff(pts[1], pts[0]);
		v1 = V.PDiff(pts[2], pts[0]);
		d00 = V.dot(v0, v0);
		d01 = V.dot(v0, v1);
		d11 = V.dot(v1, v1);
		invDenom = 1.0 / (d00 * d11 - d01 * d01);

		computeBounds();
	}
	public override void computeBounds()
	{
		foreach (var item in pts)
		{
			if (item.x < bounds.xmin)
				bounds.xmin = item.x - eps;
			if (item.x > bounds.xmax)
				bounds.xmax = item.x + eps;
			if (item.y < bounds.ymin)
				bounds.ymin = item.y - eps;
			if (item.y > bounds.ymax)
				bounds.ymax = item.y + eps;
			if (item.z < bounds.zmin)
				bounds.zmin = item.z - eps;
			if (item.z > bounds.zmax)
				bounds.zmax = item.z + eps;
		}
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
		Point P1 = pts[0];
		Point P2 = pts[1];
		Point P3 = pts[2];
		if (P == P1 || P == P2 || P == P3)
		{
			if (faceNormal == null)
				faceNormal = V.normalize(V.cross(V.PDiff(P2, P1), V.PDiff(P3, P1)));
			return faceNormal;
		}
		UVW bc = getBarycentricCoordsAtPoint(P);

		//Scale each normal by its barycentric value, add them all together, then normalize.  Voila - normal at point P
		Point N1 = V.vsMult(bc.u, new Point(P1.nx, P1.ny, P1.nz));
		Point N2 = V.vsMult(bc.v, new Point(P2.nx, P2.ny, P2.nz));
		Point N3 = V.vsMult(bc.w, new Point(P3.nx, P3.ny, P3.nz));
		Point normal = V.normalize(V.sumVV(N3, V.sumVV(N1, N2)));
		//double cos = V.dot(P, normal);
		//if (cos < 0)
		//{
		//    normal.x *= -1;
		//    normal.y *= -1;
		//    normal.z *= -1;
		//}
		return normal;
	}

	public struct UVW
	{
		public double u;
		public double v;
		public double w;
	}
	

	public UVW getBarycentricCoordsAtPoint(Point P)
	{
		//Point P1 = pts[0];
		//Point P2 = pts[1];
		//Point P3 = pts[2];

		//double t1 = Math.Abs(V.VLength(V.cross(V.PDiff(P2, P), V.PDiff(P3, P))));
		//double t2 = Math.Abs(V.VLength(V.cross(V.PDiff(P3, P), V.PDiff(P1, P))));
		//double t3 = Math.Abs(V.VLength(V.cross(V.PDiff(P1, P), V.PDiff(P2, P))));
		//double total = t1 + t2 + t3;
		//double p1 = t1 / total;
		//double p2 = t2 / total;
		//double p3 = t3 / total;

		//return new Tuple<double, double, double>(p1, p2, p3);
		
		Point v2 = V.PDiff(P, pts[0]);

		double d20 = V.dot(v2, v0);
		double d21 = V.dot(v2, v1);
		double vC = (d11 * d20 - d01 * d21) * invDenom;
		double wC = (d00 * d21 - d01 * d20) * invDenom;
		double uC = 1.0f - vC - wC;

		return new UVW {u = uC, v=vC, w=wC };
	}

	public override double IntersectDistance(Point Ray)
	{
		Point A = pts[0];
		Point B = pts[1];
		Point C = pts[2];

		Point N = triNormal(A);
		double T = V.dot(N, V.PDiff(A, PP.E)) / V.dot(N, Ray);
		return T;
	}

	public override BBox getBounds()
	{
		return bounds;
	}
}


