[[ -s "$HOME/.profile" ]] && source "$HOME/.profile" # Load the default .profile
#Ruby version manager
[[ -s "$HOME/.rvm/scripts/rvm" ]] && source "$HOME/.rvm/scripts/rvm" # Load RVM into a shell session *as a function*

#aliases
alias ls="ls -al"
alias cd..="cd .."
alias notebook="jupyter notebook ~/code/notebooks/"
alias projects="cd ~/projects"

#Bash prompt customizations. I like to see the date/time and the path location
#on one line and the user and prompt on another line. Each has a unique color.
#Carriage returns separate each command execution so I can easily read a busy
#terminal.
dateColor=$(tput setaf 6)
userColor=$(tput setaf 202)
locationColor=$(tput setaf 255)
promptColor=$(tput setaf 196)
reset=$(tput sgr0)
PS1="\n\[$dateColor\]\d, \@ \[$reset\]\[$locationColor\]\w\[$reset\]\n\[$userColor\]\u \[$reset\]\[$promptColor\]$ \[$reset\]";
export PS1;

#Used to make git bash_completion to work
if [ -f `brew --prefix`/etc/bash_completion ]; then
    . `brew --prefix`/etc/bash_completion
fi
