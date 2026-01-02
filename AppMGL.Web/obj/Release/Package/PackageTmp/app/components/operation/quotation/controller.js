"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "quotationService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.quotation;
        $scope.tabs = appUrl.quotation.tabs;
        $scope.$parent.pageTitle = "Create Quotation";
        $scope.$parent.breadcrumbs = ["Shipment", "Quotation", "Create Quotation"];
        //$scope.departments = [];
        $scope.selectedCarrierName;
        $scope.selectedCarrierId;

        $scope.enquiryID;
        $scope.deleteRemarks = "";
        $scope.isComplete = false;
        $scope.actionRemarksList = [];
        $scope.searchResult = [];
        $scope.emailMessage;
        $scope.truckingChargesList = [];
        $scope.carrierList = [];
        $scope.carrierChargesList = [];
        $scope.carrierRemarksList = [];
        $scope.carrierRates = [];
        $scope.selectedCarrierRates = [];
        $scope.emailRemarks = '';
        $scope.carrierRowIndix = 0;
        $scope.gridAction = '';
        $scope.selectedTransitTime = '';
        $scope.selectedFrequency = '';
        $scope.selectedRemarks = '';
        $scope.emailMessage = '';
        $scope.enquiryDetail = {};

        $scope.ckOptions = {
            toolbar: [
                { name: 'basicstyles', items: ['Bold', 'Italic', 'Underline'] },
                { name: 'insert', items: ['Image', 'Link', 'Unlink'] },
                { name: 'styles', items: ['Styles', 'Format', 'Font', 'FontSize'] },
                { name: 'colors', items: ['TextColor', 'BGColor'] },
                { name: 'indent', groups: ['list', 'indent', 'align'], items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] }
            ]
        };

        
        $scope.deleteReasons = [
                       { optionValue: "1", optionName: "Move To Enquiry" },
                       { optionValue: "2", optionName: "Cancel Quotation" }
        ];
        var lastAction = "";
        //#endregion

        //#region Lookup       

        $scope.lookups = { currencies: [], siplUsers: [], carriers: [], remarks: [], enquiryContainers: [], carrierAllCharges: [] };

        $scope.initDropdown = function () {

            //$scope.fetchLookupData("TradeService", 0, "TradeServiceId", "TradeServices", null);
            $scope.fetchLookupData("sipluser", 0, "name", "siplUsers", null);
            $scope.getCurrencies();
            $scope.getAllRemarks();
            $scope.getAllCarriers();
            
            $scope.getCarrierAllCharges();

        };
        
        
        $scope.setLookups = function (source, lookup, output, index) {
            if (lookup == "originCountry") {
                $scope.entity.originCountryID = output.data[0].countryId;
            }
            else if (lookup == "dischargeCountry") {
                $scope.entity.destinationCountryID = output.data[0].countryId;
            }
            
            else if (lookup == "origin") {
                $scope.entity.originID = output.data[0].portId;
                $scope.entity.originCountryID = output.data[0].countryId;
                $scope.entity.originCountry = output.data[0].countryName;
                
            }
            else if (lookup == "discharge") {
                $scope.entity.dischargeID = output.data[0].portId;
                $scope.entity.destinationCountryID = output.data[0].countryId;
                $scope.entity.dischargeCountry = output.data[0].countryName;               
            }
            
        };

        $scope.clearLookups = function (source, lookup, index) {

            
            if (lookup == "originCountry") {
                $scope.entity.originCountryID = null;
            }
            else if (lookup == "dischargeCountry") {
                $scope.entity.destinationCountryID = null;
            }
            else if (lookup == "originState") {
                $scope.entity.orignStateID = null;
            }
            else if (lookup == "dischargeState") {
                $scope.entity.destinationStateID = null;
            }
            else if (lookup == "originCity") {
                $scope.entity.originCityID = null;
            }
            else if (lookup == "dischargeCity") {
                $scope.entity.destinationCityID = null;
            }
            else if (lookup == "originPort") {
                $scope.entity.originID = null;
            }
            else if (lookup == "dischargePort") {
                $scope.entity.dischargeID = null;
            }
            else if (lookup == "RailRamp") {
                $scope.entity.orgnRailRampId = null;
            }
            else if (lookup == "Terminal") {
                $scope.entity.destnTerminalId = null;
            }
        };

        $scope.customClearLookups = function (source, lookupModule, lookupIndex, lookupField) {

            if (lookupModule == "originCountry" || lookupModule == "dischargeCountry" || lookupModule == "originPort" || lookupModule == "dischargePort" ) {

                if (lookupModule == "SIPLContact" || lookupModule == "RailRamp") {
                    if ($scope.entity[lookupField] == null || $scope.entity[lookupField] == "") {
                        $scope.clearLookups(source, lookupModule, lookupIndex);
                    }
                }
                else if ($scope.entity[lookupModule] == null || $scope.entity[lookupModule] == "") {
                    $scope.clearLookups(source, lookupModule, lookupIndex);
                }
            }
            if ($scope.isInvalidData == true) {
                $scope.clearLookups(source, lookupModule, lookupIndex);
            }
        };



        $scope.getDefaultEmailRemarks = function () {
            var emailTerms = '';
            emailTerms += "<div style='text-align:left;'>";
            emailTerms += "<h3 style='color:red;'>TERMS & CONDITIONS:</h3>";
            emailTerms += "<h4><span style='background-color: aqua;'>Above Rates Are Excluding of:</span></h4>";
            emailTerms += "<ul style='color: blue; font-size:12px;font-weight:normal; font-family:Arial,Helvetica,sans-serif;'>";
            emailTerms += "<li style='padding-bottom:10px;'> <p style='color: blue;'><b><b style='color: black;'> </b></b>Destination charges / Cargo Insurance / taxes / duties / Title validation.</p></li>";
            emailTerms += "<li style='padding-bottom:10px;'> <p style='color: blue;'><b><b style='color: black;'> </b></b>Demurrage / Detention / Storage / Overweight surcharges that may be incurred.</p></li>";
            emailTerms += "</ul>";
            emailTerms += "<h4><span style'background-color: deeppink;'>Rates are on:</span></h4>";
            emailTerms += "<ul style='color: blue; font-size:12px;font-weight:normal; font-family:Arial,Helvetica,sans-serif;'>";
            emailTerms += "<li style='padding-bottom:10px;'> <p style='color: blue;'><b><b style='color: black;'> </b></b>on PER UNIT basis.</p></li>";
            emailTerms += "<li style='padding-bottom:10px;'> <p style='color: blue;'><b><b style='color: black;'> </b></b>Prepaid basis.</p></li>";
            emailTerms += "<li style='padding-bottom:10px;'> <p style='color: blue;'><b><b style='color: black;'> </b></b>VATOS- Valid at time of shipping.</p></li>";
            emailTerms += "<li style='padding-bottom:10px;'> <p style='color: blue;'><b><b style='color: black;'> </b></b>All surcharges subject to change without any prior notice.</p></li>";
            emailTerms += "<li style='padding-bottom:10px;'> <p style='color: blue;'><b><b style='color: black;'> </b></b>On shippers stow, load and count basis</p></li>";
            emailTerms += "</ul>";
            emailTerms += "<h4><span style='background-color: yellow;'>Rates are subject to:</span></h4>";
            emailTerms += "<ul style='color: blue; font-size:12px;font-weight:normal; font-family:Arial,Helvetica,sans-serif;'>";
            emailTerms += "<li style='padding-bottom:10px;'>GRI (General Rate Increase) – if applicable .</li>";
            emailTerms += "<li style='padding-bottom:10px;'>Telex Fee (if applicable)</p></li>";
            emailTerms += "<li style='padding-bottom:10px;'>All Destination local charges on consignee’s account.</li>";
            emailTerms += "<li style='padding-bottom:10px;'>Availability of equipment/space</li>";
            emailTerms += "<li style='padding-bottom:10px;'>Hazardous cargo acceptance/approval.</li>";
            emailTerms += "<li style='padding-bottom:10px;'>FUEL MUST BE DRAINED and BATTERIES SHOULD BE DISCONNECTED .</li>";
            emailTerms += "<li style='padding-bottom:10px;'>Any additional charges at cost against official receipts.</li>";
            emailTerms += "</ul>";
            emailTerms += "<h4><span style='background-color: green;'>Trucking rates are subject to:</span></h4>";
            emailTerms += "<ul style='color: blue; font-size:12px;font-weight:normal; font-family:Arial,Helvetica,sans-serif;'>";
            emailTerms += "<li style='padding-bottom:10px;'>2 hours free live load and thereafter USD 85/hr. Drop n pick will be double unless otherwise advised .</li>";
            emailTerms += "<li style='padding-bottom:10px;'>Chassis availability from shipping line ...if chassis need to rent out then there will be extra charges by trucker which will be as per actual.</li>";
            emailTerms += "<li style='padding-bottom:10px;'>Chassis repositioning charges (if trucker has to pull out and return the chassis to different yard .charges will be additional).</li>";
            emailTerms += "<li style='padding-bottom:10px;'>Maximum pay loader per 20’ std = 44000 LBS ( 20000 KGS ) WITH TRI AXLE</li>";
            emailTerms += "<li style='padding-bottom:10px;'>All containers MUST observe standard US road weight limitations = MAX 44,000 LBS</li>";
            emailTerms += "<li style='padding-bottom:10px;'>Chassis rental fee for one day included. All charges thereafter will be charged @ $30/ DAY.</li>";
            emailTerms += "<li style='padding-bottom:10px;'>LOADING, LASHING, SECURING of cargo will be the responsibility of shipper.</li>";
            emailTerms += "<li style='padding-bottom:10px;'>STORAGE/DEMURRAGE CHARGES / CUSTOMS VACIS EXAMINATION fee OR if any other cost will be charged as per actual.</li>";
            emailTerms += "<li style='padding-bottom:10px;'>Pre-Pull charges if loading is schedule for early morning appointment .</li>";
            emailTerms += "<li style='padding-bottom:10px;'>If TWO stop loading is required, you will get only ONE Hour free at each stop means total 2 hrs. for both the stops .</li>";
            emailTerms += "</ul>";
            emailTerms += "<h3><span style='background-color: yellow;'><b>RATES ARE </b></span><b style='color: red; background-color: white;'>NOT </b><span style='background-color: yellow;'><b>SUBJECT TO ANY COMMISSION.</b></span></h3>";
            emailTerms += "<ul>";
            emailTerms += "<li> <p style='font-size: 16px; font-weight: bold; color: #009933; line-height: 30px; padding: 0 5px;'><span style='font-size: 13px; color: #000000;'>Air & Ocean NVO/Forwarder, Warehousing & Distribution</span></p><br /></li>";
            emailTerms += "</ul>";
            emailTerms += "<div style='text-align:left;'><p style='padding: 20px 0 0 0;'>";
            emailTerms += "<img alt='' src='Images/Icons/mgl_icon_full.png' /></p>";
            emailTerms += "<p style='font-size:12px; font-weight:normal; color:green; font-family:Arial,Helvetica,sans-serif; line-height: 25px;'>";
            emailTerms += "140 Ethel Road West; Unit 'S&T'<br />";
            emailTerms += "Piscataway, NJ 08854 - USA";
            emailTerms += "<br />";
            emailTerms += "<span style='text-decoration: underline; font-style: italic; font-size:12px;'>Phone : </span>+1-732-640-2365";
            emailTerms += "<br />";            
            emailTerms += "<span style='text-decoration: underline; font-style: italic; font-size:12px;'>FMC Lic. #</span> 023172NF /<span style='text-decoration: underline; font-style: italic; font-size:12px;'> IAC #:</span> NE1210010";
            emailTerms += "<br />";
            emailTerms += "</p>";
            emailTerms += "<h4 style='font-weight: bold;background-color: yellow;'>PROFESSIONAL ATTITUDE AT ALL ALTITUDES  </h4>";
            emailTerms += "</div>";
            emailTerms += "</div> ";
            return emailTerms;
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
        $scope.getCurrencies = function () {
            var getCurrencies = entityService.getCurrencies().then(
                function (output) {
                    $scope.lookups.currencies = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }
        $scope.getAllRemarks = function () {
            var getCurrencie = entityService.getAllRemarks().then(
                function (output) {
                    $scope.lookups.remarks = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }
        $scope.getEnquiryContainers = function (id) {
            var getContainers = entityService.getEnquiryContainers(id).then(
                function (output) {
                    $scope.lookups.enquiryContainers = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }
        $scope.getCarrierAllCharges = function () {
            var getCarrierCharges = entityService.getCarrierAllCharges().then(
                function (output) {
                    $scope.lookups.carrierAllCharges = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }

        //$scope.getEmailData = function (id) {
        //    var getEmaildetails = entityService.getEmailData(id).then(
        //        function (output) {
        //            $scope.emailData = output.data.data[0];
        //            if ($scope.emailData.emailTo != null) {
        //                $scope.entity.emailTo = $scope.emailData.emailTo;
        //            }
        //            if ($scope.emailData.emailcc != null) {
        //                $scope.entity.emailcc = $scope.emailData.emailcc;
        //            }
        //            if ($scope.emailData.emailBcc != null) {
        //                $scope.entity.emailBcc = $scope.emailData.emailBcc;
        //            }

        //            $scope.entity.emailSubject = $scope.emailData.emailSubject;
        //            $scope.emailMessage = $scope.emailData.emailContent;
        //        },
        //        function (output) {
        //            ngNotifier.showError($scope.authentication, output);
        //        }
        //    );
        //}
        $scope.getAllCarriers = function () {
            var getCurrencies = entityService.getAllCarriers().then(
                function (output) {
                    $scope.lookups.carriers = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }

        

        $scope.getCarrierName = function (selectedCarrier) {
            $scope.selectedCarrierName = $scope.entity.selectedCarrier.companyName;
            $scope.selectedCarrierId = $scope.entity.selectedCarrier.contactID;
        };

        

        //$scope.getEnquiryDetail = function (id) {
        //    $scope.entity.enquiryID = parseFloat(id);
        //    $scope.entity.isComplete = 1;
        //    $scope.entity.siteId = $scope.$parent.selectedSiteId;
        //    $scope.entity.userID = $scope.$parent.authentication.userId;
        //    if (id > 0) {
        //        entityService.getEnquiryDetail($scope.entity).then(
        //                 function (output) {
        //                     $scope.enquiryDetail = output.data.data;
        //                 },
        //                 function (output) {
        //                     ngNotifier.showError($scope.authentication, output);
        //                 }
        //             );

        //    }
        //}
        $scope.getCurrentDate = function () {
            //fill enquiry default date
            var today = new Date();
            var dd = today.getDate();
            var mm = today.getMonth() + 1; //January is 0!
            var yyyy = today.getFullYear();
            if (dd < 10) {
                dd = '0' + dd;
            }
            if (mm < 10) {
                mm = '0' + mm;
            }
            var today = mm + '/' + dd + '/' + yyyy;
            return today;
        }
        $scope.afterPerformAction = function (source, fromList) {
            var action = source.currentTarget.attributes["action"].value;
            switch (action) {
                case "add":

                    break;
                case "edit":
                    //$scope.getcarrierAllRates($scope.entity.enquiryID);
                    $scope.getEnquiryContainers($scope.entity.enquiryID);


                    break;

            }
        };

        $scope.afterGetDetail = function (action) {
            if ($scope.entity.quotationID > 0)
                $scope.isComplete = false;
            else
                $scope.isComplete = true;
            if (action == 'edit') {
               
                if ($scope.truckingChargesList.length > 0) {
                    $scope.truckingChargesList.forEach(function (item) {
                        if (item.trkBuyingRate > 0 || item.trkSellingRate > 0) {
                            $scope.entity.isTruckingExists = true;
                            return;
                        }
                    });
                }               
            }
        };

        $scope.getLatestQuotationNo = function () {

            var getEnquirynoValue = entityService.getQuotationNo().then(
                function (output) {
                    $scope.entity.quotationNo = output.data.data.enquiryNo;

                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
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
                if (dashboardOption == 'QYTD') {
                    $scope.dashboardOption = "QYTD";
                }
                if (dashboardOption == 'QMTD') {
                    $scope.dashboardOption = "QMTD";
                }
                if (dashboardOption == 'QPAU') {
                    $scope.dashboardOption = "QPAU";
                }
                if (dashboardOption == 'QPAF') {
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
            dashboardOption: $scope.dashboardOption
        };
        //#region Methods
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

                var dataitems = entityService.list(listParams).then(
                    function (output) {
                        $scope.validateUser(output);
                        $scope.items = output.data.data;
                        params.total(output.data.count);
                        $scope.$parent.pageTitle = "Create Quotation";
                        //document.getElementById('divAdd').hidden = "true";                        
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                    }
                );
            }
        });
        //#endregion

        $scope.performQuotationSearch = function (source, selectOption, searchBox) {

            var action = source.currentTarget.attributes["action"].value;
            $scope.searchParam = {
                optionValue: selectOption,
                seachValue: searchBox,
                dashboardOption: ""
            };
            $scope.quotationlistTable.reload();
        };
        $scope.approveQuotation = function (source, fromList) {
            var entities = [];
            $scope.items.forEach(function (item) {
                if (item.selected) {
                    item.isDraft = false
                    item.isApproved = false
                    item.isBooked = false
                    entities.push(item);
                }
            });
            if (entities.length === 0) {
                ngNotifier.info("Please, select atleast one record to perform action.");
            }
            else {
                ngNotifier.confirm("Are you sure, do you want to Approve for Capturing Shipper's choice?", null, function () {
                    entityService.approveQuotation(entities).then(
                            function (output) {
                                $scope.entity = {};
                                //$scope.enquirylistTable.reload();

                                $scope.goBack();
                                ngNotifier.show(output.data);
                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });
                    
                    $scope.$parent.pageTitle = "Pending Shipper Approval";
                    $scope.$parent.breadcrumbs = ["Shipment - Quotation - Pending Shipper Approval"];

                    $location.path("/operation/quotationApproval");
                });
            }
        }

        $scope.saveQuotation = function (source, fromList) {

            $scope.$broadcast("show-errors-check-validity");           
            if ($scope.entity.validTill == null) {
                ngNotifier.error("Please select Valid Till date");
                return;
            }
            
            if ($scope.entity.nextActionRemarks != null) {
                if ($scope.entity.nextActionDueDate != null)
                {
                    if ($scope.entity.nextActionRemarks.trim() != '' && $scope.entity.nextActionDueDate=='') {
                        ngNotifier.error("Please select Next Action Due date");
                        return;
                    }
                }
                else {
                    ngNotifier.error("Please select Next Action Due date");
                    return;
                }
            }
            if ($scope.entity.nextActionDueDate != null) {
                if ($scope.entity.nextActionRemarks != null) {
                    if ($scope.entity.nextActionDueDate != '' && $scope.entity.nextActionRemarks.trim() == '') {
                        ngNotifier.error("Please Enter Next Action Remarks");
                        return;
                    }
                }
                else {
                    ngNotifier.error("Please select Next Action Remarks");
                    return;
                }
            }
            if ($scope.carrierList == null || $scope.carrierList.length == 0)
            {
                ngNotifier.error("Please add valid Carriers");
                return;
            }
            $scope.entity.truckingChargesList = $scope.truckingChargesList;
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.createdBy = $scope.$parent.authentication.userId;
            $scope.entity.updatedBy = $scope.$parent.authentication.userId;
            //$scope.entity.CarrierDTOList = $scope.carrierList;
            
            //$scope.carrierRemarksList.forEach(function (i) {
            //    $scope.carrierList.forEach(function (j) {
            //        if(j.carrierId == i.carrierId)
            //        {
            //            j.carrierRemarksDTOList.push(i)
            //        }
            //    });
                
            //});
            //$scope.carrierChargesList.forEach(function (i) {
            //    $scope.carrierList.forEach(function (j) {
            //        if (j.carrierId == i.carrierId) {
            //            j.CarrierChargesDTOList.push(i)
            //        }
            //    });

            //});
            
            $scope.entity.carrierDTOList = $scope.carrierList;
           
            //$scope.entity.CarrierDTOList.CarrierChargesDTOList = $scope.carrierChargesList;
            //$scope.entity.CarrierRemarksDTOList = $scope.carrierRemarksList;
            //$scope.entity.enquiryID = 
            entityService.saveQuotation($scope.entity).then(
                function (output) {
                    $scope.enquiryID = output.data.data;
                    $scope.entity = {};
                    $scope.carrierList = [];
                    $scope.carrierChargesList = [];
                    $scope.carrierRemarksList = [];
                    $scope.carrierRates = [];
                    $scope.selectedCarrierRates = [];
                    $scope.quotationlistTable.reload();
                    $scope.$parent.pageTitle = "Create Quotation";
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
        };
        $scope.sendEmail = function (source, fromList) {
            if ($scope.entity.emailTo == undefined || $scope.entity.emailTo == "") {
                ngNotifier.error("Please enter To Emailid");
                return;
            }
            if ($scope.entity.emailSubject == undefined) {
                ngNotifier.error("Please enter Email Subject");
                return;
            }
            var emailEntity = {};            
            emailEntity.emailTo = $scope.entity.emailTo;
            if ($scope.entity.emailcc != '') {
                emailEntity.emailcc = $scope.entity.emailcc;
            }
            if ($scope.entity.emailBcc != '') {
                emailEntity.emailBcc = $scope.entity.emailBcc;
            }
            emailEntity.emailSubject = $scope.entity.emailSubject;
            emailEntity.emailBody = $scope.entity.emailBody + "\n\n" + $scope.emailMessage + "\n\n" + $scope.emailRemarks;

            emailEntity.quotationID = $scope.entity.quotationID;
            entityService.sendEmail(emailEntity).then(
                function (output) {
                    $scope.entity = {
                    };
                    $scope.carrierList = [];
                    $scope.carrierChargesList = [];
                    $scope.carrierRemarksList = []
                    $scope.quotationlistTable.reload();
                    $scope.$parent.pageTitle = "Create Quotation";
                    $scope.goBack();
                    ngNotifier.show(output.data);
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                });

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

        var switchTab = function (title, action) {

            $scope.tabs.forEach(function (tab) {
                tab.active = false;
                tab.disabled = ((action === "add" || action === "copy" || action === "edit") && tab !== $scope.tabs[0]);
                if (tab.title === title) {
                    tab.active = true;
                }
            });
        };

        var copyEnquiry = function ()
        {
            
            return true;
        }


        var removeBatch = function (enquiryID, quotationID) {
            var entity = {};
            entity.enquiryID = enquiryID;
            entity.quotationID = quotationID;

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/operation/quotation/cancelRemarks.html",
                controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                    $scope.deleteReasons = [
                        { optionValue: "1", optionName: "Move To Enquiry" },
                        { optionValue: "2", optionName: "Cancel Quotation" }
                    ];
                    $scope.deleteReason = "1";
                    $scope.select = function (action) {
                        //$scope.deleteRemarks = deleteRemarks.value;

                        if ((action == 'delete') && $scope.deleteRemarks == null) {
                            ngNotifier.error("Please Enter Delete Remarks");
                            return;
                        }
                        var outputData = {}
                        outputData.remarks = $scope.deleteRemarks;
                        outputData.deleteReason = $scope.deleteReason;
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
                        if (output.deleteReason == "1") {
                            entity.isCancelled = "No";
                        }
                        else {
                            entity.isCancelled = "Yes";
                        }

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

        };

        var viewDetail = function () {
            $scope.viewList = false;
            $scope.page.urls.container = "app/views/shared/container.html";
            $scope.entity = {};
        };

        $scope.showEnquiryDetail = function (action, id, isComplete) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                windowClass: "modal-lg-custom",
                templateUrl: "app/components/operation/quotation/enquirydetail.html",
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
                             if ($scope.entity.quotationID == 0){
                                 $scope.$parent.pageTitle = "Create Quotation - Pending for Quote Preparation";
                             }
                             else if ($scope.entity.mailSend == '1'){
                                 $scope.$parent.pageTitle = "Create Quotation - Shipper's Approval Awaited";
                             }
                             else if ($scope.entity.mailSend == '') {
                                 $scope.$parent.pageTitle = "Create Quotation - Quotations Ready for Sending to Shipper";
                             }
                            
                             $scope.carrierList = [];
                             $scope.carrierChargesList = [];
                             $scope.carrierRemarksList = [];
                             $scope.truckingChargesList = [];
                             $scope.carrierRates = [];
                             $scope.selectedCarrierRates = [];
                             $scope.showPickupType($scope.entity.pickupType)
                             if ($scope.entity.emailRemarks != null) {
                                 if ($scope.entity.emailRemarks != "") {
                                     $scope.emailRemarks = $scope.entity.emailRemarks.replace("&lt;%=imgpath %&gt;&lt;%=ProjectName%&gt;/images/logo.jpg", "Images/Icons/mgl_icon_full.png");
                                     $scope.entity.emailRemarks = $scope.emailRemarks;
                                 }
                                 else {
                                     $scope.emailRemarks = $scope.getDefaultEmailRemarks();
                                     $scope.entity.emailRemarks = $scope.emailRemarks;
                                 }
                             }
                             else {
                                 $scope.emailRemarks = $scope.getDefaultEmailRemarks();
                                 $scope.entity.emailRemarks = $scope.emailRemarks;
                             }
                             if ($scope.entity.nextActionRemarksDTOList != null)
                             {
                                 $scope.actionRemarksList = $scope.entity.nextActionRemarksDTOList;
                             }
                             if ($scope.entity.quotationID > 0)
                             {
                                 //$scope.getEmailData($scope.entity.quotationID)
                                 if($scope.entity.emailMessageDTO != null){
                                     if ($scope.entity.emailMessageDTO.emailTo != null) {
                                         $scope.entity.emailTo = $scope.entity.emailMessageDTO.emailTo;
                                     }
                                     if ($scope.entity.emailMessageDTO.emailcc != null) {
                                         $scope.entity.emailcc = $scope.entity.emailMessageDTO.emailcc;
                                     }
                                     if ($scope.entity.emailMessageDTO.emailBcc != null) {
                                         $scope.entity.emailBcc = $scope.entity.emailMessageDTO.emailBcc;
                                     }

                                 $scope.entity.emailSubject = $scope.entity.emailMessageDTO.emailSubject;
                                 $scope.emailMessage = $scope.entity.emailMessageDTO.emailContent;
                             }
                             $scope.tabs[1].disabled = false;
                         }
                         else {
                             $scope.tabs[1].disabled = true;
                         }

                         //$scope.getAllCarriers($scope.entity.quotationID)
                         $scope.carrierList = [];
                         if ($scope.entity.carrierDTOList != null) {
                             if ($scope.entity.carrierDTOList.length > 0) {
                                 $scope.carrierList = $scope.entity.carrierDTOList;
                             }
                         }
                         $scope.truckingChargesList = [];
                         if ($scope.entity.truckingChargesList != null) {
                             if ($scope.entity.truckingChargesList.length > 0) {
                                 $scope.truckingChargesList = $scope.entity.truckingChargesList;
                             }
                         }
                         $scope.carrierChargesList = [];
                         $scope.carrierRemarksList = []
                         $scope.afterGetDetail(action);
                     },
                     function (output) {
                         ngNotifier.showError($scope.authentication, output);
                     }
                 );

            }
            else {
                $scope.$parent.pageTitle = "Create Quotation";
                $scope.goBack();
            }
        };

        $scope.performQuotationAction = function (source, fromList, enquiryID, quotationID) {

            var action = source.currentTarget.attributes["action"].value;

            //$scope.$broadcast("show-errors-check-validity");

            //if (action != "cancel" && $scope.form.detail != undefined && $scope.form.detail.$invalid) {
            //    if ($scope.form.detail.$error.required != undefined && $scope.form.detail.$error.required.length > 0) {
            //        ngNotifier.error("Required Field(s) are missing data.");
            //    }
            //    else if ($scope.form.detail.usrPwdC.$invalid) {
            //        ngNotifier.error("Password do not match with Confirm Password.");
            //    }
            //    return;
            //}

            if (action == "save" && $scope.validateAction != undefined) {
                if (!$scope.validateAction(source)) {
                    return;
                }
            }

            if (fromList) {
                if (action == "copy") {
                    var entities = [];
                    $scope.items.forEach(function (item) {
                        if (item.selected) {
                            entities.push(item);
                        }
                    });
                    if (entities.length == 0 || entities.length > 1) {
                        ngNotifier.error("Please, select one record to perform action.");
                        return false;
                    }
                    else {
                        $scope.showQuotationDetail(action, enquiryID, quotationID);
                    }
                }
                else {
                    $scope.showQuotationDetail(action, enquiryID, quotationID);
                }
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
                    lastAction = action;
                    //$scope.entity.enquiryID = 0;                    
                    break;
                    //lastAction = 'copy';
                    //$scope.entityId = 0;
                    //$scope.entity = {};
                    //$("input[input-date]").each(function (index, element) { $(element).val(null); });
                    break;
                case "edit":
                    lastAction = action;
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
                    lastAction = "";
                    break;
                case "delete":
                    remove();
                    lastAction = "";
                    break;
                case "deleteBatch":
                    removeBatch(enquiryID, quotationID);
                    lastAction = "";
                    break;
                case "verify":
                case "activate":
                case "deactivate":
                    $scope.changeStatus(action);
                    lastAction = "";
                    break;
                default:
                    lastAction = "";
                    break;
            }

            if ($scope.afterPerformAction != undefined) {
                $scope.afterPerformAction(source, fromList);
            }
        };
        $scope.showPickupType = function (pickupValue) {
            if (pickupValue == "0") {
                $scope.isTruckExistsLBLVisible = true;
                $scope.isTruckExistsVisible = true;
                $scope.isTruckChargesLBLVisible = true;
                //$scope.isBuyingLBLVisible = true;
                //$scope.istruckBuyingVisible = true;
                //$scope.isSellingLBLVisible = true;
                //$scope.istruckSellingVisible = true;
                $scope.isTruckRemarksLBLVisible = true;
                $scope.istruckingRemarksVisible = true;
                $scope.iscurrencyLBLVisible = false;
                $scope.iscurrencyVisible = false;
            }
            if (pickupValue == "1") {
                $scope.isTruckExistsLBLVisible = false;
                $scope.isTruckExistsVisible = false;
                $scope.isTruckChargesLBLVisible = false;
                //$scope.isBuyingLBLVisible = false;
                //$scope.istruckBuyingVisible = false;
                //$scope.isSellingLBLVisible = false;
                //$scope.istruckSellingVisible = false;
                $scope.isTruckRemarksLBLVisible = false;
                $scope.istruckingRemarksVisible = false;
                $scope.iscurrencyLBLVisible = false;
                $scope.iscurrencyVisible = false;
            }
            if (pickupValue == "2") {
                $scope.isTruckExistsLBLVisible = true;
                $scope.isTruckExistsVisible = true;
                $scope.isTruckChargesLBLVisible = true;
                //$scope.isBuyingLBLVisible = true;
                //$scope.istruckBuyingVisible = true;
                //$scope.isSellingLBLVisible = true;
                //$scope.istruckSellingVisible = true;
                $scope.isTruckRemarksLBLVisible = true;
                $scope.istruckingRemarksVisible = true;
                $scope.iscurrencyLBLVisible = true;
                $scope.iscurrencyVisible = true;
            }
            if (pickupValue == "3") {
                $scope.isTruckExistsLBLVisible = false;
                $scope.isTruckExistsVisible = false;
                $scope.isTruckChargesLBLVisible = false;
                //$scope.isBuyingLBLVisible = false;
                //$scope.istruckBuyingVisible = false;
                //$scope.isSellingLBLVisible = false;
                //$scope.istruckSellingVisible = false;
                $scope.isTruckRemarksLBLVisible = false;
                $scope.istruckingRemarksVisible = false;
                $scope.iscurrencyLBLVisible = true;
                $scope.iscurrencyVisible = true;
            }
            if (pickupValue == "4") {
                $scope.isTruckExistsLBLVisible = false;
                $scope.isTruckExistsVisible = false;
                $scope.isTruckChargesLBLVisible = false;
                //$scope.isBuyingLBLVisible = false;
                //$scope.istruckBuyingVisible = false;
                //$scope.isSellingLBLVisible = false;
                //$scope.istruckSellingVisible = false;
                $scope.isTruckRemarksLBLVisible = false;
                $scope.istruckingRemarksVisible = false;
                $scope.iscurrencyLBLVisible = false;
                $scope.iscurrencyVisible = false;
            }
        }

        $scope.showCarrierModel = function (action) {
            var isCarrierExists = false;
            if (action == 'add') {
                $scope.gridAction = 'add';
                //$scope.carrierAllRates.forEach(function (o) {
                //    o.buyingRate = 0;
                //    o.sellingRate = 0;
                //    o.isSelected = false;
                //});

                if ($scope.entity.validTill == null) {
                    ngNotifier.error("Please select Valid Till date");
                    return;
                }

                if ($scope.entity.selectedCarrier == null || $scope.entity.selectedCarrier == undefined) {
                    ngNotifier.error("Please select Carrier");
                    return;
                }
                else {
                    if ($scope.entity.selectedCarrier.contactID == 0) {
                        ngNotifier.error("Please select Carrier");
                        return;
                    } else {
                        $scope.selectedCarrierName = $scope.entity.selectedCarrier.companyName;
                        $scope.selectedCarrierId = $scope.entity.selectedCarrier.contactID;
                    }
                }

                $scope.carrierList.forEach(function (o) {
                    if (o.carrierID == $scope.entity.selectedCarrier.contactID) {
                        isCarrierExists = true;
                        ngNotifier.error("This carrier is already exists");
                        return;
                    }
                });

            }
            if (!isCarrierExists) {
                var modalInstance = $uibModal.open({
                    animation: false,
                    backdrop: "static",
                    keyboard: false,
                    size: "lg",
                    templateUrl: "app/components/operation/quotation/addCarrier.html",
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
                        
                        $scope.carrierRowIndix = requestData.carrierRowIndix,
                        $scope.enquiryContainers = requestData.enquiryContainers,
                        $scope.carrierAllCharges = requestData.carrierAllCharges
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
                                var isOceanFrieghtExists = false;
                                var count = 0;
                                $scope.enquiryContainers.forEach(function (containerItem) {
                                    if (count > 0) {
                                        if (isOceanFrieghtExists == false) {
                                            return;
                                        }
                                        else {
                                            isOceanFrieghtExists = false;
                                        }
                                    }
                                    $scope.carrierRates.forEach(function (rateItem) {
                                        if (containerItem.containerTypeId == rateItem.fkContainerID && rateItem.refName == 'Ocean Frieght') {
                                            isOceanFrieghtExists = true;
                                            return;
                                        }
                                    });
                                    if (isOceanFrieghtExists == false) {
                                        count = count + 1;
                                        return;
                                    }
                                    count = count + 1;
                                });
                                if (isOceanFrieghtExists == false) {
                                    ngNotifier.error("Please add Ocean Frieght charges for all the containers");
                                    return;
                                }
                            }
                            var carrierItem = {};
                            var carrierRemarksDTOList = [];
                            var carrierChargesDTOList = [];
                            carrierItem.carrierName = $scope.carrierName;
                            carrierItem.carrierID = $scope.carrierID;
                            carrierItem.quotationCarrierId = 0;
                            carrierItem.startDate = new Date();
                            carrierItem.endDate = $scope.validTillDate
                            carrierItem.approved = true;
                            carrierItem.remarks = $scope.remarks
                            carrierItem.transitTime = $scope.transitTime;
                            carrierItem.frequency = $scope.frequency;
                            carrierItem.carrierRemarksDTOList = carrierRemarksDTOList;
                            carrierItem.carrierChargesDTOList = carrierChargesDTOList;

                            var outputData = {}
                            if (action == 'update') {
                                var carrierChargesList = []
                                var carrierChargeItem = {}
                                var containerId;
                                $scope.carrierRates.forEach(function (item) {

                                    //if ((item.isSelected && item.refName != 'Total') || item.refName == 'Ocean Frieght') {
                                    //    if (item.refName == 'Ocean Frieght') {
                                    //        containerId = item.refId;
                                    //    }
                                    carrierChargeItem.fkContainerID = item.fkContainerID;
                                    carrierChargeItem.containerTypeName = item.containerTypeName;
                                    carrierChargeItem.carrierID = $scope.carrierID;
                                    carrierChargeItem.refId = item.refId;
                                    carrierChargeItem.refName = item.refName;
                                    carrierChargeItem.buyingRate = item.buyingRate;
                                    carrierChargeItem.sellingRate = item.sellingRate;
                                    carrierChargeItem.isApplicable = item.isApplicable;
                                    carrierItem.carrierChargesDTOList.push(carrierChargeItem);
                                    carrierChargeItem = {};
                                    //}
                                    //if (item.refName == 'Total') {
                                    //    containerId = 0;
                                    //}
                                });

                                

                                outputData.carrierItem = carrierItem;
                                outputData.action = 'update';
                            }
                            else {
                                outputData.action = 'close';
                            }
                            $uibModalInstance.close(outputData);
                        };
                        $scope.addChargesRow = function () {
                            var chargesItem = {};
                            //if (chargesItem.description == 'Ocean Frieght') {
                            //    containerId = 343;
                            //}
                            if ($scope.buyingRate == null || $scope.buyingRate == '' || $scope.sellingRate == null || $scope.sellingRate == '')
                            {
                                ngNotifier.error("Please enter valid Buying Rate / Selling Rate");
                                return;
                            }
                            chargesItem.fkContainerID = $scope.containerItem.containerTypeId;;
                            chargesItem.containerTypeName = $scope.containerItem.name;
                            chargesItem.carrierID = $scope.carrierID;
                            chargesItem.refId = $scope.chargeItem.tradeServiceId;
                            chargesItem.refName = $scope.chargeItem.description;
                            chargesItem.buyingRate = $scope.buyingRate;
                            chargesItem.sellingRate = $scope.sellingRate;
                            chargesItem.isApplicable = 1;
                            $scope.carrierRates.push(chargesItem)
                        };
                        $scope.removeChargesRow = function (rownum) {
                            $scope.carrierRates.splice(rownum, 1);
                            //calculateAirTotal();
                        }

                        $scope.getContainerdetail= function(item)
                        {

                        }
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
                                carrierRowIndix: $scope.carrierRowIndix,
                                enquiryContainers: $scope.lookups.enquiryContainers,
                                carrierAllCharges: $scope.lookups.carrierAllCharges,
                            };
                        }
                    }
                });
            }
            if (!isCarrierExists) {
                modalInstance.result.then(
                    function (output) {
                        if (output.action == "update") {
                            //$scope.carrierChargesList = output.carrierChargesList;
                            //var carrierRemarksList = [];
                            if ($scope.carrierList.length > 0 && $scope.gridAction == 'edit') {
                                if ($scope.carrierList[$scope.carrierRowIndix].carrierRemarksDTOList != null) {
                                    $scope.carrierList[$scope.carrierRowIndix].carrierRemarksDTOList.length > 0
                                    {
                                        output.carrierItem.carrierRemarksDTOList = $scope.carrierList[$scope.carrierRowIndix].carrierRemarksDTOList;
                                    }
                                }
                                $scope.carrierList.splice($scope.carrierRowIndix, 1)
                            }
                            $scope.carrierList.push(output.carrierItem);

                        }
                        else if (output == "close") {

                        }
                        $scope.gridAction = '';
                        $scope.carrierRowIndix = 0;
                        $scope.selectedCarrierName = '';
                        $scope.selectedCarrierId = 0;
                        $scope.selectedTransitTime = '';
                        $scope.selectedRemarks = '';
                        $scope.selectedFrequency = '';
                        $scope.selectedCarrierRates = [];
                        $scope.carrierRates =[];
                    },
                    function (output) {
                        ngNotifier.logError(output);
                    });
            }
        };

        $scope.showRemarksModel = function (rownum, quotationCarrierId, carrierID, carrierName) {
            $scope.carrierRowIndix = rownum;
            $scope.selectedCarrierId = carrierID;
            $scope.selectedCarrierName = carrierName;
            $scope.lookups.remarks.forEach(function (o) {
                o.remarks = '';
                o.isSelected = false;
            });
            //if (quotationCarrierId == 0) {s
            $scope.selectedCarrierRemarks = [];
            $scope.selectedCarrierRemarks = $scope.carrierList[rownum].carrierRemarksDTOList;
            if ($scope.selectedCarrierRemarks != null) {
                if ($scope.selectedCarrierRemarks.length > 0) {
                    $scope.selectedCarrierRemarks.forEach(function (editItem) {
                        $scope.lookups.remarks.forEach(function (item) {
                            if (item.refId == editItem.refId) {
                                item.remarks = editItem.remarks;
                                item.isSelected = true;
                            }
                        });
                    });
                };
            }
            //}
            //else {
            //$scope.getSelectedCarrierRates(carrierId);
            // }

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/operation/quotation/addRemarks.html",
                controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                    $scope.portOriginName = requestData.portOriginName;
                    $scope.portDischargeName = requestData.portDischargeName;
                    $scope.carrierName = requestData.carrierName;
                    $scope.carrierID = requestData.carrierID,
                    $scope.carrierRemarks = requestData.carrierRemarks
                    $scope.addCarrierRemarks = function (action) {
                        var outputData = {}
                        if (action == 'update') {
                            var selectedRemarksList = []
                            var remarksItem = {}
                            $scope.carrierRemarks.forEach(function (item) {
                                if (item.isSelected) {
                                    remarksItem = {}
                                    remarksItem.carrierID = $scope.carrierID;
                                    remarksItem.refId = item.refId;
                                    remarksItem.remarks = item.remarks
                                    selectedRemarksList.push(remarksItem);
                                }
                            });

                            outputData.selectedRemarksList = selectedRemarksList;
                            outputData.action = 'update';
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
                    $scope.selectedRemarks = '';
                    $scope.selectedFrequency = '';
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };
        
        $scope.showNextActionRemarksModel = function () {          

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/operation/quotation/nextActionRemarks.html",
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
        

        $scope.deleteCarrierRow = function (rownum) {
            for (var i = $scope.carrierRemarksList.length - 1; i >= 0; i--) {
                if ($scope.carrierList(rownum).carrierID = $scope.carrierRemarksList(i).carrierID) {
                    $scope.carrierRemarksList.splice(i, 1);
                }
            }
            $scope.carrierList.splice(rownum, 1);
            
        }

        $scope.editCarrierRow = function (rownum, quotationCarrierId, carrierID, carrierName, transitTime, frequency, remarks) {
            
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
            //$scope.entity.txtPieceCount = $scope.airServiceList[rownum].pieceCount;
            //$scope.entity.txtwtpckg = $scope.airServiceList[rownum].weight;
            //$scope.entity.txtTotalWtkg = $scope.airServiceList[rownum].totalWt;
            //$scope.entity.txtLengthInch = $scope.airServiceList[rownum].length;
            //$scope.entity.txtwidthInch = $scope.airServiceList[rownum].width;
            //$scope.entity.txtHightInch = $scope.airServiceList[rownum].height;
            //$scope.entity.airVolume = '';
            //$scope.airServiceList.splice(rownum, 1);
            //calculateAirTotal();
        }

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("quotationController", controller);

});
