/*
 * Initializes the symptoms toolbar with accessories from symptomLib.js
 * Accessories may be added to the avatar by dragging and dropping into predefined spaces
 * Since a symptom may be present anywhere, the user may drag a symptom into any predefined spot
 * As a business rule, no symptom may cover up the Avatar, hence the reason we have predefined placement for symptoms
 */
function init() {
	//Initializes accessories icons
	initToolbar();
	
	//Initializes symptom icons
	initSymptoms();
	
	initMerge();
	
	//Keeps track of layer IDs by making sure they are unique
	objcount = 0;
}

/*
 * Initializes the toolbar with accessories from toolbarLib.js
 * Accessories may be added to the avatar by clicking them
 * The toolbar library assigns them certain x and y values so that they are placed in the right spots
 * Hats go in the hat position, shoes go in the shoes position, etc.
 */
function initToolbar() {
	for (var i = 0; i< toolbar.length; i++){
		var name = toolbar[i]['name'];
		registerToolCategory(toolbar[i]);
			for (var j = 0; j< toolbar[i].length; j++){
				registerTool(toolbar[i], toolbar[i][j]);
			}
	}	
	
	$('._toolbarCategory').mouseover(function() {
		$(this).children('ul').css('display', 'block');
		
	});

	$('._toolbarCategory').mouseout(function() {
		$(this).children('ul').css('display', 'none');
	});
	
	var ua = navigator.userAgent,
    event = (ua.match(/iPad/i)) ? "touchstart" : "click";

	$("._toolbarCategory").bind(event, function(e) {
		//do shit here
		var visibility = $(this).children('ul').css('display');
		if (visibility == 'block') {
			$(this).children('ul').css('display', 'none');
		} else {
			$(this).children('ul').css('display', 'block');
		}
	});

	
}

function registerToolCategory(cat) {
	$('#toolbar').append('<div class="_toolbarCategory"><div class="toolbarLabel">'+cat['name']+'</div><ul class="toolbarCategory" id="'+cat['name']+'" style="display:none;"></ul></div>');
}

function registerTool(cat, tool) {
	//alert(tool['name']);
	$('#'+cat['name']).append('<li class="tool" id="tool_'+tool['id']+'"><img src="images/'+tool['src']+'" /></li>');
	$('#tool_'+tool['id']).click(function() {
		addObject(cat, tool);
	});
}

/*
 * Adds an object to the avatar
 * An object may be one or more symptoms or any accessory
 * Objects may be removed by clicking the X button that appears on hover
 */
function addObject(cat, tool) {
	clearObjects(cat);
	var zIndex = tool['zindex'];
	
	//Close button
	$('#container').append('<div class="object" id="obj_'+tool["id"]+'" style="z-index:'+zIndex+' !important"><div id="close_'+tool["id"]+'" class="close-btn"><img src="images/close-circled.png"></div><img id="img_'+tool['id']+'" src="images/'+tool["src"]+'"></div>');
	$('#obj_'+tool['id']).css('top', tool['y']+'px');
	$('#obj_'+tool['id']).css('left', tool['x']+'px');
	$('#obj_'+tool['id']).css('z-index',tool['zindex']);	
	
	//register in global array
	objects.push(tool);
	
	$('#obj_'+tool["id"]).mouseover(function() {
		$('#close_'+tool["id"]).addClass('showclose');
	});
	
	$('#obj_'+tool["id"]).mouseout(function() {	
		$('#close_'+tool["id"]).removeClass('showclose');
	});
	
	//Remove object when close button is clicked
	$('#obj_'+tool["id"]).click(function(){
		
		//this div's id
		var tool_id = tool["id"];;
		
		//remove from symptom_icons array
		remove_object(tool_id);
	});
	
	
}

function addObjectInOrder(obj, tool) {
	//first remove from DOM
	$('#obj_'+tool['id']).remove();
	$('#container').append('<div class="object" id="obj_'+tool["id"]+'"><div id="close_'+tool["id"]+'" class="close-btn"><img src="images/close-circled.png"></div><img id="img_'+tool['id']+'" src="images/'+tool["src"]+'"></div>');
	$('#obj_'+tool['id']).css('top', tool['y']+'px');
	$('#obj_'+tool['id']).css('left', tool['x']+'px');
	
}

function sortByZIndex(objects) {
	//sort objects by z-index
	objects.sort(function(a, b) {
   		return a.zindex - b.zindex;
	});
}

function remove_object(tool_id) {
	//remove this object (tool) from the global objects array
	for (var i = 0; i < objects.length; i++) {
		if (objects[i]['id'] == tool_id) {
			objects.splice(i, 1);
		}
	}
	
	//remove from DOM
	$('#obj_'+tool_id).remove();
}

function clearObjects(cat) {
	
	//remove from DOM
	for (var i = 0; i< cat.length; i++){
		if ($('#obj_'+cat[i]['id']).length) {
			$('#obj_'+cat[i]['id']).remove();
		}
	}
	
	//remove from objects array
	for (var i = 0; i< objects.length; i++){
		if (objects[i]['category'] == cat['name']) {
			objects.splice(i, 1);
		}
	
	}
}

function initMerge() {
	$('#merge').click(function() {
		mergeCanvas();
	});
}
 
function initSymptoms() {
	for (var i = 0; i< symptoms.length; i++){
		var name = symptoms[i]['name'];
		registerSymptomCategory(symptoms[i]);
			for (var j = 0; j< symptoms[i].length; j++){
				//alert(symptoms[i][j]['name']);
				//alert(symptoms[i].length);
				registerSymptom(symptoms[i], symptoms[i][j]);
			}
	}

	$('._symptomCategory').mouseover(function() {
		$(this).children('ul').css('display', 'block');
	});

	$('._symptomCategory').mouseout(function() {
		$(this).children('ul').css('display', 'none');
	});
	
	var ua = navigator.userAgent,
    event = (ua.match(/iPad/i)) ? "touchstart" : "click";

	$("._symptomCategory").bind(event, function(e) {
		//do shit here
		var visibility = $(this).children('ul').css('display');
		if (visibility == 'block') {
			$(this).children('ul').css('display', 'none');
		} else {
			$(this).children('ul').css('display', 'block');
		}
	});
	
	
	//any object of class "symptoms" is draggable
	$(".symptoms").draggable({ snap: ".droppable", snapMode: "inner", revert:"invalid", appendTo:"body",
	 helper: function( event ) {
		var icon = this.src;
		var dragged = this.id;
		return $( '<img src="'+icon+'" id="'+dragged+'" />' );
	},
	start: function(ui) {
		// do nothing to start
	
	}});
	$(".droppable").droppable({drop:function(event, ui) {
			var dragged = ui.helper[0].id;			
			fillSymptom(dragged, event['target']['id']);
			$(ui.helper).remove(); //destroy clone
		}
	});
}

function registerSymptomCategory(cat) {
	$('#symptoms').append('<div class="_symptomCategory"><div class="symptomLabel">'+cat['name']+'</div><ul class="symptomCategory" id="'+cat['id']+'" style="display:none;"></ul></div>');
}

function registerSymptom(cat, symptom) {
		$('#'+cat['id']).append('<li class="symptom" id="symptom_'+symptom['id']+'"><img src="images/'+symptom['src']+'" class="symptoms" id="'+symptom['id']+'" /></li>');
	$('#symptom_'+symptom['id']).click(function() {	
	
	});

}

function fillSymptom(dragged_id, target_id) {
	
	//increment the object counter
	objcount++;
	
	var symptom = get_symptom_by_id(dragged_id);
	
	var img_src = symptom['src'];
	//clear existing target of other items
	$('#'+target_id).empty();
	
	//display img where it is supposed to go
	$('#'+target_id).append('<div id="obj_'+objcount+'" class="objlayer"><div id="close_'+objcount+'" class="close-btn obj"><img src="images/close-circled.png"></div><img src="images/'+img_src+'" id="'+symptom['id']+'" /></div>');
	
	$('.objlayer').mouseover(function(){
		$(this).find('>:first-child').addClass('showclose'); 
		//on mouseover, show the close button
	});
	
	$('.objlayer').mouseout(function(){
		$(this).find('>:first-child').removeClass('showclose'); 
		//on mouseover, show the close button
	});
	
	$('#obj_'+objcount).click(function(){
		//on click, remove object
		
		//this div's id
		var layer_id = this.id;
		
		//remove from symptom_icons array
		remove_symptom(layer_id);
	});
	
	//get xy pos
	var xy = get_position_xy(target_id);
	var x = xy[0];
	var y = xy[1];
	
	//clear list of existing items in that spot
	if (symptom_icons.length) {
		for (var i = 0; i < symptom_icons.length; i++) {
			if (symptom_icons[i]['x'] == x && symptom_icons[i]['y'] == y) {
				symptom_icons.splice(i,1);
				
			}
		}
	}
	
	var selection = new Array();
	selection['id'] = symptom['id'];
	selection['x'] = x;
	selection['y'] = y;
	selection['layer_id'] = 'obj_'+objcount;
	symptom_icons.push(selection);
	
}

function remove_symptom(layer_id) {
	//remove from dom
	$('#'+layer_id).remove();
	
	//remove from global symptoms array
	for (var i = 0; i < symptom_icons.length; i++) {
		if (symptom_icons[i]['layer_id'] == layer_id) {
			symptom_icons.splice(i,1);		
		}
	}
}

/*
 * User may only drop an item onto select spots.  These spots are indexed here along with their x, y co-ordinates
 */
function get_position_xy(position) {
	var xy = new Array();
	var x = 0;
	var y = 0;
	
	switch (position) {
		case 'droppable1': 
			x = 10;
			y = 0;
			break;
		case 'droppable2': 
			x = 0;
			y = 95;
			break;
		case 'droppable3':
		 	x = 0;
			y = 190;
			break;
		case 'droppable4': 
			x = 0;
			y = 285;
			break;
		case 'droppable5': 
			x = 0;
			y = 380;
			break;
		case 'droppable6':
			x = 120;
			y = 325; 
			break;
		case 'droppable7':
			x = 120;
			y = 390; 
			break;
		case 'droppable8': 
			x = 120;
			y = 455;
			break;
		case 'droppable9': 
			x = 150;
			y = 520;
			break;
		case 'droppable11': 
			x = 250;
			y = 0;
			break;
		case 'droppable12': 
			x = 270;
			y = 95;
			break;
		case 'droppable13':
		 	x = 280;
			y = 190;
			break;
		case 'droppable14': 
			x = 270;
			y = 285;
			break;
		case 'droppable15': 
			x = 250;
			y = 380;
			break;
		case 'droppable16':
			x = 430;
			y = 325; 
			break;
		case 'droppable17':
			x = 430;
			y = 390; 
			break;
		case 'droppable18': 
			x = 430;
			y = 455;
			break;
		case 'droppable19': 
			x = 430;
			y = 520;
			break;
	}
	xy.push(x);
	xy.push(y);
	
	return xy;
}

function get_symptom_by_id(list_id) {	
	for (var i=0; i< symptoms.length; i++) {
		for (var j=0; j < symptoms[i].length; j++) {
			if (symptoms[i][j]['id'] == list_id) {
				return symptoms[i][j];
			}
		}
	}
}

function get_symptom_by_src(img_src) {
	//alert(list_id);
	for (var i=0; i< symptoms.length; i++) {
		if (symptoms[i]['src'] == img_src) {
			return symptoms[i];
		}
	}
}

function openSelector(sid) {	
	var id = 'selector_'+sid.replace(/\D/g,'');
	
	//close any that may still be open
	$('.selector').css('display','none');
	
	$('#'+id).fadeIn();
	$('#'+id).mouseout(function() {
		$('#'+id).css('display', 'none');
	});
}

/*
 * Extracts PNG data from canvas and merges it into a saveable image
 */
 
function mergeCanvas() {
	//alert('merge canvas running');
	var c=document.getElementById("canvas");
	var ctx=c.getContext("2d");
	var img=document.getElementById("figure");
	ctx.drawImage(img,0,0);
	
	//sort objects by z-index (to fix layering problem)
	objects.sort(function(a, b) {
   		return a.zindex - b.zindex;
	});
	
	
	//loop thru objects
	for (var i = 0; i< objects.length; i++){
		var img=document.getElementById("img_"+objects[i]['id']);
		var x = objects[i]['x'];
		var y = objects[i]['y'];
		
		ctx.drawImage(img,x,y);
	}
	
	//loop thru symptoms
	for (var i = 0; i< symptom_icons.length; i++){
		var img=document.getElementById(symptom_icons[i]['id']);
		var x = symptom_icons[i]['x'];
		var y = symptom_icons[i]['y'];
		
		ctx.drawImage(img,x,y);
	}
	
	  // pull out png data url from the canvas
	  var pngUrl = canvas.toDataURL('image/jpeg');
	  
	  //append to urldata for saving image file
	  $('#pngdata').append(pngUrl);
	  $('#sample').attr('src', pngUrl);
	  
	  $('#urldata').val(pngUrl);
	  return true;
	
}


