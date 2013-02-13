using System;

public static class PP
{
	public const double XRES = 50;
	public const double YRES = 50;

	public static Point Up = new Point(0, 1, 0);
	
	//Cello
	// 	public static Point At = new Point(-1, .4, -1);
	// 	public static Point E = new Point(3, 0, 3);
	// 	public static Point LS = new Point(0, 0, 3);//800,0,-1000,0,0,0);
	//public const double Lu = 30;
	//public const double Lv = 300;
	
	//Elf
// 	public static Point At = new Point(0, 230, 0);
// 	public static Point E = new Point(5, 230, 90);
// 	public static Point LS = new Point(50, 230, 100);//800,0,-1000,0,0,0);
// 	public const double Lu = 100;
// 	public const double Lv = 150;

	//Test
	public static Point At = new Point(0, 0, 0);
	public static Point E = new Point(0, 0, 2);
	public static Point LS = new Point(0, 0, 2);//800,0,-1000,0,0,0);
	public const double Lu = 10;
	public const double Lv = 10;


	public const double ambient = .1;
	public const double Pexp = 3;
}
