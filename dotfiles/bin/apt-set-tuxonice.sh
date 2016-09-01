#!/bin/bash
#
# puts a hold on the linux headers/images packages so they shouldn't want to be updated
# unless tuxonice wants to update them. May need to figure out some way to test for
# when a new tuxonice update exists.
#
# `apt-set-tuxonice.sh 1` should suffice in ignoring standard kernel updates
#
# `apt-set-tuxonice.sh` should suffice to allow standard kernel updates
#
# default is to allow kernel updates in case this is run on another system

input=$1

case "${input}" in
	start*|1*|true*)
		type=hold;
		;;
	*)
		type=unhold;
		;;
esac;

sudo apt-mark ${type} linux-headers-generic linux-signed-generic linux-signed-image-generic
