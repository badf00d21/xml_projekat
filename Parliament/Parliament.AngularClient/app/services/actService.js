var fake_propisi = [
    {
        uri: 'http://www.parliament.rs/documents/acts/14897332-5c5e-4edc-9f3e-5ebbb76655b2.xml',
        nazivPropisa: "Propis 1",
        status: "Predlozen"
    },
    {
        uri: 'http://www.parliament.rs/documents/acts/14897332-5c5e-4edc-9f3e-5ebbb76655b2.xml',
        nazivPropisa: "Propis 2",
        status: "Predlozen"
    },
    {
        uri: 'http://www.parliament.rs/documents/acts/14897332-5c5e-4edc-9f3e-5ebbb76655b2.xml',
        nazivPropisa: "Propis 3",
        status: "Predlozen"
    },
    {
        uri: 'http://www.parliament.rs/documents/acts/14897332-5c5e-4edc-9f3e-5ebbb76655b2.xml',
        nazivPropisa: "Propis 4",
        status: "Usvojen"
    },
    {
        uri: 'http://www.parliament.rs/documents/acts/14897332-5c5e-4edc-9f3e-5ebbb76655b2.xml',
        nazivPropisa: "Propis 5",
        status: "Usvojen"
    },
    {
        uri: 'http://www.parliament.rs/documents/acts/14897332-5c5e-4edc-9f3e-5ebbb76655b2.xml',
        nazivPropisa: "Propis 6",
        status: "Usvojen"
    },
];

restApiBaseUrl = 'https://localhost:8973/api/';


app.service('actService', ['$http', 'localStorageService', function ($http, localStorageService) {
    
    //$http.defaults.headers.common.Authorization = "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1bmlxdWVfbmFtZSI6Impvam9Aam8uY29tIiwic3ViIjoiam9qb0Bqby5jb20iLCJyb2xlIjoiQWxkZXJtYW4iLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0Ojg5MDQiLCJhdWQiOiI3ZmI2MTMyODRmNTA0Nzc2YWQ5NGRkYWRiNjUwMzZiZCIsImV4cCI6MTQ2NjA5NDQ5OCwibmJmIjoxNDY2MDA4MDk4fQ.WTtvkxNHWwEd1Zq08EYYUziUhI83y6Yr1ZV80ZkKHno";
    
    this.ucitajSveAkte = function () {
        return $http.get(restApiBaseUrl + 'documents/acts/');
        //return fake_propisi;
    }

    this.ucitajPredlozeneAkte = function () {
        return $http.get(restApiBaseUrl + 'documents/acts/proposed/');
        //predlozeni = [];
        //predlozeni.push(fake_propisi[0]);
        //predlozeni.push(fake_propisi[1]);
        //predlozeni.push(fake_propisi[2]);
        //return predlozeni;
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
        return $http.post(restApiBaseUrl + "documents/acts/filter/", body);
        //return "u izgradnji!";
    }


    
}]);