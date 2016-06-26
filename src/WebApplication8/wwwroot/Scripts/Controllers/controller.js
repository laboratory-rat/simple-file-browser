(function () {
    'use strict';

    angular
        .module('app')
        .controller('explorerController', ['$scope', 'apiFactory', explorerController]);

    explorerController.$inject = ['$scope'];

    function explorerController($scope, apiFactory) {
        $scope.data = null;

        $scope.UpdateData = function (path) {
            apiFactory.getData(path).success(function (data)
            {
                $scope.data = data;
            });
        };

        activate();

        function activate() { $scope.UpdateData(""); }
    }
})();