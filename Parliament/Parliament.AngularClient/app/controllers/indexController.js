app.controller('indexController', ['$scope', '$location', '$http', 'authService', function ($scope, $location, $http, authService) {

    $scope.logOut = function () {
        authService.logOut();
        $location.path('/home');
    }

    $scope.authentication = authService.authentication;


    $scope.$watch(function () {
        return $location.path();
    },
    function (newValue, oldValue) {
        if (authService.authentication.isAuth == false && newValue != '/login' )
            $location.path('/login');
        else if (authService.authentication.isAuth == true && (newValue == '/home' || newValue == '/login' || newValue == '/signup'))
            redirectToRightRole();
        else if (authService.authentication.isAuth == true && newValue == '/profile')
            redirectToRightRole();
    });

    var redirectToRightRole = function () {
        if (authService.authentication.role == "Citizen")
            $location.path('/citizenhome');
        else if (authService.authentication.role == "Chairman")
            $location.path('/chairmanhome');
        else
            $location.path('/aldermanhome');
    }
}]);