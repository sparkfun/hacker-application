NOTES:

--I focused on javascript mostly because that is my strongest coding language

--I like knockout more than jQuery... but I know a lot about both!

--I tried to get some examples of my PHP work from Blistt but they are unavailable at this time.
  My machine containing the dev work I did for them burned in a fire so you get ASP.NET C# instead


WHAT's IN HERE:

--PagedObservableArray.js/Persons.html

	I wrote this plugin after growing tired on not having a simple way to implement a Google style pagination system
	with Knockout.  I found lots of plugins to convert an observable array to a basic paged array, but nothing that 
	really dealt with my need.  So basically I looked at how they did that, and then wrote my own plugin to do exactly
	what I wanted.  You can see this implemented on the table in Persons.html.  I know it doesn't look like much, but that's
	the whole point I was trying to do.  Oh, and that HTML is utilizing Bootstrap styles so you guys can imagine
	what it looks like.

--Everything Else

	The rest of the code submitted is taken from a SPA app I'm messing with right now.  The files model.customer.js and vm.customers.js
	show some of the MVVM coding pattern at work here.  model.customer is obviously the model and vm.customers is the VM.  What you don't 
	see is the javascript datacontext I have implemented that basically handles all request that come from the VM's and handles the model
	mapping.  I also have scripts for Amplify.js calls set up to send and receive JSON data to the Web API's and use sammy.js to implement a routing
	engine to serve up views.  All the javascript follows the revealing module pattern implemented with require.js.  CustomerController.cs is 
	the Web API where you can see the basic CRUD operations implemented. CustomerRepo.cs implements from a base class repo that holds the basic CRUD
	operations, this class however required additional operations shown implemented here.  The Web API talks to a Unit of Work class to make talking to
	the repositories much easier.  I'm not showing that code though because it's a pile of code to show.  It's really just the factory pattern used
	to determine what repository is needed and building it based on a given class instance.  Repositories are talking to Entity Framework which in
	turn talks to SQL Server 2008.  Index.cshtml shows the overall structure of the SPA app page.
	 
	