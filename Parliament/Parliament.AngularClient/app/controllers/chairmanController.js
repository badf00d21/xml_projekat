'use strict';
app.controller('chairmanController', ['$scope', 'actService', function ($scope, actService) {

    $scope.predlozeniAkti = [];
    $scope.usvojeniAkti = [];

    var refreshData = function () {
        // actService.ucitajPredlozeneAkte().then(function (response) {
        //   $scope.predlozeniAkti = response.data;
        // });
        $scope.predlozeniAkti = actService.ucitajPredlozeneAkte();

        //actService.ucitajUsvojeneAkte().then(function (response) {
        //   $scope.usvojeniAkti = response.data;
        //});
        $scope.usvojeniAkti = actService.ucitajUsvojeneAkte();
    }

    refreshData()

}]);