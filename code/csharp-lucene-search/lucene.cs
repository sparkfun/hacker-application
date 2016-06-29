// _indexQueue values are set via the UI
private readonly Dictionary<string, List<Dictionary<string, string>>> _indexQueue = new Dictionary<string, List<Dictionary<string, string>>>();

// index object for tracking and storing layer/featureset index information
internal class IndexObject
{
	public IFeatureSet FeatureSet { get; private set; }
    public string LayerName { get; private set; }
    public ProjectionInfo LayerProjection { get; set; }
    public string IndexType { get; private set; }
    public List<KeyValuePair<string, string>> FieldLookup { get; private set; }
    public IndexObject(IFeatureSet featureSet, List<KeyValuePair<string, string>> fieldLookup, string layerName, ProjectionInfo projectionInfo, string indexType)
    {
		FeatureSet = featureSet;
        FieldLookup = fieldLookup;
        LayerName = layerName;
        LayerProjection = projectionInfo;
        IndexType = indexType;
    }
}

// button click on UI to prepare index objects for indexing by lucene
private void btnCreateIndex_Click(object sender, EventArgs e)
{
	if (_idxWorker.IsBusy != true)
    {
		var iobjects = new IndexObject[_indexQueue.Count];
        int count = 0;

		foreach (KeyValuePair<string, List<Dictionary<string, string>>> qKeyVal in _indexQueue)
        {
			string lyrName = qKeyVal.Key;
            // make sure a featureset exists and assign FID values if not present already
            IMapLayer mapLyr;
            _layerLookup.TryGetValue(lyrName, out mapLyr);
            var mfl = mapLyr as IMapFeatureLayer;
            IFeatureSet fs;
            if (mfl != null && mfl.DataSet != null)
            {
				fs = mfl.DataSet;
                fs.AddFid();  // make sure FID values exist for use as lookup key
                fs.Save();
            }
			else
            {
				var msg = AppContext.Instance.Get<IUserMessage>();
				msg.Warn("Error on Create Index, FeatureDataset is null", new Exception());
                return;
            }

            List<Dictionary<string, string>> indexList = qKeyVal.Value;
            var list = new List<KeyValuePair<string, string>>();

            // iterate through all the field indexes
            for (int i = 0; i <= indexList.Count - 1; i++)
            {
				Dictionary<string, string> d = indexList[i];
                var kvPair = new KeyValuePair<string, string>(d["lookup"], d["fieldname"]);
                list.Add(kvPair);
            }
            string idxType = GetLayerIndexTableType(lyrName);
            var io = new IndexObject(fs, list, lyrName, mapLyr.Projection, idxType);
            // add the indexobject to our array for creation
            iobjects.SetValue(io, count);
            count++;
        }
		_idxWorker.RunWorkerAsync(iobjects);
    }
}

// generate indexes for each indexobject
private void idx_DoWork(object sender, DoWorkEventArgs e)
{
	try
    {
		var iobjects = e.Argument as IndexObject[];
        Dictionary<string, List<Document>> docs = GetDocuments(iobjects);
        if (docs.Count > 0)
        {
			var path = string.Empty;
            Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_30);
            foreach (KeyValuePair<string, List<Document>> keyValuePair in docs)
			{
				path = Path.Combine(TempIndexDir, "_indexes", keyValuePair.Key);

                DirectoryInfo di = System.IO.Directory.CreateDirectory(path);
                LDirectory dir = FSDirectory.Open(di);
                FileInfo[] fi = di.GetFiles();

                // single indexwriter is thread safe so lets use it in parallel
                IndexWriter writer = fi.Length == 0 ?
					new IndexWriter(dir, analyzer, true, IndexWriter.MaxFieldLength.LIMITED) :
                    new IndexWriter(dir, analyzer, false, IndexWriter.MaxFieldLength.LIMITED);

                // iterate all our documents and add them
                var documents = keyValuePair.Value;
                Parallel.ForEach(documents, delegate(Document document, ParallelLoopState state)
                {
					// check if this document already exists in the index
                    var fid = document.GetField(Fid).StringValue;
                    var lyr = document.GetField(Lyrname).StringValue;

                    Query qfid = new TermQuery(new Term(Fid, fid));
                    Query qlyr = new TermQuery(new Term(Lyrname, lyr));

                    var query = new BooleanQuery { { qfid, Occur.MUST }, { qlyr, Occur.MUST } };

                    writer.DeleteDocuments(query);
                    writer.AddDocument(document);
                });
                writer.Optimize();
                writer.Dispose();
            }
        }
    }
    catch (Exception ex)
    {
		var msg = AppContext.Instance.Get<IUserMessage>();
        msg.Error("Error on creating index :: BackgroundWorker Failed", ex);
    }
}

private Dictionary<string, List<Document>> GetDocuments(IndexObject[] iobjects)
{
	var docs = new Dictionary<string, List<Document>>();
    if (iobjects.Length <= 0) return docs;

    foreach (IndexObject o in iobjects)
    {
		var docList = new List<Document>();
        IFeatureSet fs = o.FeatureSet;

        if (o.LayerProjection.ToEsriString() != KnownCoordinateSystems.Geographic.World.WGS1984.ToEsriString())
        {
			// reproject the in-memory representation of our featureset (actual file remains unchanged)
            fs.Reproject(KnownCoordinateSystems.Geographic.World.WGS1984);
        }
        foreach (ShapeRange shapeRange in fs.ShapeIndices)  // cycle through each shape/record
        {
			var doc = new Document();  // new index doc for this record
            // snatch the row affiliated with this shape-range
            DataRow dr = fs.DataTable.Rows[shapeRange.RecordNumber - 1];
            // add standardized lookup fields for each record
            doc.Add(new Field(Fid, dr[Fid].ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field(Lyrname, o.LayerName, Field.Store.YES, Field.Index.NOT_ANALYZED));

            if (fs.FeatureType.ToString() != "Polygon")
			{
				// snatch the shape affiliated with the shape-range
				Shape shp = fs.GetShape(shapeRange.RecordNumber - 1, false);
				IGeometry geom = shp.ToGeometry(); // cast shape to geometry for wkt serialization

				// serialize the geometry into wkt (which will be read by spatial4n for lucene indexing)
				var wktWriter = new WktWriter();
				var wkt = wktWriter.Write((Geometry) geom);

				// create our strategy for spatial indexing using NTS context
				var ctx = NtsSpatialContext.GEO; // using NTS (provides polygon/line/point models)
				SpatialStrategy strategy = new RecursivePrefixTreeStrategy(new GeohashPrefixTree(ctx, 24), Geoshape);
				try // the esri and ogc definitions regarding polygons can differ and cause issues here
                {
					Spatial4n.Core.Shapes.Shape wktShp = ctx.ReadShape(wkt);
					foreach (var f in strategy.CreateIndexableFields(wktShp))
					{
						doc.Add(f); // add the geometry to the index for queries
					}
					// store the actual shape for later use on intersections searches
					doc.Add(new Field(strategy.GetFieldName(), ctx.ToString(wktShp), Field.Store.YES, Field.Index.NOT_ANALYZED));
				}
				catch (Exception ex)
				{
					LogGeometryIndexError(o.LayerName, dr[Fid].ToString(), shp, wkt, ex);
					var msg = AppContext.Instance.Get<IUserMessage>();
					msg.Error("Error creating index :: FeatureClass: " + o.LayerName + " FeatureID: " + dr[Fid], ex);
				}
			}
			// handle all other non-spatial field lookups
			// TODO: refactor so we dont need 2 loops; instead use the shape itself and grab attributes at same time (see example above)
			var list = o.FieldLookup;
			foreach (KeyValuePair<string, string> kv in list)
			{
				doc.Add(new Field(kv.Key, dr[kv.Value].ToString(), Field.Store.YES, Field.Index.ANALYZED));
                docList.Add(doc);  // add the new document to the documents list
            }
            if (docs.ContainsKey(o.IndexType))
            {
				// TODO: add a check to look for duplicates
                // if this index is already in existence, append our new docs
                List<Document> oldList;
                docs.TryGetValue(o.IndexType, out oldList);
                if (oldList != null) oldList.AddRange(docList);
            }
            else
            {
				docs.Add(o.IndexType, docList);
            }
            // reproject the in-memory representation of our featureset back to original projection if needed
            if (o.LayerProjection.ToEsriString() != KnownCoordinateSystems.Geographic.World.WGS1984.ToEsriString())
            {
				fs.Reproject(o.LayerProjection);
            }
        }
    return docs;
}

// UI button clicked to perform search
private void btnPerformSearch_Click(object sender, EventArgs eventArgs)
{
	if (_searchPanel.SearchQuery.Length <= 0) return;
	var q = _searchPanel.SearchQuery;
    _searchPanel.ClearSearches();  // clear any existing searches
    // setup columns, ordering, etc for results datagridview
    PrepareDataGridView();
    // execute our lucene query
    var hits = ExecuteLuceneQuery(q);
    FormatQueryResults(hits);
}

private IEnumerable<ScoreDoc> ExecuteLuceneQuery(string sq)
{
	ScoreDoc[] hits = null;
    switch (SearchMode)
    {
		case SearchMode.Address:
			hits = ExecuteScoredAddressQuery(sq);
			break;
        case SearchMode.Road:
			hits = ExecuteScoredRoadQuery(sq);
            break;
        case SearchMode.Intersection:
			hits = ExecuteGetIntersectionsQuery(sq);
            break;
        case SearchMode.All:
            hits = ExecuteScoredAllIndexesQuery(sq);
            break;
    }
    return hits;
}

private void FormatQueryResults(IEnumerable<ScoreDoc> hits)
{
	if (hits == null) return;
    switch (PluginSettings.Instance.SearchMode)
    {
		case SearchMode.Intersection:
			PopulateSingleQueryResultsToDgv(hits);
            UpdateIntersectedFeatures(hits);
            break;
        case SearchMode.All:
			PopulateMultiQueryResultsToDgv(hits);
            break;
        default:
			PopulateSingleQueryResultsToDgv(hits);
            break;
    }
}

private ScoreDoc[] ExecuteScoredAllIndexesQuery(string q)
{
	if (_indexSearcher.IndexReader == null)
    {
		return new ScoreDoc[0];
    }
    TopDocs docs = _indexSearcher.Search(GetSearchAllQuery(q), _indexSearcher.IndexReader.NumDocs());
    // return our results
    return docs.ScoreDocs;
}

private Query GetSearchAllQuery(string q)
{
	// fetch all the avilable field names that are searchable regardless of index
	var fldList = _indexSearcher.IndexReader.GetFieldNames(IndexReader.FieldOption.INDEXED);
	// arrays for storing all the values to pass into the index search
	var values = new ArrayList();
	var fields = new ArrayList();
	var occurs = new ArrayList();

	string[] qTermArray = q.Split(' ');
	foreach (string fld in fldList)
	{
		// no need to search these fields as they are for internal use only
		if (fld == "FID" || fld == "LYRNAME" || fld == "GEOSHAPE") continue;
		foreach (var qTerm in qTermArray)
		{
			values.Add(qTerm);
			fields.Add(fld);
			occurs.Add(Occur.SHOULD);
		}
	}
	// turn the array lists into static arrays
	var vals = (string[])values.ToArray(typeof(string));
	var flds = (string[])fields.ToArray(typeof(string));
	var ocrs = (Occur[])occurs.ToArray(typeof(Occur));

	// create lucene query from query string arrays
	Query query = MultiFieldQueryParser.Parse(
		LuceneVersion,
		vals,
		flds,
		ocrs,
		LuceneAnalyzer
	);
	return query;
}

private ScoreDoc[] ExecuteScoredAddressQuery(string q)
{
	// parse our input address into a valid streetaddress object
	StreetAddress streetAddress = StreetAddressParser.Parse(q, ProjectSettings.Instance.SearchUsePretypes);
	LogStreetAddressParsedQuery(q, streetAddress);
	// arrays for storing all the values to pass into the index search
	var values = new ArrayList();
	var fields = new ArrayList();
	var occurs = new ArrayList();
	// assemble our query string now
	if (streetAddress.Number != null)
	{
		values.Add(streetAddress.Number);
		fields.Add("Structure Number");
		occurs.Add(Occur.MUST);
	}
	if (streetAddress.Predirectional != null)
	{
		values.Add(streetAddress.Predirectional);
		fields.Add("Pre Directional");
		occurs.Add(Occur.SHOULD);
	}
	if (ProjectSettings.Instance.SearchUsePretypes)
	{
		if (streetAddress.PreType != null)
		{
			values.Add(streetAddress.PreType);
			fields.Add("Pre Type");
			occurs.Add(Occur.SHOULD);
		}
    }
    if (streetAddress.StreetName != null)
	{
		values.Add(streetAddress.StreetName);
		fields.Add("Street Name");
		occurs.Add(Occur.MUST);
	}
	if (streetAddress.StreetType != null)
	{
		values.Add(streetAddress.StreetType);
		fields.Add("Street Type");
		occurs.Add(Occur.SHOULD);
	}
	if (streetAddress.Postdirectional != null)
	{
		values.Add(streetAddress.Postdirectional);
		fields.Add("Post Directional");
		occurs.Add(Occur.SHOULD);
	}
	if (streetAddress.SubUnitType != null)
	{
		values.Add(streetAddress.SubUnitType);
		fields.Add("Sub Unit Type");
		occurs.Add(Occur.SHOULD);
	}
	if (streetAddress.SubUnitValue != null)
	{
		values.Add(streetAddress.SubUnitValue);
		fields.Add("Sub Unit Designation");
		occurs.Add(Occur.SHOULD);
	}
	var vals = (string[]) values.ToArray(typeof (string));
	var flds = (string[]) fields.ToArray(typeof (string));
	var ocrs = (Occur[]) occurs.ToArray(typeof (Occur));
	// create lucene query from query string arrays
	Query query = MultiFieldQueryParser.Parse(
		LuceneVersion,
		vals,
		flds,
		ocrs,
		LuceneAnalyzer
	);

	if (_indexSearcher.IndexReader == null)
	{
		return new ScoreDoc[0];
	}
	TopDocs docs = _indexSearcher.Search(query, _indexSearcher.IndexReader.NumDocs());
	// return our results
	return docs.ScoreDocs;
}

private void PopulateSingleQueryResultsToDgv(IEnumerable<ScoreDoc> hits)
{
	if (hits == null) return;
	foreach (var hit in hits)
	{
		// generate all the empty cells we need for a full row
        var newCells = new DataGridViewCell[_columnNames.Length];
        for (int i = 0; i <= _columnNames.Length - 1; i++)
        {
			var txtCell = new DataGridViewTextBoxCell();
            newCells[i] = txtCell;
        }
        // create the row and populate it
        var dgvRow = new DataGridViewRow();
        dgvRow.Cells.AddRange(newCells);
        // snatch the ranked document
        var doc = _indexSearcher.Doc(hit.Doc);
        foreach (var field in _columnNames)
        {
			var idx = GetColumnDisplayIndex(field, _dataGridView);
            var val = doc.Get(field);
			dgvRow.Cells[idx].Value = val;
        }
        // add the fid and layername textbox cells
        var fidCell = new DataGridViewTextBoxCell {Value = doc.Get(FID)};
		dgvRow.Cells.Add(fidCell);
		var lyrCell = new DataGridViewTextBoxCell {Value = doc.Get(LYRNAME)};
		dgvRow.Cells.Add(lyrCell);
		var shpCell = new DataGridViewTextBoxCell { Value = doc.Get(GEOSHAPE) };
		dgvRow.Cells.Add(shpCell);
		_dataGridView.Rows.Add(dgvRow);
	}
}

private void PopulateMultiQueryResultsToDgv(IEnumerable<ScoreDoc> hits)
{
	if (hits == null) return;

	IFragmenter fragmenter = new NullFragmenter();
	IScorer scorer = new QueryScorer(GetSearchAllQuery(_searchPanel.SearchQuery));
	Highlighter highlighter = new Highlighter(scorer) { TextFragmenter = fragmenter };

	foreach (var hit in hits)
	{
		var doc = _indexSearcher.Doc(hit.Doc);
		var fds = doc.GetFields();
		foreach (var fld in fds)
		{
			// check if this field is a match
			var f = highlighter.GetBestFragment(LuceneAnalyzer, fld.Name, fld.StringValue);
			if (f != null)
			{
				// generate the empty cells required for a full row
				var newCells = new DataGridViewCell[_columnNames.Length];
				for (int i = 0; i <= _columnNames.Length - 1; i++)
				{
					newCells[i] = new DataGridViewTextBoxCell();
				}
				// create the row and populate it
				var dgvRow = new DataGridViewRow();
				dgvRow.Cells.AddRange(newCells);

				// default columns displayed on an all index search
				var idx = GetColumnDisplayIndex("Layer Name", _dataGridView);
				dgvRow.Cells[idx].Value = doc.Get(LYRNAME);
				idx = GetColumnDisplayIndex("Field Name", _dataGridView);
				dgvRow.Cells[idx].Value = fld.Name;
				idx = GetColumnDisplayIndex("Field Value", _dataGridView);
				dgvRow.Cells[idx].Value = fld.StringValue;

				// add the fid and layername textbox cells (used by various functions)
				var fidCell = new DataGridViewTextBoxCell { Value = doc.Get(FID) };
				dgvRow.Cells.Add(fidCell);
				var lyrCell = new DataGridViewTextBoxCell { Value = doc.Get(LYRNAME) };
				dgvRow.Cells.Add(lyrCell);
				var shpCell = new DataGridViewTextBoxCell { Value = doc.Get(GEOSHAPE) };
				dgvRow.Cells.Add(shpCell);
				_dataGridView.Rows.Add(dgvRow);
			}
        }
    }
}
