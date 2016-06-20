
var app = angular.module('AngularAuthApp', ['ngRoute', 'LocalStorageModule', 'angular-loading-bar', ]);

app.config(function ($routeProvider) {

    $routeProvider.when("/home", {
        controller: "homeController",
        templateUrl: "/app/views/home.html"
    });

    $routeProvider.when("/login", {
        controller: "loginController",
        templateUrl: "/app/views/login.html"
    });
    
    $routeProvider.when("/citizenhome", {
        controller: "citizenController",
        templateUrl: "/app/views/citizenHome.html"
    });

    $routeProvider.when("/chairmanhome", {
        controller: "chairmanController",
        templateUrl: "/app/views/chairmanHome.html"
    });

    $routeProvider.when("/aldermanhome", {
        controller: "aldermanController",
        templateUrl: "/app/views/aldermanHome.html"
    });

    $routeProvider.when("/aldermanhome", {
        controller: "aldermanController",
        templateUrl: "/app/views/aldermanHome.html"
    });

    $routeProvider.when("/newact", {
        controller: "newActController",
        templateUrl: "/app/views/newAct.html"
    });

    $routeProvider.otherwise({ redirectTo: "/home" });

});

app.constant('ngAuthSettings', {
    apiServiceBaseUri: 'https://localhost:44364/api/auth/',
    clientId: '7fb613284f504776ad94ddadb65036bd'
});

app.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptorService');
});

app.run(['authService', function (authService) {
    authService.fillAuthData();
}]);


