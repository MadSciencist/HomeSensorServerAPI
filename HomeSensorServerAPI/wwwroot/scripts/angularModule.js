let ChangeMainPanelName = function (name) {
    const names = [
        {
            id: 'charts',
            fullName: 'Wykresy z danych wszystkich czujników'
        },
        {
            id: 'control',
            fullName: 'Steruj swoimi urządzeniami'
        },

        {
            id: 'nodes',
            fullName: 'Zarządzaj swoimi urządzeniami'
        },
        {
            id: 'new-device-type',
            fullName: 'Zdefiniuj nowe urządzenie'
        },
        {
            id: 'users',
            fullName: 'Zarządzaj użytkownikami systemu'
        },
        {
            id: 'my-profile',
            fullName: 'Twoje dane'
        }];

    let fullName = names.filter(n => n.id === name)[0].fullName;

    $('#view-title').text(fullName);
};

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






app.controller("HomeController", function ($scope, $http, $window, $route) {


    $scope.login = function () {

        let credentials = {
            username: $scope.username,
            password: $scope.password
        };

        $.ajax({
            type: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=UTF-8'
            },
            data: JSON.stringify(credentials),
            url: '/api/token/',
            success: function (result) {
                hideNotAuthorizedPanel();
                sessionStorage.setItem("token", result.token);
                sessionStorage.setItem("userId", result.userId);
                getUserPublicData();
                hideLoginForm();
                $route.reload();
            },
            error: function (result) {
                checkIfNotAuthorized(result);
            }
        });
    };

    let checkIfNotAuthorized = function (response) {
        if (response.status === 401) {
            showNotAuthorizedPanel('Nie masz uprawnień do przeglądania tej strony. Zaloguj się.');
        }
    };

    let showNotAuthorizedPanel = function (text) {
        $('#not-authorized-error-text').text(' '.concat(text));
        $('#not-authorized-error').show();
    };

    let hideNotAuthorizedPanel = function () {
        $('#not-authorized-error').hide();
    };

let hideLoginForm = function () {
    document.getElementById("login_form").style.display = "none";
    document.getElementById("user_panel").style.display = "block";
    document.getElementById("email").value = "";
    document.getElementById("password").value = "";
};

let showLoginForm = function () {
    document.getElementById("login_form").style.display = "block";
    document.getElementById("user_panel").style.display = "none";
};

let getUserPublicData = function () {
    const token = sessionStorage.getItem('token');
    const myId = sessionStorage.getItem('userId');
    $.ajax({
        type: 'GET',
        headers: {
            'Content-Type': 'application-json; charset=UTF-8',
            'Authorization': 'Bearer '.concat(token)
        },
        url: '/api/users/'.concat(myId),
        success: function (response) {
            sessionStorage.setItem("user", response);
            document.getElementById("user_name").innerHTML = 'Witaj ponownie '.concat(response.name);
        }
    });
};

$scope.logout = function () {
    sessionStorage.clear();
    showLoginForm();
    $route.reload();
};

});


