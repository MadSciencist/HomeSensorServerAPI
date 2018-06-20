var app = angular.module("smartHomeApp", ["ngRoute", "html5.sortable"]);

app.config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "pages/charts.html",
            controller: "HomeController"
        })
        .when("/charts", {
            templateUrl: "pages/charts.html",
            controller: "ChartsController"
        })
        .when("/control", {
            templateUrl: "pages/control.html",
            controller: "NodesController"
        })
        .when("/nodes", {
            templateUrl: "pages/nodes.html",
            controller: "NodesController"
        })
        .when("/users", {
            templateUrl: "pages/users.html",
            controller: "UsersController"
        })
        .when("/my-profile", {
            templateUrl: "pages/user.html",
            controller: "UserController"
        })
        .otherwise({
            templateUrl: "pages/not-found.html"
        });
});