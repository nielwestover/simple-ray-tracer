using System;

public static class PP
{
	public const double XRES = 400;
	public const double YRES = 400;

	public static Point Up = new Point(0, 0, 0, 0, 1, 0);
	public static Point E = new Point(0, 0, 10, 0, 0, 0);
	public static Point At = new Point(0, 0, -1, 0, 0, 0);
	public static Point LS = new Point(500, 0, 300, 0, 0, 0);//800,0,-1000,0,0,0);
	public const double Lu = 10;
	public const double Lv = 10;
	public const double ambient = .1;
	public const double Pexp = 3;
}
