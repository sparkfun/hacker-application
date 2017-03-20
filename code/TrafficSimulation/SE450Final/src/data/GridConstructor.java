package data;

import java.util.ArrayList;
import java.util.List;
import java.util.AbstractList;
import java.util.Observer;
import java.util.Observable;
import agent.TimeServerQueue;
import agent.*;
import model.Agent;
import model.AnimatorBuilder;
import road.RoadWay;
import road.IntersectionObj;
import road.SourceObj;
import road.RoadSegmentObj;
import util.Animator;
import light.LightObj;

public class GridConstructor extends Observable {
	//private RoadSet[][] roads;
	public TimeServerQueue _timeServer;
	public Parameters _parameters;
	 private List<model.Agent> _agents;
	  private Animator _animator;
	  private boolean _disposed;
	  private double _time;
	  private AnimatorBuilder _builder;
	  private List<List<RoadSegmentObj>> _NSRoads;
	  private List<List<RoadSegmentObj>> _EWRoads;
	  //private RoadSegmentObj[][] _NSRoads;
	  //private RoadSegmentObj[][] _EWRoads;
	//public static double _startTime;
	
	public GridConstructor(Parameters parameters, AnimatorBuilder builder)
	{
		_EWRoads = new ArrayList<List<RoadSegmentObj>>();
		_NSRoads = new ArrayList<List<RoadSegmentObj>>();
		_builder = builder;
		_parameters = parameters;
		_timeServer = parameters.timeServer();
	      _agents = new ArrayList<model.Agent>();
			setup(_builder);		
		_timeServer.run(_parameters.runTime());
		_parameters.setStartTime(_timeServer.currentTime());
		setup(builder);
		_animator = _builder.getAnimator();
	      super.addObserver(_animator);


	}

	
	 private void setup(AnimatorBuilder builder) {
		 int rows = _parameters.gridSize()[1];
		 int columns = _parameters.gridSize()[0];
		 
		 List<RoadSegmentObj> roads = new ArrayList<RoadSegmentObj>();
		 IntersectionObj[][] intersections = new IntersectionObj[rows][columns];

	
		    // Add Intersections
		    for (int i=0; i<rows; i++) {
		      for (int j=0; j<columns; j++) {
		    	  intersections[i][j] = new IntersectionObj(_parameters.intersectionLength(), _parameters, this);
		      }
		    }
		    
		    // Add Horizontal Roads
		    boolean eastToWest = false;
		    for (int i=0; i<rows; i++) {
		    	ArrayList<RoadSegmentObj> segments = new ArrayList<RoadSegmentObj>();
		    	_EWRoads.add(segments);
		      for (int j=0; j<=columns; j++) {
		        RoadSegmentObj l = new RoadSegmentObj(_parameters, eastToWest, this, false);
		        segments.add(l);
		        if (j > 0)
		        	intersections[i][j-1].setNextRoadWay(l); 
		        if (j < columns)
		        	l.setNextRoadWay(intersections[i][j]);
		        if (j > 0)
		        	segments.get(j-1).setNextRoadWay((RoadWay) l);
		        _timeServer.enqueue(_timeServer.currentTime() ,l);
		        SourceObj source;
		        if (j == 0)
		        {
		        	source = new SourceObj(_parameters, this, l);
		        	_timeServer.enqueue(_timeServer.currentTime() ,source);
		        }
		        if (eastToWest)
		        	builder.addHorizontalRoad(l, i,columns  - j, eastToWest);
		        else
		        	builder.addHorizontalRoad(l, i, j, eastToWest);
		        roads.add(l);
		      }
		      if (_parameters.patternType() == Parameters.pattern.alternating)
		    	  eastToWest = !eastToWest;
		    }
		    
		    // Add Vertical Roads
		    boolean southToNorth = false;
		    for (int j=0; j<columns; j++) {
		    	ArrayList<RoadSegmentObj> segments = new ArrayList<RoadSegmentObj>();
		    	_NSRoads.add(segments);
		      for (int i=0; i<=rows; i++) {
		        RoadSegmentObj l = new RoadSegmentObj(_parameters, southToNorth, this, true);
		        segments.add(l);
		        if (i > 0)
		        	intersections[i-1][j].setNextRoadWay(l);
		        if (i < rows)
		        	l.setNextRoadWay(intersections[i][j]);
		        if (i > 0)
		        	segments.get(i-1).setNextRoadWay((RoadWay) l);
		        _timeServer.enqueue(_timeServer.currentTime(), l);
		        SourceObj source;
		        if (i == 0)
		        {
		        	source = new SourceObj(_parameters, this, l);
		        	_timeServer.enqueue(_timeServer.currentTime() ,source);
		        }
		        if (southToNorth)
		        	builder.addVerticalRoad(l, rows - i, j, southToNorth);
		        else
		        	builder.addVerticalRoad(l, i, j, southToNorth);
		        roads.add(l);
		      }
		      if (_parameters.patternType() == Parameters.pattern.alternating)
		    	  southToNorth = !southToNorth;
		    }
	 }
	
	  public void dispose() {
		    _animator.dispose();
		    _disposed = true;
		  }
	  
	  public void run(int duration) {
		    if (_disposed)
		      throw new IllegalStateException();
		    for (int i=0; i<duration; i++) {
		      _time++;
		      model.Agent[] agents_copy = _agents.toArray(new model.Agent[0]);
		      for (model.Agent a : agents_copy) {
		        a.run(_time);
		      }
		      super.setChanged();
		      super.notifyObservers();
		    }
	  }
	  
	  public List<model.Agent> agents()
	  {
		  return _agents;
	  }
}
