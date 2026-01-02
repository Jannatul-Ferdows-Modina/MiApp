// <reference path="controller.js" />
"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "exportRegisterReportService"];

    var controller = function ($scope, $filter, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.exportRegisterReport;
        $scope.tabs = appUrl.exportRegisterReport.tabs;
        
        $scope.actionRemarksList = [];
        $scope.shipmentDocsList = [];
        $scope.searchOptions2 = [];
        $scope.searchOptions3 = [];
        $scope.isOption2disabled = true;
        $scope.isOption3disabled = true;
        $scope.lookups = { siplDepartments: [], miamiBookingStatus: [] };
        $scope.searchResult = [];


        $scope.initDropdown = function () {
            $scope.fetchLookupData("sipldepartment", 0, "displayOrder", "siplDepartments", null);
            $scope.fetchLookupData("siplBookingStatus", 0, "Status", "miamiBookingStatus", null);
        };
        $scope.afterFetchLookupData = function (lookupKey) {
            if (lookupKey == "siplDepartments") { $scope.lookups.siplDepartments.unshift({ "departmentID": 0, "department": "-Select-" }); } //
            if (lookupKey == "miamiBookingStatus") { $scope.lookups.miamiBookingStatus.unshift({ "statusID": 0, "status": "-Select-" }); } //
            
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

            if (lookupModule == "SIPLContact")
            {
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



        };

        $scope.selectDateOption = "-Select-";
        $scope.fromDate = "",
        $scope.toDate = "",        
        $scope.galBookingStatusID = 0,          
        $scope.departmentID= 0,
        $scope.searchOption1Value = "-Select-";
        $scope.searchBox1Value = "",
        $scope.searchOption2Value = "-Select-";
        $scope.searchBox2Value = "",
        $scope.searchOption3Value ="-Select-";
        $scope.searchBox3Value = ""
        $scope.isCurrentYear = true;
        $scope.isExcludeCancellRollover= true;
        //$scope.entity.isCurrentYear = true;
        //$scope.entity.isExcludeCancellRollover = true;

        $scope.searchParam = {
            optionDateValue : $scope.selectDateOption,
            fromDate : $scope.fromDate,
            toDate : $scope.toDate,
            galBookingStatusID: $scope.galBookingStatusID,        
            departmentID : $scope.departmentID,
            searchOption1Value: $scope.searchOption1Value,
            searchBox1Value:$scope.searchBox1Value,
            searchOption2Value: $scope.searchOption2Value,
            searchBox2Value: $scope.searchBox2Value,
            searchOption3Value: $scope.searchOption3Value,
            searchBox3Value: $scope.searchBox3Value,
            isCurrentYear: $scope.isCurrentYear,
            isExcludeCancellRollover: $scope.isExcludeCancellRollover
        };
               
        $scope.clearDates = function (dateoption) {
            if(dateoption == "")
            {
                $scope.fromDate = null;
                //angular.element("#toDate")[0].value = null;
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
        $scope.performBookingSearch = function (source, selectDateOption, fromDate, toDate, galBookingStatusID, departmentID, searchOption1, searchBox1, searchOption2, searchBox2, searchOption3, searchBox3,isCurrentYear, isExcludeCancellRollover) {

            var action = source.currentTarget.attributes["action"].value;
            if (selectDateOption == null)
            {
                selectDateOption = "-Select-";
            }
            if (selectDateOption != null && selectDateOption != "-Select-" && selectDateOption != '') {

                if (fromDate == null || fromDate == '' || toDate == null || toDate == '') {
                    ngNotifier.error("Please enter valid From Date and To Date");
                    return;
                }
            }
            if (isCurrentYear == false && (fromDate == null || fromDate == '' || toDate == null || toDate == ''))
            {
                ngNotifier.error("Please enter valid From Date and To Date");
                return;
            }
            if ($scope.entity.selectOption1 == null){
                $scope.entity.selectOption1 = "-Select-";
            }
            if ($scope.entity.selectOption2 == null) {
                $scope.entity.selectOption2 = "-Select-";
            }
            if ($scope.entity.selectOption3 == null) {
                $scope.entity.selectOption3 = "-Select-";
            }
           
            if (searchBox1 == null) {
                searchBox1 = "";
            }
            
            if (searchBox2 == null) {
                searchBox2 = "";
            }
            
            if (searchBox3 == null) {
                searchBox3 = "";
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
                searchOption1Value: $scope.entity.selectOption1 ? $scope.entity.selectOption1 : "",
                searchBox1Value: searchBox1,
                searchOption2Value: $scope.entity.selectOption2 ? $scope.entity.selectOption2 : "",
                searchBox2Value: searchBox2,
                searchOption3Value: $scope.entity.selectOption3?$scope.entity.selectOption3:"",
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
            $scope.searchOptions.forEach(function(item) {
                if (item.optionValue != selectOption1) {
                    $scope.searchOptions2.push(item);
                }
            });
        };
        $scope.UpdateSearch3 = function (selectOption1,selectOption2) {            
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
                    //$scope.getcarrierAllRates($scope.entity.enquiryID);


                    break;

            }
        };

        $scope.deleteShipmentDoc = function (rownum,documentCommonID, filename) {
            $scope.entities = {};
            $scope.entities.documentCommonID = documentCommonID;
            $scope.entities.docName = filename;
            ngNotifier.confirm("Are you sure you want to DELETE Document?", null, function () {
                $scope.shipmentDocsList.splice(rownum, 1);
                entityService.deleteShipmentDoc($scope.entities).then(
                            function (output) {
                                
                                //$scope.entity = {};
                                //$scope.quotationlistTable.reload();
                                //$scope.goBack();
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
                     if ($scope.entity.isPublic == 1)
                     {
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

        $scope.showViewDocumentModel = function (documentCommonID) {
            $scope.entities = {};
            $scope.entities.documentCommonID = documentCommonID;            
            $scope.entities.siteId = $scope.$parent.selectedSiteId;

            entityService.getExportRegisterDetail($scope.entities).then(
                 function (output) {                     
                     if (output.data.resultId == 2005) {
                         ngNotifier.showError($scope.authentication, output);
                         $scope.logOut()
                     }
                     $scope.entity = output.data.data;
                     //start model
                     var modalInstance = $uibModal.open({
                         animation: false,
                         backdrop: "static",
                         keyboard: false,
                         size: "lg",
                         templateUrl: "app/components/report/exportRegisterReport/viewDocumentDetail.html",
                         controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                             $scope.entity = requestData.documentEntity
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
                                     documentEntity: $scope.entity
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

        $scope.callDocumentUploadModel = function (source) {

            //$scope.$parent.selectedSiteId
            //$scope.$parent.authentication.userId

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "lg",
                templateUrl: "app/components/report/documentUploadModel/detail.html",
                controller: "documentUploadModelController",
                resolve: {
                    requestData: function () {

                        return {
                            documentCommonID: (source || 0),
                            siteId: $scope.$parent.selectedSiteId,
                            userId: $scope.$parent.authentication.userId
                        };
                    }
                }
            });
            modalInstance.result.then(
                function (output) {
                    if (output.resultId == 1001) {
                        $scope.entity.contractID = output.data.contractID;

                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        $scope.uploadAttachment = function (documentCommonID) {
            $scope.entity.customerfile.forEach(function (file) {
           
                if (file) {

                    if (file.size > 10485760) {
                        ngNotifier.error("File cannot exceeds more than 10 MB size.");
                    }
                    else if (file.type != "application/pdf" && file.type != "application/docx" && file.type != "application/doc" && file.type != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && file.type != "application/xlsx" && file.type != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                        ngNotifier.error("Please upload only these file types - pdf,docx,doc,xlsx");
                        return;
                    }
                    else {
                        var attachment = {
                            DisplayName: documentCommonID + '_' + file.name,
                            DocumentCommonID: documentCommonID
                        };
                        entityService.uploadFile(attachment, file).then(
                            function (output) {
                                //ngNotifier.show(output.data.output);
                            },
                            function (output) {
                                ngNotifier.error(output.data.output.messages);
                            }
                        );
                    }

                }
            });

        };

        $scope.downloadAttachment = function (documentCommonID,filename) {
            //window.open('', '_blank', '');
            $scope.entities = {};
            $scope.entities.documentCommonID = documentCommonID;
            $scope.entities.attachFile = filename;
            
            if ($scope.entities.attachFile != null) {
                entityService.downloadAttachment($scope.entities).then(
                            function (output) {
                                var blob = new Blob([output.data], { type: 'application/octet-stream' });
                                saveAs(blob, output.config.headers.fileName);
                            },
                            function (output) {
                                ngNotifier.logError(output);
                            }
                        );
            }
        }

        $scope.showDocumentDetail = function (documentCommonID) {

            $scope.onClickTab($scope.tabs[0]);
            viewDetail();
            initControls('view');
            $scope.entity.documentCommonID = documentCommonID;
            $scope.entity.siteId = $scope.$parent.selectedSiteId;

            entityService.getDocumentDetail($scope.entity).then(
                 function (output) {
                     if (output.data.resultId == 2005) {
                         ngNotifier.showError($scope.authentication, output);
                         $scope.logOut()
                     }
                     $scope.entity = output.data.data;                    

                     //$scope.afterGetDetail(action);

                 },
                 function (output) {
                     ngNotifier.showError($scope.authentication, output);
                 }
             );


        };


        

           //to view email detail
        $scope.callEmailUploadModel = function (enqid) {
           
            entityService.getEmailDocumentDetail(enqid).then(
               function (output) {
                   if (output.data.resultId == 2005) {
                       ngNotifier.showError($scope.authentication, output);
                       $scope.logOut()
                   }
                   $scope.emaildetail = output.data.data;

                   if ($scope.emaildetail[0].emailTo !=null) {
                      
                      
                       
                       var modalInstance = $uibModal.open({
                           animation: false,
                           backdrop: "static",
                           keyboard: false,
                           size: "lg",
                           templateUrl: "app/components/report/exportRegisterReport/emaildetail.html",
                           //scope: $scope,
                           //controller: "exportRegisterReportController"

                           //});

                           controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                               $scope.emaildetail = requestData.emaildetail;
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
                                       emaildetail: $scope.emaildetail
                                   };
                               }
                           }
                       });

                   } else {
                       ngNotifier.error("Email detail is not find.");

                   }



               });

        };
        // Pre alert functionality
        $scope.PreAlert = function (enqid, documentCommonID) {

            $scope.entities = {};
            $scope.entities.documentCommonID = documentCommonID;
            $scope.entities.siteId = $scope.$parent.selectedSiteId;
            $scope.entities.createdBy = $scope.userInfo.usrId;
            entityService.prealert(enqid, $scope.entities).then(
              function (output) {
                  if (output.data.resultId == 2005) {
                      ngNotifier.showError($scope.authentication, output);
                      $scope.logOut()
                  }
                  $scope.emaildetail = output.data.data;

                  
                  $scope.emaildetail[0].createdBy = $scope.userInfo.usrId;
                  $scope.emaildetail[0].updatedBy = $scope.userInfo.usrId;
                      $scope.emaildetail[0].SitId = $scope.$parent.selectedSiteId;
                      var modalInstance = $uibModal.open({
                          animation: false,
                          backdrop: "static",
                          keyboard: false,
                          size: "lg",
                          templateUrl: "app/components/report/exportRegisterReport/prealert.html",
                          //scope: $scope,
                          //controller: "exportRegisterReportController"

                          //});

                          controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                              $scope.emaildetail = requestData.emaildetail;
                              $scope.select = function (action) {
                                  var outputData = {}
                                  if (action == 'update') {

                                  }
                                  else {
                                      outputData.action = 'close';
                                  }
                                  $uibModalInstance.close(outputData);
                              };

                              $scope.downloadAttachmentPreAlert = function (documentCommonID, filename,docType) {
                                  //window.open('', '_blank', '');
                                  $scope.entities = {};
                                  $scope.entities.documentCommonID = documentCommonID;
                                  $scope.entities.attachFile = filename;
                                  $scope.entities.documentType = docType;
                                  $scope.entities.isSystemGenerated = 0;

                                  if ($scope.emaildetail[0].shipmentDocsDTOList != null) {
                                      entityService.downloadAttachmentPreAlert($scope.entities).then(
                                                  function (output) {
                                                      var blob = new Blob([output.data], { type: 'application/octet-stream' });
                                                      saveAs(blob, output.config.headers.fileName);
                                                  },
                                                  function (output) {
                                                      ngNotifier.logError(output);
                                                  }
                                              );
                                  }
                              };

                              $scope.uploadAttachmentPreAlert = function (documentCommonID) {
                                  $scope.emaildetail[0].fileAttachementDTOList.forEach(function (file) {

                                      if (file) {

                                          if (file.size > 10485760) {
                                              ngNotifier.error("File cannot exceeds more than 10 MB size.");
                                          }
                                          else if (file.type != "application/pdf" && file.type != "application/docx" && file.type != "application/doc" && file.type != "application/vnd.openxmlformats-officedocument.wordprocessingml.document" && file.type != "application/xlsx" && file.type != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {
                                              ngNotifier.error("Please upload only these file types - pdf,docx,doc,xlsx");
                                              return;
                                          }
                                          else {
                                              var attachment = {
                                                  DisplayName: documentCommonID + '_' + file.name,
                                                  DocumentCommonID: documentCommonID,
                                                  DocumentType: 'ExtDoc'
                                              };
                                              entityService.uploadFile(attachment, file).then(
                                                  function (output) {
                                                      //ngNotifier.show(output.data.output);
                                                  },
                                                  function (output) {
                                                      ngNotifier.error(output.data.output.messages);
                                                  }
                                              );
                                          }

                                      }
                                  });

                              };

                              $scope.saveDocumentAttachementPreAlert = function () {


                                  var isValid = true;
                                  if ($scope.emaildetail[0].fileAttachementDTOList != null) {

                                      if ($scope.emaildetail[0].fileAttachementDTOList.length == 0) {
                                          ngNotifier.error("Please select atleast one file");
                                          return;
                                      }
                                      else {
                                          $scope.emaildetail[0].fileAttachementDTOList.forEach(function (file) {

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
                                              // $scope.entity.shipmentDocsDTOList = [];
                                              $scope.emaildetail[0].fileAttachementDTOList.forEach(function (file) {
                                                  fileItem = {};
                                                  fileItem.docName = file.name;
                                                  fileItem.documentType = 'ExtDoc';
                                                  fileItem.documentCommonID = $scope.emaildetail[0].documentCommonID;
                                                  $scope.emaildetail[0].shipmentDocsDTOList.push(fileItem);
                                              });
                                          }


                                      }
                                  }
                                  if ($scope.emaildetail[0].fileAttachementDTOList == undefined) {
                                      ngNotifier.error("Please attach atleast one file");
                                      return;
                                  }
                                  if (isValid == true) {
                                      $scope.entity = {};
                                      $scope.entity.siteId =   $scope.emaildetail[0].SitId;
                                      $scope.entity.createdBy = 1;// $scope.emaildetail[0].createdBy;
                                      $scope.entity.updatedBy = 1;// $scope.emaildetail[0].createdBy;
                                      $scope.entity.documentCommonID = $scope.emaildetail[0].documentCommonID;
                                      $scope.entity.documentType = "ExtDoc";
                                      entityService.saveCustomerDocAttachementsPreAlert($scope.entity).then(
                                          function (output) {
                                              
                                              $scope.uploadAttachmentPreAlert($scope.emaildetail[0].documentCommonID);
                                              
                                              //$scope.entity = {};
                                              ngNotifier.show(output.data);

                                          },
                                          function (output) {
                                              ngNotifier.showError($scope.authentication, output);
                                              //$scope.editMode = false;
                                              //$scope.disabledInsert = true;
                                              //$scope.disabledUpdate = true;
                                              //$scope.requiredInsert = false;
                                              //$scope.requiredUpdate = false;
                                          });
                                  }
                              };


                              $scope.deleteattachement = function (docname,doctype) {

                                  $.each($scope.emaildetail[0].shipmentDocsDTOList, function (i, el) {
                                      if (this.docName == docname && this.documentType == doctype) {
                                          $scope.emaildetail[0].shipmentDocsDTOList.splice(i, 1);
                                      }
                                  });

                              };

                              $scope.sendprealertemail = function()
                              {
                                  entityService.prealertsendemail($scope.emaildetail[0]).then(
                                  function (output) {
                                      var outputData = {}                                      
                                      outputData.action = 'close';                                    
                                      $uibModalInstance.close(outputData);
                                      ngNotifier.show(output.data);
                                  });

                              };
                          },
                          resolve: {
                              requestData: function () {
                                  return {
                                      emaildetail: $scope.emaildetail
                                  };
                              }
                          }
                      });

                  


              });
        };









     
        $(document).ready(function() {
            $("#reportTable").tableHeadFixer();
            setTimeout(function(){
                var _children = $('#tableContainer').children();
                $('#reportTable').insertBefore(_children[0]);
            },200);
        });

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("exportRegisterReportController", controller);

});
