app.service('httpService', function ($http) {
 
    this.getData = function (url) {
        console.log('Retrieving data from: ' + url);
        return $http.get(url,
            {
                headers: {
                    'Authorization': 'Bearer '.concat(localStorage.getItem('token')),
                    'Content-Type': 'application-json; charset=UTF-8'
                }
            });
    };
});