Please answer the following general questions:

#### 1. When can you start?
ASAP


#### 2. Do you have any planned vacations/absences in the next six months that we should know about? 
No.  


#### 3. SparkFun is a dog-friendly environment. Are comfortable working around dogs? If you are a dog-owner, would you see yourself bringing dogs to work?
I do not have a dog, but love dogs and working around them. My previous workspaces were/are dog-friendly.


#### 4. The active developer keeps up on happenings in the world of development and technology. Where do you you get your news?  
Flipboard app   
Javascript Jabber podcast  

I look at the following websites quite often for new content:   
(https://getpocket.com/@addyosmani)  
(http://reactkungfu.com/)  
(https://github.com/markerikson/react-redux-links/blob/master/redux-tutorials.md)  
(https://remzolotykh.net/)  
(http://start.jcolemorrison.com/)  
(http://wesbos.com/)  
(http://mherman.org/)
    
At least a few times a month, I do a google search on a flavor of 
"javascript tutorial [react/redux/etc/etc]". I set my search tools to only search within 
the last month/week and pick out a few to try. 

There are a few emails I subscribe to:  
(http://www.webdesignerdepot.com/)  
(https://www.raywenderlich.com/)  

I enjoy learning from videos. Besides the website sources listed above, I have completed  
several video courses at Udemy: 
Modern React with Redux  
Advanced React and Redux  
Webpack2: The Complete Developer's Guide  
The Complete React Native and Redux Course
    
The next video course I am taking (but yet to complete) is:  
AWS Certified Solutions Architect - Associate 2017


#### 5. The sane developer does things other than writing code. What else occupies your time?  
I enjoy hiking and try to get out daily as much as my schedule will allow. I live in 
North Boulder, close to Wonderland Lake, and really enjoy the closeness and convenience 
of the hiking trails there. 
I am also a fairly decent cook and make my own meals several days/week. 


#### 6. The worldy developer is in touch with some form of culture (books, music, film, etc.). How do you go about feeding this part of your brain?  
I have been involved in the local Boulder music scene for 20+ years as a sound technician 
working mainly at The Fox Theatre and The Boulder Theater, but also worked several other  
clubs around Boulder and Denver. I toured with Big Head Todd and The Monsters for 7+ 
years, among others, and have traveled to and worked a show in all 50 states and some outlying islands.

I also enjoy finding interesting things to watch on Netflix - Star Trek and anything sci-fi, 
serial mysteries and documentaries are my favorites. 

I have a long list of books I would like to read and usually have 2 or 3 (or 4) open at any 
one time. Currently I am reading "The Alienist" - Caleb Carr, "Foundation" - Asimov, 
"Sharp Objects" - Gillian Flynn and "Dune" - Frank Herbert.


#### 7. Please critique any of the SparkFun websites. With influence over the SparkFun dev team what would *you* focus on improving?

## Sparkfun.com
I tried to observe all pages within the sites, with the exception of the order flow beyond adding a few 
items to the cart.  
Most of the following critiques, with the exception of the oddities I observed in the page loading times, 
appear to be easy fixes. If I were given the task of prioritizing issues, I would say that the consistency 
in the site navigation would be the top priority.   
A mobile version of the site would also rank high in that list - but that would obviously require much more 
effort and coordination.  
FYI - for the most part, these observations were made on March 20th between 4:30 and 6:30PM MST

##### Navigation
+ Consistency during page-to-page navigation between major tabs - ie Forum and Data have no header  
+ Consistency of pages presented with _blank option - avc.sparkfun.com  
+ Overall consistency of authority for navigation - major tabs and subheadings
    - Tutorials in shop>tutorials takes me to learn>tutorials - archived tutorials are at /tutorials - confusing to the user   
    - Make tutorials its own top-level navigation? 502 tutorials could be broken down into smaller sub-nav sections
          
##### Breadcrumb navigation - consistency  
+ Jumps position from shop pages to others (when shop category menu is displayed or not). 
+ Not included on all pages. 
  
##### Products
+ Products do not show price when out of stock and not available for backorder.
+ Products out of stock, but not on back order, do not show a hover indication of status.
+ The sort by/paging bar could be smaller
  - Suggestions:
  - could be combined with list and grid view options
  - sort by box length could be shortened by changing text in "quantity available:..." lines
  - update button could be changed to a refresh icon
  - could be right aligned and combined with the breadcrumb nav
+ When drilled down to a single product listing, the icons for json and csv seem out of place and mis-aligned 
+ navigation history is not being handled correctly in some cases:
  - steps to reproduce (I haven't figured out how to reliably recreate - it may take a few tries to see the behavior)
  - (1) visit (https://www.sparkfun.com/categories/273) (Audio)
  - (2) click on any product - I chose SparkFun Sound Detector
  - (3) scroll down a bit and under documents, click on any of the links there
  - (4) hit back on your browser history and you end up back at (1)
  - This does not happen on all links - nor does it happen 100% of the time. 

##### Forums
+ The forums are active and have a long history - is any customer service handled via the forums?
  - add links to customer service on forums page if warranted and approved by customer service dept
+ Could the forums be re-styled to better match the site style? 

##### "Tell us about your project" (https://www.sparkfun.com/project_calls)
+ This page indicates that "there were some problems submitting your project" though I don't think I have ever visited this page
+ This must have also triggered the error messages to show above every required input.
+ Was this a HubSpot forms issue?
+ Styling of the error messages needs to be adjusted as the boxes are overlapping other page elements
+ Update - submission error issues are non-existent on March 21 10:15AM MST
  - Clicking on continue without filling out any fields does not show error messages other than a tooltip indicating the first of the required fields
   
##### Newsletter
+ Does the signup widget bar merit being higher in the page?
+ Social links lack title attribute
    
##### Feeds (https://www.sparkfun.com/feeds)
+ Under Product Reviews - listed items exist with no title
    
##### About (https://www.sparkfun.com/about_sparkfun)  
+ Logo looks blurry. 
+ Is this the official logo banner? Is this the only place used? (Sort of same on youtube/facebook/twitter - different text treatment)
+ Color scheme does not match rest of site
    
##### Shop siderbar menu
+ Is it necessary to include for /volume, /sponsor, /jobs, /static/team, /tutorials, /quiz,
  /comment_guidelines, /pages/irc_faq, /videos?
+ Should it be included on /categories? 

##### Chat
+ Chat was not available during advertised hours - Monday? 
+ Chat is not listed as a contact option on the customer support page (https://www.sparkfun.com/support)
+ Chat IS listed on the contact page (https://www.sparkfun.com/static/contact)

##### General
+ No mobile version of site? (iphone7/chrome browser)
+ broken link - from site map page - (https://www.sparkfun.com/hackerinresidence)
+ site map is only listed in footer links - could also live in help section
  - could footer be retooled to be included in the About us/Help/Programs/Community section?
+ Work to make all major pages, subdomains, etc more consistent. 
+ A bit more whitespace overall - SparkFunEducation site is a good example and consistent
+ Could contact and support page be combined? Possible duplication of similar data.
+ Consistency of page titles included on pages - some do, some don't? 
  - Position changes - (https://www.sparkfun.com/products/14189) vs (https://www.sparkfun.com/videos)
+ Is the FAQ updated regularly to reflect customer inquiries? 
  - Is it possible to create faq items based upon popular customer requests?
  - aka it is easy enough for customer service to submit faq entries?
+ Is there a need for user comments on the https://www.sparkfun.com/privacy page? 
+ The compliance page states that "SparkFun last submitted a completed SAQ-D August 26, 2015" 
  - From my understanding, you may be required to do this every year - but beyond this issue, is this a hardcoded date or is it fed in dynamically? 

##### Page Load times
+ Times are not consistent - Something seems amiss. 
+ How long are pages cached? Are the pages/sub sites cached with similar configs? 
+ Time of day may be a factor?
+ Data stream traffic may be a factor?
+ Does the presentation of the data site require being tied to the backend ops - aka same server?
  - Could there be a page showing the overview with links off to the relevant services (create, explore, docs, deploy, bugs)? 
+ After observation - it seems like the data.sparkfun site has issues that do not 
necessarily apply to the rest of the site(s), but there are some anomalies with the loading 
times on other pages as well.  

##### Observed March 20th - roughly between 4:30 and 6:30 PM MST. google dev tools reports    
+ Initial load - Data page
  + DOMContentLoaded: 3.60s | Load: 3.79s
+ Subsequent loads - shortly thereafter
  + DOMContentLoaded: 1.00s | Load: 1.18s
  + DOMContentLoaded: 1.00s | Load: 1.18s
+ A page refresh after 4 minutes or so...
  + DOMContentLoaded: 5.73s | Load: 5.89s
  
##### Observed March 20 - 10:15PM MST - load times have improved tremendously 
+ initial and subsequent loads - data page
  + DOMContentLoaded: 980ms | Load: 1.11s

##### Observed March 21 
+ site generally seems more responsive than yesterday afternoon
+ however, I just experienced the following today on the data page
  - 09:00AM MST DOMContentLoaded: 33.69s | Load: 33.83s
  - 10:41AM MST DOMContentLoaded: 26.41s | Load: 26.56s
  - 10:51AM MST DOMContentLoaded: 16.67s | Load: 21.00s
  - 06:22AM MST DOMContentLoaded: 25.93s | Load: 32.88s
+ shop page times are *generally* under 2.5s with cache disabled and under 1.5s without cache disabled
  - 06:22PM MST DOMContentLoaded: 6.10s | Load: 6.67s
  
  
## SparkFunEducation.com

+ Load times are greatly improved
+ Consistency from page to page is improved
+ Mobile version is great!
+ Blog does not have grey header like other pages, but does include breadcrumb nav. 
+ Resources tab does not have a default link - you must drill into drop down menu to nav. 
  - This dropdown does not work on mobile.
+ Other resources in footer, with one exception, lead to the www.sparkfun site and open in new tabs. 
  - include an indicator on the links to external sites 

