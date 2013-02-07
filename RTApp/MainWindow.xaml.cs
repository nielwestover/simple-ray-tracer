using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Diagnostics;

namespace RTApp
{

	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		
		Bitmap bitmap, bitmap2;
		const int numSpheres = 1;
		Sphere[] spheres = new Sphere[numSpheres];
		const int numTris = 1;
		Tri[] tris = new Tri[numTris];
		
		bool obstructionBetween(Point A, Point ColorMe)
		{
			Point Ray = V.normalize(V.PDiff(A, ColorMe));
			for (int k = 0; k < numSpheres; k++)
			{
				double a = Ray.xNorm * Ray.xNorm + Ray.yNorm * Ray.yNorm + Ray.zNorm * Ray.zNorm;
				double b = 2 * ((ColorMe.x - spheres[k].x) * Ray.xNorm + (ColorMe.y - spheres[k].y) * Ray.yNorm + (ColorMe.z - spheres[k].z) * Ray.zNorm);
				double c = ((ColorMe.x - spheres[k].x) * (ColorMe.x - spheres[k].x) + (ColorMe.y - spheres[k].y) * (ColorMe.y - spheres[k].y) + (ColorMe.z - spheres[k].z) * (ColorMe.z - spheres[k].z) - spheres[k].radius * spheres[k].radius);
				//cout<<"OUT"<<endl;
				if (b * b > 4 * a * c)
				{
					double squareRt = Math.Sqrt(b * b - 4 * a * c);
					//cout<<"IN"<<endl;
					double T1 = (-b + squareRt) / (2 * a);
					double T2 = (-b - squareRt) / (2 * a);

					if (T1 > 0.1 || T2 > 0.1)
					{
						//cout<<"SPHERE: "<<T1<<"   "<<T2<<endl;
						return true;
					}
				}

			}
			for (int k = 0; k < numTris; k++)
			{
				Point Aa = tris[k].pts[0];
				Point B = tris[k].pts[1];
				Point C = tris[k].pts[2];
				int fe = V.Vj;
				Point N = tris[k].triNormal(Aa);
				//N.Print();
				double D = -(N.xNorm * Aa.x) - (N.yNorm * Aa.y) - (N.zNorm * Aa.z);
				double T2 = -(N.xNorm * PP.E.x + N.yNorm * PP.E.y + N.zNorm * PP.E.z + D) / (N.xNorm * Ray.xNorm + N.yNorm * Ray.yNorm + N.zNorm * Ray.zNorm);

				double T1 = V.dot(N, V.PDiff(Aa, ColorMe)) / V.dot(N, Ray);
				if (T1 < .0001)
					continue;
				Point P = V.sumPV(ColorMe, V.vsMult(T1, Ray));

				Point v0 = V.PDiff(C, Aa);
				Point v1 = V.PDiff(B, Aa);
				Point v2 = V.PDiff(P, Aa);

				// Compute dot products
				double dot00 = V.dot(v0, v0);
				double dot01 = V.dot(v0, v1);
				double dot02 = V.dot(v0, v2);
				double dot11 = V.dot(v1, v1);
				double dot12 = V.dot(v1, v2);

				// Compute barycentric coordinates
				double invDenom = 1.0 / (dot00 * dot11 - dot01 * dot01);
				double uC = (dot11 * dot02 - dot01 * dot12) * invDenom;
				double vC = (dot00 * dot12 - dot01 * dot02) * invDenom;

				// Check if point is in triangle
				if ((uC >= 0) && (vC >= 0) && (uC + vC < 1))
				{
					return true;
				}


			}
			return false;
		}

		void colorPoint(int i, int j, Point P, Point N, int SI)
		{
			//cout<<endl<<"P: ";
			//P.Print();
			//cout<<"N: ";
			//N.Print();
			Point L = V.normalize(V.PDiff(PP.LS, P));
			//cout<<"L: ";
			//L.Print();
			double LdotN = V.dot(L, N);
			//cout<<"LDOTN: "<<LdotN<<endl;
			Point ACC = V.vsMult(2 * LdotN, N);
			Point R = new Point(0, 0, 0, ACC.xNorm - L.xNorm, ACC.yNorm - L.yNorm, ACC.zNorm - L.zNorm);
			//cout<<"R: ";
			//R.Print();
			double Ca = PP.ambient + (1 - PP.ambient) * LdotN;
			//cout<<"Ca: "<<Ca<<endl;
			double ACC2 = V.dot(V.normalize(R), V.normalize(V.PDiff(PP.E, P)));
			//cout<<"ACC2: "<<ACC2<<endl;
			double Cs = Math.Pow(ACC2, PP.Pexp);
			//cout<<"Cs: "<<Cs<<endl;
			double newR, newG, newB;
			if (SI < 10)
			{
				newR = (Ca * spheres[SI].color.r + (1 - Ca * spheres[SI].color.r) * Cs);
				newG = (Ca * spheres[SI].color.g + (1 - Ca * spheres[SI].color.g) * Cs);
				newB = (Ca * spheres[SI].color.b + (1 - Ca * spheres[SI].color.b) * Cs);
			}
			else
			{
				newR = (Ca * tris[SI - 10].color.r + (1 - Ca * tris[SI - 10].color.r) * Cs);
				newG = (Ca * tris[SI - 10].color.g + (1 - Ca * tris[SI - 10].color.g) * Cs);
				newB = (Ca * tris[SI - 10].color.b + (1 - Ca * tris[SI - 10].color.b) * Cs);
			}

			setPixelColor(i, j, newR, newG, newB);

		}
		void colorAmbient(int i, int j, Point P, Point N, int SI)
		{
			//cout<<endl<<"P: ";
			//P.Print();
			//cout<<"N: ";
			//N.Print();
			Point L = V.normalize(V.PDiff(PP.LS, P));
			//cout<<"L: ";
			//L.Print();
			double LdotN = V.dot(L, N);
			//cout<<"LDOTN: "<<LdotN<<endl;
			double Ca = PP.ambient + (1 - PP.ambient) * LdotN;
			double newR, newG, newB;
			if (SI < 10)
			{
				newR = PP.ambient * spheres[SI].color.r;
				newG = PP.ambient * spheres[SI].color.g;
				newB = PP.ambient * spheres[SI].color.b;
			}
			else
			{
				newR = PP.ambient * tris[SI - 10].color.r;
				newG = PP.ambient * tris[SI - 10].color.g;
				newB = PP.ambient * tris[SI - 10].color.b;
			}
			setPixelColor(i, j, newR, newG, newB);
		}

		public MainWindow()
		{
			InitializeComponent();

			spheres[0] = new Sphere(0, 0, -5, 4, 1, 0, 0);
			//spheres[1] = new Sphere(3, 5, -5, 2, 0, 1, 0);
			//spheres[1] = new Sphere(-4, 10, 0, 1, 0, 1, 0);
			//spheres[2] = new Sphere(10, 10, 0, 3, 0, 0, 1);
			//spheres[3] = new Sphere(-2, 10, -5, 2, 0, 1, 1);
			tris[0] = new Tri(new Point(3, 4, 0), new Point(3, -4, 0), new Point(6, -4, 0), new RGB(0, 1,0));
			//tris[1] = new Tri(new Point(2,2, -5), new Point(5, 7, -5), new Point(12, 12, -5), new RGB(.5, 1, 1));
			Width = PP.XRES;
			Height = PP.YRES;
			//this.WindowState = WindowState.Maximized;
			bitmap = new Bitmap((int)PP.XRES, (int)PP.YRES);
			bitmap2 = new Bitmap((int)PP.XRES, (int)PP.YRES);

			Point w = V.normalize(V.PDiff(PP.At, PP.E));
			Point u = V.normalize(V.cross(w, PP.Up));
			Point v = V.cross(u, w);

			for (int j = 0; j < PP.YRES; j++)
			{
				for (int i = 0; i < PP.XRES; i++)
				{
					double Cu = ((2.0 * (double)i + 1.0) / (2.0 * PP.XRES) - .5) * PP.Lu;
					double Cv = -((2.0 * (double)j + 1.0) / (2.0 * PP.YRES) - .5) * PP.Lv;
					Point Pij = V.sumPV(PP.At, V.sumVV(V.vsMult(Cu, u), V.vsMult(Cv, v)));
					Point Ray = V.normalize(V.PDiff(Pij, PP.E));
					//cout<<"RAY: "<<endl;
					//Ray.Print();
					int SI = -1;//sphereIndex
					double T = 1000000000;
					for (int k = 0; k < numSpheres; k++)
					{
						//calculate T for each sphere
						double a = Ray.xNorm * Ray.xNorm + Ray.yNorm * Ray.yNorm + Ray.zNorm * Ray.zNorm;
						double b = 2 * ((PP.E.x - spheres[k].x) * Ray.xNorm + (PP.E.y - spheres[k].y) * Ray.yNorm + (PP.E.z - spheres[k].z) * Ray.zNorm);
						double c = ((PP.E.x - spheres[k].x) * (PP.E.x - spheres[k].x) + (PP.E.y - spheres[k].y) * (PP.E.y - spheres[k].y) + (PP.E.z - spheres[k].z) * (PP.E.z - spheres[k].z) - spheres[k].radius * spheres[k].radius);
						//cout<<"OUT"<<endl;
						if (b * b > 4 * a * c)
						{
							double squareRt = Math.Sqrt(b * b - 4 * a * c);
							//cout<<"IN"<<endl;
							double T1 = (-b + squareRt) / (2 * a);
							double T2 = (-b - squareRt) / (2 * a);

							if (T1 > 0 && T1 <= T2 && T1 < T)
							{
								T = T1;
								SI = k;
								//cout<<"T: "<<T<<"    S: "<<SI<<endl;
							}
							else if (T2 > 0 && T2 <= T1 && T2 < T)
							{
								T = T2;
								SI = k;
								//cout<<"T: "<<T<<"    S: "<<SI<<endl;
							}


						}
					}

					//cout<<"FINAL T: "<<T<<"    FINAL S: "<<SI<<endl;
					//compute T for Triangles as well here, find lowest T value
					for (int k = 0; k < numTris; k++)
					{
						Point A = tris[k].pts[0];
						Point B = tris[k].pts[1];
						Point C = tris[k].pts[2];

						Point N = tris[k].triNormal(A);
						//N.Print();
						double D = -(N.xNorm * A.x) - (N.yNorm * A.y) - (N.zNorm * A.z);
						double T2 = -(N.xNorm * PP.E.x + N.yNorm * PP.E.y + N.zNorm * PP.E.z + D) / (N.xNorm * Ray.xNorm + N.yNorm * Ray.yNorm + N.zNorm * Ray.zNorm);
						double T1 = V.dot(N, V.PDiff(A, PP.E)) / V.dot(N, Ray);
						if (j == PP.YRES / 2 && i == PP.XRES / 2)
 							Debug.Print("stop");
						Point P = V.sumPV(PP.E, V.vsMult(T1, Ray));

						Point v0 = V.PDiff(C, A);
						Point v1 = V.PDiff(B, A);
						Point v2 = V.PDiff(P, A);

						// Compute dot products
						double dot00 = V.dot(v0, v0);
						double dot01 = V.dot(v0, v1);
						double dot02 = V.dot(v0, v2);
						double dot11 = V.dot(v1, v1);
						double dot12 = V.dot(v1, v2);

						// Compute barycentric coordinates
						double invDenom = 1.0 / (dot00 * dot11 - dot01 * dot01);
						double uC = (dot11 * dot02 - dot01 * dot12) * invDenom;
						double vC = (dot00 * dot12 - dot01 * dot02) * invDenom;

						// Check if point is in triangle
						if ((uC >= 0) && (vC >= 0) && (uC + vC < 1.0))
						{
							if (T1 < T)
						 	{
						 		T = T1;
						 		SI = k + 10;
						 	}
						}
					}
					if (T != 1000000000)
					{
						Point ColorMe = V.sumPV(PP.E, V.vsMult(T, Ray));
						bool obs = obstructionBetween(PP.LS, ColorMe);
						//cout<<"OBS: "<<obs<<endl;
						Point N;
						if (SI < 10)
							N = V.normalize(spheres[SI].GetSphereNormal(ColorMe));
						else
							N = tris[SI - 10].triNormal(V.PDiff(PP.E, tris[SI - 10].pts[0]));
						if (!obs)
							colorPoint(i, j, ColorMe, N, SI);
						else
							colorAmbient(i, j, ColorMe, N, SI);
					}
					else
						setPixelColor(i, j, 0, 0, 0);
				}
			}

			antialias(4);
			WindowState = WindowState.Maximized;
			image.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
			bitmap.GetHbitmap(),
			IntPtr.Zero,
			Int32Rect.Empty,
			BitmapSizeOptions.FromEmptyOptions());

			image2.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
			bitmap2.GetHbitmap(),
			IntPtr.Zero,
			Int32Rect.Empty,
			BitmapSizeOptions.FromEmptyOptions());
			bitmap.Save("C:/Users/a2558/Documents/mybitmap.bmp");
		}

		private void antialias(int p)
		{
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
		}

		// 		private Color getPixelColor(int x, int y)
		// 		{
		// 			
		// 		}
		void setPixelColor(int x, int y, double r, double g, double b)
		{
			if (double.IsNaN(r) || double.IsNaN(g) || double.IsNaN(b))
				return;
			r = Math.Max(r, 0);
			g = Math.Max(g, 0);
			b = Math.Max(b, 0);
			bitmap.SetPixel(x, y, Color.FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255)));
		}
	}
}
