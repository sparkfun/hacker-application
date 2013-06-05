"automatically close vim if nerdtree is last buffer
au bufenter * if (winnr("$") == 1 && exists("b:NERDTreeType") && b:NERDTreeType == "primary") | q | endif

" automatically reload vimrc when it's saved
au BufWritePost .vimrc so ~/.vimrc

" fold all /**/ comments in js, useful for code documented with docco
au FileType javascript :set fmr=/*,*/ fdm=marker fdc=1

"omni completion
au FileType ruby,eruby set omnifunc=rubycomplete#Complete

"buffer/rails/global completion
au FileType ruby,eruby let g:rubycomplete_buffer_loading = 1

"rails support
au FileType ruby,eruby let g:rubycomplete_rails = 1
au FileType ruby,eruby let g:rubycomplete_classes_in_global = 1


" remember to set the nocmpatible flag as well as filetype to off
" before running :BundleInstall or if using the cli: vim +BundleInstall +qall
"
"let vundle manage vundle
set rtp+=~/.vim/bundle/vundle
call vundle#rc()
Bundle 'gmarik/vundle'

"bundles used
Bundle 'Lokaltog/vim-easymotion'
Bundle 'Shougo/neocomplcache'
Bundle 'altercation/solarized'
Bundle 'altercation/vim-colors-solarized'
Bundle 'c9s/bufexplorer'
Bundle 'digitaltoad/vim-jade'
Bundle 'ecomba/vim-ruby-refactoring'
Bundle 'edsono/vim-matchit'
Bundle 'ervandew/supertab'
Bundle 'godlygeek/tabular'
Bundle 'guileen/vim-node'
Bundle 'int3/vim-extradite'
Bundle 'jamescarr/snipmate-nodejs'
Bundle 'jceb/vim-orgmode'
Bundle 'jeetsukumaran/vim-buffergator'
Bundle 'kchmck/vim-coffee-script'
Bundle 'kien/ctrlp.vim'
Bundle 'majutsushi/tagbar'
Bundle 'mattn/gist-vim'
Bundle 'mattn/gist-vim'
Bundle 'mileszs/ack.vim'
Bundle 'msanders/snipmate.vim'
Bundle 'nathanaelkane/vim-indent-guides'
Bundle 'pangloss/vim-javascript'
Bundle 'pangloss/vim-javascript'
Bundle 'scrooloose/nerdcommenter'
Bundle 'scrooloose/nerdtree'
Bundle 'scrooloose/syntastic'
Bundle 'sjl/splice.vim'
Bundle 'spf13/vim-colors'
Bundle 'tomtom/tcomment_vim'
Bundle 'tpope/vim-cucumber'
Bundle 'tpope/vim-fugitive'
Bundle 'tpope/vim-haml'
Bundle 'tpope/vim-markdown'
Bundle 'tpope/vim-rails'
Bundle 'tpope/vim-surround'
Bundle 'vim-scripts/Colour-Sampler-Pack'
Bundle 'vim-scripts/django.vim'
Bundle 'vim-scripts/python.vim--Vasiliev'
Bundle 'wavded/vim-stylus'
Bundle 'wookiehangover/jshint.vim'

Bundle 'git://git.wincent.com/command-t.git'

Bundle 'jsbeautify'
" Bundle 'CSApprox'
" Bundle 'vim-scripts/EvalSelection'
" Bundle 'davidhalter/jedi-vim'
" Bundle 'kevinw/pyflakes-vim'
" Bundle 'Proxino/TypedJS'
" Bundle 'myusuf3/numbers.vim'
" Bundle 'Lokaltog/vim-powerline.git'
" Bundle 'xolox/vim-session'


" Bundle 'hallettj/jslint.vim'
"ctl-p for fuzzyfinder in janus

filetype on
filetype plugin on
filetype indent on
syntax   on

"delete forward
imap <C-d> <C-[>diwi

" give me a listing of ctags w/ js support
" nmap <F8> :TagbarToggle<CR>
let g:tagbar_type_javascript = {
    \ 'ctagsbin' : '/path/to/jsctags'
\ }

let g:JSHintHighlightErrorLine = 0
let javascript_fold            = 1
let loaded_matchit             = 1
let ruby_fold                  = 1

"f10 to delete all and paste from clipboard
map <F10> VG"+p

"Met-z to save
map <M-z> :w<CR>
map <M-z> <Esc>:w<CR>i

"map <M-0> :JSLintToggle

" autoindent with two spaces, always expand tabs
" autocmd FileType ruby,eruby,yaml set ai sw=2 sts=2
map <C-l> :tabn<CR>
map <C-h> :tabp<CR>
map <C-b> g:Jsbeautify()

" http://stackoverflow.com/questions/1521501/vim-how-to-open-files-in-vertically-horizontal-split-windows
" http://vimdoc.sourceforge.net/htmldoc/scroll.html#scroll-binding

" a red line highlighter:
" hi CursorLine   cterm=NONE ctermbg=darkred ctermfg=white guibg=darkred guifg=white
" hi CursorColumn cterm=NONE ctermbg=darkred ctermfg=white guibg=darkred guifg=white
" nnoremap <Leader>c :set cursorline! cursorcolumn!<CR>
" http://vim.wikia.com/wiki/Highlight_current_line

" Jump key
" nnoremap ` '
" nnoremap ' `


nmap <F8> :set hlsearch<CR>
nmap <F9> :set nohlsearch<CR>

nmap <C-t> :tabnew .<CR>
nmap <C-c> :close<CR>

nmap <C-e> :edit .<CR>

" Toggle cursorline
nnoremap <Leader>c :set cursorline!<CR>

" hide/show numbers
nnoremap <leader>n :set nonumber<CR>
nnoremap <leader>N :set number<CR>

nnoremap <leader>h :let g:JSHintHighlightErrorLine=0<CR>
nnoremap <leader>H :let g:JSHintHighlightErrorLine=1<CR>

" wrap/nowrap
nnoremap <leader>w :set wrap<CR>
nnoremap <leader>W :set nowrap<CR>


" Load matchit (% to bounce from do to end, etc.)
runtime! macros/matchit.vim


set ai
set bg=dark
set expandtab
set foldenable
set hlsearch
set incsearch
set linebreak
set modeline
set nocompatible
set noswapfile
set nowrap
set number
" set scrollbind
set showcmd

" let backspace work in vim
set bs          =2
set laststatus  =2
set ls          =2
set shiftwidth  =2
set softtabstop =2
set backupdir   =/home/heath/.vim-autobups/
set directory   =/home/heath/.vim-swapfiles
set encoding    =utf-8
set cpoptions+=$

set foldopen =all
" set foldmethod =syntax
set foldlevelstart =20


" PATHS
set path+=/home/heath/Devel/i11/USPTO_mindmap

set statusline =%F%m%r%h%w\ [FORMAT=%{&ff}]\ [TYPE=%Y]\ [POS=%l,%v][%p%%]\ %{strftime(\"%d/%m/%y\ -\ %H:%M\")}

if exists('&undodir')
  set undodir=~/.vim/undo,.
endif

" set nobackup
" set cursorline
set nocursorcolumn
" set ignorecase
" set  wildmode =list

" VIM SCRIPTS TO CHECK OUT
" damlowe vim-slurper
" altercation vim-colors-solarized
" depuracao vim-rdoc
" godlygeek tabular
" jgdavey tslime.vim
" jgdavey vim-turbux
" kchmck vim-coffee-script
" Lokaltog vim-powerline
" mileszs ack.vim
" pangloss vim-javascript
" tpope vim-abolish
" tpope vim-bundler
" tpope vim-cucumber
" tpope vim-endwise
" tpope vim-fugitive
" tpope vim-git
" tpope vim-haml
" tpope vim-pathogen
" tpope vim-ragtag
" tpope vim-rails
" tpope vim-rake
" tpope vim-repeat
" tpope vim-surround
" tpope vim-unimpaired
" vim-ruby vim-ruby
" vim-scripts matchit.zip

set t_Co=256
" colorscheme calmar256-dark
colorscheme wombat256
set background=dark
set ts=2 sw=2 et
let g:indent_guides_start_level = 2
let g:indent_guides_start_size = 1

" let g:Powerline_symols = 'fancy'

let g:indent_guides_auto_colors = 0
autocmd VimEnter,Colorscheme * :hi IndentGuidesOdd  guibg=red   ctermbg=3
autocmd VimEnter,Colorscheme * :hi IndentGuidesEven guibg=green ctermbg=4

" adjust size of veritically split wins
" ctl_w + >
" ctl_w + <

" give me tags!
au BufWritePost *.c,*.cpp,*.h silent! !ctags -R &
let mapleader = ","
map <leader>r :!rake test_integration TEST=%<CR>

let g:ctrlp_map = '<c-p>'
let g:ctrlp_cmd = 'CtrlP'

let g:ctrlp_working_path_mode = '/home/heath/Devel/i11/uspto/USPTO_dir_frontend'

set smartcase
