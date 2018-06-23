var app = angular.module("smartHomeApp", ["ngRoute", "html5.sortable"]);

app.config(function ($routeProvider) {
    $routeProvider
        .when("/", {
            templateUrl: "pages/charts.html",
            controller: "ChartsController",
            auth: true
        })
        .when("/charts", {
            templateUrl: "pages/charts.html",
            controller: "ChartsController",
            auth: true
        })
        .when("/control", {
            templateUrl: "pages/control.html",
            controller: "NodesController",
            auth: true
        })
        .when("/nodes", {
            templateUrl: "pages/nodes.html",
            controller: "NodesController",
            auth: true
        })
        .when("/users", {
            templateUrl: "pages/users.html",
            controller: "UserController",
            auth: true
        })
        .when("/my-profile", {
            templateUrl: "pages/user.html",
            controller: "UserController",
            auth: true
        })
        .when("/login", {
            templateUrl: "pages/login.html",
            controller: "LoginController"
        })
        .otherwise({
            templateUrl: "pages/not-found.html"
        });
});

app.run(["$rootScope", "$location", "$window", function ($rootScope, $location, $window) {
    $rootScope.$on('$routeChangeStart', function (event, next, current) {

        const token = $window.localStorage.getItem('token');
        const userId = $window.localStorage.getItem('userId');

        if (next.$$route.auth) {
            if (userId === null) {
                console.log('NULL USER ID');
                $location.path('/login');
            }
        }
        
    });
}]);