package road;

import java.util.AbstractList;

import agent.TimeServerQueue;
import car.CarObj;
import java.util.AbstractList;
import java.util.ArrayList;
import data.Parameters;
import data.GridConstructor;
import light.LightController;
import model.Agent;

public class RoadSegmentObj implements RoadSegment{
	private double _length;
	private AbstractList<CarObj> _cars = new ArrayList<CarObj>();
	private RoadWay _nextRoadWay;
	private Parameters _parameters;
	private Boolean _isReversed;
	private GridConstructor _grid;
	private TimeServerQueue _timeServer;
	private Boolean _isNorthSouth;
	
	public AbstractList<CarObj> getCars()
	{
		return _cars;
	}
	
	/*
	 * What does it really need to know?  Can get length from parameters, RoadSet is being phased out
	 * isReversed is something that should be passed from the GridConstructor.  Does it need to know its coords?
	 */
	public RoadSegmentObj(Parameters parameters, Boolean isReversed, GridConstructor grid, Boolean isNorthSouth)
	{
		_parameters = parameters;
		_length = _parameters.roadSegmentLength();
		_isReversed = isReversed;
		_grid = grid;
		_timeServer = _grid._timeServer;
		_isNorthSouth = isNorthSouth;
		LightController _lightController = new LightController(_parameters);
		_timeServer.enqueue(_timeServer.currentTime() ,_lightController);
	}
	
	public double length()
	{
		return _length;
	}
	
	public Boolean isNorthSouth()
	{
		return _isNorthSouth;
	}
	
	public void acceptCar(CarObj car)
	{
		_cars.add(car);
		car.resetRoadWayPosition();
		//_timeServer.enqueue(_timeServer.currentTime() + _parameters.timeStep(), car);
	}
	
	public void passCar(CarObj car)
	{
		if (_nextRoadWay == null)
		{
			_cars.remove(car);
			_grid.agents().remove(car);
		}	
		else
		{
			_nextRoadWay.acceptCar(car);
			car.setCurrentRoadWay(_nextRoadWay);
			_cars.remove(car);
			//TODO Import Grid and Time Server.  Also do this for IntersectionObj
			_grid.agents().add((model.Agent) car);
			_timeServer.enqueue(_timeServer.currentTime(), car);
		}
	}
	
	public void setNextRoadWay(RoadWay road)
	{
		_nextRoadWay = road;
	}
	
	public double nextObstacle(CarObj car)
	{
		double distanceToNextCar = _parameters.carBrakeDistance() + 1;
		double distanceToNextIntersection;
		CarObj nextCar = null;
		if (_cars.indexOf(car) +1 < _cars.size())
		{
			nextCar =_cars.get(_cars.indexOf(car) + 1);
			distanceToNextCar = nextCar.rearPosition() - car.currentPosition();
		}
		distanceToNextIntersection = _length - car.currentPosition();
		//need to determine next intersection
		IntersectionObj nextIntersection = null;
		/*
		if (!nextIntersection.isObstacle(this) && nextIntersection != null )
			return Math.min(distanceToNextCar, distanceToNextIntersection);
		else*/
			return distanceToNextCar;
	}
	
	public RoadWay nextRoadWay()
	{
		return _nextRoadWay;
	}
	
	public void run()
	{
		_timeServer.enqueue(_parameters.timeStep() + _timeServer.currentTime(), this);

	}
	
	public void run(double time)
	{
		_timeServer.enqueue(_parameters.carEntryRate() + _timeServer.currentTime(), this);
	}
}
