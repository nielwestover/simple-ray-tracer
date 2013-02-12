// Point.cpp: implementation of the Point class.
//
//////////////////////////////////////////////////////////////////////

public class Point
{

	public RGB color = new RGB();
	public double y;
	public double x;
	public double z;

	public double u;
	public double v;

	public double nx;
	public double ny;
	public double nz;

	public Point()
	{
		x = y = z = 0;
	}


	public Point(double x, double y)
	{
		this.x = x;
		this.y = y;
		//color = RGB((double)(rand()%1000)/1000,(double)(rand()%1000)/1000,(double)(rand()%1000)/1000);
		//	cout<<"X: "<<x<<endl;
		//	cout<<"Y: "<<y<<endl;
	}
	public Point(double x, double y, double z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public Point(Meshomatic.Vector3 v)
	{
		this.x = v.X;
		this.y = v.Y;
		this.z = v.Z;
	}

	public Point(Meshomatic.Vector3 point, Meshomatic.Vector2 tc, Meshomatic.Vector3 normal)
	{
		// TODO: Complete member initialization
		x = point.X;
		y = point.Y;
		z = point.Z;
		u = tc.X;
		v = tc.Y;
		nx = normal.X;
		ny = normal.Y;
		nz = normal.Z;
	}

	public void Print()
	{
		string s = "";
		s += "x: "+ x +
			" y: " + y +
			" z: " + z + 
			" x: " + x + 
			" y: " + y + 
			" z: " + z;
		System.Console.WriteLine(s);
		color.Print();
	}
}
