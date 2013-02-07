// Group.cpp: implementation of the Group class.
//
//////////////////////////////////////////////////////////////////////

#include "Group.h"
using namespace std;

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

Group::Group()
{
	numPolys = 0;
}

Group::Group(int size)
{
	numPolys = 0;
	polys = new Tri[size+1];
}

Group::~Group()
{
	//delete polys;
}

void Group::add(Tri T)
{
	polys[numPolys++]= T;
}


void Group::SetColor(RGB c)
{
	for(int i=0;i<numPolys;i++)
	{
		polys[i].SetColor(c);
	}
}

void Group::Print()
{
	for(int i=0;i<numPolys;i++)
	{
		polys[i].Print();
	}
}