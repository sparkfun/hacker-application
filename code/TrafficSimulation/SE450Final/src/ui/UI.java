package ui;

public interface UI {
  public void processMenu(UIMenu menu);
  public String[] processForm(UIForm form);
  public void displayMessage(String message);
  public void displayError(String message);
}
