app.controller("UserController", function ($rootScope, $scope, $http, $route, userService) {

    userService.getUserData();
    console.log('USER CONTROLLER CREATED');

});