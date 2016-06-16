'use strict';
app.controller('aldermanController', ['$scope', '$location', 'actService', function ($scope, $location, actService) {

    $scope.predlozeniAkti = [];
    $scope.usvojeniAkti = [];

    var refreshData = function () {
        actService.ucitajPredlozeneAkte().then(function (response) {
            $scope.predlozeniAkti = response.data;
         });
        //$scope.predlozeniAkti = actService.ucitajPredlozeneAkte();

        actService.ucitajUsvojeneAkte().then(function (response) {
            $scope.usvojeniAkti = response.data;
        });
        //$scope.usvojeniAkti = actService.ucitajUsvojeneAkte();
    }

    refreshData()


}]);