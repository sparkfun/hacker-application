
Because these examples are to be read and not executed I have included samples from a variety of projects showcasing skills from your required/preferred qualifications. Admittedly it has been some time since I have written any major code in PHP, as such it has not been included. Instead I am submitting examples in Javascript, Python, and CSharp. Further I am more than confident in my ability to write PHP once I dip my feet back into the pool.

### Brief overview of code samples:

#### simple-splash-page

A simple static splash page originally deployed via Heroku via Ruby/Rack.

Items of Note:
* Leverages Bootstrap.css for base styling
* Custom.css has been created to further expand page styling
* Utilizes media queries to appropriately resize specific CSS elements based on screen size
* Extensive favicons created for viewing on standard browsers as well as all mobile browser options

#### python-rest-app

Python Flask application originally deployed via Heroku leveraging REST principles for use by both mobile and web based applications.

Items of Note:
* Back-end is configured for use with Postgres
* SQLAlchemy handles multiple complex joins including multiple relationships, and self-referential joins
* Redis implemented by a secondary worker to handle queuing of phone contacts syncing
* Integrated migrations for handling model changes
* Flask-Login Authentication for handling signup/logins

#### js-mobile-app

A Javascript application using Trigger.io for deployment as both a web application and a "native" mobile app.

Items of Note:
* Interacts directly with the python-rest-app above
* Leverages vanilla Backbone to bring MVC struture to the front-end
* Uses Bootstrap and JQuery for front-end components (although older versions)
* Provides basic login/signup/logout functionality
* Primary area of interest is the contacts sync functions within app.js
* Templating provided using Handlebars

#### csharp-lucene-search

CSharp implementation of an application to search geographic data including road and address datasets

Items of Note:
* Implements Lucene to create and search indexes
* Handles various types of searches including address, road, and intersections
* Additionally allows users to search across all indexes for any keyword
* Results are formatted for display to a DataGridView on the UI
