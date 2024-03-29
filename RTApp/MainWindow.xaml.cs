﻿using System;
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
		public MainWindow()
		{
			InitializeComponent();

			Width = Math.Max(PP.XRES, 200);
			Height = Math.Max(PP.YRES, 200);
			WindowState = WindowState.Normal;
			

			//RT rt = new RT("C:/Niel/obj/cello.obj");
			//RT rt = new RT("C:/Niel/obj/female elf-obj.obj");
			RT rt = new RT("C:/Niel/obj/girl.obj");
			//RT rt = new RT("C:/Niel/obj/elf-test.obj");
			//RT rt = new RT("C:/Niel/obj/test.obj");
			//RT rt = new RT("C:/Niel/obj/teapot.obj");
			double Tau = 2 * Math.PI;
			int count = 0;
			double numRenders = 180;
			double i = 0;
			//for (i = 0; i < Tau; i += Tau / numRenders)
			//{
				System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
				sw.Start();
				System.Console.WriteLine(count + " of " + numRenders);
				//PP.E.x = Math.Sin(i) * 100;
				//PP.E.z = Math.Cos(i) * 100;
				Bitmap bitmap = rt.GetRayTracedScene();


				image.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
				bitmap.GetHbitmap(),
				IntPtr.Zero,
				Int32Rect.Empty,
				BitmapSizeOptions.FromEmptyOptions());
				bitmap.Save("C:/Niel/obj/render" + (count++).ToString("") + ".png");
				System.Console.WriteLine("Elapsed: " + (double)(sw.ElapsedMilliseconds / 1000.0) + " s");

			//}

		}

		
	}
}
