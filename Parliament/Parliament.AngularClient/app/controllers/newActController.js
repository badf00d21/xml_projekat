app.controller('newActController', ['$scope','$location','actService', function ($scope, $location, actService) {

    $scope.xmlStr = "";
          //proveri da l je logovan
    /*x2js = new X2JS();
    console.log(x2js.xml_str2json("<Data><Ime>Petar</Ime></Data>"));
            (function () {
                $scope.options = {
                    schemaUri: 'http://localhost:8973/api/documents/schema/Propis1.xsd/',
                    schemaName: 'Propis1.xsd/',
                    rootElement: 'Propis',
                    submitPath: 'http://localhost:8973/api/propose/act/'
                }
            }());*/

    $scope.postXml = function () {
        console.log('usao');
        actService.napraviAkt($scope.xmlStr).then(function (response) {
            
            console.log(response);
            $location.path('/citizenhome');

        })
    };

        }]);