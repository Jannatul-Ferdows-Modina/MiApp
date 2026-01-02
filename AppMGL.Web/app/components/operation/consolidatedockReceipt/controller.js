"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "consolidatedockReceiptService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        var lastAction = "";
        $scope.page = appUrl.consolidatedockReceipt;
        $scope.tabs = appUrl.consolidatedockReceipt.tabs;
        $scope.$parent.pageTitle = "Dock Receipt";
        $scope.$parent.breadcrumbs = ["Shipment", " Consolidate -Dock Receipt"];
        $scope.isPrintAddress = false;
        $scope.documentStatus = 0;
        $scope.fileNo = '';
        //#endregion

        //#region Private 
        $scope.documentTable = new NgTableParams(
            {
                page: 1,
                count: 10,
                sorting: $.parseJSON("{ \"SerialNo\": \"asc\" }")
            }, {
            counts: [],
            getData: function (params) {
                var listParams = {
                    PageIndex: params.page(),
                    PageSize: params.count(),
                    Sort: JSON.stringify(params.sorting()),
                    SitId: $scope.$parent.selectedSiteId,
                    FileNo:$.trim( $("#txtfileNo").val())
                };
              
                return entityService.getConsolidateDockReceiptList(listParams).then(
                    function (output) {
                        params.total(output.data.count);
                        return output.data.data;
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            }
        });
        $scope.performSearch = function (source, selectOption, searchBox) {
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox
            };
            $scope.documentTable.reload();
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

        var initCommodityDetail = function (index) {
            return {
                coOtherID: index,
                coid: 0,
                number: "",
                package: "",
                commodity: "",
                weight: "",
                measuremant: "",
                type: "",
                wtType: "KILO",
                mtType: "CBM"
            };
        };

        $scope.filterDocReceiptData = function () {
            var dashboardOption = localStorageService.get("dashboardOption");
            
            if (dashboardOption != null) {
                if (dashboardOption == 'TPDRU' || dashboardOption == 'TPDRF') {
                    $scope.documentStatus = 14;
                }               
                localStorageService.remove("dashboardOption");
            }
            else {
                $scope.documentStatus = 0;
            }
        };
        $scope.filterDocReceiptData();

        var getDockReceipt = function () {

            $scope.entityId = id;
            $scope.entity.bookedId = parseFloat(id);
            entityService.getDockReceipt($scope.entity).then(
                function (output) {
                    $scope.document = output.data.data;
                    if ($scope.entity.commodityDetail.length == 0) {
                        $scope.entity.commodityDetail = [initCommodityDetail(0)];
                    }
                    $scope.entity.docCommId = $scope.entity.documentCommonId;
                  //  $scope.detailTable.reload();
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

        var viewDocumentList = function () {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/document/viewDocumentList/index.html",
                controller: "viewDocumentListController",
                resolve: {
                    requestData: function () {
                        return {
                            SitId: $scope.$parent.selectedSiteId,
                            documentStatus:$scope.documentStatus
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.data && output.resultId == 1001) {
                        if (output.data.exportRef != "" && output.docCommId != "") {
                            $scope.entity.filterFieldName = "systemRefNo";
                            $scope.entity.filterFieldValue = output.data.exportRef;                            
                            $scope.entity.docCommId = output.docCommId;
                            getDockReceipt();
                        }
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        var resetFilterField = function () {

            $scope.entity.miamiRefNo = ($scope.entity.filterFieldName == "miamiRefNo") ? $scope.entity.filterFieldValue : "";
            $scope.entity.systemRefNo = ($scope.entity.filterFieldName == "systemRefNo") ? $scope.entity.filterFieldValue : "";
            $scope.entity.bookingNo = ($scope.entity.filterFieldName == "bookingNo") ? $scope.entity.filterFieldValue : "";
        };

        var editDocument = function () {

            initControls("edit");
        };

        $scope.SaveConsolidateDockReceipt = function () {
            $scope.$broadcast("show-errors-check-validity");
            if ($scope.entity.exporterName == null) {
                ngNotifier.error("Please Select Exporter/Cargo Pickup Location .");
                return;
            }
            if ($scope.entity.blNumber == null) {
                ngNotifier.error("Please enter B/L or AWB No.");
                return;
            }
            if ($scope.entity.fileNo == null) {
                ngNotifier.error("Please enter MGL Reference No.");
                return;
            }
            if ($scope.entity.consignedToName == null) {
                ngNotifier.error("Please select Consigned To.");
                return;
            }

            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userId = $scope.$parent.authentication.userId;
            entityService.SaveConsolidateDockReceipt($scope.entity).then(
                function (output) {
                    $scope.dockReceiptId = output.data.data;
                    $scope.entity = {};
                    $scope.documentTable.reload();
                    $scope.editMode = false;
                    $scope.goBack();
                    ngNotifier.show(output.data);
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );         
           // sendEmail();
        };

        var saveDocumentPdf = function () {
            $scope.entity.createdBy = $scope.$parent.userInfo.usrId;
            $scope.entity.modifiedBy = $scope.$parent.userInfo.usrId;
            entityService.saveDockRecPdf($scope.document).then(
                function (output) {
                    ngNotifier.show(output.data);
                    initControls("save");
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
            
        };

        var cancelDocument = function () {

            initControls("cancel");
            getDockReceipt();
        };

        var exportPdf = function () {            
            resetFilterField();
            var reportParams = {
                MiamiRefNo: $scope.entity.miamiRefNo || "",
                SystemRefNo: $scope.entity.systemRefNo || "",
                BookingNo: $scope.entity.bookingNo || "",
                SitId: $scope.$parent.selectedSiteId || "",
                DocCommonId: $scope.entity.docCommId                   
            };
            entityService.exportPdf(reportParams).then(
                function (output) {
                    var blobData = new Blob([output.data], { type: output.headers()["content-type"] });
                    var fileName = output.headers()["x-filename"];
                    saveAs(blobData, fileName);
                },
                function (output) {
                    ngNotifier.error(output);
                }
            );
            saveDocumentPdf();
        };


        var sendEmail = function () {

            $scope.entity.createdBy = $scope.$parent.authentication.userId;
            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "md",
                templateUrl: "app/components/document/sendEmail/index.html",
                controller: function ($scope, $timeout, $uibModalInstance, ngNotifier, requestData) {
                    $scope.entity = {};
                    $scope.entity.emailSubject = "Booking No #" + requestData.entity.systemRefNo;
                    $scope.send = function () {
                        $scope.$broadcast("show-errors-check-validity");
                        if ($scope.form.detail.$error.required != undefined && $scope.form.detail.$error.required.length > 0) {
                            ngNotifier.error("Required Field(s) are missing data.");
                            return;
                        }
                        $uibModalInstance.close({ data: $scope.entity, resultId: 1001 });
                    };
                    $scope.close = function () {
                        $uibModalInstance.close({ data: null, resultId: 1001 });
                    };
                    $scope.getEmailIds = function (viewValue, lookupModule, lookupField1, lookupMethod) {
                        var resultItem = {};
                        return entityService.getEmailIds(viewValue).then(
                                function (output) {
                                    $scope.searchResult = [];
                                    output.data.data.forEach(function (o) {
                                        resultItem = {}
                                        resultItem.email = o.email;
                                        $scope.searchResult.push(resultItem)
                                    });
                                    return $scope.searchResult;
                                }
                            );

                    };
                },
                resolve: {
                    requestData: function () {
                        return {
                            entity: $scope.entity
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.data && output.resultId == 1001) {
                        resetFilterField();
                        var params = {
                            MiamiRefNo: $scope.entity.miamiRefNo || "",
                            SystemRefNo: $scope.entity.systemRefNo || "",
                            BookingNo: $scope.entity.bookingNo || "",
                            SitId: $scope.$parent.selectedSiteId,
                            EmailTo: output.data.emailTo,
                            EmailCc: output.data.emailCc,
                            EmailBcc: output.data.emailBcc,
                            EmailSubject: output.data.emailSubject,
                            EmailBody: output.data.emailBody,
                            CreatedBy: $scope.$parent.userInfo.usrId

                        };
                        entityService.sendEmail(params).then(
                            function (output) {
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            }
                        );
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        //#endregion

        //#region Lookup 

        $scope.lookups = { departmentTypes: [], copyReceiptTypes: [], viewDockReceipts: [], filterFields: [], consignedToTypes: [] };

        $scope.setLookups = function (source, lookup, output, index) {
            if (lookup == "origin") {
                   $scope.entity.transshipment = output.data[0].portId;
            }
            if (lookup == "SIPLContact") {
                $scope.entity.checkedBY = output.data[0].contactID;
                $scope.entity.exporterAddress = output.data[0].address;
                $scope.entity.exporterId = output.data[0].contactID;
            }
          if (lookup == "SIPLContact1") {
                
                $scope.entity.consigneeAddress = output.data[0].address;
                $scope.entity.consigneeId = output.data[0].contactID;
            }
            if (lookup == "SIPLContact2") {
                
                $scope.entity.consignedToAddress = output.data[0].address;
                $scope.entity.consignedToId = output.data[0].contactID;
            }
            if (lookup == "SIPLContact3") {

                $scope.entity.fwdAgentAddress = output.data[0].address;
                $scope.entity.fwdAgentId = output.data[0].contactID;
            }
            if (lookup == "pickupLocation") {

                $scope.entity.emptyPickup = output.data[0].companyName+' '+output.data[0].address;
                $scope.entity.EmptyPickupId = output.data[0].contactID;
            }
            if (lookup == "returnLocation") {

                $scope.entity.fullReturn = output.data[0].companyName + ' ' + output.data[0].address;
                $scope.entity.fullReturnId = output.data[0].contactID;
            }
        };

        $scope.clearLookups = function (source, lookup, index) {
            if (lookup == "origin") {
                $scope.entity.transshipment = null;
            }
            if (lookup == "SIPLContact") {
                $scope.entity.checkedBY = null;
            }
        };

        $scope.updateIsPrintAddress = function (isPrntAddVal) {
            if (isPrntAddVal) {
                $scope.isPrintAddress = true;
                $scope.entity.isPrintAddress = 1;
                //$scope.entity.commodityIds = [];
            }
            else {
                $scope.isPrintAddress = false;
                $scope.entity.isPrintAddress = 2;
                //$scope.entity.commodityIds = [];
            }
        };

        $scope.initDropdown = function () {

            $scope.lookups.departmentTypes = [];
            $scope.lookups.departmentTypes.push({ fieldName: 1, fieldValue: "Ocean" });
            $scope.lookups.departmentTypes.push({ fieldName: 2, fieldValue: "Air" });

            $scope.lookups.copyReceiptTypes = [];
            $scope.lookups.copyReceiptTypes.push({ fieldName: 1, fieldValue: "Transporter" });
            $scope.lookups.copyReceiptTypes.push({ fieldName: 2, fieldValue: "Custom Clearance" });

            $scope.lookups.viewDockReceipts = [];
            $scope.lookups.viewDockReceipts.push({ fieldName: 1, fieldValue: "Normal Flow" });
            $scope.lookups.viewDockReceipts.push({ fieldName: 2, fieldValue: "Exception" });

            $scope.lookups.filterFields = [];
            //$scope.lookups.filterFields.push({ fieldName: "miamiRefNo", fieldValue: "Miami Ref No" });
            $scope.lookups.filterFields.push({ fieldName: "systemRefNo", fieldValue: "System Ref No" });
            $scope.lookups.filterFields.push({ fieldName: "bookingNo", fieldValue: "Booking No" });

            $scope.lookups.consignedToTypes = [];
            $scope.lookups.consignedToTypes.push({ fieldName: 1, fieldValue: "To The Order Of" });
            $scope.lookups.consignedToTypes.push({ fieldName: 2, fieldValue: "To Order" });

            $scope.entity.departmentType = 1;
            $scope.entity.copyReceiptType = 1;
            $scope.entity.viewDockReceipt = 1;
            $scope.entity.filterFieldName = "systemRefNo";

            $scope.entity.weightType = "KILO";
            $scope.entity.measurementType = "CBM";
        };

        $scope.callExporter = function (source) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplCompanyModal/detail.html",
                controller: "siplCompanyModalController",
                resolve: {
                    requestData: function () {
                        return {
                            companyID: ($scope.entity.exporterId || 0)
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.exporterId = output.data.contactID;
                        $scope.entity.exporterAddress = output.data.address;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.callConsignedTo = function (source) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplCompanyModal/detail.html",
                controller: "siplCompanyModalController",
                resolve: {
                    requestData: function () {
                        return {
                            companyID: ($scope.entity.consignedToId || 0)
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.consignedToId = output.data.contactID;
                        $scope.entity.consignedToAddress = output.data.address;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.callForwardingAgent = function (source) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplCompanyModal/detail.html",
                controller: "siplCompanyModalController",
                resolve: {
                    requestData: function () {
                        return {
                            companyID: ($scope.entity.fwdAgentId || 0)
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.fwdAgentId = output.data.contactID;
                        $scope.entity.fwdAgentAddress = output.data.address;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.callConsignee = function (source) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplCompanyModal/detail.html",
                controller: "siplCompanyModalController",
                resolve: {
                    requestData: function () {
                        return {
                            companyID: ($scope.entity.consigneeId || 0)
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.consigneeId = output.data.contactID;
                        $scope.entity.consigneeAddress = output.data.address;
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.callLandingPort = function (source) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplPortModal/detail.html",
                controller: "siplPortModalController",
                resolve: {
                    requestData: function () {
                        return {
                            originID: ($scope.entity.landingPortId || 0)
                        };
                    }
                }
            });

            modalInstance.result.then(
               function (output) {
                   if (output.resultId == 1001) {
                       $scope.entity.landingPortId = output.data[0].portId;
                       $scope.entity.landingPort = output.data.name;
                   }
               },
               function (output) {
                   ngNotifier.logError(output);
               });
        }

        $scope.callForeignPort = function (source) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/siplPortModal/detail.html",
                controller: "siplPortModalController",
                resolve: {
                    requestData: function () {
                        return {
                            originID: ($scope.entity.foreignPortId || 0)
                        };
                    }
                }
            });

            modalInstance.result.then(
               function (output) {
                   if (output.resultId == 1001) {
                       $scope.entity.foreignPortId = output.data[0].portId;
                       $scope.entity.foreignPort = output.data.name;
                   }
               },
               function (output) {
                   ngNotifier.logError(output);
               });
        }

        //#endregion

        //#region Detail

        $scope.document = {};

        $scope.detailTable = new NgTableParams(
        {
            page: 1,
            count: 10,
            sorting: $.parseJSON("{ \"DocumentCommonId\": \"asc\" }")
        }, {
            counts: [],
            getData: function (params) {
                if ($scope.document && $scope.entity.commodityDetail && $scope.entity.commodityDetail.length == 0) {
                    params.total(0);
                    return null;
                }
                if ($scope.entity.commodityDetail != undefined)// add by vikas
                {
                    $scope.entity.weightType = ($scope.entity.commodityDetail[0].wtType == "LBS") ? "LBS" : "KILO";
                    $scope.entity.measurementType = ($scope.entity.commodityDetail[0].mtType == "CFT") ? "CFT" : "CBM";
                }
                
                params.total($scope.entity.commodityDetail==undefined?0: $scope.entity.commodityDetail.length);
                return $scope.entity.commodityDetail;
            }
        });

        $scope.performSubAction = function (source, fromList) {
            var action = source.currentTarget.attributes["action"].value;
            $scope.$broadcast("show-errors-check-validity");
            if (action != "cancel" && $scope.form.detail != undefined && $scope.form.detail.$invalid) {
                if ($scope.form.detail.$error.required != undefined && $scope.form.detail.$error.required.length > 0) {
                    ngNotifier.error("Required Field(s) are missing data.");
                }
                else if ($scope.form.detail.usrPwdC.$invalid) {
                    ngNotifier.error("Password do not match with Confirm Password.");
                }
                return;
            }
            if (action == "save" && $scope.validateAction != undefined) {
                if (!$scope.validateAction(source)) {
                    return;
                }
            }

            if (fromList) {
                $scope.getConsolidateDockReceipt(action, source.currentTarget.attributes["entityId"].value);

            } else {
                initControls(action);
            }
            switchTab("Detail", action);
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

                case "edit":
                    lastAction = action;
                    break;
                case "save":
                    save(action);
                    break;
                case "cancel":
                    $scope.getConsolidateDockReceipt('viewDetail', $scope.entity.containerId);
                    lastAction = "";
                    break;
                case "delete":
                    remove();
                    lastAction = "";
                    break;
                default:
                    lastAction = "";
                    break;
         //   var action = source.currentTarget.attributes["action"].value;

         ///*   if (action != "viewDocumentList") {
         //       if ($scope.entity.filterFieldValue == null || $scope.entity.filterFieldValue == "") {
         //           ngNotifier.info("Please select document before performing action.");
         //           return;
         //       }
         //   } */

         //   switch (action) {
         //       case "searchDoc":
         //           $scope.documentTable.reload();
         //           break;
         //       case "search":
         //           getDockReceipt();
         //           break;
         //       case "selectDocument":
         //           debugger;

                    
         //           $scope.onClickTab($scope.tabs[0]);
         //           $scope.viewList = false;
         //           $scope.page.urls.container = "app/views/shared/container.html";
         //           $scope.entity = {};
         //           initControls('viewDetail');

         //           var fileNo = source.currentTarget.attributes["entityid"].value;
         //           var docCommId = source.currentTarget.attributes["doccommid"].value;

         //           $scope.entity.filterFieldName = "systemRefNo";
         //           $scope.entity.filterFieldValue = fileNo;
         //           $scope.entity.docCommId = docCommId;
         //         //  editDocument();
         //           getDockReceipt();
         //           break;
         //       case "viewDocumentList":
         //           viewDocumentList();
         //           break;
         //       case "editDocument":
         //           editDocument();
         //           break;
         //       case "saveDocument":
         //           saveDocument();
         //           break;
         //       case "cancelDocument":
         //           cancelDocument();
         //           break;
         //       case "exportPdf":
         //           exportPdf();
         //           break;
         //       case "sendEmail":
         //           sendEmail();
         //           break;
         //       default:
         //           //TODO
         //           break;
            }
        };
        var viewDetail = function () {
            $scope.viewList = false;
            $scope.page.urls.container = "app/views/shared/container.html";
            $scope.entity = {};
        };
        $scope.getConsolidateDockReceipt= function (action, id) {

            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.entityId = id;
            $scope.entity.bookedId = parseFloat(id);
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userId = $scope.$parent.authentication.userId;
            if ($scope.entityId > 0) {
                entityService.getDockReceipt($scope.entity).then(
                    function (output) {
                        if (output.data.resultId == 2005) {
                            ngNotifier.showError($scope.authentication, output);
                            $scope.logOut()
                        }

                        $scope.entity = output.data.data;
                       
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
        var switchTab = function (title, action) {

            $scope.tabs.forEach(function (tab) {
                tab.active = false;
                tab.disabled = ((action === "add" || action === "edit") && tab !== $scope.tabs[0]);
                if (tab.title === title) {
                    tab.active = true;
                }
            });
        };
        $scope.dynamicRow = function (isAdded, target, index) {

            if (isAdded) {
                $scope.entity.commodityDetail.push(initCommodityDetail(index+1));
            }
            else {
                $scope.entity.commodityDetail = $scope.entity.commodityDetail.filter(function (item) { return item !== target; });
            }
            $scope.detailTable.reload();
        };

        $scope.sameAsShipper = function () {

            if ($scope.entity.sameAsShipper) {
                $scope.entity.exporterId = $scope.entity.consignedToId;
                $scope.entity.exporterName = $scope.entity.consignedToName;
                $scope.entity.exporterAddress = $scope.entity.consignedToAddress;
            }
            else {
                $scope.entity.exporterId = 0;
                $scope.entity.exporterName = "";
                $scope.entity.exporterAddress = "";
            }
        };

        $scope.sameAsConsignedTo = function () {

            if ($scope.entity.sameAsConsignedTo) {
                $scope.entity.consigneeId = $scope.entity.consignedToId;
                $scope.entity.consigneeName = $scope.entity.consignedToName;
                $scope.entity.consigneeAddress = $scope.entity.consignedToAddress;
            }
            else {
                $scope.entity.consigneeId = 0;
                $scope.entity.consigneeName = "";
                $scope.entity.consigneeAddress = "";
            }
        };

        $scope.updateWeight = function () {
            $scope.entity.commodityDetail.forEach(function (c) {
                if ($scope.entity.weightType == "LBS") {
                    c.weight = c.weight * 2.205;
                    c.wtType = "LBS";
                }
                else {
                    c.weight = c.weight / 2.205;
                    c.wtType = "KILO";
                }
            });
        };

        $scope.updateMeasurement = function () {
            $scope.entity.commodityDetail.forEach(function (c) {
                if ($scope.entity.measurementType == "CFT") {
                    c.measuremant = c.measuremant * 35.315;
                    c.mtType = "CFT";
                }
                else {
                    c.measuremant = c.measuremant / 35.315;
                    c.mtType = "CBM";
                }
            });
        };

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("consolidatedockReceiptController", controller);

});
