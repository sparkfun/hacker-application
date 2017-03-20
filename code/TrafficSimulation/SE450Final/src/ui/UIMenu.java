package ui;

/**
 * @see UIMenuBuilder
 */
public final class UIMenu {
  private final String _heading;
  private final Pair[] _menu;

  static final class Pair {
    final String prompt;
    final UIMenuAction action;

    Pair(String thePrompt, UIMenuAction theAction) {
      prompt = thePrompt;
      action = theAction;
    }
  }

  UIMenu(String heading, Pair[] menu) {
    _heading = heading;
    _menu = menu;
  }
  public int size() {
    return _menu.length;
  }
  public String getHeading() {
    return _heading;
  }
  public String getPrompt(int i) {
    return _menu[i].prompt;
  }
  public void runAction(int i) {
    _menu[i].action.run();
  }
}
