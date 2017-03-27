////////////////////////////////////////////////////////////
// SHOWS.js
//
// database access methods for the shows table
////////////////////////////////////////////////////////////

'use strict';

var knex = require('../../config/db/knex');

////////////////////////////////////////////////////////
// PUBLIC methods
////////////////////////////////////////////////////////

// returns show list by first querying showdates for:
//    active, dateinfuture, all statuses ok
//    corresponding show should be active - disregard status
// showtickets should be active, disregard status and inventory
//    inventory will be handled on client
// catalog implies that all are active
module.exports.getShowCatalog = function(){
  
  // get show dates where active and in future
  var now = knex.fn.now();
  
  var sql = 'SELECT sd.* ';
  sql += 'FROM showdates sd LEFT OUTER JOIN shows s ON s.id = sd.show_id ';
  sql += 'WHERE sd.dateofshow > ? AND sd.active = true AND ';
  sql += 's.active = true ';
  sql += 'ORDER BY sd.dateofshow ASC ';
  
  return buildShowCatalogFromQuery( knex.raw(sql, [now]) );
};

// create new or update existing
module.exports.createOrUpdate = function(input, current){
  
  // auto update the url
  input.url = '/store/shows/' + convertNameForUrl(input.name);
  
  return knex('shows').where({member_id: input.member_id, name: input.name}).first()
  .then(function(data) {
    var existing = data;
    
    if(!current){
      if(existing){
        throw Error('A show with that name already exists for the current member');
      }
      
      return knex('shows').insert(input).returning('id');
    } else {
      // if there is an existing with a different id
      if(existing && existing.id !== current.id){
        throw Error('A show with that name already exists for the current member');
      }
      
      return knex('shows').update(input).where({id:current.id}).returning('id');
    }
  })
};

// retrieve by id
module.exports.getShowById = function(show_id){
  return buildShowFromInitialQuery(
    knex('shows').where({id:show_id})
    .orderBy('name','asc'));
};

// retrieve a members show listing
module.exports.getMemberShowListing = function(member_id) {
  return buildShowFromInitialQuery(
    knex('shows').where({member_id: member_id})
    .orderBy('name', 'asc'));
};

////////////////////////////////////////////////////////
// PRIVATE methods
////////////////////////////////////////////////////////

// Given a query(promise) fetch data with the given pattern
var buildShowCatalogFromQuery = function(query) {
  var showListing = {};
  // TODO implement correct ordering
  
  return query
  .then(function (data) {
    showListing.showdates = data.rows;
    return knex('shows').whereIn('id', showListing.showdates.map(e => e.show_id))
    .andWhere('active', true);
  })
  .then(function(data) {
    showListing.shows = data;
    
    return knex('showtickets').whereIn('showdate_id', showListing.showdates.map(e => e.id))
    .whereRaw('((allotted-sold+refunded) > 0)')
    .andWhere('active', true).andWhere('status','on sale');
  })
  .then(function(data){
    showListing.showtickets = data;
    return showListing;
  });
};


// TODO this is a bit contrived - make this more robust
var convertNameForUrl = function(name){
  return name.toLowerCase().replace(/\W/g,'-').replace(/\-+/g,'-');
};

// Given a query(promise) fetch data with the given pattern
var buildShowFromInitialQuery = function(query) {
  var showListing = {};
  // TODO implement correct ordering
  
  return query
  .then(function (data) {
    showListing.shows = data;
    return knex('showdates').whereIn('show_id', showListing.shows.map(e => e.id));
  })
  .then(function(data) {
    showListing.showdates = data;
    return knex('showtickets').whereIn('showdate_id', showListing.showdates.map(e => e.id));
  })
  .then(function(data){
    showListing.showtickets = data;
    return showListing;
  });
};
