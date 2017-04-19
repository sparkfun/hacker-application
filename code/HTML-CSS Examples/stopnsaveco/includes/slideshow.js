
// Show slide shows of images.
function imgSlideOnOff(imgSrc, imgDefaultPhoto) {
 	if (document.images) 
	{
	
		var newImage = new Image();
		newImage.src = imgSrc;
		
		document.images[imgDefaultPhoto].src = newImage.src;
		
	}
 }