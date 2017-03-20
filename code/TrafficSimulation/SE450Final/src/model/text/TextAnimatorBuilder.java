package model.text;

import java.util.ArrayList;
import java.util.List;
import java.util.Observable;
import java.util.Observer;
import model.AnimatorBuilder;
import model.Light;
import model.MP;
import util.Animator;
import road.RoadSegmentObj;
import car.CarObj;
import light.LightObj;

/** 
 * A class for building Animators.
 */
public class TextAnimatorBuilder implements AnimatorBuilder {
  TextAnimator _animator;
  public TextAnimatorBuilder() {
    _animator = new TextAnimator();
  }
  public Animator getAnimator() {
    if (_animator == null) { throw new IllegalStateException(); }
    Animator returnValue = _animator;
    _animator = null;
    return returnValue;
  }
  public void addLight(LightObj d, int i, int j) {
    _animator.addLight(d,i,j);
  }
  public void addHorizontalRoad(RoadSegmentObj l, int i, int j, boolean eastToWest) {
    _animator.addRoad(l,i,j);
  }
  public void addVerticalRoad(RoadSegmentObj l, int i, int j, boolean southToNorth) {
    _animator.addRoad(l,i,j);
  }


  /** Class for drawing the Model. */
  private static class TextAnimator implements Animator {

    /** Triple of a model element <code>x</code> and coordinates. */
    private static class Element<T> {
      T x;
      int i;
      int j;
      Element(T x, int i, int j) {
        this.x = x;
        this.i = i;
        this.j = j;
      }
    }
    
    private List<Element<RoadSegmentObj>> _roadElements;
    private List<Element<LightObj>> _lightElements;
    TextAnimator() {
      _roadElements = new ArrayList<Element<RoadSegmentObj>>();
      _lightElements = new ArrayList<Element<LightObj>>();
    }    
    void addLight(LightObj x, int i, int j) {
      _lightElements.add(new Element<LightObj>(x,i,j));
    }
    void addRoad(RoadSegmentObj x, int i, int j) {
      _roadElements.add(new Element<RoadSegmentObj>(x,i,j));
    }
    
    public void dispose() { }
    
    public void update(Observable o, Object arg) {
      for (Element<LightObj> e : _lightElements) {
        System.out.print("Light at (" + e.i + "," + e.j + "): ");
        if (e.x.getState()) {
          System.out.println("Blue");
        } else {
          System.out.println("Yellow");
        }
      }
      for (Element<RoadSegmentObj> e : _roadElements) {
        for (CarObj d : e.x.getCars()) {
          System.out.print("Road at (" + e.i + "," + e.j + "): ");
          System.out.println("Car at " + d.currentPosition());
        }
      }
    }
  }
}
