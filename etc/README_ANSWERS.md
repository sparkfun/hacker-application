Please answer the following general questions:

1. When can you start? 
ASAP


2. Do you have any planned vacations/absences in the next six months that we should know about?
No, but I do have a few weekend engagements that I am slated to work at least through 
April.  


3. SparkFun is a dog-friendly environment. Are comfortable working around dogs? If you are a dog-owner, would you see yourself bringing dogs to work?
I do not have a dog, but love dogs and working around them. I am hopeful that 
one day I will not only live in a place where the HOA approves, but where I 
have enough yard for a dog to run around in.


4. The active developer keeps up on happenings in the world of development and technology. Where do you you get your news?
Flipboard app - During my bus commute or while waiting for the rice to simmer, I read 
what I can and send the rest to my "readmelater" email. I generally read those emails 
before going to bed or first thing in the morning

I also look at these websites first thing every morning to see what new things I can learn 
https://getpocket.com/@addyosmani
http://reactkungfu.com/
https://github.com/markerikson/react-redux-links/blob/master/redux-tutorials.md
https://remzolotykh.net/
http://start.jcolemorrison.com/
http://wesbos.com/

At least a few times a month, I do a google search on a flavor of 
"javascript tutorial [react/redux/etc/etc]". I set my search tools to only search within 
the last month/week and pick out a few to try. 

There are a few helpful emails I am subscribed to - the most notable being 
http://www.webdesignerdepot.com/


5. The sane developer does things other than writing code. What else occupies your time?
I enjoy hiking and try to get out daily as much as my schedule will allow. I live in 
North Boulder, close to Wonderland Lake, and really enjoy the closeness and convenience 
of the hiking trails there. I am also a fairly decent cook and try to make my own meals 
several days/week. Over the past year, I tried to focus on quick preparation while not 
sacrificing taste, mainly to buy more time for schoolwork.


6. The worldy developer is in touch with some form of culture (books, music, film, etc.). How do you go about feeding this part of your brain?
I have been involved in the local Boulder music scene for 20+ years as a sound technician. 
I have mainly worked at The Fox Theatre and The Boulder Theater, as well as several  
clubs around Boulder and Denver. I have also travelled around the country (and a small 
part of the outside world) with national touring acts. I have done a show in all 50 states 
and some outlying islands. During my travels, I try to read and take pictures as much 
as my schedule will allow.

I also enjoy finding interesting things to watch on Netflix - serial mysteries and 
documentaries are my favorites.

I have a long list of books I would like to read and usually have 2 or three open at any 
one time. Currently I am reading "The Alienist" - Caleb Carr, "Foundation" - Asimov, 
"Sharp Objects" - Gillian Flynn and "Dune" - Frank Herbert.


7. Please critique any of the SparkFun websites. With influence over the SparkFun dev team what would *you* focus on improving?

## Sparkfun.com

* Navigation
  * Consistency during page-to-page navigation between major tabs - ie Forum and Data have no header
  * Consistency of pages presented with _blank option - avc.sparkfun.com
  * Overall consistency of authority for navigation
  * tutorials in shop>tutorials takes me to learn>tutorials 
    * page flashes during loading and the sub-navigation context I was just looking at has changed
    * now I am in the Learn Tab and confused as to context
    * consider making tutorials its own top-level navigation? 502 tutorials could be broken down into smaller sub-nav sections
    
      
* Breadcrumb navigation - consistency  
  * Jumps position from shop pages to others. 
  * Not included on all pages. 
  * Perhaps it could be made smaller or live in a different area on the screen?


* Page loading times could be improved 
  * For example - Data page - https://data.sparkfun.com/ - google dev tools reports run
  * Initial load (I had already visited the page some minutes ago - but think the cache has reset by now)
  * 0/19 requests | 0B / 155KB transferred | Finish: 8.57s | DOMContentLoaded: 3.60s | Load: 3.79s
  * Subsequent loads - shortly thereafter
  * 0/19 requests | 0B / 156KB transferred | Finish: 5.75s | DOMContentLoaded: 1.00s | Load: 1.18s
  * 0/19 requests | 0B / 156KB transferred | Finish: 5.75s | DOMContentLoaded: 1.00s | Load: 1.18s
  * How long is the page cached? Are the pages/sub sites cached with similar configs? Something seems amiss. 
  * A page refresh after 4 minutes or so...
  * 0/19 requests | 0B / 156KB transferred | Finish: 10.54s | DOMContentLoaded: 5.73s | Load: 5.89s


* Forums
* The forums are somewhat active and have a long history - do you handle any customer service via the forums?
* Could the forums be re-styled to better match the rest of the site? 


* Is it necessary to include shop sidebar menu for https://www.sparkfun.com/volume, https://www.sparkfun.com/sponsor, 
https://www.sparkfun.com/jobs, https://www.sparkfun.com/static/team, https://www.sparkfun.com/tutorials, https://www.sparkfun.com/quiz,
https://www.sparkfun.com/comment_guidelines, https://www.sparkfun.com/pages/irc_faq, https://www.sparkfun.com/videos
* should it be included on https://www.sparkfun.com/categories, 


* "Tell us about your project" https://www.sparkfun.com/project_calls
  * this page indicates that "there were some problems submitting your project" though I don't think I have ever visited this page
  * ^^^ must have also triggered the error messages to show above every required input.
  * the styling of the error messages needs to be adjusted as the boxes are overlapping other page elements
 
  
* How active is the newsletter...
  * Does the signup widget merit being higher in the page?
  * Social links lack title tags
  
  
* https://www.sparkfun.com/feeds
  * Under Product Reviews it is possible to list an item with no title
  
  
* https://www.sparkfun.com/about_sparkfun  
  * the logo looks blurry. 
  * Is this the official logo banner? Is this the only place used? (Sort of same on youtube/facebook/twitter - different text treatment)
  * Color scheme does not match rest of site
  
* General
* this link (from site map) is broken - https://www.sparkfun.com/hackerinresidence
* site map is only listed in footer links - could also live in help section
  * could footer be retooled to be included in the About us/Help/Programs/Community section?
* Work to make all major pages, subdomains, etc more consistent. 
* I would like to see more whitespace overall
* Is chat page ever manned? It is not listed as a contact option on the customer support page either. 
* Chat is listed contact page https://www.sparkfun.com/static/contact
* Could contact and support page be combined? Possible duplication of similar data.
* Consistency of page titles included on pages - some do some don't? Position changes - https://www.sparkfun.com/products/14189 vs https://www.sparkfun.com/videos
* Is the FAQ updated regularly to reflect customer inquiries? Is it possible to create faq items based upon popular customer requests?  
* Is there a need for user comments on the https://www.sparkfun.com/privacy page? 
* The compliance page states that "SparkFun last submitted a completed SAQ-D August 26, 2015" From my understanding, you 
may be required to do this every year - but beyond this issue, is this a hardcoded date or is it fed in dynamically? 


## Sparkfuneducation.com
