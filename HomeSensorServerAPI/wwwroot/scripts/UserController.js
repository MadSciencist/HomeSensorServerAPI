app.controller("UserController", function ($scope, $http, $location, $route, httpService) {

    const userUrl = '/api/users/'.concat(localStorage.getItem('userId'));
    $scope.isAvatarUploaded = false;
    $scope.uploadedAvatarUrl = null;

    $scope.editUser = function () {
        const token = localStorage.getItem('token');
        const userId = localStorage.getItem('userId');

        let editedUser = JSON.stringify({
            Login: $scope.userData.login,
            Name: $scope.userData.name,
            Lastname: $scope.userData.lastname,
            Password: $scope.userData.password,
            Email: $scope.userData.email,
            Gender: $scope.userGender,
            Birthdate: $scope.userData.birthdate,
            PhotoUrl: $scope.uploadedAvatarUrl
        });

        $http.put('/api/users/'.concat(userId), editedUser, {
            headers: {
                'Content-Type': 'application/json; charset=UTF-8',
                'Authorization': 'Bearer '.concat(token)
            },
            'Accept': 'application/json'
        }).then(function onSuccess(response) {
            $location.path('/my-profile');
            $scope.get();

        }, function onError(error) {
            console.log(error);
        });
    };

    $scope.uploadFile = function () {
        $.ajax({
            url: 'api/photoupload/upload',
            type: 'POST',

            data: new FormData($('#avatarUploadForm')[0]),
            cache: false,
            contentType: false,
            processData: false,

        }).then(function (response) {
            const splitedUrl = response.url.split("\\")[8];
            const avatarFullUrl = getAvatarFullUrl(splitedUrl);

            $scope.uploadedAvatarUrl = avatarFullUrl;
            $scope.isAvatarUploaded = true;
            $scope.$apply();
        }).catch(function (error) {
            console.log("Error while writing data: " + error);
        });
    };

    let getAvatarFullUrl = function (name) {
        return '/img/uploads/avatars/' + name;
    };

    $scope.get = function () {
        httpService.getData(userUrl)
            .then(function (response) {
                $scope.userData = getFullNamesOfUserAttributes(response.data);
                $scope.userGender = $scope.userData.gender;
            }).catch(error => console.log("Error while retrieving data: " + error.data));
    };
    
    let getFullNamesOfUserAttributes = function (user) {
        let expandedUser = user;
        expandedUser.genderLut = genderLookUpTable(user.gender);
        expandedUser.roleLut = roleLookUpTable(user.role);
        expandedUser.birthdateFormated = formatDate(user.birthdate, false);
        expandedUser.lastValidLogin = formatDate(user.lastValidLogin, true);
        expandedUser.lastInvalidLogin = formatDate(user.lastInvalidLogin, true);
        return expandedUser;
    };

    $scope.availableGender = [{ id: 0, name: 'Mężczyzna' }, { id: 1, name: 'Kobieta' }];

    let genderLookUpTable = function (genderId) {
        return $scope.availableGender.filter(g => g.id === genderId)[0].name;
    };

});