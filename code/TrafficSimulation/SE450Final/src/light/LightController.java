package light;

import car.CarObj;
import model.Agent;
import data.Parameters;
import agent.TimeServerQueue;


public class LightController implements Agent{

	public enum state{GreenNS, YellowNS, GreenEW, YellowEW};
	private state _currentState;
	private state[] _stateCycle = {state.GreenNS, state.YellowNS, state.GreenEW, state.YellowEW};
	private int _stateIndex;
	private TimeServerQueue _time;
	private Parameters _parameters;
	private Double _greenDurationNS;
	private Double _yellowDurationNS;
	private Double _greenDurationEW;
	private Double _yellowDurationEW;
	private Double[] _timeCycle = {_greenDurationNS, _yellowDurationNS, _greenDurationEW, _yellowDurationEW};
	
	public LightController(Parameters parameters)
	{
		_parameters = parameters;
		_time = _parameters.timeServer();
		_greenDurationNS = _parameters.lightGreenTime();
		_yellowDurationNS = _parameters.lightYellowTime();
		_greenDurationEW = _parameters.lightGreenTime();
		_yellowDurationEW = _parameters.lightYellowTime();
	}
	
	public state currentState()
	{
		return _currentState;
	}
	
	public void cycleStates()
	{
		_currentState =  _stateCycle[++_stateIndex % 4];
	}
	
	public void run()
	{
		//cycleStates();
		//_time.enqueue(_time.currentTime() + _timeCycle[_stateIndex], this);
	}
	
	public void run(double time)
	{
		cycleStates();
		_time.enqueue(_time.currentTime() + _timeCycle[_stateIndex], this);
	}
	
}
