﻿const pump = require('pump');
const gulp = require('gulp');
const concat = require('gulp-concat');
const uglify = require('gulp-uglify');
var url = require('url');
const browserSync = require('browser-sync').create();
const proxy = require('proxy-middleware');

let devMode = false;

const stylesDev = [
    "./node_modules/bootstrap/dist/css/bootstrap.css",
    "./node_modules/js-datepicker/datepicker.css",
    "./node_modules/@fortawesome/fontawesome-free/css/all.css",
    "./src/css/**/*.css"
];

const stylesProd = [
    "./node_modules/bootstrap/dist/css/bootstrap.min.css",
    "./node_modules/js-datepicker/datepicker.min.css",
    "./node_modules/@fortawesome/fontawesome-free/css/all.min.css",
    "./src/css/**/*.css"
];

const scriptsDev = [
    "./node_modules/jquery/dist/jquery.js",
    "./node_modules/angular/angular.js",
    "./node_modules/angular-route/angular-route.js",
    "./node_modules/bootstrap/dist/js/bootstrap.js",
    "./node_modules/js-datepicker/datepicker.js",
    "./node_modules/chart.js/dist/Chart.js",
    "./src/js/**/*.js"
];

const scriptsProd = [
    "./node_modules/jquery/dist/jquery.min.js",
    "./node_modules/angular/angular.min.js",
    "./node_modules/angular-route/angular-route.min.js",
    "./node_modules/bootstrap/dist/js/bootstrap.min.js",
    "./node_modules/js-datepicker/datepicker.min.js",
    "./node_modules/chart.js/dist/Chart.min.js",
    "./src/js/**/*.js"
];

const fonts1 = [
    "./node_modules/bootstrap/fonts/glyphicons-halflings-regular.woff2",
    "./node_modules/bootstrap/fonts/glyphicons-halflings-regular.woff"
]

const webFonts1 = [
    escape("./node_modules/@fortawesome/fontawesome-free/webfonts/fa-solid-900.woff2")
]

gulp.task('css', function () {
    gulp.src(devMode ? stylesDev : stylesProd)
        .pipe(concat('bundle.css'))
        .pipe(gulp.dest('./dist/css'))
        .pipe(browserSync.reload({
            stream: true
        }));
});

gulp.task('js', function (cb) {
    if (devMode) {
        pump([
            gulp.src(scriptsDev),
            concat('bundle.js'),
            gulp.dest('./dist/js'),
            browserSync.reload({
                stream: true
            })
        ],
            cb
        );
    }else {
        pump([
            gulp.src(scriptsProd),
            uglify(),
            concat('bundle.min.js'),
            gulp.dest('./dist/js'),
            browserSync.reload({
                stream: true
            })
        ],
            cb
        );
    }

});

gulp.task('html', function () {
    gulp.src('./src/templates/**/*.html')
        .pipe(gulp.dest('./dist/'))
        .pipe(browserSync.reload({
            stream: true
        }));
});

gulp.task('fonts', function () {
    gulp.src(fonts1)
        .pipe(gulp.dest('./dist/fonts/'))
        .pipe(browserSync.reload({
            stream: true
        }));
});

gulp.task('webFonts', function () {
    gulp.src(webFonts1)
        .pipe(gulp.dest('./dist/webfonts/'))
        .pipe(browserSync.reload({
            stream: true
        }));
});

gulp.task('build', function () {
    gulp.start(['css', 'js', 'html', 'fonts', 'webFonts']);
});


gulp.task('browser-sync', function () {
    let proxyOptionsApi = url.parse('http://localhost/api/');
    proxyOptionsApi.route = '/api/';

    let proxyOptionsImg = url.parse('http://localhost/img/');
    proxyOptionsImg.route = '/img/';

    browserSync.init(null, {
        open: false,
        port: 3000,
        server: {
            baseDir: 'dist',
            middleware: [proxy(proxyOptionsApi), proxy(proxyOptionsImg)]
        }
    });
});

gulp.task('startDev', function () {
    devMode = true;
    gulp.start(['build', 'browser-sync']);
    gulp.watch(['./src/css/**/*.css'], ['css']);
    gulp.watch(['./src/js/**/*.js'], ['js']);
    gulp.watch(['./src/templates/**/*.html'], ['html']);
});

gulp.task('buildProd', function () {
    devMode = false;
    gulp.start(['build']);
});