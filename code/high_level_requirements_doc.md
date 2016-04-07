`Below is a writing sample of what came out of a 15 minute requirements gathering meeting. The task was to figure out if we could update our internal Liquid Planner account with our clients' JIRA accounts and visa versa. From this doc we created more granular requirements and functional documents.`

#JIRA/Liquid Planner Integration by [Jason Schertz](https://github.com/jschertz)

##Requirements for Jira to Liquid Planner:
 - Jira global administrative permissions
 - A Jira Webhook
 - A Worker that serves as the Jira webhook target
 - Liquid Planner API access for calling CRUD operations

##Requirements for Liquid Planner to Jira:
 - Liquid Planner admin access
 - A Liquid Planner webhook
 - A Worker that serves as the Liquid Planner webhook target
 - Jira API access for calling CRUD operations

###References
 - [Jira Webhooks](https://developer.atlassian.com/jiradev/jira-architecture/webhooks)
 - [Jira REST API Docs](https://developer.atlassian.com/jiradev/jira-apis/jira-rest-apis)
 - [Liquid Planner Webhooks](https://liquidplanner.zendesk.com/entries/21590390-webhooks-getting-started)
 - [Liquid Planner API Docs](http://www.liquidplanner.com/assets/api/liquidplanner_API.pdf)