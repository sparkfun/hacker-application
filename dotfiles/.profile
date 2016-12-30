export PATH=/usr/local/bin:/usr/local/sbin:$PATH

alias restart="sudo apachectl -k restart";
alias stop="sudo apachectl -k stop";
alias start="sudo apachectl -k start";
alias log="tail -f /usr/local/var/log/apache2/error_log";
alias conf="open -e /usr/local/etc/apache2/2.4/httpd.conf";
alias mysqlstart='mysql.server start';
alias mysqlstop='mysql.server stop';
alias vhosts='open -e /usr/local/etc/apache2/2.4/extra/httpd-vhosts.conf';
alias profile='open ~/.profile';
alias sites='cd ~/sites';

function gitexport() {
  git archive --format zip --output "$1.zip" master -0
}

function makekey() {
  ssh-keygen -t rsa -b 4096 -C "$1"
}

function makecert() {
  openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout $1.key -out $1.crt
}

# Colors
BLACK="\[\e[0;30m\]"
BLUE="\[\e[1;34m\]"
GREEN="\[\e[0;32m\]"
LIME="\[\e[1;32m\]"
CYAN="\[\e[0;36m\]"
ORANGE="\[\e[1;31m\]"
RED="\[\e[0;31m\]"
PURPLE="\[\e[0;35m\]"
BROWN="\[\e[0;33m\]"
WHITE="\[\e[1;37m\]"
ENDCOLOR="\[\e[m\]"

parse_git_branch() {
  git branch 2> /dev/null | sed -e '/^[^*]/d' -e 's/* \(.*\)/ \(\1\)/'
}

PS1="$GREEN($LIME\w$GREEN)$PURPLE\$(parse_git_branch) $WHITE\u$GREEN> $ENDCOLOR"
[[ -s "$HOME/.rvm/scripts/rvm" ]] && source "$HOME/.rvm/scripts/rvm" # Load RVM into a shell session *as a function*
