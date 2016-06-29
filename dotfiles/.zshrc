# Path to oh-my-zsh configuration.
ZSH=$HOME/.oh-my-zsh

# Set name of the theme to load.
# Look in ~/.oh-my-zsh/themes/
# Optionally, if you set this to "random", it'll load a random theme each
# time that oh-my-zsh is loaded.
ZSH_THEME="candy"

# thefuck alias, for correcting previous console commands
alias fuck='eval $(thefuck $(fc -ln -1 | tail -n 1)); fc -R'

# Example aliases
# alias zshconfig="mate ~/.zshrc"
# alias ohmyzsh="mate ~/.oh-my-zsh"

# Set to this to use case-sensitive completion
# CASE_SENSITIVE="true"

# Uncomment this to disable bi-weekly auto-update checks
# DISABLE_AUTO_UPDATE="true"

# Uncomment to change how often before auto-updates occur? (in days)
# export UPDATE_ZSH_DAYS=13

# Uncomment following line if you want to disable colors in ls
# DISABLE_LS_COLORS="true"

# Uncomment following line if you want to disable autosetting terminal title.
# DISABLE_AUTO_TITLE="true"

# Uncomment following line if you want to disable command autocorrection
# DISABLE_CORRECTION="true"

# Uncomment following line if you want red dots to be displayed while waiting for completion
# COMPLETION_WAITING_DOTS="true"

# Uncomment following line if you want to disable marking untracked files under
# VCS as dirty. This makes repository status check for large repositories much,
# much faster.
# DISABLE_UNTRACKED_FILES_DIRTY="true"

# Which plugins would you like to load? (plugins can be found in ~/.oh-my-zsh/plugins/*)
# Custom plugins may be added to ~/.oh-my-zsh/custom/plugins/
# Example format: plugins=(rails git textmate ruby lighthouse)
plugins=(git heroku)

source $ZSH/oh-my-zsh.sh

# Customize to your needs...
fpath=($HOME/.homesick/repos/homeshick/completions $fpath)
source "$HOME/.homesick/repos/homeshick/homeshick.sh"

SSH_ENV="$HOME/.ssh/environment"

# Start a new SSH agent
function start_agent {
    echo "Initialising new SSH agent..."
    /usr/bin/ssh-agent | sed 's/^echo/#echo/' > "${SSH_ENV}"
    echo succeeded
    chmod 600 "${SSH_ENV}"
    . "${SSH_ENV}" > /dev/null
    /usr/bin/ssh-add;
}

# Source SSH settings, if applicable
if [ -f "${SSH_ENV}" ]; then
    . "${SSH_ENV}" > /dev/null
    #ps ${SSH_AGENT_PID} doesn't work under cywgin #
    ps -ef | grep ${SSH_AGENT_PID} | grep ssh-agent$ > /dev/null || {
        start_agent;
    }
else
    start_agent;
fi

# RVM added to path
# PATH=$PATH:$HOME/.rvm/bin # Add RVM to PATH for scripting

# RVM settings
#if [[ -s ~/.rvm/scripts/rvm ]] ; then
#  RPS1='%{$fg[green]%}[`~/.rvm/bin/rvm-prompt`]%{$reset_color%} $EPS1'
#else
#  if which rbenv &> /dev/null; then
#    RPS1='%{$fg[green]%}[`rbenv version | sed -e "s/ (set.*$//"`]%{$reset_color%} $EPS1'
#  else
#    RPS1='$EPS1'
#  fi
#fi

# NVM added to path
export PATH=$HOME/local/bin:$PATH

export NVM_DIR="/home/jpenka/.nvm"
[ -s "$NVM_DIR/nvm.sh" ] && . "$NVM_DIR/nvm.sh"  # This loads nvm

# APM SITL development added to path
export PATH=$PATH:$HOME/Workspace/Drone/jsbsim/src
export PATH=$PATH:$HOME/Workspace/Drone/ardupilot/Tools/autotest
export PATH=/usr/lib/ccache:$PATH

# Added by the Heroku Toolbelt
export PATH="/usr/local/heroku/bin:$PATH"

# android sdk development
export PATH=$PATH:$HOME/Android/Sdk/tools
export PATH=$PATH:$HOME/Android/Sdk/platform-tools
