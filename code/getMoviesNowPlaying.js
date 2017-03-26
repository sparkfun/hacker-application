/*
Drew I. Parker
26 March 2016

A function that calls The Movie Database (TMDB) and returns an array of objects
representing movies currently playing in theaters. The API limits the results to 20
per page.

About TMDB API: www.themoviedb.org/documentation/api

Ideally, this code sample will demonstrate to the reader the author's
understanding of jasvascript's asynchronous nature and some tools to manage it
(Promises). The sample should also convey to the reader the author's comfortableness
with ES6, which is the "new hotness."
*/

function getMoviesNowPlaying(page=1) {
    var tmdbKey = `api_key=1f809315a3a8c0a1456dd83615b4d783`;
    var movies = []; // An array to collect the movie objects.

    // The following TMDB API url will return 20 movies listed as `now playing`
    // when called. Each movie has several properties, but a few important details
    // are missing (i.e., Director, rating, etc.). These details exist at unique
    // endpoints (per movie), so when the results come back, I make a new API call
    // on each movie in order to collect them.
    var url = `https://api.themoviedb.org/3/movie/now_playing?${tmdbKey}&page=${page}`;
    $.getJSON(url).then(nowPlayingResults => {
        // When the call returns, iterate over the results with the Promise.all
        // method and make subsequent, unique API calls with the id property.
        return Promise.all(nowPlayingResults.results.map(result => {
            var movie = {}; // Build a unique object for each movie.
            var id = result.id;

            var detailedUrl = `https://api.themoviedb.org/3/movie/${id}?${tmdbKey}&append_to_response=credits,release_dates`;
            return $.getJSON(detailedUrl).then(detailedResult => {
                // When the this call returns, append a selection of result
                // properties to our own movie object.
                movie.title = detailedResult.title;
                movie.homepage = detailedResult.homepage;
                movie.imdb_id = detailedResult.imdb_id;
                movie.overview = detailedResult.overview;
                movie.poster_path = detailedResult.poster_path;
                movie.release_date = detailedResult.release_date;
                movie.runtime = detailedResult.runtime;
                movie.vote_average = detailedResult.vote_average;
                movie.vote_count = detailedResult.vote_count;

                // Find the director amongst the crew and assign his or her
                // name to the director property. First, assign `no info` text
                // in the event no director was found.
                movie.director = 'No info on director';
                var crew = detailedResult.credits.crew;
                for (let member of crew) {
                    if (member.job === 'Director') {
                        movie.director = member.name;
                        break;
                    }
                }
                // Find the US certification and assign that value to the
                // mpaa property. First, assign `NR` in the event no MPAA was
                // found.
                movie.mpaa = 'NR'
                var releaseCountries = detailedResult.release_dates.results
                for (let country of releaseCountries) {
                    if (country.iso_3166_1 === 'US') {
                        var certifications = country.release_dates;
                        for (let item of certifications) {
                            var mpaa = item.certification;
                            if (mpaa !== '') {
                                movie.mpaa = mpaa;
                                break;
                            }
                        }
                    }
                }
                // Push the object to our movie objects array
                return movies.push(movie);
            });
        }));
    })
    // Return the array of movie objects
    .then(() => {
        return movies;
    })
    // Throw any errors
    .catch(error => {
        throw error;
    });
}
