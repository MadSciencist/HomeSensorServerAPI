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

    this.putData = function (url, data) {
        console.log('Updating data to: ' + url)
        console.log('Data:');
        console.log(data);
        $http.put(url, data, {
            headers: {
                'Authorization': 'Bearer '.concat(localStorage.getItem('token')),
                'Accept': 'application-json'
            },
            'Accept': 'application/json'
        });
    };
});