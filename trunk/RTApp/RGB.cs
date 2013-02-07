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
}