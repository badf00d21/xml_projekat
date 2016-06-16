﻿
app.controller('citizenController', ['$scope', 'actService', function ($scope, actService) {

    x2js = new X2JS();
    $scope.predlozeniAkti = [];
    $scope.usvojeniAkti = [];
    $scope.sviAkti = [];


    var refreshData = function () {
        actService.ucitajPredlozeneAkte().then(function (response) {
            $scope.predlozeniAkti = x2js.xml_str2json(response.data).Propisi.Propis;
        });
        actService.ucitajUsvojeneAkte().then(function (response) {
            $scope.usvojeniAkti = x2js.xml_str2json(response.data).Propisi.Propis;
        });
        actService.ucitajSveAkte().then(function (response) {
            $scope.sviAkti = x2js.xml_str2json(response.data).Propisi.Propis;
        });
    }

    refreshData();

}]);