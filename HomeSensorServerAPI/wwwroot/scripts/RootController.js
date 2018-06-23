app.controller("RootController", function ($rootScope, $scope, $http, $window, $route, $location) {

    $scope.logout = function () {
        console.log('Log out');
        $window.localStorage.removeItem('token');
        $window.localStorage.removeItem('userId');
        $window.localStorage.removeItem('tokenIssueTime');
        $window.localStorage.removeItem('validTo');
        $route.reload();
    };

});