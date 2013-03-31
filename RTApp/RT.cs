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
		Node root = new Node();
		List<Shape> shapes = new List<Shape>();
		Bitmap bitmap = new Bitmap(PP.TOTALXRES, PP.TOTALYRES);

		bool obstructionBetween(Point A, Point ColorMe)
		{
			Point Ray = V.normalize(V.PDiff(A, ColorMe));
			List<BBox> bboxList = new List<BBox>();
			root.getBoxIntersections(Ray, ColorMe, bboxList);
			Shape winner = null;
			foreach (BBox curBBox in bboxList)
			{
				//calculate T for each shape
				foreach (Shape shape in curBBox.members)
				{
					//if (i == 40 && j == 11 && (shape as Tri).ID == 3817)
					//	Debug.Print((shape as Tri).ID.ToString());
					if (shape.bounds.intersectsBox(ColorMe, Ray))
					{
						if (shape is Sphere)
						{
							Sphere s = shape as Sphere;
							double a = Ray.x * Ray.x + Ray.y * Ray.y + Ray.z * Ray.z;
							double b = 2 * ((ColorMe.x - s.center.x) * Ray.x + (ColorMe.y - s.center.y) * Ray.y + (ColorMe.z - s.center.z) * Ray.z);
							double c = ((ColorMe.x - s.center.x) * (ColorMe.x - s.center.x) + (ColorMe.y - s.center.y) * (ColorMe.y - s.center.y) + (ColorMe.z - s.center.z) * (ColorMe.z - s.center.z) - s.radius * s.radius);
							if (b * b > 4 * a * c)
							{
								double squareRt = Math.Sqrt(b * b - 4 * a * c);
								double T1 = (-b + squareRt) / (2 * a);
								double T2 = (-b - squareRt) / (2 * a);

								if (T1 > 0.00000001 || T2 > 0.00000001)
								{
									return true;
								}
							}
						}
						if (shape is Tri)
						{
							Tri tri = shape as Tri;
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
				}
			}
			return false;
		}

		RGB getPhongColorAtPoint(int i, int j, Point P, Point N, Shape shape)
		{
			double Ks = 1;
			double Kd = .6;
			double Ka = 0;
			double alpha = 100;
			RGB Ia = shape.color;
			RGB Id = shape.color;

			RGB final = Ka * Ia;
			foreach (Point m in PP.Lights)
			{
				Point Lm = V.normalize(V.PDiff(m, P));
				double LmdotN = V.dot(Lm, N);
				double TwoLmdotN = 2 * LmdotN;
				Point TwoLmdotN_N = V.vsMult(TwoLmdotN, N);
				Point Rm = V.normalize(V.sumVV(TwoLmdotN_N, V.vsMult(-1, Lm)));
				Point E = V.normalize(V.PDiff(PP.E, P));
				double RmdotV = V.dot(Rm, E);
				if (LmdotN > 0)
				{
					final += Kd * LmdotN * Id;
				}
				RGB white = new RGB(1,1,1);
				if (RmdotV > 0)
				{
					final += Ks * Math.Pow(RmdotV, alpha) * white;
				}
			}

			return final;

		}

// 		RGB getAmbientColor(int i, int j, Point P, Point N, Shape shape)
// 		{
// 			Point L = V.normalize(V.PDiff(LS, P));
// 			double LdotN = V.dot(L, N);
// 			double Ca = PP.ambient + (1 - PP.ambient) * LdotN;
// 			double newR = Ca * winner.color.r;
// 			double newG = Ca * winner.color.g;
// 			double newB = Ca * winner.color.b;
// 			return new RGB(newR, newG, newB);
// 		}

		private System.Drawing.Bitmap antialias(Bitmap rt)
		{
			int totalSamples = PP.samples * PP.samples;
			Bitmap sampled = new Bitmap(PP.XRES, PP.YRES);
			
			int r, g, b;
			for (int j = 0; j < PP.YRES; ++j)
			{
				for (int i = 0; i < PP.XRES; ++i)
				{
					r = g = b = 0;
					for (int xsample = 0; xsample < PP.samples; ++xsample)
					{
						for (int ysample = 0; ysample < PP.samples; ++ysample)
						{
							Color pixel = rt.GetPixel(i * PP.samples + xsample, j * PP.samples + ysample);
							r += pixel.R;
							g += pixel.G;
							b += pixel.B;

						}
					}
					r /= totalSamples;
					g /= totalSamples;
					b /= totalSamples;
					sampled.SetPixel(i, j, Color.FromArgb(r, g, b));
				}
			}
			return sampled;
		}

		void setPixelColor(int x, int y, RGB color)
		{
			//if (double.IsNaN(r) || double.IsNaN(g) || double.IsNaN(b))
			//	return;
			color.Clamp();
			int r = (int)Math.Max(color.r * 255, 1);
			int g = (int)Math.Max(color.g * 255, 1);
			int b = (int)Math.Max(color.b * 255, 1);
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
			int i = 0;
			//BBox modelBounds = new BBox();
			foreach (var item in md.Tris)
			{
				Point a = new Point(md.Vertices[item.P1.Vertex], md.TexCoords[item.P1.TexCoord], md.Normals[item.P1.Normal]);
				Point b = new Point(md.Vertices[item.P2.Vertex], md.TexCoords[item.P2.TexCoord], md.Normals[item.P2.Normal]);
				Point c = new Point(md.Vertices[item.P3.Vertex], md.TexCoords[item.P3.TexCoord], md.Normals[item.P3.Normal]);
				Tri t = new Tri(a, b, c, new RGB(1, 1, 1), i++);
				shapes.Add(t);
				//BBox l = t.getBounds();
				//modelBounds.expand(l);
			}

			//Bounds ls = s.getBounds();
			//modelBounds.expand(ls);

			//modelBounds.print();

			root = new Node();
			root.bbox.members = shapes;
			root.bbox.computeLimits();

			medianSplit(root, 0);
		}

		const int MAX_OBJECTS = 12;
		const int MAX_SPLIT_DEPTH = 31;

		private void medianSplit(Node curNode, int depth)
		{
			//if (curNode.bbox.members.Any(n => (n as Tri).ID == 4434))
			//{
			//	Debug.Print("Found at depth " + depth);
			//}
			if (curNode.bbox.members.Count <= MAX_OBJECTS)
				return;
			else if (depth >= MAX_SPLIT_DEPTH)
				return;
			else
			{
				double xdiff = curNode.bbox.xmax - curNode.bbox.xmin;
				double ydiff = curNode.bbox.ymax - curNode.bbox.ymin;
				double zdiff = curNode.bbox.zmax - curNode.bbox.zmin;

				BBox bbox1;
				BBox bbox2;
				BBox.MEDIAN_DIRECTION dir;
				if (xdiff >= ydiff && xdiff >= zdiff)
				{
					double maxx1;
					maxx1 = curNode.bbox.getMedian(dir = RTApp.BBox.MEDIAN_DIRECTION.X, xdiff);
					double minx2 = maxx1;
					bbox1 = new BBox(curNode.bbox.xmin, curNode.bbox.ymin, curNode.bbox.zmin, maxx1, curNode.bbox.ymax, curNode.bbox.zmax);
					bbox2 = new BBox(minx2, curNode.bbox.ymin, curNode.bbox.zmin, curNode.bbox.xmax, curNode.bbox.ymax, curNode.bbox.zmax);
				}
				else if (ydiff >= xdiff && ydiff >= zdiff)
				{
					double maxy1;
					maxy1 = curNode.bbox.getMedian(dir = RTApp.BBox.MEDIAN_DIRECTION.Y, ydiff);
					double miny2 = maxy1;
					bbox1 = new BBox(curNode.bbox.xmin, curNode.bbox.ymin, curNode.bbox.zmin, curNode.bbox.xmax, maxy1, curNode.bbox.zmax);
					bbox2 = new BBox(curNode.bbox.xmin, miny2, curNode.bbox.zmin, curNode.bbox.xmax, curNode.bbox.ymax, curNode.bbox.zmax);
				}
				else
				{
					double maxz1;
					maxz1 = curNode.bbox.getMedian(dir = RTApp.BBox.MEDIAN_DIRECTION.Z, zdiff);
					double minz2 = maxz1;
					bbox1 = new BBox(curNode.bbox.xmin, curNode.bbox.ymin, curNode.bbox.zmin, curNode.bbox.xmax, curNode.bbox.ymax, maxz1);
					bbox2 = new BBox(curNode.bbox.xmin, curNode.bbox.ymin, minz2, curNode.bbox.xmax, curNode.bbox.ymax, curNode.bbox.zmax);
				}
				foreach (var item in curNode.bbox.members)
				{
					//if ((item as Tri).ID == 4434)
					//	Debug.Flush();
					if (bbox1.InBox(item))
					{
						//if ((item as Tri).ID == 4434)
						//	Debug.Print(dir.ToString() + " 0" + " D" + depth);
						bbox1.members.Add(item);
					}
					if (bbox2.InBox(item))
					{
						//if ((item as Tri).ID == 4434)
						//	Debug.Print(dir.ToString() + " 1" + " D" + depth);
						bbox2.members.Add(item);
					}
				}
				curNode.bbox.members.Clear();
				if (bbox1.members.Count > 0)
				{
					Node n = new Node();
					n.bbox = bbox1;
					curNode.children.Add(n);
				}

				if (bbox2.members.Count > 0)
				{
					Node n = new Node();
					n.bbox = bbox2;
					curNode.children.Add(n);
				}

				foreach (Node n in curNode.children)
				{
					medianSplit(n, depth+1);
				}
			}
		}

		internal System.Drawing.Bitmap GetRayTracedScene()
		{
			LoadShapes();

			Point w = V.normalize(V.PDiff(PP.At, PP.E));
			Point u = V.normalize(V.cross(w, PP.Up));
			Point v = V.cross(u, w);

			for (int j = 0; j < PP.TOTALYRES; j++)
			{
				if (j % 10 == 0)
					System.Console.WriteLine(j + " of " + PP.TOTALYRES);
				for (int i = 0; i < PP.TOTALXRES; i++)
				{
					double Cu = ((2.0 * (double)i + 1.0) / (2.0 * PP.TOTALXRES) - .5) * PP.Lu;
					double Cv = -((2.0 * (double)j + 1.0) / (2.0 * PP.TOTALYRES) - .5) * PP.Lv;
					Point Pij = V.sumPV(PP.At, V.sumVV(V.vsMult(Cu, u), V.vsMult(Cv, v)));
					RGB pixelColor = getColorAtPixel(i, j, Pij);
					setPixelColor(i, j, pixelColor);
				}
			}
			return antialias(bitmap);

		}

		private RGB getColorAtPixel(int i, int j, Point Pij)
		{
			//if (i == 40 && j == 11)
			//	Debug.Print("YO GABBA GABBA");
			Point Ray = V.normalize(V.PDiff(Pij, PP.E));

			double T = double.PositiveInfinity;
			List<BBox> bboxList = new List<BBox>();
			root.getBoxIntersections(Ray, Pij, bboxList);
			Shape winner = null;
			foreach (BBox curBBox in bboxList)
			{
				//calculate T for each shape
				foreach (Shape shape in curBBox.members)
				{
					//if (i == 40 && j == 11 && (shape as Tri).ID == 3817)
					//	Debug.Print((shape as Tri).ID.ToString());
					if (shape.bounds.intersectsBox(Pij, Ray))
					{
						if (shape is Tri)
						{
							
							//Calc T
							double newT = shape.IntersectDistance(Ray);
							//Find intersection point at distance T
							Point P = V.sumPV(PP.E, V.vsMult(newT, Ray));

							//Get barycentric coords for intersect point
							Tri.UVW bc = (shape as Tri).getBarycentricCoordsAtPoint(P);

							//See if intersect point is inside triangle
							if ((bc.v >= 0) && (bc.w >= 0) && (bc.v + bc.w < 1.0))
							{
								//Closer than old T? Update T and index
								if (newT < T)
								{
									T = newT;
									winner = shape;
								}
							}
						}
						else if (shape is Sphere)
						{
							double newT = shape.IntersectDistance(Ray);

							if (newT != double.PositiveInfinity)
							{
								T = newT;
								winner = shape;
							}
						}
					}
				}				
			}
			RGB colorAtPixel = new RGB(0, 0, 0);
			if (T != double.PositiveInfinity)
			{
				Point ColorMe = V.sumPV(PP.E, V.vsMult(T, Ray));
				//bool obs = obstructionBetween(LS, ColorMe);
				Point N;
				if (winner is Sphere)
					N = V.normalize((winner as Sphere).GetSphereNormal(ColorMe));
				else
					N = (winner as Tri).triNormal(ColorMe);
				//if (!obs)
					colorAtPixel += getPhongColorAtPoint(i, j, ColorMe, N, winner);
				//else
				//	colorAtPixel += getAmbientColor(i, j, ColorMe, N, winner);
			}
			return colorAtPixel;
		}
	}
}
