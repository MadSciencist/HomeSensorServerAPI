﻿app.controller("LoginController", function ($rootScope, $scope, $http, $window, $route, $location) {

    $scope.login = function () {

        let credentials = JSON.stringify({
            username: $scope.username,
            password: $scope.password
        });

        $http.post('/api/token/', credentials, {
            headers: {
                'Content-Type': 'application/json; charset=UTF-8'
            },
            'Accept': 'application/json'
        }).then(function onSuccess(response) {
            $scope.badAuthentication = false;
            $scope.username = '';
            $scope.password = '';
            $window.localStorage.setItem('token', response.data.token);
            $window.localStorage.setItem('userId', response.data.userId);
            $window.localStorage.setItem('validTo', response.data.tokenValidTo);

            $location.path('/');

        }, function onError(error) {
            $scope.username = '';
            $scope.password = '';
            $rootScope.badAuthentication = true;
            $rootScope.badAuthenticationMessage = error.data;
        });
    };
});