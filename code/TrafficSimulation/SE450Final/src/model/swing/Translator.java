package model.swing;

/**
 * A translator from relative model space to screen graphics.
 */
abstract class Translator {
  double _tX;
  double _tY;
  double _tWidth;
  double _tHeight;
  double _tScaleFactor;
  Translator(double tX, double tY, double tWidth, double tHeight, double tScaleFactor) {
    _tX = tX;
    _tY = tY;
    _tWidth = tWidth;
    _tHeight = tHeight;
    _tScaleFactor = tScaleFactor;
  }
  int scale(double arg) {
    return (int) Math.ceil(arg * _tScaleFactor); 
  }
  abstract int getX(double x, double y, double width, double height);
  abstract int getY(double x, double y, double width, double height);
  abstract int getWidth(double width, double height);
  abstract int getHeight(double width, double height);
}

class TranslatorWE extends Translator {
  TranslatorWE(double tX, double tY, double tWidth, double tHeight, double tScaleFactor) {
    super(tX, tY, tWidth, tHeight, tScaleFactor);
  }
  int getX(double x, double y, double width, double height) { return scale(_tX+x); }
  int getY(double x, double y, double width, double height) { return scale(_tY+y); }
  int getWidth(double width, double height) { return scale(width); }
  int getHeight(double width, double height)  { return scale(height); }
}

class TranslatorEW extends Translator {
  TranslatorEW(double tX, double tY, double tWidth, double tHeight, double tScaleFactor) {
    super(tX, tY, tWidth, tHeight, tScaleFactor);
  }
  int getX(double x, double y, double width, double height) { return scale(_tX+_tWidth-x-width); }
  int getY(double x, double y, double width, double height) { return scale(_tY+_tHeight-y-height); }
  int getWidth(double width, double height) { return scale(width); }
  int getHeight(double width, double height)  { return scale(height); }
}

class TranslatorNS extends Translator {
  TranslatorNS(double tX, double tY, double tWidth, double tHeight, double tScaleFactor) {
    super(tX, tY, tWidth, tHeight, tScaleFactor);
  }
  int getX(double x, double y, double width, double height) { return scale(_tX+y); }
  int getY(double x, double y, double width, double height) { return scale(_tY+x); }
  int getWidth(double width, double height) { return scale(height); }
  int getHeight(double width, double height)  { return scale(width); }
}

class TranslatorSN extends Translator {
  TranslatorSN(double tX, double tY, double tWidth, double tHeight, double tScaleFactor) {
    super(tX, tY, tWidth, tHeight, tScaleFactor);
  }
  int getX(double x, double y, double width, double height) { return scale(_tX+_tHeight-y-height); }
  int getY(double x, double y, double width, double height) { return scale(_tY+_tWidth-x-width); }
  int getWidth(double width, double height) { return scale(height); }
  int getHeight(double width, double height)  { return scale(width); }
}

