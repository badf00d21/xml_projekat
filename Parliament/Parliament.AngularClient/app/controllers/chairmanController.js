
app.controller('chairmanController', ['$scope', 'actService','authService', function ($scope, actService,authService) {

    x2js = new X2JS();
    $scope.akti = [];
    $scope.authentication = authService.authentication;
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

    $scope.kaoHtml = function (idAkta) {
        actService.aktKaoHtml(idAkta).then(function (response) {
            
        });
    }

    $scope.kaoXml = function (idAkta) {
        actService.aktKaoXml(idAkta).then(function (response) {

        });
    }

    $scope.kaoPdf = function (idAkta) {
        actService.aktKaoPdf(idAkta).then(function (response) {

        });
    }

    $scope.usvojCeo = function (idAkta) {
        actService.usvojiUCelosti(idAkta).then(function (response) {
            $scope.$apply();
        });
    }

    $scope.usvojNacelo = function (idAkta) {
        actService.usvojiUNacelu(idAkta).then(function (response) {
            $scope.$apply();
        });
    }
}]);