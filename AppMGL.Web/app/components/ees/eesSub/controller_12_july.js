
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "eessubService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {


        $scope.page = appUrl.eessub;
        $scope.tabs = appUrl.eessub.tabs;
        $scope.comindex = 0;
        $scope.isDel = "D";
        $scope.ExpLicVal;
        $(".lineNumberTitle").text('');

        $scope.next = function () {

            $scope.selecteditCommodities($scope.comindex + 1);
        }
        $scope.prev = function () {
            $scope.selecteditCommodities($scope.comindex - 1);

        }
        $scope.hiddefence = function (value) {
            if (value == "Y") {
                $("#divdtci").show();
            }
            else { $("#divdtci").hide(); }
        }
        $scope.hideshow = function (name) {
            if (name == "lineDetailsDiv") {
                $("#lineDetailsDiv").show();
                $("#tbllinesummary").hide();
                $scope.HideShowNextPrevious($scope.comindex);
                $(".lineNumberTitle").text(1);
                $scope.comindex = 0;
                $scope.selecteditCommodities(0);
                $scope.entity.IsGovermentAgency = "N";
                $scope.hiddefence('N');
            }
            else {
                $("#lineDetailsDiv").hide();
                $("#tbllinesummary").show();
                $(".lineNumberTitle").text('');
            }
        };
        //$scope.getFiltered = function (obj, idx) {

        //    return ((obj.line_No == $scope.comindex));
        //}
        $scope.addNewLine = function () {
            var length = $scope.entity.commodityDetail.length;
            if (length > 0) {
                $scope.comindex = length;
            }
            $scope.entity.commodityDetail.push({
                isType: "N", exportCode: "", scheduleB: "", commodityDescription: "",
                firstQuantity: "", firstUOM: "", secondQuantity: "", secondUOM: "", originofGoods: "",
                valueofGoods: "", shippingWeight: "", eccn: "", licenseTypeCode: "", isGovermentAgency: "Y", documentCommonId: $scope.entity.documentCommonID, siteId: $scope.entity.siteId
            });
            $(".lineNumberTitle").text(length + 1);
            // $("#hdid").val($scope.comindex + 1)
            $scope.BlankCommoditesControl();
            // $scope.hideshowVin();
            $scope.entity.IsGovermentAgency = "N";
            $scope.hiddefence('N');
        };
        $scope.addNewLine1 = function () {
            $scope.addNewLine();
            $("#lineDetailsDiv").show();
            $("#tbllinesummary").hide();
            $("#LineSummary_tab_li").removeClass("active");
            $("#LineDetails_tab_li").addClass("active");
            $scope.HideShowNextPrevious($scope.comindex);

        };

        $scope.CloseAES = function () {
            $scope.bookinglistTable.reload();
            $scope.goBack();

        };

        $scope.addCommodities = function (data) {
            
            var index = $scope.comindex;
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
            $scope.entity.commodityDetail[index].isGovermentAgency = $scope.entity.IsGovermentAgency;
            $scope.entity.commodityDetail[index].documentCommonID = $scope.entity.documentCommonID;
            $scope.entity.commodityDetail[index].siteId = $scope.entity.siteId;

            $scope.entity.commodityDetail[index].dDTCITAR = $scope.entity.dDTCITAR;
            $scope.entity.commodityDetail[index].dDTCReg = $scope.entity.dDTCReg;
            $scope.entity.commodityDetail[index].dDTCSignificant = $scope.entity.dDTCSignificant;
            $scope.entity.commodityDetail[index].dDTCEligible = $scope.entity.dDTCEligible;
            $scope.entity.commodityDetail[index].dDTCUSML = $scope.entity.dDTCUSML;
            $scope.entity.commodityDetail[index].dDTCUnit = $scope.entity.dDTCUnit;
            $scope.entity.commodityDetail[index].dDTCQuantity = $scope.entity.dDTCQuantity;
            $scope.entity.commodityDetail[index].dDTCLicense = $scope.entity.dDTCLicense;

            $scope.BlankCommoditesControl();
           
            return false;
        };
        $scope.BlankCommoditesControl = function () {
            $scope.entity.ExportCode = "-";
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
            $scope.entity.LicenseTypeCode = "";
            $scope.entity.ExpLic = "";
            $scope.entity.IsGovermentAgency = "N";
            $scope.entity.dDTCITAR = "";
            $scope.entity.dDTCReg = "";
            $scope.entity.dDTCSignificant = "";
            $scope.entity.dDTCEligible = "";
            $scope.entity.dDTCUSML = "";
            $scope.entity.dDTCUnit = "";
            $scope.entity.dDTCQuantity = "";
            $scope.entity.dDTCLicense = "";
            $scope.hiddefence('N');
        }
        $scope.delCommodities = function (rownum) {
            var cind = $scope.entity.commodityDetail[rownum];
            if (cind.isType == "O") {
                $scope.entity.commodityDetail[rownum].isType = "D";
            }
            else {
                $scope.entity.commodityDetail.splice(rownum, 1);
            }
        };
        $scope.delCommodi = function () {
            
            $scope.delCommodities($scope.comindex);
            $scope.hideshow('');

        };
        $scope.selecteditCommodities = function (id) {
            $scope.comindex = id;
            var comm = $scope.entity.commodityDetail[id];
            // $scope.entity.Line_No = comm.line_No;
            $scope.entity.ExportCode = comm.exportCode;
            $scope.entity.ScheduleB = comm.scheduleB;
            $scope.entity.CommodityDescription = comm.commodityDescription;
            $scope.entity.FirstQuantity = comm.firstQuantity;
            $scope.entity.FirstUOM = comm.firstUOM;
            $scope.entity.SecondQuantity = comm.secondQuantity;
            $scope.entity.SecondUOM = comm.secondUOM
            $scope.entity.OriginofGoods = comm.originofGoods;
            $scope.entity.ValueofGoods = comm.valueofGoods;
            $scope.entity.ShippingWeight = comm.shippingWeight;
            $scope.entity.Eccn = comm.eccn;
            $scope.entity.LicenseTypeCode = comm.licenseTypeCode;
            $scope.entity.ExpLic = comm.expLic;
            $scope.entity.IsGovermentAgency = comm.isGovermentAgency;
            $scope.entity.dDTCITAR = comm.dDTCITAR;
            $scope.entity.dDTCReg = comm.dDTCReg;
            $scope.entity.dDTCSignificant = comm.dDTCSignificant;
            $scope.entity.dDTCEligible = comm.dDTCEligible;
            $scope.entity.dDTCUSML = comm.dDTCUSML;
            $scope.entity.dDTCUnit = comm.dDTCUnit;
            $scope.entity.dDTCQuantity = comm.dDTCQuantity;
            $scope.entity.dDTCLicense = comm.dDTCLicense;
            
            $scope.hiddefence(comm.isGovermentAgency);
           
            $("#lineDetailsDiv").show();
            $("#tbllinesummary").hide();
            $(".lineNumberTitle").text($scope.comindex + 1);
            $("#LineSummary_tab_li").removeClass("active");
            $("#LineDetails_tab_li").addClass("active");
            $scope.HideShowNextPrevious($scope.comindex);

        };
        $scope.HideShowNextPrevious = function (rownum) {
            if ($scope.entity.commodityDetail[rownum + 1] != null)
                $("#nextline").show();
            else
                $("#nextline").hide();
            if ($scope.entity.commodityDetail[rownum - 1] != null)
                $("#previousline").show();
            else
                $("#previousline").hide();
            
        }
        $scope.callCompanyModal = function (ctype) {

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
                            companyID: ($scope.entity.fkCompanyID || 0)
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                       
                        if (ctype == 'u') {
                            $scope.entity.ultimateCompanyName = output.data.companyName;
                            $scope.entity.ultimateAddressLine1 = output.data.address;
                            if (output.data.contactPerson != "" && output.data.contactPerson != undefined) {
                                $scope.entity.ultimateFirstName = output.data.contactPerson.split(" ")[0];
                                $scope.entity.ultimateLastName = output.data.contactPerson.split(" ")[1];
                            }
                            $scope.entity.ultimatePhoneNumber = output.data.telNo;
                            $scope.entity.ultimateCountryId = output.data.countryName;
                            $scope.entity.ultimatePostalCodeId = output.data.zipCode;
                            $scope.entity.ultimateCity = output.data.cityName;
                            $scope.entity.ultimateStateId = output.data.stateName;
                            $scope.entity.ultimateEmail = output.data.email;
                            $scope.entity.ultimateIdNumber = output.data.idNumber;
                            $scope.entity.ultimateIdNumberTypeId = output.data.idNumberType;

                        }
                        else {
                            $scope.entity.interCompanyName = output.data.companyName;
                            $scope.entity.interAddressLine1 = output.data.address;
                            if (output.data.contactPerson != "" && output.data.contactPerson != undefined) {
                                $scope.entity.interFirstName = output.data.contactPerson.split(" ")[0];
                                $scope.entity.interLastName = output.data.contactPerson.split(" ")[1];
                            }
                            $scope.entity.interPhoneNumber = output.data.telNo;
                            $scope.entity.interCountryId = output.data.countryName;
                            $scope.entity.interPostalCodeId = output.data.zipCode;
                            $scope.entity.interCity = output.data.city;
                            $scope.entity.interStateId = output.data.stateName;
                            $scope.entity.interEmail = output.data.email;
                            $scope.entity.interIdNumber = output.data.idNumber;
                            $scope.entity.interIdNumberTypeId = output.data.idNumberType;

                        }
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.addEquipmentLine = function (data) {
            debugger
            var length = $scope.entity.equipmentLineDetail.length;
            var LineNo = (length + 1).toString();
            $scope.entity.equipmentLineDetail.push({
                lineNo: LineNo, equipmentNumber: "", eealNumber: "", documentCommonId: $scope.entity.documentCommonID, siteId: $scope.entity.siteId
            });
            return false;
        };
        $scope.delEquipmentLine = function (rownum) {

            $scope.entity.equipmentLineDetail.splice(rownum, 1);

        };
        $scope.callHtsCodeModal = function () {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/datamanagement/htsCodeModal/detail.html",
                controller: "htsCodeModalController"
                
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.ScheduleB = output.data.htsNumber;
                        
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };
        function getSelectedIndex(id) {
            for (var i = 0; i < $scope.entity.equipmentLineDetail.length; i++)
                if ($scope.entity.equipmentLineDetail[i].lineNo == id)
                    return i;
            return -1;
        };
        function getSelectedIndexcom(id) {
            for (var i = 0; i < $scope.entity.commodityDetail.length; i++)
                if ($scope.entity.commodityDetail[i].line_No == id)
                    return i;
            return -1;
        };
        function getSelectedIndexVin(id) {
            for (var i = 0; i < $scope.entity.vinDetail.length; i++)
                if ($scope.entity.vinDetail[i].vIN == id)
                    return i;
            return -1;
        };
        $scope.addVin = function () {

            $scope.entity.vinDetail.push({
                vin: "Select", vinNumber: "", vehicleTitleNum: "", vehicleTitleState: "", siteId: $scope.entity.siteId, documentCommonId: $scope.entity.documentCommonID, line_No: $scope.comindex
            });

        };
        $scope.delvinDetail = function (rownum) {

            $scope.entity.vinDetail.splice(rownum, 1);

        };
        $scope.showVin = function (ScheduleB) {
            if (ScheduleB == "8703.80.0000") {
                $("#VIN_section").show();
                $("#VIN_tab_li").show();
            }
            else {
                $("#VIN_section").hide();
                $("#VIN_tab_li").hide();
            }
        }
        $scope.actionRemarksList = [];
        $scope.shipmentDocsList = [];
        $scope.searchOptions2 = [];
        $scope.searchOptions3 = [];
        $scope.isOption2disabled = true;
        $scope.isOption3disabled = true;
        $scope.lookups = { siplDepartments: [], miamiBookingStatus: [], countrynames: [], statenames: [], filingOptions: [], modeOfTransports: [], exportInformationCodes: [], uomList: [], htsCode: [], portofExport: [] };
        $scope.searchResult = [];

        $scope.initDropdown = function () {
            $scope.fetchLookupData("sipldepartment", 0, "displayOrder", "siplDepartments", null);
            $scope.fetchLookupData("siplBookingStatus", 0, "Status", "miamiBookingStatus", null);
            $scope.fetchLookupData("siplcountry", 0, "name", "countrynames", null);
            $scope.fetchLookupData("siplstate", 0, "name", "statenames", null);
            $scope.fetchLookupData("Booking", 0, "name", "filingOptions", "FilingOption");
            $scope.fetchLookupData("Booking", 0, "name", "modeOfTransports", "ModeOfTransport");
            $scope.fetchLookupData("Booking", 0, "name", "exportInformationCodes", "ExportInformationCode");
            $scope.fetchLookupData("lgvwcity", 0, "name", "citynames", null);
            $scope.fetchLookupData("Booking", 0, "name", "uomList", "uomList");
            $scope.fetchLookupData("Booking", 0, "name", "lienseExemptionCode", "lienseExemptionCode");
        };
        $scope.afterFetchLookupData = function (lookupKey) {
            if (lookupKey == "siplDepartments") { $scope.lookups.siplDepartments.unshift({ "departmentID": 0, "department": "-Select-" }); } //
            if (lookupKey == "miamiBookingStatus") { $scope.lookups.miamiBookingStatus.unshift({ "statusID": 0, "status": "-Select-" }); } //
            if (lookupKey == "countrynames") { $scope.lookups.countrynames.unshift({ "countryId": 0, "country": "-Select-" }); } //
            if (lookupKey == "statenames") { $scope.lookups.statenames.unshift({ "stateId": 0, "state": "-Select-" }); } //
            if (lookupKey == "filingOptions") { $scope.lookups.filingOptions.unshift({ "id": 0, "name": "-Select-" }); } //
            if (lookupKey == "modeOfTransports") { $scope.lookups.modeOfTransports.unshift({ "id": 0, "name": "-Select-" }); } //
            if (lookupKey == "exportInformationCodes") { $scope.lookups.exportInformationCodes.unshift({ "id": 0, "name": "-Select-" }); } //
            if (lookupKey == "citynames") { $scope.lookups.citynames.unshift({ "id": 0, "name": "-Select-" }); } //
            if (lookupKey == "uomList") { $scope.lookups.uomList.unshift({ "id": 0, "name": "-Select-" }); } //
            if (lookupKey == "htsCode") { $scope.lookups.htsCode.unshift({ "id": 0, "name": "-Select-" }); } //
            // if (lookupKey == "portofExport") { $scope.lookups.portofExport.unshift({ "id": 0, "name": "-Select-" }); } //
            if (lookupKey == "lienseExemptionCode")
            {
                $scope.lookups.lienseExemptionCode.unshift({ "id": 0, "name": "-Select-" });
                $scope.ExpLicVal = $scope.lookups.lienseExemptionCode;

            } 

        };
        //#endregion       

        $scope.searchOptions = [
            { optionValue: "", optionName: "-Select-" },
            { optionValue: "PONO", optionName: "Consignee's PO #" },
            { optionValue: "ShipperCustomerName", optionName: "Consignee's Name" },
            { optionValue: "CompanyName", optionName: "Customer" },
            { optionValue: "FileNo", optionName: "System Ref No" },
            { optionValue: "Supplier", optionName: "Supplier Name" },
            { optionValue: "DocumentNumber", optionName: "Line Booking No" },
            { optionValue: "QuotationNO", optionName: "Quotation NO" },
            { optionValue: "EnquiryNO", optionName: "Enquiry NO" },
            { optionValue: "railRamp", optionName: "Rail Ramp Port" },
            { optionValue: "LoadingPort", optionName: "Loading Port" },
            { optionValue: "DestinationPort", optionName: "Destination Port" },
            { optionValue: "Shipping", optionName: "Shipping Line" },
            { optionValue: "Transporter", optionName: "Transporter" },
            { optionValue: "CNTNo", optionName: "Container No." },
            { optionValue: "BLNumber", optionName: "Line B/L#" },

        ];

        $scope.seachDateFilters = [
            { optionValue: "", optionName: "-Select-" },
            { optionValue: "ERD", optionName: "ERD" },
            { optionValue: "ETA", optionName: "ETA" },
            { optionValue: "ETS", optionName: "ETS" },
            { optionValue: "BookingDate", optionName: "Booking Date" },
            { optionValue: "EarliestPickupDate", optionName: "Earliest Pickup Date" },
            { optionValue: "DocumentCutOffDate", optionName: "Document CutOff Date" },
            { optionValue: "CargoCutOffDate", optionName: "Cargo CutOff Date" },
        ];

        $scope.seachInvoiceStatus = [
            { optionValue: "", optionName: "-Select-" },
            { optionValue: "MixedStatus", optionName: "MIXED STATUS" },
            { optionValue: "NotReadyForInvoicing", optionName: "NOT READY FOR INVOICING" },
            { optionValue: "Invoiced", optionName: "INVOICED" },
            { optionValue: "Void", optionName: "VOID" },
            { optionValue: "RecordCancelled", optionName: "RECORD CANCELLED" },
            { optionValue: "RecordRolledOver", optionName: "RECORD ROLLED OVER" },
            { optionValue: "ReadyForInvoicing", optionName: "READY FOR INVOICING" }
        ];

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
                if ($scope.entity.selectOption1 == "railRamp") {
                    lookupModule = "RailRamp";
                    routeType = "railRamp";
                    lookupField = "railRamp"
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
                if ($scope.entity.selectOption2 == "railRamp") {
                    lookupModule = "RailRamp";
                    routeType = "railRamp";
                    lookupField = "railRamp"
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
                if ($scope.entity.selectOption3 == "railRamp") {
                    lookupModule = "RailRamp";
                    routeType = "railRamp";
                    lookupField = "railRamp"
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
            if (lookupModule == "LGVWPort" || lookupModule == "RailRamp") {
                return $scope.callTypeahead(viewValue, lookupModule, lookupField, null).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.forEach(function (o) {
                            resultItem = {}
                            if (lookupModule == "LGVWPort") {
                                resultItem.name = o.name;
                                resultItem.portId = o.portId;
                                $scope.searchResult.push(resultItem)
                            }
                            if (lookupModule == "RailRamp") {
                                resultItem.name = o.railRamp;
                                resultItem.railId = o.railId;
                                $scope.searchResult.push(resultItem)
                            }
                        });
                        return $scope.searchResult;
                    }
                );
            }
            if (selectType == "htsCode" || selectType == "htsCode") {
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

                            resultItem.name = o.name;
                            resultItem.id = o.id;
                            resultItem.unit = o.unit;
                            resultItem.unit1 = o.unit1;
                            resultItem.description = o.description;
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
                            resultItem.code = o.code;
                            $scope.searchResult.push(resultItem);

                        });
                        return $scope.searchResult;

                    }
                );
            }

            if (selectType == "countryofDestination") {
                lookupModule = "Booking";
                lookupField = "countryofDestination";

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

                            resultItem.name = o.name;
                            resultItem.id = o.id;
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

                            resultItem.name = o.name;
                            resultItem.id = o.id;
                            $scope.searchResult.push(resultItem)

                        });
                        return $scope.searchResult;

                    }
                );
            }
        };

        $scope.selectDateOption = "-Select-";
        $scope.fromDate = "",
            $scope.toDate = "",
            $scope.galBookingStatusID = 0,
            $scope.departmentID = 0,
            $scope.searchOption1Value = "-Select-";
        $scope.searchBox1Value = "",
            $scope.searchOption2Value = "-Select-";
        $scope.searchBox2Value = "",
            $scope.searchOption3Value = "-Select-";
        $scope.searchBox3Value = ""
        $scope.isCurrentYear = true;
        $scope.isExcludeCancellRollover = true;
       
        $scope.searchParam = {
            optionDateValue: $scope.selectDateOption,
            fromDate: $scope.fromDate,
            toDate: $scope.toDate,
            galBookingStatusID: $scope.galBookingStatusID,
            departmentID: $scope.departmentID,
            searchOption1Value: $scope.searchOption1Value,
            searchBox1Value: $scope.searchBox1Value,
            searchOption2Value: $scope.searchOption2Value,
            searchBox2Value: $scope.searchBox2Value,
            searchOption3Value: $scope.searchOption3Value,
            searchBox3Value: $scope.searchBox3Value,
            isCurrentYear: $scope.isCurrentYear,
            isExcludeCancellRollover: $scope.isExcludeCancellRollover
        };

        $scope.clearDates = function (dateoption) {
            if (dateoption == "") {
                $scope.fromDate = null;
                
                $scope.toDate = null
            }
        };
        $scope.bookinglistTable = new NgTableParams(
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

                var dataitems = entityService.getExportRegisterList(listParams).then(
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
        $scope.performBookingSearch = function (source, selectDateOption, fromDate, toDate, galBookingStatusID, departmentID, searchOption1, searchBox1, searchOption2, searchBox2, searchOption3, searchBox3, isCurrentYear, isExcludeCancellRollover) {
            debugger;
            fromDate = $("[name=fromDate]").val();
            toDate = $("[name=toDate]").val();
            var action = source.currentTarget.attributes["action"].value;
            if (selectDateOption == null) {
                selectDateOption = "-Select-";
            }
            if (selectDateOption != null && selectDateOption != "-Select-" && selectDateOption != '') {

                if (fromDate == null || fromDate == '' || toDate == null || toDate == '') {
                    ngNotifier.error("Please enter valid From Date and To Date");
                    return;
                }
            }
            if (isCurrentYear == false && (fromDate == null || fromDate == '' || toDate == null || toDate == '')) {
                ngNotifier.error("Please enter valid From Date and To Date");
                return;
            }
            if ($scope.entity.selectOption1 == null) {
                $scope.entity.selectOption1 = "-Select-";
            }
            if ($scope.entity.selectOption2 == null) {
                $scope.entity.selectOption2 = "-Select-";
            }
            if ($scope.entity.selectOption3 == null) {
                $scope.entity.selectOption3 = "-Select-";
            }

            if ($scope.entity.searchBox1 == null) {
                searchBox1 = "";
            } else {
                searchBox1 = $scope.entity.searchBox1;
            }

            if ($scope.entity.searchBox2 == null) {
                searchBox2 = "";
            } else {
                searchBox2 = $scope.entity.searchBox2;
            }

            if ($scope.entity.searchBox3 == null) {
                searchBox3 = "";
            } else {
                searchBox3 = $scope.entity.searchBox3;
            }
            $scope.searchParam = {
                optionDateValue: selectDateOption,
                fromDate: fromDate,
                toDate: toDate,
                galBookingStatusID: galBookingStatusID,
                departmentID: departmentID,
                searchOption1Value: $scope.entity.selectOption1,
                searchBox1Value: searchBox1,
                searchOption2Value: $scope.entity.selectOption2,
                searchBox2Value: searchBox2,
                searchOption3Value: $scope.entity.selectOption3,
                searchBox3Value: searchBox3,
                isCurrentYear: isCurrentYear,
                isExcludeCancellRollover: isExcludeCancellRollover
            };
            $scope.bookinglistTable.reload();
        };

        $scope.exportRegisterReport = function (source, selectDateOption, fromDate, toDate, galBookingStatusID, departmentID, searchOption1, searchBox1, searchOption2, searchBox2, searchOption3, searchBox3, isCurrentYear, isExcludeCancellRollover) {

            var action = source.currentTarget.attributes["action"].value;
            if (selectDateOption == null) {
                selectDateOption = "-Select-";
            }
            if (selectDateOption != null && selectDateOption != "-Select-" && selectDateOption != '') {

                if (fromDate == null || fromDate == '' || toDate == null || toDate == '') {
                    ngNotifier.error("Please enter valid From Date and To Date");
                    return;
                }
            }
            if (isCurrentYear == false && (fromDate == null || fromDate == '' || toDate == null || toDate == '')) {
                ngNotifier.error("Please enter valid From Date and To Date");
                return;
            }

            if (fromDate == '') { fromDate = '1900-01-01' };
            if (toDate == '') { toDate = '1900-01-01' };
            if (searchOption1 == null) {
                searchOption1 = "-Select-";
            }
            if (searchBox1 == null) {
                searchBox1 = " ";
            }
            if (searchOption2 == null) {
                searchOption2 = "-Select-";
            }
            if (searchBox2 == null) {
                searchBox2 = " ";
            }
            if (searchOption3 == null) {
                searchOption3 = "-Select-";
            }
            if (searchBox3 == null) {
                searchBox3 = " ";
            }
            var reportParams = {
                optionDateValue: selectDateOption,
                fromDate: fromDate,
                toDate: toDate,
                galBookingStatusID: galBookingStatusID,
                departmentID: departmentID,
                searchOption1Value: searchOption1,
                searchBox1Value: searchBox1,
                searchOption2Value: searchOption2,
                searchBox2Value: searchBox2,
                searchOption3Value: searchOption3,
                searchBox3Value: searchBox3,
                isCurrentYear: isCurrentYear,
                isExcludeCancellRollover: isExcludeCancellRollover,
                SitId: $scope.$parent.selectedSiteId
            };
            entityService.exportRegisterReport(reportParams).then(
                function (output) {
                    var blobData = new Blob([output.data], { type: output.headers()["content-type"] });
                    var fileName = output.headers()["x-filename"];
                    saveAs(blobData, fileName);
                },
                function (output) {
                    ngNotifier.error(output);
                }
            );
        }

        $scope.UpdateSearch2 = function (selectOption1) {
            $scope.searchOptions2 = [];
            $scope.searchOptions3 = [];
            $scope.isOption2disabled = false;
            //$scope.isOption3disabled = true;
            $scope.searchOptions.forEach(function (item) {
                if (item.optionValue != selectOption1) {
                    $scope.searchOptions2.push(item);
                }
            });
        };
        $scope.UpdateSearch3 = function (selectOption1, selectOption2) {
            $scope.searchOptions3 = [];
            $scope.isOption3disabled = false;
            $scope.searchOptions.forEach(function (item) {
                if (item.optionValue != selectOption1 && item.optionValue != selectOption2) {
                    $scope.searchOptions3.push(item);
                }
            });
        };

        $scope.performBookingAction = function (source, fromList) {

            var action = source.currentTarget.attributes["action"].value;



            if (action == "save" && $scope.validateAction != undefined) {
                if (!$scope.validateAction(source)) {
                    return;
                }
            }

            if (fromList) {



            } else {
                initControls(action);
            }



            switch (action) {
                case "search":
                    filterList();
                    break;
                case "add":
                    lastAction = action;
                    $scope.entityId = 0;
                    $scope.entity = {};
                    $scope.entity.Commodities = [];
                    $scope.getDocumentAttachmentDetail(action, documentCommonID);
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
                    //$scope.showQuotationDetail(action, enquiryID, quotationID);
                    //$scope.showEnquiryDetail('viewDetail', $scope.entity.enquiryID, $scope.entity.isComplete);
                    //lastAction = "";
                    $scope.goBack();
                    break;
                case "delete":
                    remove();
                    //lastAction = "";
                    break;
                case "deleteBatch":
                    deleteShipmentDoc(enquiryID, quotationID);
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
                    break;

            }
        };

        $scope.deleteShipmentDoc = function (rownum, documentCommonID, filename) {
            $scope.entities = {};
            $scope.entities.documentCommonID = documentCommonID;
            $scope.entities.docName = filename;
            ngNotifier.confirm("Are you sure you want to DELETE Document?", null, function () {
                $scope.shipmentDocsList.splice(rownum, 1);
                entityService.deleteShipmentDoc($scope.entities).then(
                    function (output) {

                        ngNotifier.show(output.data);
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    });


            });

        };

        $scope.saveDocumentAttachement = function (source, fromList) {

            if ($scope.entity.fileNo == "" || $scope.entity.fileNo == null) {
                ngNotifier.error("Please enter System Ref");
                return;
            }

            if ($scope.entity.vessel == "" || $scope.entity.vessel == null) {
                ngNotifier.error("Please enter vessel");
                return;
            }
            if ($scope.entity.voyage == "" || $scope.entity.voyage == null) {
                ngNotifier.error("Please enter voyage");
                return;
            }
            var isValid = true;
            if ($scope.entity.customerfile != null) {

                if ($scope.entity.customerfile.length == 0) {
                    ngNotifier.error("Please attach atleast one file");
                    return;
                }
                else {
                    $scope.entity.customerfile.forEach(function (file) {

                        if (file) {

                            if (file.size > 10485760) {
                                ngNotifier.error("File cannot exceeds more than 10 MB size.");
                                isValid = false;
                            }
                            else if (file.type != "application/pdf" && file.type != "application/docx" && file.type != "application/doc" && file.type != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && file.type != "application/xlsx" && file.type != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                                ngNotifier.error("Please upload only these file types - pdf,docx,doc,xlsx");
                                isValid = false;
                                return;
                            }
                        }
                    });
                    if (isValid == true) {
                        var fileItem = {};
                        $scope.entity.shipmentDocsDTOList = [];
                        $scope.entity.customerfile.forEach(function (file) {
                            fileItem = {};
                            fileItem.docName = file.name;
                            $scope.entity.shipmentDocsDTOList.push(fileItem);
                        });
                    }

                    //$scope.entity.shipmentDocsDTOList = $scope.shipmentDocsList;
                }
            }
            if (isValid == true) {
                $scope.entity.galBookingStatusID = 18;  //Pending Shipper Confirmation Email
                $scope.entity.siteId = $scope.$parent.selectedSiteId;
                $scope.entity.createdBy = $scope.$parent.authentication.userId;
                $scope.entity.updatedBy = $scope.$parent.authentication.userId;
                entityService.saveDocumentAttachement($scope.entity).then(
                    function (output) {
                        $scope.documentID = output.data.data;
                        if ($scope.entity.customerfile != null) {
                            $scope.uploadAttachment($scope.documentID[0]);
                        }
                        $scope.entity = {};
                        $scope.actionRemarksList = [];
                        $scope.selectOption = "SystemRefNo";
                        $scope.searchBox = "";
                        $scope.bookinglistTable.reload();
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
                    });
            }
        };

        $scope.getDocumentAttachmentDetail = function (source, documentCommonID) {
            var action = source.currentTarget.attributes["action"].value;
            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls(action);
            $scope.entity = {};
            $scope.entity.documentCommonID = documentCommonID;

            entityService.getDocumentAttachmentDetail($scope.entity).then(
                function (output) {
                    if (output.data.resultId == 2005) {
                        ngNotifier.showError($scope.authentication, output);
                        $scope.logOut()
                    }
                    $scope.entity = output.data.data;
                    if ($scope.entity.isPublic == 1) {
                        $scope.entity.isPublic = "1";
                    }
                    else {
                        $scope.entity.isPublic = "0";
                    }
                    $scope.actionRemarksList = [];

                    if ($scope.entity.shipmentDocsDTOList != null) {
                        $scope.shipmentDocsList = $scope.entity.shipmentDocsDTOList;
                    }
                    if ($scope.entity.nextActionRemarksDTOList != null) {
                        $scope.actionRemarksList = $scope.entity.nextActionRemarksDTOList;
                    }
                    //$scope.afterGetDetail(action);
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };


        $scope.showNextActionRemarksModel = function (documentCommonID) {
            $scope.entities = {};
            $scope.entities.documentCommonID = documentCommonID;
            entityService.GetAllActionRemarks($scope.entities).then(
                function (output) {
                    $scope.actionRemarksList = [];
                    if (output.data.data.nextActionRemarksDTOList != null) {
                        $scope.actionRemarksList = output.data.data.nextActionRemarksDTOList;
                    }

                    //start model
                    var modalInstance = $uibModal.open({
                        animation: false,
                        backdrop: "static",
                        keyboard: false,
                        size: "lg",
                        templateUrl: "app/components/report/exportRegisterReport/nextActionRemarks.html",
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



                    //end model
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );


        };
        $scope.openCity = function (evt, cityName) {
            // Declare all variables
            var i, tabcontent, tablinks;

            // Get all elements with class="tabcontent" and hide them
            tabcontent = document.getElementsByClassName("tabcontent");
            for (i = 0; i < tabcontent.length; i++) {
                tabcontent[i].style.display = "none";
            }

            // Get all elements with class="tablinks" and remove the class "active"
            tablinks = document.getElementsByClassName("tablinks");
            for (i = 0; i < tablinks.length; i++) {
                tablinks[i].className = tablinks[i].className.replace(" active", "");
            }

            // Show the current tab, and add an "active" class to the button that opened the tab
            document.getElementById(cityName).style.display = "block";
            evt.currentTarget.className += " active";
            $(".lineNumberTitle").text('');
        };

        $scope.showViewDocumentModel = function (documentCommonID,type) {
            if (type == 'N') {
                if (confirm("Are you sure you want to refresh the data?")) {
                 
                }
                else
                    return false;
            }
            $scope.entities = {};
            $scope.entities.documentCommonID = documentCommonID;
            $scope.entities.siteId = $scope.$parent.selectedSiteId;
            $scope.entities.createdBy = $scope.$parent.authentication.userId;
            $scope.entities.email = $scope.$parent.authentication.userName;
            $scope.entities.etype = type;
            entityService.getExportRegisterDetailEES($scope.entities).then(
                function (output) {
                    if (output.data.resultId == 2005) {
                        ngNotifier.showError($scope.authentication, output);
                        $scope.logOut()
                    }
                    var object = output.data.data
                    $scope.entity = object;
                    $scope.entity.documentCommonID = object.documentCommonID;
                    $scope.entity.shipmentNumber = object.shipmentNumber
                    $scope.entity.emailResponseAddress = object.emailResponseAddress;
                    $scope.entity.filingOption = parseInt(object.filingOption);
                    $scope.entity.modeofTransport = parseInt(object.modeofTransport);
                    //$scope.entity.portofExport = parseInt(object.portofExport)
                    $scope.entity.portofExport = object.portofExport;
                    $scope.entity.portofExportCode = object.portofExportCode;
                    //$scope.entity.portofUnloading = parseInt(object.portofUnloading);
                    $scope.entity.portofUnloading = object.portofUnloading;
                    $scope.entity.portofUnloadingCode = object.portofUnloadingCode;
                    $scope.entity.departureDate = object.departureDate;
                    $scope.entity.originState = object.originState
                    $scope.entity.originStateCode = object.originStateCode

                    $scope.entity.countryofDestination = object.countryOfDestination == null ? '' : object.countryOfDestination;
                    $scope.entity.countryCode = object.countryCode == null ? '' : object.countryCode;
                    $scope.entity.inbondType = object.inbondType;
                    $scope.entity.foreginTradeZone = object.foreginTradeZone
                    $scope.entity.importEntry = object.importEntry;
                    $scope.entity.originalITN = object.originalITN;
                    $scope.entity.isRoutedTransaction = object.isRoutedTransaction;
                    $scope.entity.isUltimateConsigneeCompanies = object.isUltimateConsigneeCompanies;
                    $scope.entity.isHazardousMaterial = object.isHazardousMaterial;
                    $scope.entity.CommodityDetail = object.CommodityDetail;
                    $scope.entity.EquipmentLineDetail = object.EquipmentLineDetail;
                    $scope.entity.VinDetail = object.VinDetail;
                    // $scope.entity.VinComDetail = object.VinDetail;
                    //USPPI

                    $scope.entity.uSPPIIDNumberTypeId = object.usppiidNumberTypeId;
                    $scope.entity.uSPPIIDNumber = object.usppiidNumber;
                    $scope.entity.uSPPICompanyName = object.usppiCompanyName;
                    $scope.entity.iRSNumber = object.iRSNumber;
                    $scope.entity.uSPPIFirstName = object.usppiFirstName;
                    $scope.entity.uSPPILastName = object.usppiLastName;
                    $scope.entity.uSPPIPhoneNumber = object.usppiPhoneNumber;
                    $scope.entity.uSPPIAddressLine1 = object.usppiAddressLine1;
                    $scope.entity.uSPPIAddressLine2 = object.usppiAddressLine2;
                    $scope.entity.uSPPIPostalCodeId = object.usppiPostalCodeId;
                    $scope.entity.uSPPICity = object.usppiCity;
                    $scope.entity.uSPPIStateId = object.usppiStateId;
                    $scope.entity.uSPPIStateCode = object.usppiStateCode;
                    $scope.entity.uSPPIStateCode = object.usppiStateCode;
                    $scope.entity.uSPPIEmail = object.usppiEmail;

                    //Ultimate
                    $scope.entity.isSoldEnRoute = object.isSoldEnRoute;
                    $scope.entity.consigneeTypeId = object.consigneeTypeId;
                    $scope.entity.ultimateIdNumberTypeId = object.ultimateIdNumberTypeId;
                    $scope.entity.ultimateIdNumber = object.ultimateIdNumber;
                    $scope.entity.ultimateCompanyName = object.ultimateCompanyName;
                    $scope.entity.ultimateFirstName = object.ultimateFirstName;
                    $scope.entity.ultimateLastName = object.ultimateLastName;
                    $scope.entity.ultimatePhoneNumber = object.ultimatePhoneNumber;
                    $scope.entity.ultimateAddressLine1 = object.ultimateAddressLine1;
                    $scope.entity.ultimateAddressLine2 = object.ultimateAddressLine2;
                    $scope.entity.ultimateCountryId = object.ultimateCountryId;
                    $scope.entity.ultimateCountryCode = object.ultimateCountryCode;
                    $scope.entity.ultimatePostalCodeId = object.ultimatePostalCodeId;
                    $scope.entity.ultimateCity = object.ultimateCity;
                    $scope.entity.ultimateStateId = object.ultimateStateId;
                    $scope.entity.ultimateStateCode = object.ultimateStateCode;
                    $scope.entity.ultimateEmail = object.ultimateEmail;
                    //intermediate consignee
                    $scope.entity.interIdNumberTypeId = object.interIdNumberTypeId;
                    $scope.entity.interIdNumber = object.interIdNumber;
                    $scope.entity.interCompanyName = object.interCompanyName;
                    $scope.entity.interFirstName = object.interFirstName;
                    $scope.entity.interLastName = object.interLastName;
                    $scope.entity.interPhoneNumber = object.interPhoneNumber;
                    $scope.entity.interAddressLine1 = object.interAddressLine1;
                    $scope.entity.interAddressLine2 = object.interAddressLine2;
                    $scope.entity.interCountryId = object.interCountryId;
                    $scope.entity.interCountryCode = object.interCountryCode;
                    $scope.entity.interPostalCodeId = object.interPostalCodeId;
                    $scope.entity.interCity = object.interCity;
                    $scope.entity.interStateId = object.interStateId;
                    $scope.entity.interStateCode = object.interStateCode;
                    $scope.entity.interEmail = object.interEmail;
                    //Freight Forwarder
                    $scope.entity.freightIdNumberTypeId = object.freightIdNumberTypeId;
                    $scope.entity.freightIdNumber = object.freightIdNumber;
                    $scope.entity.freightCompanyName = object.freightCompanyName;
                    $scope.entity.freightFirstName = object.freightFirstName;
                    $scope.entity.freightLastName = object.freightLastName;
                    $scope.entity.freightPhoneNumber = object.freightPhoneNumber;
                    $scope.entity.freightAddressLine1 = object.freightAddressLine1;
                    $scope.entity.freightAddressLine2 = object.freightAddressLine2;
                    $scope.entity.freightPostalCodeId = object.freightPostalCodeId;
                    $scope.entity.freightCity = object.freightCity;
                    $scope.entity.freightStateId = object.freightStateId;
                    $scope.entity.freightStateCode = object.freightStateCode;
                    $scope.entity.freightEmail = object.freightEmail;
                    $scope.entity.carrierSCAC = object.carrierSCAC;
                    $scope.entity.conveyanceName = object.conveyanceName;
                    $scope.entity.transportationReferenceNumber = object.transportationReferenceNumber;
                    $scope.entity.aestype = object.aestype;
                    $scope.entity.exporterName = object.exporterName;
                    $scope.entity.exporterId = object.exporterId;
                    $scope.entity.exporterAddress = object.exporterAddress;
                    $scope.entity.etype = type;
                    $scope.entity.isuploaded = object.isuploaded;
                    $scope.entity.FileList = object.fileList;
                    //if ($scope.entity.id == 0) { $("#aestype").hide(); }
                    //else { $("#aestype").show(); }
                    var length = $scope.entity.commodityDetail.length;
                    if (length == 0) {
                        $scope.addNewLine();
                    }
                    $(".lineNumberTitle").text('');

                    if ($scope.entity.isuploaded == '1') {
                        $(".addload").hide();
                        $(".isload").show();
                    }
                    if ($scope.entity.isuploaded == '0') {
                        $(".isload").hide();
                        $(".addload").show();
                    }

                    $scope.onClickTab($scope.tabs[0]);
                    $scope.viewList = false;
                    $scope.page.urls.container = "app/components/ees/eessub/detail_ees.html";
                    initControls('add');


                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };
        $scope.addAesSubmission = function () {

            var errormsg = "";
            if ($scope.entity.isDraft == false) {
                if ($scope.entity.filingOption == undefined || $scope.entity.filingOption == "") {
                    errormsg="Please select filing option";
                }
                if ($scope.entity.portofExport == undefined || $scope.entity.portofExport == "") {
                    errormsg += "<br/>Please select Port of Export ";
                   // ngNotifier.error(errormsg);
                   // return;
                }
                if ($scope.entity.modeofTransport == undefined || $scope.entity.modeofTransport == "") {
                    errormsg += "<br/>Please select Mode of Transport.";
                    //return;
                }
                if ($scope.entity.portofUnloading == undefined || $scope.entity.portofUnloading == "") {
                    errormsg += "<br/>Please select Port of Unloading.";
                    //return;
                }
                if ($scope.entity.departureDate == undefined || $scope.entity.departureDate == "") {
                    errormsg += "<br/>Departure Date is required. ";
                    //return;
                }
                if ($scope.entity.originState == undefined || $scope.entity.originState == "") {
                    errormsg += "<br/>Please select Origin State.";
                    //return;
                }
                if ($scope.entity.countryofDestination == undefined || $scope.entity.countryofDestination == "") {
                    errormsg += "<br/>Please select Country of Destination.";
                    //return;
                }
                if ($scope.entity.inbondType == undefined || $scope.entity.inbondType == "") {
                    errormsg += "<br/>Please select Inbond Type.";
                    //return;
                }
                if ($scope.entity.exporterName == "" || $scope.entity.exporterName == undefined) {
                    $scope.entity.exporterId = "0";
                }
                if ($scope.entity.shipmentNumber == "" || $scope.entity.shipmentNumber == undefined) {
                    errormsg += "<br/>Shipment Reference Number is required.";
                   // return;
                }
                if ($scope.entity.emailResponseAddress == "" || $scope.entity.emailResponseAddress == undefined) {
                    errormsg += "<br/>Email Response Address is required.";
                    //return;
                }
                
                if ($scope.entity.originState == "" || $scope.entity.originState == undefined) {
                    errormsg += "<br/>Origin State is required.";
                    //return;
                }
                if ($scope.entity.countryofDestination == "" || $scope.entity.countryofDestination == undefined) {
                    errormsg += "<br/>Country of Destination is required.";
                   // return;
                }
                if ($scope.entity.isRoutedTransaction == "" || $scope.entity.isRoutedTransaction == undefined) {
                    errormsg += "<br/>Is this a Routed Transaction is required.";
                    //return;
                }
                if ($scope.entity.isUltimateConsigneeCompanies == "" || $scope.entity.isUltimateConsigneeCompanies == undefined) {
                    errormsg += "<br/>Are USPPI and Ultimate Consignee Companies related? is required.";
                    //return;
                }

                if ($scope.entity.isHazardousMaterial == "" || $scope.entity.isHazardousMaterial == undefined) {
                    errormsg += "<br/>Does shipment contain hazardous material? is required.";
                    //return;
                }

                //usspi

                if ($scope.entity.uSPPIIDNumberTypeId == "Please Select" || $scope.entity.uSPPIIDNumberTypeId == undefined) {
                    errormsg += "<br/>USPPI ID Number Type is required.";
                    //return;
                }
                if ($scope.entity.uSPPIIDNumber == "" || $scope.entity.uSPPIIDNumber == undefined) {
                    errormsg += "<br/>USPPI ID Number is required.";
                   // return;
                }
                if ($scope.entity.uSPPICompanyName == "" || $scope.entity.uSPPICompanyName == undefined) {
                    errormsg += "<br/>USPPI Company Name is required.";
                    //return;
                }
                if ($scope.entity.uSPPIFirstName == "" || $scope.entity.uSPPIFirstName == undefined) {
                    errormsg += "<br/>USPPI First Name is required.";
                   // return;
                }
                if ($scope.entity.uSPPILastName == "" || $scope.entity.uSPPILastName == undefined) {
                    errormsg += "<br/>USPPI Last Name is required.";
                    //return;
                }
                if ($scope.entity.uSPPIPhoneNumber == "" || $scope.entity.uSPPIPhoneNumber == undefined) {
                    errormsg += "<br/>USPPI Phone No is required.";
                    //return;
                }
                if ($scope.entity.uSPPIAddressLine1 == "" || $scope.entity.uSPPIAddressLine1 == undefined) {
                    errormsg += "<br/>USPPI Address line 1 is required.";
                    //return;
                }
                
                if ($scope.entity.uSPPIPostalCodeId == "" || $scope.entity.uSPPIPostalCodeId == undefined) {
                    errormsg += "<br/>USPPI Postal Code is required.";
                    //return;
                }
                if ($scope.entity.uSPPICity == "" || $scope.entity.uSPPICity == undefined) {
                    errormsg += "<br/>USPPI City is required.";
                    //return;
                }
                if ($scope.entity.uSPPIStateId == "" || $scope.entity.uSPPIStateId == undefined) {
                    errormsg += "<br/>USPPI State is required.";
                   // return;
                }

                // Ultimate Consignee
                if ($scope.entity.isSoldEnRoute == "" || $scope.entity.isSoldEnRoute == undefined) {
                    errormsg += "<br/>Sold En Route? is required.";
                    //return;
                }
                if ($scope.entity.consigneeTypeId == "" || $scope.entity.consigneeTypeId == undefined) {
                    errormsg += "<br/>Consignee Type is required.";
                    //return;
                }
                if ($scope.entity.ultimateIdNumberTypeId == "Please Select" || $scope.entity.ultimateIdNumberTypeId == undefined) {
                    errormsg += "<br/>Ultimate Consignee ID Number Type is required.";
                    //return;
                }
                
                if ($scope.entity.ultimateCompanyName == "" || $scope.entity.ultimateCompanyName == undefined) {
                    errormsg += "<br/>Ultimate Consignee Company Name is required.";
                   // return;
                }
                if ($scope.entity.ultimateFirstName == "" || $scope.entity.ultimateFirstName == undefined) {
                    errormsg += "<br/>USPPI First Name is required.";
                   // return;
                }
                if ($scope.entity.ultimateLastName == "" || $scope.entity.ultimateLastName == undefined) {
                    errormsg += "<br/>Ultimate Consignee Last Name is required.";
                   // return;
                }
                if ($scope.entity.ultimatePhoneNumber == "" || $scope.entity.ultimatePhoneNumber == undefined) {
                    errormsg += "<br/>Ultimate Consignee Phone No is required.";
                   // return;
                }
                if ($scope.entity.ultimateAddressLine1 == "" || $scope.entity.ultimateAddressLine1 == undefined) {
                    errormsg += "<br/>Ultimate Consignee Address line 1 is required.";
                    //return;
                }
                
                if ($scope.entity.ultimateCountryId == "" || $scope.entity.ultimateCountryId == undefined) {
                    errormsg += "<br/>Ultimate Consignee Country is required.";
                    //return;
                }
                if ($scope.entity.ultimatePostalCodeId == "" || $scope.entity.ultimatePostalCodeId == undefined) {
                    errormsg += "<br/>Ultimate Consignee Postal Code is required.";
                    //return;
                }
                if ($scope.entity.ultimateCity == "" || $scope.entity.ultimateCity == undefined) {
                    errormsg += "<br/>Ultimate Consignee City is required.";
                    //return;
                }
                if ($scope.entity.ultimateStateId == "" || $scope.entity.ultimateStateId == undefined) {
                    errormsg += "<br/>Ultimate Consignee State is required.";
                    //return;
                }


                //intermediate

                //if ($scope.entity.interIdNumberTypeId == "Please Select" || $scope.entity.interIdNumberTypeId == undefined) {
                //    errormsg += "<br/>Intermediate Consignee ID Number Type is required.";
                //    //return;
                //}
                //if ($scope.entity.interIdNumber == "" || $scope.entity.interIdNumber == undefined) {
                //    errormsg += "<br/>Intermediate Consignee ID Number is required.";
                //    //return;
                //}
                //if ($scope.entity.interCompanyName == "" || $scope.entity.interCompanyName == undefined) {
                //    errormsg += "<br/>Intermediate Consignee Company Name is required.";
                //    //return;
                //}
                //if ($scope.entity.interFirstName == "" || $scope.entity.interFirstName == undefined) {
                //    errormsg += "<br/>Intermediate Consignee First Name is required.";
                //   // return;
                //}
                //if ($scope.entity.interLastName == "" || $scope.entity.interLastName == undefined) {
                //    errormsg += "<br/>Intermediate Consignee Last Name is required.";
                //   // return;
                //}
                //if ($scope.entity.interPhoneNumber == "" || $scope.entity.interPhoneNumber == undefined) {
                //    errormsg += "<br/>Intermediate Consignee Phone No is required.";
                //    //return;
                //}
                //if ($scope.entity.interAddressLine1 == "" || $scope.entity.interAddressLine1 == undefined) {
                //    errormsg += "<br/>Intermediate Consignee Address line 1 is required.";
                //   // return;
                //}
               
                //if ($scope.entity.interCountryId == "" || $scope.entity.interCountryId == undefined) {
                //    errormsg += "<br/>Intermediate Consignee Country is required.";
                //    //return;
                //}
                //if ($scope.entity.interPostalCodeId == "" || $scope.entity.interPostalCodeId == undefined) {
                //    errormsg += "<br/>Intermediate Consignee Postal Code is required.";
                //    //return;
                //}
                //if ($scope.entity.interCity == "" || $scope.entity.interCity == undefined) {
                //    errormsg += "<br/>Intermediate Consignee City is required.";
                //   // return;
                //}
                //if ($scope.entity.interStateId == "" || $scope.entity.interStateId == undefined) {
                //    errormsg += "<br/>Intermediate Consignee State is required.";
                //    //return;
                //}

                //Freight Forwarder

                if ($scope.entity.freightIdNumberTypeId == "Please Select" || $scope.entity.freightIdNumberTypeId == undefined) {
                    errormsg += "<br/>Freight Forwarder ID Number Type is required.";
                   // return;
                }
                if ($scope.entity.freightIdNumber == "" || $scope.entity.freightIdNumber == undefined) {
                    errormsg += "<br/>Freight Forwarder ID Number is required.";
                    //return;
                }
                if ($scope.entity.freightCompanyName == "" || $scope.entity.freightCompanyName == undefined) {
                    errormsg += "<br/>Freight Forwarder Company Name is required.";
                    //return;
                }
                if ($scope.entity.freightFirstName == "" || $scope.entity.freightFirstName == undefined) {
                    errormsg += "<br/>Freight Forwarder First Name is required.";
                    //return;
                }
                if ($scope.entity.freightLastName == "" || $scope.entity.freightLastName == undefined) {
                    errormsg += "<br/>Freight Forwarder Last Name is required.";
                    //return;
                }
                if ($scope.entity.freightPhoneNumber == "" || $scope.entity.freightPhoneNumber == undefined) {
                    errormsg += "<br/>Freight Forwarder Phone No is required.";
                    //return;
                }
                if ($scope.entity.freightAddressLine1 == "" || $scope.entity.freightAddressLine1 == undefined) {
                    errormsg += "<br/>Freight Forwarder Address line 1 is required.";
                    //return;
                }
                
                if ($scope.entity.freightPostalCodeId == "" || $scope.entity.freightPostalCodeId == undefined) {
                    errormsg += "<br/>Freight Forwarder Postal Code is required.";
                    //return;
                }
                if ($scope.entity.freightCity == "" || $scope.entity.freightCity == undefined) {
                    errormsg += "<br/>Freight Forwarder City is required.";
                    //return;
                }
                if ($scope.entity.freightStateId == "" || $scope.entity.freightStateId == undefined) {
                    errormsg += "<br/>Freight Forwarder state is required.";
                    //return;
                }
                if ($scope.entity.conveyanceName == "" || $scope.entity.conveyanceName == undefined) {
                    errormsg += "<br/>Freight Forwarder email is required.";
                    //return;
                }
                // Transportation
                if ($scope.entity.carrierSCAC == "" || $scope.entity.carrierSCAC == undefined) {
                    errormsg += "<br/>Carrier SCAC/IATA is required.";
                    //return;
                }
                if ($scope.entity.conveyanceName == "" || $scope.entity.conveyanceName == undefined) {
                    errormsg += "<br/>Conveyance Name/Carrier Name is required.";
                    //return;
                }
                if ($scope.entity.transportationReferenceNumber == "" || $scope.entity.transportationReferenceNumber == undefined) {
                    errormsg += "<br/>Transportation Reference Number Name is required.";
                    //return;
                }
                  if (errormsg != "") {
                      ngNotifier.error(errormsg);
                      return;
                  }
                for (var i = 0; i < $scope.entity.commodityDetail.length; i++) {

                    if ($scope.entity.commodityDetail[i].siteId != "0") {
                    if ($scope.entity.commodityDetail[i].isType != "D") {
                        if ($scope.entity.commodityDetail[i].exportCode == '-' || $scope.entity.commodityDetail[i].exportCode == '-Select-' || $scope.entity.commodityDetail[i].exportCode == '' || $scope.entity.commodityDetail[i].exportCode == undefined) {
                            ngNotifier.error("Export Information Code is required in Line item");
                            return;
                        }
                        if ($scope.entity.commodityDetail[i].scheduleB == '') {
                            ngNotifier.error("Schedule B or HTS Number is required in Line item");
                            return;
                        }
                        if ($scope.entity.commodityDetail[i].firstQuantity == '') {
                            ngNotifier.error("First Quantity is required in Line item");
                            return;
                        }
                        if ($scope.entity.commodityDetail[i].firstUOM == '-Select-' || $scope.entity.commodityDetail[i].firstUOM == '' || $scope.entity.commodityDetail[i].firstUOM == undefined) {
                            ngNotifier.error("First UOM is required in Line item");
                            return;
                        }


                        if ($scope.entity.commodityDetail[i].originofGoods == '-Select-' || $scope.entity.commodityDetail[i].originofGoods == '' || $scope.entity.commodityDetail[i].originofGoods == undefined) {
                            ngNotifier.error("Origin of Goods is required in Line item");
                            return;
                        }
                        if ($scope.entity.commodityDetail[i].valueofGoods == '') {
                            ngNotifier.error("Value of Goods is required in Line item");
                            return;
                        }
                        if ($scope.entity.commodityDetail[i].shippingWeight == '') {
                            ngNotifier.error("Shipping Weight of is required in Line item");
                            return;
                        }

                        if ($scope.entity.commodityDetail[i].licenseTypeCode == '-Select-' || $scope.entity.commodityDetail[i].licenseTypeCode == '' || $scope.entity.commodityDetail[i].licenseTypeCode == undefined) {
                            ngNotifier.error("License Type Code is required in Line item");
                            return;
                        }
                        if ($scope.entity.commodityDetail[i].isGovermentAgency == '' || $scope.entity.commodityDetail[i].isGovermentAgency == undefined) {
                            ngNotifier.error("Does this filing require Participating Goverment agency data is required in Line item");
                            return;
                        }
                    }
                }

                }

                
            }

            entityService.addAesSubmission($scope.entity).then(

                function (output) {
                    if (output.data.resultId == 2005) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                    else {
                        $scope.bookinglistTable.reload();
                        $scope.goBack();
                        ngNotifier.show(output.data);
                    }
                });

        }
        $scope.selectTypeaheadAES = function ($item, $model, $label, source, stype) {
            debugger
            var lookupModule = null;
            var lookupIndex = null;
            var target = $(source.currentTarget);

            if (stype == "htsCode") {
                $scope.entity.ScheduleB = $item.id;
                $scope.entity.FirstUOM = $item.unit;
                $scope.entity.SecondUOM = $item.unit1;
                $scope.entity.CommodityDescription = $item.description;
                $scope.showVin($item.id);
            }
            if (stype == "portofExport") {
                $scope.entity.portofExportCode = $item.id;
            }
            if (stype == "portofUnloading") {
                $scope.entity.portofUnloadingCode = $item.id;
            }
            if (stype == "originState") {
                $scope.entity.originStateCode = $item.id;
            }
            if (stype == "countryofDestination") {
                $scope.entity.countryCode = $item.id;
            }
            if (stype == "uSPPIStateId") {
                $scope.entity.uSPPIStateCode = $item.id;
            }
            if (stype == "uSPPIStateCode") {
                $scope.entity.uSPPIStateCode = $item.id;
            }
            if (stype == "ultimateCountryCode") {
                $scope.entity.ultimateCountryCode = $item.id;
            }
            if (stype == "ultimateStateCode") {
                $scope.entity.ultimateStateCode = $item.id;
            }
            if (stype == "interCountryCode") {
                $scope.entity.interCountryCode = $item.id;
            }
            if (stype == "interStateCode") {
                $scope.entity.interStateCode = $item.id;
            }
            if (stype == "freightStateCode") {
                $scope.entity.freightStateCode = $item.id;
            }
            if (stype == "SIPLContact1USPPI")
            {
                $scope.entity.uSPPICompanyName = $item.name;
                $scope.entity.uSPPIFirstName =  $item.firstName;
                $scope.entity.uSPPILastName =   $item.lastName;
                $scope.entity.uSPPIAddressLine1 = $item.address;
                $scope.entity.uSPPIPostalCodeId = $item.zipCode;
                
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
        $scope.setLookups = function (source, lookup, output, index) {
            
            if (lookup == "SIPLContact") {
                $scope.entity.exporterAddress = output.data[0].address;
                $scope.entity.exporterId = output.data[0].contactID;
            }
            if (lookup == "SIPLContact1") {
                $scope.entity.ultimateAddressLine1 = output.data[0].address;
                if (output.data[0].contactPerson != "" && output.data[0].contactPerson != undefined) {
                    $scope.entity.ultimateFirstName = output.data[0].contactPerson.split(" ")[0];
                    $scope.entity.ultimateLastName = output.data[0].contactPerson.split(" ")[1];
                }
                //$scope.entity.ultimateFirstName = output.data[0].firstName;
                //$scope.entity.ultimateLastName = output.data[0].lastName;
                $scope.entity.ultimatePhoneNumber = output.data[0].telNo.replace('|', '').replace('-','');
                $scope.entity.ultimateCountryId = output.data[0].countryName;
                $scope.entity.ultimatePostalCodeId = output.data[0].zipCode;
                $scope.entity.ultimateCity = output.data[0].cityName;
                $scope.entity.ultimateStateId = output.data[0].stateName;
                $scope.entity.ultimateEmail = output.data[0].email;
                $scope.entity.ultimateIdNumber = output.data[0].idNumber;
                $scope.entity.ultimateIdNumberTypeId = output.data[0].idNumberType;
                
            }
            if (lookup == "SIPLContact2") {
                $scope.entity.interAddressLine1 = output.data[0].address;
                if (output.data[0].contactPerson != "" && output.data[0].contactPerson != undefined) {
                    $scope.entity.interFirstName = output.data[0].contactPerson.split(" ")[0];
                    $scope.entity.interLastName = output.data[0].contactPerson.split(" ")[1];
                }
               // $scope.entity.interFirstName = output.data[0].firstName;
              //  $scope.entity.interLastName = output.data[0].lastName;
                $scope.entity.interPhoneNumber = output.data[0].telNo.replace('|', '').replace('-','');
                $scope.entity.interCountryId = output.data[0].countryName;
                $scope.entity.interPostalCodeId = output.data[0].zipCode;
                $scope.entity.interCity = output.data[0].cityName;
                $scope.entity.interStateId = output.data[0].stateName;
                $scope.entity.interEmail = output.data[0].email;
                $scope.entity.interIdNumber = output.data[0].idNumber;
                $scope.entity.interIdNumberTypeId = output.data[0].idNumberType;
            }
            if (lookup == "SIPLContact3") {
                $scope.entity.exporterAddress = output.data[0].fullAddress;
                $scope.entity.exporterId = output.data[0].contactID;
            }
            
        };
        $(document).ready(function () {
            $("#reportTable").tableHeadFixer();
            setTimeout(function () {
                var _children = $('#tableContainer').children();
                $('#reportTable').insertBefore(_children[0]);
            }, 200);
        });
        $scope.UploadEssFile = function (documentCommonID, aesFileName) {
            if (aesFileName != "" && aesFileName != null) {
                var listParams = {
                    DocumentCommonID: documentCommonID.toString(),
                    FileName: aesFileName
                };
                entityService.UploadEssFile(listParams).then(
                    function (output) {
                        alert(output.data.output.messages[0]);
                    }
                    
                );
            }
            else { alert("File not generated yet. please generate file first"); }
        };
        
        $scope.searchValuesCompany = function (viewValue, selectOption) {
            var resultItem = {};
            var lookupField = "companyName";

            if (selectOption == "companyName") {

                var listParams = {
                    SiteId: $scope.selectedSite.siteId,
                    CwtId: $scope.userWorkTypeId,
                    ModuleId: $scope.page.moduleId,
                    PageIndex: 1,
                    PageSize: 25,
                    Sort: "{ \"" + lookupField + "\": \"asc\" }",
                    Filter: viewValue
                };
                return entityService.getCompanySearch(listParams).then(
                    function (output) {
                        $scope.searchResult = [];
                        output.data.data.forEach(function (o) {
                            resultItem = {}

                            resultItem.name = o.companyName;
                            resultItem.contactID = o.contactID;
                            resultItem.address = o.address;
                            resultItem.fullAddress = o.fullAddress;
                            resultItem.countryName = o.countryName
                            resultItem.stateName = o.stateName
                            resultItem.cityName = o.cityName
                            resultItem.zipCode = o.zipCode
                            resultItem.email = o.email
                            resultItem.telNo = o.telNo
                            resultItem.firstName = o.firstName
                            resultItem.lastName = o.lastName
                            resultItem.contactPerson = o.contactPerson
                            resultItem.idNumber = o.idNumber
                            resultItem.irsNumber = o.irsNumber
                            resultItem.idNumberType = o.idNumberType
                            $scope.searchResult.push(resultItem)

                        });
                        return $scope.searchResult;

                    }
                );
            }

        };
        $scope.DownloadEssFile = function (documentCommonID, aesFileName) {
            var aesFileName1 = aesFileName + ".out";

            entityService.DownloadEssFile(documentCommonID, aesFileName).then(
                function (output) {
                    var blob = new Blob([output.data], { type: 'application/octet-stream' });
                    saveAs(blob, aesFileName1);

                },
                function (output) {
                    alert("File not available");
                }
            );
        };
        $scope.SelectedRow = function (code) {
            for (var j = 0; j < $scope.ExpLicVal.length; j++) {
              
                if ($scope.ExpLicVal[j].code == code) {
                    $("#ExpLic").val($scope.ExpLicVal[j].licval);
                    $scope.entity.ExpLic = $scope.ExpLicVal[j].licval;
                }
            }
        }
        $scope.showViewFiles = function (documentCommonID,FileList) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/ees/eesSub/eesfilehistory.html",
                controller: "fileshistoryModelController",
                resolve: {
                    requestData: function () {
                        return {
                            siteId: $scope.$parent.selectedSiteId,
                            FileList: FileList
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {

                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };
        $scope.saveEssInDraft = function () {
            $scope.confirmationResult = false;
            var modalInstance = $uibModal.open({
                animation: true,
                size: "md",
                templateUrl: "app/components/ees/eesSub/confirmation.html",
                controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                    $scope.select = function (action) {
                        var outputData = {}
                        outputData.action = action;
                        $uibModalInstance.close(outputData);
                    };
                },
                resolve: {
                    requestData: function () {
                        return {

                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.action == "yes") {
                        $scope.entity.isDraft = true;
                        $scope.addAesSubmission();
                    }
                    else if (output.action == "no") {
                        $scope.entity.isDraft = false;
                        $scope.addAesSubmission();
                    }
                    else if (output == "close") {

                    }

                },
                function (output) {
                    ngNotifier.logError(output);
                });


        };
   
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("eessubController", controller);

});

