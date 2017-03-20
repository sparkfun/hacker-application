package road;
import car.CarObj;

import agent.TimeServerQueue;
import data.*;
import java.util.Observable;
import java.util.Observer;
import util.Animator;
import model.Agent;

public class SourceObj implements Source{
	private TimeServerQueue _time;
	Parameters _parameters;
	private RoadSegmentObj _road;
	private GridConstructor _grid;
	
	public SourceObj(Parameters parameters, GridConstructor grid, RoadSegmentObj road)
	{
		//System.out.printf("\t\tInitializing Road Source\n");
		_parameters = parameters;
		_time = _parameters.timeServer();
		_road = road;
		_grid = grid;
	}
	public void run()
	{
		//if ((_time.currentTime() - _parameters.startTime()) < _parameters.runTime())
		CarObj newCar = new CarObj(_parameters, _road, _grid);
		_road.acceptCar(newCar);
		_grid.agents().add((model.Agent) newCar);
		_time.enqueue(_time.currentTime(), newCar);
		
		//Enqueue the source in the time server for the next wake
		_time.enqueue(_parameters.carEntryRate() + _time.currentTime(), this);

	}
	
	public void run(double time)
	{
		_time.enqueue(_parameters.carEntryRate() + _time.currentTime(), this);
	}
}
