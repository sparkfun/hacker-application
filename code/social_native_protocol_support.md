
#Technical Feasibility Writing Sample by [Jason Schertz](https://github.com/jschertz)

###Abstract:
-----
The task in question is to evaluate whether the image share links for Facebook and Twitter can open up native applications on the mobile website.


----------


###Result:
Facebook and Twitter apps do have protocols that can launch their respective native apps. However, Facebook only allows specific actions to be done within the native Facebook application. Twitter seems to allow pre-populated tweets.

####Facebook

> fb:// is the protocol name
> The list is pretty extensive, though some additional research will be required to find out if sharing can happen as their is no equivalent to the sharer.php within the Facebook native application.

reference:

 - http://wiki.akosma.com/IPhone_URL_Schemes#Facebook


####Twitter

> twitter://status?id=12345
twitter://user?screen_name=lorenb
twitter://user?id=12345
twitter://status?id=12345
twitter://timeline
twitter://mentions
twitter://messages
twitter://list?screen_name=lorenb&slug=abcd
twitter://post?message=hello%20world
twitter://post?message=hello%20world&in_reply_to_status_id=12345
twitter://search?query=%23hashtag

Reference:

- http://wiki.akosma.com/IPhone_URL_Schemes#Twitter


###A Word on Users without Native Apps Installed
Should the user not have the native Facebook or Twitter application installed on their phone, an error will occur that effectively dead-ends users.

There is currently no out-of-the-box workaround to this scenario. A custom library would need to be developed in order to support a link that works for both users with and without native Facebook/Twitter applications. In addition, this custom library would likely need to be a back-end service that tests the user's device for custom protocol support.

![Error for non-installed users](https://s3.amazonaws.com/f.cl.ly/items/1L2p341M2H1o2y2J0g44/Screen%20Shot%202015-04-15%20at%203.53.50%20PM.png)