#!/bin/bash

current_dir=`pwd | sed -r "s@$HOME/@@"`;

echo $current_dir;

for i in $( \
	ls -A \
	| grep -vE '(\.bak$|~$)' \
	| grep -vE '^\.(config|ssh)$' \
	| grep -vE '^(\.(git(attributes|ignore|modules)?|svn))$' \
	| grep -vE 'README(\.md)?' \
	| grep -vE 'add-submodules.sh|create-links.sh' \
); do
#	if [ "x$i" != "x." ]; then
#		if [ "x$i" != "x.." ]; then
			if [ -d ~/${i} ]; then
				echo "~/${i} is a directory... deleting.";
				rm -fr ~/${i};
			elif [ -L ~/${i} ]; then
				echo "~/${i} is a symbolic link... deleting.";
				rm ~/${i};
			elif [ -f ~/${i} ]; then
				echo "~/${i} is a normal file... deleting.";
				rm ~/${i};
			elif [ -e ~/${i} ]; then
				echo "~/${i} exists, but unknown... deleting.";
				rm -r ~/${i};
			else
				echo "~/${i} does not exist.";
			fi;
			echo "Attempting to create the link ~/${i}";
			ln -s $current_dir/${i} ~/${i};
#		fi;
#	fi;
done;

interiorDirs="
.config
";
# now to make ~/.config/bash a soft link pointing to
# ~/.home/home-core/.config/bash
for interiorDir in $interiorDirs; do
	if [ -e ${interiorDir} ]; then
		cd ${interiorDir}
		current_dir=`pwd | sed -r "s@$HOME/@@"`;
		
		echo $current_dir;
		test -e ~/${interiorDir} || mkdir -p ~/${interiorDir}
		
		for i in $( \
			ls -A \
			| grep -vE '(\.bak$|~$)' \
			| grep -vE '^\.(config|ssh)$' \
			| grep -vE '^(\.(git(attributes|ignore|modules)?|svn))$' \
			| grep -vE 'README(\.md)?' \
			| grep -vE 'add-submodules.sh|create-links.sh' \
		); do
			if [ -L ~/${interiorDir}/${i} ]; then
				echo "~/${interiorDir}/${i} is a symbolic link... deleting.";
				rm ~/${interiorDir}/${i};
			elif [ -d ~/${interiorDir}/${i} ]; then
				echo "~/${interiorDir}/${i} is a directory...";
				echo 'Press any key to continue or CTRL+C to cancel and check or move directory';
				read -n 1 -s;
				rm -fr ~/${interiorDir}/${i};
			elif [ -f ~/${interiorDir}/${i} ]; then
				echo "~/${interiorDir}/${i} is a normal file... deleting.";
				rm ~/${interiorDir}/${i};
			elif [ -e ~/${interiorDir}/${i} ]; then
				echo "~/${interiorDir}/${i} exists, but unknown... deleting.";
				echo 'Press any key to continue or CTRL+C to cancel and check or move directory';
				read -n 1 -s;
				rm -r ~/${interiorDir}/${i};
			else
				echo "~/${interiorDir}/${i} does not exist.";
			fi;
			echo "Attempting to create the link ~/${interiorDir}/${i}";
			echo "ln -s ../$current_dir/${i} ~/${interiorDir}/${i}";
			ln -s ../$current_dir/${i} ~/${interiorDir}/${i};
		done;

	fi;
done;

