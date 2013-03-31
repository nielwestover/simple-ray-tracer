// RGB.cpp: implementation of the RGB class.
//
//////////////////////////////////////////////////////////////////////

public class RGB
{
	public double b;
	public double g;
	public double r;
	public double z;

	public RGB() { }
	public RGB(double r, double g, double b)
	{
		this.r = r;
		this.g = g;
		this.b = b;
	}
	public RGB(double r, double g, double b, double z)
	{
		this.r = r;
		this.g = g;
		this.b = b;
		this.z = z;
	}

	public void Print()
	{
		System.Console.WriteLine("R: " + r);
		System.Console.WriteLine("G: " + g);
		System.Console.WriteLine("B: " + b);
	}

	public static RGB operator +(RGB a, RGB b)
	{
		return new RGB(a.r + b.r, a.g + b.g, a.b + b.b);
	}
	public static RGB operator *(double k, RGB I)
	{
		return new RGB(k * I.r, k * I.g, k * I.b);
	}

	internal void Clamp()
	{
		if (r < 0) r = 0;
		if (r > 1) r = 1;
		if (g < 0) g = 0;
		if (g > 1) g = 1;
		if (b < 0) b = 0;
		if (b > 1) b = 1;
	}

	internal void Reset()
	{
		r = 0;
		g = 0;
		b = 0;
	}
}