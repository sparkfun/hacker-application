# ISA Filter System

The ISA Filter System is a method of filtering out Industrial Service Agreement's. The database in which these documents are stored numbers in the 10,000+ range, so some method of filtering is important.

I've included two of the filters themselves, along with the factory that constructs the Zend SQL object used to find the ISA documents.

The factory utilizes the decorator pattern, to construct many small objects which act upon a base Zend DB Select object, if you look at the top of the Wm_Isa_Filter_Factory file, you'll find a "library" of the filters, a key => value array of class names of the filters. The class' constructor takes an array of field names (filtered, and validated, of course) that the user can modify, and finds their corresponding filter class, constructs the class, sets the value, and modifies the select statement.

The array passed into the constructor is expected to be in the following format:

```
array(
	'filter_name' => 'filter_value'
);
```

So, a filter construction array may look like:

```
array(
	'legal_entity' => 'Jason Campbell\'s legal entity'
);
```

This will construct a legal entity filter, setting the value to "Jason Campbell's Legal Entity" and will ultimately modify the base select object to filter out rows from the DB that do not have a name like "Jason Campbell's Legal Entity"