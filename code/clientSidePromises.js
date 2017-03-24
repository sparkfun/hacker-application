//*********************************************************************
//The code below is used on the client side to process data for rendering 24 hour change in snow depth to the DOM.
//Deployed version:
//http://sunshine-daydream.surge.sh/
//The full repository can be found at:
//https://github.com/StephenHanzlik/ghostTowns
//The snippet below starts at line 241 of this document:
//https://github.com/StephenHanzlik/ghostTowns/blob/master/js/init.js
//*********************************************************************

//Create an array that will hold the returned promises for the Promise.all()
var allRequests = [];

//Send requests to the API for each specific snow station that we want to display
allRequests.push($.getJSON('https://g-powderlines.herokuapp.com/station/663:CO:SNTL'));

allRequests.push($.getJSON('https://g-powderlines.herokuapp.com/station/335:CO:SNTL'));

allRequests.push($.getJSON('https://g-powderlines.herokuapp.com/station/415:CO:SNTL'));

allRequests.push($.getJSON('https://g-powderlines.herokuapp.com/station/322:CO:SNTL'));

//Use a Promise.all to make sure all promises are fullfilled before attempting to render data in the DOM
Promise.all(allRequests).then(function(results) {
  //Create an simplified object with the results from the API
  var dailySnowObj = {};
  for (var i = 0; i < results.length; i++) {
    dailySnowObj[results[i].station_information.name] = results[i].data;
  }

  //Convert data type to an array form and get only the most recent observation
  var dailySnowArr = [];
  dailySnowArr[0] = dailySnowObj.NIWOT[5]["Change In Snow Depth (in)"];
  dailySnowArr[1] = dailySnowObj["COPPER MOUNTAIN"][5]["Change In Snow Depth (in)"];
  dailySnowArr[2] = dailySnowObj["BERTHOUD SUMMIT"][5]["Change In Snow Depth (in)"];
  dailySnowArr[3] = dailySnowObj["BEAR LAKE"][5]["Change In Snow Depth (in)"];

  //Convert data in the array to a string and check that a null value has not comeback from the API before rendering to DOM.
  var dailySnowNumArr = [];
  for (var c = 0; c < dailySnowArr.length; c++) {
    if (!isNaN(parseInt(dailySnowArr[c], 10))) {
      dailySnowNumArr.push(parseInt(dailySnowArr[c], 10));
    }
  }
  //Data can eventually be rendered to the DOM usign jQuery but is excluded here for brevity.
});
