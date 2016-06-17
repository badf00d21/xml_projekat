
app.controller('citizenController', ['$scope', 'actService', function ($scope, actService, ModalService ) {

    $scope.akti = [];
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
    
        $scope.show = function () {
            ModalService.showModal({
                templateUrl: 'modal.html',
                controller: "modalController"
            }).then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {
                    $scope.message = "You said " + result;
                });
            });
        };
        
}]);