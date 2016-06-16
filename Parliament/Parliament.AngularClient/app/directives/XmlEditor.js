app.directive('xmlEditor', function () {
            return {
                restrict: 'E',
                template: '<iframe class="editor" ng-src="{{ createUrl() }}" width="100%" height="630">',
                scope: {
                    options: '='
                },
                link: function (scope) {
                    scope.createUrl = function () {
                        var url = '/xml-editor/editor.html', first = true;
                        for (var key in scope.options) {
                            if (scope.options.hasOwnProperty(key)) {
                                url += (first ? '?' : '&') + key + '=' + scope.options[key];
                                first = false;
                            }
                        }
                        return url;
                    };
                }
            }
        });