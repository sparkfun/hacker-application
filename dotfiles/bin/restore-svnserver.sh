#/bin/sh

#/usr/sbin
#/usr/bin/svnserve
new=/usr/sbin/svnserve;

svnserve=`which svnserve`;

if [ "$new" != "$svnserve" ]; then
	#dir=`echo $svnserve | sed -r "s/\/[^\/]+$/\//"`;

	#echo mv $svnserve ${svnserve}2
	#mv $svnserve ${svnserve}2

	echo making new svnserve bash script
	echo "#!/bin/sh
# wrapper script for svnserve
umask 007
${svnserve} -t -r /var/svn/
" > $new;

	chmod ugo+rx $new;
fi;

