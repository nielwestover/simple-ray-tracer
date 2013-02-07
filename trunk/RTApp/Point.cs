// Point.cpp: implementation of the Point class.
//
//////////////////////////////////////////////////////////////////////

public class Point
{

	public RGB color = new RGB();
	public double y;
	public double x;
	public double z;
	public double w;
	public double xNorm;
	public double yNorm;
	public double zNorm;

	public Point()
	{
		x = y = z = xNorm = yNorm = zNorm = 0;
		w = 1;
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

	public Point(double x, double y, double z, double xNorm, double yNorm, double zNorm)
	{
		this.x = x;
		this.y = y;
		this.z = z;
		this.xNorm = xNorm;
		this.yNorm = yNorm;
		this.zNorm = zNorm;
		w = 1;
		//color = RGB((double)(rand()%1000)/1000,(double)(rand()%1000)/1000,(double)(rand()%1000)/1000);
		//	cout<<"X: "<<x<<endl;
		//	cout<<"Y: "<<y<<endl;
	}

	public void Print()
	{
		string s = "";
		s += "x: "+ x +
			" y: " + y +
			" z: " + z + 
			" xNorm: " + xNorm + 
			" yNorm: " + yNorm + 
			" zNorm: " + zNorm;
		System.Console.WriteLine(s);
		color.Print();
	}
}
