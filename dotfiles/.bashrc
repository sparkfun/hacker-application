# ~/.bashrc: executed by bash(1) for non-login shells.
# see /usr/share/doc/bash/examples/startup-files (in the package bash-doc)
# for examples

# If not running interactively, don't do anything:
[ -z "$PS1" ] && return

##########################################################
if [ -d ~/.config/bash ]; then
	export BASH_CONFIG_DIR=~/.config/bash;
elif [ -d ~/.home/home-core/.config/bash ]; then
	export BASH_CONFIG_DIR=~/.home/home-core/.config/bash;
elif [ -d ~/.patrick-home/home-core/.config/bash ]; then
	export BASH_CONFIG_DIR=~/.patrick-home/home-core/.config/bash;
else
	export BASH_CONFIG_DIR=${HOME};
fi;
#echo $BASH_CONFIG_DIR;

##########################################################
# check the window size after each command and, if necessary,
# update the values of LINES and COLUMNS.
shopt -s checkwinsize


##########################################################
# Pretty bash prompts

#source ${BASH_CONFIG_DIR}/prompt.fade1
#source ${BASH_CONFIG_DIR}/prompt.fade2
source ${BASH_CONFIG_DIR}/prompt.fade3
#source ${BASH_CONFIG_DIR}/prompt.puphpet

#source ~/bin/tools.sh


##########################################################
# Alias definitions.
# You may want to put all your additions into a separate file like
# ~/.bash_aliases, instead of adding them here directly.
# See /usr/share/doc/bash-doc/examples in the bash-doc package.
if [ -f ${BASH_CONFIG_DIR}/bash.aliases ]; then
	. ${BASH_CONFIG_DIR}/bash.aliases
#	echo "using BASH_CONFIG_DIR to load aliases";
elif [ -f ~/.bash_aliases ]; then
	. ~/.bash_aliases
#	echo 'using ~/.bash_aliases';
fi

##########################################################
if [ -f ${BASH_CONFIG_DIR}/bash.env ]; then
	. ${BASH_CONFIG_DIR}/bash.env
#	echo "using BASH_CONFIG_DIR to load env";
elif [ -f ~/.bash_env ]; then
	. ~/.bash_env
#	echo ' using ~/.bash_env';
fi

##########################################################
# enable programmable completion features (you don't need to enable
# this, if it's already enabled in /etc/bash.bashrc and /etc/profiles
# sources /etc/bash.bashrc).
if [ -f /etc/bash_completion ]; then
	. /etc/bash_completion
fi


