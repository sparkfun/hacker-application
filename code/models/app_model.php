<?php
class AppModel extends Model{

	function escapeValues($arr)
	{
		foreach($arr as &$v)
		{
			$v = $this->escape($v);
		}
		return $arr;
	}

}
