<!--*********************************************************************
The code below is a class I created while gaining some practice with classes and objects in PHP.   It is presented below in its entirety and it will compile as expected.
Github repository:
https://github.com/StephenHanzlik/phpDogs
********************************************************************* -->

<html>
    <head>
      <title>Dogs in the House</title>
    </head>
	<body>
      <p>
      <?php
        //Creating a class and adding properties
        class Dog {
            public $isHome;
            public $name;
            public $age;
            public $isFed;

            public function __construct($isHome, $name, $age, $isFed) {
              $this->isHome = $isHome;
              $this->name = $name;
              $this->age = $age;
              $this->isFed = $isFed;
            }

            // Adding a method to check the status of the dog.  Control flow statements dictate the final string that will be printed.
            public function howAreYouDog() {
             $fedOrNotString = "I dont want my dog food but I'll have whatever you are eating.</br>";
             if($this->isFed === false){
               $fedOrNotString = "and I am hungry!</br>";
             }
             if($this->isHome === true){
               return "Woof, I am home and my name is " . $this->name . ".  I am " . $this->age . " years old " . $fedOrNotString;
             }
             if($this->isHome === false){
               return "Woof, I am NOT home.  My name is " . $this->name . ".  I am " . $this->age . " years old and " . $fedOrNotString;
             }
           }
          }

        // Create new dogs
        $jazz = new Dog(true, 'Jazz', 3, false);
        $egg = new Dog(false, 'Egg', 2, false);
        $dinky = new Dog(true, 'Dinky', 3, true);

        // Print out how the dogs are doing
        echo $jazz->howAreYouDog();
        echo $egg->howAreYouDog();
        echo $dinky->howAreYouDog();
        ?>
      </p>
    </body>
</html>
