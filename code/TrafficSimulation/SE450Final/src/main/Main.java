package main;

import main.Control;
import ui.UI;


public class Main {
	public enum UIType{text, swing}
	  private Main() {}
	  public static void main(String[] args) {
	    UI ui;
	    UIType type;

	    /*s
	    if (Math.random() <= 0.5) 
	    {
	    	ui = new ui.TextUI();
	    	type = UIType.text;
	    }
	    else
	    {
	    	*/
	    	ui = new ui.PopupUI();
	    	type = UIType.swing;
//	    }
	    Control control = new Control(ui, type);
	    control.run();
	  }
	}
