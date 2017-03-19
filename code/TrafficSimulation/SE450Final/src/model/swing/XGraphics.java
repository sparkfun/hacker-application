package model.swing;

import java.awt.Graphics;

/**
 * Static class for drawing using a Translation.
 */
class XGraphics {
  private XGraphics() {}
  static void clearRect(Graphics g, Translator t, double x, double y, double w, double h) {
    g.clearRect(t.getX(x,y,w,h), t.getY(x,y,w,h), t.getWidth(w,h), t.getHeight(w,h));
  }
  static void drawOval(Graphics g, Translator t, double x, double y, double w, double h) {
    g.drawOval(t.getX(x,y,w,h), t.getY(x,y,w,h), t.getWidth(w,h), t.getHeight(w,h));
  }
  static void drawRect(Graphics g, Translator t, double x, double y, double w, double h) {
    g.drawRect(t.getX(x,y,w,h), t.getY(x,y,w,h), t.getWidth(w,h), t.getHeight(w,h));
  }
  static void fillOval(Graphics g, Translator t, double x, double y, double w, double h) {
    g.fillOval(t.getX(x,y,w,h), t.getY(x,y,w,h), t.getWidth(w,h), t.getHeight(w,h));
  }
  static void fillRect(Graphics g, Translator t, double x, double y, double w, double h) {
    g.fillRect(t.getX(x,y,w,h), t.getY(x,y,w,h), t.getWidth(w,h), t.getHeight(w,h));
  }
  static void drawString(Graphics g, Translator t, String str, double x, double y) {
    g.drawString(str, t.getX(x,y,0,0), t.getY(x,y,0,0));
  }
}
