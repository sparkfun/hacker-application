/* 
 * Sets up the avatar's accessories with indiviaul parameters.  
 * TODO: Refactor as objects rather than assoc arrays
 * Note: ids cannot contain numbers or underscores, only letters
 */

var symptoms = new Array();

/*********************/
/* Bricks Category */
/*********************/

var bricks = new Array();
bricks['name'] = "Bricks";
bricks['id'] = "bricks";


var bricks_icon = new Array();
bricks_icon['name'] = "Bricks";
bricks_icon['id'] = "bricks_icon";
bricks_icon['src'] = "bricks.png";
bricks.push(bricks_icon);

symptoms.push(bricks);

/*********************/
/* Boxing Gloves Category */
/*********************/

var gloves = new Array();
gloves['name'] = "Boxing Gloves";
gloves['id'] = "gloves";

var gloves_right = new Array();
gloves_right['name'] = "Boxing Gloves";
gloves_right['id'] = "gloves-right";
gloves_right['src'] = "gloves.png";
gloves.push(gloves_right);

var gloves_left = new Array();
gloves_left['name'] = "Boxing Gloves";
gloves_left['id'] = "gloves-left";
gloves_left['src'] = "gloves-right.png";
gloves.push(gloves_left);

symptoms.push(gloves);

/*********************/
/* Lightning Bolt Category */
/*********************/

var bolt = new Array();
bolt['name'] = "Lightning Bolt";
bolt['id'] = "bolt";

var bolt_right = new Array();
bolt_right['name'] = "Lightning Bolt";
bolt_right['id'] = "bolt-right";
bolt_right['src'] = "lightning.png"; 
bolt.push(bolt_right);

var bolt_left = new Array();
bolt_left['name'] = "Lightning Bolt";
bolt_left['id'] = "bolt-left";
bolt_left['src'] = "lightning-right.png"; 
bolt.push(bolt_left);

symptoms.push(bolt);