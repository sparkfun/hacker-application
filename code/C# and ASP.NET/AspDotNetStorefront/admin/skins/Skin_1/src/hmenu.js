/**
 *  Dynarch Horizontal Menu, hmenu-2.4
 *  Copyright Dynarch.com, 2003-2005.  All Rights Reserved.
 *  http://dynarch.com/products/hmenu/
 *
 *  THIS NOTICE MUST REMAIN INTACT!
 *
 *           LICENSEE: Discovery Productions, Inc.
 *        License key: 4694276c07e315e8e82516d734a9cd33
 *      Purchase date: Thu Jan 13 17:19:43 2005 GMT
 *       License type: developer
 *
 *  For details on this license please visit
 *  the product homepage at the URL above.
 */

// set some global variables that will remember the browser type
(function (){
	var UA = navigator.userAgent, w = window;

	w.is_gecko = /gecko/i.test(UA);
	w.is_opera = /opera/i.test(UA);
	w.is_ie = /msie/i.test(UA) && !is_opera && !(/mac_powerpc/i.test(UA));
	w.is_ie5 = is_ie && /msie 5\.[^5]/i.test(UA);
	w.is_mac_ie = /msie.*mac/i.test(UA);
	w.is_khtml = /Konqueror|Safari|KHTML/i.test(navigator.userAgent);
	if (typeof w._dynarch_menu_url == "undefined")
		w._dynarch_menu_url = "/hmenu/";
	else
		w._dynarch_menu_url = w._dynarch_menu_url.replace(/\x2f*$/, '/');
	// preload some images (isn't IE totally FUCKED UP??)
	w._dynarch_menu_shadow = new Image();
	w._dynarch_menu_shadow.src = w._dynarch_menu_url + "src/img/shadow.png";
	w._dynarch_menu_ediv = "<div unselectable='on'>&nbsp;</div>";
})();

function DynarchMenu(el, config) {
	var T1, a, i;
	if (config.d_profile) {
		DynarchMenu.profile = {
			item : 0,
			tree : 0
		};
		T1 = (new Date()).getTime(); // DEBUG.PROFILE
	}
	this._baseMenuInfo = null;
	this._popupMenus = [];
	this._activeKeymap = null;
	this._globalKeymap = null;
	this._activePopup = null;
	this._fixed = false;
	this.items = {};
	this.target = null;
	this.config = config;
	try {
		this._df = document.createDocumentFragment();
	} catch(e) {
		this._df = null;
		this._ca = [];
	}
	el.parentNode.insertBefore(this.createMenuTree(el, !config.vertical), el);
	if (this._df) {
		window.self.document.body.appendChild(this._df);
		this._df = null;
	} else {
		a = this._ca;
		for (i = a.length; --i >= 0;)
			window.self.document.body.appendChild(a[i]);
	}
	if (el.parentNode)
		el.parentNode.removeChild(el);
	if (config.d_profile)
		alert("DynarchMenu: generated in " + (((new Date()).getTime() - T1) / 1000) + " sec.\n" +
		      "containing " + DynarchMenu.profile.item + " items in " +
		      DynarchMenu.profile.tree + " (sub)menus."); // DEBUG.PROFILE
};

DynarchMenu._hiderID = 0;
DynarchMenu._createHider = function() {
	var f = null;
	if (is_ie && !is_ie5) {
		var filter = 'filter:progid:DXImageTransform.Microsoft.alpha(style=0,opacity=0);';
		var id = 'dynarch-menu-hider-' + (++this._hiderID);
		window.self.document.body.insertAdjacentHTML
			('beforeEnd', '<iframe id="' + id + '" scroll="no" frameborder="0" ' +
			 'style="position:absolute;visibility:hidden;' + filter +
			 'border:0;top:0;left:0;width:0;height:0;" ' +
			 'src="javascript:false;"></iframe>');
		f = window.self.document.getElementById(id);
	}
	return f;
};

DynarchMenu._setupHider = function(f, x, y, w, h) {
	if (f) {
		var s = f.style;
		s.left = x + "px";
		s.top = y + "px";
		s.width = w + "px";
		s.height = h + "px";
		s.visibility = "visible";
	}
};

DynarchMenu._closeHider = function(f) {
	if (f)
		f.style.visibility = "hidden";
};

DynarchMenu._C = null;
DynarchMenu._T = null;
DynarchMenu._OT = null;
DynarchMenu._RE_PR = /(^|\s+)pressed(\s+|$)/ig;
DynarchMenu._RE_AH = /(^|\s+)active|hover(\s+|$)/ig;
DynarchMenu._RE_DS = /(^|\s+)disabled(\s+|$)/ig;
DynarchMenu._RE_CTX_ID = /context-for-(.*)/;
DynarchMenu._RE_CTX_CL = /context-class-([^-]+)-(.+)/;
DynarchMenu._activeItem = null;
DynarchMenu._menus = null;
DynarchMenu.setup = function(el, config) {
	if (typeof config == "undefined") config = {};
	function param_default(name, value) { if (typeof config[name] == "undefined") { config[name] = value; } };
	param_default("className", null);
	param_default("tooltips", false);
	param_default("shadows", [4, 4]);
	param_default("smoothShadow", true);
	param_default("dx", 0);
	param_default("dy", 0);
	param_default("basedx", 0);
	param_default("basedy", 0);
	param_default("timeout", 150);
	param_default("context", false);
	param_default("vertical", false);
	param_default("electric", config.vertical ? 250 : false);
	param_default("blink", false);
	param_default("lazy", false);
	param_default("d_profile", false);
	param_default("toolbar", false);
	if (typeof el == "string")
		el = window.self.document.getElementById(el);
	if (is_mac_ie)
		return null;		// we don't support it, period.
	if (is_ie5)
		config.smoothShadow = false;
	if (config.context)
		config.vertical = true;
	if (!el) {
		alert("Error: menu element not found.");
		return false;
	}
	el.style.display = "none";
	var i, els, a = DynarchMenu._menus, tmp, tmp2;
	if (!a) {
		a = DynarchMenu._menus = [];
		els = [window, window.self.document];
		tmp = document.getElementsByTagName("iframe");
		for (i = tmp.length; --i >= 0;) {
			tmp2 = tmp[i];
			tmp2 = is_opera ? tmp2 : (tmp2.contentWindow || tmp2.window);
			els[els.length] = tmp2;
		}
		for (i = els.length; --i >= 0;) {
			tmp = els[i];
			if (tmp) {
				DynarchMenu._addEvent(tmp, (is_ie || is_opera) ? "keydown" : "keypress", DynarchMenu._documentKeyPress);
				DynarchMenu._addEvent(tmp, "mousedown", DynarchMenu._documentMouseDown);
				DynarchMenu._addEvent(tmp, "mouseup", DynarchMenu._documentMouseUp);
				DynarchMenu._addEvent(tmp, "mouseover", DynarchMenu._documentMouseOver);
			}
		}
	}
	return a[a.length] = new DynarchMenu(el, config);
};

DynarchMenu._clearTimeout = function() {
	if (DynarchMenu._T) {
		clearTimeout(DynarchMenu._T);
		DynarchMenu._T = null;
	}
};

DynarchMenu._forAllMenus = function(callback) {
	for (var i = DynarchMenu._menus.length; --i >= 0 && !callback(DynarchMenu._menus[i]););
};

DynarchMenu._closeOtherMenus = function(menu) {
	DynarchMenu._forAllMenus(function(tmp) {
		if (tmp != menu) {
			var a = tmp._popupMenus, i;
			for (i = a.length; --i >= 0;)
				a[i].close(false, true);
			tmp._baseMenuInfo.close();
			window.status = "";
		}
	});
};

DynarchMenu.addIcon = function(item, icon) {
	var CE = DynarchMenu._createElement, t,
		l = item.firstChild.firstChild,
		r = CE("tr", CE("tbody", t = CE("table"))),
		td1 = CE("td", r),
		td2 = CE("td", r);
	icon.unselectable = "on";
	td1.appendChild(icon);
	while (l) {
		td1 = l.nextSibling;
		td2.appendChild(l);
		l = td1;
	}
	t.cellSpacing = t.cellPadding = 0;
	t.style.borderCollapse = "collapse";
	item.firstChild.appendChild(t);
};

DynarchMenu.prototype.createMenuItem = function(li, parent, horiz, arrow) {
	var tmp, ctx = null, cfg = this.config, licl = li.className, icon = null, label, html_popup = true, tooltip, action = null, item, info, self = this, key = null, disabled = DynarchMenu._RE_DS.test(licl);
	if (cfg.d_profile)
		++DynarchMenu.profile.item;
	tmp = DynarchMenu._getChildrenByTagName(li, "a");
	tmp = tmp.length > 0 ? tmp[0] : li;
	label = DynarchMenu._getLabel(tmp);
	if (typeof label == "string") {
		label = label.replace(/(^\s+|\s+$)/g, '');
		if (!/^<img/i.test(label)) {
			label = label.replace(/_([a-zA-Z0-9])/, "<u unselectable='on'>$1</u>");
			key = RegExp.$1;
			label = label.replace(/__/, "_");
		}
		html_popup = false;
	}
	tooltip = /^\s*$/.test(tmp.title) ? "" : tmp.title;
	if (tmp.href && /\S/.test(tmp.href)) {
		if (/^javascript:(.*)$/i.test(tmp.href))
			action = new DynarchMenu.JSAction(RegExp.$1);
		else
			action = new DynarchMenu.LinkAction(tmp.href, tmp.target);
	} else
		action = new DynarchMenu.DefaultAction(li);
	if (/^a$/.test(tmp.tagName))
		tmp.parentNode.removeChild(tmp);
	tmp = DynarchMenu._getChildrenByTagName(li, "img");
	if (tmp.length > 0)
		icon = tmp[0];
	info = new DynarchMenu.MenuItem({
		html_popup : html_popup,
		separator  : html_popup || !/\S/.test(label) && !icon,
		icon       : icon,
		label      : label,
		parent     : parent,
		submenu    : null,
		tooltip    : tooltip,
		action     : action,
		menu       : this,
		disabled   : disabled
	});
	if (li.id)
		this.items[info.id = li.id] = info;
	if (action)
		action.info = info;
	if (horiz) {
		item = DynarchMenu._createElement("td");
		info.labelTD = item;
		if (info.separator)
			item.innerHTML = "<div unselectable='on'></div>";
		else {
			item.innerHTML = "<div unselectable='on'>" + label + "</div>";
			if (icon)
				DynarchMenu.addIcon(item, icon);
		}
	} else {
		item = DynarchMenu._createElement("tr");
		tmp = DynarchMenu._createElement("td", item);
		if (info.separator && !html_popup) {
			tmp.innerHTML = _dynarch_menu_ediv;
			tmp.colSpan = 3;
		} else {
			tmp.className = "icon";
			if (icon)
				tmp.appendChild(icon);
			else
				tmp.innerHTML = _dynarch_menu_ediv;
			tmp = DynarchMenu._createElement("td", item);
			tmp.className = "label";
			info.labelTD = tmp;
			if (html_popup)
				tmp.appendChild(label);
			else
				tmp.innerHTML = label;
			tmp = DynarchMenu._createElement("td", item);
			tmp.className = "end";
			tmp.innerHTML = _dynarch_menu_ediv;
			if (arrow)
				tmp.className += " arrow";
		}
	}
	info.element = item;
	item.className = (info.separator && !html_popup) ? "separator" : "item";
	if (disabled)
		info.disabled = true;
	if (cfg.tooltips)
		item.title = info.tooltip;
	DynarchMenu.addInfo(item, "__msh_info", info);
	if (DynarchMenu._RE_CTX_ID.test(licl)) {
		ctx = window.self.document.getElementById(RegExp.$1);
		if (ctx) DynarchMenu.setupContext(ctx, info);
	} else if (DynarchMenu._RE_CTX_CL.test(licl)) {
		ctx = window.self.document.getElementsByTagName(RegExp.$1);
		tmp = new RegExp('(^|\\s)' + RegExp.$2 + '(\\s|$)');
		for (i = ctx.length; --i >= 0;)
			if (tmp.test(ctx[i].className))
				DynarchMenu.setupContext(ctx[i], info);
	} else if (licl)
		item.className += " " + licl;
	if (html_popup) item.onmouseover = DynarchMenu.EventHandlers.popup_resetActive;
	if (key)
		parent.keymap[key.toLowerCase()] = info;
	item.onmouseover = DynarchMenu.EventHandlers.item_onMouseOver;
	if (info.separator)
		return item;
	item.onmouseout = DynarchMenu.EventHandlers.item_onMouseOut;
	item.onmousedown = DynarchMenu.EventHandlers.item_onMouseDown;
	return item;
};

DynarchMenu._documentMouseDown = function(ev) {
	ev || (ev = window.event);
	var el = is_ie ? ev.srcElement : ev.target, j;
	for (j = el; j && !j.__msh_info; j = j.parentNode);
	if (!j || j.__msh_info.base)
		DynarchMenu._closeOtherMenus(j && j.__msh_info.menu);
};

DynarchMenu._msupTimeout = null;
DynarchMenu._documentMouseUp = function(ev) {
	ev || (ev = window.event);
	if (DynarchMenu._msupTimeout)
		return false;
	var menu = DynarchMenu._C, el, info;
	if (menu) {
		el = is_ie ? ev.srcElement : ev.target;
		for (; el && !(info = el.__msh_info); el = el.parentNode);
		if (!el)
			DynarchMenu._closeOtherMenus(null);
		else if (info && info.exec)
			info.exec();
	}
	DynarchMenu._C = null;
	DynarchMenu._activeItem = null;
};

DynarchMenu._documentMouseOver = function(ev) {
	var menu = DynarchMenu._C, el, tmout;
	if (menu && menu.config.electric) {
		ev || (ev = window.event);
		el = is_ie ? ev.srcElement : ev.target;
		for (; el && !el.__msh_info; el = el.parentNode);
		if (!el) {
			tmout = menu.config.electric;
			if (tmout == true)
				tmout = 1;
			if (!DynarchMenu._T)
				DynarchMenu._T = setTimeout('DynarchMenu._closeOtherMenus(null); DynarchMenu._T = null;', tmout);
		} else DynarchMenu._clearTimeout();
	}
};

DynarchMenu._documentKeyPress = function(ev) {
	ev || (ev = window.event);
	DynarchMenu._forAllMenus(function(menu) {
		var tmp = menu._activePopup, item = tmp ? tmp.active_item : null, kmap;
		function do_27() {
			if (tmp) {
				tmp.close(true, true);
				if (item) item.mouseout();
				if (tmp.base || (tmp.parent.base && tmp.config.context)) {
					tmp.resetActive();
					tmp.active_submenu = null;
					DynarchMenu._activeItem = null;
					DynarchMenu._closeOtherMenus(null);
				}
				DynarchMenu._stopEvent(ev);
			}
		};
		function do_13() {
			if (!item)
				return;
			item.activate(true, true);
			if (item.action && !item.submenu)
				item.exec();
			DynarchMenu._stopEvent(ev);
		};
		function do_ud(up) {
			if (tmp) {
				if (!item)
					item = up ? tmp.getFirstItem(item) : tmp.getLastItem(item);
				else
					item = up ? tmp.getNextItem(item) : tmp.getPrevItem(item);
				item.hover(false, true);
				tmp.active_item = item;
				DynarchMenu._stopEvent(ev);
			}
		};
		function serveKeymap(keymap) {
			var key = String.fromCharCode((is_ie || is_opera) ? ev.keyCode : ev.charCode).toLowerCase();
			item = keymap[key];
			if (typeof item != "undefined") {
				item.hover(true, true);
				if (!item.submenu)
					item.exec();
				tmp = item.submenu;
				item = null;
				do_ud(true);
				DynarchMenu._stopEvent(ev);
			}
		};
		switch (ev.keyCode) {
		    case 27: do_27(); break;
		    case 13:
			do_13();
			if (item) {
				tmp = item.submenu;
				item = null;
				do_ud(true);
			}
			break;
		    case 37: // left
			if (!menu._activeKeymap)
				break;
			if (tmp.parent && !tmp.parent.horiz)
				do_27();
			else {
				if (tmp.parent) {
					tmp = tmp.parent;
					item = tmp.active_item;
				}
				do_ud(false);
				item.activate(false, true);
			}
			break;
		    case 39: // right
			if (!menu._activeKeymap)
				break;
			if (item && !item.parent.horiz && item.submenu) {
				do_13();
				tmp = item.submenu;
				item = null;
				do_ud(true);
			} else {
				while (tmp.parent) {
					tmp = tmp.parent;
					item = tmp.active_item;
				}
				do_ud(true);
				item.activate(false, true);
			}
			break;
		    case 40: // down
		    case 38: // up
			if (!menu._activeKeymap)
				break;
			do_ud(ev.keyCode == 40);
			break;
		    default:
			kmap = ev.altKey ? menu._globalKeymap : menu._activeKeymap;
			if (kmap)
				serveKeymap(kmap);
		}
	});
};

DynarchMenu.prototype.createMenuTree = function(ul, horiz) {
	var base, a_li, div, table, i, info, li, item, tmp, ret = null, self = this, cfg = this.config, ctx = cfg.context;
	if (cfg.d_profile)
		++DynarchMenu.profile.tree;
	base = !this._baseMenuInfo;
	a_li = DynarchMenu._getChildrenByTagName(ul, "li");
	if (a_li.length == 0)
		return;
	ret = div = DynarchMenu._createElement("div");
	div.className = (base && horiz) ? "dynarch-horiz-menu" : "dynarch-popup-menu";
	if (base && horiz && cfg.toolbar)
		div.className += " dynarch-menu-toolbar";
	if (base && !horiz && !ctx)
		div.className += " dynarch-popup-base-menu";
	if (ul.className)
		div.className += " " + ul.className;
	tmp = ["a", "b", "c", "d"];
	for (i = tmp.length; --i >= 0; (div = DynarchMenu._createElement("div", div)).className = tmp[i]);
	info = new DynarchMenu.MenuTree({
		menu           : this,
		base           : base,
		horiz          : horiz,
		element        : ret,
		active_submenu : null,
		active_item    : null,
		visible        : false,
		keymap         : {},
		config         : cfg,
		_T_close       : null
	});
	if (ul.id)
		info.id = ul.id;
	DynarchMenu.addInfo(ret, "__msh_info", info);
	info.table = table = DynarchMenu._createElement("table", div);
	table.cellSpacing = 0;
	table.cellPadding = 0;
	tmp = DynarchMenu._createElement("tbody", table);
	DynarchMenu._class(ret, null, cfg.className);
	if (base) {
		this._globalKeymap = info.keymap;
		this._baseMenuInfo = info;
		if (ctx)
			ret.style.display = "none";
	} else {
		ret.style.display = "none";
		if (this.config.lazy)
			document.body.appendChild(ret);
		else if (this._df)
			this._df.appendChild(ret);
		else
			this._ca[this._ca.length] = ret;
	}
	if (horiz) {
		info.parent = null;
		div = DynarchMenu._createElement("tr", tmp);
	} else
		div = tmp;
	for (i = 0; i < a_li.length; ++i) {
		li = a_li[i];
		tmp = DynarchMenu._getChildrenByTagName(li, "ul");
		var submenu = tmp.length > 0;
		item = this.createMenuItem(li, info, horiz, submenu);
		div.appendChild(item);
		if (submenu) {
			item.__msh_info.ul = ul = tmp[0];
			item.__msh_info.submenu = function() {
				var menu = this.menu;
				submenu = this.submenu = menu.createMenuTree(this.ul, false).__msh_info;
				submenu.parent = info;
				submenu.parent_item = this;
				menu._popupMenus[menu._popupMenus.length] = submenu;
			};
			if (!this.config.lazy)
				item.__msh_info.submenu();
		}
	}
	ret.onmouseover = DynarchMenu.EventHandlers.tree_onMouseOver;
	ret.onmouseout = DynarchMenu.EventHandlers.tree_onMouseOut;
	return ret;
};

DynarchMenu.prototype.destroy = function() {
	var a = this._baseMenuInfo.element, i;
	a.parentNode.removeChild(a);
	a = this._popupMenus;
	for (i = a.length; --i >= 0;)
		a[i].element.parentNode.removeChild(a[i].element);
	a = DynarchMenu._menus;
	for (i = a.length; --i >= 0;)
		if (a[i] == this)
			a.splice(i, 1);
};

DynarchMenu._stopEvent = function(ev) {
	if (is_ie) {
		ev.cancelBubble = true;
		ev.returnValue = false;
	} else {
		ev.preventDefault();
		ev.stopPropagation();
	}
};

DynarchMenu._removeEvent = function(el, evname, func) {
	if (el.removeEventListener)
		el.removeEventListener(evname, func, true);
	else if (el.detachEvent)
		el.detachEvent("on" + evname, func);
	else
		el["on" + evname] = null;
};

DynarchMenu._addEvent = function(el, evname, func) {
	if (el.addEventListener)
		el.addEventListener(evname, func, true);
	else if (el.attachEvent)
		el.attachEvent("on" + evname, func);
	else
		el["on" + evname] = func;
};

DynarchMenu._getChildrenByTagName = function(el, tag) {
	var i, a = [];
	if (tag)
		tag = tag.toLowerCase();
	for (i = el.firstChild; i; i = i.nextSibling) {
		if (i.nodeType != 1)
			continue;
		if (!tag || tag == i.tagName.toLowerCase())
			a[a.length] = i;
	}
	return a;
};

DynarchMenu._createElement = function(tagName, parent, doc) {
	if (!doc)
		doc = document;
	var el = doc.createElement(tagName);
	if (is_ie)
		el.unselectable = "on";
	else if (is_gecko)
		el.style.setProperty("-moz-user-select", "none", "");
	if (parent)
		parent.appendChild(el);
	return el;
};

DynarchMenu._getLabel = function(el) {
	var i, c, txt;
	if (el.tagName.toLowerCase() == "a") {
		if (is_ie) {
			c = DynarchMenu._getChildrenByTagName(el, null);
			for (i = c.length; --i >= 0; c[i].unselectable = "on");
		}
		return el.innerHTML;
	}
	c = DynarchMenu._getChildrenByTagName(el, 'div');
	if (c.length)
		return c[0];
	txt = "";
	for (i = el.firstChild; i; i = i.nextSibling)
		if (i.nodeType == 3)
			txt += i.data;
	return txt;
};

DynarchMenu._getPos = function (el) {
	if (/^body$/i.test(el.tagName))
		return { x: 0, y: 0 };
	var SL = 0, ST = 0, is_div = /^div$/i.test(el.tagName), r, tmp;
	if (is_div && el.scrollLeft)
		SL = el.scrollLeft;
	if (is_div && el.scrollTop)
		ST = el.scrollTop;
	r = { x: el.offsetLeft - SL, y: el.offsetTop - ST };
	if (el.offsetParent) {
		tmp = this._getPos(el.offsetParent);
		r.x += tmp.x;
		r.y += tmp.y;
	}
	return r;
};

DynarchMenu._class = function(el, del, add) {
	if (!el)
		return;
	if (el.element)
		el = el.element;
	if (del)
		el.className = el.className.replace(del, ' ');
	if (add)
		el.className += " " + add;
};

DynarchMenu._related = function(element, ev) {
	var related, type;
	if (is_ie) {
		type = ev.type;
		if (type == "mouseover")
			related = ev.fromElement;
		else if (type == "mouseout")
			related = ev.toElement;
	} else
		related = ev.relatedTarget;
	for (; related; related = related.parentNode)
		if (related == element)
			return true;
	return false;
};

DynarchMenu.psLeft = function() {
	return window.self.document.documentElement.scrollLeft ||
		window.self.document.body.scrollLeft;
};

DynarchMenu.psTop = function() {
	return window.self.document.documentElement.scrollTop ||
		window.self.document.body.scrollTop;
};

//** global **// -*- javascript -*-

DynarchMenu._infoMap = null;
DynarchMenu._cleanUp = function() {
	var a = DynarchMenu._infoMap, i, o, p;
	for (i = a.length; --i >= 0;) {
		o = a[i][0];
		p = a[i][1];
		o[p] = null;
	}
};
DynarchMenu.addInfo = function(el, name, value) {
	el[name] = value;
	var a = this._infoMap;
	if (!a) {
		a = this._infoMap = [];
		DynarchMenu._addEvent(window, "unload", DynarchMenu._cleanUp);
	}
	a[a.length] = [el, name];
};

DynarchMenu.setupContext = function(ctx, tree) {
	this.addInfo(ctx, "__msh_info2", tree);
	var buttons = 2, b;
	if (/dynarch-menu-ctxbutton-([a-z]+)/.test(ctx.className)) {
		b = RegExp.$1;
		buttons = ((b == "left") ? 1 : ((b == "both") ? 3 : buttons));
	}
	if (buttons & 1)
		ctx.onclick = DynarchMenu.EventHandlers.ctx_onContextMenu;
	if (buttons & 2)
		ctx[is_opera ? "onmousedown" : "oncontextmenu"] = DynarchMenu.EventHandlers.ctx_onContextMenu;
};

//** Action handler objects **//

DynarchMenu.JSAction = function(code) {
	this.js = code.replace(/%20/g, ' ');
};

DynarchMenu.JSAction.prototype.exec = function() {
	var retval = false;
	eval(this.js);
	return retval;
};

//--

DynarchMenu.LinkAction = function(url, target) {
	if (!(target && /\S/.test(target)))
		target = null;
	this.url = url;
	this.target = target;
};

DynarchMenu.LinkAction.prototype.exec = function() {
	if (this.target) {
		if (!is_ie)
			window.self.open(this.url, this.target);
		else {
			var tmp = window.self.document.getElementById(this.target);
			if (!tmp) {
				tmp = window.self.document.getElementsByName(this.target);
				tmp = tmp.length ? tmp[0] : null;
			}
			tmp = tmp ? (is_opera ? tmp : tmp.contentWindow) : window.self;
			tmp.location = this.url;
		}
	} else
		window.self.location = this.url;
	return false;
};

//--

DynarchMenu.DefaultAction = function(li) {
	this.params = li;
	while (li && /^([uo]l|li)$/i.test(li.tagName)) {
		if (li.onclick) {
			this.action = li.onclick;
			break;
		}
		li = li.parentNode;
	}
};

DynarchMenu.DefaultAction.prototype.exec = function() {
	if (!this.info.submenu) {
		if (typeof this.action == "function")
			return this.action(this.info);
		else try {
			var retval = false;
			eval(this.action);
			return retval;
		} catch(e) {};
	}
	return true;
};

//** Event handlers **//

DynarchMenu.EventHandlers = {
	popup_resetActive : function(ev) {
		this.__msh_info.parent.resetActive();
		return false;
	},
	item_onMouseOver : function(ev) {
		ev || (ev = window.event);
		if (DynarchMenu._related(this, ev))
			return false;
		var item = this.__msh_info;
		if (!item.separator)
			return item.hover();
	},
	item_onMouseOut : function(ev) {
		ev || (ev = window.event);
		if (DynarchMenu._related(this, ev))
			return false;
		return this.__msh_info.mouseout();
	},
	item_onMouseDown : function(ev) {
		ev || (ev = window.event);
		var info = this.__msh_info, ret;
		DynarchMenu._C = info.menu;
		DynarchMenu._stopEvent(ev);
		DynarchMenu._activeItem = info;
		if (info.parent && !info.parent.base)
			info.parent.closePopups();
		ret = info.activate(false, true);
		return ret;
	},
	tree_onMouseOver : function(ev) {
		ev || (ev = window.event);
		if (!DynarchMenu._related(this, ev)) {
			var info = this.__msh_info;
			if (info.parent) {
				info.parent.resetActive(info.parent_item, "active");
				info.parent.active_submenu = info;
			}
		}
		return false;
	},
	tree_onMouseOut : function(ev) {
		ev || (ev = window.event);
		if (!DynarchMenu._related(this, ev)) {
			var info = this.__msh_info;
			if (!info.active_submenu)
				this.__msh_info.resetActive();
		}
		return false;
	},
	ctx_onContextMenu : function(ev) {
		ev || (ev = window.event);
		if (!is_opera || ev.button == 2) {
			if (DynarchMenu._msupTimeout)
				clearTimeout(DynarchMenu._msupTimeout);
			DynarchMenu._msupTimeout = setTimeout(function() { DynarchMenu._msupTimeout = null; }, 150);
			var info = this.__msh_info2;
			info.submenu.openContext(ev, this);
			setTimeout(function() {
				DynarchMenu._C = info.menu;
			}, info.menu.config.timeout);
			DynarchMenu._stopEvent(ev);
			return false;
		}
	}
};

//** Menu Item Objects **//

DynarchMenu.populateObject = function(o, props) {
	for (var i in props)
		o[i] = props[i];
};

DynarchMenu.MenuItem = function(props) {
	this.visible = true;
	this.pressed = false;
	DynarchMenu.populateObject(this, props);
};

DynarchMenu.MenuItem.prototype.disable = function(dis) {
	if (typeof dis == "undefined") dis = true;
	this.disabled = dis;
	DynarchMenu._class(this.element, DynarchMenu._RE_DS, dis ? "disabled" : null);
};

DynarchMenu.MenuItem.prototype.display = function(dis) {
	if (typeof dis == "undefined")
		dis = !this.visible;
	this.visible = dis;
	this.element.style.display = dis ? "" : "none";
};

DynarchMenu.MenuItem.prototype._exec = function() {
	if (!this.disabled && !this.separator && this.action && !this.action.exec()) {
		DynarchMenu._class(this.element, DynarchMenu._RE_AH);
		var a = this.menu._popupMenus, i;
		for (i = a.length; --i >= 0;)
			a[i].close(false, true);
		this.menu._baseMenuInfo.close();
		window.status = "";
	}
};

DynarchMenu.MenuItem.prototype.exec = function() {
	if (this.submenu || !this.menu.config.blink)
		return this._exec();
	var self = this;
	var step = 7;
	var timer = setInterval(function() {
		DynarchMenu._class(self.element, DynarchMenu._RE_AH, --step & 1 ? 'active' : null);
		if (!step) {
			clearInterval(timer);
			self._exec();
		}
	}, 60);
};

DynarchMenu.MenuItem.prototype.setLabel = function(text) {
	this.labelTD.innerHTML = "<div unselectable='on'>" + text + "</div>";
	this.label = text;
};

DynarchMenu.MenuItem.prototype.hover = function(activate, instant) {
	var menu = this.parent, el = this.element;
	if (this.disabled && menu.base) {
		menu.clearPopups(this);
		menu.resetActive();
		return;
	}
	if (menu.active_item == this)
		return false;
	menu.clearTimeout();
	window.status = this.tooltip;
	el.title = menu.config.tooltips ? this.tooltip : "";
	if (typeof activate == "undefined")
		activate = this.submenu && (menu.config.electric || !menu.base || menu.active_submenu);
	menu.clearPopups(this);
	if (menu.resetActive(this))
		DynarchMenu._clearTimeout();
	if (activate)
		this.activate(true, instant);
	return false;
};

DynarchMenu.MenuItem.prototype.activate = function(noclose, instant) {
	if (!this.disabled) {
		var menu = this.parent, submenu = this.submenu, el = this.element;
		menu.resetActive(this);
		if (submenu) {
			if (typeof submenu == "function") {
				this.submenu();
				submenu = this.submenu;
			}
			if (!noclose && !menu.config.electric && menu.base && submenu == menu.active_submenu) {
				submenu.close(false, true);
				DynarchMenu._activeItem = null;
				menu.resetActive(this, "hover");
			} else submenu.open(el, this, instant);
		}
	}
	return false;
};

DynarchMenu.MenuItem.prototype.setClass = function(del, add) {
	DynarchMenu._class(this.element, del, add);
};

DynarchMenu.MenuItem.prototype.setPressed = function(state) {
	if (typeof state == "undefined") state = !this.pressed;
	this.pressed = state;
	this.setClass(DynarchMenu._RE_PR, state ? "pressed" : null);
};

DynarchMenu.MenuItem.prototype.mouseout = function() {
	var
		p = this.parent,
		s = this.submenu;
	if (s && DynarchMenu._OT)
		clearTimeout(DynarchMenu._OT);
	DynarchMenu._clearTimeout();
	// FIXME: separators fix, but better not.
 	if (!s || !s.visible)
 		p.resetActive();
	window.status = "";
	return false;
};

//** Menu Tree Objects **//

DynarchMenu.MenuTree = function(props) {
	DynarchMenu.populateObject(this, props);
	if (!this.base)
		this.hider = DynarchMenu._createHider();
};

DynarchMenu.MenuTree.prototype.getNextItem = function(item) {
	var i = item.element.nextSibling;
	while (i && i.__msh_info.separator)
		i = i.nextSibling;
	if (!i)
		i = item.element.parentNode.firstChild;
	return i.__msh_info;
};

DynarchMenu.MenuTree.prototype.getPrevItem = function(item) {
	var i = item.element.previousSibling;
	while (i && i.__msh_info.separator)
		i = i.previousSibling;
	if (!i)
		i = item.element.parentNode.lastChild;
	return i.__msh_info;
};

DynarchMenu.MenuTree.prototype.resetActive = function(item, cls) {
	item || (item = null);
	cls || (cls = "hover");
	DynarchMenu._class(this.active_item, DynarchMenu._RE_AH);
	DynarchMenu._class(item, DynarchMenu._RE_AH, DynarchMenu._activeItem == item ? "active" : cls);
	var tmp = this.active_item != item;
	this.active_item = item;
	return tmp;
};

DynarchMenu.MenuTree.prototype.clearPopups = function(item) {
	var m = this.active_submenu;
	if (m && m != item.submenu)
		m.close();
};

DynarchMenu.MenuTree.prototype.closePopups = function() {
	var i, m;
	for (i = this.getFirstItem().element; i; i = i.nextSibling) {
		m = i.__msh_info.submenu;
		if (m && typeof m != "function")
			m.closePopups().close(false, true);
	}
	return this;
};

DynarchMenu.MenuTree.prototype.clearTimeout = function() {
	if (this._T_close) {
		clearTimeout(this._T_close);
		this._T_close = null;
	}
};

DynarchMenu.MenuTree.prototype.close = function(by_key, instant) {
	var self = this.menu;
	if (this.base) {
		self._activeKeymap = null;
		self._activePopup = null;
	} else {
		if (!this.visible || (this._T_close && !instant))
			return false;
		var info = this;
		tmp = this.closePopups().parent;
		tmp.resetActive(by_key ? tmp.active_item : null);
		DynarchMenu._class(this.active_item, DynarchMenu._RE_AH);
		if (!by_key)
			tmp.active_item = null;
		tmp.active_submenu = null;
		this.active_item = this.active_submenu = null;
		if (instant || this.parent.base)
			this._close();
		else this._T_close = setTimeout(function() {
			info._close();
			info._T_close = null;
		}, self.config.timeout);
	}
};

DynarchMenu.MenuTree.prototype.getFirstItem = function() {
	return this.horiz ?
		this.element.firstChild.__msh_info :
		this.table.firstChild.firstChild.__msh_info;
};

DynarchMenu.MenuTree.prototype.getLastItem = function() {
	return this.horiz ?
		this.element.lastChild.__msh_info :
		this.table.lastChild.lastChild.__msh_info;
};

DynarchMenu.MenuTree.prototype.openContext = function(ev, trigger) {
	if (!trigger)
		trigger = null;
	this.menu.target = trigger;
	this.open(null, null, true,
	{ x: ev.clientX + document.body.scrollLeft, y: ev.clientY + document.body.scrollTop });
};

DynarchMenu.MenuTree.prototype.open = function(el, item, instant, pos) {
	this.clearTimeout();
	DynarchMenu._clearTimeout();
	if (DynarchMenu._OT)
		clearTimeout(DynarchMenu._OT);
	var info = this;
	if (instant || this.parent.base)
		this._open(el, item, pos);
	else DynarchMenu._OT = setTimeout(function() {
		info._open(el, item, pos);
		DynarchMenu._OT = null;
	}, this.menu.config.timeout);
};

DynarchMenu.MenuTree.prototype._close = function() {
	this.element.style.display = "none";
	this.visible = false;
	this.menu._activePopup = this.parent;
	this.menu._activeKeymap = this.parent.keymap;
	if (this._shadow)
		this._shadow.style.display = "none";
	for (var i = this.getFirstItem().element; i; i = i.nextSibling)
		DynarchMenu._class(i, DynarchMenu._RE_AH);
	DynarchMenu._closeHider(this.hider);
};

DynarchMenu.MenuTree.prototype._open = function(el, item, pos) {
	var
		m = this.element,
		self = this.menu,
		p = el ? DynarchMenu._getPos(el) : pos,
		cfg = self.config,
		pe,
		base = this.parent ? this.parent.base : false,
		dx = base ? cfg.basedx : cfg.dx,
		dy = base ? cfg.basedy : cfg.dy,
		horiz = this.parent ? this.parent.horiz : false,
		tmp, s, vw, sw;

	if (!el) el = { offsetHeight : 0, offsetWidth: 0 };

	if (self._fixed && !is_ie && base) {
		p.x += DynarchMenu.psLeft();
		p.y += DynarchMenu.psTop();
	}
	pe = { x: p.x, y: p.y };

	DynarchMenu._C = self;
	DynarchMenu._closeOtherMenus(self);
	if (!base && item)
		item.parent.closePopups();

	if (horiz)
		p.y += el.offsetHeight;
	else {
		if (!is_khtml) {
			p.x += el.offsetWidth;
		} else if (el) {
			// FIXME: remove this when Konqueror has proper offsets support
			p = DynarchMenu._getPos(el.lastChild);
			p.x += el.lastChild.offsetWidth;
		}
	}

	vw = DynarchMenu.getWinSize();
	vw.x += DynarchMenu.psLeft();
	vw.y += DynarchMenu.psTop();

	sw = cfg.shadows || [0, 0];

	s = m.style;
	if (is_ie)
		s.position = "absolute";
	s.display = "block";
	if (p.x + m.offsetWidth > vw.x) {
		p.x = pe.x - m.offsetWidth + (horiz ? el.offsetWidth : 0);
		dx = -dx;
	}
	if (p.y + m.offsetHeight > vw.y && pe.y > m.offsetHeight) {
		p.y = pe.y - m.offsetHeight +
			(horiz ? 0 :
			 (DynarchMenu._getPos(m).y + m.offsetHeight - DynarchMenu._getPos(this.getLastItem().element).y));
		dy = -dy;
	} else if (!horiz)
		p.y -= DynarchMenu._getPos(this.getFirstItem().element).y - DynarchMenu._getPos(m).y;
	if (p.x + m.offsetWidth + sw[0] > vw.x)
		p.x -= sw[0];
	p.x += dx;
	p.y += dy;
	s.left = p.x + "px";
	s.top = p.y + "px";
	DynarchMenu._setupHider(this.hider, p.x, p.y, m.offsetWidth + sw[0], m.offsetHeight + sw[1]);
	if (this.parent) {
		this.parent.active_submenu = this;
		this.parent.resetActive(item, "active");
	}
	this.visible = true;
	self._activePopup = this;
	self._activeKeymap = this.keymap;

	tmp = this._shadow;
	if (cfg.shadows) {
		if (!tmp) {
			var SS = cfg.smoothShadow;
			this._shadow = tmp = DynarchMenu._createElement((SS && !is_ie) ? "img" : "div");
			if (SS)
				tmp.src = _dynarch_menu_shadow.src;
			tmp.className = "dynarch-menu-shadow";
			DynarchMenu.addInfo(tmp, '__msh_info', this);
			if (is_ie)
				tmp.style.position = "absolute";
			if (SS && is_ie && !is_ie5) {
				tmp.className = "dynarch-IE6-shadow";
				tmp.runtimeStyle.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + _dynarch_menu_shadow.src + "',sizingMethod='scale')";
			}
			tmp.style.width = "2px";
			tmp.style.height = "2px";
			document.body.appendChild(tmp);
		}
		s = tmp.style;
		s.left = p.x + sw[0] + "px";
		s.top = p.y + sw[1] + "px";
		s.height = m.offsetHeight + "px";
		s.width = m.offsetWidth + "px";
		s.display = "block";
		// window.scrollTo(p.x, p.y);
		// FIXME: scroll the window ONLY if the popup menu is not visible
	}
};

DynarchMenu.getWinSize = function() {
	if (is_gecko) {
		if (document.documentElement.clientWidth)
			return { x: document.documentElement.clientWidth, y: document.documentElement.clientHeight };
		else
			return { x: window.innerWidth, y: window.innerHeight };
	}
	if (is_opera)
		return { x: window.innerWidth, y: window.innerHeight };
	if (is_ie) {
		if (!document.compatMode || document.compatMode == "BackCompat")
			return { x: document.body.clientWidth, y: document.body.clientHeight };
		else
			return { x: document.documentElement.clientWidth, y: document.documentElement.clientHeight };
	}
	// let's hope we never get to use this hack.
	var div = document.createElement("div"), s = div.style;
	s.position = "absolute";
	s.bottom = s.right = "0px";
	document.body.appendChild(div);
	s = { x: div.offsetLeft, y: div.offsetTop };
	document.body.removeChild(div);
	return s;
};


// DO NOT CHANGE THIS!
DynarchMenu._nfo={product:"hmenu-2.4",licensee:"Discovery Productions, Inc.",license_key:"4694276c07e315e8e82516d734a9cd33",purchase_date:"Thu Jan 13 17:19:43 2005 GMT",license_type:"developer"};
