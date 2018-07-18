app.controller("UsersController", function ($scope, httpService) {
    const allUsersUrl = "/api/users/";

    $scope.users = [];

    $scope.getUsers = function () {
        httpService.getData(allUsersUrl)
            .then(function (response) {

                $scope.users = response.data;
                performRoleLookUp(response.data);

            }).catch(error => console.log("Error while retrieving data: " + error.data));
    };

    const performRoleLookUp = function (rawUsers) {
        for (let i = 0; i < rawUsers.length; i++) {
            $scope.users[i].role = roleLookUpTable(rawUsers[i].role);
        }
    };
});