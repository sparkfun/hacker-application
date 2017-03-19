package model.swing;
import java.awt.Color;
import java.awt.Graphics;
import java.util.ArrayList;
import java.util.List;

//import SwingAnimatorBuilder.MyPainter.Element;
import model.AnimatorBuilder;
import model.Light;
import model.MP;
import util.Animator;
import util.SwingAnimator;
import util.SwingAnimatorPainter;
import road.RoadSegmentObj;
import road.RoadWay;
import car.CarObj;
import light.LightObj;

/** 
 * A class for building Animators.
 */
public class SwingAnimatorBuilder implements AnimatorBuilder {
  MyPainter _painter;
  public SwingAnimatorBuilder() {
    _painter = new MyPainter();
  }
  public Animator getAnimator() {
    if (_painter == null) { throw new IllegalStateException(); }
    Animator returnValue = new SwingAnimator(_painter, "Traffic Simulator",
                                             VP.displayWidth, VP.displayHeight, VP.displayDelay);
    _painter = null;
    return returnValue;
  }
  private static double skipInit = VP.gap;
  private static double skipRoad = VP.gap + MP.roadLength;
  private static double skipCar = VP.gap + VP.elementWidth;
  private static double skipRoadCar = skipRoad + skipCar;
  public void addLight(LightObj d, int i, int j) {
    double x = skipInit + skipRoad + j*skipRoadCar;
    double y = skipInit + skipRoad + i*skipRoadCar;
    Translator t = new TranslatorWE(x, y, MP.carLength, VP.elementWidth, VP.scaleFactor);
    _painter.addLight(d,t);
  }
  public void addHorizontalRoad(RoadSegmentObj l, int i, int j, boolean eastToWest) {
    double x = skipInit + j*skipRoadCar;
    double y = skipInit + skipRoad + i*skipRoadCar;
    Translator t = eastToWest ? new TranslatorEW(x, y, MP.roadLength, VP.elementWidth, VP.scaleFactor)
                              : new TranslatorWE(x, y, MP.roadLength, VP.elementWidth, VP.scaleFactor);
    _painter.addRoad(l,t);
  }
  public void addVerticalRoad(RoadSegmentObj l, int i, int j, boolean southToNorth) {
    double x = skipInit + skipRoad + j*skipRoadCar;
    double y = skipInit + i*skipRoadCar;
    Translator t = southToNorth ? new TranslatorSN(x, y, MP.roadLength, VP.elementWidth, VP.scaleFactor)
                                : new TranslatorNS(x, y, MP.roadLength, VP.elementWidth, VP.scaleFactor);
    _painter.addRoad(l,t);
  }


  /** Class for drawing the Model. */
  private static class MyPainter implements SwingAnimatorPainter {

    /** Pair of a model element <code>x</code> and a translator <code>t</code>. */
    private static class Element<T> {
      T x;
      Translator t;
      Element(T x, Translator t) {
        this.x = x;
        this.t = t;
      }
    }
    
    private List<Element<RoadSegmentObj>> _roadElements;
    private List<Element<LightObj>> _lightElements;
    MyPainter() {
      _roadElements = new ArrayList<Element<RoadSegmentObj>>();
      _lightElements = new ArrayList<Element<LightObj>>();
    }    
    void addLight(LightObj x, Translator t) {
      _lightElements.add(new Element<LightObj>(x,t));
    }
    void addRoad(RoadSegmentObj x, Translator t) {
      _roadElements.add(new Element<RoadSegmentObj>(x,t));
    }
    
    public void paint(Graphics g) {
      // This method is called by the swing thread, so may be called
      // at any time during execution...

      // First draw the background elements
      for (Element<LightObj> e : _lightElements) {
        if (e.x.getState()) {
          g.setColor(Color.BLUE);
        } else {
          g.setColor(Color.YELLOW);
        }
        XGraphics.fillOval(g, e.t, 0, 0, MP.carLength, VP.elementWidth);
      }
      g.setColor(Color.BLACK);
      for (Element<RoadSegmentObj> e : _roadElements) {
        XGraphics.fillRect(g, e.t, 0, 0, MP.roadLength, VP.elementWidth);
      }
      
      // Then draw the foreground elements
      // Maybe instead of iterating at the top level through the roadsegment objects
      // I should be iterating through RoadSets, then drill down to the segments and the cars.
      
      for (Element<RoadSegmentObj> e : _roadElements) {
          // iterate through a copy because e.x.getCars() may change during iteration...
          for (CarObj d : e.x.getCars().toArray(new CarObj[0])) {
            g.setColor(d.getColor());
            XGraphics.fillOval(g, e.t, d.currentPosition(), 0, MP.carLength, VP.elementWidth);
          }
      
          /*
      for (Element<RoadSegmentObj> e : _roadElements) {
          // iterate through a copy because e.x.getCars() may change during iteration...
      	  //need to iterate through each road segment and get each segment's list of cars.
      	  for (RoadWay RoadSeg : e.x.road().segments())
          for (CarObj d : RoadSeg.getCars().toArray(new CarObj[0])) 
    	  {
        	  g.setColor(d.getColor());
        	  //instead of d.currentPosition, should try setting an absolute maybe?  Try
        	  //doing the location of the start point of RoadSeg + d.currentPosition();
        	  XGraphics.fillOval(g, e.t, d.currentPosition(), 0, MP.carLength, VP.elementWidth);
        }*/
      }
    }
  }
}

