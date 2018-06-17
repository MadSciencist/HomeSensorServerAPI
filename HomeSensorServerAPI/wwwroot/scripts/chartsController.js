app.controller("chartsController", function ($scope, $http, $window) {

    $scope.sensorNames = [];
    const colsPerRow = 2;
    let rowsCount = 0, colsCount = 0;

    $scope.getCharts = function () {
        const token = sessionStorage.getItem('token');
        $.ajax({
            type: 'GET',
            url: '/api/nodes/type/nodesensor',
            headers: {
                'Content-Type': 'application-json; charset=UTF-8',
                'Authorization': 'Bearer '.concat(token)
            },
            success: function (response) {
                for (var i = 0; i < response.length; i++) {
                    $scope.sensorNames.push(response[i].identifier);
                    getSpecifiedSesor(response[i].identifier);

                }
                createRows($scope.sensorNames.length, $scope.sensorNames);
            },
            error: function (err) {
                checkIfNotAuthorized(err);
            }
        });
    };


    let getSpecifiedSesor = function (identifier) {
        const token = sessionStorage.getItem('token');
        $.ajax({
            type: 'GET',
            url: '/api/sensors/'.concat(identifier),
            headers: {
                'Content-Type': 'application-json; charset=UTF-8',
                'Authorization': 'Bearer '.concat(token)
            },
            success: function (response) {
                createChart(identifier, response);
            },
            error: function (err) {
                console.log(error);
                checkIfNotAuthorized(err);
            }
        });
    };


    let createRows = function (elementsToFit, sensorNames) {
        let neededRows = Math.ceil(elementsToFit / 2);
        let sensorNamesForThisRow = [];

        let j = 0;
        for (let i = 1; i <= neededRows; i++){
            if (i % colsPerRow === 0) {
                j = rowsCount * colsPerRow;
                sensorNamesForThisRow = [];
            }

            sensorNamesForThisRow.push(sensorNames[j]);
            sensorNamesForThisRow.push(sensorNames[j+1]);

            createRow(i, sensorNamesForThisRow);
        }
    };

    let createRow = function (rowNum, sensorNamesForThisRow) {
        let row = document.createElement("div");
        row.className = "row";
        row.id = "row".concat("_").concat(rowNum);

        row.appendChild(createSpacerColumn());

        let colLeft = createColumn(row.id, 'left', sensorNamesForThisRow[0]);
        row.appendChild(colLeft);

        let colRight = createColumn(row.id, 'right', sensorNamesForThisRow[1]);
        row.appendChild(colRight);

        row.appendChild(createSpacerColumn());

        document.getElementById("chart-parent-container").appendChild(row);
        rowsCount++;
    };

    let createColumn = function (rowId, columnName, sensorName) {

        let col = document.createElement("div");
        col.className = "col-md-5";
        col.id = rowId.concat("_col_").concat(columnName);

        let canvas = createCanvas(sensorName);
        col.appendChild(canvas);

        colsCount++;
        return col;
    };

    let createSpacerColumn = function () {
        let spacerCol = document.createElement("div");
        spacerCol.className = "col-md-1";
        return spacerCol;
    };

    let createCanvas = function (sensorName) {
        let canvas = document.createElement("canvas");
        canvas.id = "canvas_".concat(sensorName);
        return canvas;
    };


    function createChart(container, dataArray) {
        let _container = "canvas_".concat(container);
        const processedStamps = processTimestamps(dataArray);
        const sensorValues_Y = getValuesArray(dataArray);

        var ctx = document.getElementById(_container).getContext('2d');
        var stackedLine = new Chart(ctx, {
            type: 'line',
            data: {
                datasets: [{
                    data: sensorValues_Y
                }],
                labels: processedStamps,
            },
            options: {
                responsive: true,
                legend: {
                    display: false,
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
    }

    getValuesArray = function (sensorAllDataArray) {
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

    processTimestamps = function (d) {
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