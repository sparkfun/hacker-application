package com.coleingraham.utils;

import toxi.geom.Rect;

import com.coleingraham.curves.CurvePackingFile;
import com.coleingraham.curves.CurvePackingRegion;
import com.coleingraham.curves.CurveRegionGroup;
import com.coleingraham.curves.CurveShape;
import com.coleingraham.lines.Line;
import com.coleingraham.lines.LineFile;
import com.coleingraham.lines.PolyLine;
import com.coleingraham.lines.PolyLineFile;

public class FileSorter {

	/**
	 * Sorts a LineFile by a grid for faster drawing.
	 * @param input
	 * @return
	 */
	public static LineFile sort(LineFile input)
	{
		LineFile output = new LineFile();

		output.width = input.width;
		output.height = input.height;
		output.randomSeed = input.randomSeed;
		output.noiseSeed = input.noiseSeed;

		Rect testArea = new Rect(0,0,10,10);

		Line l;

		for(int y=0; y<input.height-10; y+=10)
		{
			for(int x=0; x<input.width-10; x+=10)
			{
				for(int i=0; i<input.lines.size(); i++)
				{
					testArea.x = x;
					testArea.y = y;
					l = input.lines.get(i);

					if( testArea.toPolygon2D().containsPoint(l.a) || testArea.toPolygon2D().containsPoint(l.b) )
					{
						output.lines.add(l);
						input.lines.remove(i);
						System.out.println(input.lines.size() + " remaining..." );
					}
				}
			}
		}

		return output;
	}

	public static PolyLineFile sort(PolyLineFile input)
	{
		PolyLineFile output = new PolyLineFile();

		output.width = input.width;
		output.height = input.height;
		output.randomSeed = input.randomSeed;
		output.noiseSeed = input.noiseSeed;

		Rect testArea = new Rect(0,0,10,10);

		PolyLine l;

		for(int y=0; y<input.height-10; y+=10)
		{
			for(int x=0; x<input.width-10; x+=10)
			{
				for(int i=0; i<input.lines.size(); i++)
				{
					testArea.x = x;
					testArea.y = y;
					l = input.lines.get(i);

					for(int j=0; j<l.verts.size(); j++)
					{

						if( testArea.toPolygon2D().containsPoint( l.verts.get(j) ) )
						{
							output.lines.add(l);
							input.lines.remove(i);
							System.out.println(input.lines.size() + " remaining..." );
							break;
						}
					}
					
				}
			}
		}

		return output;
	}

	public static CurvePackingFile sort(CurvePackingFile input)
	{
		CurvePackingFile output = new CurvePackingFile();

		output.width = input.width;
		output.height = input.height;
		output.randomSeed = input.randomSeed;
		output.noiseSeed = input.noiseSeed;

		Rect testArea = new Rect(0,0,10,10);

		CurveShape c;

		System.out.println(input.getType());

		switch(input.getType())
		{
		case 0:
			for(int r=0; r<input.regions.size(); r++)
			{
				output.add( new CurvePackingRegion(input.getRegion(r).bounds) );

				int width = input.width;
				int height = input.height;

				if(width == 0) { width = 1200; }
				if(height == 0) { height = 800; }

				for(int y=0; y<height-10; y+=10)
				{
					for(int x=0; x<width-10; x+=10)
					{
						for(int i=0; i<input.getRegion(r).curves.size(); i++)
						{
							testArea.x = x;
							testArea.y = y;
							c = input.getRegion(r).curves.get(i);

							if( testArea.toPolygon2D().containsPoint(c.origin) )
							{
								System.out.println("moved");
								output.getRegion(r).curves.add(c);
								input.getRegion(r).curves.remove(i);
								System.out.println(input.getRegion(r).curves.size() + " remaining..." );
							}
						}
					}
				}
			}
			break;
		case 1:
			for(int g=0; g<input.groups.size(); g++)
			{
				output.add( new CurveRegionGroup() );

				for(int r=0; r<input.getGroup(g).regions.size(); r++)
				{
					output.add( new CurvePackingRegion(input.getGroup(g).regions.get(r).bounds) );

					int width = input.width;
					int height = input.height;

					if(width == 0) { width = 1200; }
					if(height == 0) { height = 800; }

					for(int y=0; y<height-10; y+=10)
					{
						for(int x=0; x<width-10; x+=10)
						{
							for(int i=0; i<input.getRegion(r).curves.size(); i++)
							{
								testArea.x = x;
								testArea.y = y;
								c = input.getRegion(r).curves.get(i);

								if( testArea.toPolygon2D().containsPoint(c.origin) )
								{
									output.getRegion(r).curves.add(c);
									input.getRegion(r).curves.remove(i);
								}
							}
						}
					}
				}
			}
			break;
		}

		return output;
	}

}
