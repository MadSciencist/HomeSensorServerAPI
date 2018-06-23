app.controller("RootController", function ($rootScope, $scope, $window, $route) {

    $scope.logout = function () {
        console.log('Log out');
        $window.localStorage.clear();

        $rootScope.badAuthentication = false;
        $rootScope.badAuthenticationMessage = '';

        $route.reload();
    };
});