package road;

import java.util.AbstractList;
import agent.Agent;

public interface Road extends Agent{
	//Return an array of the road segments that comprise the road.
	AbstractList<RoadWay> segments();
	
	//Return the number of road segments
	int size();
	
	/*
	 * This might need to have methods inherited from AbstractList, such as add.
	 * I might want to add more later, such as an array of just intersections, 
	 * just road segments, the total length of the road, etc.
	 */
}
