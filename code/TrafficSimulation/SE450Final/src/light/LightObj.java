package light;

import model.Agent;

public class LightObj implements Agent{
	private double _greenTime;
	private double _yellowTime;
	private boolean _isGreen;
	public LightObj(double greenTime, double yellowTime)
	{
		_greenTime = greenTime;
		_yellowTime = yellowTime;
	}
	
	public double greenTime()
	{
		return _greenTime;
	}
	
	public double yellowTime()
	{
		return _yellowTime;
	}
	
	//TODO need to enqueue the light in the time server, will sleep for green time or yellow time
	//depending on state.  Could also sleep for its companions combined green and yellow time.
	public void run(){}
	public void run(double time){}
	public boolean getState()
	{
		return _isGreen;
	}
}
