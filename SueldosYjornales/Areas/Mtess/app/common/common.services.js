var app;
(function (app) {
    var common;
    (function (common) {
        angular.module("common.services", ["ngResource", "ui.bootstrap", "checklist-model"]);
    })(common = app.common || (app.common = {}));
})(app || (app = {}));
