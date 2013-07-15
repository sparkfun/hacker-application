Ideal Web Application Developer candidates will be comfortable building web
applications with PHP and MySQL on Linux. Ideal Test Developer candidates will
be comfortable building automated tests for such applications.

All will have real-world experience with other languages and platforms.

We want to see your code. Please include a representative sample of your
work in this directory.

All languages and paradigms are welcome, but we're most interested in
submissions demonstrating one or more of the following:

  - Clean, secure, object oriented PHP.
  - Usable, modern, and reasonably standard HTML, CSS, and JavaScript.
  - Clear, sensible, and consistently formatted code.
  - Sympathy for the plight of future maintainers.

If you maintain or contribute to any open source projects, please supply links
and a short description of your role in each project.

**Things to avoid:**

  - Excessive amounts of code. Your samples will be *read*, not *executed*. More
    than 1000 lines of code and you're doing it wrong.
  - Irrelevant dependencies. The large XML library that your class relies upon
    is neither interesting nor helpful.
  - Binaries. Just don't.
  - Third party libraries. If you didn't write it, don't include it. If you only
    edited the configuration file for it, don't include it.

Select samples that are brief and show off different characteristics of your skill set.
A brief, well-written class, a neat little algorithm, stuff like that. Less is more.


Here are three samples of code I wrote in PHP, T-SQL, and Javascript.  I've put comments above them to give the reader an idea of their puporse.

//--------------------------------------------------------
// 						PHP
//--------------------------------------------------------
// This is a class I made for my site at Skyrimcalc.com
// I wanted to use paremeterized queries with bound arguments, but I also needed to use an IN statement in the WHERE clause of the query
// This meant I had to dynamically generate the IN(?,?,?,?,?,?,?,?,?) part of the query, as well as specify the data types (string or int) for each argument, which is what this data access object accomplished.

<?php
Class Potion_DAO{
	protected $db;
	public function __construct($db){
		$this->db = $db;
	}
	/**
	 * Search for potions that may only contain the specified ingredients.
	 * @param array $ingredientList
	 * @param array $effectList
	 * @param int $potionType
	 * @param array $pagingData
	 * @return array
	 */
	public function findPotionsByingredients($ingedients,$effects,$potionType,$pagingData = null){
		$orderBySQL = '';
		$pagingSQL = '';
		if($pagingData){
			$orderBySQL = 'ORDER BY p.value DESC,CASE WHEN p.ingredient3_id IS NULL THEN 1 ELSE 0 END DESC,p.ingredient1_id ASC,p.ingredient2_id ASC,p.ingredient3_id ASC,p.effect_count DESC';
			$limit = $pagingData['pageSize'] <= 100 ? $pagingData['pageSize'] : 100;
			$offset = $pagingData['pageIndex'] * $pagingData['pageSize'];
			$pagingSQL = "LIMIT {$offset},{$limit}";
		}
		
		$ingredientSQL = trim(str_repeat('?,',count($ingedients)),',');
		$effectsSQL = '';
		if(count($effects)){
			$effectsSQL = trim(str_repeat('?,',count($effects)),',');
			$effectsSQL = "AND (p.effect1_id IN({$effectsSQL}) OR p.effect2_id IN({$effectsSQL}) OR p.effect3_id IN({$effectsSQL}) OR p.effect4_id IN({$effectsSQL}) OR p.effect5_id IN({$effectsSQL}))";
		}
		
		$potionTypeSQL = '';
		if($potionType == 1){
			$potionTypeSQL = 'AND p.type IN(2,3)';
		}elseif($potionType == 2){
			$potionTypeSQL = 'AND p.type = 2';
		}elseif($potionType == 3){
			$potionTypeSQL = 'AND p.type = 3';
		}elseif($potionType == 4){
			$potionTypeSQL = 'AND p.type = 4';
		}
		
		$selectSQL = 'COUNT(p.potion_id) AS total';
		if($pagingData){
			$selectSQL = "
				i.name AS ingredient1
				,i2.name AS ingredient2
				,i3.name AS ingredient3
				,TRIM(TRAILING ', ' FROM CONCAT(e.name,', ',IFNULL(e2.name,''),', ',IFNULL(e3.name,''),', ',IFNULL(e4.name,''),', ',IFNULL(e5.name,''))) AS effects
				,ROUND(p.value,1) AS `value`
				,pt.name AS potionType
			";
		}
		$sql = "SELECT
					{$selectSQL}
				FROM `skyrimca_skyrim`.potion p
				INNER JOIN `skyrimca_skyrim`.ingredient i ON i.ingredient_id = p.ingredient1_id
				INNER JOIN `skyrimca_skyrim`.ingredient i2 ON i2.ingredient_id = p.ingredient2_id
				LEFT JOIN `skyrimca_skyrim`.ingredient i3 ON i3.ingredient_id = p.ingredient3_id
				INNER JOIN `skyrimca_skyrim`.effect e ON e.effect_id = p.effect1_id
				LEFT JOIN `skyrimca_skyrim`.effect e2 ON e2.effect_id = p.effect2_id
				LEFT JOIN `skyrimca_skyrim`.effect e3 ON e3.effect_id = p.effect3_id
				LEFT JOIN `skyrimca_skyrim`.effect e4 ON e4.effect_id = p.effect4_id
				LEFT JOIN `skyrimca_skyrim`.effect e5 ON e5.effect_id = p.effect5_id
				INNER JOIN `skyrimca_skyrim`.potionType pt ON pt.potion_type_id = p.type
				WHERE (p.ingredient1_id IN({$ingredientSQL}) AND p.ingredient2_id IN({$ingredientSQL}) AND (p.ingredient3_id IS NULL OR p.ingredient3_id IN({$ingredientSQL})))
				AND p.redundent = 0
				{$effectsSQL}
				{$potionTypeSQL}
				{$orderBySQL}
				{$pagingSQL}
				";
				//echo '<pre>',implode(',',$ingedients),"\n",$sql,'</pre>';
		$argTypes = str_repeat('i',count($ingedients)*3);
		$args = array_merge($ingedients,$ingedients,$ingedients);
		if(count($effects)){
			$argTypes .= str_repeat('i',count($effects)*5);
			$args = array_merge($args,$effects,$effects,$effects,$effects,$effects);
		}
		$results = $this->db->query($sql,$argTypes,$args);
		
		$potions = array();
		if($pagingData){
			while($result = $results->next()){
				$potions[] = $result;
			}
		}else{
			if($result = $results->next()){
				$potions = $result['total'];
			}
		}
		return $potions;
	}
}
?>


//--------------------------------------------------------
// 						Javascript
//--------------------------------------------------------
// This is another class I made for my hobby site at Skyrimcalc.com
// I wanted the functionality of what is called an "option transfer".  Basically it is two multiselect boxes that allow you to pass data from one to the other, and visa versa.
// This was meant to be a generic implementation where you instantiate two instances of MultiSelectList, and call the setPartner() method on each to tell the object who it sends data to.

var genericConstructor = function(){return function(){this.initialize.apply(this,arguments);};};
MultiSelectList = genericConstructor();
MultiSelectList.prototype ={
	initialize: function(el,transferOneButton,transferAllButton){
		this.el = jQuery(el);
		this.transferOne = jQuery(transferOneButton);
		this.transferAllButton = jQuery(transferAllButton);
		this.partner = null;
		this.hook();
	},
	hook:function(){
		var _self = this;
		this.el.dblclick(function(){
			_self.transfer();
		});
		this.transferOne.click(function(){
			_self.transfer();
		});
		this.transferAllButton.click(function(){
			_self.selectAll();
			_self.transfer();
		});
	},
	selectedOptionCount: function(){
		return jQuery(this.el).find('option').length;
	},
	setPartner: function(partnerObj){
		this.partner = partnerObj;
	},
	transfer: function(){
		var _self = this;
		jQuery(this.el.children('option:selected')).remove().appendTo(_self.partner.el);
		this.partner.sortByValue();
	},
	sortByValue: function(){
		this.sort('value');
	},
	sortByText: function(){
		this.sort('text');
	},
	sort: function(sortBy){
		var options = this.el.find('option');
		var selectedOptions = [];
		for(var i=0;i<options.length;i++){
			var o = jQuery(options[i]);
			if(o.prop('selected')){
				selectedOptions[o.prop('value')] = o.prop('value');
			}
		}
		if(sortBy == 'value'){
			options.sort(function(a,b){return jQuery(a).prop('value') - jQuery(b).prop('value')});
		}else if(sortBy == 'text'){
			options.sort(function(a,b){return jQuery(a).text() - jQuery(b).text()});
		}
		this.el.text('');
		for(var i=0;i<options.length;i++){
			var o = jQuery(options[i]);
			var option = new Option(o.text(),o.prop('value'));
			jQuery(option).html(o.text());
			this.el.append(option);
		}
		var options = this.el.find('option');
		for(var i=0;i<options.length;i++){
			var o = jQuery(options[i]);
			if(typeof selectedOptions[o.prop('value')] != 'undefined'){
				o.prop('selected',true);
			}
		}
	},
	selectAll: function(){
		this.changeAll(true);
	},
	removeAll: function(){
		this.changeAll(false);
	},
	changeAll: function(value){
		var changeTo = (value) ? true : false;
		var options = this.el.find('option');
		for(var i=0;i<options.length;i++){
			jQuery(options[i]).prop('selected',changeTo);
		}
	}
}


//--------------------------------------------------------
// 							SQL
//--------------------------------------------------------
// Here is a query I wrote to get a list of characters from the MMO Shaiya and sum the characters' stats from all possible locations (base stats, gear, etc).
// It mainly gets used to detect cheating players as if they have a lot more stats than they should have, this makes it obvious.

SELECT TOP 200
	c.CharName,
	c.Str + characterItems.ConstStr + lapisSum.ConstStr AS [Str],
	c.Dex + characterItems.ConstDex + lapisSum.ConstDex AS [Dex],
	c.Rec + characterItems.ConstRec + lapisSum.ConstRec AS [Rec],
	c.Int + characterItems.ConstInt + lapisSum.ConstInt AS [Int],
	c.Wis + characterItems.ConstWis + lapisSum.ConstWis AS [Wis],
	c.Luc + characterItems.ConstLuc + lapisSum.ConstLuc AS [Luc],
	c.Str + characterItems.ConstStr + lapisSum.ConstStr + c.Dex + characterItems.ConstDex + lapisSum.ConstDex + c.Rec + characterItems.ConstRec + lapisSum.ConstRec + c.Int + characterItems.ConstInt + lapisSum.ConstInt + c.Wis + characterItems.ConstWis + lapisSum.ConstWis + c.Luc + characterItems.ConstLuc + lapisSum.ConstLuc AS TotalStat,
	c.Rec + characterItems.ConstRec + lapisSum.ConstRec + characterItems.Defense + lapisSum.Defense AS Defense,
	c.Wis + characterItems.ConstWis + lapisSum.ConstWis + characterItems.MagicResist + lapisSum.MagicResist AS MagicResist,
	characterItems.Absorb + lapisSum.Absorb AS Absorb,
	CASE
		WHEN c.Job < 3 THEN
			(c.Str + characterItems.ConstStr + lapisSum.ConstStr) * 1.3 + (c.Dex + characterItems.ConstDex + lapisSum.ConstDex) * .2 + characterItems.Attack + lapisSum.Attack
		WHEN c.Job = 3 THEN
			(c.Str + characterItems.ConstStr + lapisSum.ConstStr) + (c.Dex + characterItems.ConstDex + lapisSum.ConstDex) * .2 + (c.Luc + characterItems.ConstLuc + lapisSum.ConstLuc) * .3 + characterItems.Attack + lapisSum.Attack
		ELSE
			(c.Int + characterItems.ConstInt + lapisSum.ConstInt) + (c.Wis + characterItems.ConstWis + lapisSum.ConstWis) * .2 + characterItems.Attack + lapisSum.Attack
		END AS AttackMin,
	CASE
		WHEN c.Job < 3 THEN
			(c.Str + characterItems.ConstStr + lapisSum.ConstStr) * 1.3 + (c.Dex + characterItems.ConstDex + lapisSum.ConstDex) * .2 + characterItems.Attack + lapisSum.Attack + characterItems.AttackModifier + lapisSum.AttackModifier
		WHEN c.Job = 3 THEN
			(c.Str + characterItems.ConstStr + lapisSum.ConstStr) + (c.Dex + characterItems.ConstDex + lapisSum.ConstDex) * .2 + (c.Luc + characterItems.ConstLuc + lapisSum.ConstLuc) * .3 + characterItems.Attack + lapisSum.Attack + characterItems.AttackModifier + lapisSum.AttackModifier
		ELSE
			(c.Int + characterItems.ConstInt + lapisSum.ConstInt) + (c.Wis + characterItems.ConstWis + lapisSum.ConstWis) * .2 + characterItems.Attack + lapisSum.Attack + characterItems.AttackModifier + lapisSum.AttackModifier
		END AS AttackMax
FROM [Ps_GameData].[dbo].[Chars] c
INNER JOIN PS_UserData.dbo.Users_Master AS u ON c.UserUID = u.UserUID
INNER JOIN (
	SELECT
		DISTINCT ci.CharID AS CharID,
		SUM(i.ConstStr + CASE WHEN i.ReqWis > 0 AND LEN(ci.CraftName) = 20 THEN CONVERT(int,SUBSTRING(ci.CraftName,1,2)) ELSE 0 END) AS ConstStr,
		SUM(i.ConstDex + CASE WHEN i.ReqWis > 0 AND LEN(ci.CraftName) = 20 THEN CONVERT(int,SUBSTRING(ci.CraftName,3,2)) ELSE 0 END) AS ConstDex,
		SUM(i.ConstRec + CASE WHEN i.ReqWis > 0 AND LEN(ci.CraftName) = 20 THEN CONVERT(int,SUBSTRING(ci.CraftName,5,2)) ELSE 0 END) AS ConstRec,
		SUM(i.ConstInt + CASE WHEN i.ReqWis > 0 AND LEN(ci.CraftName) = 20 THEN CONVERT(int,SUBSTRING(ci.CraftName,7,2)) ELSE 0 END) AS ConstInt,
		SUM(i.ConstWis + CASE WHEN i.ReqWis > 0 AND LEN(ci.CraftName) = 20 THEN CONVERT(int,SUBSTRING(ci.CraftName,9,2)) ELSE 0 END) AS ConstWis,
		SUM(i.ConstLuc + CASE WHEN i.ReqWis > 0 AND LEN(ci.CraftName) = 20 THEN CONVERT(int,SUBSTRING(ci.CraftName,11,2)) ELSE 0 END) AS ConstLuc,
		SUM(i.Effect1 + CASE WHEN i.ReqWis > 0 AND LEN(ci.CraftName) = 20 AND (CONVERT(int,SUBSTRING(ci.CraftName,19,2)) BETWEEN 1 AND 20) THEN 
			CASE
				WHEN SUBSTRING(ci.CraftName,19,2) = '01' THEN 7 WHEN SUBSTRING(ci.CraftName,19,2) = '02' THEN 14 WHEN SUBSTRING(ci.CraftName,19,2) = '03' THEN 21
				WHEN SUBSTRING(ci.CraftName,19,2) = '04' THEN 31 WHEN SUBSTRING(ci.CraftName,19,2) = '05' THEN 41 WHEN SUBSTRING(ci.CraftName,19,2) = '06' THEN 51
				WHEN SUBSTRING(ci.CraftName,19,2) = '07' THEN 64 WHEN SUBSTRING(ci.CraftName,19,2) = '08' THEN 77 WHEN SUBSTRING(ci.CraftName,19,2) = '09' THEN 90
				WHEN SUBSTRING(ci.CraftName,19,2) = '10' THEN 106 WHEN SUBSTRING(ci.CraftName,19,2) = '11' THEN 122 WHEN SUBSTRING(ci.CraftName,19,2) = '12' THEN 138
				WHEN SUBSTRING(ci.CraftName,19,2) = '13' THEN 157 WHEN SUBSTRING(ci.CraftName,19,2) = '14' THEN 176 WHEN SUBSTRING(ci.CraftName,19,2) = '15' THEN 195
				WHEN SUBSTRING(ci.CraftName,19,2) = '16' THEN 217 WHEN SUBSTRING(ci.CraftName,19,2) = '17' THEN 239 WHEN SUBSTRING(ci.CraftName,19,2) = '18' THEN 261
				WHEN SUBSTRING(ci.CraftName,19,2) = '19' THEN 286 WHEN SUBSTRING(ci.CraftName,19,2) = '20' THEN 311 
			END
		ELSE 0 END) AS Attack,
		SUM(i.Effect2) AS AttackModifier,
		SUM(i.Effect3) AS Defense,
		SUM(i.Effect4) AS MagicResist,
		SUM(CASE WHEN i.ReqWis > 0 AND (CONVERT(int,SUBSTRING(ci.CraftName,19,2)) BETWEEN 51 AND 70) THEN ((CONVERT(int,SUBSTRING(ci.CraftName,19,2)) - 50) * 5) ELSE 0 END) AS Absorb
	FROM [Ps_GameData].[dbo].[CharItems] ci
		INNER JOIN [Ps_GameDefs].[dbo].Items i ON i.ItemID = ci.ItemID
	WHERE ci.Bag = 0
		AND ci.Slot >= 0
		AND ci.Slot != 13
	GROUP BY ci.CharID
) AS characterItems ON c.CharID = characterItems.CharID
LEFT JOIN (
	SELECT
		DISTINCT ci.CharID AS CharID,
		SUM(ISNULL(lapis.ConstStr,0)) AS ConstStr,
		SUM(ISNULL(lapis.ConstDex,0)) AS ConstDex,
		SUM(ISNULL(lapis.ConstRec,0)) AS ConstRec,
		SUM(ISNULL(lapis.ConstInt,0)) AS ConstInt,
		SUM(ISNULL(lapis.ConstWis,0)) AS ConstWis,
		SUM(ISNULL(lapis.ConstLuc,0)) AS ConstLuc,
		SUM(ISNULL(lapis.Effect1,0)) AS Attack,
		SUM(ISNULL(lapis.Effect2,0)) AS AttackModifier,
		SUM(ISNULL(lapis.Effect3,0)) AS Defense,
		SUM(ISNULL(lapis.Effect4,0)) AS MagicResist,
		SUM(ISNULL(lapis.Exp,0)) AS Absorb
	FROM [Ps_GameData].[dbo].[CharItems] ci
		INNER JOIN [Ps_GameDefs].[dbo].Items i ON ci.ItemID = i.ItemID
		LEFT JOIN [Ps_GameDefs].[dbo].Items lapis ON lapis.Type = 30 AND lapis.TypeID IN(ci.Gem1,ci.Gem2,ci.Gem3,ci.Gem4,ci.Gem5,ci.Gem6)
	WHERE ci.Bag = 0
		AND ci.Slot >= 0
		AND ci.Slot != 13
	GROUP BY ci.CharID
) AS lapisSum ON c.CharID = lapisSum.CharID
WHERE
	c.Del = 0
	AND u.AdminLevel = 0
ORDER BY TotalStat DESC;