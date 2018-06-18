﻿app.controller("chartsController", function ($scope, $http, $window, $document) {

    $scope.sensorsIdentifiers = [];

    $scope.getIdentifiers = function () {
        const token = sessionStorage.getItem('token');
        $http({
            method: 'GET',
            url: '/api/nodes/type/nodesensor',
            headers: {
                'Content-Type': 'application-json; charset=UTF-8',
                'Authorization': 'Bearer '.concat(token)
            }
        }).then(function successCallback(response) {
            for (let i = 0; i < response.data.length; i++) {
                $scope.sensorsIdentifiers.push(response.data[i].identifier);
            }
        }, function errorCallback(response) {
            console.log(response);
        });
    };

    $scope.getSpecifiedSesorData = function (identifier) {
        const token = sessionStorage.getItem('token');
        $http({
            method: 'GET',
            url: '/api/sensors/'.concat(identifier),
            headers: {
                'Content-Type': 'application-json; charset=UTF-8',
                'Authorization': 'Bearer '.concat(token)
            }
        }).then(function successCallback(response) {
            let name = 'canvas-chart-'.concat(identifier);
            $scope.createChart(name, response.data);
        }, function errorCallback(response) {
            console.log(response);
        });
    };

    $scope.createChart = function (container, dataArray) {

        const processedStamps = processTimestamps(dataArray);
        const sensorValues_Y = getValuesArray(dataArray);

        var ctx = document.getElementById(container).getContext('2d');
        var stackedLine = new Chart(ctx, {
            type: 'line',
            data: {
                datasets: [{
                    data: sensorValues_Y
                }],
                labels: processedStamps
            },
            options: {
                responsive: true,
                legend: {
                    display: false
                },
                title: {
                    display: true,
                    text: 'Wykres temperatury z czujnika '.concat(dataArray[0].identifier)
                    //text: 'Wykres temperatury z czujnika '
                },
                scales: {
                    yAxes: [{
                        stacked: true
                    }]
                }
            }
        });
    };

    let getValuesArray = function (sensorAllDataArray) {
        let valuesArray = [];
        for (let i = 0; i < sensorAllDataArray.length; i++) {
            let indexOfStartValue = sensorAllDataArray[i].data.indexOf(':');
            let indexOfEndValue = sensorAllDataArray[i].data.indexOf('*');
            let stringValue = sensorAllDataArray[i].data.slice(indexOfStartValue + 1, indexOfEndValue);
            let numberValue = parseInt(stringValue);
            valuesArray.push(numberValue);
        }
        return valuesArray;
    };

    let processTimestamps = function (d) {
        const days = ['Nd', 'Pon', 'Wt', 'Sr', 'Czw', 'Pt', 'Sob'];
        let stampsArray = [];
        for (i = 0; i < d.length; i++) {
            let date = new Date(Date.parse(d[i].timeStamp));
            let dateFormatted = days[date.getDay()] + ' ' + date.getHours() + ':' + date.getMinutes();
            stampsArray.push(dateFormatted);
        }
        return stampsArray;
    };
});