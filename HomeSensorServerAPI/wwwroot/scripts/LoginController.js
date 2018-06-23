app.controller("LoginController", function ($rootScope, $scope, $http, $window, $route, $location) {

    $scope.login = function () {

        let credentials = JSON.stringify({
            username: $scope.username,
            password: $scope.password
        });

        $http.post('/api/token/', credentials, {
            headers: {
                'Content-Type': 'application/json; charset=UTF-8'
            },
            'Accept': 'application/json'
        }).then(function onSuccess(response) {
            $scope.badAuthentication = false;
            $scope.username = '';
            $scope.password = '';
            $window.localStorage.setItem('token', response.data.token);
            $window.localStorage.setItem('userId', response.data.userId);
            $window.localStorage.setItem('validTo', response.data.tokenValidTo);
            getUserData();
        }, function onError(error) {
            $scope.username = '';
            $scope.password = '';
            $rootScope.badAuthentication = true;
            $rootScope.badAuthenticationMessage = error.data;
        });
    };

    let getUserData = function () {
        const token = sessionStorage.getItem('token');
        $http({
            method: 'GET',
            url: '/api/users/'.concat(1),
            headers: {
                'Content-Type': 'application-json; charset=UTF-8',
                'Authorization': 'Bearer '.concat(token)
            }
        }).then(function successCallback(response) {
            $scope.userData = response.data;
            $scope.userData.role = roleLookUpTable($scope.userData.role);
            $scope.userData.gender = genderLookUpTable($scope.userData.gender);
            $rootScope.user = $scope.userData;
            $location.path('/');
        }, function errorCallback(response) {
            console.error(response);
        });
    };

    let roleLookUpTable = function (roleId) {
        const roles = ['Admin', 'Manager', 'Viewer', 'Sensor'];
        return roles[roleId];
    };

    let genderLookUpTable = function (genderId) {
        const genders = ['Męzczyzna', 'Kobieta'];
        return genders[genderId];
    };
});