My dotfiles are stored on Github and deployed using [homeshick](https://github.com/andsens/homeshick.git).

For information regarding requirements and installation instructions see the following link [https://github.com/h0st1le/dotfiles](https://github.com/h0st1le/dotfiles)

### Brief overview of my current dotfiles:

#### .gitconfig

Simple .gitconfig file including the most basic of default settings

#### .gitignore:

Global .gitignore file, containing only 'vim' specific files. (not everyone uses vim, prevents pollution of project specific .gitignore's)

#### .zshrc

I prefer to use [zsh](http://www.zsh.org/) for my shell, and [oh-my-zsh](https://github.com/robbyrussell/oh-my-zsh) for managing it. This config sources 'oh-my-zsh' and adds additional options for SSH agents, homeshick, NVM, RVM, Heroku, and any additional development resources as needed.

#### .vimrc

I tend to use vim for most of my text editing on both my local machine and on servers. It is usually already installed, allows for advanced customization, and historically proven. I keep meaning to spend some time with [neovim](https://github.com/neovim/neovim) but have not yet gotten around to it.

#### .gvimrc

Separate config file for gvim (graphic) specific settings.

#### .jshintc

Configuration file for [jshint](https://github.com/jshint/jshint), a Javascript analysis tool used by vim.
