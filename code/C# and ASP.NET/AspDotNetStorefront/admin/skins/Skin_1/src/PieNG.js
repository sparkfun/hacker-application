// (c) dynarch.com 2004
// Author: Mihai Bazon, http://dynarch.com/mishoo/

var PieNG;
(PieNG = function(base) {
	if (/MSIE (5\.5|6).*Windows/.test(navigator.userAgent) && !/opera/i.test(navigator.userAgent)) {
		// fucked-up browser (Internet Explorer for Windows)
		var blank = new Image;
		blank.src = _dynarch_menu_url + 'img/blank.gif';
		base || (base = document);
		var imgs = base.getElementsByTagName("img");
		for (var i = imgs.length; --i >= 0;) {
			var img = imgs[i];
			var src = img.src;
			if (!/\.png$/.test(src))
				continue;
			var s = img.style;
			s.width = img.offsetWidth + "px";
			s.height = img.offsetHeight + "px";
			s.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(src='" + src + "',sizingMethod='scale')";
			img.src = blank.src;
		}
	}
})();
