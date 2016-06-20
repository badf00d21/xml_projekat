

restApiBaseUrl = 'http://localhost:8973/api/';


app.service('actService', ['$http', 'localStorageService', '$window', function ($http, localStorageService, $window) {
    x2js = new X2JS();
    
    this.ucitajSveAkte = function () {
        return $http.get(restApiBaseUrl + 'documents/acts/');
    }

    this.ucitajPredlozeneAkte = function () {
        return $http.get(restApiBaseUrl + 'documents/acts/proposed/');
    }

    this.napraviAkt = function (xmlStr) {
        //return $http.post(restApiBaseUrl + 'documents/acts/propose/', x2js.json2xml_str(xmlStr));

        return $http({
            method: 'POST',
            url: restApiBaseUrl + 'documents/acts/propose/',
            headers: {
                'Content-Type': 'text/xml' /*or whatever type is relevant */
            },
            data:
                /* You probably need to send some data if you plan to log in */
                x2js.json2xml_str(xmlStr),

        });

    }

    this.ucitajUsvojeneAkte = function () {
        return $http.get(restApiBaseUrl + 'documents/acts/adopted/');
    }

    this.pretraziAkte = function (body) {
        return $http.post(restApiBaseUrl + "documents/acts/filter/",body);
    }

    this.aktKaoHtml = function (idAkta) {
        $http.get(restApiBaseUrl + 'documents/acts/' + idAkta + '/html').then(function (data) {
            $window.open(data);
        })
    }

    this.aktKaoPdf = function (idAkta) {
       $http.get(restApiBaseUrl + 'documents/acts/' + idAkta + '/pdf')
            .then(function (data) {
                $window.open(data);
            })
    }

    this.aktKaoXml = function (idAkta) {
        $http.get(restApiBaseUrl + 'documents/acts/' + idAkta).then(function (data) {
            $window.open(data);
        })
    }

}]);