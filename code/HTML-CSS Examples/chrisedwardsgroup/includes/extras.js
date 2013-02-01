var newWindow;

function PrintContents()
{
	window.print();
}

function ContactAbout(windowURL) {
	newWindow = window.open(windowURL,'Details','resizable=yes,width=370,height=450');
	newWindow.moveTo(250,20);
	newWindow.focus();
	return false;
}

function EmailAFriend(windowURL) {
	newWindow = window.open(windowURL,'Details','resizable=yes,width=385,height=400');
	newWindow.moveTo(250,45);
	newWindow.focus();
	return false;
}

function ExtraPhoto(windowURL) {
	newWindow = window.open(windowURL,'Details','resizable=yes,width=380,height=400');
	newWindow.moveTo(100,40);
	newWindow.focus();
	return false;
}

function ShowTour(windowURL) {
	newWindow = window.open(windowURL,'','width=745,height=600,left=10,top=10,status=no,toolbar=no,menubar=no,scrollbars=yes,resizable=no');
	return false;
}