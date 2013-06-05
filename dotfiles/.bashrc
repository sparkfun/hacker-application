#IT ISN'T CLEAN, DON'T H8

#~/.bashrc: executed by bash(1) for non-login shells.
# see /usr/share/doc/bash/examples/startup-files (in the package bash-doc)
# for examples

# If not running interactively, don't do anything
[ -z "$PS1" ] && return

# don't put duplicate lines in the history. See bash(1) for more options
# ... or force ignoredups and ignorespace
HISTCONTROL=ignoredups:ignorespace

# append to the history file, don't overwrite it
shopt -s histappend

# maxium available space for storing history
# HISTSIZE=2**31-1
# HISTFILESIZE=2**31-1
# this for some reason is not working as expected

HISTSIZE=999999
HISTFILESIZE=999999
HISTCONTROL=ignoredups:erasedups
#colorful date
HISTTIMEFORMAT="[$(tput setaf 6)%F %T$(tput sgr0)]: "

# helpful for filtering based on date
HISTTIMEFORMAT='%F %T '

# check the window size after each command and, if necessary,
# update the values of LINES and COLUMNS.
shopt -s checkwinsize

# make less more friendly for non-text input files, see lesspipe(1)
[ -x /usr/bin/lesspipe ] && eval "$(SHELL=/bin/sh lesspipe)"

# set variable identifying the chroot you work in (used in the prompt below)
if [ -z "$debian_chroot" ] && [ -r /etc/debian_chroot ]; then
    debian_chroot=$(cat /etc/debian_chroot)
fi

# set a fancy prompt (non-color, unless we know we "want" color)
case "$TERM" in
    xterm-color) color_prompt=yes;;
esac

# uncomment for a colored prompt, if the terminal has the capability; turned
# off by default to not distract the user: the focus in a terminal window
# should be on the output of commands, not on the prompt
#force_color_prompt=yes

if [ -n "$force_color_prompt" ]; then
    if [ -x /usr/bin/tput ] && tput setaf 1 >&/dev/null; then
	# We have color support; assume it's compliant with Ecma-48
	# (ISO/IEC-6429). (Lack of such support is extremely rare, and such
	# a case would tend to support setf rather than setaf.)
	color_prompt=yes
    else
	color_prompt=
    fi
fi

if [ "$color_prompt" = yes ]; then
    PS1='${debian_chroot:+($debian_chroot)}\[\033[01;32m\]\u@\h\[\033[00m\]:\[\033[01;34m\]\w\[\033[00m\]\$ '
else
    PS1='${debian_chroot:+($debian_chroot)}\u@\h:\w\$ '
fi
unset color_prompt force_color_prompt

# If this is an xterm set the title to user@host:dir
case "$TERM" in
xterm*|rxvt*)
    PS1="\[\e]0;${debian_chroot:+($debian_chroot)}\u@\h: \w\a\]$PS1"
    ;;
*)
    ;;
esac

# enable color support of ls and also add handy aliases
if [ -x /usr/bin/dircolors ]; then
    test -r ~/.dircolors && eval "$(dircolors -b ~/.dircolors)" || eval "$(dircolors -b)"
    alias ls='ls --color=auto'
    #alias dir='dir --color=auto'
    #alias vdir='vdir --color=auto'

    alias grep='grep --color=auto'
    alias fgrep='fgrep --color=auto'
    alias egrep='egrep --color=auto'
fi

# some more ls aliases
alias ll='ls -alF'
alias la='ls -A'

# Add an "alert" alias for long running commands.  Use like so:
#   sleep 10; alert
alias alert='notify-send --urgency=low -i "$([ $? = 0 ] && echo terminal || echo error)" "$(history|tail -n1|sed -e '\''s/^\s*[0-9]\+\s*//;s/[;&|]\s*alert$//'\'')"'

# Alias definitions.
# You may want to put all your additions into a separate file like
# ~/.bash_aliases, instead of adding them here directly.
# See /usr/share/doc/bash-doc/examples in the bash-doc package.

if [ -f ~/.bash_aliases ]; then
    . ~/.bash_aliases
fi

# enable programmable completion features (you don't need to enable
# this, if it's already enabled in /etc/bash.bashrc and /etc/profile
# sources /etc/bash.bashrc).
if [ -f /etc/bash_completion ] && ! shopt -oq posix; then
    . /etc/bash_completion
fi

#QTDIR
#KDE_SRC=/home/heath/kde/src
#KDE_BUILD=/home/heath/kde/build/
#KDEDIR=/home/heath/kde/inst/
#KDEHOME=/home/heath/kde/home

freq() { sort "$1" | uniq -c | sort -nr | less; }

[[ -s "$HOME/.rvm/scripts/rvm" ]] && . "$HOME/.rvm/scripts/rvm" # Load RVM function

PYTHONPATH=/usr/local/lib/:$PYTHONPATH
#! /bin/bash

alias "oslssh"="ssh -l diysci 69.163.39.14"
alias "oslftp"="sftp diysci@69.163.39.14"
alias "dpkg"="sudo dpkg"
alias "apt-get"="sudo apt-get"
alias "install"="sudo apt-get install"
alias "update"="sudo apt-get update"
alias "remove"="sudo apt-get remove"
alias "search"="apt-cache search"
alias "apt-add-repository"="sudo apt-add-repository"
alias "g"="grep -i -n -r --color=auto"
alias "gvend"="grep -i -n -r --color=auto --exclude-dir *vendor*"
alias "h"="history"
alias "bp"="bpython"
alias "ip"="ipython"
alias "ht"="htop"
alias "grab"="wget -e robots=off --no-parent --random-wait --user-agent='Mozilla/5.0 (X11; U; Linux i686; en-US; rv: 1.9.0.3) Gecko/200809216 Firefox/3.0.3'"
alias "hci"="runhaskell Setup clean && runhaskell Setup configure --prefix=$HOME --user && runhaskell Setup build && runhaskell Setup install"
alias "lsau"="lsof /dev/dsp* /dev/audio* /dev/mixer* /dev/snd*"
alias "webcamview"="mplayer tv:// -tv driver=v4l2:width=640:height=480:device=/dev/video0
"
alias "e"="emacs -nw"
alias "em"="emacs -bg black -fg white"
alias "mv"="mv -i"
alias "ws"="cd /home/heath/Local/warsow_1.0/ && ./warsow"
alias "v"="vim"
alias "vi"="vim"
alias "thesaurus"="aiksaurus"
alias "uzbl"="uzbl-browser"

# i have a fun alarm in the mornings
alias "km"="killall mplayer"

alias "gdebi"="sudo gdebi"
alias "updatedb"="sudo updatedb"
alias "lst"="ls -t | less"
alias "reboot"="sudo reboot"
alias "gnusha"="ssh -l ybit 131.252.130.248"
alias "wcgo"="mplayer tv:// -tv driver=v4l2:width=640:height=480:device=/dev/video0"
alias "tree"="ls -R | grep ':$' | sed -e 's/:$//' -e 's/[^-][^\/]*\//--/g' -e 's/^/   /' -e 's/-/|/'"
alias "gpo"="git push origin"
alias "commit"="git commit -m "
alias "rcon"="rails console"
alias "rserv"="rails server -u -p 4000"
alias "gitstart"="git init && git add . && git commit -m 'initial commit'"
alias 'rdu'='for each in $(ls); do du -sb "$each"; done | sort -n'

#rake db:seed rake db:reset also populates
alias 'hard_reset'='bundle exec rake db:drop db:create db:migrate db:seed db:test:prepare'
alias 'be'='bundle exec'

#alias "rm -rf"="rm -rfi"

alias "vb"="vim ~/.bashrc"
alias "vr"="vim ~/.vimrc"
alias "vundle"="vim +BundleInstall +qall"

#disable terminal flow control
stty -ixon

#use vim as a syntax highlighting pager
alias vless="vim -u /usr/share/vim/vim73/macros/less.vim"
EMBEDLY_KEY="a80d8fc49a1e11e19fef4040aae4d8c9"

alias trash="mv -t ~/.local/share/Trash/files --backup=t --verbose"
alias sr="screen -rd"
alias remove-swaps="find . -iname *.swp -exec rm -i '{}' \;"
export KDEDIRS=$KDEDIRS:$HOME/Local/oyranos/inst

# git aliases
alias gb="git branch"
alias gc="git commit -v"

alias jshintrc="vim ~/.jshintrc"

# get a decent color scheme when viewing files with root privs
alias cm="cd /home/heath/Devel/i11/canvas-magnet"
alias mayhem="cd /home/heath/Devel/i11/Mayhem/public/draggable-items-demo/"

# you'll never go back after you try this
alias "1.."="cd .."
alias "2.."="cd ../.."
alias "3.."="cd ../../.."
alias "4.."="cd ../../../.."
alias "5.."="cd ../../../../.."
alias "6.."="cd ../../../../../.."
alias "7.."="cd ../../../../../../.."

xset r rate 150 110
alias 'heath'='xset r rate 145 95'

alias 'l'='less'

shopt -s autocd

alias "checkForGlobals"="g 'window\..*\s*\=' *"

# alias 'node'='env NODE_NO_READLINE=1 rlwrap -p Green -S "node >>> " node'
NODE_PATH=/usr/local/lib/node_modules/:/home/heath/.npm/
export CHROMIUM_ROOT=/home/heath/Local/chromium/src/
PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin:/home/heath/.cabal/bin:/home/heath/Local/bin:/usr/bin/:/home/heath/Local/phantomjs/bin
export PATH="$PATH:/home/heath/Local/android-sdk2/android-sdk-linux/platform-tools:/usr/local/bin/:/home/heath/Local/depot_tools:/home/heath/.cabal-dev/bin/:/home/heath/.rvm/bin:/usr/local/heroku/bin"
JAVA_HOME="/usr/lib/jvm/java-6-openjdk-amd64/jre/"
PATH=$PATH:/home/heath/.Renv/bin:/home/heath/$JAVA_HOME/bin
# that's a lot of PATH, don't you think?

# tmux, i've tried it
PS1="$PS1"'$([ -n "$TMUX" ] && tmux setenv TMUXPWD_$(tmux display -p "#D" | tr -d %) "$PWD")'

# i like to be efficient
countdown () { MIN=$1; for ((i=$((MIN*60));i>=0;i--));do echo -ne "\r$(date -d"0+$i sec" +%H:%M:%S)";sleep 1;done;}

TEST_DRIVER=chrome

alias "lsg"="ls --group-directories-first"
alias "bsb"="cd /home/heath/busysail_backend"
alias "bsf"="cd /home/heath/busysail_frontend"

alias "eclipse"="cd /home/heath/Local/eclipse/eclipse && ./eclipse &&"

source /usr/local/bin/virtualenvwrapper.sh
source /home/heath/.django_bash_completion.sh

alias 'd'='workon d'
alias 't'='ipython'
alias 'dt'='workon d && ipython'
alias 'nb'='ipython notebook --pylab'

source /home/heath/Local/sandboxer/sandboxer.sh
alias 's'='sandboxer'
sandboxer activate d

source ~/Local/nvm/nvm.sh

PATH=$PATH:$HOME/.rvm/bin # Add RVM to PATH for scripting

source "$HOME/.rvm/scripts/rvm" 

# {{{
# Node Completion - no touchy
shopt -s progcomp
for f in $(command ls ~/.node-completion); do
  f="$HOME/.node-completion/$f"
  test -f "$f" && . "$f"
done
# }}}
