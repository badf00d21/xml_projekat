(function (angular) {
    'use strict';

    angular.module('app.Acts', [])

        .factory('Acts', ['$http', function ($http) {

            return {
                getById: function (id) {
                    return $http({
                        method: 'GET',
                        url: 'api/acts/' + id
                    });
                },
                get: function (filter) {
                    return $http({
                        method: 'GET',
                        url: 'api/acts/',
                        params: filter
                    });
                },
                getHtml: function (id) {
                    return $http({
                        method: 'GET',
                        url: 'api/export/html/' + id
                    });
                },
                getHtmlEdit: function (id) {
                    return $http({
                        method: 'GET',
                        url: 'api/export/html-edit/' + id
                    });
                }
            };
        }]);

}(angular));