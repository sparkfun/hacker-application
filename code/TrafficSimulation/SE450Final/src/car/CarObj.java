package car;

import java.util.AbstractList;
import data.*;
import agent.TimeServerQueue;
import road.IntersectionObj;
import road.RoadSegmentObj;
import road.RoadWay;
import model.Agent;

public class CarObj implements Agent{
	
	private double _length;
	private double _maxVelocity;
	private double _brakeDistance;
	private double _stopDistance;
	static TimeServerQueue _timeServer;
	private double _roadPosition;
	private double _rawPosition = 0;
	private double _roadWayPosition = 0;
	private RoadWay _road;
	private Parameters _parameters;
	private GridConstructor _grid;
	private RoadWay _currentRoadWay;
	 private java.awt.Color _color = new java.awt.Color((int)Math.ceil(Math.random()*255),(int)Math.ceil(Math.random()*255),(int)Math.ceil(Math.random()*255));
	
	
	public CarObj(Parameters parameters, RoadWay road, GridConstructor grid)
	{
		_grid = grid;
		_parameters = parameters;
		_timeServer = _parameters.timeServer();
		_length = _parameters.carLength();
		_maxVelocity = _parameters.carMaxVelocity();
		_brakeDistance = _parameters.carBrakeDistance();
		_stopDistance = _parameters.carStopDistance();
		_road = road;
		_roadPosition = 0;
		_roadWayPosition = 0;
		_currentRoadWay = road;
	}
	
	  public java.awt.Color getColor() {
		    return _color;
		  }
	
	public double length()
	{
		return _length;
	}
	
	public double maxVelocity()
	{
		return _maxVelocity;
	}
	
	public double brakeDistance()
	{
		return _brakeDistance;
	}
	
	public double stopDistance()
	{
		return _stopDistance;
	}
	
	public double currentPosition()
	{
		return _roadPosition;
	}
	
	public double rearPosition()
	{
		return _roadPosition - _length;
	}
	
	public void setCurrentRoadWay(RoadWay roadWay)
	{
		_currentRoadWay = roadWay;
	}
	
	public RoadWay currentRoadWay()
	{
		return _currentRoadWay;
	}
	
	public void resetRoadWayPosition()
	{
		_roadWayPosition = 0;
		//_rawPosition = 0;
	}
	
	public void run()
	{

	}
	
	public double nextObstacle()
	{
		double distanceToNextIntersection = _currentRoadWay.length() - _roadWayPosition;
		double distanceToNextCar = _brakeDistance + 1;
		CarObj nextCar = null;
		AbstractList<CarObj> sharedCars = _currentRoadWay.getCars();
		if (_currentRoadWay.getCars().indexOf(this) +1 < sharedCars.size())
		{
			nextCar =sharedCars.get(sharedCars.indexOf(this) + 1);
			distanceToNextCar = nextCar.rearPosition() - _roadWayPosition;
		}
		else if (_currentRoadWay.nextRoadWay() != null && !_currentRoadWay.nextRoadWay().getCars().isEmpty())
		{
			nextCar = _currentRoadWay.nextRoadWay().getCars().get(0);
			distanceToNextCar = nextCar.currentPosition() + distanceToNextIntersection;
		}
		if (_currentRoadWay.nextRoadWay() instanceof IntersectionObj && ((IntersectionObj)_currentRoadWay.nextRoadWay()).isObstacle(this))
			return Math.min(distanceToNextCar, distanceToNextIntersection);
		else
			return distanceToNextCar;
	}
	
	public void run(double time)
	{
		//Determine what the cars position will be when next waking.
		if (_timeServer.currentTime() - _parameters.startTime() < _parameters.runTime() &&  _roadWayPosition < _currentRoadWay.length())
		{
		double velocity = (_maxVelocity / (_brakeDistance - _stopDistance))
                 * (nextObstacle() - _stopDistance);
		velocity = Math.max(0.0, velocity);
		velocity = Math.min(_maxVelocity, velocity);
		double displacement = velocity * _parameters.timeStep();
		_rawPosition += displacement;
		_roadPosition = (_rawPosition / _currentRoadWay.length()) * (model.MP.roadLength - model.MP.carLength);
		_roadWayPosition += displacement;
		_timeServer.enqueue(_parameters.timeStep() + _timeServer.currentTime(), this);
		}
		else
		{
			_currentRoadWay.passCar(this);
			//_road.removeCar(this);
			_grid.agents().remove((model.Agent) this);
		}
	}
}

