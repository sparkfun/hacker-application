
PATH=$PATH:$HOME/.rvm/bin # Add RVM to PATH for scripting
[[ -s "$HOME/.rvm/scripts/rvm" ]] && source "$HOME/.rvm/scripts/rvm" 

# ALIASES
alias bashrc='source ~/.bashrc' # reload bashrc

alias v='vim'
alias ..='cd ..;ls'
alias ...='cd ../../;ls'

alias la='ls -a'
alias ls='ls -aF --color=always' # color and filetype
alias lx='ls -lXB' # sort by extension
alias lt='ls -ltr' # sort by date

alias glog='git log --name-status'
alias gs='git status'

# ALIASES -- PYTHON/DJANGO DEVELOPMENT 
alias seba='source env/bin/activate'
