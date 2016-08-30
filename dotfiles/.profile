
if [ "$TERM" = "console" ]; then
	echo "\033[37;44m\033[8]" #
# or use setterm.
#	setterm -foreground white -background blue -store
fi

PATH=~/bin:"${PATH}"
export LD_LIBRARY_PATH=/usr/lib/xulrunner/
