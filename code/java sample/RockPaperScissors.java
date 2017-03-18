/*
While I have not worked extensively with object oriented PHP, I wanted to include some of my Java code 
so that you can see my capabilities with Object Oriented programming in general.  I am confident I can
use this knowledge to pick up Object Oriented PHP quickly.
*/

import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
public class JRockPaperScissors extends JApplet implements ActionListener
{
   //below I created the three buttons for the user choice, defined the layout
   //added a container and added some Strings which will be used to create a tally!
   private JButton rockButton = new JButton("Rock");
   private JButton paperButton = new JButton("Paper");
   private JButton scissorsButton = new JButton("Scissors");
   private FlowLayout flow = new FlowLayout();
   private Container con = getContentPane();
   private String compWinStr = "";
   private String userWinStr = "";
   private String tieStr = "";

   //below I'm adding the labels that create the text on the screen.  
   //later the total results will be defined by tallys but for now 
   //I'm starting with zero.  
   private JLabel title = new JLabel("Rock, Paper, Scissors");
   private JLabel resultsText = new JLabel("**************************RESULTS****************************");
   private JLabel totalResults = new JLabel("Computer: 0 User: 0 Ties: 0");
   //the choices label will be initiated when the user picks a button
   //then it will display what they picked (rock,paper, or scissors) and what
   //the computer picked by random number generating
   private JLabel choices = new JLabel();

   
   public void init()
   {
      //adding contents to the container in the order desired :-)
      con.setLayout(flow);
      title.setFont(new Font("Arial", Font.BOLD, 30));
      con.add(title);
      con.add(rockButton);
      con.add(paperButton);
      con.add(scissorsButton);
      con.add(resultsText);
      con.add(totalResults);
      con.add(choices);
      //adding action listeners
      rockButton.addActionListener(this);
      paperButton.addActionListener(this);
      scissorsButton.addActionListener(this);   
   }
   
   public void actionPerformed(ActionEvent event)
   {
      //here I'm randomly assigning a computer choice
      //then I'm initiating the userString and compString so that 
      //they can be defined later as Rock, Paper, or Scissors strings
      //based on the user choice and the computer's random number
      int compChoice = (int)(Math.random()*3);
      String userString = "";
      String compString = "";
      //here I'm using the action event to get the user's choice
      //and change the userString appropriately     
      Object source = event.getSource();    
      if(source == rockButton)
         userString = "Rock";
      else if(source == paperButton)
         userString = "Paper";
      else if(source == scissorsButton)
         userString = "Scissors";
      //here I'm using the random number to assign the appropriate
      //string to the compString (computer choice)   
      if(compChoice == 0)
         compString = "Rock";
      else if(compChoice == 1)
         compString = "Paper";
      else if(compChoice ==2)
         compString = "Scissors";
      //here I'm printing what the choices were between rock, paper, and scissors
      //for each plaer            
      choices.setText("You picked: " + userString + "  */*/*/*/*" +
         "  Computer picked: " + compString);            

      //here I'm creating a tally!
      if(userString.equals(compString))
      {
         tieStr = tieStr + "|";
      }
      else if( compString.equals("Rock") && userString.equals("Scissors") ||
               compString.equals("Paper") && userString.equals("Rock") ||
               compString.equals("Scissors") && userString.equals("Paper"))
      {
         compWinStr = compWinStr + "|";
      }
      else
      {
         userWinStr = userWinStr + "|";
      }     
      totalResults.setText("Computer: " + compWinStr + " User: " + userWinStr + " Ties: " + tieStr);        
   }
   //now I'm printing my information in the lower left corner!
   public void paint(Graphics brush)
  {
      super.paint(brush);
      String courseInfo1 = "Author: Erica Peharda";
      String courseInfo2 = "Course: CIS 263AA";
      String courseInfo3 = "Section Number: 33254";
      String courseInfo4 = "MEID: ERI2203192";
      brush.drawString(courseInfo1, 10, 350);
      brush.drawString(courseInfo2, 10, 365);
      brush.drawString(courseInfo3, 10, 380);
      brush.drawString(courseInfo4, 10, 395);
  }
}