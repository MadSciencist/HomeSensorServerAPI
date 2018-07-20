app.controller("UsersController", function ($scope, $rootScope, httpService) {

    const baseUsersUrl = "/api/users/";

    $scope.userToEdit = null;
    $scope.users = [];
    $scope.isAuthorizedToViewAllUsers = false;
    $scope.isUpdateSuccess = false;
    $scope.isUpdateFailed = false;
    $scope.resultMessage = "";
    $scope.isDeleteSuccess = false;
    $scope.isDeleteFailed = false;

    $scope.getUsers = function () {
        if (localStorage.getItem('role') === "Admin") {
            httpService.getData(baseUsersUrl)
                .then(function (response) {
                    $scope.isAuthorizedToViewAllUsers = true;
                    $scope.users = response.data;
                }).catch(error => {
                    $scope.isAuthorizedToViewAllUsers = false;
                    console.log("Error while retrieving data: " + error)
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
        const url = baseUsersUrl + $scope.userToEdit.id;
        httpService.putData(url, JSON.stringify($scope.userToEdit))
            .then(function (response) {
                $scope.isUpdateSuccess = true;
                $scope.isUpdateFailed = false;
                $scope.resultMessage = "Udało się! Użytkownik " +
                    $scope.userToEdit.name + " " + $scope.userToEdit.lastname
                    + " ma teraz rolę: " + $scope.getUserRoleFromDictionary($scope.userToEdit.role);
                $scope.getUsers();
            }).catch(error => {
                $scope.isUpdateFailed = true;
                $scope.isUpdateSuccess = false;
                $scope.resultMessage = "Wystąpił błąd :("
                console.log("Error while retrieving data: ", error);
            });
    };

    $scope.deleteUser = function (id) {
        const url = baseUsersUrl + id;
        httpService.deleteData(url)
            .then(function (response) {
                $scope.isDeleteSuccess = true;
                $scope.isDeleteFailed = false;
                $scope.resultMessage = "Udało się! Użytkownik " +
                    $scope.userToEdit.name + " " + $scope.userToEdit.lastname
                    + " został usunięty."
                $scope.getUsers();
            }).catch(error => {
                $scope.isDeleteSuccess = false;
                $scope.isDeleteFailed = true;
                $scope.resultMessage = "Nie udało się usunąć użytkowika."
                console.log("Error while retrieving data: " + error)
            });
    };

});