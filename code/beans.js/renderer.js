Beans.Renderer = function(canvasId) {
  var canvas = document.getElementById(canvasId);
  var context = canvas.getContext("2d");
  var imageData = context.createImageData(canvas.width, canvas.height);
  var refreshRate = 1000 / 60; // 60 fps
  var objectsToRender = new Array();

  this.add = function(object) {
    objectsToRender.push(object);
  }

  this.start = function(external) {
    // Save a reference to this context because "this" changes
    // in the anonymous function below.
    var that = this;

    // The game loop :D
    setInterval(function() {
      external();
      
      // Go through all of the objects and fill up the image data buffer
      // with pixels.
      for(var i = 0; i < objectsToRender.length; i++) {
        objectsToRender[i]._draw(that);
      }

      // Blast the pixels to the screen!
      context.putImageData(imageData, 0, 0);
      imageData = context.createImageData(canvas.width, canvas.height);
    }, refreshRate);
  }

  this._plot = function(x, y, red, green, blue, alpha) {
    // Since the drawing view is just on big array of pixels,
    // calculate where to correctly plot this point based on
    // the fact every fourth index location in the array is
    // an actual pixel.
    
    // Adjust the x and y points to world coordinate space
    // origin (0, 0)
    var localToWorldX = x + canvas.width/2;
    var localToWorldY = y + canvas.height/2;
    
    // Don't render anything out of the screen's bounds
    if(localToWorldX >= canvas.width || localToWorldX < 0 ||
      localToWorldY >= canvas.height || localToWorldY < 0)
      return;
    
    var index = (localToWorldX * 4) + (localToWorldY * canvas.width * 4);
    imageData.data[index] = red;
    imageData.data[index+1] = green;
    imageData.data[index+2] = blue;
    imageData.data[index+3] = alpha;
  }
}
