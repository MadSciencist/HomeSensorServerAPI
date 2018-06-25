app.controller("RootController", function ($rootScope, $http, $scope, $window, $route, $location) {

    $scope.logout = function () {
        console.log('Log out');
        $window.localStorage.clear();

        $rootScope.badAuthentication = false;
        $rootScope.badAuthenticationMessage = '';

        $route.reload();
    };


});