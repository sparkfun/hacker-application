var container, module, controllerScope, cs;

module = angular.module('alphaGame', []);

module.controller('alphaGameCtrl', function($scope, appStorage) {
//module.controller('alphaGameCtrl', ['$scope', function($scope) {
	controllerScope = $scope;
	cs = controllerScope;
	
	$scope.appStorage = appStorage;
	
	/* BEGIN GAME CONTROL */
	
	$scope.gameState = 'init'; //Options: init, encounter, minigame, map
	$scope.prevGameState = false;
	
	$scope.gameOptions = {
		'sound': {
			'music': 100,
			'sfx': 100,
			'voice': 75
		},
		'language': {
			'foreignLanguageEnabled': true,
			'foreignLanguage': 'ElderFuthark',
			'foreignLanguageSkill': 0
		}
	};
	
	/* END GAME CONTROL */
	
	/* BEGIN GAME MAP */
	$scope.map = {};
	$scope.map.perRow = 15;
	$scope.map.numRows = 10;
	$scope.map.currentMap = false;
	
	$scope.partyMapX = 5;
	$scope.partyMapY = 7;
	
	$scope.tileset = {
		"moveTiles": [
			{"name":"Grass","style":{"background":"#00CC00","border":"1px green solid"},"chanceForEncounter":15},
			{"name":"Forest","style":{"background":"#00FF00","border":"1px brown solid"},"fa":"fa fa-tree","chanceForEncounter":25},
			{"name":"DirtRoad","style":{"background":"#996633"},"chanceForEncounter":5},
			{"name":"TownRoad","style":{"background":"#D6C2AD"},"chanceForEncounter":0}			
		],
		"blockTiles": [
			{"name":"Mountain","style":{"background":"#472400"},"canMove":false},
			{"name":"River","style":{"background":"#0000EE"},"canMove":false}
		],
		"townTiles": [
			{"name":"Town","style":{"background":"#CCEE00"},"fa":"fa fa-home","chanceForEncounter":0}
		],
		"dungeonTiles": [
			{"name":"Dungeon of Dastardly Doom","style":{"background":"#EE0055"},"fa":"fa fa-level-down","chanceForEncounter":0,"moveRequirements":[{"type":"stat","stat":"level","amount":2}]},
			{"name":"Hyacinth Labyrinth","style":{"background":"#EE0055"},"fa":"fa fa-level-down","chanceForEncounter":0,"moveRequirements":[{"type":"stat","stat":"level","amount":2}]}
		]
	};
	
	/* END GAME MAP */
	
	/* BEGIN ALPHA GRID */
	$scope.perRow = 4;
	$scope.numRows = 4;
	$scope.grid = [];
	$scope.vowels = 0; //Vowel count on the grid
	$scope.consonants = 0; //Consonant count on the grid
	$scope.vowelCodes = [ 65, 69, 73, 79, 85 ]; //ASCII vowel codes
	$scope.consonantCodes = [ 66, 67, 68, 70, 71, 72, 74, 75, 76, 77, 78, 80, 81, 82, 83, 84, 86, 87, 88, 89, 90 ]; //ASCII consonant codes
	
	$scope.futureTileSize = 4;
	$scope.futureTiles = [];
	
	$scope.letterValues = [ 1, 1, 1, 1, 1, 2, 2, 2, 1, 2, 3, 1, 2, 1, 1, 2, 4, 1, 1, 1, 1, 2, 2, 4, 2, 3 ]; //Values for each A - Z
	
	$scope.spellWord = []; //Word currently being spelled
	$scope.spellWordSnippets = []; //Pieces of a word, including Concentrations
	$scope.isValidWord = false;
	
	$scope.partyTileBankSize = 4;
	$scope.partyTileBank = [];
	$scope.partyTileTrash = [];
	/* END ALPHA GRID */
	
	/* BEGIN UI PARAMETERS */
	$scope.activeUiEntity = false; //Entity displayed in center UI box
	$scope.activeUiEntityType = false;	
	
	$scope.selectedPlayerCategory = false; //Player.combatOptions main menu, e.g. 'Attack', 'Spell', etc
	$scope.selectedPlayerAbility = false; //combatOptions sub-selecton
	$scope.selectedTargets = [];
	/* END UI PARAMETERS */
	
	/* BEGIN MODEL DATA */
	$scope.playerGear = [
	 //{
		{"id":1,"name":"Spectacular Spectacles","notes":"Awesome eyewear. +3 LCK, +4 HP","slot":"head","bonuses":[{"stat":"lck","bonus":3},{"stat":"maxHp","bonus":4}]},
		{"id":2,"name":"Scholarly Robes","notes":"Standard student uniform. +1 DEF","slot":"torso","bonuses":[{"stat":"def","bonus":1}],"equipped":true},
		{"id":3,"name":"Pontificating Pantaloons","notes":"Pants. +1 DEF, +1 MP","slot":"legs","bonuses":[{"stat":"def","bonus":1},{"stat":"maxMp","bonus":1}]},
		{"id":4,"name":"Porous Thesaurus","notes":"A slightly hole-y handbook  for lazy poets everywhere. +1 ATK","slot":"lhand",
			"bonuses":[{"stat":"atk","bonus":1}],
			"abilities":[ 1, 2 ]
		},
		{"id":5,"name":"Pen of Perilous Platitudes","notes":"Be careful what you wish for, even if it's well-wishes for your enemy. +2 ATK","slot":"rhand","bonuses":[{"stat":"atk","bonus":1}],"abilities":[{"name":"Mightier Than","notes":"Strike a solid line through a single enemy.","target":"enemy","damageBonus":0.35},{"name":"Stunning Strikethrough","notes":"Attack all enemies with a chance to Stun.","target":"enemies","damageBonus":0,"chance":{"stun":0.25},"cost":{"mp":1}}]},
		{"id":6,"name":"Duncen's Cap","notes":"Reduces ATK and DEF by 3.  A terrible item.  Don't wear this.","slot":"head","bonuses":[{"stat":"atk","bonus":-3},{"stat":"def","bonus":-3}]}
	 //}
	];
	
	$scope.items = [
		//{
		{"id":1,"name":"Healing Swig","notes":"Restores 20 HP."},
		{"id":2,"name":"Mnemonic Noms","notes":"Restores 10 MP."}
		//}
	];
	
	$scope.spells = [
		//{
		{"id":1,"name":"Burning Words","notes":"Add a Fire bonus to four tiles.","action":{"tileEffect":"fire","count":4},"cost":{"mp":1}},
		{"id":2,"name":"Cold Scold","notes":"Add an Ice bonus to four tiles.","action":{"tileEffect":"ice","count":4},"cost":{"mp":1}},
		{"id":3,"name":"Catholicon","notes":"Quickly bind your wounds to assuage your pain (but not your guilt).","action":{"heal":1,"amountMin":5,"amountMax":10},"cost":{"mp":1}},
		{"id":4,"name":"Medicament","notes":"Convert all vowels to Healing Tiles.  Point values for these tiles will restore triple their value in HP.","action":{"tileEffect":"heal","tileType":"vowel"},"cost":{"mp":1,"hp":2}}
		//}
	];
	
	$scope.abilities = [
		{"id":1,"name":"Spinal Tap","notes":"Smack a single enemy with the leather-bound spine of your thesaurus.","target":"enemy","damageBonus":0.35},
		{"id":2,"name":"Leafy Assault","notes":"Fling sharp, knowledgeable pages at all enemies with a chance to Weaken Defense.","target":"enemies","damageBonus":0,"cost":{"mp":1}}		
	];
	
	$scope.player1 = {
		"id": 1,
		"name": "Player1",
		"stats": {
			"level": 1,
			"hp": 25,
			"maxHp":25,
			"mp":5,
			"maxMp":5,
			"atk":1,
			"def":1,
			"lck":3,
			"con":2,
			"xp": 5,

			"xpToLevel": 100,
		},
		"items":{
			"head": false,
			"torso": false,
			"legs": false,
			"lhand": false,
			"rhand": false
		},
		"spells":[
			1, 2
		],
		"abilities":[
			
		],
		"index":0, //0-based array index in $scope.players[]
		"inventory":[
			{"id":1,"type":"playerGear","equipped":true},
			{"id":2,"type":"playerGear","equipped":true},
			{"id":3,"type":"playerGear","equipped":true},
			{"id":4,"type":"playerGear","equipped":true
			},
			{"id":5,"type":"playerGear","equipped":true},
			{"id":1,"type":"items","count":3},
			{"id":2,"type":"items","count":3},
			{"id":6,"type":"playerGear","equipped":false}
		],
		"fullDisplay":false, //Show or hide Player UI
		"showTab":"main", //UI tab to display
		"combatOptions":[],
		"concentrationWord":[],
		"holdConcentrationWord":false
	};
	
	$scope.player2 = {
		"id": 2,
		"name": "P2",
		"stats": {
			"level": 1,
			"hp": 25,
			"maxHp":25,
			"mp":5,
			"maxMp":5,
			"atk":1,
			"def":1,
			"lck":3,
			"con":4,
			"xp": 1,

			"xpToLevel": 100,
		},
		"items":{
			"head": false,
			"torso": false,
			"legs": false,
			"lhand": false,
			"rhand": false
		},
		"spells":[
			3, 4
		],
		"abilities":[
			
		],
		"index":1, //0-based array index for self in $scope.players[]
		"inventory":[],
		"fullDisplay":false,
		"showTab":"main",
		"combatOptions":[],
		"concentrationWord":[],
		"holdConcentrationWord":false
	};
	
	$scope.players = [ $scope.player1, $scope.player2 ];


	
	$scope.monsters = [
		{"id":1,"name":"Wordworm","stats":{"hp":20,"atk":1,"def":1,"lck":2,"mp":3},"notes":"A tiny annelid that subsists on lexicography.","rewards":{"xp":3}},
		{"id":2,"name":"Gnarly Gnat ","stats":{"hp":16,"atk":2,"def":1,"lck":2,"mp":2},"notes":"Tubular little flying dude. Beware the poisonous proboscis.","rewards":{"xp":4}},
		{"id":3,"name":"Slippery Sloth","stats":{"hp":24,"atk":1,"def":2,"lck":4,"mp":3},"notes":"A greased-up, slow-moving tree dweller.","rewards":{"xp":4}},
		{"id":4,"name":"Surly Skeleton","stats":{"hp":20,"atk":2,"def":1,"lck":3,"mp":5},"notes":"This skeleton is rather bitter at having shuffled off its mortal coil.  You bear the brunt of its ire through a few cutting insults.","rewards":{"xp":5}}
		
	];
	
	$scope.encounter = { //Current battle
		partyInEncounter: false,
		enemies: [],
		players: [],
		initiatives: {
			players:[],
			enemies:[]
		},
		concentrations:[], //Player-held concentration snippets
		currentPlayerTurn: 0,
		currentEnemyTurn: -1,
		currentTurn: 'player',
		currentEntity: false,
		currentEntityAction: false,
		currentEntityActionCategory: false,
		currentEntityTarget: false,
		currentTurnComplete: false,
		chainActionFromEntity:false,
		lastTurnResult: { 'type': false, 'result': false }
		
	};
	
	$scope.previousEncounter = false;
	
	$scope.genericActions = [
		{"name":"Attack","category":"abilities"},
		{"name":"Spell","category":"spells"},
		{"name":"Defend","category":"$scope.players"},
		{"name":"Flee","category":false}
	];
	
	$scope.playerGenericActions = [
		{"name":"Concentrate","category":"player"}
	];
	/* END MODEL DATA */
	
	/* BEGIN PLAYER FUNCTIONS */
	$scope.initPlayers = function() {
		angular.forEach($scope.players, function(player, _i) {
			//var combatOptions = {"Attack":[],"Spell":[],"Defend":[],"Flee":[]};
			var combatOptions = $scope.genericActions.slice();
			//combatOptions = combatOptions.join($scope.playerGenericActions);
			Array.prototype.push.apply(combatOptions, $scope.playerGenericActions);
			console.log(':::: init player:'+player.name);
			angular.forEach(player.inventory, function(item, _j) { //Process inventory
				if (item.hasOwnProperty("slot") && item.hasOwnProperty("equipped") && item.equipped === true) {
					$scope.equipItem(player, item); //Equip gear
				}
			});
			player.combatOptions = combatOptions;
		});
	};
	
	$scope.equipItem = function(player, item) {
		var slot, curItem;
		if (!player || !item) {
			return false;
		}
		slot = item.slot;
		console.log('equip: ' + item.name);
		curItem = $scope.player.items[slot];
		if (curItem) { //Unequip previous item
			if (curItem.name != item.name) {
				$scope.unequipItem(player, curItem);
			} else {
			console.log('ALREADY EQUIPPED '+ item.name +' to '+ player.name);
			return false;
			}
			
		}
		if (item.hasOwnProperty("abilities")) {
			angular.forEach(item.abilities, function(ability, _i) {
				$scope.addPlayerAbility(player, ability, item);
			});
		}
		if (item.hasOwnProperty("bonuses")) {
			angular.forEach(item.bonuses, function(bonus, _i) {
				$scope.addPlayerBonus(player, bonus);
			});
		}
		player.items[slot] = item;
		return true;
	};
	
	$scope.unequipItem = function(player, item) {
		var slot;
		if (!player || !item)
			return false;
		slot = item.slot;
		console.log('unequip '+item.name +' from '+ player.name);
		if (item.hasOwnProperty("abilities")) {
			angular.forEach(item.abilities, function(ability, _i) {
				$scope.removePlayerAbility(player, ability, item);
			});
		}
		if (item.hasOwnProperty("bonuses")) {
			angular.forEach(item.bonuses, function(bonus, _i) {
				$scope.removePlayerBonus(player, bonus);
			});
		}
		player.items[slot] = false;
	};
	
	$scope.addPlayerBonus = function(player, bonus) {
		if (player.stats.hasOwnProperty(bonus.stat)) {
			player.stats[bonus.stat] += bonus.bonus;
		} else {
			console.log('-- BONUS: stat = '+ bonus.stat +' not found for player '+ player.name);
		}
		
	};
	
	$scope.removePlayerBonus = function(player, bonus) {
		if (player.stats.hasOwnProperty(bonus.stat)) {
			player.stats[bonus.stat] -= bonus.bonus;
		} else {
			console.log('-- BONUS: stat = '+ bonus.stat +' not found for player '+ player.name);
		}
	};
	
	$scope.addPlayerAbility = function(player, ability, item) {
		/* var type = 'basic';
		if (ability.hasOwnProperty("cost")) {
			if (ability.cost.hasOwnProperty("mp")) {
				type = 'mp';
			}
			if (abilitycost.hasOwnProperty("hp")) {
				type = 'hp';
			}
		}
		switch(type) {
		case 'mp':
		
			break;
		case 'hp':
		
			break;
		
		case 'basic':
		default:
			
		}
		*/
		//$scope.players[player.index].abilities.push({"ability":ability, "item": item});
		player.abilities.push({"ability":ability, "item": item});
	};
	
	$scope.removePlayerAbility = function(player, ability, item, index) {
		var removeMe;
		angular.forEach(player.abilities, function(a, _i) {
			if (ability.name == a.name) {
				removeMe = _i;
			}
		});
		
		player.abilities.splice(removeMe, 1);
		//player.abilities.splice(index, 1);
	};
	
	$scope.playerConcentrate = function(player) {
		var isValidWordSnippet = false;
		var concentrationWord = [];
		if ($scope.spellWord.length === 0) {
			console.log('Concentrate failed - no spellWord tiles selected');
			return false;
		}
		//Check if valid snippet
		if (player.concentrationWord.length > 0) {
			console.log(player.holdConcentrationWord +'/'+ typeof player.holdConcentrationWord);
			if (player.holdConcentrationWord != true) {
				console.log('Removing previous concentrationWord');
				concentrationWord = $scope.spellWord.slice();
				//isValidWordSnippet = $scope.checkWordSnippet(concentrationWord);
				isValidWordSnippet = true;
			} else {			
				console.log('Appending to previous concentrationWord');
				concentrationWord = player.concentrationWord.slice();
				Array.prototype.push.apply(concentrationWord, $scope.spellWord);
				//isValidWordSnippet = $scope.checkWordSnippet(concentrationWord + BLAHArray.join.apply
				isValidWordSnippet = true;
			}
		} else {
			//isValidWordSnippet = $scope.checkWordSnippet(concentrationWord);
			isValidWordSnippet = true;
			concentrationWord = $scope.spellWord.slice();
		}
		console.log('concentrate player ' + player.name + ' isVWS ' + isValidWordSnippet);
		console.log(concentrationWord);
		if (isValidWordSnippet === true) {
			player.concentrationWord = concentrationWord;
			$scope.removeAlphas(concentrationWord); //Update alphaGrid
			$scope.spellWord = $scope.spellWord.splice(); //Clear word
			//Finish turn
			//$scope.encounter.currentEntityTarget = player;
			$scope.encounter.currentEntityTarget = false;
			//$scope.encounter.currentEntityAction = 'concentrate';
			$scope.encounter.currentEntityAction = 'bank';
			//$scope.encounter.currentTurnComplete = true;
			//$scope.nextEncounterTurn();
			
		} else {
			console.log('Concentrate final error - concentrationWord:');
			console.log(concentrationWord);
		}
		
	};
	
	$scope.playerBankFinished = function() {
		var player;
		console.log('playerBankFinished...');
		if ($scope.encounter.currentTurn !== 'player') {
			console.log('Cannot playerBankFinished for non-player:' +$scope.encounter.currentTurn);
			return false;
		}
		$scope.encounter.currentTurnComplete = true;
		$scope.nextEncounterTurn();
	};
	
	$scope.fleeEncounter = function() {
		var canFlee = false;
		
		//canFlee = fleechexkFunc(); //FIXME
		canFlee = true;
		if (canFlee) {
			$scope.partyInEncounter = false;
			$scope.gameState = 'map';
		}
	};
	
	/* END PLAYER FUNCTIONS */
	
	/* BEGIN ENCOUNTER FUNCTIONS */
	$scope.generateEncounter = function(enemiesMin, enemiesMax, enemiesCnt, enemyLevel) {
		var toGenerate = 1;
		var enemies = [];
		var initiatives = {"players": $scope.players.slice()};
		var totalEnemies = $scope.monsters.length;
		//initiatives = $scope.players.slice();
		if (typeof enemiesCnt === "undefined" || !enemiesCnt || enemiesCnt <= 0) {
			toGenerate = $scope.getRandomInt(enemiesMin, enemiesMax, false, 1);
		}
		for (var i = 0; i < toGenerate; i++) {
			var enemy = $scope.monsters[$scope.getRandomInt(0, totalEnemies-1)];
			enemy.combatOptions = $scope.genericActions;
			enemies.push(enemy);
			//initiatives.push(enemy);
		}
		initiatives['enemies'] = enemies;
		
		$scope.encounter.initiatives = initiatives;
		$scope.encounter.enemies = enemies;
		$scope.encounter.players = $scope.players;
		
		$scope.encounter.currentTurn = 'player';
		$scope.encounter.currentPlayerTurn = 0;
		$scope.encounter.currentEnemyTurn = -1;
		$scope.encounter.currentEntity = $scope.players[0];
		$scope.encounter.partyInEncounter = true;
	};
	
	$scope.nextEncounterTurn = function() {
		var cur = $scope.encounter.currentTurn;
		var curIndex, next, nextEntity;
		if ($scope.encounter.currentTurn == 'player' && $scope.encounter.currentTurnComplete !== true) {
			alert('Turn not finished for ' + cur + ': ' + $scope.encounter.currentEntity.name);
			return false;
		}
		if (cur !== 'player') {
			next = 'player';
			$scope.encounter.currentPlayerTurn += 1;
			if ($scope.encounter.currentPlayerTurn >= $scope.encounter.initiatives.players.length) {
				$scope.encounter.currentPlayerTurn = 0;
			}
			nextEntity = $scope.encounter.players[$scope.encounter.currentPlayerTurn];
		} else {
			next = 'enemy';
			$scope.encounter.currentEnemyTurn += 1;
			if ($scope.encounter.currentEnemyTurn >= $scope.encounter.initiatives.enemies.length) {
				$scope.encounter.currentEnemyTurn = 0;
			}
			nextEntity = $scope.encounter.enemies[$scope.encounter.currentEnemyTurn];
			
		}
		if (nextEntity) {
			$scope.encounter.currentEntityAction = false;
			$scope.encounter.currentEntityActionCategory = false;
			$scope.encounter.currentEntityTarget = false;
			
			$scope.encounter.currentTurnComplete = false;
			$scope.encounter.currentTurn = next;
			$scope.encounter.currentEntity = nextEntity;
			if (next === 'enemy') {
				//$scope.randomEnemyAction(nextEntity); //Automated Enemy turn
			} else if (next === 'player') {
				$scope.playerTurnInit();
			}
		} else {
			alert('failed to go to next turn');
			console.log($scope.encounter);
		}
		/*
		if (isNaN(cur)) {
			cur = -1; //Default to first if none found
		}
		cur += 1;
		if (cur === $scope.encounter.initiatives.length) {
			cur = 0;
		}
		$scope.encounter.currentTurn = cur;
		*/
	};
	
	$scope.selectTarget = function(enemyIndex) {
		var enemies = $scope.encounter.enemies;
		var enemy;
		if (enemies.length === 0) {
			return false;
		}
		enemy = enemies[enemyIndex];
		return enemy;
	};
	
	$scope.attackSingleTarget = function(target, ability) {
		if (!target) {
			target = $scope.encounter.currentEntityTarget ? $scope.encounter.currentEntityTarget : false;
		}
		if (!ability) {
			ability = $scope.encounter.currentEntityAbility || false;
		}
		if (!ability || !target) {
			console.log('FAIL Attack single target - ability/target:'); console.log(ability); console.log(target);
			return false;
		}
		console.log('attacking ' + target.name + ' with ' + ability.ability.name);
		
	};
	
	$scope.attackMultipleTargets = function(targets, ability) {
		if (!targets || !ability) {
			console.log('FAIL Attack multi targets - ability/targets:'); console.log(ability); console.log(targets);
		}
		console.log('Multi-attacking: cnt/ability.name: ' + targets.length + '/' + ability.ability.name);
		
	};
	
	$scope.randomEnemyAction = function(enemy) {
		var actionIndex, action;
		var actionResult;
		if (!enemy) {
			enemy = $scope.encounter.currentEntity;
			if (!enemy || enemy.type !== 'enemy') {
				return false;
			}
		}
		action = enemy.combatOptions[0]; //FIXME auto-select Attack for Enemy turns
		$scope.encounter.currentEntityAction = action;
		$scope.encounter.currentEntityTarget = $scope.encounter.players[0]; //FIXME attack random player
		console.log('Random enemy action/target:'); console.log(action); console.log($scope.encounter.currentEntityTarget);
		
		switch(action.name) {
		case 'Defend':
		case 'Spell':
			break;
		case 'Attack':
		default:
			var minDamage, maxDamage, damage, finalDamage;
			minDamage = 1;
			maxDamage = $scope.getRandomInt(3,7);
			damage = $scope.getRandomInt(1, maxDamage);
			console.log('min/max/dmg:'+minDamage+'/'+maxDamage+'/'+damage);
			finalDamage = $scope.entityTakesDamage(damage, $scope.encounter.currentEntityTarget);
			
			actionResult = { 'damage': finalDamage };
			$scope.encounter.lastTurnResult.type = enemy.type;
			$scope.encounter.lastTurnResult.result = actionResult;
			$scope.encounter.currentTurnComplete = true;
			
			$scope.nextEncounterTurn();
		}
		return actionResult;
	};
	
	$scope.entityTakesDamage = function(damage, entity) {
		if (isNaN(damage)) {
			damage = 0;
		}
		//Calculate Def, Spell Resist, etc FIXME
		entity.stats.hp -= damage;
		console.log('Hit ' + entity.name + ' for ' + damage);
		if (entity. stats.hp <= 0) {
			console.log('Entity death:' + entity.name + ', ' + entity.stats.hp);
		}
		return damage;
	};
	
	$scope.playerTurnInit = function() {
		var player;
		console.log('PlayerTurnInit...');
		if ($scope.encounter.currentTurn !== 'player') {
			console.log('Cannot playerTurnInit for non-player:' +$scope.encounter.currentTurn);
			return false;
		}
		player = $scope.encounter.currentEntity;
		if (!player)
			return false;
		if (player.concentrationWord.length > 0) {
			console.log('Existing concentrationWord for player: ' + player.name); console.log(player.concentrationWord);
		} else {
			console.log('No concentrationWord for player: ' + player.name);
		}
		
	};
	
	/* END ENCOUNTER FUNCTIONS */
	
	
	/* BEGIN ALPHA/WORD FUNCTIONS */

	$scope.getRandomInt = function(min, max) {
		var i = Math.floor(Math.random() * (max - min + 1) + min);
		return i;
	};
	
	$scope.generateGridAlphas = function() {
		//Regnerates a new AlphaGrid
		var total = $scope.numRows * $scope.perRow;
		//var total = $scope.futureTileSize + gridTotal;
		var grid = [], futureTiles = [], partyTileBank = [], partyTileTrash = [];
		
		$scope.vowels = 0;
		$scope.consonants = 0;
		
		for (var i = 0; i < total; i++) {
			var data = $scope.getRandomAlpha();
			data.index = i;
			$scope[data.type] += 1;
			grid.push(data);
		}
		for (var i = 0; i < $scope.futureTileSize; i++) {
			var data = $scope.getRandomAlpha();
			data.index = i;
			//$scope[data.type] += 1;
			futureTiles.push(data);
		}
		/*for (var i = 0; i < $scope.partyTileBankSize; i++) {
			var data = $scope.getRandomAlpha();
			data.index = i;
			partyTileBank.push(data);
		}*/
		$scope.grid = grid;
		$scope.futureTiles = futureTiles;
		$scope.partyTileBank = partyTileBank;
		$scope.partyTileTrash = partyTileTrash;
	};
	
	$scope.getRandomAlpha = function() {
		var min = 65, max = 90;
		var code = $scope.getRandomInt(min, max);
		/*var data = { "alpha": String.fromCharCode(alpha), "code":alpha, selected:false,"index":null,"type":"consonants", "baseValue": $scope.letterValues[alpha - 65] };
		if (data.code === 81) {
			data.alpha += 'u';
		}
		if ($scope.vowelCodes.indexOf(data.code) !== -1) {
			data.type = 'vowels';
		}*/
		var data = $scope.getAlphaFromAsciiCode(code);
		return data;
	};

	$scope.getRandomVowel = function() {
		var min = 1, max = $scope.vowelCodes.length;
		var randomIndex = $scope.getRandomInt(min, max) - 1;
		var code = $scope.vowelCodes[randomIndex];
		console.log(randomIndex +'/'+code);
		return $scope.getAlphaFromAsciiCode(code);
	};
	
	$scope.getRandomConsonant = function() {
		var min = 1, max = $scope.consonantCodes.length;
		var randomIndex = $scope.getRandomInt(min, max) - 1;
		var code = $scope.consonantCodes[randomIndex];
		return $scope.getAlphaFromAsciiCode(code);
	};
	
	$scope.getAlphaFromAsciiCode = function(code) {
		var data;
		if (isNaN(code)) {
			console.log('Error: getAlphaFromAsciiCode - code: '+ code);
			return false;
		}
		data = { "alpha": String.fromCharCode(code), "code":code, selected:false,"index":null,"type":"consonants", "baseValue": $scope.letterValues[code - 65] };
		if (data.code === 81) {
			data.alpha += 'u'; //Append a 'u' with all 'Q' tiles
		}
		if ($scope.vowelCodes.indexOf(data.code) !== -1) {
			data.type = 'vowels';
		}
		return data;
	};
	
	$scope.selectAlpha = function(index) {
		var alpha = $scope.grid[index];
		if (alpha.selected === true) {
			return false;
		}
		$scope.spellWord.push(alpha);
		alpha.selected = true;
		
		//isValidWord = checkWord(finalWord);
		var isValidWord = true;
		$scope.isValidWord = isValidWord;
		
		console.log('sel '+alpha.alpha);
	};
	
	$scope.deselectAlpha = function(index) {
		var alpha = $scope.spellWord[index];
		console.log('desel '+ index);
		console.log($scope.spellWord);
		if (alpha.selected === false) {
			return false;
		}
		$scope.spellWord.splice(index, 1);
		alpha.selected = false;
		
		//isValidWord = checkWord(finalWord);
		var isValidWord = true;
		$scope.isValidWord = isValidWord;
		if ($scope.spellWord.length === 0) {
			$scope.isValidWord = false;
		}
		
		console.log('desel '+alpha.alpha);
	};
	
	$scope.removeAlphas = function(alphas) {
		if (alphas.length === 0) {
			return false;
		}
		angular.forEach(alphas, function(alpha, _i) {
			var data = $scope.grid[alpha.index];
			var nextData = $scope.futureTiles.shift();
			var newData = $scope.futureTiles.push($scope.getRandomAlpha());
			var typeChange;
			if (!nextData) {
				nextData = $scope.futureTiles.shift();
				newData = $scope.futureTiles.push($scope.getRandomAlpha());
			}
			nextData.index = data.index;
			$scope.grid.splice(data.index, 1, nextData);
			if (data.type !== nextData.type) {
				$scope[data.type] -= 1;
				$scope[nextData.type] += 1;	
			}
		});
	};
	
	$scope.submitWord = function() {
		var alphas = [];
		var word = [], finalWord, isValidWord = false;
		var wordValue = 0;
		if ($scope.spellWord.length < 2) {
			console.log('insufficient spellWord length:');
			console.log($scope.spellWord);
			return false;
		}
		angular.forEach($scope.spellWord, function(data, _i) {
			word.push(data.alpha);
			alphas.push(data);
			wordValue += data.baseValue;
		});
		finalWord = word.join('');
		console.log('submitted finalWord: ' + finalWord +'/'+wordValue);
		
		//isValidWord = checkWord(finalWord);
		isValidWord = true;
		
		if (isValidWord) {
			$scope.removeAlphas(alphas);
			$scope.spellWord = $scope.spellWord.splice();
			$scope.isValidWord = false;
			
			//$scope.nextEncounterTurn();
			$scope.currentEntityAction = 'bank';
		}
		
		
		
	};
	
	$scope.refillTiles = function(container) {
		var cnt = container.length;
		var hasEmpty = true;
		var loops = 0;
		if (cnt === 0) {
			console.log('Refill fail - container.length = 0');
			return false;
		}
		/*do {
			
			
			loops++;
			if (loops >= cnt) {
				hasEmpty = false;
			}
		} while (hasEmpty === true);*/
		angular.forEach(container, function(tile, _i) {
			if (!tile) {
				var data = $scope.getRandomAlpha();
				console.log('refill with new tile:'); console.log(tile);
				tile = data;
			} else {
				console.log(tile);
			}
		});
		
	};
	
	/* END ALPHA/WORD FUNCTIONS */
	
	/* BEGIN UI FUNCTIONS */
	$scope.showEntityInfo = function(entity, type) { //Displays an Entity in the middle of the main UI
		$scope.activeUiEntityType = type;
		$scope.activeUiEntity = entity;
	};
	
	$scope.hideEntityInfo = function() {
		$scope.activeUiEntityType = false;
		$scope.activeUiEntity = false;
	};
	
	$scope.selectUiTab = function(tab, player) {
		player.showTab = tab;
	};
	
	$scope.moveSiblingDown = function(event) {
		//var i = $(event.target).parent
		console.log('MoveSibDown event:');
		console.log(event);
		$(event.target).parent().insertBefore($(event.target).parent().prev());
	};
	
	$scope.moveSiblingUp = function(event) {
		console.log('MoveSibUp event:');
		console.log(event);
		$(event.target).parent().insertAfter($(event.target).parent().next());
	};
	
	/* END UI FUNCTIONS */
	
	/* BEGIN GAME FUNCTIONS */ 
	$scope.initGame = function() {
		//Initialize the Game, states, players, etc
		//$scope.appStorage.loadData();
		$scope.appStorage.loadData($scope.initGameData);
		
		var specialTiles = { "dungeonTiles": { "min": 1, "max": 3 }, "townTiles": { "min": 1, "max": 2 } }; //Number of specialTiles
		var bank = [];
		
		$scope.generateGridAlphas(); //Generate initial alphagrid
		$scope.initPlayers(); //Initialize player gear, stats, abilities, etc
		
		for (var i = 0; i < $scope.partyTileBankSize; i++) {
			bank.push($scope.getRandomAlpha());
		}
		$scope.partyTileBank = bank;
		$scope.partyTileTrash = [];
		$scope.refillTiles($scope.partyTileBank);
		
		//$scope.loadGameMap(mapId, mapName);
		$scope.generateRandomGameMap($scope.map.perRow, $scope.map.numRows, $scope.tileset, specialTiles);
		console.log('initGame POST generateRandomGameMap');
		$scope.gameState = 'map';
		//$scope.generateEncounter(1, 5, 0, 1); //Generate 1-5 enemies
		
		
	};
	
	$scope.initGameData = function(data) {
		//Restores the previous game, or creates a new one if no data is found
		console.log('initGameData:: data = ', data);
		if (typeof data == "object") {
			if (data.hasOwnProperty('players')) {
				//Load players
				//$scope.players = data.players;
			} else {
				console.log('initGameData::No players found?!');
			}
			if (data.hasOwnProperty('gameConfig')) {
				var cfg = data.gameConfig;
				$scope.gameState = cfg.gameState || 'map';
				if (cfg.hasOwnProperty('encounter')) {
					$scope.encounter = cfg.encounter;
				} else {
					console.log('initGameData::No cfg.encounter found?!');
				}
				
				$scope.partyMapX = cfg.partyMapX || 1;
				$scope.partyMapY = cfg.partyMapY || 1;
				
				if (cfg.hasOwnProperty('alphaGrid')) {
					$scope.alphaGrid = cfg.alphaGrid;
				} else {
					console.log('initGameData::No cfg.alphaGrid found?! Generating new...');
					$scope.generateGridAlphas(); //Generate initial alphagrid
				}
				
				if (cfg.hasOwnProperty('partyTileBank')) {
					$scope.partyTileBank = cfg.partyTileBank;
				} else {
					console.log('initGameData::No cfg.partyTileBank found?!');
					//$scope.partyTileBank = [];
					$scope.partyTileBank = Array.apply(null, Array($scope.partyTileBankSize)).map(function () {});
				}
				
				if (cfg.hasOwnProperty('futureTiles')) {
					$scope.futureTiles = cfg.futureTiles;
				} else {
					console.log('initGameData::No cfg.futureTiles found?!');
					$scope.futureTiles = Array.apply(null, Array($scope.futureTileSize)).map(function() {});
				}
			}
		} else {
			console.log('Error - cannot load data: ', data);
		}
	};
	
	$scope.saveGame = function() {
		//Saves current game to $scope.appStorage
		var saveData = { players: $scope.players, gameConfig: { gameState: $scope.gameState, partyMapX: $scope.partyMapX, partyMapY: $scope.partyMapY, partyTileBank: $scope.partyTileBank, futureTiles: $scope.futureTiles, gameOptions: $scope.gameOptions } };
		$scope.appStorage.data = saveData;
		$scope.appStorage.saveData();
	};
	
	$scope.generateRandomGameMap = function(perRow, numRows, tileset, specialTiles) {
		var tiles = [];
		var tileRows = [];
		var thisRow = [];
		var map = { "tiles": [], "tileRows": [], "partyMapX": 1, "partyMapY": 1 };
		var total = numRows * perRow;
		var moveCnt = $scope.tileset.moveTiles.length;
		var tileIndex;
		var tile, lastTile, lastTileCnt;
		var specialTileLocations = {};
		console.log('generateRandomGameMap FIRE, total:'+ total);
		console.log(tileset);
		for (var i = 0; i < total; i++) {
			if ( i % perRow === 0) {
				if (thisRow.length > 0) {
					tileRows.push(thisRow); //Add previous loop's rows
				}
				thisRow = [];
				
				tileIndex = $scope.getRandomInt(1, moveCnt) - 1;
				console.log('firstTileInRow i/tileIndex='+i+'/'+tileIndex);
				tile = $scope.tileset.moveTiles.slice(tileIndex);
				console.log(tile);
				tile['index'] = i;
				tiles.push(tile[0]);
				thisRow.push(tile[0]);
			} else {
				
				//tileIndex = $scope.getRandomInt(1, moveCnt) - 1;
				//tile = $scope.tileset.moveTiles.slice(tileIndex, 1;
				tile = lastTile; //LAZY -- do same tile as last tile
				tile['index'] = i;
				tiles.push(tile[0]);
				thisRow.push(tile[0]);
			}
			lastTile = tile;
		}
		tileRows.push(thisRow); //Add final row
		
		angular.forEach(specialTiles, function(params, tilesetname, specialTileLocations) {
			//var type = params.type || false;
			var min = params.min;
			var max = params.max;
			var cnt = $scope.getRandomInt(min, max);
			var tile;
			console.log('specialTiles generate min/max/cnt:'+min+'/'+max+'/'+cnt);
			if (cnt > 0) {
				var typeCnt = $scope.tileset[tilesetname].length;
				for (var i = 0; i < cnt; i++) {
					var tileIndex = (typeCnt > 1 ? $scope.getRandomInt(1, typeCnt) : 0);
					var yPos = $scope.getRandomInt(1, $scope.map.perRow) - 1;
					var xPos = $scope.getRandomInt(1, $scope.map.numRows) - 1;
					/*
					if (!specialTileLocations.hasOwnProperty(xPos)) {
						specialTileLocations[xPos] = { yPos: true };
					} else {
						if (specialTileLocations[xPos].hasOwnProperty(yPos)) {
							console.log('DUPLICATE SPECIAL TILE LOC xPos/yPos:'+xPos+'/'+yPos);
							console.log(specialTileLocations);
						}
					}
					*/
					console.log('SPECIAL TILE LOC xPos/yPos/tileI:'+xPos+'/'+yPos+'/'+tileIndex);
					tile = $scope.tileset[tilesetname].slice(tileIndex);
					console.log(tile);
					tileRows[xPos][yPos] = tile[0];
					
				}
			}
		});
		tile = $scope.tileset.blockTiles[0];
		tileRows[2][1] = tile;
		console.log(tile);
		map.tiles = tiles;
		map.tileRows = tileRows;
		$scope.map.currentMap = map;
	};
	
	$scope.loadGameMap = function(mapId, mapName) {
		//Load a pre-made map
		console.log('(EMPTYFUNCTION)loadGameMap mapId/mapName:'+mapId+'/'+mapName);
		//TODO
		
	};
	
	$scope.moveParty = function(dir) {
		var curPosX = $scope.map.currentMap.partyMapX || false;
		var curPosY = $scope.map.currentMap.partyMapY || false;
		var newTile;
		var canMove = false;
		if (!dir) {
			console.log('moveParty:ERROR: dir:'+dir);
			return false;
		}
		
		switch(dir) {
		case 'r':
			curPosY +=1;
			break;
		case 'l':
			curPosY -= 1;
			break;
		case 'd':
			curPosX += 1;
			break;
		case 'u':
		default:
			curPosX -= 1;
		}
		
		if (curPosX < 0 || curPosY < 0) {
			console.log('moveParty:ERROR: invalid curPosX/Y:' + curPosX +'/'+ curPosY);
			return false;
		}
		if (curPosX >= 0 && curPosY >= 0) {
			newTile = $scope.map.currentMap.tileRows[curPosX][curPosY];
			console.log('moveParty:x/y:'+ curPosX +'/'+ curPosY);
			console.log(newTile);
			
			canMove = $scope.checkTileBeforeMove(newTile);
			
			if (canMove) {
				$scope.map.currentMap.partyMapX = curPosX;
				$scope.map.currentMap.partyMapY = curPosY;
				
				//Roll for Encounter
				if (newTile.hasOwnProperty("chanceForEncounter") && newTile.chanceForEncounter > 0) {
					var max = 100, chance = newTile.chanceForEncounter;
					var roll = $scope.getRandomInt(1, 100);
					console.log('Encounter roll chance/roll:'+chance+'/'+roll);
					if (roll <= chance) {
						$scope.gameState = 'encounter';
						$scope.generateEncounter(1, 5, 0, 1);
					}
				}
				
			} else {
				console.log('moveParty:CannotMove: canMove/curPosX/curPosY:'+canMove+'/'+ curPosX +'/'+ curPosY);
			}
		}
	};
	
	$scope.checkTileBeforeMove = function(tile) {
		var canMove = false;
		console.log(tile);
		if (!tile) {
			console.log('ERROR: tile:'+tile);
			return false;
		}
		if (tile.hasOwnProperty("canMove") && tile.canMove === false) {
			console.log('Cannot move to tile:');
			console.log(tile);
			return false;
		}
		if (tile.hasOwnProperty("moveRequirements")) {
			console.log(tile.moveRequirements);
			//TODO
		}
		canMove = true;
		return canMove;
	};
	
	/* END GAME FUNCTIONS */
	
	$scope.initGame();

//}]);
});