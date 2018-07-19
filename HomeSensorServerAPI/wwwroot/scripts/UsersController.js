app.controller("UsersController", function ($scope, httpService) {
    const allUsersUrl = "/api/users/";

    $scope.users = [];
    $scope.isAuthorizedToViewAllUsers = false;

    $scope.getUsers = function () {
        if (localStorage.getItem('role') === "Admin") {
            httpService.getData(allUsersUrl)
                .then(function (response) {
                    $scope.isAuthorizedToViewAllUsers = true;
                    $scope.users = response.data;
                    performRoleLookUp(response.data);
                }).catch(error => {
                    $scope.isAuthorizedToViewAllUsers = false;
                    console.log("Error while retrieving data: " + error.data)
                });
        }
    };

    //on click privilledges, update current watched user to PUT
    $scope.editUserRole = function (id) {
        $scope.userToEdit = ($scope.users.filter(u => u.id === id))[0];
    };

    $scope.editUserRoleSubmit = function () {
        const url = '/api/users/' + $scope.userToEdit.id;
        let json = getUserObject();
        console.log(json);
        httpService.putData(url, json);
    }

    $scope.availableRoles = [
        { id: 'Sensor', name: 'Sensor - może dodawać nowe pomiary' },
        { id: 'Viewer', name: 'Viewer - może wyświetlać' },
        { id: 'Manager', name: 'Manager - może edytować' },
        { id: 'Admin', name: 'Administrator' }
    ];

    const performRoleLookUp = function (rawUsers) {
        for (let i = 0; i < rawUsers.length; i++) {
            $scope.users[i].role = roleLookUpTable(rawUsers[i].role);
        }
    };

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