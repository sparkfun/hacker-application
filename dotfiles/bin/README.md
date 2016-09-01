# ~/bin

This is a filtered list of files I have in my ``~/bin/`` as some of the files
are very old and need to be fixed before I'm willing to push them publicly.

* ``apt-set-tuxonice.sh`` - This is for laptops making use of tuxonice as
	usually there's a newer kernel before tuxonice has a new kernel.
* ``bofh`` - easy way to just run ``fortune bofh-excuses``.
* ``create-domain-dir.sh`` - give it a list of domains and it will reverse the
	order.
* ``gopro-convert-video.sh`` - script I set up to convert multiple gopro vids
	from my roadtrip into 1 and into a format youtube would accept. It also
	echo's the command in case something went wrong and/or I decided to fast
	forward sections I could just modify the command as needed.
* ``removecase`` - moves all non-dotfiles in a directory to an all lowercase
	version of the filename.
* ``restore-svnserver.sh`` - creates a wrapper shell script for svnserve to
	define a directory where the repositories go.
* ``show_ansi`` - outputs a table chart of ANSI colors and the comments gives
	more information. I don't remember where I found this, but I did not write
	this.
* ``stream-twitch.sh`` - This is a modified script, source listed in comments,
	and converted to use avconv instead of ffmpeg. This does not reside in my
	home-common repository, but in 1 of my other repositories. As it pulls the
	key out of a separate file I figured I'd include it here.
* ``tcpdump-data-filter.pl`` - This is used to filter output from tcpdump,
	source listed in comments... ``tcpdump -s 1500 -l -x ... |
	tcpdump-data-filter.pl``

