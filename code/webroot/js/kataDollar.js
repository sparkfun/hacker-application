// $ function already exists? dont install. except IE. who does STRANGE things.

if (typeof($) != 'function') {
function $() {
	var elems = new Array();
	for (var i = 0; i < arguments.length; i++) {
		var el = arguments[i];
		if ('string' == typeof element) {
			el = false;
			if (document.getElementById)  {
				el = document.getElementById(name);
			} else if (document.all) {
				el = document.all[name];
			} else if (document.layers) {
				el = document.layers[name];
			}
		}
		if (arguments.length == 1) {
			return el;
		}
		elems.push(el);
	}
	return elems;
}
}

