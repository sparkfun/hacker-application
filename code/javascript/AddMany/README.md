#AddMany:
In simplest terms allows relationships between posts.
The visual interface gives WordPress admin the ability to assign one-to-many relationships with children that share no other parents. You can also allow many-to-many relationships where children may have many parents and vice versa. You can even create shared fields between parent and children which is important for things like products that may change price at different store locations. More on that later.

Similar to ACF (Advanced Custom Fields), AddMany has the ability to create and repeat sets of fields. The main difference being, it puts control back into the hands of the developer and allows you to write custom MySQL queries if need be.

##Use Cases
 * relate posts to other posts
 * control the order of posts (custom post types)
 * assign modules or panels to a layout that are customizable
 * repeat an arbitrary number of fields (like ACF repeater)
 * overriding a post(s) fields on a case by case basis without affecting the original
 * create site navigation (future option)
 * keeps context by allowing you to create child posts on the same page

##Requirements

AddMany would not be possible without [The TacoWordPress framework – An ORM for custom post types.] (https://github.com/tacowordpress/tacowordpress) This is a requirement.

######Other requirements:
 * PHP >= 5.4
 * Knowledge of requiring packages through Composer
 * Prior knowledge of TacoWordpress
 * Object-oriented programming

######Built with [React](https://facebook.github.io/react/) and PHP

##Installation
Depending on where you put your project's vendor directory, installation may or may not work. A solution is currently being worked on to resolve this.

In your project's composer.json file, add the packages below in the require section:

```
"require": {
  "tacowordpress/tacowordpress": "dev-master",
  "tacowordpress/addmany": "dev-master",
  "tacowordpress/util": "dev-master"
}
```

In your theme's function file, add the below:
```php

// Add this so your project has access to Composer's autoloaded files.
// Please replace "{path_to_autolaod}".
require_once '{path_to_autoload}/autoload.php';

// Make sure to initialize the core dependency of AddMany, "TacoWordPress".
\Taco\Loader::init();

// Initialize AddMany
\Taco\AddMany\Loader::init();

```

##Example Usage
With the examples below, you should have prior knowledge of how TacoWordPress works. If not, please consult the docs here:
https://github.com/tacowordpress/tacowordpress/wiki.

###One-to-Many
```php

// Example configuration for a basic AddMany Field

  public function getFields() {
    return [
      'staff_members' => \Taco\AddMany\Factory::create(
        [
          'first_name' => ['type' => 'text'],
          'last_name' => ['type' => 'text'],
          'bio' => ['type' => 'textarea']
        ],
        ['limit_range' => [2, 3]] // Enforce a minimum of 2 items, but no more than 3.
       )->toArray()
    ];
  }
```

###Many-to-Many (AddBySearch)

```php
// Example configuration for an AddMany field with AddBySearch
// Adds a search field for querying posts via AJAX

  public function getFields() {
    return [
      'employees' => \Taco\AddMany\Factory::createWithAddBySearch('Employee')->toArray()
    ];
  }
 ```

###Many-to-Many with unique common fields between 2 posts (like a junction table)
In this example, the shared fields are between the parent post and the child posts of "products".
 ```php
// Example AddBySearch with shared fields

class Store extends \Taco\Post {
  public function getFields() {
    return [
      'products' => \Taco\AddMany\Factory::createWithAddBySearch('Product',[
        'price' => ['type' => 'text'],
        'tax' => ['type' => 'text']
      ])->toArray()
    ];
  }
 }
 ```
 Because the above will reference external "product" posts, you have the ability to extend their values ("price" and "tax" is a good use case) while also keeping their original values. This is useful for creating products and allowing them to slightly vary between stores.



###One-to-Many with field variations

 ```php

// Example AddMany field with field variations – Adds a dropdown for users to select

  public function getFields() {
    return [
      'staff_members' => \Taco\AddMany\Factory::create(
        [
          'board_members' => [
            'first_name' => ['type' => 'text'],
            'last_name' => ['type' => 'text'],
            'bio' => ['type' => 'textarea']
          ],
          'general_staff' => [
            'first_name' => ['type' => 'text'],
            'last_name' => ['type' => 'text'],
            'department' => ['type' => 'select', 'options' => $this->getDepartments()]
          ],
        ]
       )->toArray()
    ];
  }
```

###One-to-One
```php

// You can simulate a one-to-one relationship by limiting the number of items to 1

class Person extends \Taco\Post {
  public function getFields() {
    return [
      'spouse' => \Taco\AddMany\Factory::create(
        [
          'first_name' => ['type' => 'text'],
          'phone' => ['type' => 'text']
        ],
        ['limit_range' => [0, 1]] // Do not allow more than 1 item to be added
       )->toArray()
    ];
  }
 }
```


##Getting a post's relations


In your template you can get related posts by accessing the field name through your object,
e.g. ```$blog_post->related_posts```
This will return a collection of post objects.

In order to utilize the above, you must use the AddMany Mixin within your class.

```php

class Post extends \Taco\Post {
  use \Taco\AddMany\Mixins;
  ...
```
This will let you do the following:

```php
// In your template (Example)

$blog_post = \Taco\Post\Factory::create($post); ?>

<?php foreach($blog_post->related_posts as $rp): ?>
   <?= $rp->post_title; ?>
  ...
<?php endforeach; ?>

```
######What if no related posts exist in the object?
In other words, the admin did not manually select them.
You can define a fallback method. This will alow for cleaner code in your template by removing any logic.

This example shows a method that is defined in the Post class:
```php
  public function getFallBackRelatedPosts($key) {
    global $post;
    $post_id = (is_object($post) && isset($post->ID))
      ? $post->ID
      : null;
    if($key === 'related_posts') {
      return \Taco\Post::getWhere(['posts_per_page' => 3, 'exclude' => $post_id]);
      // The above actually just gets the 3 most recent posts, excluding the current one.
      // This is a poor example. Don't be this lazy!
    }
  }
```

IMPORTANT: The method you define must be named "getFallBackRelatedPosts". It can handle more than one field if you allow it. Just create a switch statement or some logic to check the key and then return the appropriate posts.


##Getting original values of a referenced post if overwritten
With AddMany you can override values from a post that you reference through AddBySearch. This is extremely useful if you have a template (of some sort) or even a product that may need its values replaced without having to recreate it.

Let's say there are a chain of stores that all carry the same product/s but the prices vary from location to location.
The following code will allow this:

```php

class Store extends \Taco\Post {
  use \Taco\AddMany\Mixins;

  public function getFields() {
    return [
      'products' => \Taco\AddMany\Factory::createWithAddBySearch('Product', [
        'price' => ['type' => 'text']
      ])->toArray()
    ];
  }
  ...
```

The admin interface will give you the ability to find and add the products to a list. This list will contain the products with an additional field of "price". Typing a value in these fields will override it but not replace the original.

To understand this concept better, let's create the template code that displays some basic product information.

```php
/* Single Store - single-store.php */
<?php $store = Store::find($post->ID); // load the store and iterate through the products

foreach($store->products as $product): ?>
  Product title: <?= $product->getTheTitle(); ?><br>
  product price: $<?= $product->get('price'); ?><br>
  Original price: $<?= $product->original_fields->price; ?> <br>
  Savings: $<?= $product->original_fields->price - $product->get('price'); ?><br><br>
<?php endforeach; ?>
```
By accessing the property of "original_fields", you will get the original value while keeping the new value.
`$product->original_fields->price;`

This is also useful to show product savings after a reduction in price.



##Convenience methods
The Factory class of AddMany has a few convenience methods to make your code just a little cleaner:

Basic AddMany
`\Taco\AddMany\createAndGet()` will return the array without having to call `->toArray()`

AddMany with AddBySearch

`\Taco\AddMany\createAndGetWithAddBySearch()` will also return an array


##Contributing (Coming soon)
