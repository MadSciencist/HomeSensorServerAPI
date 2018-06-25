app.controller("UserController", function ($scope, $http, $route) {

    //TODO: user id should be gathered after user login
    $scope.getUserData = function () {
        const token = sessionStorage.getItem('token');
        $http({
            method: 'GET',
            url: '/api/users/'.concat(9),
            headers: {
                'Content-Type': 'application-json; charset=UTF-8',
                'Authorization': 'Bearer '.concat(token)
            }
        }).then(function successCallback(response) {
            $scope.userData = response.data;
            $scope.userData.role = roleLookUpTable($scope.userData.role);
            $scope.userData.gender = genderLookUpTable($scope.userData.gender);
        }, function errorCallback(response) {
            console.error('Błąd pobierania danych użytkownika...');
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