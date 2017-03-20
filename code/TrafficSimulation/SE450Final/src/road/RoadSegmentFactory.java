package road;

import data.Parameters;
import data.GridConstructor;

public class RoadSegmentFactory {
	private RoadSegmentFactory(){}
	static public RoadSegmentObj newRoadSegment(Parameters parameters, Boolean isReversed, GridConstructor grid, Boolean isNorthSouth)
	{
		return new RoadSegmentObj(parameters, isReversed, grid, isNorthSouth);
	}
}
