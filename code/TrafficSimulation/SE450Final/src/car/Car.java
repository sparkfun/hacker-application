package car;

import agent.Agent;

public interface Car extends Agent{
	//Return the length of the car in meters.
	public double length();
	
	//Return the maximum velocity of the car in meters/second.
	public double maxVelocity();
	
	//Return the braking distance of the car.
	public double brakeDistance();
	
	//Return the stop distance for the car.
	public double stopDistance();
	
}
