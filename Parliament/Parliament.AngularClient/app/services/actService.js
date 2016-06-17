

restApiBaseUrl = 'http://localhost:8973/api/';


app.service('actService', ['$http', 'localStorageService', function ($http, localStorageService) {
    x2js = new X2JS();
    
    this.ucitajSveAkte = function () {
        return $http.get(restApiBaseUrl + 'documents/acts/');
        //return fake_propisi;
    }

    this.ucitajPredlozeneAkte = function () {
        return $http.get(restApiBaseUrl + 'documents/acts/proposed/');
    }

    this.ucitajUsvojeneAkte = function () {
        return $http.get(restApiBaseUrl + 'documents/acts/adopted/');
        //usvojeni = [];
        //usvojeni.push(fake_propisi[3]);
        //usvojeni.push(fake_propisi[4]);
        //usvojeni.push(fake_propisi[5]);
        //return usvojeni;
    }

    this.pretraziAkte = function (body) {
        return $http.post(restApiBaseUrl + "documents/acts/filter/",body);
        //return "u izgradnji!";
    }


    
}]);