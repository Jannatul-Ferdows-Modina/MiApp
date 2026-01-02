"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "mcsReportCorporateService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {
        //#region General
        
        $scope.pageTitle = "SP MCS";
        $scope.breadcrumbs = ["Home", "SP MCS"];
        $scope.page = appUrl.mcsReport;
        $scope.tabs = appUrl.mcsReport.tabs;
       
        //#endregion

        //#region Private 

        var exportReport = function () {

            var reportParams = {
                SiteId: $scope.entity.siteId.toString(),
               // SiteId: $scope.$parent.selectedSiteId,
                StartBookingDate: $scope.entity.startBookingDate || "",
                EndBookingDate: $scope.entity.endBookingDate || "",
                DeptId: $scope.entity.departmentId,
                isinvoiceready:$('input[name="isinvoiceready"]:checked').val(), // $("#chkisinvoiceready").is(":checked") ? '1' : '0'
                SystemRefNo: $scope.entity.SystemRefNo,
                QBStatus: $("#qbstatus").val(),
                QBMaturedMonth: $("#txtMATUREDMONTH").val(),
                UserId :  $scope.$parent.authentication.userId
            };
            entityService.exportReport(reportParams).then(
                function (output) {
                    var blobData = new Blob([output.data], { type: output.headers()["content-type"] });
                    var fileName = output.headers()["x-filename"];
                    saveAs(blobData, fileName);
                },
                function (output) {
                    ngNotifier.error(output);
                }
            );
        };

        //#endregion

        //#region Lookup 

        $scope.lookups = { departments: [], sites: [] };

        $scope.initDropdown = function () {
            debugger;
            $scope.fetchLookupData("sipldepartment", 0, "displayOrder", "departments", null);

            var sitAll = $.map($scope.authentication.userSite, function (o) { return o.SitId; }).join(",");
            $scope.lookups.sites = angular.copy($scope.authentication.userSite);
            $scope.lookups.sites.unshift({ SitId: sitAll, SitName: "All" });
            $scope.entity.siteId = sitAll;
            var today = new Date();
            var yyyy = today.getFullYear();
            $scope.entity.startBookingDate = Utility.getDateISO(new Date("01/01/" + yyyy+""));
            $scope.entity.endBookingDate = Utility.getDateISO(new Date("12/31/" + yyyy + ""));
            $scope.entity.departmentId = 2;
            $scope.entity.SystemRefNo = '';
            setTimeout(function () { $("#chkisinvoiceready1").prop('checked', true) }, 1000);
            $scope.entity.QBStatus = '';
        };

        $scope.afterFetchLookupData = function (lookupKey) {

            if (lookupKey == "departments") {
                $scope.lookups[lookupKey].splice(0, 0, { departmentID: 0, department: "Select any one Department" });
                $scope.entity.departmentId = 2;
            }

            $timeout(function () {
                $scope.entity.siteId = $scope.$parent.selectedSiteId.toString().split(',');

               // $('#selsiteId').val($scope.$parent.selectedSiteId);
                $('#selsiteId').multiselect('destroy');
                $('#selsiteId').multiselect({ buttonWidth: '100%' });
                $('.multiselect-container').css('width', '300px');
                

            }, 500);
        };

        $scope.resetInvoice = function (source, target) {

            var docid = source.currentTarget.attributes["entityId"].value;
           var message = "Are you sure you want to DELETE the existing Invoice ?"
            ngNotifier.confirm(message, null, function () {

                entityService.resetinvoice($scope.$parent.authentication.userId, docid).then(

                      function (output) {
                     


                          // alert('last control'+output.data);
                          //$scope.entity = {};
                          //// $scope.customerContactlistTable.reload();
                          //$scope.reportTable.reload();
                          //$scope.goBack();
                          ngNotifier.show(output.data);
                          if (output.data.resultId === 1001) {
                              $scope.reportTable.reload();
                          }

                      },
                      function (output) {
                          ngNotifier.showError($scope.authentication, output);
                      });

            });

        }


        //#endregion
        $scope.performQuickBookContactAction = function (source, target) {
        
            var action = source.currentTarget.attributes["action"].value;
            var filename = source.currentTarget.attributes["entityId"].value;
            var docno = source.currentTarget.attributes["docno"].value;
           
            var ishbl = source.currentTarget.attributes["ishbl"].value;
            var ismbl = source.currentTarget.attributes["ismbl"].value;
            var isco = source.currentTarget.attributes["isco"].value;
            var isdockreceipt = source.currentTarget.attributes["isdockreceipt"].value;
            var isshipper = source.currentTarget.attributes["isshipper"].value;
            var isbooking = source.currentTarget.attributes["isbooking"].value;
            var isother = source.currentTarget.attributes["isother"].value;
            var isaes = source.currentTarget.attributes["isaes"].value;
            var docrequired = source.currentTarget.attributes["docrequired"].value;
            var isporttoport = source.currentTarget.attributes["isporttoport"].value;
            var islinembl = source.currentTarget.attributes["islinembl"].value;

            var qb = filename.split(";")[1];
            switch (action) {
                
                case "QBSignInBatch":
                    var docreqmess = "";
                    var j = 1;
                    if (docrequired != undefined && docrequired != null && docrequired != "") {

                        var docreq = docrequired.split(",");
                       
                        for (let i = 0; i < docreq.length; i++) {
                            if (docreq[i] == 1 && ishbl == 0) {
                                docreqmess += j + "." + "HBL document are required please upload HBL document.\n";
                                j = j + 1;
                            }
                            if (docreq[i] == 2 && ismbl == 0) {
                                docreqmess += j + "." + "MBL document are required please upload MBL document.\n";
                                j = j + 1;
                            }
                            if (docreq[i] == 3 && isdockreceipt == 0 && isporttoport != "1") {

                                docreqmess += j + "." + "Doc Receipt document are required please upload Doc Receipt document.\n";
                                j = j + 1;
                            }
                            if (docreq[i] == 4 && isco == 0) {
                                docreqmess += j + "." + "CO document are required please upload CO document.\n";
                                j = j + 1;
                            }
                            if (docreq[i] == 5 && isshipper == 0) {
                                docreqmess += j + "." + "Shipper  document are required please upload Shipper document.\n";
                                j = j + 1;
                            }
                            if (docreq[i] == 6 && isbooking == 0) {
                                docreqmess += j + "." + "Booking  document are required please upload Booking document.\n";
                                j = j + 1;
                            }
                            if (isother[i] == 7 && isother == 0) {
                                docreqmess += j + "." + "Other  document are required please upload Other document.\n";
                                j = j + 1;
                            }
                            if (docreq[i] == 8 && isaes == 0) {
                                docreqmess += j + "." + "AES  document are required please upload AES document.\n";
                                j = j + 1;
                            }
                            if (islinembl == 0) {
                                docreqmess += j + "." + "Line MBL  document are required please upload Line MBL document.\n";
                                j = j + 1;
                            }

                        }
                        
                    }
                    else {
                        if (islinembl == 0) {
                            docreqmess += j + "." + "Line MBL  document are required please upload Line MBL document.\n";
                            j = j + 1;
                        }
                        if (isdockreceipt == 0 && isporttoport != "1") {

                            docreqmess += j + "." + "Doc Receipt document are required please upload Doc Receipt document.\n";
                            j = j + 1;
                        }
                        if (isaes == 0) {
                            docreqmess += j + "." + "AES  document are required please upload AES document.\n";
                            j = j + 1;
                        }
                    }
                    //if (docreqmess != "") {
                    //    var j = 1;
                    //    confirm(docreqmess);
                       
                    //}
                    var message = "";
                    if (qb == "Y") {
                        docreqmess += " \n Note-Are you sure you want to DELETE the existing Invoice and Bill in Quick Book, then Re-Create invoice and bill in the QB?"
                    }
                    else {
                        docreqmess += " \n Note-Are you sure you want to Create Invoice and bill in the QuickBook?"
                    }
                    if (window.confirm(docreqmess)) {
                        entityService.test($scope.$parent.authentication.userId, docno).then(

                            function (output) {

                                ngNotifier.show(output.data);
                                if (output.data.resultId === 1001) {
                                    $scope.reportTable.reload();
                                }

                            },
                            function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });

                    }
                    else {
                        return false;
                    }
                    //confirm(docreqmess, null, function () {
                        
                    //    entityService.test($scope.$parent.authentication.userId, docno).then(

                    //          function (output) {
                                 
                    //              ngNotifier.show(output.data);
                    //              if(output.data.resultId === 1001)
                    //              {
                    //                  $scope.reportTable.reload();
                    //              }
                                
                    //          },
                    //          function (output) {
                    //              ngNotifier.showError($scope.authentication, output);
                    //          });

                        

                    //});
                   
                    break;
            }
            if ($scope.afterPerformAction != undefined) {
                $scope.afterPerformAction(source, fromList);
            }
        }
        //#region Detail

        $scope.reportTable = new NgTableParams(
        {
          
            page: 1,
            count: 10,
            sorting: $.parseJSON("{ \"SerialNo\": \"asc\" }"),
            group: {
                sitId: "asc"
            }
        }, {
            counts: [],
            getData: function (params) {

                debugger;
                var listParams = {
                      SiteId: $scope.entity.siteId.toString(),
                   // SiteId: $scope.$parent.selectedSiteId,
                    StartBookingDate: $scope.entity.startBookingDate || "",
                    EndBookingDate: $scope.entity.endBookingDate || "",
                    DeptId: $scope.entity.departmentId,
                    isinvoiceready: $('input[name="isinvoiceready"]:checked').val(),    //    $("#chkisinvoiceready").is(":checked") ? '1' : '0'
                    SystemRefNo: $scope.entity.SystemRefNo,
                    QBStatus: $("#qbstatus").val() ,
                    QBMaturedMonth: $("#txtMATUREDMONTH").val(),
                    UserId :  $scope.$parent.authentication.userId
                };
                return entityService.getMCSList(listParams).then(
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

        $scope.sum = function (data, field) {
            return Utility.sumArray(data, field);
        }

        $scope.performSubAction = function (source, target) {

            var action = source.currentTarget.attributes["action"].value;

            $scope.$broadcast("show-errors-check-validity");
            if ($scope.form.detail.$error.required != undefined && $scope.form.detail.$error.required.length > 0) {
                ngNotifier.error("Required Field(s) are missing data.");
                return;
            }

            if ($scope.entity.departmentId == 0) {
                ngNotifier.error("First, select any one department.");
                return;
            }

            switch (action) {
                case "showReport":
                    $scope.reportTable.reload();
                    break;
                case "exportReport":
                    exportReport();
                    break;
                default:
                    //TODO
                    break;
            }
        };

        //#endregion
        $scope.quickbookcall = function () {
            
            entityService.qbRefreshTokens().then(function (data) {
                if (data.status == 200) {
                    ngNotifier.success("Refresh Token updated successfully.");
                   
                }
                else {
                    alert(data.statusText);
                }
            });

        }
        $scope.callCompanyModal = function (CompanyID) {

            $scope.entity.fkCompanyID = CompanyID;
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
                            companyID: ($scope.entity.fkCompanyID || 0),
                            isquickbook: "1"
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

        $scope.performQuickBookStatus = function (source, target) {

           
            var docno = source.currentTarget.attributes["docno"].value;
            entityService.performQuickBookStatus(docno).then(function (output) {
                ngNotifier.show(output.data);
                if (output.data.resultId === 1001) {
                    $scope.reportTable.reload();
                }




                //if (data.status == 200) {
                //    ngNotifier.success(data.statusText);
                //     $scope.reportTable.reload();
                //}
                //else {
                //    alert(data.statusText);
                //}
            },function (output) {
                                ngNotifier.showError($scope.authentication, output);
                            });


        }


        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("mcsReportCorporateController", controller);

});
