# .bash_profile

# Get the aliases and functions
if [ -f ~/.bashrc ]; then
	. ~/.bashrc
fi

# User specific environment and startup programs

PATH=$PATH:$HOME/.local/bin:$HOME/bin

export PATH
#####appendbashrc-start#####
export EDITOR=vim
export PS1="\n\e[1;37m[\e[0;32m\u\e[0;35m@\e[0;32m\h\e[1;37m]\e[1;37m[\e[0;31m\w\e[1;37m]\e[0m\n$ "
alias ls='ls --color=auto -F'
#####appendbashrc-stop#####
