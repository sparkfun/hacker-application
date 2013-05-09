(function (ko) {
    ko.PagedObservableArray = function (options) {
        options = options || {};
        if ($.isArray(options))
            options = { data: options };
        var
		//the complete data collection
        _allData = ko.observableArray(options.data || []),

		//the size of the pages to display
        _pageSize = ko.observable(options.pageSize || 10),

        //number of page tabs to display
        _pageTabCount = ko.observable(options.pageTabCount || 5),

		//the index of the current page
        _pageIndex = ko.observable(0),

		//the current page data
        _page = ko.computed(function () {
            var pageSize = _pageSize(),
                pageIndex = _pageIndex(),
                startIndex = pageSize * pageIndex,
                endIndex = pageSize * (pageIndex + 1);

            return _allData().slice(startIndex, endIndex);
        }, this),

        _pageTabs = ko.computed(function () {
            var pageArr = ko.observableArray([]),
                currIndex = _pageIndex(),
                count = Math.ceil(_allData().length / _pageSize()) || 1;
            var pageTab = function (index, active) {
                this.index = ko.observable(index);
                this.active = ko.observable(active);
            };
            var midpoint = _pageTabCount();

            //for the monster crazy pager
            if (count >= ((midpoint * 2) + 1)) {
                if (currIndex + 1 <= midpoint) {
                    for (var x = 1; x <= (midpoint * 2) + 1; x++) {
                        if (x == currIndex + 1) {
                            pageArr.push(new pageTab(x, true));
                        } else {
                            pageArr.push(new pageTab(x, false));
                        }
                    }
                } else if ((currIndex + 1) >= (count - midpoint)) {
                    for (var x = (count - (midpoint * 2)); x <= count; x++) {
                        if (x == currIndex + 1) {
                            pageArr.push(new pageTab(x, true));
                        } else {
                            pageArr.push(new pageTab(x, false));
                        }
                    }
                } else {
                    for (var x = (currIndex - midpoint) ; x < currIndex; x++) {
                        pageArr.push(new pageTab(x + 1, false));
                    }
                    pageArr.push(new pageTab(currIndex + 1, true));
                    for (var x = (currIndex + 1) ; x <= currIndex + midpoint; x++) {
                        pageArr.push(new pageTab(x + 1, false));
                    }
                }
            } else { //for the not monster pager
                for (x = 1; x <= count; x++){
                    if (x == currIndex + 1) {
                        pageArr.push(new pageTab(x, true));
                    } else {
                        pageArr.push(new pageTab(x, false));
                    }
                }
            }
            return pageArr;
        }, this),

		//the number of pages
        _pageCount = ko.computed(function () {
            return Math.ceil(_allData().length / _pageSize()) || 1;
        }),

        //go to specific page
        _goToPage = function (pageNum) {
            _pageIndex((pageNum - 1));
        },

        _firstPage = function () {
            _pageIndex(0);
        },
        _lastPage = function () {
            _pageIndex(_pageCount() - 1);
        },
		//move to the next page
        _nextPage = function () {
            if (_pageIndex() < (_pageCount() - 1))
                _pageIndex(_pageIndex() + 1);
        },

		//move to the previous page
        _previousPage = function () {
            if (_pageIndex() > 0)
                _pageIndex(_pageIndex() - 1);
        };

        //reset page index when page size changes
        _pageSize.subscribe(function () { _pageIndex(0); });
        _allData.subscribe(function () { _pageIndex(0); });

        //public members
        this.allData = _allData;
        this.pageSize = _pageSize;
        this.pageIndex = _pageIndex;
        this.page = _page;
        this.pageCount = _pageCount;
        this.nextPage = _nextPage;
        this.previousPage = _previousPage;
        this.pageTabs = _pageTabs;
        this.goToPage = _goToPage;
        this.firstPage = _firstPage;
        this.lastPage = _lastPage;
    };
})(ko);