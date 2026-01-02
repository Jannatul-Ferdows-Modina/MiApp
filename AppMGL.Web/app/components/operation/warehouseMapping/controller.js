"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$timeout", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "warehousemappingService"];

    var controller = function ($scope, $filter, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {
        var removewarehouse = function () {

            if ($scope.entityId > 0) {
                ngNotifier.confirm("Are you sure you want to DELETE the data?", null, function () {
                    entityService.deletewarehouse($scope.entityId).then(
                        function (output) {
                            dataChanged = true;
                            ngNotifier.show(output.data);
                            $scope.goBack();
                        },
                        function (output) {
                            ngNotifier.showError($scope.authentication, output);
                        });
                }, function (output) {
                    ngNotifier.showError($scope.authentication, output);
                });
            }
        };

        var typingTimer; // Timer for debounce
        const typingDelay = 500; // Adjust delay (500ms is a good starting point)

        // Function that gets called when user types
        $scope.delayedSearch = function () {
            if (typingTimer) {
                $timeout.cancel(typingTimer); // Clear previous timeout
            }
            typingTimer = $timeout(function () {
                $scope.searchValuesCompany($scope.companySearch, 'companyName'); // Call API after delay
            }, typingDelay);
        };



        //#region General
          var _criteria = [];
          var dataChanged = false;
          var lastAction = "";
          var logs = [];
          var logCriteria = [];
          $scope.selectTypeaheadAES = function ($item, $model, $label, source, stype) {

              var lookupModule = null;
              var lookupIndex = null;
              var target = $(source.currentTarget);

              if (stype == "SIPLContact5") {
                  $scope.entity.shipperName = $item.name;
                  
              }
             else if (stype == "SIPLContact6") {
                  $scope.entity.consigneeName = $item.name;                 
             }
             else if (stype == "SIPLContact1") {
                 $("input[name='ShipperNameF']").val($item.name);
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
          $scope.searchValuesCompany = function (viewValue, selectOption) {
              var resultItem = {};
             
              var lookupField = "companyName";

              if (selectOption == "companyName") {

                  var listParams = {
                      SiteId: $scope.selectedSiteId,
                      CwtId: $scope.userWorkTypeId,
                      ModuleId: 1,
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
                              $scope.searchResult.push(resultItem)

                          });
                          return $scope.searchResult;

                      }
                  );
              }

          };
          $scope.afterGoBack = function()
          {
              //$scope.criteria();
              $scope.listTablewarehouse.reload();
          }


          $scope.warehouseinwardno = function () {
               entityService.warehouseinwardno().then(
                     function (output) {
                         
                          $scope.entity.warehouseinwardno = output.data.data;

                     }
                 );
          };


          $scope.searchQuotation = function(e)
          {
              var modalInstance = $uibModal.open({
                  animation: false,
                  backdrop: "static",
                  keyboard: false,
                  size: "lg",
                  templateUrl: "app/components/operation/warehousemapping/searchquotation.html",
                  controller: "searchQuotationController",
                  resolve: {
                      requestData: function () {
                          return {
                              siteid: $scope.selectedSiteId,
                              finalQuotations: $scope.entity.quatationNo == 'undefined' ? '' : $scope.entity.quatationNo

                          };
                      }
                  }
              });
              modalInstance.result.then(
                  function (output) {
                      if (output.resultId == 1001) {
                          $scope.entity.quatationNo = output.finalQuotations;
                      }
                  },
                  function (output) {
                      ngNotifier.logError(output);
                  });
          };

         

          $scope.lookups = { storageLocation: [] }


          $scope.searchValuesWarehose = function (viewValue, selectOption) {
              var resultItem = {};
              var lookupField = "companyName";

              var listParams = {
                  SiteId: $scope.selectedSiteId,
                  CwtId: 0,
                  ModuleId: 1,
                  PageIndex: 1,
                  PageSize: 25,
                  Sort: "{ \"" + lookupField + "\": \"asc\" }",
                  Filter: viewValue
              };


              if (selectOption == "quotationno") {

                 
                  return entityService.SearchQuotation(listParams).then(
                      function (output) {
                          $scope.searchResult = [];
                          output.data.data.forEach(function (o) {
                              resultItem = {}
                              resultItem.quotationID = o.quotationID;
                              resultItem.quotationNo = o.quotationNo;
                              $scope.searchResult.push(resultItem)

                          });
                          return $scope.searchResult;

                      }
                  );
              }
              else if (selectOption == "warehouselocation") {
                  
                  return entityService.SearchWarehouselocation(listParams).then(
                      function (output) {
                          $scope.searchResult = [];
                          output.data.data.forEach(function (o) {
                              resultItem = {}

                              resultItem.locationId = o.locationId;
                              resultItem.storageLocation = o.storageLocation;
                              $scope.searchResult.push(resultItem);

                          });
                          return $scope.searchResult;

                      }
                  );

              }

          };
          $scope.setLookups = function (source, lookup, output, index) {

              if (lookup == "quotationno") {
                  $scope.entity.quatationId = output.data[0].quotationID;
              }
              else if (lookup == "warehouselocation") {
                  $scope.entity.wareHouseId = output.data[0].locationId;
                  entityService.getblocknumberlist($scope.entity.wareHouseId).then(
                      function (output) {
                          //$timeout(function () {
                          if (output.data.resultId == 2005) {
                              ngNotifier.showError($scope.authentication, output);
                              $scope.logOut()
                          }
                          $scope.lookups.storageLocation = output.data.data;
                          $timeout(function () {

                              $('#selblocknumber').multiselect('destroy');
                              $('#selblocknumber').multiselect({ buttonWidth: '100%' });

                             /* $('#selblocknumber').multiselect({
                                  includeSelectAllOption: true, // Adds "Select All" checkbox
                                  nonSelectedText: 'Select Options', // Placeholder text
                                  enableFiltering: true, // Search bar in dropdown
                                  buttonWidth: '100%', // Adjust button width
                                  maxHeight: 300, // Limit dropdown height
                                  enableCaseInsensitiveFiltering: true, // Case insensitive search
                                  checkboxName: 'multiselect[]' // Ensure checkboxes appear

                                  ,buttonText: function(options, select) {
                                  if (options.length === 0) {
                                      return 'Select Options'; // Placeholder when nothing is selected
                                  } else {
                                      var labels = [];
                                      options.each(function() {
                                          labels.push($(this).text());
                                      });
                                      return labels.join(', '); // Join selected options as text
                                  }
                              }
                              }); */
                          },500);

                          if ($scope.afterGetDetail != undefined) {
                              $scope.afterGetDetail();
                          }
                          //}, 100);
                      },
                      function (output) {
                          ngNotifier.showError($scope.authentication, output);
                      }
                  );


              }
             
          }
          $scope.clearLookups = function (source, lookup, index) {
              if (lookup == "quotationno") {
                  $scope.entity.quatationId = null;
              }
              else if (lookup == "warehouselocation") {
                  $scope.entity.wareHouseId = null;
              }
              
          }



          $scope.getDetailwarehouse = function () {

              if ($scope.entityId > 0) {
                  entityService.getdetailwarehousemapping($scope.entityId).then(
                      function (output) {
                          //$timeout(function () {
                          if (output.data.resultId == 2005) {
                              ngNotifier.showError($scope.authentication, output);
                              $scope.logOut()
                          }
                         
                          $scope.lookups.storageLocation = output.data.data.blocknumberlist;
                          $scope.entity = output.data.data;                         
                          $scope.entity.blockId = $scope.entity.blockId.split(',');
                          $("#selblocknumber").val($scope.entity.blockId);
                          $timeout(function () {

                              $scope.entity.blockId.forEach(function (value, index) {
                                  
                                  $('#selblocknumber option').each(function() {
                                      // 'this' refers to the current option element
                                      var optionValue = $(this).val();
                                      var optionText = $(this).text();
                                      if (parseInt(optionValue.split(':')[1], 10) === parseInt(value))
                                      {
                                          $(this).attr("selected", "selected");
                                      }
                                      
                                  });

                              });
                              $('#selblocknumber').multiselect('destroy');
                              $('#selblocknumber').multiselect({ buttonWidth: '100%' });

                             /* $('#selblocknumber').multiselect({
                                  includeSelectAllOption: true, // Adds "Select All" checkbox
                                  nonSelectedText: 'Select Options', // Placeholder text
                                  enableFiltering: true, // Search bar in dropdown
                                  buttonWidth: '100%', // Adjust button width
                                  maxHeight: 300, // Limit dropdown height
                                  enableCaseInsensitiveFiltering: true, // Case insensitive search
                                  checkboxName: 'multiselect[]' // Ensure checkboxes appear

                                  , buttonText: function (options, select) {
                                      if (options.length === 0) {
                                          return 'Select Options'; // Placeholder when nothing is selected
                                      } else {
                                          var labels = [];
                                          options.each(function () {
                                              labels.push($(this).text());
                                          });
                                          return labels.join(', '); // Join selected options as text
                                      }
                                  }
                              }); */
                          }, 500); 




                          if ($scope.afterGetDetail != undefined) {
                              $scope.afterGetDetail();
                          }
                          //}, 100);
                      },
                      function (output) {
                          ngNotifier.showError($scope.authentication, output);
                      }
                  );
              } else {
                  $scope.goBack();
              }
          };


          $scope.showDetailWarehose = function (action, id) {

              $scope.onClickTab($scope.tabs[0]);

              $scope.entityId = id;

              viewDetail();

              initControls(action);

              if ($scope.entityId > 0) {
                  $scope.getDetailwarehouse();
              }
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


          var viewDetail = function () {
              $scope.viewList = false;
              $scope.page.urls.container = "app/views/shared/container.html";
              $scope.entity = {};
          };

         

        $scope.page = appUrl.warehousemapping;
        $scope.tabs = appUrl.warehousemapping.tabs;
        var switchTab = function (title, action) {

            $scope.tabs.forEach(function (tab) {
                tab.active = false;
                tab.disabled = ((action === "add" || action === "edit") && tab !== $scope.tabs[0]);
                // tab.disabled = ((action === "add" || action === "edit") && tab !== $scope.tabs[0]);
                if (tab.title === title) {
                    tab.active = true;
                }
                if (tab.title == "Mapping" && action === "edit") {
                    tab.active = true;
                    tab.disabled = false;
                }
            });
        };
        //#endregion
        var save = function (action) {

            if ($scope.beforeSave != undefined) {
                $scope.beforeSave(action, lastAction);
                if ($scope.hasError) {
                    initControls(lastAction);
                    return;
                }
            }

            if (lastAction === "add") {

                $scope.entity.blockId = $scope.entity.blockId.toString();
                $scope.entity.SIT_ID = $scope.selectedSiteId;
                entityService.insertWarehouse($scope.entity).then(
                    function (output) {
                        if (output.data.resultId == 1001) {
                            dataChanged = true;
                            $scope.entity = output.data.data;
                            $scope.entityId = $scope.entity[Utility.lowerFirstChar($scope.page.keyField)];
                            ngNotifier.show(output.data);
                            lastAction = "";
                            if ($scope.afterSave != undefined) {
                                $scope.afterSave(lastAction);
                            }
                        }
                        else {
                            ngNotifier.showError($scope.authentication, output);
                            initControls(lastAction);
                        }
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                        initControls(lastAction);
                    });
            }
            else if (lastAction === "edit" && $scope.entityId > 0) {
                $scope.entity.blockId = $scope.entity.blockId.toString();
                $scope.entity.SIT_ID = $scope.selectedSiteId;
                entityService.insertWarehouse($scope.entity).then(
                    function (output) {
                        if (output.data.resultId == 1001) {
                            dataChanged = true;
                            $scope.entity = output.data.data;
                            ngNotifier.show(output.data);
                            lastAction = "";
                            if ($scope.afterSave != undefined) {
                                $scope.afterSave(lastAction);
                            }
                        }
                        else {
                            ngNotifier.showError($scope.authentication, output);
                            initControls(lastAction);
                        }
                    },
                    function (output) {
                        ngNotifier.showError($scope.authentication, output);
                        initControls(lastAction);
                    });
            }
        };
        $scope.showDetail = function (action, id) {

            $scope.onClickTab($scope.tabs[0]);

            $scope.entityId = id;

            viewDetail();

            initControls(action);

            if ($scope.entityId > 0) {
                $scope.showDetailWarehose();
            }
        };


        //#region Detail
        $scope.performActionWareHouse = function (source, fromList) {
            var action = source.currentTarget.attributes["action"].value;
            if (action === 'search') {

                 _criteria = [];

                //$scope.page.criteria.forEach(function (filter) {
                //    var fieldValue = $("input[name='" + filter.name + "F']").val();
                //    if (fieldValue && fieldValue != "") {
                //        filter.value = fieldValue;
                //        filter.operator = "contains";
                //        _criteria.push(filter);
                //    }
                //});

                 $scope.criteria = {

                     name: $("input[name='NameF']").val(),
                     challanno: $("input[name='ChallanNoF']").val(),
                     challandate: $("input[name='ChallanDateF']").val(),
                     challanenddate: $("input[name='ChallanEndDateF']").val(),

                 };

                $scope.listTablewarehouse.reload();
            }
            if (action == "save" && $scope.validateAction != undefined) {
                if (!$scope.validateAction(source)) {
                    return;
                }
            }
            if (fromList) {
                $scope.showDetailWarehose(action, source.currentTarget.attributes["entityId"].value);
            } else {
                initControls(action);
            }

            switchTab("Detail", action);

            switch (action) {
                case "search":
                    //filterList();
                    break;
                case "add":



                    lastAction = action;
                    $scope.entityId = 0;
                    $scope.entity = {};
                    $timeout(function () {
                        $('#selblocknumber').multiselect({buttonWidth:'100%'});
                        $('.multiselect-container').css('width', '300px');
                    }, 500);

                    $("input[input-date]").each(function (index, element) { $(element).val(null); });
                    $scope.warehouseinwardno();
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
                    $scope.getDetailwarehouse();
                    lastAction = "";
                    break;
                case "delete":
                    $scope.entityId = source.currentTarget.attributes["entityId"].value;
                    removewarehouse();
                    lastAction = "";
                    break;
                case "deleteBatch":
                    removeBatch();
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
        $scope.criteria = function()
        {
            _criteria = [];
            //$scope.page.criteria.forEach(function (filter) {
            //    var fieldValue = $("input[name='" + filter.name + "F']").val();
            //    if (fieldValue && fieldValue != "") {
            //        filter.value = fieldValue;
            //        filter.operator = "contains";
            //        _criteria.push(filter);
            //    }
            //});
            



            $scope.criteria = {  
                
                name: $("input[name='NameF']").val(),
                challanno: $("input[name='ChallanNoF']").val(),
                challandate: $("input[name='ChallanDateF']").val(),
                challanenddate: $("input[name='ChallanEndDateF']").val(),
                quotationno: $("input[name='QuotationNoF']").val(),
                shippername: $("input[name='ShipperNameF']").val()
            }

        }

        $scope.listTablewarehouse = new NgTableParams(
    {
        page: 1,
        count: 10,
        sorting: $.parseJSON("{ \"" + $scope.page.sortField + "\": \"" + $scope.page.sortType + "\" }")
    }, {
        getData: function (params) {
            $scope.criteria = {
                name: $("input[name='NameF']").val(),
                challanno: $("input[name='ChallanNoF']").val(),
                challandate: $("input[name='ChallanDateF']").val(),
                challanenddate: $("input[name='ChallanEndDateF']").val(),
                quotationno: $("input[name='QuotationNoF']").val(),
                shippername: $("input[name='ShipperNameF']").val()
            }
            var listParams = {
                SiteId: $scope.selectedSiteId,
                UserId: $scope.authentication.userId,
                UserWorkTypeId: $scope.$parent.userWorkTypeId,
                ModuleId: $scope.page.moduleId,
                PageIndex: params.page(),
                PageSize: params.count(),
                Sort: JSON.stringify(params.sorting()),
                Filter: JSON.stringify($scope.criteria)
            };
            return entityService.wareHouseList(listParams).then(
                function (output) {
                    $scope.validateUser(output);
                    $scope.items = output.data.data;
                    params.total(output.data.count);
                    return output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        }
    });
        //$scope.criteria();
        //$scope.listTablewarehouse.reload();
        $scope.beforeSave = function (action, lastAction) {

            if (lastAction == "add") {
                $scope.entity.createdOn = new Date();
                $scope.entity.createdBy = $scope.$parent.userInfo.usrId;
                
            }
            else {
                $scope.entity.modifiedOn = new Date();
                $scope.entity.modifiedBy = $scope.$parent.userInfo.usrId; 
            }
        };

        //#endregion

        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));

    };

    controller.$inject = injectParams;

    app.register.controller("warehousemappingController", controller);

});
