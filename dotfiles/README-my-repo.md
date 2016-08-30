# Patrick's Core Home Directory

This is a repo of all the files I keep in my home directory
across many linux/bsd boxes - both headless & gui.

If you want to use the files exactly as they are, then
clone this repository somewhere (I usually clone into
```~/.home/home-core```), cd into that directory, and then
run the ```./create-links.sh``` script. Please note that
it is destructive as it deletes all existing files &
replaces them with a soft link to the existing files in
this repository.

So:
```sh
	mkdir -p ~/.home/home-core
	cd ~/.home/home-core
	git clone https://path/to/git/repo.git .
	./create-links.sh
```

#################################

everything below this has been added as an extra explanation for the
hacker-application fork.

I have multiple home-* git repositories for different specific things that I
usually don't share. These include, but are not limited to:

* home-hidden that contains things like ``.pastebinit.xml`` with config
    settings for using the ``pastebinit`` cli program.
* home-ssh that ignores the local ``id_*`` & ``id_*.pub`` files, but keeps
    track of ``authorized_keys``, ``config``, and ``knownhosts``.
* home-xchat where I have config files for the xchat IRC GUI client that don't
    need to go on headless boxes
