define('model.customer',
    ['ko', 'config'],
    function (ko, config) {
        var
            
            Customer = function () {
                var self = this;
                self.customerId = ko.observable();
                self.firstName = ko.observable();
                self.lastName = ko.observable();
                self.fullName = ko.computed(function () {
                    return self.firstName() + ' ' + self.lastName();
                }, self);

                self.email = ko.observable().extend({ email: true });
                self.city = ko.observable();
                self.state = ko.observable();
                self.street1 = ko.observable();
                self.street2 = ko.observable();
                self.zipcode = ko.observable();
                //self.f_Zip = ko.computed(function () {
                //    return config.numberFormat(self.zipcode());
                //});
                self.phone1 = ko.observable();
                //self.f_Phone1 = ko.computed(function () {
                //    return config.numberFormat(self.phone1());
                //});
                self.phone2 = ko.observable();
                //self.f_phone2 = ko.computed(function () {
                //    return config.numberFormat(self.phone2());
                //});



                //self.imageSource = ko.observable();
                //self.imageName = ko.computed(function () {
                //    var source = self.imageSource();
                //    if (!source) {
                //        source = settings.unknownPersonImageSource;
                //    }
                //    return settings.imageBasePath + source;
                //}, self);

                self.isBrief = ko.observable(true);

                self.isNullo = false;

                self.dirtyFlag = new ko.DirtyFlag([
                    self.firstName,
                    self.lastName,
                    self.email,
                    self.city,
                    self.state,
                    self.street1,
                    self.street2,
                    self.zipcode,
                    self.phone1,
                    self.phone2
                                ]);
                return self;
            };

        Customer.Nullo = new Customer()
            .customerId(0)
            .firstName('Not a')
            .lastName('Person')
            .email('')
            .city('')
            .state('')
            .street1('')
            .street2('')
            .zipcode('')
            .phone1('')
            .phone2('');
        Customer.Nullo.isNullo = true;
        Customer.Nullo.isBrief = function () { return false; }; // nullo is never brief
        Customer.Nullo.dirtyFlag().reset();


        return Customer;
    });