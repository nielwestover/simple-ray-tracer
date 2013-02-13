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
		public MainWindow()
		{
			InitializeComponent();

			//Width = PP.XRES * 2;
			//Height = PP.YRES;
			//WindowState = WindowState.Maximized;

			RT rt = new RT("C:/Niel/obj/cello.obj");
			//RT rt = new RT("C:/Niel/obj/female elf-obj.obj");
			//RT rt = new RT("C:/Niel/obj/elf-test.obj");
			//RT rt = new RT("C:/Niel/obj/test.obj");
			Bitmap bitmap = rt.GetRayTracedScene();
			//Bitmap bitmap2 = rt.AntialiasedScene(3);

			image.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
			bitmap.GetHbitmap(),
			IntPtr.Zero,
			Int32Rect.Empty,
			BitmapSizeOptions.FromEmptyOptions());
			bitmap.Save("C:/Niel/obj/render.png");

// 			image2.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
// 			bitmap2.GetHbitmap(),
// 			IntPtr.Zero,
// 			Int32Rect.Empty,
// 			BitmapSizeOptions.FromEmptyOptions());
		}

		
	}
}
