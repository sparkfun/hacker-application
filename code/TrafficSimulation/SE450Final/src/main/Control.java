package main;

import model.*;
import model.swing.*;
import model.text.*;
import data.Parameters;
import ui.UI;
import ui.UIError;
import ui.UIForm;
import ui.UIFormBuilder;
import ui.UIFormTest;
import ui.UIMenu;
import ui.UIMenuAction;
import ui.UIMenuBuilder;
import data.GridConstructor;
import java.lang.IllegalArgumentException;

class Control {
	  private static final int EXITED = 0;
	  private static final int EXIT = 1;
	  private static final int START = 2;
	  private static final int PARAMS = 3;
	  private static final int PATTERN = 4;
	  private static final int NUMSTATES = 10;
	  private UIMenu[] _menus;
	  private UIForm _twoDoubles;
	  private UIForm _oneDouble;
	  private UIForm _twoInts;
	  private UIForm _pattern;
	  private UIFormTest _integerTest;
	  private UIFormTest _patternTest;
	  private int _state;

 Parameters _parameters = new Parameters();
	    
	  private UI _ui;
	  private Main.UIType _type;
	  
	  Control(UI ui, Main.UIType type) {
	    _ui = ui;
	    _type = type;

	    _menus = new UIMenu[NUMSTATES];
	    _state = START;
	    addSTART(START);
	    addEXIT(EXIT);
	    addPARAMS(PARAMS);
	    addPATTERN(PATTERN);
	    
	    UIFormTest _doubleTest = new UIFormTest() {
	        public boolean run(String input) {
	          try {
	            Double.parseDouble(input);
	            return true;
	          } catch (NumberFormatException e) {
	            return false;
	          }
	        }
	      };
	    _integerTest = new UIFormTest() {
	        public boolean run(String input) {
	          try {
	            Integer.parseInt(input);
	            return true;
	          } catch (NumberFormatException e) {
	            return false;
	          }
	        }
	      };
		    _patternTest = new UIFormTest() {
		        public boolean run(String input) {
		          try {	  
		            return (input == "simple" || input == "alternating");
		          } catch (IllegalArgumentException e) {
		            return false;
		          }
		        }
		      };
	    
	    
	    UIFormBuilder f = new UIFormBuilder();
	    f.add("Minimum Value", _doubleTest);
	    f.add("Maximum Value", _doubleTest);
	    _twoDoubles = f.toUIForm("Modify Parameters");	
	    
	    UIFormBuilder h = new UIFormBuilder();
	    h.add("New Row Value", _integerTest);
	    h.add("New Column Value", _integerTest);
	    _twoInts = h.toUIForm("Modify Parameters");
	    
	    UIFormBuilder g = new UIFormBuilder();
	    g.add("New Value", _doubleTest);
	    _oneDouble = g.toUIForm("Modify Parameters");	
	    
	    UIFormBuilder i = new UIFormBuilder();
	    i.add("New Pattern", _patternTest);
	    _pattern = i.toUIForm("Modify Parameters");
	    
	  }

	  
	  void run() {
	    try {
	      while (_state != EXITED) {
	        _ui.processMenu(_menus[_state]);
	      }
	    } catch (UIError e) {
	      _ui.displayError("UI closed");
	    }
	  }
	  
	  private void addSTART(int stateNum) {
	    UIMenuBuilder m = new UIMenuBuilder();
	    
	    m.add("Default",
	      new UIMenuAction() {
	        public void run() {
	          _ui.displayError("Please select a valid option.");
	        }
	      });
	    m.add("Run Simulation",
	      new UIMenuAction() 
	    	{
	        public void run() 
	        {
	          AnimatorBuilder m;
	          if (_type == Main.UIType.swing)
	          {
	        	 m = new SwingAnimatorBuilder();
	          }
	          else
	          {
	        	  m = new TextAnimatorBuilder();
	          }
	          _parameters.setAnimator(m);
	          GridConstructor newSimulation = new GridConstructor(_parameters, m);

        	  newSimulation.run((int) Math.ceil(_parameters.runTime()));
        	  newSimulation.dispose();

	         }
	        }
	      );
	    m.add("Edit Simulation Parameters",
	    	new UIMenuAction()
	    {
	    	
	    	public void run()
	    	{
	    		_state = PARAMS;
	    	}
	    }
	    );
	    m.add("Exit",
	    	      new UIMenuAction() {
	    	        public void run() {
	    	          _state = EXIT;
	    	        }
	    	      });
	    
		  _menus[stateNum] = m.toUIMenu("Traffic Simulation");
	  }
	   
	  private void addPATTERN(int stateNum)
	  {
		  UIMenuBuilder m = new UIMenuBuilder();
		    m.add("Default",
			  	      new UIMenuAction() {
			  	        public void run() {
			  	          _ui.displayError("Please select a valid option.");
			  	        }
			  	      });
		    m.add("Simple",
			  	      new UIMenuAction() {
			  	        public void run() {
			  	        	_parameters.setPatternType(Parameters.pattern.simple);
			  	        	_state = 3;
			  	        }
			  	      });
		    m.add("Alternating",
			  	      new UIMenuAction() {
			  	        public void run() {
			  	        	_parameters.setPatternType(Parameters.pattern.alternating);
			  	        	_state = 3;
			  	        }
			  	      });
		    m.add("Previous Menu",
			  	      new UIMenuAction() {
			  	        public void run() {
			  	        	_state = 3;
			  	        }
			  	      });
		    _menus[stateNum] = m.toUIMenu("Change Traffic Pattern");
	  }

	  private void addPARAMS(int stateNum)
	  {
		  UIMenuBuilder m = new UIMenuBuilder();
		    m.add("Default",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	          _ui.displayError("Please select a valid option.");
		  	        }
		  	      });
		    m.add("Show Current Values",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	          //TODO Display all parameters
		  	        	_ui.displayMessage(
		  	        			"Time Step (seconds) \t[" + Double.toString(_parameters.timeStep()) + "]\n"
		  	        			+ "Run Time (seconds) \t[" + Double.toString(_parameters.runTime()) + "]\n"
		  	        			+ "Grid Size (number of roads) \t [row = " + Integer.toString(_parameters.gridSize()[0]) + ", column = " + Integer.toString(_parameters.gridSize()[1]) + "]\n"
		  	        			+ "Traffic Pattern \t[" + _parameters.patternType() + "]\n"
		  	        			+ "Car Entry Rate (seconds/car) \t[min = " + Double.toString(_parameters.carEntryRateValues()[0]) + ", max = " + Double.toString(_parameters.carEntryRateValues()[1]) + "]\n"
		  	        			+ "Road Segment Length (meters) \t[min = " + Double.toString(_parameters.roadSegmentLengthValues()[0]) + ", max = " + Double.toString(_parameters.roadSegmentLengthValues()[1]) + "]\n"
		  	        			+ "Intersection Length (meters) \t[min = " + Double.toString(_parameters.intersectionLengthValues()[0]) + ", max = " + Double.toString(_parameters.intersectionLengthValues()[1]) + "]\n"
		  	        			+ "Car Length (meters) \t[min = " + Double.toString(_parameters.carLengthValues()[0]) + ",max = " + Double.toString(_parameters.carLengthValues()[1]) + "]\n"
		  	        			+ "Car Maximum Velocity (meters/second) \t[min = " + Double.toString(_parameters.carMaxVelocityValues()[0]) + ", max = " + Double.toString(_parameters.carMaxVelocityValues()[1]) + "]\n"
		  	        			+ "Car Stop Distance (meters) \t[min = " + Double.toString(_parameters.carStopDistanceValues()[0]) + ", max = " + Double.toString(_parameters.carStopDistanceValues()[1]) + "]\n"
		  	        			+ "Car Brake Distance (meters) \t[min = " + Double.toString(_parameters.carBrakeDistanceValues()[0]) + ", max = " + Double.toString(_parameters.carBrakeDistanceValues()[1]) + "]\n"
		  	        			+ "Traffic Light Green Time (seconds) \t[min = " + Double.toString(_parameters.greenTimeValues()[0]) + ", max = " + Double.toString(_parameters.greenTimeValues()[1]) + "]\n"
		  	        			+ "Traffic Light Yellow Time (seconds) \t[min = " + Double.toString(_parameters.yellowTimeValues()[0]) + ", max = " + Double.toString(_parameters.yellowTimeValues()[1]) + "]\n"
  		  	        		);
		  	        }
		  	      });
		    m.add("Simulation Time Step",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	          //TODO Set Time Step
		  	               String[] result1 = _ui.processForm(_oneDouble);
		  	               _parameters.setTimeStep(Double.parseDouble(result1[0]));
		  	        }
		  	      });
		    m.add("Simulation Run Time",
			  	      new UIMenuAction() {
			  	        public void run() {
			  	          //TODO Set Time Step
			  	               String[] result1 = _ui.processForm(_oneDouble);
			  	               _parameters.setRunTime(Double.parseDouble(result1[0]));
			  	        }
			  	      });
		    m.add("Grid Size",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	          //TODO Set Grid Size
		  	               String[] result1 = _ui.processForm(_twoInts);
		  	               _parameters.setGridSize(Integer.parseInt(result1[0]), Integer.parseInt(result1[1]));
		  	        }
		  	      });
		    m.add("Traffic Pattern",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	          //TODO Set Traffic Pattern
		  	        	/*
		  	               String[] result1 = _ui.processForm(_pattern);
		  	               if (result1[0] == "simple")
		  	            	   _parameters.setPatternType(Parameters.pattern.simple);
		  	               else
		  	            	   _parameters.setPatternType(Parameters.pattern.alternating);
		  	            	   */
		  	        	_state = PATTERN;
		  	        }
		  	      });
		    m.add("Car Entry Rate",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	               String[] result1 = _ui.processForm(_twoDoubles);
		  	               _parameters.setCarEntryRate(Double.parseDouble(result1[0]), Double.parseDouble(result1[1]));
		  	        }
		  	      });
		    m.add("Road Segment Length",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	          //TODO set road segment length
		  	               String[] result1 = _ui.processForm(_twoDoubles);
		  	               _parameters.setRoadSegmentLength(Double.parseDouble(result1[0]), Double.parseDouble(result1[1]));
		  	        }
		  	      });
		    m.add("Intersection Length",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	          //TODO set intersection length
		  	               String[] result1 = _ui.processForm(_twoDoubles);
		  	               _parameters.setIntersectionLength(Double.parseDouble(result1[0]), Double.parseDouble(result1[1]));
		  	        }
		  	      });
		    m.add("Car Length",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	          //TODO Set car length
		  	               String[] result1 = _ui.processForm(_twoDoubles);
		  	               _parameters.setCarLength(Double.parseDouble(result1[0]), Double.parseDouble(result1[1]));
		  	        }
		  	      });
		    m.add("Car Maximum Velocity",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	          //TODO set car max velocity
		  	               String[] result1 = _ui.processForm(_twoDoubles);
		  	               _parameters.setMaxCarVelocity(Double.parseDouble(result1[0]), Double.parseDouble(result1[1]));
		  	        }
		  	      });
		    m.add("Car Stop Distance",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	          //TODO set car stop distance
		  	               String[] result1 = _ui.processForm(_twoDoubles);
		  	               _parameters.setCarStopDistance(Double.parseDouble(result1[0]), Double.parseDouble(result1[1]));
		  	        }
		  	      });
		    m.add("Car Brake Distance",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	          //TODO set car brake distance
		  	               String[] result1 = _ui.processForm(_twoDoubles);
		  	               _parameters.setCarBrakeDistance(Double.parseDouble(result1[0]), Double.parseDouble(result1[1]));
		  	        }
		  	      });
		    m.add("Traffic Light Green Time",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	          //TODO set green time
		  	               String[] result1 = _ui.processForm(_twoDoubles);
		  	               _parameters.setGreenTime(Double.parseDouble(result1[0]), Double.parseDouble(result1[1]));
		  	        }
		  	      });
		    m.add("Traffic Light Yellow Time",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	          //TODO set yellow time
		  	               String[] result1 = _ui.processForm(_twoDoubles);
		  	               _parameters.setYellowTime(Double.parseDouble(result1[0]), Double.parseDouble(result1[1]));
		  	        }
		  	      });
		    m.add("Reset simulation and return to the main menu.",
		  	      new UIMenuAction() {
		  	        public void run() {
		  	          _state = START;
		  	        }
		  	      });
		    
		    _menus[stateNum] = m.toUIMenu("Edit Simulation Parameters");
	  }

	  private void addEXIT(int stateNum) {
	    UIMenuBuilder m = new UIMenuBuilder();
	    
	    m.add("Default", new UIMenuAction() { public void run() {} });
	    m.add("Yes",
	      new UIMenuAction() {
	        public void run() {
	          _state = EXITED;
	        }
	      });
	    m.add("No",
	      new UIMenuAction() {
	        public void run() {
	          _state = START;
	        }
	      });
	    
	    _menus[stateNum] = m.toUIMenu("Are you sure you want to exit?");
	  }
	}