"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "bookingCaptureExpensesService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General
        $scope.page = appUrl.bookingCaptureExpenses;
        $scope.tabs = appUrl.bookingCaptureExpenses.tabs;
        //$scope.departments = [];
        $scope.expenseList = [];
        $scope.totalExpensesellingAmount  = 0;
        


        var lastAction = "";
        //#endregion

        //#region Lookup


        $scope.lookups = { siplExpenseHeads: []};

        $scope.initDropdown = function () {

            //$scope.fetchLookupData("sipldepartment", 0, "displayOrder", "siplDepartments", null);
            $scope.getExpenseHeads()
        };
        $scope.afterFetchLookupData = function (lookupKey) {
            //if (lookupKey == "siplDepartments") { $scope.lookups.countries.unshift({ "departmentID": 0, "department": "" }); } //
        }
        $scope.getExpenseHeads = function () {
            var getExpenseHead = entityService.getExpenseHeads().then(
                function (output) {
                    $scope.lookups.siplExpenseHeads = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }
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
                    break;


            }
        };

        $scope.afterGetDetail = function (action) {
             

            //$scope.calculateWeight('hazweight');
            //$scope.calculateVolume('hazVolume');
        };


        //#endregion
        $scope.enquiryTypes = [
                { optionValue: "0", optionName: "Select One" },
                { optionValue: "1", optionName: "By Email" },
                { optionValue: "2", optionName: "By Mail" },
                { optionValue: "3", optionName: "By Telecall" }
        ];
        $scope.pickupTypes = [
                { optionValue: "1", optionName: "SD/Port" },
                { optionValue: "2", optionName: "Port/Port" },
                { optionValue: "3", optionName: "Door/Door" },
                { optionValue: "4", optionName: "CFS/CFS" },
                { optionValue: "5", optionName: "RailRamp/Port" },
                { optionValue: "6", optionName: "CY/CY" },
                { optionValue: "7", optionName: "SD/CY" }
        ];
        $scope.sipl_MailMode = [
                { optionValue: 1, optionName: "Telephonic" },
                { optionValue: 2, optionName: "eMail" },
                { optionValue: 3, optionName: "Fax " }
        ];
        $scope.sipl_ContractBooking = [
                { optionValue: 1, optionName: "NotKnown" },
                { optionValue: 2, optionName: "Booked  as per contract" },
                { optionValue: 3, optionName: "Booked out side contract " }
        ];
        $scope.sipl_InvoiceStatus = [
                { optionValue: "1", optionName: "NOT READY FOR  INVOICING" },
                { optionValue: "2", optionName: "INVOICED" },
                { optionValue: "4", optionName: "VOID" },
                { optionValue: "5", optionName: "RECORD CANCELLED" },
                { optionValue: "6", optionName: "RECORD ROLLED OVER" },
                { optionValue: "7", optionName: "READY FOR INVOICING" }
        ];
        $scope.sipl_LoadType = [
                { optionValue: "1", optionName: "FCL" },
                { optionValue: "2", optionName: "LCL" },
                { optionValue: "3", optionName: "RORO" },
                { optionValue: "4", optionName: "TOTO" },
                { optionValue: "5", optionName: "BREAK BULK" },
                { optionValue: "7", optionName: "AIR" },
                { optionValue: "13", optionName: "SPECIAL CARGO" },
                { optionValue: "16", optionName: "Candle Consignment" }
        ];

        //$scope.entity.enquiryTypeID = 0;

        $scope.searchOptions = [                             
                { optionValue: "SystemRefNo", optionName: "System Ref No" },                
                { optionValue: "LineBookingNo", optionName: "Line Booking No" }
        ];

        $scope.selectOption = "SystemRefNo";
        $scope.searchBox = "";
        

        //$scope.searchParam = {
        //    optionValue: $scope.selectOption,
        //    seachValue: $scope.searchBox,
        //    departmentID: 0
        //};
        //#region Methods
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

               var dataitems = entityService.getDocumentList(listParams).then(
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
        //#endregion

        $scope.performBookingSearch = function (source, selectOption, searchBox, departmentValue) {

            //var action = source.currentTarget.attributes["action"].value;
            //$scope.searchParam = {
            //    optionValue: selectOption,
            //    seachValue: searchBox,
            //    departmentID: departmentValue
            //};
            //$scope.bookinglistTable.reload();
        };
              

        $scope.saveBookingExpenses = function (source, fromList) {

            if ($scope.entity.fileNo == "" || $scope.entity.fileNo == null) {
               
                ngNotifier.error("Please enter System Ref");
                return;
            }
            if ($scope.expenseList.length > 0) {
                $scope.entity.expenseDetailDTOList = $scope.expenseList;
            }
            //else {
            //    ngNotifier.error("Please Add atleast one Expense Head");
            //    return;
            //}
            
            $scope.entity.siteId = $scope.$parent.selectedSiteId;
            $scope.entity.userID = $scope.$parent.authentication.userId;
            $scope.entity.updatedBy = $scope.$parent.authentication.userId;
            entityService.saveBookingExpenses($scope.entity).then(
                function (output) {
                    //$scope.enquiryID = output.data.data;
                    $scope.entity = {};
                    $scope.expenseList = [];
                    $scope.selectOption = "SystemRefNo";
                    $scope.searchBox = "";

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
                case "rollOver":
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

        var copyEnquiry = function () {

            return true;
        }
       

        
        var viewDetail = function () {
            $scope.viewList = false;
            $scope.page.urls.container = "app/views/shared/container.html";
            $scope.entity = {};
        };

        $scope.showDocumnetExpenseDetail = function (action, selectOption, searchBox) {

            //$scope.onClickTab($scope.tabs[0]);
            //viewDetail();
            //initControls(action);
            if (searchBox == null || searchBox == "") {
                var errorMsg = "Please enter value for " + selectOption
                ngNotifier.error(errorMsg);
                return;
            }
            $scope.entity = {};
            $scope.entity.optionVale = selectOption;
            $scope.entity.searchVale = searchBox;
            $scope.entity.siteId = $scope.$parent.selectedSiteId;

            entityService.showDocumnetExpenseDetail($scope.entity).then(
                 function (output) {
                     if (output.data.resultId == 2005) {
                         ngNotifier.showError($scope.authentication, output);
                         $scope.logOut()
                     }
                     $scope.entity = {};
                     $scope.entity = output.data.data;
                     $scope.expenseList = [];
                     $scope.totalExpensesellingAmount  = 0;
                     
                     if ($scope.entity.expenseDetailDTOList != null) {
                         $scope.expenseList = $scope.entity.expenseDetailDTOList;
                         //calculateExpenseTotal();
                     }                    

                     $scope.afterGetDetail(action);

                 },
                 function (output) {
                     ngNotifier.showError($scope.authentication, output);
                 }
             );


        };
        
        $scope.addExpenseRow = function () {
            var expenseItem = {};
            if ($scope.entity.vendorName == null && $scope.entity.docNo == null
                && $scope.entity.docDate == null && $scope.entity.sellingAmount == null
                && $scope.entity.narration == null && $scope.entity.expenseHead == null 
                && $scope.entity.remarks == null && $scope.entity.buyingAmount == null) {
                ngNotifier.error("Please enter valid values ");
                return;
            }
            if ($scope.entity.sellingAmount == null || $scope.entity.sellingAmount == "") {
                ngNotifier.error("Please enter valid Selling Amount ");
                return;
            }
            if ($scope.entity.buyingAmount == null || $scope.entity.buyingAmount == "") {
                ngNotifier.error("Please enter valid Buying Amount ");
                return;
            }
            if ($scope.entity.expenseHead == null || $scope.entity.expenseHead == "") {
                ngNotifier.error("Please select ExpenseHead");
                return;
            }
            expenseItem.vendorName = $scope.entity.vendorName;
            expenseItem.docNo = $scope.entity.docNo;
            expenseItem.docDate = $scope.entity.docDate;
            if ($scope.entity.sellingAmount != null || $scope.entity.sellingAmount != "") {
                expenseItem.sellingAmount = $scope.entity.sellingAmount;
            }
            else {
                expenseItem.sellingAmount = 0;
            }
            if ($scope.entity.buyingAmount != null || $scope.entity.buyingAmount != "") {
                expenseItem.buyingAmount = $scope.entity.buyingAmount;
            }
            else {
                expenseItem.buyingAmount = 0;
            }
            expenseItem.narration = $scope.entity.narration;
            if($scope.entity.expenseHead !=null){
                //expenseItem.expenseHead = $scope.entity.expenseHead.expenseName;
                expenseItem.expenseHeadID = $scope.entity.expenseHead.expenseId;
            }            
            expenseItem.remarks = $scope.entity.remarks;
            expenseItem.isActive = true;
            $scope.expenseList.push(expenseItem);
            //calculateExpenseTotal();
            $scope.entity.vendorName = "";
            $scope.entity.docNo = "";
            $scope.entity.docDate = null;
            $scope.entity.sellingAmount = "";
            $scope.entity.buyingAmount = "";
            $scope.entity.narration = "";
            //$scope.entity.expenseHead = "";
            $scope.entity.remarks = "";            
        };

        $scope.removeExpenseRow = function (rownum) {
            $scope.expenseList.splice(rownum, 1);
            //calculateExpenseTotal();
        }
        var calculateExpenseTotal = function () {
            
            var total = 0;
            if ($scope.expenseList.length > 0) {
                for (var i = 0; i < $scope.expenseList.length; i++) {
                    if ($scope.expenseList[i].sellingAmount != null && $scope.expenseList[i].sellingAmount !="") {
                    total += parseInt($scope.expenseList[i].sellingAmount);
                    }                    
                }
                $scope.totalExpensesellingAmount = total;
            }
        }

        

    

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("bookingCaptureExpensesController", controller);

});
