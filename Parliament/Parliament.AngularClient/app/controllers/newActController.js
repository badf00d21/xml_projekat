app.controller('newActController', function ($scope, $location) {

          //proveri da l je logovan
    x2js = new X2JS();
    console.log(x2js.xml_str2json("<Data><Ime>Petar</Ime></Data>"));
            (function () {
                $scope.options = {
                    schemaUri: 'http://localhost:8973/api/documents/schema/Propis1.xsd/',
                    schemaName: 'Propis1.xsd/',
                    rootElement: 'Propis',
                    submitPath: 'http://localhost:8973/api/propose/act/'
                }
            }());
        });