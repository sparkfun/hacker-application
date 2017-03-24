#I use zsh as my shell and the oh-my-zsh plugin for the incredible ammount of shortcuts and styling options.

# Path to my oh-my-zsh installation.
export ZSH=/Users/stephenhanzlik/.oh-my-zsh

# Set name of the theme to load.
ZSH_THEME="robbyrussell"

# Using the git plugin to give better graphical insight into git
plugins=(git)

source $ZSH/oh-my-zsh.sh

# User configuration
# Setting atom as defualt text editor,  open atom if commit message is forgotten
export EDITOR='atom -w'

# I am using node version manager
export NVM_DIR="/Users/stephenhanzlik/.nvm"
[ -s "$NVM_DIR/nvm.sh" ] && . "$NVM_DIR/nvm.sh"  # This loads nvm

#virtual env for playing around with python
export WORKON_HOME=~/.virtualenvs
source /usr/local/bin/virtualenvwrapper.sh
