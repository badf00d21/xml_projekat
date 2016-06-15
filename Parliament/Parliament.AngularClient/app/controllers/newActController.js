app.controller('newActController', function ($scope, $location, Auth) {

          /*  Auth.isLogged(function (isLogged) {
                if (!isLogged) {
                    $location.path('/login');
                }
            });*/

            (function () {
                $scope.options = {
                    schemaUri: '/api/documents/schema',
                    schemaName: 'act.xsd',
                    rootElement: 'act',
                    submitPath: '/api/acts'
                }
            }());
        });