(function () {
    'use strict';

    angular
        .module('app')
        .factory('apiFactory', ['$http', apiFactory]);

    apiFactory.$inject = ['$http'];

    function apiFactory($http) {
        var service = {
            getData: getData
        };

        return service;

        function getData(path) { return $http.get('/api/explorer/' + path);}
    }
})();