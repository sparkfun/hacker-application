<?php

$apiConfig = [
	'name' => 'Etymonline.com (scrape)',
	'baseUrl' => 'http://www.etymonline.com/index.php',
	'callable' => true,
	'queryMethods' => [ 'GET' ],
	'queryParameters' => [
		'l' => [
			'type' => 'string',
			'format' => 'a',
			'default' => 'a',
			'description' => 'Alpha character',
			'required' => true
		],
		'p' => [
			'type' => 'int',
			'format' => 0,
			'default' => 0,
			'description' => 'Page number (0-based)',
			'required' => false
		]
	],
	'methods' => [],
	/* Not returning <script> anymore
	'responseHandler' => json_encode('function(data, xhr) {
	var eleSelector = xhr._options.eleSelector || false;
	var finalData = data;
	console.log("CustomResponse for etymonline!");
	console.log(eleSelector);
	console.log(data);
	console.log(xhr);
	if (typeof eleSelector == "string" && eleSelector.length > 0) {
		finalData = $(data).find(eleSelector);
		console.log(finalData);
	}
	return finalData;
	
}')*/
];