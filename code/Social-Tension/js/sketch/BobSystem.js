//Bob System
//A system of Bobs.
//Handles behavior that happens on a system wide level.
function BobSystem(options) {
  var bobs = [],
      bobRunOptions = {},
      bobOptions = {
        bobSize: options.bobSize,
        doRunBobs: options.doRunBobs,
        doRunInterference: options.doRunInterference,
        fieldPulseRate: options.fieldPulseRate,
        doDisplayBob: options.doDisplayBob
      },
      mouseClickVector,
      activeBob;

  //addBob()
  //Adds a new bob to the system
  this.addBob = function(){
    var newBob = new Bob(bobOptions);
    bobs.push(newBob);
  }
  
  //runBobs(sliderOptions)
  //Operates all bobs
  //Params
  //sliderOptions {}
  //An object with data from the GUI sliders
  this.runBobs = function(sliderOptions){
    this.setBobRunOptions(sliderOptions);
    
    if(sliderOptions.bobAmount > bobs.length) {
      for(var i = bobs.length; i < sliderOptions.bobAmount; i++){
        this.addBob();
      }
    } else if(sliderOptions.bobAmount < bobs.length) {
      for(var i = sliderOptions.bobAmount; i < bobs.length; i++){
        bobs.pop();
      }
    }

  	bobs.forEach(this.runBob);
  }

  //runBob(bob, index, bobs)
  //Runs an individual bob
  //Params
  //bob Bob
  //The bob being run
  //index int
  //The index of the current bob being run
  //bobs []
  //The list of all bobs
  this.runBob = function(bob, index, bobs) {
  	bob.run(bobs, bobRunOptions);
  }

  //setBobRunOptions(sliderOptions)
  //Passes the slider options into the bob run options
  //Params
  //sliderOptions {}
  //An object containing GUI slider data
  this.setBobRunOptions = function(sliderOptions) {
    var tempFieldSize;

    tempFieldSize = 800 - 42 * (sliderOptions.bobAmount - 2);
    bobRunOptions.forceValue = sliderOptions.forceValue;
    bobRunOptions.activeBobMode = sliderOptions.activeBobMode;
    bobRunOptions.isPassThrough = sliderOptions.isPassThrough;
    bobRunOptions.isPairing = sliderOptions.isPairing;
    bobRunOptions.diversityValue = sliderOptions.diversityValue;
    bobRunOptions.fieldSize = tempFieldSize;
  }

  //checkActiveBob(xPos, yPos)
  //WHen in active bob mode and when mouse clicked,
  //checks to see if you have clicked a bob to set
  //it as active. If so, sets the bob as active.
  //Params
  //xPos float
  //x position of the mouse click
  //yPos float
  //y position of the mouse click
  this.checkActiveBob = function(xPos, yPos) {
    mouseClickVector = createVector(xPos, yPos);

    bobs.forEach(this.setActiveBobIndex);
    bobs.forEach(this.setActiveBob);
  }

  //setActiveBobIndex(bob, index, bobs)
  //Checks to see if bob has been clicked
  //Params
  //bob Bob
  //The bob being checked
  //index int
  //The index of the current bob
  //bobs []
  //The list of all bobs
  this.setActiveBobIndex = function(bob, index, bobs) {
    var vectorToMouse, mouseDistance;
    
    vectorToMouse = p5.Vector.sub(bob.position, mouseClickVector);
    mouseDistance = vectorToMouse.mag();

    if(mouseDistance <= bob.radius) {
      activeBob = index;
    }
  }

  //setActiveBobIndex(bob, index, bobs)
  //Sets bob as active if clicked, not active if not
  //Params
  //bob Bob
  //The bob being checked
  //index int
  //The index of the current bob
  //bobs []
  //The list of all bobs
  this.setActiveBob = function(bob, index, bobs) {
    if(activeBob === index) {
      bob.isActiveBob = true;
    } else {
      bob.isActiveBob = false;
    }
  }
}