package agent;

import java.util.Observable;
import java.util.PriorityQueue;
import model.Agent;

public final class TimeServerQueue extends Observable implements TimeServer {
  private static final class Node implements Comparable<Node> {
    final double waketime;
    final Agent agent;
    public Node(double waketime, Agent agent) {
      this.waketime = waketime;
      this.agent = agent;
    }
    public int compareTo(Node that) {
      return (int) (Math.signum(this.waketime - that.waketime));
    }
  }
  private double _currentTime;
  private PriorityQueue<Node> _queue;

  public TimeServerQueue() {
    _queue = new PriorityQueue<Node>();
  }

  public String toString() {
    StringBuilder sb = new StringBuilder("[");
    String sep = "";
    Node[] nodes = _queue.toArray(new Node[0]);
    java.util.Arrays.sort(nodes);
    for (Node node : nodes) {
      sb.append(sep).append("(").append(node.waketime).append(",")
        .append(node.agent).append(")");
      sep = ";";
    }
    sb.append("]");
    return (sb.toString());
  }

  public double currentTime() {
    return _currentTime;
  }

  public void enqueue(double waketime, Agent agent)
    throws IllegalArgumentException
  {
    if (waketime < _currentTime)
      throw new IllegalArgumentException();
    _queue.add(new Node(waketime, agent));
  }

  Agent dequeue()
  {
    return _queue.remove().agent;
  }

  int size() {
    return _queue.size();
  }

  boolean empty() {
    return _queue.isEmpty();
  }

  public void run(double duration) {
    double endtime = _currentTime + duration;
    while ((!empty()) && (_queue.peek().waketime <= endtime)) {
      if ((_currentTime - _queue.peek().waketime) > 1e-09) {
        super.setChanged();
        super.notifyObservers();
      }
      _currentTime = _queue.peek().waketime;
      dequeue().run();
    }
    _currentTime = endtime;
  }
}



