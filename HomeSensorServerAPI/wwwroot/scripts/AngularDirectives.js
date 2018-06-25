app.directive('ipaddress', function () {
    return {
        require: 'ngModel',
        link: function (scope, elem, attrs, ctrl) {
            ctrl.$validators.ipaddress = function (modelValue, viewValue) {
                if (ctrl.$isEmpty(modelValue)) {
                    return false;
                }
                var matcher;
                if ((matcher = viewValue.match(/^([0-9]{1,3})\.([0-9]{1,3})\.([0-9]{1,3})\.([0-9]{1,3})$/)) !== null) {
                    var i;
                    var previous = "255";
                    for (i = 1; i < 5; i++) {
                        var octet = parseInt(matcher[i]);
                        if (octet > 255) return false;
                    }
                    return true;
                }
                else {
                    return false;
                }
            };
        }
    };
});

app.factory('userService', function ($http, $rootScope) {
    return {
        getUserData: function () {
            const token = localStorage.getItem('token');
            const userId = localStorage.getItem('userId');
            console.log('SERVICE: Getting data of user of id= '.concat(userId).concat('...'));
            $http({
                method: 'GET',
                url: '/api/users/'.concat(userId),
                headers: {
                    'Content-Type': 'application-json; charset=UTF-8',
                    'Authorization': 'Bearer '.concat(token)
                }
            }).then(function successCallback(response) {
                $rootScope.userData = response.data;
                $rootScope.userData.birthdate = formatDate($rootScope.userData.birthdate, false);
                $rootScope.userData.lastValidLogin = formatDate($rootScope.userData.lastValidLogin, true);
                $rootScope.userData.lastInvalidLogin = formatDate($rootScope.userData.lastInvalidLogin, true);
                $rootScope.userData.role = roleLookUpTable($rootScope.userData.role);
                $rootScope.userData.gender = genderLookUpTable($rootScope.userData.gender);
            }, function errorCallback(response) {
                console.error(response);
            });
        }
    };
});

let formatDate = function (rawDate, addTime) {
    let date = new Date(rawDate);

    var monthNames = [
        "January", "February", "March",
        "April", "May", "June", "July",
        "August", "September", "October",
        "November", "December"
    ];

    var day = date.getDate();
    var monthIndex = date.getMonth();
    var year = date.getFullYear();
    var hour = date.getHours();
    var minute = date.getMinutes();

    let dateString = day + ' ' + monthNames[monthIndex] + ' ' + year;

    if (addTime)
        dateString = dateString.concat(' ').concat(hour).concat(':').concat(minute);

    return dateString;
};

let roleLookUpTable = function (roleId) {
    const roles = ['Admin', 'Manager', 'Viewer', 'Sensor'];
    return roles[roleId];
};

let genderLookUpTable = function (genderId) {
    const genders = ['Męzczyzna', 'Kobieta'];
    return genders[genderId];
};