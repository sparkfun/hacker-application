<?php

$apiConfig = [
	'name' => 'NASA Open API',
	'baseUrl' => 'https://api.nasa.gov/',
	'queryMethods' => ['GET'], // [ 'GET', 'POST' ]
	'queryParameters' => [
		'api_key' => [
			'type' => 'string',
			'format' => '{api_key}',
			'default' => 'RkZmeMuSHXMvKSyaDCXJuUZnNlPggonlc5TWH1yr',//'DEMO_KEY',
			'description' => 'api.nasa.gov key for expanded usage',
			'required' => true
		]
	],
	'methods' => [
		'planetary/apod' => [
			'name' => 'Astronomy Picture of the Day',
			'callable' => true,
			'description' => 'One of the most popular websites at NASA is the Astronomy Picture of the Day. In fact, this website is one of the most popular websites across all federal agencies. It has the popular appeal of a Justin Bieber video. This endpoint structures the APOD imagery and associated metadata so that it can be repurposed for other applications. In addition, if the concept_tags parameter is set to True, then keywords derived from the image explanation are returned. These keywords could be used as auto-generated hashtags for twitter or instagram feeds; but generally help with discoverability of relevant imagery.',
			'queryParameters' => [
				'date' => [
					'type' => 'date',
					'format' => 'YYYY-MM-DD',
					'default' => 'today',
					'description' => 'The date of the APOD image to retrieve',
					'required' => true
				],
				'hd' => [
					'type' => 'bool',
					'format' => false,
					'default' => 'False',
					'description' => 'Retrieve the URL for the high resolution image',
					'required' => false
				]
				
			],
			'responseHandler' => 'function(data, xhr) {
				console.log(data);
				console.log(xhr);
			}'
		],
		'neo/rest/v1/' => [
			'name' => 'Asteroids - NeoWs',
			'callable' => false, //This level of the API cannot be queried, but the below 'methods' can
			'description' => 'NeoWs (Near Earth Object Web Service) is a RESTful web service for near earth Asteroid information. With NeoWs a user can: search for Asteroids based on their closest approach date to Earth, lookup a specific Asteroid with its NASA JPL small body id, as well as browse the overall data-set.\nData-set: All the data is from the NASA JPL Asteroid team (http://neo.jpl.nasa.gov/).\nThis API is maintained by SpaceRocks Team: David Greenfield, Arezu Sarvestani, Jason English and Peter Baunach.',
			'methods' => [
				'feed' => [
					'name' => 'NeoWs - Feed',
					'description' => 'Retrieve a list of Asteroids based on their closest approach date to Earth',
					'queryParameters' => [
						'start_date' => [
							'type' => 'date',
							'format' => 'YYYY-MM-DD',
							'default' => 'none',
							'description' => 'Starting date for asteroid search',
							'required' => true
						],
						'end_date' => [
							'type' => 'date',
							'format' => 'YYYY-MM-DD',
							'default' => '7 days after start_date',
							'description' => 'Ending date for asteroid search',
							'required' => false
							
						]
					]
				],
				'neo/:SPK-ID' => [
					'name' => 'NeoWs - Lookup',
					'description' => 'Lookup a specific Asteroid based on its NASA JPL small body (SPK-ID) ID',
					'queryParameters' => [
						'SPK-ID' => [
							'type' => 'integer',
							'format' => false,
							'default' => 'none',
							'description' => 'NASA JPL small body (SPK-ID) ID',
							'restfulParameter' => true,
							'required' => true
						],
					]
				],
				'neo/browse' => [
					'name' => 'NeoWs - Browse',
					'description' => 'Browse the overall Asteroid data-set',
					'queryParameters' => []
				]
				
			],		
			'queryParameters' => []
		]
	]
];