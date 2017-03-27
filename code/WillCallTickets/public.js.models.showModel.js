angular.module('wctApp').factory('Show',
  ['$http', '$q', 'moment', function($http, $q, moment){
    
    function Show(row, dateModels = null){
      this.id = row.id;
      this.created_at = row.created_at;
      this.updated_at = row.updated_at;
      this.member_id = row.member_id;
      this.venue = row.venue;
      this.url = row.url;
      this.name = row.name;
      this.description = row.description;
      this.images = row.images;
      this.active = row.active;
      this.announcedate = row.announcedate;
      this.enddate = row.enddate;
      this.showdates = [];
      
      if(dateModels && dateModels.length > 0){
        this.showdates = dateModels.map(function(e){
          e.parentShow(row);
          return e;
        });
      }
    };
    
    Show.prototype = {
      firstDate: function(){
        return moment(this.showDates.map(e => e)
          .sort((a,b) => a.dateofshow - b.dateofshow)[0].dateofshow)
        .format('YYYY/MM/DD hh:mm a');
      },
      lastDate: function(){
        return moment(this.showDates.map(e => e).reverse()
          .sort((a,b) => a.dateofshow - b.dateofshow)[0].dateofshow)
        .format('YYYY/MM/DD hh:mm a');
      },
      dateRange: function(){
        if(this.firstDate() === this.lastDate()){
          return this.firstDate();
        }
        return this.firstDate() + ' - ' + this.lastDate();
      },
      showtickets: function(){
        return this.showdates.reduce(function(prev,cur){
          return prev = prev.concat(cur.showtickets);
        },[]);
      },
      // only issue a warning if it will cause a show not to display tickets
      ticketWarningIfEmpty: function(){
        return this.showtickets().filter(function (itm) {
          return itm.active && itm.available() > 0 && itm.price > 0;
        });
      }
    };
    
    // STATIC methods
    
    // process form input
    Show.processForm = function(form, input, currentShow){
      
      var deferred = $q.defer();
      
      // console.log('FORM', form)
      // console.log('INPUT', input)
      // console.log('CURRENT', currentShow)
      
      var errors = [];
      
      $http.post('/api/shows', {
        input: input,
        current: currentShow
      })
      .then(function(data){
        var returnData = data.data;
        deferred.resolve(returnData);
      })
      .catch(function(err){
        //convert err to array and return
        errors.push(err.data);
        deferred.reject(errors);
      })
      
      return deferred.promise;
    };
    
    
    // Convert db rows and showDate collection to Show Objects
    Show.buildShowCollection = function(showRows, dateModels) {
      // console.log('building...', dateRows)
      return showRows.map(function (show) {
        var matches = dateModels.filter(e => e.show_id === show.id);
        if (matches.length > 1) {
          matches.sort((a, b) => a.dateofshow - b.dateofshow);
        }
        return new Show(show, matches);
      });
    };
    
    return Show;
    
  }]);
