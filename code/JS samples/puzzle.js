/*
I included this code for fun!  Enjoy


One Away:  There are three types of edits that can be performed on strings:  insert a character, remove a character, or replace a character.  Given two strings, write a function to check if they are one edit (or zero edits) away.
Example:
pale, ple     true
pales, pale   true
pale, bale    true
pale bake   false
*/

function oneAway(string1, string2){

  var string1array = string1.split("");
  console.log("string1array sorted:" + string1array);


  var string2array = string2.split("");
  console.log("string2array sorted:" + string2array);
  
  var bigArray = [];
  var smallArray = [];
  var edits = 0;
  
  if(string1array.length === string2array.length + 1){
    bigArray = string1array;
    smallArray = string2array;

  }else if(string2array.length === string1array.length + 1){
    bigArray = string2array;
    smallArray = string1array;

  }else if(string1array.length === string2array.length){
    bigArray = string1array;
    smallArray = string2array;

  }else{
    edits = 2;
  }

  for( var i =0; i < smallArray.length;i++){
    var j = i;
    if(smallArray[i] !== bigArray[i]){
      if(bigArray.length === smallArray.length){
        edits++;
      }else{
          bigArray.splice(i,1);
          edits++;
        }
    }
  }
  if(edits > 1){
    return false;
  }else{
    return true;
  }
}
/*test the code below*/
console.log(oneAway("pale","ale"));

/*
Runtime:  If S=length of long string (or both strings), R= length of short string, O(R)
*/