app.controller("UsersController", function ($scope, $rootScope, httpService) {

    const allUsersUrl = "/api/users/";

    $scope.users = [];
    $scope.isAuthorizedToViewAllUsers = false;

    $scope.getUsers = function () {
        if (localStorage.getItem('role') === "Admin") {
            httpService.getData(allUsersUrl)
                .then(function (response) {
                    $scope.isAuthorizedToViewAllUsers = true;
                    $scope.users = response.data;
                }).catch(error => {
                    $scope.isAuthorizedToViewAllUsers = false;
                    console.log("Error while retrieving data: " + error.data)
                });
        }
    };


    $scope.getUserRoleFromDictionary = function (roleId) {
        return ($rootScope.roleDictionary.filter(r => r.value === roleId))[0].dictionary;
    };

    //on click privilledges, update current watched user to PUT
    $scope.editUserRole = function (id) {
        $scope.userToEdit = ($scope.users.filter(u => u.id === id))[0];
    };

    $scope.editUserRoleSubmit = function () {
        const url = '/api/users/' + $scope.userToEdit.id;
        let json = JSON.stringify($scope.userToEdit);
        console.log(json);
        httpService.putData(url, json);
    }

    const getUserObject = function () {
        return JSON.stringify({
            id: $scope.userToEdit.id,
            name: $scope.userToEdit.name,
            lastname: $scope.userToEdit.lastname,
            email: $scope.userToEdit.email,
            gender: $scope.userToEdit.gender,
            role: "Sensor"
        });
    };
});