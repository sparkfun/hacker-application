git_prompt ()
{
    if ! git rev-parse --git-dir > /dev/null 2>&1; then
        return 0
    fi

    git_branch=$(git branch 2>/dev/null| sed -n '/^\*/s/^\* //p')
    git_repo=$(git config --get-regexp remote.*.url | sed 's/.*\/\(.*\)$/\1/g')

    echo " [$git_repo/$git_branch] "
}

do_pwd ()
{
    last_command=$(history | tail -1 | awk '{ print $2 }')
    if [ $last_command = "cd" ]
        then
            echo "$PWD"
        else
            echo "\W"
    fi
}

PROMPT_COMMAND='PS1="[\u@$(hostname) $(do_pwd)]$(git_prompt)\$ "'
