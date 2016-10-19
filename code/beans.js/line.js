Beans.Line = function(start, end, color) {
  this.startVec = start;
  this.endVec = end;
  this.color = color;
  
  var origEndVec = new Beans.Vector2(end.x, end.y);
  
  this.rotate = function(angle) {
    angle = (Math.PI / 180) * angle;
    var rotation = new Beans.Matrix2x2(Math.cos(angle), Math.sin(angle),
      -Math.sin(angle), Math.cos(angle));
    
    // Before rotation subtract the pivot point (start vector)
    var newLocX = origEndVec.x - this.startVec.x;
    var newLocY = origEndVec.y - this.startVec.y;
    
    // Multiply by rotation vector and then add pivot point back
    var rotatedVec = rotation.multiply(new Beans.Vector2(newLocX, newLocY));
    this.endVec.x = Math.ceil(rotatedVec.x + this.startVec.x);
    this.endVec.y = Math.ceil(rotatedVec.y + this.startVec.y);
  }

  // The line drawing algorithm below implements the
  // Bresenham's line algorithm. More info found here
  // http://en.wikipedia.org/wiki/Bresenham's_line_algorithm
  this._draw = function(context) {
    var deltaX = Math.abs(this.endVec.x - this.startVec.x);
    var deltaY = Math.abs(this.endVec.y - this.startVec.y);
    var signX = this.startVec.x < this.endVec.x ? 1 : -1;
    var signY = this.startVec.y < this.endVec.y ? 1 : -1;
    var error = (deltaX > deltaY ? deltaX : -deltaY) / 2;
    
    var startXCopy = this.startVec.x;
    var startYCopy = this.startVec.y;

    while(true) {
      context._plot(startXCopy, startYCopy, this.color.red,
        this.color.green, this.color.blue, this.color.alpha);

      if(startXCopy == this.endVec.x && startYCopy == this.endVec.y)
        break;

      var tempError = error;

      if(tempError > -deltaX) {
        error -= deltaY;
        startXCopy += signX;
      }

      if(tempError < deltaY) {
        error += deltaX;
        startYCopy += signY;
      }
    }
  }
}
