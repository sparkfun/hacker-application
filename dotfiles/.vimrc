" ===============================================================
" Desc: Required (For the sake of sanity)
" ===============================================================

set nocompatible                    " use vim improved
set autoread                        " autoread files when modified externally
set noeb vb t_vb=                   " disable all sounds and screen flashes
set modelines=0                     " disable modelines (security issue)
set history=1000                    " increase the number of lines to remember
set ttyfast                         " improved redraw performance

" enable mouse support for terminal
set mouse=a
set ttymouse=xterm2

" enable ms-win key bindings (cut/copy/paste)
source $VIMRUNTIME/mswin.vim

" ===============================================================
" Desc: Education (Back to school fix those bad habits)
" ===============================================================

" for shame if using arrow keys
function! ArrowTraining()
    inoremap <up> <nop>
    inoremap <down> <nop>
    inoremap <left> <nop>
    inoremap <right> <nop>
    nnoremap <up> <nop>
    nnoremap <down> <nop>
    nnoremap <left> <nop>
    nnoremap <right> <nop>
endfunction

command ArrowTraining call ArrowTraining()

" ===============================================================
" Desc: Vundle (Proper plugin management)
" ===============================================================

filetype off                        " !!REQUIRED!! (vundle)
set rtp+=~/.vim/bundle/vundle/
call vundle#rc()

" let vundle manage vundle          " !!REQUIRED!! (vundle)
Bundle 'gmarik/vundle'

" original repos on github
Bundle 'SirVer/ultisnips',
Bundle 'sjl/gundo.vim.git',
Bundle 'mapbox/carto', {'rtp': 'build/vim-carto/'}
Bundle 'scrooloose/syntastic.git'
Bundle 'scrooloose/nerdtree.git'
Bundle 'tpope/vim-fugitive.git'
Bundle 'tpope/vim-surround.git'
Bundle 'jelera/vim-javascript-syntax.git'
Bundle 'fholgado/minibufexpl.vim.git'
Bundle 'nvie/vim-flake8.git'
Bundle 'groenewege/vim-less.git'
Bundle 'jeffkreeftmeijer/vim-numbertoggle.git'
Bundle 'mbadran/headlights'
Bundle 'mileszs/ack.vim.git'
Bundle 'skammer/vim-css-color.git'
Bundle 'itchyny/lightline.vim'
Bundle 'mustache/vim-mustache-handlebars'
Bundle 'plasticboy/vim-markdown'
Bundle 'greyblake/vim-preview'
Bundle 'leafgarland/typescript-vim'
Bundle 'elzr/vim-json'
" vim-scripts repos on github
Bundle 'wombat256.vim'
Bundle 'python.vim'
Bundle 'JSON.vim'
Bundle 'openscad.vim'

" enable filetype plugin
filetype plugin on                  " !!REQUIRED!! (vundle)
filetype indent on                  " !!REQUIRED!! (vundle)

" ===============================================================
" Desc: Plugin Options (Tweak till your heart is content)
" ===============================================================

" syntastic global options
let g:syntastic_enable_signs=1
let g:syntastic_auto_loc_list=1
"let g:syntastic_auto_jump=1

" run flake8 on python files when saving
autocmd BufWritePost *.py call Flake8()

" set json file type on read/create
autocmd BufRead,BufNewFile *.json set filetype=json

" set openscad file type on read/create
autocmd BufRead,BufNewFile *.scad set filetype=openscad

" ===============================================================
" Desc: Remaps and Custom Commands (Momma didnt raise no fool)
" ===============================================================

" disable syntax concealing
" let g:vim_json_syntax_conceal = 0

" use sane regexes
nnoremap / /\v
vnoremap / /\v

" change map leader key
let mapleader = ","

" make Y behave (copy from cursor to eol not full line)
map Y y$

" mode commands made easy (save them key strokes)
nnoremap ; :

" improved movement on wrapped lines
nnoremap j gj
nnoremap k gk

" ,T - retab and format file with spaces
nnoremap <leader>T :set expandtab<cr>:retab!<cr>

" ,W - strip all trailing whitespaces
nmap <leader>W :call Preserve("%s/\\s\\+$//e")<CR>

" ,I - fix all indentation on the file
nmap <leader>I :call Preserve("normal gg=G")<CR>

" w!! - forget sudo on file open (improved over tee)
cnoremap w!! w !sudo dd of=%

" <esc><esc> - remove highlights
nnoremap <silent> <Esc><Esc> :noh<CR>

" <F2> - toggle line numbers and column fold for easy copying
nnoremap <F2> :set nonumber!<CR>:set foldcolumn=0<CR>

" <F5> - toggle gundo plugin
nnoremap <F5> :GundoToggle<CR>

" <F12> - toggle between 'paste' and 'nopaste'
set pastetoggle=<F12>

" ctrl-n - toggle nerdtree panel
nmap <silent> <c-n> :NERDTreeToggle<CR>

" ctrl-l - toggle absolute numbers
let g:NumberToggleTrigger="<c-l>"

" highlight word at cursor position
nnoremap <leader>h *<C-O>

" disable default folding for plasticboy markdown plugin
let g:vim_markdown_folding_disabled=1

" highlight word at cursor and ack for it
nnoremap <leader>H *<C-O>:AckFromSearch!<CR>

" ,w - split the window and move into the split
nnoremap <leader>w <C-w>v<C-w>l

" ctrl-h,j,k,l - split navigation made easy
nnoremap <C-h> <C-w>h
nnoremap <C-j> <C-w>j
nnoremap <C-k> <C-w>k
nnoremap <C-l> <C-w>l

" simple function to save the current state
function! Preserve(command)
  " save last search, and cursor position.
  let _s=@/
  let l = line(".")
  let c = col(".")
  " execute your command
  execute a:command
  " restore previous search history, and cursor position
  let @/=_s
  call cursor(l, c)
endfunction

" ===============================================================
" Desc: Custom Handling (It's my way or the highway)
" ===============================================================

" enable modern searching
set ignorecase                      " ignore case while searching
set smartcase                       " except when using capital letters
set incsearch                       " activate search as typing
set hlsearch                        " highlight searched items

" stop certain movements from going to first char of line
set nostartofline

" instead of failing ask the user for confirmation
set confirm

" apply substitutions globally on a line
set gdefault

" enable command line completion similar to bash
set wildmenu
set wildmode=list:longest
set wildignore=*.swp,*.bak,*pyc

" go to last cursor position on file open, do not when writing a commit log entry
autocmd BufReadPost * call SetCursorPosition()
function! SetCursorPosition()
    if &filetype !~ 'commit\c'
        if line("'\"") > 0 && line("'\"") <= line("$")
            exe "normal! g`\""
            normal! zz
        endif
    end
endfunction

" ===============================================================
" Desc: File Settings (For sensible defaults)
" ===============================================================

set ffs=unix,dos,mac                " set default file types order
set encoding=utf8                   " set encoding for display use
try
    lang en_US                      " set language for display use
catch
endtry

" ===============================================================
" Desc: Visual (Hey there good looking)
" ===============================================================

set laststatus=2                    " always display the status bar
set ruler                           " always show current position
set number                          " show line number
set nuw=6                           " show line number with 6 width spacing
set showmatch                       " show matching brackets indicator when over them
set showcmd                         " display incomplete cmds at bottom
set showmode                        " display current mode at bottom
set hidden                          " hide buffers when not displayed
set t_Co=256                        " enable 256 colors in vim

" handle long lines properly
set wrap
set textwidth=80
set formatoptions=qrn1

" resize splits when vim window is resized
au VimResized * exe "normal! \<c-w>="

" set colorscheme if it exists
try
    colorscheme wombat256mod
catch /^Vim\%((\a\+)\)\=:E185/
endtry

" highlight trailing whitespaces in red, but not while typing
highlight ExtraWhitespace ctermbg=red guibg=red
match ExtraWhitespace /\s\+$/
autocmd BufWinEnter * match ExtraWhitespace /\s\+$/
autocmd InsertEnter * match ExtraWhitespace /\s\+\%#\@<!$/
autocmd InsertLeave * match ExtraWhitespace /\s\+$/
autocmd BufWinLeave * call clearmatches()

" set the 80-char column a different color
if exists('+colorcolumn')
  highlight ColorColumn guibg=#2d2d2d ctermbg=grey
  set colorcolumn=80,120
else
  au BufWinEnter * let w:m2=matchadd('ErrorMsg', '\%>80v.\+', -1)
endif

" Set title string and apply it to xterm/screen window title
set titlestring=vim\ %<%F%(\ %)%m%h%w%=%l/%L-%P
set titlelen=70
if &term == "screen"
  set t_ts=k
  set t_fs=\
endif
if &term == "screen" || &term == "xterm"
  set title
endif

" ===============================================================
" Desc: Syntax and Indenting (The simple things in life)
" ===============================================================

set ai                              " autoindent
set shiftwidth=4                    " use < and > keys in visual marking mode to block indent/unindent
set tabstop=4                       " set tab stop to 4 chars
set expandtab                       " change tab into spaces automaticaly
set softtabstop=4                   " set softtab stop to 4 char
set backspace=indent,eol,start      " allow backspacing over everything in insert mode

" override the defaults based on file type
augroup filetype_tab_settings
    au!
    autocmd FileType html setlocal sw=2 sts=2 ts=2 et
    autocmd FileType xml setlocal sw=2 sts=2 ts=2 et
    autocmd FileType xhtml setlocal sw=2 sts=2 ts=2 et
    autocmd FileType css setlocal sw=2 sts=2 ts=2 et
    autocmd FileType json setlocal formatoptions=tcq2l
    autocmd FileType json setlocal sw=2 sts=2 ts=8 et
    autocmd FileType json setlocal foldmethod=syntax
augroup END

" ===============================================================
" Desc: Status Line w Lightline (If you dont know now you know)
" ===============================================================

let g:lightline = {
    \ 'colorscheme': 'wombat',
    \ }
