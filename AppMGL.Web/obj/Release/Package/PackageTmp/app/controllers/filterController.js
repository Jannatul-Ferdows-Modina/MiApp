"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$uibModalInstance", "ngNotifier", "requestData"];

    var controller = function ($scope, $timeout, $uibModalInstance, ngNotifier, requestData) {

        //#region Private Method

        var setDefaultOperation = function () {

            $("select[name$='Op']").each(function (index, element) {
                $(element).val($(element).find("option:nth-child(2)").val()).change();
            });
        };

        var loadCriteria = function () {

            requestData.currentCriteria.forEach(function (filter) {

                if (filter.operator == "containsIn") {
                    $scope[filter.name + "F"] = [];
                    filter.value.split(",").forEach(function (item) {
                        $scope[filter.name + "F"].push(item);
                    });
                    var selectFilter = $("select[name='" + filter.name + "F']");
                    selectFilter.multiselect('deselectAll', false);
                    selectFilter.multiselect("select", $scope[filter.name + "F"]);
                    selectFilter.change();
                    selectFilter.multiselect("updateButtonText");
                }
                else {
                    if (filter.type == "datetime") {
                        $scope[filter.name + "F"] = Utility.getDateISO(new Date(filter.value));
                    }
                    else {
                        $scope[filter.name + "F"] = filter.value;
                    }
                }

                $scope[filter.name + "Op"] = filter.operator;

                if (filter.operator == "between") {
                    $scope[filter.name + "T"] = filter.valueT;
                }
            });
        };

        var generateCriteria = function () {

            var _criteria = [];

            requestData.criteria.forEach(function (filter) {

                var fieldValue = $("input[name='" + filter.name + "F']").val();
                var fieldOperator = $("select[name='" + filter.name + "Op']").val();

                if (fieldOperator == "containsIn" && $("select[name='" + filter.name + "F'] option:selected").text() != "") {
                    fieldValue = $("select[name='" + filter.name + "F']").val();
                    fieldValue = (fieldValue) ? fieldValue.join(",") : "";
                }

                if (fieldValue && fieldValue != "") {
                    filter.value = fieldValue;
                    filter.operator = fieldOperator;
                    if (filter.operator == "between") {
                        filter.valueT = $("input[name='" + filter.name + "T']").val();
                    }
                    _criteria.push(filter);
                }
            });

            return _criteria;
        };

        //#endregion

        //#region Filter Method

        $scope.responseData = {
            criteria: [],
            resultId: 2001
        };

        $scope.performAction = function (source) {

            var action = source.currentTarget.attributes["action"].value;

            switch (action) {

                case "ok":
                    $scope.responseData.criteria = generateCriteria();
                    $scope.responseData.resultId = 1001;
                    $uibModalInstance.close($scope.responseData);
                    break;

                case "cancel":
                    $uibModalInstance.close($scope.responseData);
                    break;
            }
        };

        $scope.lookups = {
            statuses: [
                { statusId: 40, statusCaption: "Verified" },
                { statusId: 60, statusCaption: "Deactivated" }
            ],
            accountStatuses: [
                { statusId: 70, statusCaption: "Active" },
                { statusId: 80, statusCaption: "Inactive" },
                { statusId: 90, statusCaption: "Potential" }
            ],
            taskStatuses: [
                { statusId: 10, statusCaption: "Entered" },
                { statusId: 110, statusCaption: "Completed" },
                { statusId: 120, statusCaption: "Accepted" },
                { statusId: 130, statusCaption: "Rejected" }
            ],
            complaintStatuses: [
                { statusId: 210, statusCaption: "Clent Service Processing" },
                { statusId: 100, statusCaption: "Resolved" },
                { statusId: 220, statusCaption: "Sales Rep. Attn." }
            ],
            workItemStatuses: [
                { statusId: 10, statusCaption: "Entered" },
                { statusId: 70, statusCaption: "Active" },
                { statusId: 80, statusCaption: "Inactive" },
                { statusId: 140, statusCaption: "On Hold" },
                { statusId: 110, statusCaption: "Completed" },
                { statusId: 150, statusCaption: "Closed" }
            ]
        };

        $scope.filterModalLoaded = function () {

            setDefaultOperation();
            loadCriteria();
        };

        //#endregion
    };

    controller.$inject = injectParams;

    app.register.controller("filterController", controller);

});
