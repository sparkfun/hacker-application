/* 
 * Sets up the avatar's accessories with indiviaul parameters.  
 * TODO: Refactor as objects rather than assoc arrays
 */

var toolbar = new Array();

/*********************/

/* Headwear Category

/*********************/

var headwear = new Array();
headwear['name'] = "Headwear";

/*********************/
/* Floppy Hat 
/*********************/

var floppyhat = new Array
floppyhat['name'] = "Floppy Hat";
floppyhat['id'] = "floppyhat";
floppyhat['src'] = "floppyhat.png";
floppyhat['x'] = "116";
floppyhat['y'] = "10";
floppyhat['category'] = "Headwear";
floppyhat['zindex'] = "1";
headwear.push(floppyhat);

/*********************/
/* Stylist Hat 
/*********************/

var stylishhat = new Array
stylishhat['name'] = "Stylish Hat";
stylishhat['id'] = "stylishhat";
stylishhat['src'] = "stylishhat.png";
stylishhat['x'] = "126";
stylishhat['y'] = "10";
stylishhat['category'] = "Headwear";
stylishhat['zindex'] = "1";
headwear.push(stylishhat);

/*********************/
/* Cap  
/*********************/

var cap = new Array
cap['name'] = "Cap";
cap['id'] = "cap";
cap['src'] = "cap.png";
cap['x'] = "132";
cap['y'] = "7";
cap['category'] = "Headwear";
cap['zindex'] = "1";
headwear.push(cap);

/*********************/
/* Cowboy Hat 
/*********************/

var cowboyhat = new Array
cowboyhat['name'] = "Cowboy Hat";
cowboyhat['id'] = "cowboyhat";
cowboyhat['src'] = "cowboyhat.png";
cowboyhat['x'] = "129";
cowboyhat['y'] = "-10";
cowboyhat['category'] = "Headwear";
cowboyhat['zindex'] = "1";
headwear.push(cowboyhat);

toolbar.push(headwear);

/*********************/

/* Eyewear Category */

/*********************/

var eyewear = new Array();
eyewear['name'] = "Eyewear";

/*********************/
/* Nerd Glasses
/*********************/

var nerdGlasses = new Array();
nerdGlasses['name'] = "Glasses";
nerdGlasses['id'] = "nerdglasses";
nerdGlasses['src'] = "nerdglasses.png";
nerdGlasses['x'] = "146";
nerdGlasses['y'] = "83";
nerdGlasses['category'] = "Eyewear";
nerdGlasses['zindex'] = "1";
eyewear.push(nerdGlasses);

/*********************/
/* Specs
/*********************/
var specs = new Array();
specs['name'] = "Specs";
specs['id'] = "specs";
specs['src'] = "specs.png";
specs['x'] = "145";
specs['y'] = "82";
specs['category'] = "Eyewear";
specs['zindex'] = "1";
eyewear.push(specs);

/*********************/
/* Shades
/*********************/
var shades = new Array();
shades['name'] = "Shades";
shades['id'] = "shades";
shades['src'] = "shades.png";
shades['x'] = "148";
shades['y'] = "80";
shades['category'] = "Eyewear";
shades['zindex'] = "1";
eyewear.push(shades);

/*********************/
/* Horn-Rimmed Glasses
/*********************/
var hornrimmed = new Array();
hornrimmed['name'] = "Horn Rimmed Glasses";
hornrimmed['id'] = "hornrimmed";
hornrimmed['src'] = "hornrimmed.png";
hornrimmed['x'] = "142";
hornrimmed['y'] = "68";
hornrimmed['category'] = "Eyewear";
hornrimmed['zindex'] = "1";
eyewear.push(hornrimmed);

toolbar.push(eyewear);

/*********************/

/* Footwear Category */

/*********************/

var footwear = new Array();
footwear['name'] = "Footwear";

/*********************/
/* Boots
/*********************/
var boots = new Array();
boots['name'] = "Boots";
boots['id'] = "boots";
boots['src'] = "boots.png";
boots['x'] = "124";
boots['y'] = "360";
boots['category'] = "Footwear";
boots['zindex'] = "1";
footwear.push(boots);

/*********************/
/* Sneakers
/*********************/
var sneakers = new Array();
sneakers['name'] = "Sneakers";
sneakers['id'] = "sneakers";
sneakers['src'] = "sneakers.png";
sneakers['x'] = "126";
sneakers['y'] = "368";
sneakers['category'] = "Footwear";
sneakers['zindex'] = "1";
footwear.push(sneakers);

/*********************/
/* Cowboy Boots
/*********************/
var cowboyboots = new Array();
cowboyboots['name'] = "Cowboy Boots";
cowboyboots['id'] = "cowboyboots";
cowboyboots['src'] = "cowboyboots.png";
cowboyboots['x'] = "130";
cowboyboots['y'] = "340";
cowboyboots['category'] = "Footwear";
cowboyboots['zindex'] = "1";
footwear.push(cowboyboots);

/*********************/
/* Heels
/*********************/
var heels = new Array();
heels['name'] = "Heels";
heels['id'] = "heels";
heels['src'] = "heels.png";
heels['x'] = "132";
heels['y'] = "381";
heels['category'] = "Footwear";
heels['zindex'] = "1";
footwear.push(heels);

toolbar.push(footwear);

/*********************/

/* Accessories Category */

/*********************/

var gadgets = new Array();
gadgets['name'] = "Accessories";

/*********************/
/* Bowtie
/*********************/
var bowtie = new Array();
bowtie['name'] = "Bowtie";
bowtie['id'] = "bowtie";
bowtie['src'] = "bowtie.png";
bowtie['x'] = "152";
bowtie['y'] = "137";
bowtie['category'] = "Accessories";
bowtie['zindex'] = "2";
gadgets.push(bowtie);

/*********************/
/* Necklace
/*********************/
var necklace = new Array();
necklace['name'] = "Necklace";
necklace['id'] = "necklace";
necklace['src'] = "necklace.png";
necklace['x'] = "145";
necklace['y'] = "140";
necklace['category'] = "Accessories";
necklace['zindex'] = "2";
gadgets.push(necklace);

/*********************/
/* Briefcase
/*********************/
var briefcase = new Array();
briefcase['name'] = "Briefcase";
briefcase['id'] = "briefcase";
briefcase['src'] = "case.png";
briefcase['x'] = "197";
briefcase['y'] = "273";
briefcase['category'] = "Accessories";
briefcase['zindex'] = "2";
gadgets.push(briefcase);

/*********************/
/* Purse
/*********************/
var purse = new Array();
purse['name'] = "Purse";
purse['id'] = "purse";
purse['src'] = "purse.png";
purse['x'] = "120";
purse['y'] = "280";
purse['category'] = "Accessories";
purse['zindex'] = "2";
gadgets.push(purse);

toolbar.push(gadgets);
