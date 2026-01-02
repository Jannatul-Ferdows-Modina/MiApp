// <reference path="controller.js" />
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "quotationApprovalService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.quotationApproval;
        $scope.tabs = appUrl.quotationApproval.tabs;
        $scope.$parent.pageTitle = "Pending Shipper Approval";
        $scope.$parent.breadcrumbs = ["Shipment", "Quotation", "Pending Shipper Approval"];
        $scope.carrierList = [];
        $scope.carrierChargesList = [];
        $scope.carrierRemarksList = [];
        $scope.carrierRates = [];
        $scope.selectedCarrierRates = [];
        $scope.selectedCarrierID = 0;
        $scope.carrierRowIndix = 0;
        $scope.gridAction = '';
        $scope.selectedTransitTime = '';
        $scope.selectedFrequency = '';
        $scope.selectedRemarks = '';
        
        

        $scope.lookups = { currencies: [], carriers: [], remarks: [], enquiryContainers: [], carrierAllCharges: [] };

        $scope.initDropdown = function () {

        };
              

        $scope.searchValues = function (viewValue, selectOption) {
            var resultItem = {};
            if (selectOption == "customer") {
                return $scope.callTypeahead(viewValue, 'SIPLContact', 'companyName', null).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.forEach(function (o) {
                            resultItem = {}
                            resultItem.name = o.companyName;
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );
            }
        };
        //#endregion       

        $scope.searchOptions = [
                { optionValue: "", optionName: "-All-" },
                { optionValue: "customer", optionName: "Customer" },
                { optionValue: "quotationNo", optionName: "Quotation No" },
                { optionValue: "enquiryNo", optionName: "Enquiry No" }
        ];

        $scope.filterQuotations = function () {
            var dashboardOption = localStorageService.get("dashboardOption");
            $scope.selectOption = "companyName";
            $scope.searchBox = "";
            if (dashboardOption != null) {
                if (dashboardOption == 'QSAU') {
                    $scope.dashboardOption = "QSAU";
                }                
                if (dashboardOption == 'QSAF') {
                    $scope.dashboardOption = "";
                }

                localStorageService.remove("dashboardOption");
            }
            else {
                $scope.dashboardOption = "";
            }

        };

        $scope.filterQuotations();

        $scope.selectOption = "companyName";
        $scope.searchBox = "";

        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox,
            isApproved: false,
            isBooked: false,
            dashboardOption: $scope.dashboardOption
        };

        

        $scope.quotationlistTable = new NgTableParams(
       {
           page: 1,
           count: 10,
           sorting: $.parseJSON("{ \"" + $scope.page.sortField + "\": \"" + $scope.page.sortType + "\" }")
       }, {
           getData: function (params) {
               var listParams = {
                   SiteId: $scope.$parent.selectedSiteId,
                   ModuleId: $scope.page.moduleId,
                   PageIndex: params.page(),
                   PageSize: params.count(),
                   Sort: JSON.stringify(params.sorting()),
                   Filter: JSON.stringify($scope.searchParam)
               };

               var dataitems = entityService.getApprovalList(listParams).then(
                   function (output) {
                       $scope.validateUser(output);
                       $scope.items = output.data.data;
                       params.total(output.data.count);                                            
                   },
                   function (output) {
                       ngNotifier.showError($scope.authentication, output);
                   }
               );
           }
       });
        $scope.performQuotationSearch = function (source, selectOption, searchBox) {

            var action = source.currentTarget.attributes["action"].value;
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox,                
                isApproved: false,
                isBooked: false,
                dashboardOption: ""
            };
            $scope.quotationlistTable.reload();
        };

        $scope.performQuotationAction = function (source, fromList, enquiryID, quotationID) {

            var action = source.currentTarget.attributes["action"].value;
            
            if (action == "save" && $scope.validateAction != undefined) {
                if (!$scope.validateAction(source)) {
                    return;
                }
            }

            if (fromList) {

                $scope.showQuotationDetail(action, enquiryID, quotationID);

            } else {
                initControls(action);
            }

            //switchTab("Detail", action);

            switch (action) {
                case "search":
                    filterList();
                    break;
                case "add":
                    lastAction = action;
                    $scope.entityId = 0;
                    $scope.entity = {};
                    $("input[input-date]").each(function (index, element) { $(element).val(null); });
                    break;
                case "copy":
                    //lastAction = action;
                    //$scope.entity.enquiryID = 0;                    
                    break;
                    //lastAction = 'copy';
                    //$scope.entityId = 0;
                    //$scope.entity = {};
                    //$("input[input-date]").each(function (index, element) { $(element).val(null); });
                    break;
                case "edit":
                    //lastAction = action;
                    break;
                case "save":
                    save(action);
                    break;
                case "saveEmail":
                    $scope.entity.isSendEmailNow = true;
                    save(action);
                    break;
                case "cancel":
                    $scope.showQuotationDetail(action, enquiryID, quotationID);
                    //$scope.showEnquiryDetail('viewDetail', $scope.entity.enquiryID, $scope.entity.isComplete);
                    //lastAction = "";
                    break;
                case "delete":
                    remove();
                    //lastAction = "";
                    break;
                case "deleteBatch":
                    removeBatch(enquiryID, quotationID);
                    //lastAction = "";
                    break;
                case "verify":
                case "activate":
                case "deactivate":
                    $scope.changeStatus(action);
                    lastAction = "";
                    break;
                default:
                    //lastAction = "";
                    break;
            }

            if ($scope.afterPerformAction != undefined) {
                $scope.afterPerformAction(source, fromList);
            }
        };

        var viewDetail = function () {
            $scope.viewList = false;
            $scope.page.urls.container = "app/views/shared/container.html";
            $scope.entity = {};
        };
        var initControls = function (action) {

            switch (action) {
                case "add":
                    $scope.editMode = true;
                    $scope.disabledInsert = false;
                    $scope.disabledUpdate = false;
                    $scope.requiredInsert = true;
                    $scope.requiredUpdate = true;
                    break;
                case "copy":
                    $scope.editMode = true;
                    $scope.disabledInsert = false;
                    $scope.disabledUpdate = false;
                    $scope.requiredInsert = true;
                    $scope.requiredUpdate = true;
                    break;
                case "edit":
                    $scope.editMode = true;
                    $scope.disabledInsert = true;
                    $scope.disabledUpdate = false;
                    $scope.requiredInsert = false;
                    $scope.requiredUpdate = true;
                    break;
                default:
                    $scope.editMode = false;
                    $scope.disabledInsert = true;
                    $scope.disabledUpdate = true;
                    $scope.requiredInsert = false;
                    $scope.requiredUpdate = false;
                    break;
            }
        };

        $scope.afterPerformAction = function (source, fromList) {
            var action = source.currentTarget.attributes["action"].value;
            switch (action) {
                case "add":

                    break;
                case "edit":
                    //$scope.getcarrierAllRates($scope.entity.enquiryID);


                    break;

            }
        };

        var removeBatch = function (enquiryID, quotationID) {
            var entity = {};
            entity.enquiryID = enquiryID;
            entity.quotationID = quotationID;

            ngNotifier.confirm("Are you sure you want to DELETE Quotation?", null, function () {
                var modalInstance = $uibModal.open({
                    animation: false,
                    backdrop: "static",
                    keyboard: false,
                    size: "lg",
                    templateUrl: "app/components/operation/quotationApproval/cancelRemarks.html",
                    controller: function ($scope, $timeout, $uibModalInstance, requestData) {

                        $scope.select = function (action) {
                            //$scope.deleteRemarks = deleteRemarks.value;
                            if (action == 'delete' && $scope.deleteRemarks == null) {
                                ngNotifier.error("Please Enter Delete Remarks");
                                return;
                            }
                            var outputData = {}
                            outputData.remarks = $scope.deleteRemarks;
                            outputData.action = action;
                            $uibModalInstance.close(outputData);
                        };
                    },
                    resolve: {
                        requestData: function () {
                            return {
                                deleteRemarks: $scope.deleteRemarks
                            };
                        }
                    }
                });

                modalInstance.result.then(
                    function (output) {
                        if (output.action == "delete") {
                            entity.remarks = output.remarks;
                            entityService.deleteQuotation(entity).then(
                            function (output) {
                                $scope.entity = {};
                                $scope.quotationlistTable.reload();
                                $scope.goBack();
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });

                        }
                        else if (output == "close") {

                        }
                    },
                    function (output) {
                        ngNotifier.logError(output);
                    });

            });

        };

        $scope.showEnquiryDetail = function (action, id, isComplete) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                windowClass: "modal-lg-custom",
                templateUrl: "app/components/operation/quotationApproval/enquirydetail.html",
                controller: "enquiryModelController",
                resolve: {
                    requestData: function () {
                        return {
                            enquiryId: id
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    //if(output.action == "close")
                    //{

                    //}
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.showQuotationDetail = function (action, enquiryID, quotationID) {

            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.entity.enquiryID = enquiryID;
            $scope.entity.quotationID = quotationID;
            $scope.quotationID = quotationID;
            //$scope.entity.isComplete = parseInt(isComplete);
            //$scope.entity.siteId = $scope.$parent.selectedSiteId;
            //$scope.entity.userID = $scope.$parent.authentication.userId;
            if ($scope.entity.enquiryID > 0 || $scope.entity.quotationID > 0) {
                entityService.getQuotaionDetail($scope.entity).then(
                     function (output) {
                         if (output.data.resultId == 2005) {
                             ngNotifier.showError($scope.authentication, output);
                             $scope.logOut()
                         }
                         $scope.entity = output.data.data;                        

                         //$scope.getAllCarriers($scope.entity.quotationID)
                         $scope.carrierList = [];
                         if ($scope.entity.carrierDTOList != null) {
                             if ($scope.entity.carrierDTOList.length > 0) {
                                 $scope.carrierList = $scope.entity.carrierDTOList;
                                 $scope.carrierList.forEach(function (item) {
                                     if (item.approvedForBooking == true)  { item.selected = true;}
                                     else { item.selected = false; }
                                 });
                             }
                         }
                         $scope.carrierChargesList = [];
                         $scope.carrierRemarksList = []
                         //$scope.afterGetDetail(action);
                     },
                     function (output) {
                         ngNotifier.showError($scope.authentication, output);
                     }
                 );

            }
            else {
                $scope.goBack();
            }
        };

        $scope.approveCarrier = function (source, fromList) {
            var entities = [];
            var count = 0;
            if (($scope.entity.custReqCutOffDate == null || $scope.entity.custReqCutOffDate == '') && ($scope.entity.custReqSailingDate == null || $scope.entity.custReqSailingDate == '')) {
                ngNotifier.error("Please enter Cust Requested Cut Off Date / Sailing Date");
                return;
            }

            $scope.carrierList.forEach(function (item) {
                if (item.selected == true)
                {
                    item.approvedForBooking = true;
                }
                else { item.approvedForBooking = false; }
                item.custReqCutOffDate = $scope.entity.custReqCutOffDate;
                item.custReqSailingDate = $scope.entity.custReqSailingDate;
                entities.push(item);
            });
            
            if (entities.length === 0) {
                ngNotifier.error("Please, select atleast one record to perform action.");
                return;
            }
            else {
                ngNotifier.confirm("Are you sure do you want to approve Carrier?", null, function () {
                    entityService.approveQuotationCarrier(entities).then(
                            function (output) {
                                $scope.entity = {};
                                $scope.quotationlistTable.reload();
                                $scope.goBack();
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });

                });
            }
        }

        $scope.approveQuotation = function (source, fromList) {
            var entities = [];
            $scope.items.forEach(function (item) {
                if (item.selected) {
                    item.isDraft = false
                    item.isApproved = true
                    item.isBooked = false
                    entities.push(item);
                }
            });
            if (entities.length === 0) {
                ngNotifier.error("Please, select atleast one record to perform action.");
            }
            else {
                ngNotifier.confirm("Are you sure do you want to send Quotaion for Booking?", null, function () {
                    entityService.approveQuotation(entities).then(
                            function (output) {
                                $scope.entity = {};                               
                                $scope.goBack();
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });

                    $scope.$parent.pageTitle = "Sent for Booking";
                    $scope.$parent.breadcrumbs = ["Shipment - Quotation - Sent for Booking"];
                    $location.path("/operation/quotationBooking");
                });
            }
        }

        $scope.disapproveQuotation = function (source, fromList) {
            var entities = [];
            var item = {};
            item.quotationID  = $scope.entity.quotationID;
            item.isDraft = true
            item.isApproved = false
            item.isBooked = false
            entities.push(item);
                
           
            if (entities.length === 0) {
                ngNotifier.error("Please, select atleast one record to perform action.");
            }
            else {
                ngNotifier.confirm("Are you sure do you want to Disapprove Quotaion?", null, function () {
                    entityService.approveQuotation(entities).then(
                            function (output) {
                                $scope.entity = {};
                                $scope.quotationlistTable.reload();
                                $scope.goBack();
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });

                    //$scope.$parent.pageTitle = "Sent for Booking";
                    //$scope.$parent.breadcrumbs = ["Shipment - Quotation - Sent for Booking"];
                    //$location.path("/operation/quotationBooking");
                });
            }
        }

        $scope.showRemarksModel = function (rownum, quotationCarrierId, carrierID, carrierName) {
            $scope.carrierRowIndix = rownum;
            $scope.selectedCarrierId = carrierID;
            $scope.selectedCarrierName = carrierName;
            if ($scope.carrierList[rownum].carrierRemarksDTOList != null)
            {
                if($scope.carrierList[rownum].carrierRemarksDTOList.length > 0)
                {
                    $scope.lookups.remarks = $scope.carrierList[rownum].carrierRemarksDTOList;
                    $scope.lookups.remarks.forEach(function (o) {                        
                        o.isSelected = true;
                    });
                }
            }
           
            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/operation/quotationApproval/addRemarks.html",
                controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                    $scope.portOriginName = requestData.portOriginName;
                    $scope.portDischargeName = requestData.portDischargeName;
                    $scope.carrierName = requestData.carrierName;
                    $scope.carrierID = requestData.carrierID,
                    $scope.carrierRemarks = requestData.carrierRemarks
                    $scope.addCarrierRemarks = function (action) {
                        var outputData = {}
                        if (action == 'update') {
                            
                        }
                        else {
                            outputData.action = 'close';
                        }
                        $uibModalInstance.close(outputData);
                    };
                },
                resolve: {
                    requestData: function () {
                        return {
                            portOriginName: $scope.entity.portOriginName,
                            portDischargeName: $scope.entity.portDischargeName,
                            carrierName: $scope.selectedCarrierName,
                            carrierID: $scope.selectedCarrierId,
                            carrierRemarks: $scope.lookups.remarks
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.action == "update") {
                        //output.selectedRemarksList.forEach(function (item) {
                        //    $scope.carrierRemarksList.push(item);
                        //});      
                        $scope.carrierList[$scope.carrierRowIndix].carrierRemarksDTOList = [];
                        $scope.carrierList[$scope.carrierRowIndix].carrierRemarksDTOList = output.selectedRemarksList;
                    }
                    else if (output == "close") {

                    }
                    $scope.gridAction = '';
                    $scope.carrierRowIndix = 0;
                    $scope.selectedCarrierName = '';
                    $scope.selectedCarrierId = 0;
                    $scope.selectedTransitTime = '';
                    $scope.selectedFrequency = '';
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.showCarrierModel = function (action) {
            var isCarrierExists = false;
            if (action == 'add') {
                $scope.gridAction = 'add';
                //$scope.carrierAllRates.forEach(function (o) {
                //    o.buyingRate = 0;
                //    o.sellingRate = 0;
                //    o.isSelected = false;
                //});
            }
            if (!isCarrierExists) {
                var modalInstance = $uibModal.open({
                    animation: false,
                    backdrop: "static",
                    keyboard: false,
                    size: "lg",
                    templateUrl: "app/components/operation/quotationApproval/addCarrier.html",
                    controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                        $scope.portOriginName = requestData.portOriginName;
                        $scope.portDischargeName = requestData.portDischargeName;
                        $scope.carrierName = requestData.carrierName;
                        $scope.carrierID = requestData.carrierID,
                        $scope.transitTime = requestData.transitTime;
                        $scope.frequency = requestData.frequency;
                        $scope.remarks = requestData.remarks;
                        $scope.validTillDate = requestData.validTillDate,
                        $scope.carriageCurrency = requestData.carriageCurrency,
                        $scope.carrierRates = requestData.carrierRates,
                        $scope.carrierRowIndix = requestData.carrierRowIndix
                        
                        $scope.calculateTotal = function () {
                            var entities = [];
                            var buyingTotal = 0;
                            var sellingTotal = 0;
                            $scope.carrierRates.forEach(function (item) {
                                if (item.isSelected && item.refName != 'Total') {
                                    if (item.buyingRate == '') { item.buyingRate = 0; }
                                    if (item.sellingRate == '') { item.sellingRate = 0; }
                                    buyingTotal = parseFloat(buyingTotal) + parseFloat(item.buyingRate);
                                    sellingTotal = parseFloat(sellingTotal) + parseFloat(item.sellingRate);
                                    //entities.push(item);
                                }
                                if (item.refName == 'Total') {
                                    item.buyingRate = buyingTotal;
                                    item.sellingRate = sellingTotal;
                                    buyingTotal = 0;
                                    sellingTotal = 0;
                                }

                            });
                        };
                        $scope.addCarrierCharges = function (action) {
                            if (action == 'update') {

                            }
                            else {
                                //outputData.action = 'close';
                            }
                            $uibModalInstance.close();
                        };
                        $scope.addCarrierRow = function () {

                        };
                    },
                    resolve: {
                        requestData: function () {
                            return {
                                portOriginName: $scope.entity.portOriginName,
                                portDischargeName: $scope.entity.portDischargeName,
                                carrierName: $scope.selectedCarrierName,
                                carrierID: $scope.selectedCarrierId,
                                transitTime: $scope.selectedTransitTime,
                                frequency: $scope.selectedFrequency,
                                remarks: $scope.selectedRemarks,
                                validTillDate: $scope.entity.validTill,
                                carriageCurrency: $scope.entity.carriageCurrency,
                                carrierRates: $scope.selectedCarrierRates,
                                carrierRowIndix: $scope.carrierRowIndix
                            };
                        }
                    }
                });
            }            
        };

        $scope.showCarrierChargesModel = function (rownum, quotationCarrierId, carrierID, carrierName, transitTime, frequency, remarks) {
            //var action = source.currentTarget.attributes["action"].value;
            $scope.carrierRowIndix = rownum;
            $scope.selectedCarrierName = carrierName;
            $scope.selectedCarrierId = carrierID;
            $scope.selectedTransitTime = transitTime;
            $scope.selectedFrequency = frequency;
            $scope.selectedRemarks = remarks;
            $scope.gridAction = 'edit'
            //if (quotationcarrierID == 0)
            //{
            $scope.selectedCarrierRates = [];
            $scope.selectedCarrierRates = $scope.carrierList[rownum].carrierChargesDTOList;
            //}
            //else {
            //    $scope.getSelectedCarrierRates(carrierId);
            //}
            
            $scope.showCarrierModel('edit');
           
        }
        $scope.selectCarrier = function (selectedCarrierId) {
            //var selectedAll = !$scope.getSelectedAll();
            $scope.carrierList.forEach(function (item) {
                item.selected = false;
            });

            $scope.carrierList.forEach(function (item) {
                if(item.carrierID == selectedCarrierId)
                {
                    item.selected = true;
                }
            });
        };
        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("quotationApprovalController", controller);

});
