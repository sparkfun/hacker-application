var express = require('express')
var router = express.Router()
var knex = require('../db/knex')

// retrieves meals from database to send to client, joins restaurants and meals tables
router.get('/', (req, res, next) => {
  knex('meals')
    .join('restaurants', 'meals.restaurant_id', 'restaurants.id')
    .select('meals.id', 'meals.restaurant_id', 'meals.name as mealName', 'meals.details', 'meals.dietary', 'meals.pickup', 'meals.price', 'meals.user_id', 'restaurants.name')
    .then((meals) => {
      res.json(meals)
    })
})

// updates meal in meals table with user id after purchase
router.put('/', (req, res, next) => {
  var updatedMeal = req.body
  knex('meals').where('meals.id', req.body.id).first()
    .update(updatedMeal, '*')
    .then((meal) => {
      res.json('meal updated with user_id')
    })
})

// updates number of pounds for restaurant in restaurants table using restaurant id
router.put('/:id', (req, res, next) => {
  var updatedPounds = {
    name: req.body.name,
    username: req.body.username,
    email: req.body.email,
    hash: req.body.hash,
    image: req.body.image,
    pounds: req.body.pounds,
    phone: req.body.phone,
    address: req.body.address,
    city: req.body.city,
    state: req.body.state,
    zip: req.body.zip
  }

  knex('restaurants').where('restaurants.id', req.body.id).first()
    .update(updatedPounds, '*')
    .then((restaurant) => {
      res.json('restaurant pounds updated!')
    })
})

module.exports = router
