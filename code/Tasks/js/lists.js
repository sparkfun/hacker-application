/*
* initialize()
*
* Called when index loads. Finishes constructing list items
* and adds interactive functionality.
*/
function initialize() {

    setListTabs();
    setEvents();
    addSortability();
	bindAllTabs("#list li span");	

};

/*
* setListTabs()
*
* Adds helper tabs to all list items
*/
function setListTabs() {
  // WRAP LIST TEXT IN A SPAN, AND APPLY FUNCTIONALITY TABS
  $("#list li")
    .wrapInner("<span>")
    .append("<div class='draggertab tab'></div>" +
    		"<div class='colortab tab'></div>" +
    		"<div class='deletetab tab'></div>" +
    		"<div class='donetab tab'></div>");

}

/*
* setEvents()
*
* Adds event listeners to helper tabs to set functionality
*/
function setEvents() {

    $( document ).on("click", ".donetab", function(e) {

	    if(!$(this).siblings('span').children('img.crossout').length) {
	        $(this)
	            .parent()
	            .find("span")
                .append("<img src='/images/crossout.png' class='crossout' />")
                .find(".crossout")
                .animate({
                    width: "100%"
                })
                .end()
	            .animate(
		            {
	            		opacity: "0.5"
	        		},
	        		"slow",
	        		"swing",
	       			toggleDone(e.currentTarget.id, 1)
	           	);
	    }
	    else
	    {
	        $(this)
	            .siblings('span')
	                .find('img.crossout')
	                    .remove()
	                    .end()
	                .animate({
	                    opacity : 1
	                },
	                "slow",
	                "swing",
	                toggleDone(e.currentTarget.id, 0));
	            
	    }
	});

	// COLOR CYCLING
    // Does AJAX save, but no visual feedback
    $( document ).on("click", ".colortab", function(){
        $(this).parent().nextColor();

        var id = $(this).parent().attr("id"),
            color = $(this).parent().attr("color");

        $.ajax({
            type: "POST",
            url: "db-interaction/lists.php",
            data: "action=color&id=" + id + "&color=" + color,
            success: function(msg) {
                // error message
            }
        });
    });

	// AJAX style deletion of list items
    $( document ).on("click", ".deletetab", function(){
        var thiscache = $(this),
            list = $('#current-list').val(),
            id = thiscache.parent().attr("id"),
            pos = thiscache.parents('li').attr('rel');

        if (thiscache.data("readyToDelete") == "go for it") {
            $.ajax({
                type: "POST",
                url: "db-interaction/lists.php",
                data: {
                        "list":list,
                        "id":id,
                        "action":"delete",
                        "pos":pos
                    },
                success: function(r){
                        var $li = $('#list').children('li'),
                            position = 0;
                        thiscache
                            .parent()
                                .hide("explode", 400, function(){$(this).remove()});
                        $('#list')
                            .children('li')
                                .not(thiscache.parent())
                                .each(function(){
                                        $(this).attr('rel',   position);
                                    });
                    },
                error: function() {
                    $("#main").prepend("Deleting the item failed...");
                }
            });
        }
        else
        {
            thiscache.animate({
                width: "44px",
                right: "-64px"
            }, 200)
            .data("readyToDelete", "go for it");
        }
    });

	 // AJAX style adding of list items
    $('#add-new').submit(function(e){

    	e.preventDefault();

        var forList = $("#current-list").val(),
            newListItemText = $("#new-list-item-text").val(),
            URLtext = escape(newListItemText),
            newListItemRel = $('#list li').size() + 1;

        if(newListItemText.length > 0) {
            $.ajax({
                type: "POST",
                url: "db-interaction/lists.php",
                data: "action=add&list=" + forList + "&text=" + URLtext + "&pos=" + newListItemRel,
                success: function(theResponse){
                  $("#list").append("<li color='1' class='color-blue' rel='" + newListItemRel + "' id='" 
                  					+ theResponse + "'><span id='" + theResponse + newListItemRel +
                  					" title='Click to edit...'>" + newListItemText 
                  					+ "</span><div class='draggertab tab'></div><div class='colortab tab'>"
                  					+ "</div><div class='deletetab tab'></div><div class='donetab tab'></div></li>");
                  bindAllTabs("#list li[rel='" + newListItemRel + "'] span");
                  $("#new-list-item-text").val("");
                },
                error: function(){
                    alert('NO WORKY');
                }
            });
        } else {
            $("#new-list-item-text").val("");
        }
    });
}

/*
* addSortability()
*
* Attaches sortability function to dragger tabs
*/
function addSortability() {

    // MAKE THE LIST SORTABLE VIA JQUERY UI
    // calls the SaveListOrder function after a change
    // waits for one second first, for the DOM to set, otherwise it's too fast.
    $("#list").sortable({
        handle   : ".draggertab",
        update   : function(event, ui){
            var id = ui.item.attr('id');
            var rel = ui.item.attr('rel');
            var t = setTimeout("saveListOrder('" + id + "', '" + rel + "')", 500);
        },
        forcePlaceholderSize: true
    });
}

/*
* bindAllTabs(editableTarget)
*
* @param editableTarget element
*
* Adds editability to list items 
*/
function bindAllTabs(editableTarget) {
    $(editableTarget).editable("db-interaction/lists.php", {
        id        : 'listItemID',
        indicator : 'Saving...',
        tooltip   : 'Double-click to edit...',
        event     : 'dblclick',
        submit    : 'Save',
        submitdata: {action : "update"}
    });
}

/*
* toggleDone(id, isDone)
*
* @param id string
* @param isDone boolean
*
* Called when saving list items
*/
function toggleDone(id, isDone) {
    $.ajax({
        type: "POST",
        url: "db-interaction/lists.php",
        data: "action=done&id=" + id + "&done=" + isDone
    })
}

/*
* saveListOrder(itemID, itemREL)
*
* @param itemID string
* @param itemREL int
*
* Called when listem items are sorted to update list order
*/
function saveListOrder(itemID, itemREL){
    var i = 1,
        currentListID = $('#current-list').val();
    $('#list li').each(function() {
        if($(this).attr('id') == itemID) {
            var startPos = itemREL,
                currentPos = i;
            if(startPos < currentPos) {
                var direction = 'down';
            } else {
                var direction = 'up';
            }
            var postURL = "action=sort&currentListID=" + currentListID +
                 "&startPos=" + startPos +
                 "&currentPos=" + currentPos +
                 "&direction=" + direction;

            $.ajax({
                type: "POST",
                url: "db-interaction/lists.php",
                data: postURL,
                success: function(msg) {
                    // Resets the rel attribute to reflect current positions
                    var count=1;
                    $('#list li').each(function() {
                        $(this).attr('rel', count);
                        count++;
                    });
                },
                error: function(msg) {
                    // error handling here
                }
            });
        }
        i  ;
    });
}

/*
* jQuery.fn.nextColor()
*
* Adding color change functionality to jquery
* so list item colors can be cycled
*/
jQuery.fn.nextColor = function() {

    var curColor = $(this).attr("class");

    if (curColor == "color-blue") {
        $(this).removeClass("color-blue").addClass("color-yellow").attr("color","2");
    } else if (curColor == "color-yellow") {
        $(this).removeClass("color-yellow").addClass("color-red").attr("color","3");
    } else if (curColor == "color-red") {
        $(this).removeClass("color-red").addClass("color-green").attr("color","4");
    } else {
        $(this).removeClass("color-green").addClass("color-blue").attr("color","1");
    };

};