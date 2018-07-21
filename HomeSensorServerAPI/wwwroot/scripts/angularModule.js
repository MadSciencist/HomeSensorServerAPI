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
            controller: "UsersController",
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

app.run(function ($rootScope, $location, $window) {
    $rootScope.$on('$routeChangeStart', function (event, next, current) {
        if (next.$$route.auth) {
            const tokenValidTo = $window.localStorage.getItem('validTo');

            const tokenValidToObject = new Date(tokenValidTo);
            let currentTime = new Date();
            if (currentTime.getTime() > tokenValidToObject.getTime()) { //token is no longer valid
                
                if (tokenValidTo != null) {
                    $rootScope.badAuthentication = true;
                    $rootScope.badAuthenticationMessage = 'Twoja sesja wygasła. Zaloguj się ponownie.';
                }
                $location.path('/login');
            }
        }
        
    });
});