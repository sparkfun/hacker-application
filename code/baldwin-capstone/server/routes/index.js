var express = require('express');
var router = express.Router();
var knex = require('../db/knex');

router.get('/', (req, res, next) => {
    knex('meals')
        .join('restaurants', 'meals.restaurant_id', 'restaurants.id')
        .select('meals.id', 'meals.restaurant_id', 'meals.name as mealName', 'meals.details', 'meals.dietary', 'meals.pickup', 'meals.price', 'meals.user_id', 'restaurants.name')
        .then((meals) => {
            res.json(meals)
        })
});

router.get('/:id', (req, res, next) => {
    knex('meals')
        .where('restaurant_id', req.params.id)
        .join('restaurants', 'meals.restaurant_id', 'restaurants.id')
        .select('meals.id', 'meals.restaurant_id', 'meals.name as mealName', 'meals.details', 'meals.dietary', 'meals.pickup', 'meals.price', 'meals.user_id', 'restaurants.name', 'restaurants.pounds')
        .then((meals) => {
            res.json(meals)
        })
});

router.put('/', (req, res, next) => {
    let updatedMeal = req.body
    knex('meals').where('meals.id', req.body.id).first()
        .update(updatedMeal, '*')
        .then((meal) => {
            res.json('meal updated with user_id')
        })
});

router.put('/:id', (req, res, next) => {
    let updatedPounds = {
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
    };

    knex('restaurants').where('restaurants.id', req.body.id).first()
        .update(updatedPounds, '*')
        .then((restaurant) => {
            res.json('restaurant pounds updated!');
        })
});

router.post('/', (req, res, next) => {
    knex('meals').insert(req.body, '*')
        .then((results) => {
            res.json(results);
        })
});

module.exports = router;
