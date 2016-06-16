
app.factory('authService', ['$http', '$q', 'localStorageService', 'ngAuthSettings', function ($http, $q, localStorageService, ngAuthSettings) {

    var serviceBase = ngAuthSettings.apiServiceBaseUri;
    var authServiceFactory = {};

    var _authentication = {
        isAuth: false,
        fisrtName: "",
        lastName: "",
        email: "",
        role: ""
    };

   
    

    var _login = function (loginData) {

        var data = "grant_type=password&username=" + loginData.userName + "&password=" + loginData.password + "&client_id=" + "7fb613284f504776ad94ddadb65036bd";

        var username = loginData.userName.replace("@", "_");
        username = username.replace(".", "_");
        var deferred = $q.defer();


        $http.post(serviceBase + 'token', data, { headers: { 'Content-Type': 'application/x-www-form-urlencoded' } }).success(function (response) {

            console.log(response);

            localStorageService.set('authorizationData',
                {
                    token: response.access_token
                });

            $http.get('https://localhost:44337/api/users/username/' + loginData.userName + '/').success(function (userresponse) {
                x2js = new X2JS();
                userresponse = x2js.xml_str2json(userresponse).UserInfoViewModel;

                _authentication.email = userresponse.Email;
                _authentication.firstName = userresponse.FirstName;
                _authentication.lastName = userresponse.LastName;
                _authentication.role = userresponse.Role;

                localStorageService.set('authorizationData',
                {
                    
                    userName: username,
                    email: _authentication.email,
                    firstName: _authentication.firstName,
                    lastName: _authentication.lastName,
                    role: _authentication.role
                });

                _authentication.isAuth = true;
                _authentication.userName = username;

                deferred.resolve(response);
            }).error(function(err, status){
                console.log("nije pokupio podatke!!!!");

                });
            
           

        }).error(function (err, status) {
            _logOut();
            deferred.reject(err);
        });

        return deferred.promise;

    };

    var _logOut = function () {

        localStorageService.remove('authorizationData');

        _authentication.isAuth = false;
        _authentication.fisrtName = "";
        _authentication.lastName = "";
        _authentication.email = "";
        _authentication.role = "";

    };

    var _fillAuthData = function () {

        var authData = localStorageService.get('authorizationData');
        if (authData) {
            _authentication.isAuth = true;
            _authentication.fisrtName = authData.userName;
            _authentication.lastName = authData.lastName;
            _authentication.email = authData.email;
            _authentication.role = authData.role;
        }

    };

    

    authServiceFactory.login = _login;
    authServiceFactory.logOut = _logOut;
    authServiceFactory.fillAuthData = _fillAuthData;
    authServiceFactory.authentication = _authentication;


    return authServiceFactory;
}]);