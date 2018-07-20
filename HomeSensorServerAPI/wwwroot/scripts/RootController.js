app.controller("RootController", function ($rootScope, $http, httpService, $scope, $window, $route, $location) {

    $scope.getDictionaries = function () {
        const rolesDictionaryUrl = '/api/dictionaries/roles';
        const gendersDictionaryUrl = '/api/dictionaries/genders';

        httpService.getData(rolesDictionaryUrl)
            .then(function (response) {
                $rootScope.roleDictionary = response.data;
            }).catch(error => console.log("Error while retrieving data: " + error.data));

        httpService.getData(gendersDictionaryUrl)
            .then(function (response) {
                $rootScope.genderDictionary = response.data;
            }).catch(error => console.log("Error while retrieving data: " + error.data));
    };

    $scope.logout = function () {
        console.log('Log out...');
        $window.localStorage.clear();

        $rootScope.badAuthentication = false;
        $rootScope.badAuthenticationMessage = '';

        $route.reload();
    };


});