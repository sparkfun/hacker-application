define('vm.customers',
    ['ko', 'datacontext', 'utils'],
    function (ko, datacontext, utils) {
        var
            customers = ko.observableArray(),
            dataOptions = function (force) {
                return {
                    results: customers,
                    //filter: sessionFilter,
                    //sortFunction: sort.sessionSort,
                    forceRefresh: force
                };
            },
            activate = function (routeData, callback) {
                getCustomers();
                refresh(callback);
                
            },
            getCustomers = function () {
                if (!customers().length) {
                    datacontext.customers.getData({
                        results: customers
                    });
                }
            },
            refresh = function (callback) {
                $.when(datacontext.customers.getData(dataOptions(false)))
                .always(utils.invokeFunctionIfExists(callback));
            }

        return {
            customers: customers,
            dataOptions: dataOptions,
            activate: activate
        };

    });