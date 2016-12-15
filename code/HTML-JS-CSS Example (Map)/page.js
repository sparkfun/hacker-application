/** Code not for re-use. Example code only with permission from the University of Kansas **/

/**
This script allows the user to mark out a polygon on a image.
This is part of a utility to create a app-compatible navigation interface on Android and iOS.
**/

var canvasWidth = 500;
var canvasHeight = 500;

var maxDimension = 1000;

var baseCanvas, baseCtx,//Core canvas, few redraws
		editCanvas, editCtx;//Edit room canvas, lots of redraws

//Point snapping tolerance
var snapTolerance = 5;

//Room line width
var lineWidth = 4;

//Colors
var roomEditColor = '#ff00ff';
var roomDispColor = '#00cc00';

//Shift key status
var shifted = false;

jQuery( function() {

	//Build room list
	for( var i = 0; i < rooms.length; i++ ) {

		modifyRoomDisplayList( rooms[ i ] );
	}

	//Bind room click event listeners
	jQuery( document ).on( 'click', '#room-list .room-item', function() {

		//Get room id
		var id = jQuery( this ).attr( 'id' ).replace( /^room-/, '' );

		//Loop through all rooms, try to find specific id
		for( var i = 0; i < rooms.length; i++ ) {

			if( rooms[ i ][ 'id' ] == id ) {
				//Id found, cycle room edit mode for this index

				mapEndEdit();
				mapStartEdit( i );

				//Exit loop
				break;
			}
		}
	} );

	//Bind room rename click listeners
	jQuery( document ).on( 'click', '#room-list .room-item .room-title', function( event ) {

		event.stopPropagation();

		var node = jQuery( this ).parents( '.room-item' );

		var id = node.attr( 'id' ).replace( /^room-/, '' );

		//Loop through all rooms, try to find specific id
		for( var i = 0; i < rooms.length; i++ ) {

			if( rooms[ i ][ 'id' ] == id ) {
				//Id found, cycle room edit mode for this index

				showRoomNameModal( i, rooms[ i ][ 'id' ], rooms[ i ][ 'name' ], rooms[ i ][ 'identifier' ] );

				//Exit loop
				break;
			}
		}
	} );

	//Bind room delete click event listeners
	jQuery( document ).on( 'click', '#room-list .room-item .delete-button', function( event ) {

		event.stopPropagation();

		var node = jQuery( this ).parents( '.room-item' );

		if( confirm( "Delete " + node.find( '.room-title' ).text() + "?" ) ) {
			//Confirm deletion

			var id = node.attr( 'id' ).replace( /^room-/, '' );

			//If node is beind edited, end edit mode
			if( jQuery( editCanvas ).data( 'room-id' ) == id ) {

				mapEndEdit();
			}

			//Find item by item and remove it
			for( var i = 0; i < rooms.length; i++ ) {

				if( rooms[ i ][ 'id' ] == id ) {

					rooms.splice( i, 1 );
					break;
				}
			}

			//remove node from page
			node.remove();

			renderAllRooms();

			var saveDisplay = jQuery( '.save-display' );

			saveDisplay
					.find( '.unsaved' ).show()
					.siblings().hide();

			saveDisplay
					.removeClass( 'alert-success' ).removeClass( 'alert-danger' ).addClass( 'alert-warning' ).fadeIn();
		}
	} );

	//Bind room reset point array
	jQuery( document ).on( 'click', '#room-list .room-item .reset-button', function( event ) {

		event.stopPropagation();

		var node = jQuery( this ).parents( '.room-item' );

		if( confirm( "Reset points for " + node.find( '.room-title' ).text() + "?" ) ) {
			//Confirm deletion

			//Get room id
			var id = node.attr( 'id' ).replace( /^room-/, '' );

			//Loop through all rooms, try to find specific id
			for( var i = 0; i < rooms.length; i++ ) {

				if( rooms[ i ][ 'id' ] == id ) {
					//Id found

					//Reset point array
					while( rooms[ i ][ 'points' ].length > 0 ) {

						rooms[ i ][ 'points' ].pop();
					}

					//cycle room edit mode for this index
					mapEndEdit();
					mapStartEdit( i );

					//Exit loop
					break;
				}
			}
		}
	} );

	//Bind room end edit
	jQuery( document ).on( 'click', '#room-list .room-item .close-button', function( event ) {

		event.stopPropagation();

		mapEndEdit();
	} );

	//Setup base canvas
	baseCanvas = document.getElementById( 'base-canvas' );
	baseCanvas.width = canvasWidth;
	baseCanvas.height = canvasHeight;

	//Setup edit canvas
	editCanvas = document.getElementById( 'edit-canvas' );
	editCanvas.width = canvasWidth;
	editCanvas.height = canvasHeight;

	//Force wrapper to have a width equal to canvas size (plus border)
	jQuery( '.canvas-wrapper' ).css( { 'width': ( canvasWidth + 2 ) + 'px', 'height': ( canvasHeight + 2 ) + 'px' } );

	//Make sure canvas is usable
	if( baseCanvas && baseCanvas.getContext && editCanvas && editCanvas.getContext ) {

		baseCtx = baseCanvas.getContext( '2d' );
		editCtx = editCanvas.getContext( '2d' );

		//Build map canvas
		renderMapCanvasImage();
	} else {

		alert( 'Unable to use canvas. Please try later or try another browser.' );
	}
} );

/**
 * Renders the map's image to the base level canvas
 */
function renderMapCanvasImage() {

	//Setup map canvas
	var mapCanvas = document.getElementById( 'map-canvas' );
	mapCanvas.width = canvasWidth;
	mapCanvas.height = canvasHeight;

	//Make sure canvas is usable
	if( mapCanvas && mapCanvas.getContext ) {

		var mapCtx = mapCanvas.getContext( '2d' );

		//Create image element (should only occur once)
		var img = new Image();

		//Add post load listener (called after setting image path)
		img.addEventListener( 'load', function() {

			if( img.height > maxDimension || img.width > maxDimension ) {

				var widthFactor = maxDimension / img.width;
				var heightFactor = maxDimension / img.height;

				canvasWidth = maxDimension;
				canvasHeight = maxDimension;

				if( heightFactor < widthFactor ) {

					canvasWidth = maxDimension / ( img.height / img.width );
				} else if( widthFactor < heightFactor ) {

					canvasHeight = maxDimension / ( img.width / img.height );
				}
			} else {

				canvasWidth = img.width;
				canvasHeight = img.height;
			}

			// Adjust canvas sizing
			mapCanvas.width = editCanvas.width = baseCanvas.width = canvasWidth;
			mapCanvas.height = editCanvas.height = baseCanvas.height = canvasHeight;

			//Force wrapper to have a width equal to canvas size (plus border)
			jQuery( '#canvas-wrapper' ).css( { 'width': ( canvasWidth + 2 ) + 'px', 'height': ( canvasHeight + 2 ) + 'px' } );

			//Draw image
			mapCtx.drawImage( img, 0, 0, canvasWidth, canvasHeight );

			finishBuildingUI();
		}, false );

		//Set image path
		img.src = asset.map.WebPath;
	}
}

/**
 * Build UI after image is finished loading.
 */
function finishBuildingUI() {

	//Display mapping visual data
	renderAllRooms();

	jQuery( '#add-new-room' ).click( function() {

		showRoomNameModal( -1, 'new-' + rooms.length + Date.now(), '' );
	} );

	jQuery( '#set-room-name-button' ).click( saveRoomName );

	jQuery( '#room-name-input, #room-identifier-input' ).keyup( function( event ) {

		//Enter key pressed
		if( event.keyCode == 13 ) {

			saveRoomName();
		}
	} );

	//Listen for click in room or on page on main canvas
	jQuery( baseCanvas ).click( function( event ) {

		//console.log( event.offsetX, event.offsetY );
	} );

	//Listen for shift key. Forces X or Y axis snapping.
	jQuery( document ).on( 'keyup keydown', function( event ) {

		shifted = event.shiftKey;
	} );

	//Listen for edit point clicks
	jQuery( editCanvas ).click( function( event ) {

		mapMarkPoint( event.offsetX, event.offsetY );
	} );

	//Listen for save button click
	var saveButton = jQuery( '#save-btn' );

	saveButton.parents( '.navbar-fixed-bottom' ).show();
	saveButton.click( function() {

		var saveDisplay = jQuery( '.save-display' );

		saveDisplay
				.find( '.save-in-progress' ).show()
				.siblings().hide();

		saveDisplay
				.removeClass( 'alert-success' ).removeClass( 'alert-danger' ).addClass( 'alert-warning' ).fadeIn();

		jQuery.post(
				path.map_save,
				{ 'rooms': rooms },
				function( data ) {

					saveDisplay
							.find( '.save-complete' ).show()
							.siblings().hide();

					//console.log( data );

					if( data[ 'success' ] ) {

						//console.log( data );

						//Update room IDs. O( n^2 )
						for( var j = 0; j < data[ 'id_changes' ].length; j++ ) {

							//Loop through all rooms, try to find specific id
							for( var i = 0; i < rooms.length; i++ ) {

								if( rooms[ i ][ 'id' ] == data[ 'id_changes' ][ j ][ 'old' ] ) {
									//Id found, update

									//Update variables
									rooms[ i ][ 'id' ] = data[ 'id_changes' ][ j ][ 'new' ];

									//Update HTML
									jQuery( '#room-' + data[ 'id_changes' ][ j ][ 'old' ] ).attr( 'id', 'room-' + rooms[ i ][ 'id' ] );

									//Exit top loop
									break;
								}
							}
						}

						saveDisplay.find( '.save-complete' ).text( 'Rooms saved.' );

						saveDisplay
								.toggleClass( 'alert-success alert-warning', 500, 'easeOutSine' ).delay( 2500 ).fadeOut();
					} else {

						saveDisplay.find( '.save-complete' ).text( 'Save failed.' );

						saveDisplay
								.toggleClass( 'alert-danger alert-warning', 500, 'easeOutSine' );
					}
				}
		);
	} );
}

/**
 * Displays the modal for naming a room
 *
 * @param index
 * @param id
 * @param name
 * @param identifier
 */
function showRoomNameModal( index, id, name, identifier ) {

	//Set values for new room
	jQuery( '#room-index-input' ).val( index );
	jQuery( '#room-id-input' ).val( id );
	jQuery( '#room-name-input' ).val( name );
	jQuery( '#room-identifier-input' ).val( identifier );

	//Hide error messages
	jQuery( '#room-name-input-group, #room-identifier-input-group' ).removeClass( 'has-error' )
			.find( '.control-label' ).removeClass( 'in' );

	//Show modal
	jQuery( '#room-name-modal' ).modal( 'show' );

	//Focus on input
	setTimeout( function() {
		jQuery( '#room-name-input' ).focus();
	}, 500 );
}

/**
 * Uses data from #room-name-modal to add/update name of a room
 */
function saveRoomName() {

	var nameInput = jQuery( '#room-name-input' );
	var identifierInput = jQuery( '#room-identifier-input' );

	if( nameInput.val() === null || nameInput.val().length <= 0 ) {
		//Invalid name, show error

		jQuery( '#room-name-input-group' ).addClass( 'has-error' )
				.find( '.control-label' ).addClass( 'in' );

		return;
	} else if( identifierInput.val() === null || identifierInput.val().length <= 0 ) {
		//Invalid name, show error

		jQuery( '#room-identifier-input-group' ).addClass( 'has-error' )
				.find( '.control-label' ).addClass( 'in' );

		return;
	}

	var roomIndex = rooms.length;

	var indexInput = jQuery( '#room-index-input' );
	var idInput = jQuery( '#room-id-input' );

	if( indexInput.val() < 0 ) {
		//Add room to list

		rooms.push( {
			'id': idInput.val(),//Dummy ID. Replaced on saving
			'name': nameInput.val(),
			'identifier': identifierInput.val(),
			'points': []
		} );

		modifyRoomDisplayList( rooms[ roomIndex ] );

		//Proceed to go to edit mode
		mapEndEdit();
		mapStartEdit( roomIndex );
	} else if( rooms[ indexInput.val() ][ 'id' ] == idInput.val() ) {
		//Edit existing room name (if id matches)

		roomIndex = indexInput.val();

		rooms[ roomIndex ][ 'name' ] = nameInput.val();
		rooms[ roomIndex ][ 'identifier' ] = identifierInput.val();

		jQuery( '#room-' + idInput.val() + ' .room-title' ).html( rooms[ roomIndex ][ 'name' ] + "<br />" + "<small><em>" + rooms[ roomIndex ][ 'identifier' ] + "</em></small>" )
	}

	//Wipe modal values
	indexInput.val( -1 );
	idInput.val( '' );
	nameInput.val( '' );
	identifierInput.val( '' );

	//Hide modal
	jQuery( '#room-name-modal' ).modal( 'hide' );

	var saveDisplay = jQuery( '.save-display' );

	saveDisplay
			.find( '.unsaved' ).show()
			.siblings().hide();

	saveDisplay
			.removeClass( 'alert-success' ).removeClass( 'alert-danger' ).addClass( 'alert-warning' ).fadeIn();
}

/**
 * Add/Modify a room on the visible room list.
 *
 * @param room
 */
function modifyRoomDisplayList( room ) {

	if( room === null ) {
		//Not in bounds

		return;
	}

	var roomListContainer = jQuery( '#room-list' );

	if( roomListContainer.find( '#room-' + room[ 'id' ] ).length <= 0 ) {

		//Create new room node
		var node = jQuery( "<div id='room-" + room[ 'id' ] + "' class='list-group-item room-item'></div>" );

		//Add title and delete elements
		node
				.append( "<button class='btn btn-default btn-xs room-title'></button>" )
				.append( "<button class='pull-right btn btn-xs btn-danger delete-button fa fa-minus'></button>" );

		//Create interaction layer
		var interactionNode = jQuery( "<div />" )
				.addClass( 'margin-half-top interaction-wrapper row' )
				.append( "<button class='col-xs-4 btn btn-xs btn-info close-button'>Close</button>" )
				.append( "<button class='col-xs-4 btn btn-xs btn-warning reset-button'>Reset Points</button>" );

		//Append interaction layer
		node.append( interactionNode );

		//Adjust title
		node.find( '.room-title' ).html( room[ 'name' ] + "<br />" + "<small><em>" + room[ 'identifier' ] + "</em></small>" );

		//Insert into dom
		roomListContainer.append( node );
	} else {

		//Update title of existing
		roomListContainer.find( '#room-' + room[ 'id' ] + ' .room-title' ).text( room[ 'name' ] );
	}
}

/**
 * Clear the base context and draw everything
 */
function renderAllRooms() {

	//Clear
	baseCtx.clearRect( 0, 0, canvasWidth, canvasHeight );

	//Draw rooms
	for( var i = 0; i < rooms.length; i++ ) {

		drawRoomLines( rooms[ i ][ 'points' ], baseCtx, roomDispColor );
	}
}

/**
 * Sketch room's lines to given CTX
 *
 * @param pointArray    Array of room points to draw. Requires 2 or more points to create anything
 * @param ctx            Active context to draw to
 * @param color            Color to draw
 */
function drawRoomLines( pointArray, ctx, color ) {

	if( pointArray === null || pointArray.length <= 1 || ctx === null ) {
		//Each array element is a point, min functional length is 2 elements

		return;
	}

	if( color === null ) {
		//If color is null, set to a green

		color = roomDispColor;
	}

	//Prepare lines
	ctx.lineWidth = lineWidth * ( color == roomDispColor ? 1 : 2 );//Increase width for non-display modes
	ctx.strokeStyle = color;
	ctx.fillStyle = 'rgba( 0, 0, 0, 0.25 )';

	//Start pathing
	ctx.beginPath();
	ctx.moveTo( pointArray[ 0 ][ 'x' ], pointArray[ 0 ][ 'y' ] );

	for( var i = 1; i < pointArray.length; i++ ) {

		ctx.lineTo( pointArray[ i ][ 'x' ], pointArray[ i ][ 'y' ] );
	}

	//Mark lines to canvas
	ctx.closePath();
	ctx.stroke();
	ctx.fill();
}

/**
 * Compares two point objects for similarity of p2 to p1. Adjusts towards p1.
 *
 * @param p1.x
 * @param p1.y
 * @param p2.x
 * @param p2.y
 *
 * @return object
 */
function comparePoints( p1, p2 ) {

	var pr = {};

	if( Math.abs( p1[ 'x' ] - p2[ 'x' ] ) < snapTolerance ) {
		//shift X point back in line

		pr[ 'x' ] = p1[ 'x' ];
	} else {

		pr[ 'x' ] = p2[ 'x' ];
	}

	if( Math.abs( p1[ 'y' ] - p2[ 'y' ] ) < snapTolerance ) {
		//shift Y point back in line

		pr[ 'y' ] = p1[ 'y' ];
	} else {

		pr[ 'y' ] = p2[ 'y' ];
	}

	return pr;
}

/**
 * Begins edit mode for canvas
 *
 * @param index    Index of room object in rooms array
 */
function mapStartEdit( index ) {

	if( index < 0 ) {
		//No room set, abort

		mapEndEdit();

		return;
	}

	//Clear base canvas
	baseCtx.clearRect( 0, 0, canvasWidth, canvasHeight );

	//Draw rooms EXCEPT currently active room
	for( var i = 0; i < rooms.length; i++ ) {

		if( i != index ) {

			drawRoomLines( rooms[ i ][ 'points' ], baseCtx, roomDispColor );
		}
	}

	//Clear edit drawing area
	editCtx.clearRect( 0, 0, canvasWidth, canvasHeight );

	jQuery( editCanvas )
		//Tie room id to canvas
			.data( 'room-id', rooms[ index ][ 'id' ] )
		//Tie room index to canvas
			.data( 'room-index', index )
		//Mark it as room point editing
			.data( 'point-edit', true )
		//Show edit canvas
			.show();

	//Draw room's lines onto edit canvass
	drawRoomLines( rooms[ index ][ 'points' ], editCtx, roomEditColor );

	//Show delete points button
	jQuery( '#room-' + rooms[ index ][ 'id' ] ).find( '.interaction-wrapper' ).show();
}

/**
 * Marks a point on the map canvas
 *
 * @param x
 * @param y
 */
function mapMarkPoint( x, y ) {

	if( jQuery( editCanvas ).data( 'room-id' ).length <= 0 ) {
		//No room in edit mode

		//Run exit edit mode
		mapEndEdit();

		//tell event handles that this did nothing
		return false;
	}

	//Track room index and new point index
	var index = jQuery( editCanvas ).data( 'room-index' );
	var pi = rooms[ index ][ 'points' ].length;

	//Save points
	rooms[ index ][ 'points' ].push( { 'x': ( x - 2 ), 'y': ( y - 2 ) } );

	//Draw line from last point in list (>2 items in point array)
	if( rooms[ index ][ 'points' ].length > 1 ) {

		if( shifted ) {
			//Snap to X or Y axis, whichever is greater

			if( Math.abs( rooms[ index ][ 'points' ][ pi ][ 'x' ] - rooms[ index ][ 'points' ][ pi - 1 ][ 'x' ] ) > Math.abs( rooms[ index ][ 'points' ][ pi ][ 'y' ] - rooms[ index ][ 'points' ][ pi - 1 ][ 'y' ] ) ) {
				//Snap to X

				rooms[ index ][ 'points' ][ pi ][ 'y' ] = rooms[ index ][ 'points' ][ pi - 1 ][ 'y' ];
			} else {
				//Snap to Y

				rooms[ index ][ 'points' ][ pi ][ 'x' ] = rooms[ index ][ 'points' ][ pi - 1 ][ 'x' ];
			}
		}

		//Adjust point compared to previous
		rooms[ index ][ 'points' ][ pi ] = comparePoints( rooms[ index ][ 'points' ][ pi - 1 ], rooms[ index ][ 'points' ][ pi ] );

		//Adjust point compared to origin
		rooms[ index ][ 'points' ][ pi ] = comparePoints( rooms[ index ][ 'points' ][ 0 ], rooms[ index ][ 'points' ][ pi ] );

		//Check for point matching (not allowed)
		if( rooms[ index ][ 'points' ][ pi - 1 ][ 'x' ] === rooms[ index ][ 'points' ][ pi ][ 'x' ] && rooms[ index ][ 'points' ][ pi - 1 ][ 'y' ] === rooms[ index ][ 'points' ][ pi ][ 'y' ] ) {
			//Remove point

			rooms[ index ][ 'points' ].pop();
			return;
		}

		//Prepare lines
		editCtx.lineWidth = lineWidth;
		editCtx.strokeStyle = roomEditColor;

		//Draw
		editCtx.beginPath();
		editCtx.moveTo( rooms[ index ][ 'points' ][ pi - 1 ][ 'x' ], rooms[ index ][ 'points' ][ pi - 1 ][ 'y' ] );
		editCtx.lineTo( rooms[ index ][ 'points' ][ pi ][ 'x' ], rooms[ index ][ 'points' ][ pi ][ 'y' ] );
		editCtx.stroke();
	}

	//Big mark where click occured (with corrected points)
	editCtx.fillRect( rooms[ index ][ 'points' ][ pi ][ 'x' ], rooms[ index ][ 'points' ][ pi ][ 'y' ], 3, 3 );

	//If point === first point in list
	if( rooms[ index ][ 'points' ].length > 1 &&
	    rooms[ index ][ 'points' ][ 0 ][ 'x' ] === rooms[ index ][ 'points' ][ pi ][ 'x' ] &&
	    rooms[ index ][ 'points' ][ 0 ][ 'y' ] === rooms[ index ][ 'points' ][ pi ][ 'y' ] ) {

		//Pulse lines on edit canvas

		//Remove last point, same as first
		rooms[ index ][ 'points' ].pop();

		//Exit
		mapEndEdit();
	}
}

/**
 * Ends edit mode for canvas
 */
function mapEndEdit() {

	//Draw known mapping
	renderAllRooms();

	jQuery( editCanvas )
		//Unlink room id from canvas
			.data( 'room-id', "" )
		//Unlink room index from canvas
			.data( 'room-index', "" )
		//Mark it as room point editing
			.data( 'point-edit', false )
		//Hide edit canvas
			.hide();

	//Hide delete points button
	jQuery( '.interaction-wrapper' ).hide();

	var saveDisplay = jQuery( '.save-display' );

	saveDisplay
			.find( '.unsaved' ).show()
			.siblings().hide();

	saveDisplay
			.removeClass( 'alert-success' ).removeClass( 'alert-danger' ).addClass( 'alert-warning' ).fadeIn();
}