/* This code is for an animation on a site i was working on. It uses the library Paper.js to create the animations */

var gear_coeff = 30;
var gear_teeth = {
	'gear_huge' : 90, 
	'gear_large_spoke' : 60,
	'gear_large' : 60,
	'gear_medium_spoke' : 42,
	'gear_medium' : 30,
	'gear_small' : 23,
	'gear_tiny' : 15,
	'gear_micro' : 14
};

/* A couple of gear positions and sizes. I tried writing an algorithm for placing them inteligently without overlap, but it got really ugly doing it "brute force", and the when I tried implementing a kind of "flood fill / connected component" algorithm to locate large spaces to place the gears in, I decided it was getting overkill for something I could just configure once and be done with! Still a fun challeng, though maybe for another day. */

var gear_pos = [
{
	'size'	:	'gear_huge',
	'pos'	:	[ 7, 598 ],
	'dir'	:	-1
},
{
	'size'	:	'gear_huge',
	'pos'	:	[ 1206, 531 ],
	'dir'	:	1
},
{
	'size'	:	'gear_huge',
	'pos'	:	[ 851, 99 ],
	'dir'	:	-1
},
{
	'size'	:	'gear_large',
	'pos'	:	[ 1160, 204 ],
	'dir'	:	1
},
{
	'size'	:	'gear_large',
	'pos'	:	[ 339, 579 ],
	'dir'	:	-1
}];

for(var i=0; i<gear_pos.length; i++) {
	gear_pos[i]['raster'] = new Raster(gear_pos[i]['size']);
	x = gear_pos[i]['pos'][0] - 750;
	y = gear_pos[i]['pos'][1];
	gear_pos[i]['raster'].position.x = view.center.x + x;
	gear_pos[i]['raster'].position.y = y;
}

function onResize(event) {	
	for(var i=0; i<gear_pos.length; i++) {
		x = gear_pos[i]['pos'][0] - 750;
		gear_pos[i]['raster'].position.x = view.center.x + x;
	}
}

function onFrame() {
	for (var i=0; i<gear_pos.length; i++) {
		var dir = gear_pos[i]['dir'];
		var size = gear_pos[i]['size'];
		gear_pos[i]['raster'].rotate(gearSpeed(size, dir));
	}
}

function onMouseUp(event) {
	gear_coeff *= -1;
}

/* Utility Functions */
function gearSpeed(size, dir) {
	return (1 / gear_teeth[size]) * dir * gear_coeff;
}