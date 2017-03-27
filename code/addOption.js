/* Author: Mitchell Block
 * Date last modified: March 26th, 2017
 * Purpose: adds another input field to a form for little web decision maker
 */
var numOptions = 2;
function addOption(){
	var newOption = document.createElement("div");
	newOption.innerHTML = "OPTION " + (numOptions += 1) + ": <input id='individualOption' type='text' name='options[]'><br>";
	document.getElementById("listOfOptions").appendChild(newOption);
}