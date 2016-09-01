#!/bin/bash

input_file1="$1"
input_file2="$2"
input_file3="$3"
input_file4="$4"
input_file5="$5"
input_file6="$6"
input_file7="$7"
input_file8="$8"
input_file9="$9"

out_file=$( \
	basename ${input_file1} \
	| sed -r 's@\.[Mm][Pp]4@.16-9.0.mp4@' \
);

avconv_ver=$( avconv -version 2>&1 | grep version | awk '{print $3}' | tr '_' '-' | awk -F- '{print $1}' );

ver_check=$( echo "${avconv_ver} >= 10" | bc );

if [[ -z "${input_file2}" && "${ver_check}" = "1" ]]; then
	#avconv -i GOPR0118.MP4 -vcodec copy -acodec copy -aspect 16:9 GOPR0118.16-9.0.mp4
	echo "avconv -i ${input_file1} -vcodec copy -acodec copy -aspect 16:9 ${out_file}";
	avconv -i ${input_file1} -vcodec copy -acodec copy -aspect 16:9 ${out_file}
	echo "avconv -i ${input_file1} -vcodec copy -acodec copy -aspect 16:9 ${out_file}";
else
	#mencoder -forceidx -ovc copy -oac pcm -force-avi-aspect 16:9 -o GOPR0118.16-9.0.MP4 GOPR0118.MP4 GP010118.MP4
	echo "mencoder -forceidx -ovc copy -oac pcm -force-avi-aspect 16:9 -o ${out_file} ${input_file1} ${input_file2} ${input_file3} ${input_file4} ${input_file5} ${input_file6} ${input_file7} ${input_file8} ${input_file9}"
	mencoder -forceidx -ovc copy -oac pcm \
		-force-avi-aspect 16:9 \
		-o ${out_file} \
		${input_file1} \
		${input_file2} \
		${input_file3} \
		${input_file4} \
		${input_file5} \
		${input_file6} \
		${input_file7} \
		${input_file8} \
		${input_file9} \
	;
	echo "mencoder -forceidx -ovc copy -oac pcm -force-avi-aspect 16:9 -o ${out_file} ${input_file1} ${input_file2} ${input_file3} ${input_file4} ${input_file5} ${input_file6} ${input_file7} ${input_file8} ${input_file9}"
fi;


#avconv -i GOPR0121.16-9.0.mp4 -ss 00:01:09.0 -f image2 -vframes 1 cover-0121.png
#convert cover-0121.png cover-0121.jpg
