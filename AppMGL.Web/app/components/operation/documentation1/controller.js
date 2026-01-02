// <reference path="controller.js" />
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "documentation1Service"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.documentation1;
        $scope.tabs = appUrl.documentation1.tabs;

        //$scope.$parent.pageTitle = "Awaited Shipper Confirmation";
        //$scope.$parent.breadcrumbs = ["Shipment", "Booking", "Awaited Shipper Confirmation"];
        $scope.comindex = -1;
        $scope.actionRemarksList = [];
        $scope.shipmentDocsList = [];
        $scope.isInvalidData = false;
        $scope.lookups = { siplDepartments: [] };
        $scope.dos_Id = 1;
        $scope.ckOptions = {
            toolbar: [
                { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline'] },
                { name: 'insert', items: ['Image', 'Link', 'Unlink'] },
                { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
                { name: 'colors', items: ['TextColor', 'BGColor'] },
                { name: 'indent', groups: ['list', 'indent', 'align'], items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] }
            ]
        };

        $scope.initDropdown = function () {
            $scope.fetchLookupData("Booking", 0, "name", "exportInformationCodes", "ExportInformationCode");
            $scope.fetchLookupData("Booking", 0, "name", "uomList", "uomList");
            $scope.fetchLookupData("Booking", 0, "name", "lienseExemptionCode", "lienseExemptionCode");
            $scope.fetchLookupData("Booking", 0, "name", "ddtcitar", "ddtcitar");
            $scope.fetchLookupData("Booking", 0, "name", "ddtcusml", "ddtcusml");
        };
        $scope.afterFetchLookupData = function (lookupKey) {

            if (lookupKey == "exportInformationCodes") { $scope.lookups.exportInformationCodes.unshift({ "code": '0', "name": "-Select-" }); }
            if (lookupKey == "citynames") { $scope.lookups.citynames.unshift({ "id": 0, "name": "-Select-" }); }
            if (lookupKey == "uomList") { $scope.lookups.uomList.unshift({ "id": '', "name": "-Select-" }); }
            if (lookupKey == "htsCode") { $scope.lookups.htsCode.unshift({ "id": 0, "name": "-Select-" }); }
            if (lookupKey == "ddtcitar") { $scope.lookups.ddtcitar.unshift({ "id": '', "name": "-Select-" }); }
            if (lookupKey == "ddtcusml") { $scope.lookups.ddtcusml.unshift({ "id": '', "name": "-Select-" }); }
            if (lookupKey == "lienseExemptionCode") {
                $scope.lookups.lienseExemptionCode.unshift({ "code": '', "name": "-Select-" });
                $scope.ExpLicVal = $scope.lookups.lienseExemptionCode;

            }

        };
        $scope.addCommodities = function (data) {

            if ($scope.entity.ScheduleB == "" || $scope.entity.ScheduleB == undefined || $scope.entity.ScheduleB == null) {

                alert("Please select hts code.");
                return true;
            }
            var index = $scope.comindex;
            
            if ($scope.comindex == -1) {
                $scope.entity.commodityDetail.push({
                    exportCode: $scope.entity.ExportCode,
                    scheduleB: $scope.entity.ScheduleB,
                    commodityDescription: $scope.entity.CommodityDescription,
                    firstQuantity: $scope.entity.FirstQuantity,
                    firstUOM: $scope.entity.FirstUOM,
                    secondQuantity: $scope.entity.SecondQuantity,
                    secondUOM: $scope.entity.SecondUOM,
                    originofGoods: $scope.entity.OriginofGoods,
                    valueofGoods: $scope.entity.ValueofGoods,
                    shippingWeight: $scope.entity.ShippingWeight,
                    eccn: $scope.entity.Eccn,
                    licenseTypeCode: $scope.entity.LicenseTypeCode,
                    expLic: $scope.entity.ExpLic,
                    licValueAmount: $scope.entity.LicValueAmount,
                    dDTCITAR: $scope.entity.dDTCITAR,
                    dDTCReg: $scope.entity.dDTCReg,
                    dDTCSignificant: $scope.entity.dDTCSignificant,
                    dDTCEligible: $scope.entity.dDTCEligible,
                    dDTCUSML: $scope.entity.dDTCUSML,
                    dDTCUnit: $scope.entity.dDTCUnit,
                    dDTCQuantity: $scope.entity.dDTCQuantity,
                    dDTCLicense: $scope.entity.dDTCLicense,
                    htsCodeId: $scope.entity.htsCodeId,
                    documentCommonId: $scope.entity.documentCommonId


                });

            }
            else {

                $scope.entity.commodityDetail[index].exportCode = $scope.entity.ExportCode;
                $scope.entity.commodityDetail[index].scheduleB = $scope.entity.ScheduleB;
                $scope.entity.commodityDetail[index].commodityDescription = $scope.entity.CommodityDescription;
                $scope.entity.commodityDetail[index].firstQuantity = $scope.entity.FirstQuantity;
                $scope.entity.commodityDetail[index].firstUOM = $scope.entity.FirstUOM;
                $scope.entity.commodityDetail[index].secondQuantity = $scope.entity.SecondQuantity;
                $scope.entity.commodityDetail[index].secondUOM = $scope.entity.SecondUOM;
                $scope.entity.commodityDetail[index].originofGoods = $scope.entity.OriginofGoods;
                $scope.entity.commodityDetail[index].valueofGoods = $scope.entity.ValueofGoods;
                $scope.entity.commodityDetail[index].shippingWeight = $scope.entity.ShippingWeight;
                $scope.entity.commodityDetail[index].eccn = $scope.entity.Eccn;
                $scope.entity.commodityDetail[index].licenseTypeCode = $scope.entity.LicenseTypeCode;
                $scope.entity.commodityDetail[index].expLic = $scope.entity.ExpLic;
                $scope.entity.commodityDetail[index].licValueAmount = $scope.entity.LicValueAmount;
                //$scope.entity.commodityDetail[index].isGovermentAgency = $scope.entity.IsGovermentAgency;
                //$scope.entity.commodityDetail[index].documentCommonId = $scope.entity.documentCommonId;
                //$scope.entity.commodityDetail[index].siteId = $scope.entity.siteId;

                $scope.entity.commodityDetail[index].dDTCITAR = $scope.entity.dDTCITAR;
                $scope.entity.commodityDetail[index].dDTCReg = $scope.entity.dDTCReg;
                if ($scope.entity.dDTCSignificant == undefined)
                    $scope.entity.commodityDetail[index].dDTCSignificant = '';
                else
                    $scope.entity.commodityDetail[index].dDTCSignificant = $scope.entity.dDTCSignificant;
                if ($scope.entity.dDTCEligible == undefined)
                    $scope.entity.commodityDetail[index].dDTCEligible = '';
                else
                    $scope.entity.commodityDetail[index].dDTCEligible = $scope.entity.dDTCEligible;

                $scope.entity.commodityDetail[index].dDTCUSML = $scope.entity.dDTCUSML;
                $scope.entity.commodityDetail[index].dDTCUnit = $scope.entity.dDTCUnit;
                $scope.entity.commodityDetail[index].dDTCQuantity = $scope.entity.dDTCQuantity;
                $scope.entity.commodityDetail[index].dDTCLicense = $scope.entity.dDTCLicense;
                // $scope.entity.commodityDetail[index].ctype = $scope.entity.ctype;
                $scope.entity.commodityDetail[index].htsCodeId = $scope.entity.htsCodeId;

            }

            $scope.comindex = -1;
            $scope.BlankCommoditesControl();
        };
        $scope.selecteditCommodities = function (id) {
            $scope.comindex = id;
            var comm = $scope.entity.commodityDetail[id];
            $scope.entity.idd = comm.idd;
            if (comm.exportCode == null || comm.exportCode == "" || comm.exportCode == undefined)
                $scope.entity.ExportCode = 'OS';
            else
                $scope.entity.ExportCode = comm.exportCode;

            $scope.entity.ScheduleB = comm.scheduleB;
            $scope.entity.CommodityDescription = comm.commodityDescription;
            $scope.entity.FirstQuantity = comm.firstQuantity;
            if (comm.firstUOM == null || comm.firstUOM == "" || comm.firstUOM == undefined)
                $scope.entity.FirstUOM = 'KG';
            else
                $scope.entity.FirstUOM = comm.firstUOM

            $scope.entity.SecondQuantity = comm.secondQuantity;

            if (comm.secondUOM == null || comm.secondUOM == "" || comm.secondUOM == undefined)
                $scope.entity.SecondUOM = 'KG';
            else
                $scope.entity.SecondUOM = comm.secondUOM;

            if (comm.originofGoods == null || comm.originofGoods == "" || comm.originofGoods == undefined)
                $scope.entity.OriginofGoods = 'D - DOMESTIC';
            else
                $scope.entity.OriginofGoods = comm.originofGoods;

            $scope.entity.ValueofGoods = comm.valueofGoods;
            $scope.entity.ShippingWeight = comm.shippingWeight;
            $scope.entity.Eccn = comm.eccn;
            if (comm.licenseTypeCode == null || comm.licenseTypeCode == "" || comm.licenseTypeCode == undefined) {
                $scope.entity.LicenseTypeCode = 'C33';
                $scope.entity.ExpLic = 'NLR';
            }
            else {
                $scope.entity.LicenseTypeCode = comm.licenseTypeCode;
                $scope.entity.ExpLic = comm.expLic;
            }

            $scope.entity.LicValueAmount = comm.licValueAmount;

            $scope.entity.dDTCITAR = comm.dDTCITAR;
            $scope.entity.dDTCReg = comm.dDTCReg;
            $scope.entity.dDTCSignificant = comm.dDTCSignificant;
            $scope.entity.dDTCEligible = comm.dDTCEligible;
            $scope.entity.dDTCUSML = comm.dDTCUSML;
            $scope.entity.dDTCUnit = comm.dDTCUnit;
            $scope.entity.dDTCQuantity = comm.dDTCQuantity;
            $scope.entity.dDTCLicense = comm.dDTCLicense;

            $scope.entity.htscodeid = comm.htscodeid;
            //  $scope.hiddefence(comm.isGovermentAgency);


            // $scope.isaddcomdity = 0;


        };
        $scope.delCommodities = function (rownum) {
            var cind = $scope.entity.commodityDetail[rownum];
            $scope.entity.commodityDetail.splice(rownum, 1);
        };
        $scope.BlankCommoditesControl = function () {
            $scope.entity.idd = "0";
            $scope.entity.ExportCode = 'OS';
            $scope.entity.ScheduleB = "";
            $scope.entity.CommodityDescription = "";
            $scope.entity.FirstQuantity = "";
            $scope.entity.FirstUOM = "KG";
            $scope.entity.SecondQuantity = "";
            $scope.entity.SecondUOM = "KG";
            $scope.entity.OriginofGoods = "D - DOMESTIC";
            $scope.entity.ValueofGoods = "";
            $scope.entity.ShippingWeight = "";
            $scope.entity.Eccn = "";
            $scope.entity.LicenseTypeCode = 'C33';
            $scope.entity.ExpLic = "NLR";
            // $scope.entity.IsGovermentAgency = "N";
            $scope.entity.dDTCITAR = '';
            $scope.entity.dDTCReg = "";
            $scope.entity.dDTCSignificant = "";
            $scope.entity.dDTCEligible = "";
            $scope.entity.dDTCUSML = '';
            $scope.entity.dDTCUnit = "";
            $scope.entity.dDTCQuantity = "";
            $scope.entity.dDTCLicense = "";
            // $scope.entity.ctype == "A"
            $scope.entity.htsCodeId == "0"
            //$scope.hiddefence('N');
        }
        $scope.addVin = function () {
            var htscodeid = "";
            var htscode = "";
            var isSelected = false;
            var iscount = 0;
            if ($scope.entity.commodityDetail!= undefined && $scope.entity.commodityDetail.length> 0) {
                $scope.entity.commodityDetail.forEach(function (item) {
                    if (item.selected) {
                        htscodeid = item.htsCodeId;
                        htscode = item.scheduleB;
                        isSelected = true;
                        iscount = iscount + 1;

                    }
                });
                if (isSelected == true && iscount == 1) {
                    $scope.entity.vinDetail.push({
                        htsCodeId: htscodeid, htsCode: htscode, vin: "Select", vinNumber: "", vehicleTitleNum: "", vehicleTitleState: "", siteId: $scope.entity.siteId, documentCommonId: $scope.entity.documentCommonId, line_No: $scope.comindex
                    });
                }
                else {
                    alert("Please select one comodity item fist then add vin detail");

                }
            }
            else {
                alert("Please select one comodity item fist then add vin detail");
            }
        };
        $scope.delvinDetail = function (rownum) {

            $scope.entity.vinDetail.splice(rownum, 1);

        };

        //#endregion       

        $scope.searchOptions = [
            { optionValue: "", optionName: "-All-" },
            { optionValue: "SystemRefNo", optionName: "System Ref No" },
            { optionValue: "BookingNo", optionName: "Booking No" },
            { optionValue: "CustomerName", optionName: "Customer Name" },
            { optionValue: "CarrierName", optionName: "Carrier Name" }
        ];


        $scope.updateControls = function () {
            $scope.selectOption = "SystemRefNo";
            $scope.searchBox = "";
            $scope.dashboardOption = "";
            if ($scope.$parent.pageTitle == "Pending Shipping Instruction") {
                $scope.dos_Id = 2;
            }
            if ($scope.$parent.pageTitle == "Pending filing of SED") {
                $scope.dos_Id = 3;
            }
            if ($scope.$parent.pageTitle == "Pending B/L instruction Customer") {
                $scope.dos_Id = 4;
            }
            if ($scope.$parent.pageTitle == "Pending B/L instruction Line") {
                $scope.dos_Id = 5;
            }
            if ($scope.$parent.pageTitle == "Pending Draft B/L Line") {
                $scope.dos_Id = 6;
            }
            if ($scope.$parent.pageTitle == "Pending Send Draft B/L Customer") {
                $scope.dos_Id = 7;
            }
            if ($scope.$parent.pageTitle == "Pending Draft B/L Approval From Customer") {
                $scope.dos_Id = 8;
            }
            if ($scope.$parent.pageTitle == "Pending to Send Approved Draft B/L to Line") {
                $scope.dos_Id = 9;
            }
            if ($scope.$parent.pageTitle == "B/L Release Awaited From Line") {
                $scope.dos_Id = 10;
            }
        };

        $scope.filterBookings = function () {
            var dashboardOption = localStorageService.get("dashboardOption");
            $scope.selectOption = "companyName";
            $scope.searchBox = "";
            if (dashboardOption != null) {
                if (dashboardOption == 'BASCU') {
                    $scope.dashboardOption = "BASCU";
                }

                if (dashboardOption == 'BASCF') {
                    $scope.dashboardOption = "";
                }

                localStorageService.remove("dashboardOption");
            }
            else {
                $scope.dashboardOption = "";
            }

        };

        $scope.customClearLookups = function (source, lookupModule, lookupIndex, lookupField) {
        }

        $scope.clearLookups = function (source, lookup, index) {
        }
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
        $scope.updateControls();


        $scope.searchParam = {
            optionValue: $scope.selectOption,
            seachValue: $scope.searchBox,
            dos_Id: $scope.dos_Id,
            dashboardOption: $scope.dashboardOption,
            userName: $scope.$parent.authentication.userName,
            userType: $scope.$parent.userInfo.contact.cntType
        };

        $scope.documentationlistTable = new NgTableParams(
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

                var dataitems = entityService.getDocumentationList(listParams).then(
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
        $scope.performBookingSearch = function (source, selectOption, searchBox) {

            var action = source.currentTarget.attributes["action"].value;
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox,
                dos_Id: $scope.dos_Id,
                dashboardOption: $scope.dashboardOption,
                userName: $scope.$parent.authentication.userName,
                userType: $scope.$parent.userInfo.contact.cntType
            };

            $scope.documentationlistTable.reload();
        };

        $scope.performBookingAction = function (source, fromList) {

            var action = source.currentTarget.attributes["action"].value;


            if (action == "save" && $scope.validateAction != undefined) {
                if (!$scope.validateAction(source)) {
                    return;
                }
            }

            if (fromList) {

                ///$scope.showBookingDetail(action, documentCommonID);

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
                    //$scope.getDocumentAttachmentDetail(action, documentCommonID);
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

                    $scope.goBack();
                    break;
                case "delete":
                    remove();
                    //lastAction = "";
                    break;
                case "deleteBatch":

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

        $scope.saveShiperData = function () {
            $scope.$broadcast("show-errors-check-validity");
            if ($scope.entity.usppiCompany == null) {
                ngNotifier.error("Please enter usppi Name");
                return;
            }
            if ($scope.entity.usppiAddress == null) {
                ngNotifier.error("Please enter usppi address");
                return;
            }
            if ($scope.entity.usppiEIN == null) {
                ngNotifier.error("Please enter usspi EIN");
                return;
            }
            if ($scope.entity.usppiFirstName == null) {
                ngNotifier.error("Please enter usppi first name");
                return;
            }
            if ($scope.entity.usppiLastName == null) {
                ngNotifier.error("Please enter usppi last name");
                return;
            }
            if ($scope.entity.usppiState == null) {
                ngNotifier.error("Please enter usppi state");
                return;
            }
            if ($scope.entity.usppiCity == null) {
                ngNotifier.error("Please enter usppi city");
                return;
            }
            if ($scope.entity.usppiPostalCode == null) {
                ngNotifier.error("Please enter usppi postal code");
                return;
            }
            if ($scope.entity.ultimateCompany == null) {
                ngNotifier.error("Please enter ultimate Consignee Name");
                return;
            }
            if ($scope.entity.ultimateCountry == null) {
                ngNotifier.error("Please enter ultimate country");
                return;
            }
            if ($scope.entity.usppiCity == null) {
                ngNotifier.error("Please enter ultimate city");
                return;
            }
            if ($scope.entity.ultimateAddress == null) {
                ngNotifier.error("Please enter ultimate address");
                return;
            }
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.createdBy = $scope.$parent.authentication.userId;
            entityService.saveShiperData($scope.entity).then(
                function (output) {
                    $scope.entity = {};
                    $scope.selectOption = "SystemRefNo";
                    $scope.searchBox = "";
                    $scope.documentationlistTable.reload();
                    $scope.goBack();
                    ngNotifier.show(output.data);
                },
            function (output) {
                ngNotifier.showError($scope.authentication, output);
                $scope.editMode = false;
                $scope.disabledInsert = true;
                $scope.disabledUpdate = true;
                $scope.requiredInsert = false;
                $scope.requiredUpdate = false;
            }

            );
        };

        $scope.getDocumentationDetail = function (source, documentCommonId) {
            var action = source.currentTarget.attributes["action"].value;
            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.entity = {};
            $scope.entity.documentCommonId = documentCommonId;
            entityService.getShiperDetail($scope.entity).then(
                function (output) {
                    if (output.data.resultId == 2005) {
                        ngNotifier.showError($scope.authentication, output);
                        $scope.logOut()
                    }

                    if (output.data.data != null) { 
                        $scope.entity = output.data.data;
                        if (output.data.data.commodityDetail == null)
                            $scope.entity.commodityDetail = [];
                        if (output.data.data.vinDetail == null)
                            $scope.entity.vinDetail = [];
                }

                    else {
                        $scope.entity = [];
                       // $scope.entity.documentCommonId = documentCommonId;
                        $scope.entity.commodityDetail = [];
                        $scope.entity.vinDetail = [];
                    }
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

        $scope.showNextActionRemarksModel = function () {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/operation/bookingCaptureContainer/nextActionRemarks.html",
                controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                    $scope.actionRemarks = requestData.actionRemarks
                    $scope.select = function (action) {
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
                            actionRemarks: $scope.actionRemarksList
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.action == "update") {

                    }
                    else if (output == "close") {

                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

       

        //#endregion
        $scope.searchValues = function (viewValue, selectType, searchRouteType) {
            var resultItem = {};
            var lookupModule;
            var routeType = "";
            var lookupField = "name";
            if (selectType == "searchBox1") {
                if ($scope.entity.selectOption1 == "" || $scope.entity.selectOption1 == null) {
                    ngNotifier.error("Please select Search options");
                    return;
                }
                if ($scope.entity.selectOption1 == 'ShipperCustomerName' || $scope.entity.selectOption1 == 'CompanyName' || $scope.entity.selectOption1 == 'Supplier' || $scope.entity.selectOption1 == 'Transporter') {
                    lookupModule = "SIPLContact";
                    lookupField = "companyName";
                }
                if ($scope.entity.selectOption1 == "LoadingPort" || $scope.entity.selectOption1 == "DestinationPort") {
                    lookupModule = "LGVWPort";
                    routeType = "Port";
                }

            }
            if (selectType == "searchBox2") {
                if ($scope.entity.selectOption2 == "" || $scope.entity.selectOption2 == null) {
                    ngNotifier.error("Please select Search options");
                    return;
                }
                if ($scope.entity.selectOption2 == 'ShipperCustomerName' || $scope.entity.selectOption2 == 'CompanyName' || $scope.entity.selectOption2 == 'Supplier' || $scope.entity.selectOption2 == 'Transporter') {
                    lookupModule = "SIPLContact";
                    lookupField = "companyName";
                }
                if ($scope.entity.selectOption2 == "LoadingPort" || $scope.entity.selectOption2 == "DestinationPort") {
                    lookupModule = "LGVWPort";
                    routeType = "Port";
                }

            }
            if (selectType == "searchBox3") {
                if ($scope.entity.selectOption3 == "" || $scope.entity.selectOption3 == null) {
                    ngNotifier.error("Please select Search options");
                    return;
                }
                if ($scope.entity.selectOption3 == 'ShipperCustomerName' || $scope.entity.selectOption3 == 'CompanyName' || $scope.entity.selectOption3 == 'Supplier' || $scope.entity.selectOption3 == 'Transporter') {
                    lookupModule = "SIPLContact";
                    lookupField = "companyName";
                }
                if ($scope.entity.selectOption3 == "LoadingPort" || $scope.entity.selectOption3 == "DestinationPort") {
                    lookupModule = "LGVWPort";
                    routeType = "Port";
                }

            }

            if (lookupModule == "SIPLContact" || selectType == "companyName") {
                return $scope.callTypeahead(viewValue, 'SIPLContact', 'companyName', null).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.forEach(function (o) {
                            resultItem = {}
                            resultItem.name = o.companyName;
                            resultItem.contactID = o.contactID;
                            resultItem.address = o.address;
                            $scope.searchResult.push(resultItem)
                        });
                        return $scope.searchResult;
                    }
                );
            }

            if (selectType == "htsCode") {
                lookupModule = "Booking";
                lookupField = "htsCode";

                var listParams = {
                    SiteId: $scope.selectedSite.siteId,
                    CwtId: $scope.userWorkTypeId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: 1,
                    PageSize: 25,
                    Sort: "{ \"" + lookupField + "\": \"asc\" }",
                    Filter: viewValue
                };
                return entityService.getHTSCode(listParams).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.data.data.forEach(function (o) {
                            resultItem = {}
                            resultItem.id = o.id;
                            resultItem.name = o.name;
                            resultItem.htscode = o.htscode;
                            resultItem.unit = o.unit;
                            resultItem.unit1 = o.unit1;
                            resultItem.description = o.description;
                            resultItem.isvin = o.isvin;
                            $scope.searchResult.push(resultItem)

                        });
                        return $scope.searchResult;

                    }
                );
            }
            if (selectType == "portofExport" || selectType == "portofUnloading") {
                lookupModule = "Booking";
                lookupField = "portofExport";

                var listParams = {
                    SiteId: $scope.selectedSite.siteId,
                    CwtId: $scope.userWorkTypeId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: 1,
                    PageSize: 25,
                    Sort: "{ \"" + lookupField + "\": \"asc\" }",
                    Filter: viewValue
                };
                return entityService.getPortofExport(listParams).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.data.data.forEach(function (o) {
                            resultItem = {}

                            resultItem.name = o.name;
                            resultItem.id = o.id;
                            //resultItem.code = o.code;
                            resultItem.portId = o.portId;
                            $scope.searchResult.push(resultItem);

                        });
                        return $scope.searchResult;

                    }
                );
            }

            if (selectType == "countryDestination") {
                lookupModule = "Booking";
                lookupField = "countryDestination";

                var listParams = {
                    SiteId: $scope.selectedSite.siteId,
                    CwtId: $scope.userWorkTypeId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: 1,
                    PageSize: 25,
                    Sort: "{ \"" + lookupField + "\": \"asc\" }",
                    Filter: viewValue
                };
                return entityService.getCountryofDestination(listParams).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.data.data.forEach(function (o) {
                            resultItem = {}
                            resultItem.itemName = o.itemName;
                            resultItem.itemId = o.itemId;
                            resultItem.itemCode = o.itemCode;
                            $scope.searchResult.push(resultItem)

                        });
                        return $scope.searchResult;

                    }
                );
            }
            if (selectType == "originState") {
                lookupModule = "Booking";
                lookupField = "originState";

                var listParams = {
                    SiteId: $scope.selectedSite.siteId,
                    CwtId: $scope.userWorkTypeId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: 1,
                    PageSize: 25,
                    Sort: "{ \"" + lookupField + "\": \"asc\" }",
                    Filter: viewValue
                };
                return entityService.getOriginState(listParams).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.data.data.forEach(function (o) {
                            resultItem = {}
                            resultItem.itemId = o.itemId;
                            resultItem.itemName = o.itemName;
                            resultItem.itemCode = o.itemCode;
                            $scope.searchResult.push(resultItem)

                        });
                        return $scope.searchResult;

                    }
                );
            }
        };
        $scope.selectTypeaheadsli = function ($item, $model, $label, source, stype) {

            var lookupModule = null;
            var lookupIndex = null;
            var target = $(source.currentTarget);

            if (stype == "htsCode") {
                $scope.entity.ScheduleB = $item.htscode;
                $scope.entity.htsCodeId = $item.id;
                $scope.entity.FirstUOM = $item.unit;
                $scope.entity.SecondUOM = $item.unit1;
                $scope.entity.CommodityDescription = $item.description;
                // $scope.showVin($item.htscode, $item.isvin);
            }


            if (stype == "portofExport") {
                $scope.entity.portofExportId = $item.portId;
                $scope.entity.portofExportCode = $item.id;
            }
            if (stype == "portofUnloading") {
                $scope.entity.portofUnloadingId = $item.portId;
                $scope.entity.portofUnloadingCode = $item.id;
            }
            if (stype == "countryDestination") {
                $scope.entity.countryDestinationId = $item.itemId;
                $scope.entity.countryDestinationCode = $item.itemCode;
            }
            if (stype == "ultimateCountry") {
                $scope.entity.ultimateCountryId = $item.itemId;
                $scope.entity.ultimateCountryCode = $item.itemCode;
            }
            if (stype == "interCountry") {
                $scope.entity.interCountryId = $item.itemId;
                $scope.entity.interCountryCode = $item.itemCode;
            }

            if (stype == "originState") {
                $scope.entity.originStateCode = $item.itemCode;
                $scope.entity.originStateId = $item.itemId;
            }
            if (stype == "usppiState") {
                $scope.entity.usppiStateCode = $item.itemCode;
                $scope.entity.usppiStateId = $item.itemId;
            }
            if (stype == "ultimateState") {
                $scope.entity.ultimateStateCode = $item.itemCode;
                $scope.entity.ultimateStateId = $item.itemId;
            }

            if (stype == "interState") {
                $scope.entity.interStateCode = $item.itemCode;
                $scope.entity.interStateId = $item.itemId;
            }
            if (stype == "freightState") {
                $scope.entity.freightStateCode = $item.itemCode;
                $scope.entity.freightStateId = $item.itemId;
            }
            

            if (source.type == "click") {
                target = $(source.currentTarget).parent().parent().find("input");
            }

            var lookupModule = target.attr("lookup-module");
            var lookupIndex = target.attr("lookup-index");

            var output = { data: [] };
            output.data.push($item);

            if ($scope.setLookups != undefined) {
                $scope.setLookups(source, lookupModule, output, lookupIndex);
            }
        };
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("documentation1Controller", controller);

});
