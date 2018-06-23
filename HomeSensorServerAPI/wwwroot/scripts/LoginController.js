app.controller("LoginController", function ($scope, $http, $window, $route) {

    $scope.login = function () {

        let credentials = JSON.stringify({
            username: $scope.username,
            password: $scope.password
        });

        console.log(credentials);

        $.ajax({
            type: 'POST',
            headers: {
                'Content-Type': 'application/json; charset=UTF-8'
            },
            data: credentials,
            url: '/api/token/',
            success: function (response) {
                console.log(response);
                sessionStorage.setItem("token", response.token);
                sessionStorage.setItem("userId", response.userId);
            },
            error: function (response) {
                console.log(response);
            }
        });
    };



    $scope.logout = function () {
        sessionStorage.clear();
        showLoginForm();
        $route.reload();
    };

});