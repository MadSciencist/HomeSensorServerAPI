var app = angular.module("smartHomeApp", ['ngRoute']);

app.config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "index.html",
            controller: "RootController"
        })
        .otherwise({
            templateUrl: "pages/not-found.html"
        });
});

app.controller("RootController", function ($scope) {

    $scope.test = 'TEST TEST TESTa ';

});