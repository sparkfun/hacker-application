var MyBobSystem, options, sliderValue, sketch, sketchBackground, slider,
	isActiveBobMode = false,
	sliderOptions = {};

function setup() {
	//Set options
	colorMode(HSB);
	frameRate(10);
	sketchBackground = 0;
	options = {
				doRunBobs: true,
				doDisplayBob: true,
				bobSize: 20,
				doRunInterference: true,
				fieldPulseRate: 1
			};


	sketch = createCanvas(windowWidth, windowHeight)
							.parent("sketch-container");
	setGui();
	MyBobSystem = new BobSystem(options);
	setSliderOptions();

	//Set events
	$('input').on('change', function(e){
		setSliderOptions(e);
	});
}

function draw() {
		background(sketchBackground);

		MyBobSystem.runBobs(sliderOptions);
}

function setGui() {
		activeBobButton = createButton('OFF', true)
											.parent("active-bob-button")
											.class("button-input")
											.id("active-bob-control")
											.mousePressed(toggleActiveBobMode);
		
		bobSlider = createSlider(2, 16, 4)
								.parent("bob-slider")
								.class("control-input")
								.id("bob-control");
		
		diversitySlider = createSlider(20, 255, 100)
											.parent("diversity-slider")
											.class("control-input")
											.id("diversity-control");
		
		bounceSlider = createSlider(0, 1, 1)
										.parent("bounce-slider")
										.class("control-input switch")
										.id("bounce-control");
		
		pairSlider = createSlider(0, 1, 1)
									.parent("pair-slider")
									.class("control-input switch")
									.id("pair-control");
		
		backgroundSlider = createSlider(0, 255, 10)
												.parent("background-slider")
												.class("control-input")
												.id("background-control");
}

function setSliderOptions(e) {
	if(e){
		if(e.target.id == "active-bob-control")
			sliderOptions.activeBobMode = isActiveBobMode;

		if(e.target.id == "bob-control")
			sliderOptions.bobAmount = bobSlider.value();

		if(e.target.id == "diversity-control")
			sliderOptions.diversityValue = diversitySlider.value();

		if(e.target.id == "bounce-control")
			sliderOptions.isPassThrough = bounceSlider.value();

		if(e.target.id == "pair-control")
			sliderOptions.isPairing = pairSlider.value();
	} else {
		sliderOptions = {
			'activeBobMode': isActiveBobMode,
			'bobAmount': bobSlider.value(),
			'diversityValue': diversitySlider.value(),
			'isPassThrough': bounceSlider.value(),
			'isPairing': pairSlider.value()
		}	
	}

}

function toggleActiveBobMode() {
	var $activeBobContainer = $("#active-bob-container");

	isActiveBobMode = !isActiveBobMode;

	$activeBobContainer.toggleClass('on');

	if(isActiveBobMode){
		activeBobButton.html('ON').addClass('on');    
	} else {
		activeBobButton.html('OFF').removeClass('on');
	}

	setSliderOptions();
}

function windowResized() {
	resizeCanvas(windowWidth, windowHeight);
}

function mousePressed() {
	if(isActiveBobMode){
		MyBobSystem.checkActiveBob(mouseX, mouseY);
	}
}