app.controller("UserController", function ($scope, $http, $route, httpService) {

    $scope.get = function () {
        httpService.userGet()
            .then(function (response) {
                $scope.userData = getFullNamesOfUserAttributes(response.data);
                $scope.userGender = $scope.userData.gender;
            })
    };
    


    let getFullNamesOfUserAttributes = function (user) {
        let expandedUser = user;
        expandedUser.genderLut = genderLookUpTable(user.gender);
        expandedUser.roleLut = roleLookUpTable(user.role);
        expandedUser.birthdate = formatDate(user.birthdate, false);
        expandedUser.lastValidLogin = formatDate(user.lastValidLogin, true);
        expandedUser.lastInvalidLogin = formatDate(user.lastInvalidLogin, true);
        return expandedUser;
    };

    let formatDate = function (rawDate, addTime) {
        let date = new Date(rawDate);

        const monthNames = [
            "Styczeń", "Luty", "Marzec",
            "Kwiecień", "Maj", "Czerwiec", "Lipiec",
            "Sierpień", "Wrzesień", "Październik",
            "Listopad", "Grudzień"
        ];

        let day = date.getDate();
        let monthIndex = date.getMonth();
        let year = date.getFullYear();
        let hour = date.getHours();
        let minute = date.getMinutes();

        let dateString = day + ' ' + monthNames[monthIndex] + ' ' + year;

        if (addTime)
            dateString = dateString.concat(' ').concat(hour).concat(':').concat(minute);

        return dateString;
    };

    let roleLookUpTable = function (roleId) {
        const roles = ['Admin', 'Manager', 'Viewer', 'Sensor'];
        return roles[roleId];
    };

    $scope.availableGender = [{ id: 0, name: 'Mężczyzna' }, { id: 1, name: 'Kobieta' }];

    let genderLookUpTable = function (genderId) {
        return $scope.availableGender.filter(g => g.id === genderId)[0].name;
    };

});