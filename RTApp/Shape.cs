using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTApp
{
	public class Shape
	{
		public RGB color = new RGB();
		public virtual double IntersectDistance(Point Ray)
		{
			return double.PositiveInfinity;
		}
	}
}
