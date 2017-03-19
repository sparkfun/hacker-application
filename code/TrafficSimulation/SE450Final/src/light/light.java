package light;

import agent.Agent;

public interface light extends Agent {
	//The length of time that the light is green in seconds
	public double greenTime();
	
	//The length of time that the light is yellow in seconds
	public double yellowTime();
	
}
