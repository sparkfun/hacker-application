////////////////////////////////////////////////////////////
// Cart Service
//
// Services for handling cart functionality
////////////////////////////////////////////////////////////

angular.module('wctApp')
.service('CartService',
  [ '$http', '$q', '$stateParams',
    'ContextService',
    'Show', 'ShowDate', 'ShowTicket',
    'Product', 'ProductSku',
    'moment',
    function($http, $q, $stateParams,
             ContextService,
             Show, ShowDate, ShowTicket,
             Product, ProductSku,
             moment){
      
      // TODO assign ttl for items - merch can live a long time - but not tickets
      
      var _self = this;
      
      // construct an ad-hoc cartitem for displaying in cart
      var cartItem = function(kind, qty, item, description){
        this.idx = kind + '_' + item.id;// essentially a unique id
        this.kind = kind;
        this.qty = qty;
        this.description = description;
        this.item = item;
        //its either productsku or show
        this.itemPrice = item.price;
        this.lineItemTotal = function(){
          return this.qty * this.itemPrice;
        }
        this.mainDesc = (this.description.length) ? this.description[0] : '';
        this.secondaryDesc = (this.description.length > 1) ? this.description.slice(1) : [];
      }
      
      // loop thru items and calculate total
      this.getCartTotal = function(){
        return this.cartBasket.reduce(function(prev, cur){
          return prev += cur.lineItemTotal();
        }, 0);
      };
      
      // loop thru items and calculate total qty
      // some directives need functions to operate correctly
      this.itemsInCartTotal = function(){
        return this.cartBasket.reduce(function(prev, cur){
          return prev += cur.qty;
        }, 0);
      };
      
      // construct item description for showTicket and add to cart
      this.addShowTicketToCart = function(item){
        var description = [];
        description.push(item._parentShowDate._parentShow.name);
        description.push(item._parentShowDate._parentShow.venue);
        description.push(moment(item._parentShowDate.dateofshow)
        .format('ddd YYYY/MM/DD hh:mm'));
        
        if(item.name && item.name.trim().length > 0){
          description.push(item.name);
        }
        description.push(item.ages);
        
        var cItem = new cartItem('ticket', 1, angular.copy(item), description);
        this.addItemToCart(cItem);
      };
      
      // construct item description for product and add to cart
      this.addProductToCart = function(item){
        var description = [];
        description.push(item._parentProduct.name);
        if(item.name && item.name.trim().length > 0){
          description.push(item.name);
        }
        var cItem = new cartItem('product', 1, angular.copy(item), description);
        this.addItemToCart(cItem);
      };
      
      // add the item (product/ticket) to the cart
      // ensure only one type of item in the cart
      this.addItemToCart = function(cartItem){
        //determine quantity
        var existing = this.cartBasket.filter(function(itm){
          return cartItem.kind === itm.kind && cartItem.item.id === itm.item.id;
        });
        // only allow one of each item type
        // TODO up to max per order
        if(!existing.length){
          this.cartBasket.push(cartItem);
        }
        this.saveCartBasket();
      };
      
      // respond to qty updates
      this.updateQuantity = function(changeItem){
        var newQty = changeItem.qty;
        if(newQty === 0){
          //remove from array
          this.cartBasket = this.cartBasket.filter(function(e){
            return e.idx !== changeItem.idx;
          })
        }
        this.saveCartBasket();
      };
      
      // serialize cart to local storage
      this.saveCartBasket = function(){
        localStorage.setItem('wctCart', JSON.stringify(this.cartBasket));
      };
      
      // remove items from the cart and remove from local storage
      // institute a new cart id for uniqueness
      // update the cart in local storage
      this.clearCart = function(){
        this.cartBasket = [];
        localStorage.removeItem('wctCartId');
        this.initCartId();
        
        localStorage.removeItem('wctCart');
        this.saveCartBasket();
      };
      
      // TODO generate a more robust/unique id
      this.genId = function(){
        return moment.utc().valueOf();
      };
      
      // reinstate a saved cart
      this.initBasket = function(){
        var cart = localStorage.getItem('wctCart');
        if(cart){
          var crt = JSON.parse(cart).map(e => new cartItem(e.kind,e.qty,e.item,e.description));
          return crt;
        } else {
          return [];
        }
      };
      
      // get any existing cart id - or create a new one
      this.initCartId = function(){
        var cartId = localStorage.getItem('wctCartId');
        if(cartId){
          return JSON.parse(cartId);
        } else {
          cartId = this.genId();
          localStorage.setItem('wctCartId', JSON.stringify(cartId));
        }
      };
      
      // init!!!!
      this.cartId = this.initCartId();
      this.cartBasket = this.initBasket();
      
      // Save the cart to the db - performed at checkout
      this.saveCartToDb = function(){
        // call api to save cart to db
        return $http.post('/api/store/cartrecord', {
          cart: JSON.stringify({idx: this.cartId, items: this.cartBasket})
        })
      };
      
    }]);
    