app.service('httpService', function ($http) {
    this.userGet = function () {
        return $http.get('/api/users/'.concat(localStorage.getItem('userId')),
            {
                headers: {
                    'Authorization': 'Bearer '.concat(localStorage.getItem('token')),
                    'Content-Type': 'application-json; charset=UTF-8'
                }
            });
    }
});