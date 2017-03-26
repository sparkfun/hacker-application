/*
An Express.js routing example. My impetus for using this sample is to share with
you my knowledge of RESTful API development (with Express.js router) while also
demonstrating a skill at interacting with SQL databases (MySql).
*/

// Require Express.js
var express = require('express');
var router = express.Router();

// MySql db connection and dependicies
var mysql = require('mysql');
var config = require("../config/config.js");
var connection = mysql.createConnection({
    host: config.host,
    user: config.user,
    password: config.password,
    database: config.database
});
connection.connect();

// An endpoint that handles a front-end request to get all users' comments for a
// specific music festival. The response is sent back to the front-end as JSON.
router.use('/festivalDetail', (req, res, next) => {
    var id = req.query.festivalId;
    // Define a query to select all the comments for a specific festival joined
    // from 2 tables, comments and users.
    var selectCommentsQuery = `SELECT users.username,
                                    users.avatar,
                                    comment,
                                    festival_id,
                                    user_id,
                                    timestamp FROM comments
                                    INNER JOIN users ON users.id = comments.user_id
                                    WHERE festival_id = ${id} ORDER BY timestamp DESC`;
    // MySqljs query method to make the selection and return the results or throw
    // any errors.
    connection.query(selectCommentsQuery, (error2, results2) => {
        if (error2) throw error2;
        res.json({
            comments: results2
        });
    });
});

// Terminate the connection
connection.end();
