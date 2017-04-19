syntax on "syntax highlighting on
set history=500 "default history=20
set background=dark

"Shift key fixes
cmap W w                       
cmap WQ wq
cmap wQ wq
cmap Q q
cmap Tabe tabe

" ---- SETTINGS ----

set incsearch
set number
"Default TAB Stops and Indenting
set tabstop=4
set softtabstop=4
set shiftwidth=4
set autoindent
set smartindent
set expandtab
set smarttab
"To avoid indenting code pastes twice
set pastetoggle=<F12>

" ---- REMAPS ----

"Makes F1 Escape instead of help
inoremap <F1> <ESC>
nnoremap <F1> <ESC>
vnoremap <F1> <ESC>
"disable the arrow keys while in normal mode 
nnoremap <up> <nop>
nnoremap <down> <nop>
nnoremap <left> <nop>
nnoremap <right> <nop>
inoremap <up> <nop>
inoremap <down> <nop>
inoremap <left> <nop>
inoremap <right> <nop>
nnoremap j gj
nnoremap k gk
"don't have to hit shift to save ; works too
nnoremap ; :
"Get out of insert mode quicker
:imap kk <Esc>
"Paste at beginning/end of line
nmap m ^hp 
nmap , $p

" ---- SYNTAX HIGHLIGHTING ----

"Brute Force CSS highlighting for Less
au BufRead,BufNewFile *.less set filetype=css

"Coffeescript Hilighting
au BufRead,BufNewFile *.coffee set filetype=coffee
au! Syntax coffee source /home/austin/.vim/syntax/coffee.vim

