app.controller('newActController', function ($scope, $location) {

          //proveri da l je logovan

            (function () {
                $scope.options = {
                    schemaUri: 'http://localhost:8973/api/documents/schema/Propis1.xsd/',
                    schemaName: 'Propis1.xsd/',
                    rootElement: 'Propis',
                    submitPath: 'http://localhost:8973/api/propose/act/'
                }
            }());
        });