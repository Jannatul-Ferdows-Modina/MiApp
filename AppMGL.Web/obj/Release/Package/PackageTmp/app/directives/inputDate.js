"use strict";

define(["app"], function (app) {

    var injectParams = ["$timeout"];

    var directive = function ($timeout) {

        return {
            restrict: "A",
            require: '?ngModel',
            scope: {
                inputDate: "=",
                showTime: "="
            },
            link: function ($scope, element, attributes, model) {

                if (!model) return;

                var options = {
                    autoclose: true,
                    format: "mm/dd/yyyy",
                    minView: 2,
                    showMeridian: true
                };

                //var utcFormat = "YYYY-MM-DDTHH:mm:ss";

                if ($scope.inputDate && $scope.showTime) {
                    options.format = "mm/dd/yyyy HH:ii P";
                    options.minView = 0;
                }
                else if (!$scope.inputDate && $scope.showTime) {
                    options.format = "HH:ii P";
                    options.startView = 1;
                    options.maxView = 1;
                    options.minView = 0;
                    //utcFormat = "HH:mm:ss";
                }

                //element.on("dp.change", function () {
                //    $timeout(function () {
                //        var dtp = element.data("DateTimePicker");
                //        model.$setViewValue(dtp.date().format(utcFormat));
                //    });
                //});

                model.$render = function () {
                    if (!!model && !!model.$viewValue) {
                        element.datetimepicker("update", model.$viewValue);
                        //if (!$scope.inputDate && $scope.showTime) {
                        //    element.data("DateTimePicker").date(model.$viewValue);
                        //}
                        //else {
                        //    if (Utility.isSafari || Utility.isChromeMobile || Utility.isChromeApple) {
                        //        var d = new Date(model.$viewValue + "Z");
                        //        d.setHours(d.getHours() + 5);
                        //        element.data("DateTimePicker").date(d);
                        //    }
                        //    else if (Utility.isChrome) {
                        //        element.data("DateTimePicker").date(new Date(model.$viewValue.replace("T", " ")));
                        //    }
                        //    else {
                        //        element.data("DateTimePicker").date(new Date(model.$viewValue));
                        //    }
                        //}
                    }
                };

                element.datetimepicker(options);

                element.on("blur", function () {
                    if (!Utility.isValidDate(model.$viewValue)) {
                        element.datetimepicker("reset");
                    }
                    //var dtp = element.data("DateTimePicker");
                    //if (dtp.getMoment(model.$viewValue, options.format, true)) {
                    //    model.$setViewValue(dtp.date().format(utcFormat));
                    //}
                })
            }
        };
    };

    directive.$inject = injectParams;

    app.directive("inputDate", directive);

});
