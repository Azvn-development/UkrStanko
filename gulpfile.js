/// <binding BeforeBuild='min' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    babel = require("gulp-babel"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    sass = require("gulp-sass");

var paths = {
    webroot: "./wwwroot/"
};

// Регистрируем задачи для конвертации файлов scss в css
gulp.task("index.css:min", function () {
    return gulp.src(['Content/Sass/btn.scss',
        'Content/Sass/site.scss',
        'Content/Sass/normalize.scss',
        'Content/Sass/popUpMessage.scss',
        'Content/Sass/dialogs.scss',
        'Content/Sass/tables.scss',
        'Content/Sass/variables.scss',
        'Content/Sass/waitingWheel.scss',
        'Content/Sass/messages.scss',
        'Content/Sass/panel.scss',
        'Content/Sass/forms.scss',
        'Content/Sass/news.scss',
        'Content/Sass/details.scss',
        'Content/Sass/slider.scss'])
        .pipe(sass())
        .pipe(concat("index.min.css"))
        .pipe(cssmin())
        .pipe(gulp.dest(paths.webroot + '/css'));
});

gulp.task("login.css:min", function () {
    return gulp.src(['Content/Sass/btn.scss',
        'Content/Sass/site.scss',
        'Content/Sass/normalize.scss',
        'Content/Sass/login.scss',
        'Content/Sass/panel.scss',
        'Content/Sass/forms.scss'])
        .pipe(sass())
        .pipe(concat("login.min.css"))
        .pipe(cssmin())
        .pipe(gulp.dest(paths.webroot + '/css'));
});

// Регистрируем задачи для создания бандлов файлов js
gulp.task("index.js:min", function () {
    return gulp.src(['Content/Scripts/menu.js',
        'Content/Scripts/dialog.js',
        'Content/Scripts/dialogAjax.js',
        'Content/Scripts/routing.js',
        'Content/Scripts/notification.js',
        'Content/Scripts/formAutocomplete.js',
        'Content/Scripts/formValidate.js',
        'Content/Scripts/notices.js',
        'Content/Scripts/itemsDownload.js',
        'Content/Scripts/imagesListCreator.js',
        'Content/Scripts/imageCreator.js'])
        .pipe(concat("index.min.js"))
        .pipe(babel({ presets: ['es2015'] }))
        .pipe(uglify())
        .pipe(gulp.dest(paths.webroot + '/js'));
});

gulp.task("min", gulp.series(["index.css:min", "login.css:min", "index.js:min"]))