# SiteWideSearch

** Note to Sparkfun: ** This is not actually a repo on GitHub. This functionality was used on multiple projects at my current job and was changed a bit for presentation purposes. I've included this README for the purposes of showing how I would document something like this.

## What is it?
**SiteWideSearch** is an extension of "TacoWordPress" that allows full-text querying of data from WordPress post types and custom fields. It is designed to work with custom post type fields and allows the developer to configure the ranking system for how the search results are returned.

## Requirements
* PHP 5.4+
* [TacoWordPress](https://github.com/tacowordpress/tacowordpress)
* Knowledge of Composer
* [Taco Theme](https://github.com/tacowordpress/taco-theme) – Makes integrating with TacoWordPress and SiteWideSearch easier, but is not required.
* Post types must be setup using TacoWordPress or SiteWideSearch will not work.

#### Limitations
This functionality was designed to return more relevant results (quality vs. quantity) and does not provide pagination options. There are long term plans to provide this in the future.
To work around this, you can override the "getSearchResults" method in the child class to change ordering and offset.

## Setup
You must be autoloading TacoWordPress through Composer. Please see basic installation and setup instructions [here](https://github.com/tacowordpress/tacowordpress) before moving forward.

In the SiteWideSearch directory, you will find an already extended class for your convenience. The existence of this class is for allowing the developer to configure how the search works. You can however, use it to override methods.

The bulk of config is in the ```getSearchableFields``` method. This method will allow you to define an array of post types, fields and associated points for ranking purposes.

```php
<?php
// Example configuration for the post types of "page" and "resource"

public static function getSearchableFields() {
  return array(
    'page' => array(
      // Lower integers have less points and will rank lower in search results.
      // "Post content" is ranked lower due to the number of words that exist in body copy.
      'post_content' => 2,
      'post_title' => 4,
      // Higher integers rank higher. "post_title" and "post_title" contain less words
      // and are more specific and relevant at describing the post.
      'post_name' => 6
    ),
    'resource' => array(
      'post_title' => 2,
      'post_name' => 4,
      'post_content' => 3,
      'resource_authors' => 7,
      'terms' => 3,
      'taxonomies' => 2
    )
  );
}
```

#### Specifying which fields should be returned (JSON only)
The ```getAllowedJSONFields``` method is used for telling SiteWideSearch to only return the specified fields in JSON format because it doesn't make sense to return everything.

#### Limiting the number of results

The ```$site_wide_search_results_limit``` property can be used to change the default of 20.


## How it works
This is not a plugin and therefor will always be active behind the scenes of WordPress. During certain WordPress hooks, a script will check if a table for SiteWideSearch exists, and if not, one will be created. Adding or changing configuration in the "getSearchableFields" method will result in the search table being modified.

If a post is created, modified or deleted, the search results table will be modified to reflect the changes.

## Getting results
Example:
```php
<?php
$results = SiteWideSearch::getSearchResults($keywords); ?>

<?php foreach($results as $r): ?>
  <h1><?= $r->post_title; ?></h1>
  <?= $r->getTheContent(); ?>
<?php endforeach; ?>

```
