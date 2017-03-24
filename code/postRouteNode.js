//*********************************************************************
//The code below is written in NodeJS and is used to handle POST requests to my API.  Data is coming from an Arduino Uno with a SparkFun weather sheild and Electric Imp breakout board.  The function rp() is from a simplified request client with additional promise support.
//Deployed version:
//https://dinkydinky.herokuapp.com
//The full repository can be found at:
//https://github.com/StephenHanzlik/capstone-let-it-grow
//The snippet below starts at line 35 of this document:
//https://github.com/StephenHanzlik/capstone-let-it-grow/blob/master/routes/data.js
//*********************************************************************

//start of post route
router.post('/', (req, res, next) => {
  //Create an object out of the request body
  const { light, temperature, humidity, soil_moisture } = req.body;
  let insertPost = { light, temperature, humidity, soil_moisture };
  //Limit the humidity data to 100% for a better UX
  if (insertPost.humidity > 100) {
    insertPost.humidity = 100;
  }
  insertPost.created_at = new Date();
  //configure options for GET requst to be passed into the rp function
  const options = {
    uri: 'https://dinkydinky.herokuapp.com/smssettings',
    headers: {
      'User-Agent': 'Request-Promise'
    },
    json: true // Automatically parses the JSON string in the response
  };
  //Create a function to handle the asynchrounous nature of a GET request to smssettings within the POST route
  function asynchData(postData, fn, obj) {
    //rp() sends a request that is configured by the options passed into it
    rp(options)
      .then(function(data) {
        //after a promise is returned it has the response data from smssettings and the data that we still wish to post.  Function fn is used to continue with the POST request
        fn(data, obj);
      })
      .catch(function(err) {
        // POST request failed
      });
  }

  //This function is called passing in settings and insertPost as parameters to the function "fn" parameter from line 35
  asynchData("postData", function(settings, insertPost) {
    //Comparing current time stamp to the one present in smssettings
    let now = new Date();
    let arrNow = [];
    let nowString = now.toString();
    for (let i = 16; i <= 20; i++) {
      if (nowString.charAt(i) !== ":") {
        arrNow.push(nowString.charAt(i));
      }
    }
    let joinedNowString = arrNow.join('');
    let currentTimeInt = parseInt(joinedNowString);

    //Control flow statements compare settings from persistant form data and decide wether to trigger a text message or not.  In this case it is checking if the lights are supposed to be on.
    if (currentTimeInt >= settings.on_time && currentTimeInt <= settings.off_time) {
      //settings.text_sent is used to prevent server from sending out multiple texts when alerts are triggered.   It is reset when a new smssettings form is submited.
      if (insertPost.light < 1 && settings.text_sent < 1) {
        //A POST request is sent to smssettings to increase the value of text_sent and prevent repeat messages from being sent
        const options = {
          method: 'POST',
          uri: 'https://dinkydinky.herokuapp.com/smssettings',
          body: {
            on_time: settings.on_time,
            off_time: settings.off_time,
            max_temp: settings.max_temp,
            min_temp: settings.min_temp,
            max_humid: settings.max_humid,
            min_humid: settings.min_humid,
            text_sent: 1
          },
          json: true // Automatically stringifies the body to JSON
        };
        //Send a POST request with the options from line 61
        rp(options)
          .then(function(parsedBody) {
            // POST succeeded...
          })
          .catch(function(err) {
            // POST failed...
          });

        //Twilio is then used to send sms alerts.  Excluded here for brevity.
      }
    }
    //Other metrics and alerts are checked in the same fashion using control flow statements

    //Data is added to PostgreSQL database

  }, insertPost)
});
