$(document).ready(function () {
/* Item-to-Menu drag/drop */

  init_lists();

  $('button#add-section').click(function() {
    $('div#section-container').prepend($(document.createElement('div')).addClass('menu-section').append('<input type="text" id="section-name" placeholder="New Section"><input type="text" id="section-description" placeholder="Add a section description here"><button class="remove-section">Delete Section</button><ul class="section"></ul>'));
    init_lists();
  });


  $('div#section-container').on('click','button.remove-section',function(event){
    $(this).parent().remove();
  });
  $('button#item-edit-cancel').click(function() {
    $('body').css('overflow','auto');
    $('div#global-shadow, div#edit-item-popout').css('display','none');
    $('ul#applied-option-groups').children('li').remove();
    $('ul#applied-item-custom-options').children('li').remove();
    $('div#edit-item').children('div').remove();
  }); 
  $('button#item-edit-save').click(function() {
    save_item();  
  });
/* Within The Edit Item Menu, Add an Item Custom Option */
  $('button.item-customize-option-add').click(function() {
    $('ul#applied-item-custom-options').append('<li id="'
                +$(this).parent('li').attr('id')
                +'" class="item-customize-option" name="'
                +$(this).parent('li').attr('name')+'">'
                +$(this).parent('li').attr('name')
                +'<input type="checkbox" class="item-customize-option-selected" checked>'
                +'<button class="remove-item-custom-option">Remove</button></li>');
  });
  $('button.option-group-add').click(function() {
    $('ul#applied-option-groups').append('<li id="'
                    +$(this).parent('li').attr('id')
                    +'" class="option-group" name="'
                    +$(this).parent('li').attr('name')+'">'
                    +$(this).parent('li').attr('name')
                    +'<button class="remove-option-group">Remove</button></li>');
  });
  $('ul#applied-option-groups, ul#applied-item-custom-options').on('click','li button', function() {
    $(this).parent('li').remove();
  });
});

function fill_item_edit(item_id,item_name) {
  var restaurant_id = $('input.restaurant-id').attr('id');
  var getUrl = '/restaurants/'+restaurant_id+'/items/'+item_id+'/'; 
//  var item_name = $('li#'+String(item_id)).children('input.namechange').attr('value');
  $.getJSON(getUrl, function(data) {
    $('input#item-id').attr('value',item_id);
    $('input#item-name').attr('value',item_name);
    $('textarea#item-description').attr('value',data.description);
    $('input#available').prop('checked', data.available);
    $('input#alcohol').prop('checked', data.alcohol);
    $('input#buffer').prop('checked', data.buffer);
    for (var i in data.selected_option_groups) {
      var name = data.selected_option_groups[i]['name'];
      var id = data.selected_option_groups[i]['id'];
      $('ul#applied-option-groups').append(
            '<li class="option-group" id="'+id+'" name="'+name+'">'+name
              +'<button class="remove-option-group">Remove</button></li>');
    }
    for (var i in data.selected_item_customize_options) {
      var name = data.selected_item_customize_options[i]['name'];
      var id = data.selected_item_customize_options[i]['id'];
      var selected = data.selected_item_customize_options[i]['selected'];
      $('ul#applied-item-custom-options').append(
              '<li class="item-customize-option" id="'+id+'" name="'+name+'">'+name
              +'<input type="checkbox" class="item-customize-option-selected"'+
              (selected == true ? 'checked' : '')+'>'
              +'<button class="remove-item-custom-option">Remove</button></li>');
    }
  });
}

function save_item() {
    var restaurant_id = $('input.restaurant-id').attr('id');
    var item_id = $('input#item-id').attr('value');
    var postUrl = '/restaurants/'+restaurant_id+'/items/'+item_id+'/';
    selected_option_groups = [];
    $('ul#applied-option-groups li.option-group').each(function() {
      selected_option_groups.push({
        id : $(this).attr('id'), 
      });
    });
    item_customize_options = [];
    $('ul#applied-item-custom-options li.item-customize-option').each(function() {
      item_customize_options.push({ 
        id : $(this).attr('id'), 
        selected : $(this).children('input').prop('checked')
      });
    });
    var postdata = {
      id      : item_id,
      name    : $('input#item-name').attr('value'),
      description : $('textarea#item-description').attr('value'),
      available : $('input#available').prop('checked'),
        alcohol   : $('input#alcohol').prop('checked'),
        buffer    : $('input#buffer').prop('checked'),
      selected_option_groups : selected_option_groups,
      item_customize_options : item_customize_options,
      };
    // TODO: refactor this and cancel button click into one function
    $('body').css('overflow','auto');
    $('div#global-shadow, div#edit-item-popout').css('display','none');
    $('ul#applied-option-groups').children('li').remove();
    $('ul#applied-item-custom-options').children('li').remove();
    $('div#edit-item').children('div').remove();
    $.ajax({
      url: postUrl,
      type: 'POST',
      contentType: 'application/json; charset=utf-8',
      data: JSON.stringify(postdata),
      dataType: 'text',
      success: function(result) {
        if (result != "200") { alert(result) }
      }
    });
    $('li#'+String(item_id)+' input.namechange').attr('value',postdata['name']);
    $('li#'+String(item_id)+' span.itemname').text(postdata['name']);
}

function init_lists() {
  $('div#menu ul.section').sortable({
    opacity:0.6,
    axis:'y',
    revert: true,
    beforeStop: function(event, ui) {
      $(this).children('li.item').each(function() {
        if ($(this).find('button.item-edit').length == 0) {
          $(this).append($(document.createElement('button'))
            .addClass('item-remove').text('Remove'));
          $(this).append($(document.createElement('button'))
            .addClass('item-edit').text('Edit'));
        }
      });
    }
  });

  $('#itemlist li.item, #itemlist li.item input').draggable({
    connectToSortable:'div#menu ul.section',
    helper: "clone",
    revert: "invalid"
  }).disableSelection();

  $('div#section-container').sortable({
    opacity:0.6,
    axis:'y',
    revert: true,
    beforeStop: function(event, ui) {}
    
  }).disableSelection();

/* Draws a blue border around an item when clicked.
 * It gains the attribute 'highlighted="yes"
 * After a second click, the item's name is swapped with a textbox to become editable
 * clicking any other item deselects the previous one and updates the namechange
 */
  
  $('li.item').attr("highlighted","no"); 
  $('ul.section').on('click','li.item[highlighted="no"]',function(event) {
    var previous = $('li.item[highlighted="yes"]');
    previous.children('input.namechange').css("display","none");
    var text = previous.children('input.namechange').attr('value');
    previous.children('span.itemname').text(text);
    previous.children('span.itemname').attr('name',text);
    previous.children('span.itemname').css("display","inline");
    previous.css('border','solid 0.1em transparent');
    previous.attr("highlighted","no");
  });

  $('ul.section').on('click','li, li p, li a',function(event){
    $(this).css('border','solid 0.1em #00F');
    $(this).attr("highlighted","yes");
  });

  $('ul.section').on('click','li.item[highlighted="yes"]',function(event) { 
    $(this).children('span.itemname').css("display","none");
    $(this).children('input').css("display","inline");
    $(this).children('input').focus();
  });

/* Edit and Remove buttons for each item */

  $('ul.section').on('click','li.item button.item-remove',function(event){
    $(this).parent().remove();
  });

/* Editing an Individual Item */

  $('ul.section').on('click','li.item button.item-edit',function(event){
    var item_id = ($(this).parent('li').children('span.itemname').attr('id'));
    var name = ($(this).parent('li').children('input.namechange').attr('value'));
    $('div#edit-item button#item-edit-cancel').before(fill_item_edit(item_id,name));
    $('body').css("overflow","hidden");
    $('div#global-shadow, div#edit-item-popout').css('display','inline');
    event.stopPropagation();
  });
  
}
