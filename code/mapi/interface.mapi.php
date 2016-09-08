<?php
interface MapiInterface {
	/* An interface for MutableAPI classes */
	public function makeApiCall(array $args);
	public function saveCallResults($result, array $args);
	
}