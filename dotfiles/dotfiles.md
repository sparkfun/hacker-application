As far as dot files go I keep it pretty simple with a Homebrew setup and a script for displaying my current git branch:

```

parse_git_branch() {

    git branch 2> /dev/null | sed -e '/^[^*]/d' -e 's/* \(.*\)/ (\1)/'

}

````

Part of the reason for the simplified dotfiles is because at my previous job I used Tower to manage Git instead of with the command line.  This was a blessing and a curse, as it allowed an efficient/easy on the eyes way to manage merge conflicts but also promoted command line atrophy, which over the last couple of months I have been forcing myself to not use it.