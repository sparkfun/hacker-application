//Bob
//A colorful moving dot with a field of interference.
//This field causes friction when it intersects with
//the fields of other bobs.

function Bob(bobOptions) {

  //Internal Properties
  var bobView = this;

  bobView.properties = {
    'isActive': false,
    'size': bobOptions.bobSize,
    'radius': bobOptions.bobSize/2,
    'hue': Math.floor(Math.random() * 256),
    'saturation': '100',
    'behavior': 'pairing',
    'rings': Math.floor(Math.random() * 15 + 5)
  };

  bobView.environment = {
    'activeBobMode': false,
    'fieldPulseFrame': 0,
    'edgeCondition': 'passThrough',
    'runInterference': bobOptions.doRunInterference,
    'runBobBehavior': bobOptions.doRunBobs,
    'displayBob': bobOptions.doDisplayBob
  };

    //Scalars
  var fieldPulseRate,
      pushForce,
      driveForce = createVector(0, 0),
      tempXPos = Math.floor(Math.random() * (width-bobView.properties.size) + bobView.properties.size/2),
      tempYPos = Math.floor(Math.random() * (height-bobView.properties.size) + bobView.properties.size/2);

    //Vectors
  bobView.position = createVector(tempXPos, tempYPos);
  bobView.velocity = p5.Vector.random2D().mult(4);
  bobView.acceleration = createVector(0, 0);

    //Arrays
  bobView.forces = [];
  
  //bobView.run(bills)
  //Operates the Bob
  //Params
  //bobList []
  //List of all other bobs, passed in by bobSystem
  //bobRunOptions {} (optional)
  //Object containing dynamic variables that affect bob operation
  bobView.run = function(bobList, bobRunOptions) {
    if(bobRunOptions){
      bobView.setVariables(bobRunOptions);
    }

    if(bobView.environment.runInterference) {
      bobList.forEach(bobView.runInterference, this);
    }

    if(bobView.properties.isActive) {
      bobView.driveBob();
    }

    if(bobView.environment.runBobBehavior) {
      bobView.update(bobList);
    }

    bobView.display();
    bobView.reset();
  }

  //bobView.setVariables(bobRunOptions)
  //Set internal values with variables passed in by bobSystem
  //Params
  //bobRunOptions {}
  //Object containing variables to be passed into the bob
  bobView.setVariables = function(bobRunOptions) {
    pushForce = bobRunOptions.forceValue;
    bobView.environment.activeBobMode = bobRunOptions.activeBobMode;
    bobView.properties.saturation = bobRunOptions.diversityValue;
    
    bobView.fieldSize = bobRunOptions.fieldSize;
    bobView.fieldRadius = bobView.fieldSize/2;
    bobView.fieldIncrement = bobView.fieldRadius/bobView.properties.rings;
    bobView.fieldIncrementMultiplier = Math.random() * 2 + 1;

    if(bobView.environment.activeBobMode) {
      fieldPulseRate = 0.5;
    } else {
      fieldPulseRate = 0;
      bobView.properties.isActive = false;
    }
    if(bobRunOptions.isPassThrough) {
      bobView.environment.edgeCondition = 'passThrough';
    } else {
      bobView.environment.edgeCondition = 'bounce';
    }
    if(bobRunOptions.isPairing) {
      bobView.properties.behavior = 'pairing';
    } else {
      bobView.properties.behavior = 'grouping';
    }
  }
  
  //bobView.update()
  //Calculates the accerlation, velocity, and position of the bob
  bobView.update = function() {
    bobView.forces.forEach(bobView.addForce);
    bobView.velocity.add(bobView.acceleration.x, bobView.acceleration.y);
    bobView.velocity.limit(7);
    bobView.position.add(bobView.velocity.x, bobView.velocity.y);
    if(bobView.environment.edgeCondition == 'passThrough'){
      bobView.passThrough();
    } else {
      bobView.checkForWalls();
    }
  }

  //bobView.checkForWalls()
  //When in bounce mode, handles intersections with walls
  bobView.checkForWalls = function() {
    if((bobView.position.x - bobView.properties.radius) <= 0){  
      bobView.position.x = bobView.properties.radius;
      if(bobView.velocity.x < 0){
        bobView.velocity.x *= -1; 
      }
    }
    if((bobView.position.x + bobView.properties.radius) >= width){  
      bobView.position.x = width - bobView.properties.radius; 
      if(bobView.velocity.x > 0){
        bobView.velocity.x *= -1; 
      }
    }
    if((bobView.position.y - bobView.properties.radius) <= 0){ 
      bobView.position.y = bobView.properties.radius; 
      if(bobView.velocity.y < 0){
        bobView.velocity.y *= -1; 
      }
    }
    if((bobView.position.y + bobView.properties.radius) >= height){ 
      bobView.position.y = height - bobView.properties.radius; 
      if(bobView.velocity.y > 0){
        bobView.velocity.y *= -1; 
      }
    }
  }

  //bobView.passThrough()
  //When in pass through mode, handles passing through walls
  bobView.passThrough = function() {
    if((bobView.position.x + bobView.properties.radius) <= 0){  
      bobView.position.x = width;
    }
    if((bobView.position.x - bobView.properties.radius) >= width){  
      bobView.position.x = 0 - bobView.properties.radius;
    }
    if((bobView.position.y + bobView.properties.radius) <= 0){ 
      bobView.position.y = height;
    }
    if((bobView.position.y - bobView.properties.radius) >= height){ 
      bobView.position.y = 0 - bobView.properties.radius;
    }
  }


  //bobView.addForce(force, index, forces)
  //Adds a force vector to the acceleration vector
  //Params
  //force p5.Vector
  //Force to be added to the list
  //index int
  //index of force within list
  //forces []
  //The list of forces
  bobView.addForce = function(force, index, forces) {
    bobView.acceleration.add(force.x, force.y);
  }
  
  //bobView.display()
  //Runs the functions that create the visual appearance of the Bob
  bobView.display = function() {
    if(bobView.environment.activeBobMode && bobView.properties.isActive){
      bobView.renderField();
    }
    bobView.renderBob();
  }
  
  //bobView.runInterference(bob, index, bobList)
  //Calculates interference points between this bob's fields
  //and other bobs' fields.
  //bob Bob
  //Another bob
  //index int
  //The index of the other bob in the list of bobs
  //bobList []
  //The list of all other bobs
  bobView.runInterference = function(bob, index, bobList) {
  	var thisBob = bobView,
  		  otherBob = bob,
        distance = p5.Vector.dist(thisBob.position, otherBob.position),
        dVector = p5.Vector.sub(otherBob.position, thisBob.position),
        dNormal = dVector.normalize();

    bobView.environment.fieldPulseFrame = bobView.environment.fieldPulseFrame % bobView.fieldIncrement;
    
    //if otherBob is not thisBob
    if(distance > 0) {
      //for each ring of thisBob's field
      for(var i = bobView.environment.fieldPulseFrame; i < thisBob.fieldRadius; i+=thisBob.fieldIncrement) {
        //for each ring of otherBob's field
        for(var j = bobView.environment.fieldPulseFrame; j < otherBob.fieldRadius; j+=otherBob.fieldIncrement) {
          //check if the two rings intersect
          var areIntersecting = checkIntersect(
                                thisBob.position.x, 
                                thisBob.position.y, 
                                i, 
                                otherBob.position.x, 
                                otherBob.position.y, 
                                j
                              );
                                              
          switch(areIntersecting) {
          //fields intersect and have intersection points
            case 1:
              bobView.handleIntersection(thisBob, otherBob, distance, i, j);
              break;

          //If one of the fields is contained in the other
            case -1:
              // renderOverlapShape(i);
              break;

            default:
              break;
          }
        }
      }
    }
  }

  //bobView.handleIntersection(thisBob, otherBob, distance, i, j)
  //Handles the intersection between two bobs
  //Params
  //thisBob Bob
  //This bob
  //otherBob Bob
  //The bob currently beeing intersected with
  //distance float
  //The distance between the bobs in question
  //i int
  //The index of this bob's field currently being intersected
  //j int
  //The index of the other bob's field currently being intersected
  bobView.handleIntersection = function(thisBob, otherBob, distance, i, j) {
    var intersections,
        firstIntersectionPoint,
        secondIntersectionPoint,
        pushForceFactor,
        pushVector1,
        pushVector2,
        hueDifference;

    intersections = getIntersections(
                                      thisBob.position.x, 
                                      thisBob.position.y, 
                                      i, 
                                      otherBob.position.x, 
                                      otherBob.position.y, 
                                      j
                                    );

    firstIntersectionPoint = createVector(intersections[0], intersections[1]);
    secondIntersectionPoint = createVector(intersections[2], intersections[3]);

    if(!bobView.environment.activeBobMode || (bobView.environment.activeBobMode && bobView.properties.isActive)) {
      bobView.renderIntersectShape(intersections, distance, otherBob.properties.hue, i);
    }

    hueDifference = bobView.getHueGap(thisBob.properties.hue, otherBob.properties.hue);

    if(bobView.properties.behavior == 'pairing') {
      pushForce = (43.75 - hueDifference)/63.75;
    } else {
      pushForce = (hueDifference - 20)/63.75;
    }

    diversityFactor = map(bobView.properties.saturation, 0, 255, 0, 63.75);

    pushForce *= diversityFactor;

    pushForceFactor = pushForce / (i * j);

    pushVector1 = p5.Vector.sub(thisBob.position, firstIntersectionPoint)
      .normalize()
      .mult(pushForceFactor);

    pushVector2 = p5.Vector.sub(thisBob.position, secondIntersectionPoint)
      .normalize()
      .mult(pushForceFactor);

    bobView.forces.push(pushVector1);
    bobView.forces.push(pushVector2);
  }

  //bobView.renderBob()
  //Displays the visual appearance of the bob
  bobView.renderBob = function() {
    noStroke();

    if(bobView.properties.isActive) {
      strokeWeight(4);
      stroke(0, 0, 255, 100);
    }

    if(bobView.properties.isActive || !bobView.environment.activeBobMode){
      fill(bobView.properties.hue, bobView.properties.saturation, 200);
    } else {
      fill(bobView.properties.hue, bobView.properties.saturation, 200, 150);
    }
    ellipse(bobView.position.x, bobView.position.y, bobView.properties.size, bobView.properties.size);
  }
  
  //bobView.renderIntersectShape(intersections, distance, otherHuse, i)
  //Renders the visual appearance of the interference points
  //Params
  //intersection []
  //List of the intersection points to be rendered
  //distance float
  //Distance between the two bobs
  //otherHue float
  //The hue value of the other Bob
  //i float
  //Radius of current field ring (for calculating opacity)
  bobView.renderIntersectShape = function(intersections, distance, otherHue, i) {
    var circleNormal = createVector(bobView.properties.radius, 0),
        distIntA = createVector(intersections[0], intersections[1]),
        distIntB = createVector(intersections[2], intersections[3]),
        angle1, angle2, newHue, opacity;

    newHue = bobView.averageHues(bobView.properties.hue, otherHue);

    if(bobView.environment.activeBobMode && bobView.properties.isActive) {
      opacity = map(i, 0, bobView.fieldSize, 0, 255);
      opacity = (255-opacity);
    } else {
      opacity = 200;
    }
        
    
    //Dots
    var dotSize = 3;
    noStroke();

    for (var i = dotSize; i > 0; i--){
      fill(newHue, bobView.properties.saturation, 200, (opacity/3));
      ellipse(distIntA.x, distIntA.y, dotSize, dotSize);
      if(bobView.environment.activeBobMode && bobView.properties.isActive) {
        ellipse(distIntB.x, distIntB.y, dotSize, dotSize);
      }
    }
    //Arcs
    /*
    distIntA.sub(position);
    distIntB.sub(position);
    
    if(distance.x > 0){
      if(distIntA.y < distance.y){
        angle1 = getArcAngle(circleNormal, distIntA);
        angle2 = getArcAngle(circleNormal, distIntB);
      } else {
        angle1 = getArcAngle(circleNormal, distIntB);
        angle2 = getArcAngle(circleNormal, distIntA);
      }
      
      if((angle1 - PI) >  angle2){
        angle2 += TWO_PI;
      }
    } else {
      if(distIntA.y > distance.y){
        angle1 = getArcAngle(circleNormal, distIntA);
        angle2 = getArcAngle(circleNormal, distIntB);
      } else {
        angle1 = getArcAngle(circleNormal, distIntB);
        angle2 = getArcAngle(circleNormal, distIntA);
      }
      
      if((angle1 - PI) >  angle2){
        angle2 += TWO_PI;
      }
    }
    fill(100, 1);
    noStroke();
    arc(position.x, position.y, 2*tempSize, 2*tempSize, angle1, angle2, OPEN);
    */
  }

  //bobView.getHueGap(hue1, hue2)
  //Calculates the shortest distance between two hues
  //Params
  //hue1 float
  //The first hue value
  //hue2 float
  //The second hue value
  bobView.getHueGap = function(hue1, hue2) {
    var hueDifference, hueGap;
    hueDifference = Math.abs(hue1 - hue2);

    if(hueDifference > 128){
      hueGap = (255 - hueDifference)/2;
    } else {
      hueGap = hueDifference/2;
    }

    return hueGap;
  }

  //bobView.getHueGap(hue1, hue2)
  //Calculates the average between two hues
  //Params
  //hue1 float
  //The first hue value
  //hue2 float
  //The second hue value
  bobView.averageHues = function(hue1, hue2) {
    var baseHue, newHue, hueGap, hueDifference,
        maxHue = 255;
    hueDifference = Math.abs(hue1 - hue2);
    
    if(hueDifference > (maxHue/2)){
      if(hue1 > hue2) {
        baseHue = hue1;
      } else {
        baseHue = hue2;
      }

      hueGap = (maxHue - hueDifference)/2;
    } else {
      if(hue1 < hue2) {
        baseHue = hue1;
      } else {
        baseHue = hue2;
      }

      hueGap = hueDifference/2;
    }

    newHue = (baseHue + hueGap) % maxHue;
    return newHue;
  }
  
  //bobView.renderOverlapShape(shapeSize)
  //For rendering fields rings that overlap fully with other field rings
  bobView.renderOverlapShape = function(shapeSize){
    noFill();
    strokeWeight(2);
    stroke(0, 0, 0);
    ellipse(bobView.position.x, bobView.position.y, 2*shapeSize, 2*shapeSize);
  }
  
  //bobView.renderField()
  //Displays the bob field when in display field mode
  bobView.renderField = function() {
    strokeWeight(1);
    noFill();
    for(var i = bobView.environment.fieldPulseFrame; i < bobView.fieldRadius; i+=bobView.fieldIncrement){
      var opacity = map(i, 0, bobView.fieldRadius, 255, 0);
      stroke(bobView.properties.hue, 200, 200, opacity);
      ellipse(bobView.position.x, bobView.position.y, 2*i, 2*i);
    }
  }

  //bobView.driveBob()
  //Handles key presses when is active bob
  bobView.driveBob = function() {
    var driveForceMag,
        driveForceIncrement = 0.1;

    if (keyIsDown(LEFT_ARROW))
      driveForce.add(-1 * driveForceIncrement, 0);

    if (keyIsDown(RIGHT_ARROW))
      driveForce.add(driveForceIncrement, 0);

    if (keyIsDown(UP_ARROW))
      driveForce.add(0, -1 * driveForceIncrement);

    if (keyIsDown(DOWN_ARROW))
      driveForce.add(0, driveForceIncrement);

    bobView.forces.push(driveForce);

    driveForceMag = driveForce.mag();

    
    if(driveForceMag > 0.01){
      driveForce.mult(0.9);
    } else if(driveForceMag > 0){
      driveForce.mult(0);
    }
  }

  //bobView.reset()
  //Clears dynamic variables at end of run
  bobView.reset = function() {
    bobView.acceleration.mult(0);
    bobView.velocity.mult(0.99999);
    bobView.forces = [];
    bobView.environment.fieldPulseFrame += fieldPulseRate;
  }
}