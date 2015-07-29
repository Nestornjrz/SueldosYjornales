(function () {
    'use strict';

    angular
        .module('sueldosYjornalesApp')
        .controller('anticipoCtrl', anticipoCtrl);

    anticipoCtrl.$inject = ['$location']; 

    function anticipoCtrl($location) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'anticipoCtrl';

        activate();

        function activate() { }
    }
})();
