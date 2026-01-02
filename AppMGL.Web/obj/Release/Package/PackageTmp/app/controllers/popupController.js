"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$uibModal", "$uibModalInstance", "localStorageService", "NgTableParams", "ngNotifier", "authService", "requestData"];

    var controller = function ($scope, $filter, $timeout, $uibModal, $uibModalInstance, localStorageService, NgTableParams, ngNotifier, authService, requestData) {

        //#region Private Methods

        var setParentEntity = function () {

            $scope.parentEntity = {
                userIcon: requestData.userIcon,
                name: requestData.name,
                description: "(" + requestData.description + ")"
            };
        };

        var initPage = function () {
            if (requestData.moduleId == 1000400) {
                var month = (new Date()).getMonth();
                $scope.lastMonth = getMonthName(month, true);
                $scope.currentMonth = getMonthName(month, false);
            }
            else if (requestData.moduleId == 1000500 || requestData.moduleId == 1000600) {
                $scope.weekDuration = $filter("date")(Utility.addDays(new Date(), -21), "MM/dd/yyyy")
                    + " - " + $filter("date")(Utility.addDays(new Date(), -15), "MM/dd/yyyy");
            }
        };

        var getMonthName = function (month, isLast) {

            month = month - 1;

            if (isLast) {
                month = month - 1;
                if (month < 0) {
                    month = month + 12;
                }
            }

            var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

            return monthNames[month];
        };

        //#endregion

        //#region Popup Methods

        $scope.entity = {};
        $scope.data = [];

        $scope.responseData = {
            data: null,
            resultId: 2001
        };

        $scope.performAction = function (source, target) {

            var action = source.currentTarget.attributes["action"].value;

            switch (action) {

                case "close":
                    $uibModalInstance.close($scope.responseData);
                    break;
            }
        };

        $scope.reportTable = new NgTableParams(
        {
            page: 1,
            count: 100000,
            sorting: $.parseJSON("{ \"AccountName\": \"asc\" }"),
            group: {
                salesUsers: "asc"
            }
        }, {
            counts: [],
            getData: function (params) {
                return authService.listDashboard(requestData.moduleName).then(
                    function (output) {
                        params.total(output.data.data.length);
                        return output.data.data;
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            }
        });

        $scope.modalCompleted = function () {

            $(".box-widget .box-comments").slimscroll({
                height: "250px",
                alwaysVisible: true,
                size: "6px"
            }).css("width", "100%");
        };

        (function () {
            setParentEntity();
            initPage();
        })();

        //#endregion

    };

    controller.$inject = injectParams;

    app.register.controller("popupController", controller);

});
