Experienced developers, particularly those working in a Unix environment, tend to prefer
tools which afford a measure of customization. The standard Unix approach of dotfiles to
customize editors, shells, IRC clients, and the like to the developers liking is something
we're interested in.

In this directory, please include (at your option) either:

  a) A sampling of dotfiles or other configuration from your working environment.

    I usually only put a few short cuts or path changes in my dot files. I do 
    like to alias servers in the .bashrc file such as the following. I don't 
    have a domain controller at home so these are defined in my hosts file.

    alias pi='ssh pi@pi'
    alias wifi-pi='ssh pi@wifi-pi'

    127.0.0.1	localhost
    10.0.0.2 pi
    10.0.0.3 wifi-pi

  b) A rationale for your lack of same (complete with some description of your
     preferred working environment).

     I normally use an IDE like netbeans (for php) or PyCharm (for python).
