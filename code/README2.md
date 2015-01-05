I’d love to share more code with you, however much of my code is owned by companies that I have previously worked for.  I’m not able to share such code due to non-disclosure agreements.

I have therefor written a short set of classes to demonstrate my coding style.  This little sample application sets up classes to define a group of Aircraft in the Civil Air Patrol.  I assign to the each Aircraft relevant information about each Aircraft, such as the N-Number, the Model and a Crew Chief.  For each Crew Chief, we enter their Name, Email and Physical Address.

In this example, I demonstrate OOP concepts as well as using several different design patterns such as Iterators, Adapters, and Decorator patterns.

I’ve included a test.php program to exercise the different classes. As you may not actually execute the program, I’ve included the output below.

Best Regards,
—Stephan Cavarra
scavarra@gmail.com


—- test.php output —-

Email Addresses:  
  jimj@example.com
  melc@example.com
  stephanc@example.com

Iterating through both types of addresses (Email and Physical):
  jimj@example.com
  melc@example.com
  stephanc@example.com
  123 Any St. Boulder, CO  80303
  123 Any Blvd. Milliken, CO  80543
  123 Any Ave. Fort Collins, CO  80524

N-Number:  N9669X
  Crew Chief:  Jim Jenkins, jimj@example.com, 123 Any St., Boulder, CO, 80303
    manufacturer:  Cessna 
    type:  C182R 
    navigation:  standard 
    engine:  Continental O-470-U 
    autopilot:  none 
Final cost:  250000

N-Number:  N9559X
  Crew Chief:  Mel Callen, melc@example.com, 123 Any Blvd., Milliken, CO, 80543
    manufacturer:  Cessna 
    type:  C182S 
    navigation:  Apollo GX-55 
    engine:  Lycoming IO-540-AB1A5 230 BHP 
    autopilot:  STec System Fifty Five X 
Final cost:  350000

N-Number:  N652CP
  Crew Chief:  Stephan Cavarra, stephanc@example.com, 123 Any Ave., Fort Collins, CO, 80524
    manufacturer:  Cessna 
    type:  T182T 
    navigation:  Garmin G1000 (NAV III) 
    engine:  Lycoming TIO-540-AK1A Turbo 235 BHP 
    autopilot:  Bendix/King KAP140 
    safety:  AmSafe Airbags 
Final cost:  550000

