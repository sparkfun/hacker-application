package model.swing;

import model.MP;

/**
 * Static class for visualization parameters.
 */
class VP {
  private VP() {}
  /** Width of model elements, in meters */
  static double elementWidth = MP.carLength;
  /** Gap between model elements, in meters */
  static double gap = 1;
  /** Width of the displayed graphics window, in pixels */
  static int displayWidth = 1000;
  /** Height of the displayed graphics window, in pixels */
  static int displayHeight = 1000;
  /** Delay introduced into each graphics update, in milliseconds */
  static int displayDelay = 50;
  /** Scalefactor relating model space to pixels, in pixels/meter */
  static double scaleFactor = 1;
}
