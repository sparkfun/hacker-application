# PS1 Prompt files
The files in this directory named ``prompt.*`` modify the PS1 prompt.

The fade series I actually got from Michael Johnson when I was at
Technological Fluency Institute (a division of Pitsco). I have since heavily
modified the ``prompt.fade3`` as it's my preferred scheme overall. I don't
remember if the original version did the color change to red for the u@h part
if the user is root (even works with sudo) or if that was something I added in
later.

The ``prompt.puphpet`` is a version pulled from the
[puphpet project](https://puphpet.com/)
at some point. I pulled the ``__vcs_name()`` function from here and added it
to the ``prompt.fade3`` that I normally use.

# Bash configs

There are 2 files in here named ``bash.aliases`` & ``bash.env`` which are
typically located and named ``~/.bash_aliases`` and ``~/.bash_env``, but I
decided to move them into ``~/.config/bash/`` in order to try to keep ``~``
cleaner.

# Dircolors

If you're not familiar with ``dircolors`` it's what causes files with different
file extensions to be colored in different ways via ``ls --color=auto``, etc.
This uses ANSI colors which you can find outputs for from my
``~/bin/ansi_colors`` shell script.

Color support is loaded in via my ``~/.config/bash/bash.aliases`` file.

# INPUTRC

This is normally ``~/.inputrc``, but again I moved it here to clean up ``~``.
This will let you fix certain keybinds for certain connections (like PuTTY)
and the variable is exported via ``~/.config/bash/bash.env``.
