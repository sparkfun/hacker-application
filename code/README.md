We want to see your code. Please include a representative sample of your
work in this directory.

All languages and paradigms are welcome, but we're most interested in
submissions demonstrating one or more of the following:

  - Clean, secure, object oriented PHP.
  - Usable, modern, and reasonably standard HTML, CSS, and JavaScript.
  - Clear, sensible, and consistently formatted code.
  - Sympathy for the plight of future maintainers.

If you maintain or contribute to any open source projects, please supply links
and a short description of your role in each project.

**Tips:**

 * Avoid excessive amounts of code.
 * Exclude third party utilities and other dependencies like XML libraries or binaries.
 * Your samples will be *read*, not *executed*.
 * Less is more.
 * Select samples that are brief and show off different characteristics of your skill set.

In lieu of or in addition to code samples please also consider providing writing samples.
When applying for a more managerial position this is of particular importance as
clear written communication is critical.


Quick Project Descriptions:

MAPI: PHP7 code for a multipurpose API suite.  Configuration files and classes for each API in the /api folder are loaded to allow for RESTful API access, saving the response to a database, and later perform ETL functions (hastily taking place in /etl.php, and that functionality will be rolled into a standalone class).   At present, only the NASA API stuff has been implemented and tested (/nasa_test.php).  I'll likely roll the API config files into database tables and have one custom class per API, as in /api/class.api.nasa.php

This is the only PHP project I have at the moment; all other past PHP projects were for, or were donated to, the company I was working with.

AlphaGame: Shameless homage to "Bookworm Adventures" - a simple language-based RPG using a Boggle-like grid of letters to build words to use as attacks against enemies.  It's HTML5/JavaScript/CSS using AngularJS.  I'd like to add language support, a foreign reference dictionary and display transliterations for characters atop their English equivalents.  There's a bit of prep stuff in there for having Elder Futhark runes.  A simple word frequency list would be a great way to boost vocabulary in a foreign language.  I was coding this, between 1 and ~20 lines at a time, as a rehab exercise while my hand injury healed, and it hasn't been touched much since then.