(function (angular) {
    "use strict";

    angular.module('app.NewActCtrl', [])
        .controller('newActController', function ($scope, $location, Auth) {

          /*  Auth.isLogged(function (isLogged) {
                if (!isLogged) {
                    $location.path('/login');
                }
            });*/

            (function () {
                $scope.options = {
                    schemaUri: '/api/schemas/',
                    schemaName: 'act.xsd',
                    rootElement: 'act',
                    submitPath: '/api/acts'
                }
            }());
        });
}(angular));