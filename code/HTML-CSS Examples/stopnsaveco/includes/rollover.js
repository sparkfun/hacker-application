
// Function to activate image
function imgOnOff(imgName, imgSrc) {
 	if (document.images) {
	
		var newImage = new Image();
		newImage.src = imgSrc;
		
		document.images[imgName].src = newImage.src;
	}
 }