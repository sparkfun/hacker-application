set modeline
set laststatus=2
set synmaxcol=5000
set number
let @a = 'i__FILE__, __LINE__'
let @m = 'ivim: set expandtab tabstop=4 shiftwidth=4 autoindent smartindent:'
filetype plugin on

"Tab conversion tools
" http://vim.wikia.com/wiki/VimTip1592
" Convert all leading spaces to tabs (default range is whole file):
" :Space2Tab
" Convert lines 11 to 15 only (inclusive):
" :11,15Space2Tab
" Convert last visually-selected lines:
" :'<,'>Space2Tab
" Same, converting leading tabs to spaces:
" :'<,'>Tab2Space
command! -range=% -nargs=0 Tab2Space execute '<line1>,<line2>s#^\t\+#\=repeat(" ", len(submatch(0))*' . &ts . ')'
command! -range=% -nargs=0 Space2Tab execute '<line1>,<line2>s#^\( \{'.&ts.'\}\)\+#\=repeat("\t", len(submatch(0))/' . &ts . ')'

" :'<,'>SuperRetab 2    would change the following
"
" for {
"   that;
" }
"                        into 
" |-------for {
" |-------|-------that;
" |-------}
"
" :SuperRetab 5         would give the following from the above
" 
" for {
"      that;
" }
command! -nargs=1 -range SuperRetab <line1>,<line2>s/\v%(^ *)@<= {<args>}/\t/g

set pastetoggle=<F6>
map <F9> :tabnew  
map <F10> :tabclose<CR>
map <F11> :tabprevious<CR>
map <F12> :tabnext<CR>

