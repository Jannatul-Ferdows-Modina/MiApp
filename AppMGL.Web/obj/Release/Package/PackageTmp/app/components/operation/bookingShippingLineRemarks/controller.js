// <reference path="controller.js" />
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "bookingShippingLineRemarksService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.bookingShippingLineRemarks;
        $scope.tabs = appUrl.bookingShippingLineRemarks.tabs;
       
        $scope.remarksList = [];
        $scope.lookups = { siplDepartments: [] };

        $scope.initDropdown = function () {
            $scope.fetchLookupData("siplContact", 0, "companyName", "siplContact", null);
        };
       
        //#endregion             
        $scope.ckOptions = {
            toolbar: [
                { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline'] },
                { name: 'insert', items: ['Image', 'Link', 'Unlink'] },
                { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
                { name: 'colors', items: ['TextColor', 'BGColor'] },
                { name: 'indent', groups: ['list', 'indent', 'align'], items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] }
            ]
        };

        $scope.deleteShippingRemarks = function (source, fromList) {
            if ($scope.entity.contactID == null) {
                ngNotifier.error("Please select Shipper");
                return;
            }
            else {
                $scope.entities = {};
                $scope.entities.contactId = $scope.entity.contactID;
                ngNotifier.confirm("Are you sure you want to DELETE Shipper Remarks?", null, function () {
                    entityService.deleteShippingRemarks($scope.entities).then(
                                function (output) {
                                    $scope.entity.remarks = "";
                                    $scope.entity.shipperCommonRemarks = "";
                                    $scope.remarksList = [];
                                    ngNotifier.show(output.data);
                                },
                                function (output) {
                                    ngNotifier.showError($scope.authentication, output);
                                });


                });
            }
        };

        $scope.saveShipperRemarks = function (source, fromList) {

            if ($scope.entity.contactID == null) {
                ngNotifier.error("Please select Shipper");
                return;
            }
            if ($scope.remarksList == null || $scope.remarksList == '') {
                ngNotifier.error("Please enter Remarks");
                return;
            }
            if ($scope.entity.contactID != null && $scope.remarksList != null && $scope.remarksList !='')
            {
                $scope.entity.shipperRemarksDTOList = $scope.remarksList;

                $scope.entity.siteId = $scope.$parent.selectedSiteId;
                $scope.entity.createdBy = $scope.$parent.authentication.userId;
                $scope.entity.updatedBy = $scope.$parent.authentication.userId;
                entityService.saveShippingRemarks($scope.entity).then(
                    function (output) {
                        //$scope.documentID = output.data.data;              

                        ngNotifier.show(output.data);
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                        $scope.editMode = false;
                        $scope.disabledInsert = true;
                        $scope.disabledUpdate = true;
                        $scope.requiredInsert = false;
                        $scope.requiredUpdate = false;
                    });
            }
            
        };
        
       
        $scope.addRemarksRow = function () {
            var remarksItem = {};
            if ($scope.entity.remarks != null) {
                remarksItem.remarks = $scope.entity.remarks;
                $scope.remarksList.push(remarksItem);
                $scope.entity.remarks = '';
            }
            else {
                ngNotifier.error("Please enter Remarks");
                return;
            }
        };

        $scope.removeRemarksRow = function (rownum) {
            $scope.remarksList.splice(rownum, 1);
        }

        $scope.editRemarksRow = function (rownum) {
            $scope.entity.remarks = $scope.remarksList[rownum].remarks;
            $scope.remarksList.splice(rownum, 1);
        }

        $scope.getShipperRemarks = function (shipperId) {
            var id = parseInt(shipperId)
            $scope.entity.remarks = "";
            $scope.entity.shipperCommonRemarks = "";
            $scope.remarksList = [];
            var getRemarks = entityService.getShipperRemarks(id).then(
                function (output) {                    
                    if (output.data.data.shipperRemarksDTOList != null) {
                        //$scope.entity.contactID = output.data.data.contactID;
                        $scope.entity.shipperCommonRemarks = output.data.data.shipperCommonRemarks;
                        $scope.remarksList = output.data.data.shipperRemarksDTOList;
                        $scope.entity.remarks = '';
                    }
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };


        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("bookingShippingLineRemarksController", controller);

});
