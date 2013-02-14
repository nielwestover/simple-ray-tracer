using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;

//Reflection vector
//R = I - 2(N ● I)N

namespace RTApp
{
	public class RT
	{
		List<Shape> shapes = new List<Shape>();
		Bitmap bitmap = new Bitmap((int)PP.XRES, (int)PP.YRES);

		bool obstructionBetween(Point A, Point ColorMe)
		{
			Point Ray = V.normalize(V.PDiff(A, ColorMe));
			for (int k = 0; k < shapes.Count; k++)
			{
				if (shapes[k] is Sphere)
				{
					Sphere s = shapes[k] as Sphere;
					double a = Ray.x * Ray.x + Ray.y * Ray.y + Ray.z * Ray.z;
					double b = 2 * ((ColorMe.x - s.center.x) * Ray.x + (ColorMe.y - s.center.y) * Ray.y + (ColorMe.z - s.center.z) * Ray.z);
					double c = ((ColorMe.x - s.center.x) * (ColorMe.x - s.center.x) + (ColorMe.y - s.center.y) * (ColorMe.y - s.center.y) + (ColorMe.z - s.center.z) * (ColorMe.z - s.center.z) - s.radius * s.radius);
					if (b * b > 4 * a * c)
					{
						double squareRt = Math.Sqrt(b * b - 4 * a * c);
						double T1 = (-b + squareRt) / (2 * a);
						double T2 = (-b - squareRt) / (2 * a);

						if (T1 > 0.0001 || T2 > 0.0001)
						{
							return true;
						}
					}
				}
				if (shapes[k] is Tri)
				{
					Tri tri = shapes[k] as Tri;
					Point P1 = tri.pts[0];
					Point P2 = tri.pts[1];
					Point P3 = tri.pts[2];
					Point N = tri.triNormal(P1);
					//N.Print();
					double D = -(N.x * P1.x) - (N.y * P1.y) - (N.z * P1.z);
					double T1 = V.dot(N, V.PDiff(P1, ColorMe)) / V.dot(N, Ray);
					if (T1 < .0001)
						continue;
					Point P = V.sumPV(ColorMe, V.vsMult(T1, Ray));

					Tri.UVW bc = tri.getBarycentricCoordsAtPoint(P);
					if ((bc.v >= 0) && (bc.w >= 0) && (bc.v + bc.w < 1.0))
					{
						return true;
					}
				}

			}
			return false;
		}

		RGB getColorAtPoint(int i, int j, Point P, Point N, int SI)
		{
			Point L = V.normalize(V.PDiff(PP.LS, P));
			double LdotN = V.dot(L, N);
			Point ACC = V.vsMult(2 * LdotN, N);
			Point R = new Point(ACC.x - L.x, ACC.y - L.y, ACC.z - L.z);
			double Ca = PP.ambient + (1 - PP.ambient) * LdotN;
			double ACC2 = V.dot(V.normalize(R), V.normalize(V.PDiff(PP.E, P)));
			double Cs = Math.Pow(ACC2, PP.Pexp);
			
			double newR = (Ca * shapes[SI].color.r + (1 - Ca * shapes[SI].color.r) * Cs);
			double newG = (Ca * shapes[SI].color.g + (1 - Ca * shapes[SI].color.g) * Cs);
			double newB = (Ca * shapes[SI].color.b + (1 - Ca * shapes[SI].color.b) * Cs);
			
			return new RGB(newR, newG, newB);

		}
		RGB getAmbientColor(int i, int j, Point P, Point N, int SI)
		{
			Point L = V.normalize(V.PDiff(PP.LS, P));
			double LdotN = V.dot(L, N);
			double Ca = PP.ambient + (1 - PP.ambient) * LdotN;
			double newR = Ca * shapes[SI].color.r;
			double newG = Ca * shapes[SI].color.g;
			double newB = Ca * shapes[SI].color.b;
			return new RGB(newR, newG, newB);
		}

		private System.Drawing.Bitmap antialias(int p)
		{
			Bitmap bitmap2 = new Bitmap(bitmap.Width, bitmap.Height);
			for (int j = 0; j < PP.YRES; j++)
			{
				for (int i = 0; i < PP.XRES; i++)
				{
					//if (j == 400 && i == 400)
					//	Debug.Print("stop");
					int r = 0;
					int g = 0;
					int b = 0;
					int colors = 0;
					for (int m = 0; m < p; ++m)
					{
						for (int n = 0; n < p; ++n)
						{
							if (j + m < PP.XRES && i + n < PP.YRES)
							{
								Color c = bitmap.GetPixel(j + m, i + n);
								r += c.R;
								g += c.G;
								b += c.B;
								++colors;
							}
						}
					}
					r /= colors;
					g /= colors;
					b /= colors;
					bitmap2.SetPixel(j, i, Color.FromArgb(r, g, b));
				}
			}
			return bitmap2;
		}

		void setPixelColor(int x, int y, RGB color)
		{
			//if (double.IsNaN(r) || double.IsNaN(g) || double.IsNaN(b))
			//	return;
			int r = (int)Math.Max(color.r * 255, 0);
			int g = (int)Math.Max(color.g * 255, 0);
			int b = (int)Math.Max(color.b * 255, 0);
			bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));
		}

		string objFile;
		public RT(string objFile)
		{
			this.objFile = objFile;
		}

		private void LoadShapes()
		{
			Meshomatic.ObjLoader o = new Meshomatic.ObjLoader();
			Meshomatic.MeshData md = o.LoadFile(objFile);
			foreach (var item in md.Tris)
			{
				Point a = new Point(md.Vertices[item.P1.Vertex], md.TexCoords[item.P1.TexCoord], md.Normals[item.P1.Normal]);
				Point b = new Point(md.Vertices[item.P2.Vertex], md.TexCoords[item.P2.TexCoord], md.Normals[item.P2.Normal]);
				Point c = new Point(md.Vertices[item.P3.Vertex], md.TexCoords[item.P3.TexCoord], md.Normals[item.P3.Normal]);
				shapes.Add(new Tri(a, b, c, new RGB(1, .4, .2)));
			}

			shapes.Add(new Sphere(70, 230, 30, 20, 1, 1, 0));
			////spheres[1] = new Sphere(3, 5, -5, 2, 0, 1, 0);
			////spheres[1] = new Sphere(-4, 10, 0, 1, 0, 1, 0);
			////spheres[2] = new Sphere(10, 10, 0, 3, 0, 0, 1);
			////spheres[3] = new Sphere(-2, 10, -5, 2, 0, 1, 1);
			//tris.Add(new Tri(new Point(3, 4, 0), new Point(3, -4, 0), new Point(6, -4, 0), new RGB(0, 1, 0)));
			////tris[1] = new Tri(new Point(2,2, -5), new Point(5, 7, -5), new Point(12, 12, -5), new RGB(.5, 1, 1));
		}

		internal System.Drawing.Bitmap GetRayTracedScene()
		{
			LoadShapes();

			Point w = V.normalize(V.PDiff(PP.At, PP.E));
			Point u = V.normalize(V.cross(w, PP.Up));
			Point v = V.cross(u, w);

			for (int j = 0; j < PP.YRES; j++)
			{
				System.Console.WriteLine((j + 1) + " of " + PP.YRES);
				bitmap.Save("C:/Niel/obj/render.png");
				for (int i = 0; i < PP.XRES; i++)
				{
					double Cu = ((2.0 * (double)i + 1.0) / (2.0 * PP.XRES) - .5) * PP.Lu;
					double Cv = -((2.0 * (double)j + 1.0) / (2.0 * PP.YRES) - .5) * PP.Lv;
					Point Pij = V.sumPV(PP.At, V.sumVV(V.vsMult(Cu, u), V.vsMult(Cv, v)));
					RGB pixelColor = getColorAtPixel(i, j, Pij);
					setPixelColor(i, j, pixelColor);
					
				}
			}
			return bitmap;

		}

		private RGB getColorAtPixel(int i, int j, Point Pij)
		{
			Point Ray = V.normalize(V.PDiff(Pij, PP.E));

			int SI = -1;//sphereIndex
			double T = double.PositiveInfinity;

			//calculate T for each sphere
			for (int k = 0; k < shapes.Count; k++)
			{
				if (shapes[k] is Sphere)
				{

					double newT = shapes[k].IntersectDistance(Ray);
					if (newT != double.PositiveInfinity)
					{
						T = newT;
						SI = k;
					}
				}
				if (shapes[k] is Tri)
				{
					//Calc T
					double newT = shapes[k].IntersectDistance(Ray);
					//Find intersection point at distance T
					Point P = V.sumPV(PP.E, V.vsMult(newT, Ray));

					//Get barycentric coords for intersect point
					Tri.UVW bc = (shapes[k] as Tri).getBarycentricCoordsAtPoint(P);

					//See if intersect point is inside triangle
					if ((bc.v >= 0) && (bc.w >= 0) && (bc.v + bc.w < 1.0))
					{
						//Closer than old T? Update T and index
						if (newT < T)
						{
							T = newT;
							SI = k;
						}
					}
				}
			}
			RGB colorAtPixel;
			if (T != double.PositiveInfinity)
			{
				Point ColorMe = V.sumPV(PP.E, V.vsMult(T, Ray));
				bool obs = obstructionBetween(PP.LS, ColorMe);
				Point N;
				if (shapes[SI] is Sphere)
					N = V.normalize((shapes[SI] as Sphere).GetSphereNormal(ColorMe));
				else
					N = (shapes[SI] as Tri).triNormal(ColorMe);
				if (!obs)
					colorAtPixel = getColorAtPoint(i, j, ColorMe, N, SI);
				else
					colorAtPixel = getAmbientColor(i, j, ColorMe, N, SI);
			}
			else
				colorAtPixel = new RGB(0, 0, 0);
			return colorAtPixel;
		}



		internal System.Drawing.Bitmap AntialiasedScene(int samples)
		{
			return antialias(samples);
		}
	}
}
