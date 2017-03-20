package road;

import java.util.AbstractList;
import model.Agent;

import car.CarObj;

public interface RoadWay extends Agent {

	public AbstractList<CarObj> getCars();
	
	public double length();
	
	public void acceptCar(CarObj car);
	
	public void passCar(CarObj car);
	
	public RoadWay nextRoadWay();
}
