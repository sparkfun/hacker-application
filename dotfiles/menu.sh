#!/bin/bash

while [ 1 ]
do
	echo
	echo '+---------------------------------------------------+'
        echo ' 1. Install vimrc'
        echo ' 2. Upgrade prompt, alias ls, and set EDITOR to vim'
        echo ' 3. Install screenrc'
        echo
	echo '+--------------- THESE REQUIRE ROOT ----------------+'
        echo ' 4. Install Base Packages'
	echo ' 5. Install LAMP'
        echo ' 6. Install Development Packages'
        echo ' 7. Install EPEL Repository 6.8 (RHEL6)'
        echo ' 8. Make Apache more secure'
	echo ' 9. Do 4-7 and Update'
	echo

        read -n 1 -p '0 to exit) ' CHOICE
        echo

	if [[ $CHOICE -eq 0 ]]
        then
		rm $BASH_SOURCE
                exit
        fi

        if [[ $CHOICE -eq 1 ]]
        then
                echo 'Install vimrc...'
                if [ -d ~/.vim ]
                then
                        rm -rf ~/.vimold
                        mv -v ~/.vim ~/.vimold
                fi
                mkdir -p ~/.vim/autoload
                wget -O ~/.vimrc http://www.samtorno.com/vim/vimrc
        fi

        if [[ $CHOICE -eq 2 ]]
        then
                echo 'Install screenrc...'
                if [ -f ~/.screenrc ]
                then
                        rm -rf ~/.screenrc.old
                        mv -v ~/.screenrc ~/.screenrc.old
                fi
                wget -O ~/.screenrc http://www.samtorno.com/vim/screenrc
        fi

        if [[ $CHOICE -eq 3 ]]
        then
                echo 'Upgrade Prompt...'
                #backup bash_profile
                cp -vf ~/.bash_profile ~/.bash_profile.old

                #see if we've already installed appendbashrc
                grep '#####appendbashrc-start#####' ~/.bash_profile || {
                   #no we haven't, so remove any possible conflicting lines
                   sed -ne '/^export\ EDITOR\=/d' -ne '/^export\ PS1\=/d' -ne '/^alias\ ls\=/d' -ne 'p' -i ~/.bash_profile
                } && {
                   #yes we have, so just remove the marked block
                   sed -ne '/#####appendbashrc-start#####/,/#####appendbashrc-stop#####/d' -ne 'p' -i ~/.bash_profile
                }

                #and append the new bash stuff
                wget -o /dev/null -O - >> ~/.bash_profile http://www.samtorno.com/vim/appendbashrc
        fi
                                                                                                                                            
        if [[ $CHOICE -gt 3 && $EUID -ne 0 ]]; then                                                                                       
                echo "You must be root to perform that action!"                                                                             
                continue                                                                                                                    
        fi

	if [[ $CHOICE -eq 4 || $CHOICE -eq 9 ]]
        then
                yum -y install vim vim-enhanced bridge-utils nano subversion screen openssh-clients bind-utils policycoreutils pciutils policycoreutils-python uuid nmap
        fi

	if [[ $CHOICE -eq 5 || $CHOICE -eq 9 ]]
        then
                yum -y install httpd mod_ssl php php-cli php-mysql php-mbstring php-gd php-pdo mysql-server

        fi

        if [[ $CHOICE -eq 6 || $CHOICE -eq 9 ]]
        then
                yum -y groupinstall 'Development Tools'
        fi

        if [[ $CHOICE -eq 7 || $CHOICE -eq 9 ]]
        then
                rpm -Uvh http://dl.fedoraproject.org/pub/epel/6/x86_64/epel-release-6-8.noarch.rpm
        fi                                                                                                

	if [[ $CHOICE -eq 8 ]]
	then
		sed -i -e 's/ServerTokens OS/ServerTokens Prod/' /etc/httpd/conf/httpd.conf
		sed -i -e 's/ServerSignature On/ServerSignature Off/' /etc/httpd/conf/httpd.conf
		sed -i -e '330,/<\/Directory>/s/AllowOverride None/AllowOverride All/' /etc/httpd/conf/httpd.conffi
		if [ -f /etc/httpd/conf.d/svn503.conf ]
		then
			rm -rf /etc/httpd/conf.d/svn503.conf
		fi
		wget -O /etc/httpd/conf.d/svn503.conf http://www.samtorno.com/vim/httpd503svn
		echo
		echo '+---- REMEMBER TO RESTART HTTPD ----+'
		echo
	fi

	if [[ $CHOICE -eq 9 ]]
	then
		yum -y update
	fi
done
