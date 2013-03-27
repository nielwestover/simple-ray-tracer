using System;

public static class PP
{
	public const double XRES = 75;
	public const double YRES = 100;

	public static Point Up = new Point(0, 1, 0);
	
	//Cello
// 	public static Point At = new Point(0, .3, 0);
// 	public static Point E = new Point(2, 0, 2);
// 	public static Point LS = new Point(60, 0, 0);
// 	public const double Lu = 3;
// 	public const double Lv = 4;

	//Elf
	public static Point At = new Point(0, 230, 0);
	public static Point E = new Point(0, 230, 180);
	public static Point LS = new Point(90, 230, 100);//800,0,-1000,0,0,0);
	public const double Lu = 100;
	public const double Lv = 150;

	//Test
	//public static Point At = new Point(0, 0, 0);
	//public static Point E = new Point(0, 0, 2);
	//public static Point LS = new Point(0, 0, 2);//800,0,-1000,0,0,0);
	//public const double Lu = 5;
	//public const double Lv = 5;

	//Teapot
// 	public static Point At = new Point(0, 0, 0);
// 	public static Point E = new Point(0, 60, 200);
// 	public static Point LS = new Point(200, 0, 200);
// 	public const double Lu = 180;
// 	public const double Lv = 180;

	public const double ambient = .1;
	public const double Pexp = 6;
}
