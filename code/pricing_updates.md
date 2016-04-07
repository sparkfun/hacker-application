#Deployment Support Writing Sample by [Jason Schertz](https://github.com/jschertz)

##Objective
The objective of this document is to outline contingency plans and their associated execution for 1/1/2015 pricing updates.

##50x.html
During the hours between 12:01AM EST and 2:00PM EST on 1/1/2015, `the website` will be placed into maintenance mode. A small server instance containing the 50x.html maintenance page will be placed behind `the website` load balancer to be served up 100% to `the website url`. During this time, the normal site will be directly accessible via the 2 primary web servers.

The links that will be used to test pricing:

* `URL OMITTED`
* `URL OMITTED`

> Note: The wildcard SSL cert for `the website` only supports single sub-domains, therefore cert warnings will occur on the above urls as they contain double sub-domains. This is not a security issue on the server. 

After the maintenance window is completed, the site will be reverted from maintenance mode and the original load balancer configuration will be restored.

After successful restoration, a second round of testing will be performed by Client and Internal Teams at the primary website url: `URL OMITTED`

##Pricing Changes and Cron Jobs
Planned pricing updates on 1/1/2015 will be updated within Client web services before 2:00PM EST. Once Client production web services are updated, the website cron jobs will need to occur in order to get new pricing displaying on the individual review links (`URL OMITTED` and `URL OMITTED`). 

The order of events are as follows:

1. Prod pricing is updated in Client web services
2. Facilities cron job is run via Jenkins ~ less than a minute to complete
3. Doctors cron job is run via Jenkins ~ less than a minute to complete
4. Cache Flush cron job is run via Jenkins ~ 20-30 minutes to complete
5. Client to verify pricing updates & Internal Teams will spot review pricing on both web servers

##Troubleshooting
In the event pricing is displaying incorrectly, Client and Internal Teams will execute the following troubleshooting steps:

1. Client to verify pricing integrity from AS400 up to SOAP web services
* Client to share SOAP request XML being tested with results from Internal Teams
2. Internal Teams to verify pricing integrity in dotcom API and dotcom front-end
* Dotcom API: Pricing Model Calculations
* Dotcom API: FlushCache and individual office pricing retrieval
* Dotcom Front-end (Desktop/Mobile) Data display

##Rollback Plan
In the event troubleshooting steps do not uncover the defect in pricing or if troubleshooting is slated to go beyond 2:00PM EST, the site should be rolled back to display full-prices for 2015.

*To Rollback to full 2015 pricing:*

1. Internal Teams to manually enter full-price coupon codes in API DB `TABLE NAME OMITTED` table.
2. Cache FLush cron job is run via Jenkins - ~20-30 minutes for completion
3. Full prices will display on site
4. Site should be taken out of maintenance mode and restored to original configuration
5. Facilities automated cron job should be disabled until pricing is fixed
6. Troubleshooting steps should continue
	