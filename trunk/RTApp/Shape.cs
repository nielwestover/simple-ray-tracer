using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTApp
{
	public class BBox
	{
		const double eps = 0;//.0000001;
	
		public List<Shape> members = new List<Shape>();

		public double xmin = double.PositiveInfinity;
		public double xmax = double.NegativeInfinity;
		public double ymin = double.PositiveInfinity;
		public double ymax = double.NegativeInfinity;
		public double zmin = double.PositiveInfinity;
		public double zmax = double.NegativeInfinity;

		public BBox() { }
		public BBox(double xmin, double ymin, double zmin, double xmax, double ymax, double zmax)
		{
			this.xmin = xmin;
			this.ymin = ymin;
			this.zmin = zmin;
			this.xmax = xmax;
			this.ymax = ymax;
			this.zmax = zmax;
		}

		internal void expand(BBox l)
		{
			if (l.xmin < xmin)
				xmin = l.xmin - eps;
			if (l.ymin < ymin)
				ymin = l.ymin - eps;
			if (l.zmin < zmin)
				zmin = l.zmin - eps;
			if (l.xmax > xmax)
				xmax = l.xmax + eps;
			if (l.ymax > ymax)
				ymax = l.ymax + eps;
			if (l.zmax > zmax)
				zmax = l.zmax + eps;
		}

		public void computeLimits()
		{
			foreach (Shape item in members)
			{
				expand(item.bounds);
			}
		}

		internal void print()
		{
			System.Console.WriteLine("BOUNDS:");
			System.Console.WriteLine("			YMAX: " + ymax);
			System.Console.Write("XMIN: " + xmin);
			System.Console.WriteLine("				XMAX: " + xmax);
			System.Console.WriteLine("			YMIN: " + ymin);
			System.Console.WriteLine("\n			ZMAX: " + zmax);
			System.Console.WriteLine("			ZMIN: " + zmin);
			System.Console.WriteLine("XMID: " + (xmin + xmax) / 2);
			System.Console.WriteLine("YMID: " + (ymin + ymax) / 2);
			System.Console.WriteLine("ZMID: " + (zmin + zmax) / 2);
		}
		public enum MEDIAN_DIRECTION
		{
			X,
			Y,
			Z
		};
		public double getMedian(MEDIAN_DIRECTION direction, double d_diff)
		{
			double xdiff = xmax - xmin;
			double ydiff = ymax - ymin;
			double zdiff = zmax - zmin;

			List<double> vals = new List<double>();
			foreach (var item in members)
			{
				if (item is Sphere)
				{
					Sphere sph = (Sphere)item;
					switch (direction)
					{
						case MEDIAN_DIRECTION.X:
							vals.Add(sph.center.x);
							break;
						case MEDIAN_DIRECTION.Y:
							vals.Add(sph.center.y);
							break;
						default:
						case MEDIAN_DIRECTION.Z:
							vals.Add(sph.center.z);
							break;
					}
				}
				else if (item is Tri)
				{
					switch (direction)
					{
						case MEDIAN_DIRECTION.X:
							vals.Add(item.bounds.xmin);
							break;
						case MEDIAN_DIRECTION.Y:
							vals.Add(item.bounds.ymin);
							break;
						case MEDIAN_DIRECTION.Z:
						default:
							vals.Add(item.bounds.zmin);
							break;
					}
				}
			}
			vals = vals.OrderBy(x => x).ToList();
			int size = vals.Count;

			double median;

			switch (direction)
			{
				case MEDIAN_DIRECTION.X:
					median = .5 * (xmin + .5 * d_diff + vals[size / 2]);
					break;
				case MEDIAN_DIRECTION.Y:
					median = .5 * (ymin + .5 * d_diff + vals[size / 2]);
					break;
				case MEDIAN_DIRECTION.Z:
				default:
					median = .5 * (zmin + .5 * d_diff + vals[size / 2]);
					break;
			}

			return median + 2 * eps;

		}

		internal bool InBox(Shape item)
		{
			if (item is Sphere)
			{
				Sphere s = item as Sphere;
				Point p = s.center;
				double r = s.radius;
				if (PointInBox(p))
					return true;
				if (s.bounds.xmin <= xmax && s.bounds.xmax >= xmin)
					if (s.bounds.ymin <= ymax && s.bounds.ymax >= ymin)
						if (s.bounds.zmin <= zmax && s.bounds.zmax >= zmin)
							return true;
			}
			else if (item is Tri)
			{
				Tri t = item as Tri;
				foreach (Point pt in t.pts)
				{
					if (PointInBox(pt))
						return true;
				}
				if (t.bounds.xmin <= xmax && t.bounds.xmax >= xmin)
					if (t.bounds.ymin <= ymax && t.bounds.ymax >= ymin)
						if (t.bounds.zmin <= zmax && t.bounds.zmax >= zmin)
							return true;
			}
			return false;
		}

		public bool intersectsBox(Point origin, Point Ray)
		{
// 			double tnear = double.NegativeInfinity;
// 			double tfar = double.PositiveInfinity;
// 			if (Ray.x == 0 && (origin.x < xmin || origin.x > xmax))
// 				return false;
// 			double t1 = (xmin - origin.x) / Ray.x;
// 			double t2 = (xmax - origin.x) / Ray.x;
// 			if (t1 > t2)
// 			{
// 				double temp = t1;
// 				t1 = t2;
// 				t2 = temp;
// 			}
// 			if (t1 > tnear) tnear = t1;
// 			if (t2 < tfar) tfar = t2;
// 			if (tnear > tfar) return false;
// 			if (tfar < 0) return false;
// 
// 			//same for Y direction;
// 			if (Ray.y == 0 && (origin.y < ymin || origin.y > ymax))
// 				return false;
// 			t1 = (ymin - origin.y) / Ray.y;
// 			t2 = (ymax - origin.y) / Ray.y;
// 			if (t1 > t2)
// 			{
// 				double temp = t1;
// 				t1 = t2;
// 				t2 = temp;
// 			}
// 			if (t1 > tnear) tnear = t1;
// 			if (t2 < tfar) tfar = t2;
// 			if (tnear > tfar) return false;
// 			if (tfar < 0) return false;
// 
// 			//same for Z direction;
// 			if (Ray.z == 0 && (origin.z < zmin || origin.z > zmax))
// 				return false;
// 			t1 = (zmin - origin.z) / Ray.z;
// 			t2 = (zmax - origin.z) / Ray.z;
// 			if (t1 > t2)
// 			{
// 				double temp = t1;
// 				t1 = t2;
// 				t2 = temp;
// 			}
// 			if (t1 > tnear) tnear = t1;
// 			if (t2 < tfar) tfar = t2;
// 			if (tnear > tfar) return false;
// 			if (tfar < 0) return false;
// 
// 			return true;

			double tmin, tmax, tymin, tymax, tzmin, tzmax;
			if (Ray.x >= 0)
			{
				tmin = (xmin - origin.x) / Ray.x;
				tmax = (xmax - origin.x) / Ray.x;
			}
			else
			{
				tmin = (xmax - origin.x) / Ray.x;
				tmax = (xmin - origin.x) / Ray.x;
			}
			if (Ray.y >= 0)
			{
				tymin = (ymin - origin.y) / Ray.y;
				tymax = (ymax - origin.y) / Ray.y;
			}
			else
			{
				tymin = (ymax - origin.y) / Ray.y;
				tymax = (ymin - origin.y) / Ray.y;
			}
			if ((tmin > tymax) || (tymin > tmax))
				return false;
			if (tymin > tmin)
				tmin = tymin;
			if (tymax < tmax)
				tmax = tymax;
			if (Ray.z >= 0)
			{
				tzmin = (zmin - origin.z) / Ray.z;
				tzmax = (zmax - origin.z) / Ray.z;
			}
			else
			{
				tzmin = (zmax - origin.z) / Ray.z;
				tzmax = (zmin - origin.z) / Ray.z;
			}
			if ((tmin > tzmax) || (tzmin > tmax))
				return false;
			return true;

// 			double tx1 = (xmin - Ray.x) / Ray.x;
// 			double tx2 = (xmax - Ray.x) / Ray.x;
// 
// 			double tmin = Math.Min(tx1, tx2);
// 			double tmax = Math.Max(tx1, tx2);
// 
// 			double ty1 = (ymin - Ray.y) / Ray.y;
// 			double ty2 = (ymax - Ray.y) / Ray.y;
// 
// 			tmin = Math.Min(ty1, ty2);
// 			tmax = Math.Max(ty1, ty2);
// 
// 			double tz1 = (zmin - Ray.z) / Ray.z;
// 			double tz2 = (zmax - Ray.z) / Ray.z;
// 
// 			tmin = Math.Min(tz1, tz2);
// 			tmax = Math.Max(tz1, tz2);
// 
// 			return tmax >= tmin;
		}

		private bool PointInBox(Point p)
		{
			return (p.x >= xmin && p.x <= xmax && p.y >= ymin && p.y <= ymax && p.z >= zmin	&& p.z <= zmax);
		}
	}

	public class Shape
	{
		public RGB color = new RGB();
		public BBox bounds = new BBox();

		public virtual double IntersectDistance(Point Ray)
		{
			return double.PositiveInfinity;
		}
		public virtual BBox getBounds()
		{
			return bounds;
		}
		public virtual void computeBounds()	{}
	}
}
