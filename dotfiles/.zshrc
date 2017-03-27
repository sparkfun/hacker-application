#############################################################
# oh-my-zsh cheatsheet
# https://github.com/robbyrussell/oh-my-zsh/wiki/Cheatsheet


#############################################################
# Path to your oh-my-zsh installation.
export ZSH=/Users/robk/.oh-my-zsh


#############################################################
# Set name of the theme to load.
# Look in ~/.oh-my-zsh/themes/
# Optionally, if you set this to "random", it'll load a random theme each
# time that oh-my-zsh is loaded.
# ZSH_THEME="robbyrussell"
# ZSH_THEME="ys"
# ZSH_THEME="honukai"

# Agnoster is what I have been using for over a year now
# colorful, special fonts, git integration, etc
# requires install of powerline font
# instructions: https://github.com/robbyrussell/oh-my-zsh/wiki/themes
# note: you will need to edit some colors in your terminal - eg: selected highlights
ZSH_THEME="agnoster"


#############################################################
# ZSH Config

# Uncomment the following line to use case-sensitive completion.
# CASE_SENSITIVE="true"

# Uncomment the following line to use hyphen-insensitive completion. Case
# sensitive completion must be off. _ and - will be interchangeable.
# HYPHEN_INSENSITIVE="true"

# Uncomment the following line to disable bi-weekly auto-update checks.
# DISABLE_AUTO_UPDATE="true"

# Uncomment the following line to change how often to auto-update (in days).
# export UPDATE_ZSH_DAYS=13

# Uncomment the following line to disable colors in ls.
# DISABLE_LS_COLORS="true"

# Uncomment the following line to disable auto-setting terminal title.
# DISABLE_AUTO_TITLE="true"

# Uncomment the following line to enable command auto-correction.
# ENABLE_CORRECTION="true"

# Uncomment the following line to display red dots whilst waiting for completion.
# COMPLETION_WAITING_DOTS="true"

# Uncomment the following line if you want to disable marking untracked files
# under VCS as dirty. This makes repository status check for large repositories
# much, much faster.
# DISABLE_UNTRACKED_FILES_DIRTY="true"

# Uncomment the following line if you want to change the command execution time
# stamp shown in the history command output.
# The optional three formats: "mm/dd/yyyy"|"dd.mm.yyyy"|"yyyy-mm-dd"
# HIST_STAMPS="mm/dd/yyyy"

# Would you like to use another custom folder than $ZSH/custom?
# ZSH_CUSTOM=/path/to/new-custom-folder


#############################################################
# PLUGINS
# https://github.com/robbyrussell/oh-my-zsh/wiki/Plugins
# Which plugins would you like to load? (plugins can be found in ~/.oh-my-zsh/plugins/*)
# Custom plugins may be added to ~/.oh-my-zsh/custom/plugins/
# Example format: plugins=(rails git textmate ruby lighthouse)
# Add wisely, as too many plugins slow down shell startup.
plugins=(brew git)


#############################################################
# USER CONFIG

DEFAULT_USER="robk"

export PATH="/usr/local/bin:/usr/bin:/bin:/usr/sbin:/sbin"

# Android env vars, etc
# https://facebook.github.io/react-native/releases/0.23/docs/android-setup.html
export ANDROID_HOME=~/Library/Android/sdk
# NO!!!! DON'T DO THIS!!!! export JAVA_HOME=/usr/bin/java

# This loads nvm
export NVM_DIR="/Users/robk/.nvm"
[ -s "$NVM_DIR/nvm.sh" ] && . "$NVM_DIR/nvm.sh"

# usage >source .zshrc
source $ZSH/oh-my-zsh.sh

# You may need to manually set your language environment
# export LANG=en_US.UTF-8

# Preferred editor for local and remote sessions
# if [[ -n $SSH_CONNECTION ]]; then
#   export EDITOR='vim'
# else
#   export EDITOR='mvim'
# fi

# Compilation flags
# export ARCHFLAGS="-arch x86_64"

# ssh - more on how to - https://mattstauffer.co/blog/setting-up-a-new-os-x-development-machine-part-3-dotfiles-rc-files-and-ssh-config
# export SSH_KEY_PATH="~/.ssh/dsa_id"


#############################################################
# ALIASES
# Set personal aliases, overriding those provided by oh-my-zsh libs,
# plugins, and themes. Aliases can be placed here, though oh-my-zsh
# users are encouraged to define aliases within the ZSH_CUSTOM folder.
# For a full list of active aliases, run `alias`.
#
# Example aliases
# alias zshconfig="mate ~/.zshrc"
# alias ohmyzsh="mate ~/.oh-my-zsh"
# alias vscode="/Applications/Visual\ Studio\ Code.app"
alias webs='open $@ -a "/Applications/Webstorm.app"'
alias code='open $@ -a "Visual Studio Code"'
alias dco="docker-compose"
alias dcor="docker-compose run"

# NOT AN ALIAS!!
# but to find and kill port processes (list open files)
# lsof -n -i4TCP:[portNumber] - gets a list of processes using specified port
# kill -9 [processNumber]  (-9 means non-catchable,non-ignorable kill)

# tempted to make a function/alias for mkdir and cd?
# don't rewrite the wheel - use *take*
# take [newfolder] will create and cd you into [newfolder]


#############################################################
# FUNCTIONS
# FileSearch - http://sourabhbajaj.com/mac-setup/iTerm/zsh.html
function f() { find . -iname "*$1*" ${@:2} }
function r() { grep "$1" ${@:2} -R . }
