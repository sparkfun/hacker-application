package road;

import java.util.AbstractList;
import light.LightObj;
import java.util.ArrayList;
import data.Parameters;
import data.GridConstructor;
import agent.TimeServerQueue;
import light.LightController;

import car.CarObj;

public final class IntersectionObj implements Intersection{
	private double _length;
	private double _NSStartPosition;
	private double _EWStartPosition;
	private AbstractList<CarObj> _NSCars = new ArrayList<CarObj>();
	private AbstractList<CarObj> _EWCars = new ArrayList<CarObj>();
	private AbstractList<CarObj> _cars = new ArrayList<CarObj>();
	private RoadWay _nextRoadWay;
	private Parameters _parameters;
	private GridConstructor _grid;
	private TimeServerQueue _timeServer;
	private LightController _lightController;
	private LightObj _NSLight;
	private LightObj _EWLight;
	
	
	public IntersectionObj(double length, Parameters parameters, GridConstructor grid)
	{
		_parameters = parameters;
		_length = length;
		_grid = grid;
		_timeServer = _grid._timeServer;
		_lightController = new LightController(_parameters);
		_timeServer.enqueue(_timeServer.currentTime() ,_lightController);
	}
	
	public AbstractList<CarObj> getCars()
	{
		_cars.addAll(_NSCars);
		_cars.addAll(_EWCars);
		return _cars;
	}
	
	public double length()
	{
		return _length;
	}
	
	public void setNSStartPosition(double position)
	{
		_NSStartPosition = position;
	}
	
	public LightController.state state()
	{
		return _lightController.currentState();
	}
	
	public double NSStartPosition()
	{
		return _NSStartPosition;
	}
	
	public void setEWStartPosition(double position)
	{
		_EWStartPosition = position;
	}
	
	public double EWStartPosition()
	{
		return _EWStartPosition;
	}
	
	public void acceptCar(CarObj car)
	{
		if (((RoadSegmentObj) car.currentRoadWay()).isNorthSouth())
			_NSCars.add(car);
		else
			_EWCars.add(car);
		
	}
	
	public void passCar(CarObj car)
	{
		if (_nextRoadWay == null)
			throw new NullPointerException();
		else
		{
			_nextRoadWay.acceptCar(car);
			_cars.remove(car);
			//TODO Import Grid and Time Server.  Also do this for IntersectionObj
			//_grid.agents().add((model.Agent) car);
			_timeServer.enqueue(_timeServer.currentTime(), car);
		}
	}
	
	public void setNextRoadWay(RoadWay road)
	{
		_nextRoadWay = road;
	}
	
	public RoadWay nextRoadWay()
	{
		return _nextRoadWay;
	}
	
	public boolean isObstacle(CarObj car)
	{
		if (((RoadSegmentObj) car.currentRoadWay()).isNorthSouth())
			if (!_EWCars.isEmpty() || _lightController.currentState() == LightController.state.GreenEW || _lightController.currentState() == LightController.state.YellowEW)
				return true;
			else
				return false;
		else
			if (!_NSCars.isEmpty() || _lightController.currentState() == LightController.state.GreenNS || _lightController.currentState() == LightController.state.YellowNS)
				return true;
			else
				return false;
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
