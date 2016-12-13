var sgd_pathname = window.location.pathname;
var slash_idx = sgd_pathname.lastIndexOf('/');
sgd_pathname = sgd_pathname.substr(slash_idx+1);
slash_idx = sgd_pathname.lastIndexOf('.');
sgd_pathname = sgd_pathname.substr(0,slash_idx);

function sgd_load() {
    filterSystems();
    return true;
}
function sgd_unload() {
    return true;
}

// Every time something is typed into the filter column,
// this method is called to filter. 
function filterSystems() {
    var systable = document.getElementById("system_table");

    // check the filter row.  If there is a value, filter on load.
    var target_row = systable.rows[1];
    
    var cells = target_row.cells;
    var elements=[];
    for (var cidx = 0; cidx < cells.length; cidx++) {
        var cell_elem = cells[cidx];
        
        if (cell_elem.children[0].value != "") {
            elements.push(cell_elem.children[0]);
            //filterSystems(cell_elem.children[0]);
        } else {
            // save the value in the browser cookie
            document.cookie="sgd_system_filter_" + sgd_pathname + "_" + cidx + "=;"
        }
    }
    // if none of the filter is set, still need to add one column to reset
    // if the user hit a backspace to remove everything.
    if (elements.length == 0) {
        elements.push(cells[0].children[0]);
    }
    filter(elements);
}

// Go through each rows of data, and if the data doesn't match with
// the filter (case insensitive), hide the row.  
function filter(elems) {
    var systable = document.getElementById("system_table");

    for (var idx = 2; idx < systable.rows.length; idx++) {
        var row_visibility = 'table-row';
        var target_row = systable.rows[idx];
        var cells = target_row.cells;
        for (var i = 0; i < elems.length; i++) {
            var elem = elems[i];
            if (elem.className == "sgd_filter_ed_tx") {
                var column_idx = elem.parentNode.cellIndex;
                var cell_elem = cells[column_idx];
                var cell_value = cell_elem.textContent
                var match = -1;
                if (elem.value != null && cell_value != null) {
                    var trim_cell_value = cell_value.replace(/ /g, '');
                    document.cookie="sgd_system_filter_"  + sgd_pathname + "_" + column_idx + "=" + elem.value;
                    if (elem.value === '!') {
                        // ----- Check for empty data or values
                        if (trim_cell_value == '') {
                            match = 1;
                        }
                    } else if (elem.value === '?') {
                        // ----- Check for non-empty data or values
                        if (trim_cell_value != '') {
                            match = 1;
                        }
                    } else {
                        // ----- The following handle matching text (case in-sensitive) 
                        // ----- and elements one and two levels below the table cell
                        match = cell_value.toLowerCase().indexOf(elem.value.toLowerCase());
                    }
                } 
                if (match == -1) {
                    row_visibility = 'none';
                } else {
                    if (row_visibility != 'none') {
                        row_visibility = 'table-row';
                    }
                } 
            }
            target_row.style.display = row_visibility;
        }
    }
}

// even handler for the click to sort.
function displaySortMenu(event, column, page) {
    var sort_menu_div = document.getElementById('sort_action_menu');
    var sort_html = createSortMenu(column, page);
    if (sort_menu_div != null) {
        sort_menu_div.innerHTML = sort_html;
        // ----- Display the current action menu
        var display = 'block';
        if (sort_menu_div.style.display == 'block') {
            display = 'none';
        } else {
            sort_menu_div.style.top=event.layerY + 8;
            sort_menu_div.style.left=event.layerX - 115;
        }
        sort_menu_div.style.display = display;
    }

}

// create the actual html for the sorting pop up menu.
function createSortMenu(column, page) {

    var sort_menu_items = [
        ['ms_sort_asc', 'Sort Ascending'],
        ['ms_sort_desc', 'Sort Descending'],
        ['ms_addsort_asc', 'Add Ascending Sort'],
        ['ms_addsort_desc', 'Add Descending Sort'],
    ];

    var html = '';
    var aclass = 'ftc_action_li';
    html += '<ul class="ftc_action_top_ul">';
    for (var idx = 0; idx < sort_menu_items.length; idx++) {
        var key = sort_menu_items[idx][0];
        var desc = sort_menu_items[idx][1];
        html += '<li class="' + aclass + '" id="' + key + '">';
        html += '<a href="javascript:sortColumn(\'' + key + '\',\'' + column + '\',\'' + page + '\')">';
        html += desc;
        html += '</a>';
        html += '</li>';
    }
    html += '</ul>';

    return html;
}

// submit the action to sort the columns.
function sortColumn(key, column, page) {
    var path = window.location.pathname;
    var buri = document.baseURI;
    var tkns = buri.split(/\?/);
    var urlpage = tkns[0] + '?';   
    var current_sort = '';
    var current_desc = '';
    if (tkns.length > 1) {
        var sortval = tkns[1];
        var sorts = sortval.split("&");
        for (var i=0; i<sorts.length; i++) {
            var itm = sorts[i].split("=");
            if (itm[0] == 'sort') {
                current_sort = sorts[i];
            } else if (itm[0] == 'desc') {
                current_desc = sorts[i];
            }
        }
    }
    // set the default sorting.
    if (current_sort=='') {
        if (page=='system') {
            current_sort='sort=HW_NAME';
            current_desc='desc=0';
        } else {
            current_sort='sort=HW_NAME;HW_CONTROL_NAME';
            current_desc='desc=0;0';
        }
    }
    if (key == 'ms_sort_asc') {
        window.location=urlpage+"sort="+column+"&desc=0";
    } else if (key == 'ms_sort_desc') {
        window.location=urlpage+"sort="+column+"&desc=1";
    } else if (key == 'ms_addsort_asc') {
        window.location=urlpage+current_sort+";"+column+"&"+current_desc+";0";
    } else if (key == 'ms_addsort_desc') {
        window.location=urlpage+current_sort+";"+column+"&"+current_desc+";1";
    } 
    var sort_menu_div = document.getElementById('sort_action_menu');
   
    if (sort_menu_div != null) {
        sort_menu_div.style.display = 'none';
    }
}
