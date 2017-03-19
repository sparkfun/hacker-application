package data;

import agent.TimeServerQueue;
import model.*;
import model.swing.*;
import model.text.*;

 public final class Parameters implements ParameterSet {

	private double _timeStep;
	private double _runTime;
	private int[] _gridSize = new int[2];
	private int[][] _gridArray = new int[_gridSize[0]][_gridSize[1]];
	public enum pattern{simple, alternating}
	public pattern _patternType;
	private double[] _carEntryRate = new double[2];
	private double[] _roadSegmentLength = new double[2];
	private double[] _intersectionLength = new double[2];
	private double[] _carLength = new double[2];
	private double[] _carMaxVelocity = new double[2];
	private double[] _carStopDistance = new double[2];
	private double[] _carBrakeDistance = new double[2];
	private double[] _lightGreenTime = new double[2];
	private double[] _lightYellowTime = new double[2];
	private TimeServerQueue _timeServer;
	private double _startTime;
	private AnimatorBuilder _animator;


	public Parameters()
	{
		//TODO make these values random (perhaps add private min and max
		//variables for each and change those in each of the set methods.)
		_timeStep = 0.1;
		_runTime = 1000.0;
		_gridSize[0] = 2;
		_gridSize[1] = 3;
		_patternType = pattern.simple;
		_carEntryRate[0] = 2;
		_carEntryRate[1] = 25;
		_roadSegmentLength[0] = 200;
		_roadSegmentLength[1] = 500;
		_intersectionLength[0] = 10;
		_intersectionLength[1] = 15;
		_carLength[0] = 5;
		_carLength[1] = 10;
		_carMaxVelocity[0] = 10;
		_carMaxVelocity[1] = 30;
		_carStopDistance[0] = 0.5;
		_carStopDistance[1] = 5;
		_carBrakeDistance[0] = 9.0;
		_carBrakeDistance[1] = 10.0;
		_lightGreenTime[0] = 30.0;
		_lightGreenTime[1] = 180;
		_lightYellowTime[0] = 4.0;
		_lightYellowTime[1] = 5.0;
		_timeServer = new TimeServerQueue();
		
	}
	
	public AnimatorBuilder animator()
	{
		return _animator;
	}
	
	public void setAnimator(AnimatorBuilder animator)
	{
		_animator = animator;
	}

	public double startTime()
	{
		return _startTime;
	}
	
	public void setStartTime(double time)
	{
		_startTime = time;
	}
	
	public TimeServerQueue timeServer()
	{
		return _timeServer;
	}
	
	public void setTimeStep(double newTimeStep)
	{
		_timeStep = newTimeStep;
	}
	
	public double timeStep()
	{
		return _timeStep;
	}
	
	public void setRunTime(double newRunTime)
	{
		_runTime = newRunTime;
	}
	
	public double runTime()
	{
		return _runTime;
	}
	
	public int[] gridSizeValues()
	{
		return _gridSize;
	}
	
	public void setGridSize(int newNS, int newEW)
	{
		_gridSize[0] = newNS;
		_gridSize[1] = newEW;
	}
	
	public int[] gridSize()
	{
		return _gridSize;
	}
	
	public void setPatternType(pattern newPattern)
	{
		_patternType = newPattern;
	}
	
	public pattern patternType()
	{
		return _patternType;
	}
	
	public void setCarEntryRate(double minRate, double maxRate)
	{
		_carEntryRate[0] = minRate;
		_carEntryRate[1] = maxRate;
	}
	
	public double[] carEntryRateValues()
	{
		return _carEntryRate;
	}
	
	public double carEntryRate()
	{
		return rangeRandom(_carEntryRate[0], _carEntryRate[1]);
	}
	
	public void setRoadSegmentLength(double minLength, double maxLength)
	{
		_roadSegmentLength[0] = minLength;
		_roadSegmentLength[1] = maxLength;
	}
	
	public double[] roadSegmentLengthValues()
	{
		return _roadSegmentLength;
	}
	
	public double roadSegmentLength()
	{
		return rangeRandom(_roadSegmentLength[0], _roadSegmentLength[1]);
	}
	
	public void setIntersectionLength(double minLength, double maxLength)
	{
		_intersectionLength[0] = minLength;
		_intersectionLength[1] = maxLength;
	}
	
	public double[] intersectionLengthValues()
	{
		return _intersectionLength;
	}
	
	public double intersectionLength()
	{
		return rangeRandom(_intersectionLength[0], _intersectionLength[1]);
	}
	
	public void setCarLength(double minLength, double maxLength)
	{
		_carLength[0] = minLength;
		_carLength[1] = maxLength;
	}
	
	public double[] carLengthValues()
	{
		return _carLength;
	}
	public double carLength()
	{
		return rangeRandom(_carLength[0], _carLength[1]);
	}
	
	public void setMaxCarVelocity(double minVelocity, double maxVelocity)
	{
		_carMaxVelocity[0] = minVelocity;
		_carMaxVelocity[1] = maxVelocity;
	}
	
	public double[] carMaxVelocityValues()
	{
		return _carMaxVelocity;
	}
	
	public double carMaxVelocity()
	{
		return rangeRandom(_carMaxVelocity[0], _carMaxVelocity[1]);
	}
	
	public void setCarStopDistance(double minDistance, double maxDistance)
	{
		_carStopDistance[0] = minDistance;
		_carStopDistance[1] = maxDistance;
	}
	
	public double[] carStopDistanceValues()
	{
		return _carStopDistance;
	}
	
	public double carStopDistance()
	{
		return rangeRandom(_carStopDistance[0], _carStopDistance[1]);
	}
	
	public void setCarBrakeDistance(double minDistance, double maxDistance)
	{
		_carBrakeDistance[0] = minDistance;
		_carBrakeDistance[1] = maxDistance;
	}
	
	public double[] carBrakeDistanceValues()
	{
		return _carBrakeDistance;
	}
	
	public double carBrakeDistance()
	{
		return rangeRandom(_carBrakeDistance[0], _carBrakeDistance[1]);
	}
	
	public void setYellowTime(double minTime, double maxTime)
	{
		_lightYellowTime[0] = minTime;
		_lightYellowTime[1] = maxTime;
	}
	
	public double[] yellowTimeValues()
	{
		return _lightYellowTime;
	}
	
	public double lightYellowTime()
	{
		return rangeRandom(_lightYellowTime[0], _lightYellowTime[1]);
	}
	
	public void setGreenTime(double minTime, double maxTime)
	{
		_lightGreenTime[0] = minTime;
		_lightGreenTime[1] = maxTime;
	}
	
	public double[] greenTimeValues()
	{
		return _lightGreenTime;
	}
	public double lightGreenTime()
	{
		return rangeRandom(_lightGreenTime[0], _lightGreenTime[1]);
	}
	
	private double rangeRandom(double minValue, double maxValue)
	{
		double value = minValue + (Math.random() * (maxValue - minValue));
		return value;
	}
}

