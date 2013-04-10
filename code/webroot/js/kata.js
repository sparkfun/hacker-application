
function getCookie(name) {
	var start = document.cookie.indexOf( name + "=" );
	var len = start + name.length + 1;
	if ((!start) && (name != document.cookie.substring(0, name.length))) {
		return null;
	}
	if (start == -1) return null;
	var end = document.cookie.indexOf(';',len);
	if (end == -1) end = document.cookie.length;
	return unescape(document.cookie.substring(len,end));
}


function setCookie(name, value, expires, path, domain, secure) {
	var today = new Date();
	today.setTime(today.getTime());
	if (expires) {
		expires = expires * 1000 * 60 * 60 * 24;
	}
	var expires_date = new Date(today.getTime() + (expires));
	document.cookie = name+'='+escape(value) +
		((expires)?';expires='+expires_date.toGMTString():'')+
		((path)?';path='+path:'') +
		((domain)?';domain='+domain:'') +
		((secure)?';secure':'');
}


Array.prototype.inArray = function (value) {
	var i;
	for (i=0; i < this.length; i++) {
		if (this[i] === value) {
			return true;
		}
	}
	return false;
};


function addOnLoad(func) {
	var oldonload = window.onload;
	if (typeof window.onload != 'function') {
		window.onload = func;
	}
	else {
		window.onload = function() {
			oldonload();
			func();
		}
	}
}


function toggle(node) {
	var el = getElement(node);
	if ( el.style.display != 'none' ) {
		el.style.display = 'none';
	} else {
		el.style.display = '';
	}
}


function setCheckboxes(formhandle,checked) {
   for (i=0;i<formhandle.elements.length;i++) {
       formhandle.elements[i].checked = checked;
   }
}
