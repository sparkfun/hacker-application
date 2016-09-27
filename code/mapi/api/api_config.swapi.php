<?php

$apiConfig = [
	'name' => 'Star Wars API',
	'baseUrl' => 'https://swapi.co/api/',
	'queryMethods' => ['GET'],
	'queryParameters' => [
		'format' => [
			'type' => 'string',
			'format' => '{format}',
			'default' => '',
			'description' => 'Encodings - default is JSON.  Send \'wookiee\' to translate all content to Wookiee.',
			'required' => false
		]
	],
	'methods' => [
		'films' => [
		
		],
		'people' => [
		
		],
		'planets' => [
		
		],
		'species' => [
		
		],
		'vehicles' => [
		
		]
	]
	
];