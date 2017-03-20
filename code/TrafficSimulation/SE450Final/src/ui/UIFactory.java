package ui;

public class UIFactory {
  private UIFactory() {}
  static private UI _UI = new PopupUI();
  //static private UI _UI = new TextUI();
  static public UI ui () {
    return _UI;
  }
}
