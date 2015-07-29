/*  IMF WEO 2014 data presentation for the UN
 *
 *  IMFUNSERVER
 *  app.js
 *  @author: William Barstad
 *  Description: This is the application initialization routine.
 *
 *  This function performs the following actions:
 *  -- Initializes the web server (Express)
 *  -- Imports the data from a file and forms it into JSON
 *  -- Creates a mongodb instance called 'imfun' and populates
 *      a collection called 'subjects' with the imported data,
 *      if it doesn't already exist.
 *
 *  Public Interfaces:
 *      '\':                        Application interface
 *      '\api':                     Information page list available RESTful operations
 *      '\subjects':                Returns a list of available report subjects
 *      '\subjects\:subjectcode':   Returns all data for the requested subject code (i.e. 'NGDP')
 *
 *  NOTES:
 *         20150528(WJB): The original file as downloaded is not a genuine XLS file. It is a CSV file with a an
 *          XLS extension. Therefore, I am treating it for what it is: a CSV file. There is a previous
 *          iteration of the application that was designed to accept an XLS file as input and can
 *          be rolled in to this app at a later time.
 *
 *  TODO: Add a graceful exit routine
 *
 * */
// TODO: Add routine to accept CSV or XLS/X files
var weoXLSPath = 'data/WEOOct2014all.csv';

var express = require('express');
var path = require('path');
var favicon = require('serve-favicon');
var logger = require('morgan');
var cookieParser = require('cookie-parser');
var bodyParser = require('body-parser');
var FS = require('fs');
var bufferString = '';

/*  MONGO/MONK/DB creation   */
var mongo = require('mongodb');
var monk = require('monk');
var db = monk('localhost:27017/imfunDB');

/*  Define/fetch the COLLECTIONs */
var subjectColl = db.get('subjects');

/*  FS.readFile
 *  Description:    Read the datafile and call the import process,
 *                  if the database does not already exist.
 * */
FS.readFile(weoXLSPath, function (err, data) {
    if (err) {
        console.log('FS.readFile: ' + err.message);
        throw err;
    }

    // If the check for one record is successful, we will
    // assume the database exists and is populated, otherwise
    // populate it.
    subjectColl.findOne({}).on('success', function (doc) {
        (doc == null) ? popDB() : console.log('Database already populated!');
    });

    function popDB() {
        console.log('Populating Database (async)...');
        // Remove \r (soft returns) from the string
        bufferString = data.toString().replace(/\r/, '');
        importData(bufferString);
    }
});

/*  importData(bufferStr)
 *  Description:    This function parses through the passed buffer string
 *                  (bufferStr) to add records the the database.
 * */
function importData(bufferStr) {
    // Escape special characters in the buffer
    bufferStr = escapeRegExp(bufferStr);
    // Split into rows
    var rowList = bufferStr.split('\n');

    // Spin through the row list, adding records to database
    for (var i = 1, max = rowList.length; i < max; i++) {

        var fields = rowList[i].split('\t');

        /*  SUBJECT record schema   */
        var recStr = {
            country_code:               parseInt(fields[0]),                // WEO Country Code
            iso:                        fields[1],                          // ISO
            subject_code:               fields[2],                          // WEO Subject Code
            country:                    fields[3],                          // Country
            subject_desc:               fields[4],                          // Subject Descriptor
            subject_notes:              fields[5],                          // Subject Notes
            units:                      fields[6],                          // Units
            scale:                      fields[7],                          // Scale
            country_notes:              fields[8],                          // Country/Series specific notes
            data: [
                {x: 1980, y: (fields[9] == 'n/a') ? 0 : parseFloat(fields[9])},
                {x: 1981, y: (fields[10] == 'n/a') ? 0 : parseFloat(fields[10])},
                {x: 1982, y: (fields[11] == 'n/a') ? 0 : parseFloat(fields[11])},
                {x: 1983, y: (fields[12] == 'n/a') ? 0 : parseFloat(fields[12])},
                {x: 1984, y: (fields[13] == 'n/a') ? 0 : parseFloat(fields[13])},
                {x: 1985, y: (fields[14] == 'n/a') ? 0 : parseFloat(fields[14])},
                {x: 1986, y: (fields[15] == 'n/a') ? 0 : parseFloat(fields[15])},
                {x: 1987, y: (fields[16] == 'n/a') ? 0 : parseFloat(fields[16])},
                {x: 1988, y: (fields[17] == 'n/a') ? 0 : parseFloat(fields[17])},
                {x: 1989, y: (fields[18] == 'n/a') ? 0 : parseFloat(fields[18])},
                {x: 1990, y: (fields[19] == 'n/a') ? 0 : parseFloat(fields[19])},
                {x: 1991, y: (fields[20] == 'n/a') ? 0 : parseFloat(fields[20])},
                {x: 1992, y: (fields[21] == 'n/a') ? 0 : parseFloat(fields[21])},
                {x: 1993, y: (fields[22] == 'n/a') ? 0 : parseFloat(fields[22])},
                {x: 1994, y: (fields[23] == 'n/a') ? 0 : parseFloat(fields[23])},
                {x: 1995, y: (fields[24] == 'n/a') ? 0 : parseFloat(fields[24])},
                {x: 1996, y: (fields[25] == 'n/a') ? 0 : parseFloat(fields[25])},
                {x: 1997, y: (fields[26] == 'n/a') ? 0 : parseFloat(fields[26])},
                {x: 1998, y: (fields[27] == 'n/a') ? 0 : parseFloat(fields[27])},
                {x: 1999, y: (fields[28] == 'n/a') ? 0 : parseFloat(fields[28])},
                {x: 2000, y: (fields[29] == 'n/a') ? 0 : parseFloat(fields[29])},
                {x: 2001, y: (fields[30] == 'n/a') ? 0 : parseFloat(fields[30])},
                {x: 2002, y: (fields[31] == 'n/a') ? 0 : parseFloat(fields[31])},
                {x: 2003, y: (fields[32] == 'n/a') ? 0 : parseFloat(fields[32])},
                {x: 2004, y: (fields[33] == 'n/a') ? 0 : parseFloat(fields[33])},
                {x: 2005, y: (fields[34] == 'n/a') ? 0 : parseFloat(fields[34])},
                {x: 2006, y: (fields[35] == 'n/a') ? 0 : parseFloat(fields[35])},
                {x: 2007, y: (fields[36] == 'n/a') ? 0 : parseFloat(fields[36])},
                {x: 2008, y: (fields[37] == 'n/a') ? 0 : parseFloat(fields[37])},
                {x: 2009, y: (fields[38] == 'n/a') ? 0 : parseFloat(fields[38])},
                {x: 2010, y: (fields[39] == 'n/a') ? 0 : parseFloat(fields[39])},
                {x: 2011, y: (fields[40] == 'n/a') ? 0 : parseFloat(fields[40])},
                {x: 2012, y: (fields[41] == 'n/a') ? 0 : parseFloat(fields[41])},
                {x: 2013, y: (fields[42] == 'n/a') ? 0 : parseFloat(fields[42])},
                {x: 2014, y: (fields[43] == 'n/a') ? 0 : parseFloat(fields[43])},
                {x: 2015, y: (fields[44] == 'n/a') ? 0 : parseFloat(fields[44])},
                {x: 2016, y: (fields[45] == 'n/a') ? 0 : parseFloat(fields[45])},
                {x: 2017, y: (fields[46] == 'n/a') ? 0 : parseFloat(fields[46])},
                {x: 2018, y: (fields[47] == 'n/a') ? 0 : parseFloat(fields[47])},
                {x: 2019, y: (fields[48] == 'n/a') ? 0 : parseFloat(fields[48])}
            ],
            //TODO make this a number, not a string
            estimates_starts_after: remR(fields[49])
        };

        var recJSON = JSON.stringify(recStr);

        //  SUBJECT
        try {
            subjectColl.insert(JSON.parse(recJSON), function (err, result) {
                if (err) throw err;
                //if (result) console.log('Subject Added!');
            });
        } catch (e) {
            console.log('subject record insert: In row loop (i=' + i + '): ' + e.message);
            i = max;
        }
    }

// SUBJECTLIST
// Extract the unique subjectlist collection from the subjects dataset
    subjectColl.distinct('subject_code', function (err, result) {
        var subjectListColl = db.get('subjectlist');
        // Add records to the collection, if it is empty
        subjectListColl.findOne({}, {}, function (err, doc) {
            if (doc == null) {
                for (var j = 0, jmax = result.length; j < jmax; j++) {
                    subjectColl.findOne({'subject_code': result[j]}, {}, function (err, item) {
                        var listRec = {
                            subject_code: item.subject_code,
                            subject_desc: unEscapeRegExp(item.subject_desc),
                            subject_notes: unEscapeRegExp(item.subject_notes)
                        };
                        var listJSON = JSON.stringify(listRec);
                        subjectListColl.insert(JSON.parse(listJSON));
                    });
                }
            }
        });
    });
}

/*
 *  escapeRegExp(str)
 *  Description:    Escapes special characters in a string (str).
 * */
function escapeRegExp(str) {
    if (str) {
        return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\\\^\$\|]/g, "\\$&");
    }
}

/*
 *  unEscapeRegExp(str)
 *  Description:    unEscapes special characters in a string (str).
 * */
function unEscapeRegExp(str) {
    if (str) {
        return (str + '')
            .replace(/\\(.?)/g, function (s, n1) {
                switch (n1) {
                    case '\\':
                        return '\\';
                    case '0':
                        return '\u0000';
                    case '?':
                        return '`';
                    case '':
                        return '';
                    default:
                        return n1;
                }
            });
    }
}

/*
 * Function:    remR
 * Description: Remove \r from a string
 * */
function remR(str) {
    if (str) return str.replace(/\r/, '');
}

/*  Define ROUTES   */
var routes = require(__dirname + '/routes/index');
var subjects = require(__dirname + '/routes/subjects');

/*  Start EXPRESS   */
var app = express();

/*  Set jade as the api viewer  */
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'jade');

app.use(favicon(__dirname + '/public/favicon.ico'));
app.use(logger('dev'));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({extended: false}));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

// Attach the DB to the request object
app.use(function (req, res, next) {
    req.db = db;
    next();
});

// Define routes
app.use('/', routes);
app.use('/subjects', subjects);

/*
 *  ERROR HANDLERS
 * */

// Catch any 404 errors and pass to error handler
app.use(function (req, res, next) {
    var err = new Error('Not Found');
    err.status = 404;
    next(err);
});

//  Development error handler, prints stacktrace
if (app.get('env') === 'development') {
    app.use(function (err, req, res) {
        res.status(err.status || 500);
        res.render('error', {
            message: err.message,
            error: err
        });
    });
}

// Production error handler, no stacktraces leaked to user
app.use(function (err, req, res) {
    res.status(err.status || 500);
    res.render('error', {
        message: err.message,
        error: {}
    });
});

module.exports = app;