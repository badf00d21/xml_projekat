

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
       
        console.log(xmlStr);
        $http({
            method: 'POST',
            url: restApiBaseUrl + 'documents/propose/act/',
            data: xmlStr,
            headers: { "Content-Type": 'application/xml' }
        });

        /*
        return $http({
            method: 'POST',
            url: restApiBaseUrl + 'documents/acts/propose/',
            headers: {
                'Content-Type': 'text/xml' 
            },
            data:
                x2js.json2xml_str(xmlStr),

        });*/

    }

    this.ucitajUsvojeneAkte = function () {
        return $http.get(restApiBaseUrl + 'documents/acts/adopted/');
    }

    this.pretraziAkte = function (body) {
        return $http.post(restApiBaseUrl + "documents/acts/filter/",body);
    }

    this.aktKaoHtml = function (idAkta) {
        return $http.get(restApiBaseUrl + 'documents/acts/' + idAkta + '/html')
            .then(function (data) {
                var myWindow = window.open();
                myWindow.document.write(data.data);
            });
    }

    this.aktKaoPdf = function (idAkta) {
        return $http.get(restApiBaseUrl + 'documents/acts/' + idAkta + '/pdf')
            .then(function (data) {
                $window.open(restApiBaseUrl + 'documents/acts/' + idAkta + '/pdf');
            });
    }

    this.aktKaoXml = function (idAkta) {
        return $http.get(restApiBaseUrl + 'documents/acts/' + idAkta)
            .then(function (data) {
                var myWindow = window.open();
                console.log(x2js.json2xml_str(data.data));
                myWindow.document.write('<html><head></head><body>' + x2js.json2xml_str(data.data) + '</body></html>');
            });
    }

}]);