"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "profileService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.user;
        $scope.tabs = appUrl.user.tabs;
        $scope.profilePhoto=[];

        //#endregion

        //#region Lookup

        $scope.lookups = { siteTypes: [], titles: [], departments: [] };

        $scope.initDropdown = function () {

            $scope.lookups.contactTypes = [
                { cntTypeId: 0, cntTypeName: "Internal" },
                { cntTypeId: 1, cntTypeName: "External" }
            ];

            $scope.fetchLookupData("title", 0, "TtlName", "titles", null);
            $scope.fetchLookupData("department", 0, "DptName", "departments", null);
        };

        //#endregion

        //#region Detail

        $scope.beforeSave = function (action, lastAction) {
            if ($scope.profilePhoto.name != undefined) {
                if ($scope.profilePhoto.size > 5242880) {
                    $scope.hasError = true;
                    ngNotifier.error("File cannot exceeds more than 5 MB size.");
                    return;
                }
                if ($scope.profilePhoto.type != "image/jpeg" && $scope.profilePhoto.type != "image/png" && $scope.profilePhoto.type != "image/gif") {
                    $scope.hasError = true;
                    ngNotifier.error("Please upload only these file types - jpg,png,gif");
                    return;
                }
                $scope.entity.contact.cntImageName = $scope.$parent.userInfo.usrId + '_' + $scope.profilePhoto.name;
            }
            $scope.entity.usrUpdatedTs = new Date();
            $scope.entity.usrUpdatedBy = $scope.$parent.userInfo.usrId;
            $scope.entity.contact.cntUpdatedTs = new Date();
            $scope.entity.contact.cntUpdatedBy = $scope.$parent.userInfo.usrId;
        };

        $scope.afterSave = function (lastAction) {
            if ($scope.profilePhoto.name != undefined) {
                var attachment = {
                    FileName: $scope.$parent.userInfo.usrId + '_' + $scope.profilePhoto.name,
                    UserId: $scope.$parent.userInfo.usrId
                };
                entityService.uploadProfile(attachment, $scope.profilePhoto).then(
                    function (output) {
                        //ngNotifier.show(output.data.output);
                        $scope.$emit("initMainPage", null);                        
                    },
                    function (output) {
                        ngNotifier.error(output.data.output.messages);
                    }
                );
                
            }
        }

        $scope.updateFullName = function () {

            var firstName = ($scope.entity.contact && $scope.entity.contact.cntFirstName != "") ? $scope.entity.contact.cntFirstName : "";
            var lastName = ($scope.entity.contact && $scope.entity.contact.cntLastName != "") ? $scope.entity.contact.cntLastName : "";
            $scope.entity.fullName = firstName + (lastName != "" ? " " + lastName : "");
        };

        $scope.callChangePassword = function () {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "md",
                templateUrl: "app/components/security/changePassword/index.html",
                controller: "changePasswordController",
                resolve: {
                    requestData: function () {
                        return {
                            userId: $scope.$parent.authentication.userId,
                            aspNetUserId: $scope.$parent.userInfo.aspNetUserId
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.data && output.resultId == 1001) {
                        ngNotifier.show(output.data);
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

        (function () {
            $scope.entityId = $scope.$parent.userInfo.usrId;
            $scope.getDetail();
            //if ($scope.initDropdown != undefined) {
            //    $scope.initDropdown();
            //}
            //if ($scope.routeEntityId) {
            //    if ($scope.routeEntityId == 0) {
            //        $scope.$broadcast("show-errors-check-validity");
            //        $scope.showDetail("add", 0);
            //        switchTab("Detail", "add");
            //        lastAction = "add";
            //        $scope.entityId = 0;
            //        $scope.entity = {};
            //        $("input[input-date]").each(function (index, element) { $(element).val(null); });
            //        if ($scope.defaultBehaviorOnAdd != undefined) {
            //            $timeout(function () {
            //                $scope.defaultBehaviorOnAdd();
            //            }, 1000);
            //        }
            //    }
            //    else {
            //        $scope.showDetail('viewDetail', $scope.routeEntityId);
            //    }
            //}
        })();

    };

    controller.$inject = injectParams;

    app.register.controller("profileController", controller);

});
