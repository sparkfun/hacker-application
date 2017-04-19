So really the only thing I have is ssh keys set up and some aliases for servers I use, and MAMP, which is a LAMP stack for Mac. I realize it is sort of "cheating" to use a pre-made utility for that, but jesus, it s seriously so nice. Requires zero config, you can have everything up and running immediately, button to start and stop all the servers. I thought after setting it all up for the first time I deserved a break!

     export PATH=/usr/bin:/bin:/usr/sbin:/sbin:/usr/local/bin:/usr/X11/bin:/Applications/MPLAB/C30/bin:/Applications/MAMP/Library/bin

My web server

    alias sshmt='ssh sibicle.com@s139776.gridserver.com'

Seedbox :) That I...uh....wouldn't use at work of course

    alias sshfh='ssh sibicle@carmine.feralhosting.com'

Current work's server

    alias sshlp='ssh jnedell@lingoport.net'

Okay so why so little config? I know command line pretty well, familiar with navigating around in vim, clever onelines using grep, xargs, sed, awk etc.

Well, because my preferred operating system is OS X, and my preferred editor is [TextMate](http://macromates.com). 

It is seriously one of the most amazing programs I have every used. It has "bundles" that are context sensitive, so when you are in an html document there are shortcuts to refresh the browser, in CSS there are text expansions for shorthands. It even can render LaTeX documents inline!!

Here are some of the custom expansions I have set up:

This one sets a purple background so I can quickly test why a CSS property might not be working how I expect.

    bgt -> background: rgba(248, 54, 250, 0.2);

This one makes a property "margin: x auto y auto" for when I have a centered element that I want top margin on. Pressing tab cycles through the text insertion points.

    autot -> margin: $1 auto $2 auto;
    $0


Anyways, while I love the power of Unix, I think it is crazy to not accept the power of point, click and drag. [Transmit](http://panic.com/transmit/) is an FTP client that is another candidate for "best program in existence ever". The amount of time it saves me working on live sites is incredible.