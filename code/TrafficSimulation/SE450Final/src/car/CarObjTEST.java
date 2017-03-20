package car;

import static org.junit.Assert.*;

import org.junit.Test;
import data.Parameters;
import data.GridConstructor;
import model.swing.SwingAnimatorBuilder;
import road.RoadSegmentObj;

public class CarObjTEST {

	Parameters _parameters = new Parameters();
	SwingAnimatorBuilder _builder = new SwingAnimatorBuilder();
	GridConstructor _grid = new GridConstructor(_parameters, _builder);
	RoadSegmentObj _road = new RoadSegmentObj(_parameters, true, _grid, true);
	
	CarObj car = new CarObj(_parameters, _road, _grid);
	@Test
	public void test() {
		assertTrue(car.length() > 5 && car.length() < 10);
	}

}
