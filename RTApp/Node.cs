using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RTApp
{
	class Node
	{
		public BBox bbox = new BBox();
		public List<Node> children = new List<Node>();

		bool isLeaf(Node n)
		{
			return n.children.Count == 0;
		}

		void getBoundingBoxes(ref List<BBox> boxes)
		{

			if (isLeaf(this))
			{
				boxes.Add(bbox);
			}
			else
			{
				for (int i = 0; i < children.Count; i++)
				{
					children[i].getBoundingBoxes(ref boxes);
				}
			}
		}

		public void getBoxIntersections(Point Ray, Point O, List<BBox> list)
		{
			if (isLeaf(this))
			{
				if (bbox.intersectsBox(O, Ray))
					list.Add(bbox);
			}
			else
			{
				if (bbox.intersectsBox(O, Ray))
				{
					foreach (var item in children)
					{
						item.getBoxIntersections(Ray, O, list);
					}
				}
			}
		}
	}
}
