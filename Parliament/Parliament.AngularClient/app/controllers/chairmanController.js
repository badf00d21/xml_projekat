
app.controller('chairmanController', ['$scope', 'actService', function ($scope, actService) {

    x2js = new X2JS();
    $scope.akti = [];
    $scope.idAkta = "";
    $scope.searchBody = {
        Naziv: "",
        Status: "",
        DatumVremePredlaganja: "",
        DatumVremeUsvajanja: "",
        Text: "",
        ImeNadleznogOrgana: "",
        PrezimeNadleznogOrgana: "",
        EmailNadleznogOrgana: ""
    }

    var refreshData = function () {
        actService.ucitajSveAkte().then(function (response) {
            $scope.akti = response.data.Propisi.Propis;
        });
    }

    refreshData();

    $scope.search = function () {
        actService.pretraziAkte($scope.searchBody).then(function (response) {
            $scope.akti.length = 0;
            if (!angular.isArray(response.data.Propisi.Propis))
                $scope.akti.push(response.data.Propisi.Propis);
            else {
                for (var i = 0; i < response.data.Propisi.Propis.length; i++)
                    $scope.akti.push(response.data.Propisi.Propis[i]);
            }

        });
    }

     $scope.kaoHtml = function () {
         actService.aktKaoHtml($scope.idAkta).then(function (response) {
            
        });
    }

    $scope.kaoXml = function () {
        actService.aktKaoXml($scope.idAkta).then(function (response) {

        });
    }

    $scope.kaoPdf = function () {
        actService.aktKaoPdf($scope.idAkta).then(function (response) {

        });
    }
}]);