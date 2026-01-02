"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$filter", "$window", "$timeout", "$location", "$routeParams", "$uibModal", "localStorageService", "NgTableParams", "ngNotifier", "corporatedashboardService"];

    var controller = function ($scope, $filter, $window, $timeout, $location, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService) {

        //#region General

        $scope.page = appUrl.dashboard;
        $scope.tabs = appUrl.dashboard.tabs;
        $scope.chartInqValue = 10;
        $scope.chartQutvalue = 10;
        $scope.chartBkqValue = 10;
        $scope.citiesData = [];
        $scope.barChartData = [];
        $scope.targetlist = [];
        $scope.lookups = { sites: [] };
        $scope.sitelist = [];
        $scope.frenchieId = [];
        $scope.frenchieIdreport = [];
        $scope.targetYear = "";
        $scope.iscorporat = false;
        //$scope.initDropdown = function () {
        //  //  $scope.fetchLookupData("CustomerContact", 0, "SitName", "sitelist", "SiteList");
            


        //};
        setTimeout(() => {

            debugger;
            $scope.frenchieIdreport.push($scope.$parent.selectedSiteId);
            $scope.frenchieId.push($scope.$parent.selectedSiteId);

          var s =   {"SitId":"0","LcnId":"126","SitType":1,"SitName":"All","SitCode":"EWR","SitStatus":true,"SitReason":null,"SitCreatedTs":"2017-04-21T00:29:53.417","SitUpdatedTs":"2017-11-1"}
          $scope.sitelist.push(s);
          $scope.authentication.userSite.forEach(function (o) {
            $scope.sitelist.push(o);
        });
        },500);
        if ($scope.userWorkTypeId == 1)
            $scope.iscorporat = false
        else
            $scope.iscorporat = true;
        var cdata = new Date();
        let cyear = cdata.getFullYear();
        $scope.targetYear = cyear.toString();
       /* $scope.afterFetchLookupData = function (lookupKey) {
            if (lookupKey == "sitelist") {

                if ($scope.lookups.sitelist != undefined) {
                    if ($scope.lookups.sitelist.length > 0) {
                        $scope.lookups.sitelist.forEach(function (o) {

                            $scope.sitelist.push(o);

                        });
                    };
                };
            };
        };
        */
        //#endregion
        $scope.sendData = function (optionvalue, target) {
            localStorageService.set("dashboardOption", optionvalue);
            if (target == "enquiry") {
                $scope.$parent.pageTitle = "Enquiry";
                $scope.$parent.breadcrumbs = ["Shipment", "Enquiry"];
                $location.path("/operation/enquiry");
            }
            if (target == "quotation") {
                $scope.pageTitle = "Create Quotation";      
                $scope.breadcrumbs = ["Shipment", "Quotation", "Create Quotation"];
                $location.path("/operation/quotation");
            }
            if (target == "qutPendingApproval") {
                $scope.pageTitle = "Pending Shipper Approval";
                $scope.breadcrumbs = ["Shipment", "Quotation", "Pending Shipper Approval"];
                $location.path("/operation/quotationApproval");
            }
            if (target == "quotationBooking") {
                $scope.pageTitle = "Send for Booking";
                $scope.breadcrumbs = ["Shipment", "Quotation", "Send for Booking"];
                $location.path("/operation/quotationBooking");
            }
            if (target == "spacebooking") {
                $scope.pageTitle = "Space/cancel Booking";
                $scope.breadcrumbs = ["Shipment", "Booking", "Space/cancel Booking"];
                $location.path("/operation/bookingSpace");
            }
            if (target == "bookingToDo") {
                $scope.pageTitle = "Booking To Do List";
                $scope.breadcrumbs = ["Shipment", "Booking", "Booking To Do"];
                $location.path("/operation/booking");
            }
            if (target == "bkgWaitingLineConf") {
                $scope.pageTitle = "Waiting for Line Confirmation";
                $scope.breadcrumbs = ["Shipment", "Booking", "Waiting for Line Confirmation"];
                $location.path("/operation/bookingDocument");
            }
            if (target == "bookingConfReadyToSend") {
                $scope.pageTitle = "Booking Conf Ready To Send";
                $scope.breadcrumbs = ["Shipment", "Booking", "Booking Conf Ready To Send"];
                $location.path("/operation/bookingConfReadyToSend");
            }
            if (target == "bkgShippersConf") {
                $scope.pageTitle = "Awaited Shipper Confirmation";
                $scope.breadcrumbs = ["Shipment", "Booking", "Awaited Shipper Confirmation"];
                $location.path("/operation/bookingShipperConfirmation");
            }
            if (target == "finalizedTrans") {
                $scope.pageTitle = "Finalized Transportation";
                $scope.breadcrumbs = ["Shipment", "Trucking", "Finalized Transportation"];
                $location.path("/operation/finalizedTransportation");
            }
            if (target == "docRecipt") {
                $scope.pageTitle = "Dock Receipt";
                $scope.breadcrumbs = ["Document", "Dock Receipt"];
                $location.path("/document/dockReceipt");
            }
            if (target == "pendingMovement") {
                $scope.pageTitle = "Pending Movement";
                $scope.breadcrumbs = ["Shipment", "Trucking", "Pending Movement"];
                $location.path("/operation/pendingMovement");
            }
            if (target == "captureContainer") {
                $scope.pageTitle = "Capture Container & Seal No";
                $scope.breadcrumbs = ["Shipment", "Trucking", "Capture Container & Seal No"];
                $location.path("/operation/bookingCaptureContainer");
            }
            if (target == "invoice") {
                $scope.pageTitle = "SP MCS";
                $scope.breadcrumbs = ["Report","SP MCS"];
                $location.path("/report/mcsReport");
            }
        };

        var totalActivity = function (siteId) {
            entityService.getTotalActivity(siteId).then(
                function (output) {
                    $scope.Total = output.data.data;
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

        $scope.getSiteLocations = function () {
            entityService.getSiteLocations().then(
                function (output) {
                    $scope.citiesData = output.data.data;
                    for (var k = 0; k < $scope.citiesData.length; k++) {
                        $scope.cities.push({
                            city: $scope.citiesData[k].lcnCity,
                            desc: $scope.citiesData[k].lcnAddress1
                        });
                    };
                    $scope.loadGoogleMap();
                },
                function (output) {
                    ngNotifier.showError($scope.authentication, output);
                }
            );
        };

        var loadBarChart = function () {
            var barChartCanvas = $("#barChart").get(0).getContext("2d");
            var barChart = new Chart(barChartCanvas);
            var barChartData = $scope.barChartData;
            barChartData.datasets[0].fillColor = "#3b8bba";
            barChartData.datasets[0].strokeColor = "#3b8bba";
            barChartData.datasets[0].pointColor = "#3b8bba";
            barChartData.datasets[1].fillColor = "#00a65a";
            barChartData.datasets[1].strokeColor = "#00a65a";
            barChartData.datasets[1].pointColor = "#00a65a";
            var barChartOptions = {
                //Boolean - Whether the scale should start at zero, or an order of magnitude down from the lowest value
                scaleBeginAtZero: true,
                //Boolean - Whether grid lines are shown across the chart
                scaleShowGridLines: true,
                //String - Colour of the grid lines
                scaleGridLineColor: "rgba(0,0,0,.05)",
                //Number - Width of the grid lines
                scaleGridLineWidth: 1,
                //Boolean - Whether to show horizontal lines (except X axis)
                scaleShowHorizontalLines: true,
                //Boolean - Whether to show vertical lines (except Y axis)
                scaleShowVerticalLines: true,
                //Boolean - If there is a stroke on each bar
                barShowStroke: true,
                //Number - Pixel width of the bar stroke
                barStrokeWidth: 2,
                //Number - Spacing between each of the X value sets
                barValueSpacing: 5,
                //Number - Spacing between data sets within X values
                barDatasetSpacing: 1,
                //String - A legend template
                legendTemplate: "<ul class=\"<%=name.toLowerCase()%>-legend\"><% for (var i=0; i<datasets.length; i++){%><li><span style=\"background-color:<%=datasets[i].fillColor%>\"></span><%if(datasets[i].label){%><%=datasets[i].label%><%}%></li><%}%></ul>",
                //Boolean - whether to make the chart responsive
                responsive: true,
                maintainAspectRatio: true
            };

            barChartOptions.datasetFill = false;
            barChart.Bar(barChartData, barChartOptions);
        };
        
        $scope.fillDashboardTable = function () {
            $scope.frenchieId.push( $scope.$parent.selectedSiteId);
            entityService.getDashBoardData($scope.$parent.selectedSiteId).then(
                        function (output) {
                            $scope.validateUser(output);
                            if (output.data.data != null) {
                                //$scope.items = output.data.data;
                                $scope.entity = output.data.data;
                                //YTD
                                $scope.barChartData.datasets[0].data[0] = $scope.entity.eytd;
                                $scope.barChartData.datasets[0].data[1] = $scope.entity.qytd;
                                $scope.barChartData.datasets[0].data[2] = $scope.entity.bytd;
                                //MTD
                                $scope.barChartData.datasets[1].data[0] = $scope.entity.emtd;
                                $scope.barChartData.datasets[1].data[1] = $scope.entity.qmtd;
                                $scope.barChartData.datasets[1].data[2] = $scope.entity.bmtd;
                                loadBarChart();
                                //totalActivity($scope.$parent.selectedSiteId);
                            }

                            //return output.data.data;
                        },
                        function (output) {
                            ngNotifier.showError($scope.authentication, output);
                        }
            );
            if ($scope.$parent.selectedSiteId == "0")
                $scope.frenchieId.push(117);
            else
            $scope.frenchieId.push( $scope.$parent.selectedSiteId);

            
        };
        
        $scope.changeFrenchieId = function (frenchiedid) {
           
            var targetYear = $scope.targetYear;
            $scope.fillTargetTableData(frenchiedid, targetYear);
        };
        $scope.changeTargetYear = function (TargetYear) {
           // var frenchieId = $scope.frenchieId;
           
            var frenchieI = "";
            if ($scope.frenchieId.length > 0) {
                frenchieI = $scope.frenchieId.join();
            }
            else {
                frenchieI = $scope.frenchieId;
            }

            $scope.fillTargetTableData(frenchieI, TargetYear);
        };
        $scope.fillDashboardTable();
        $scope.ViewTargetTableData = function () {
            
            var frenchieI = "";
            if ($scope.frenchieId.length > 0) {
                frenchieI = $scope.frenchieId.join();
            }
            else {
                frenchieI = $scope.frenchieId;
            }
            $scope.fillTargetTableData(frenchieI, $scope.targetYear);
        };
        $scope.fillTargetTableData = function (frenchieId, TargetYear) {
            $scope.reportTable = new NgTableParams(
                {

                    page: 1,
                    count: 10,
                    sorting: $.parseJSON("{ \"SerialNo\": \"asc\" }"),
                    group: {
                        franchiseId: "asc"
                    }
                }, {
                counts: [],
                getData: function (params) {

                    var listParams = {

                        FrenchieId: frenchieId,
                        TargetYear: TargetYear
                    };
                    return entityService.getDashBoardTargetData(listParams).then(
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
        };
        $scope.fillTargetTableData($scope.frenchieId.join(), $scope.targetYear);
        //get site locations for google maps
        $scope.getSiteLocations();

        //datasource for chart
        $scope.dataSource = {
            "chart": {
                "caption": "",
                "xAxisname": "",
                "yAxisName": "Count",
                "numberPrefix": "",
                "plotFillAlpha": "80",
                "paletteColors": "#0075c2,#1aaf5d",
                "baseFontColor": "#333333",
                "baseFont": "Helvetica Neue,Arial",
                "captionFontSize": "14",
                "subcaptionFontSize": "14",
                "subcaptionFontBold": "0",
                "showBorder": "0",
                "bgColor": "#ffffff",
                "showShadow": "0",
                "canvasBgColor": "#ffffff",
                "canvasBorderAlpha": "0",
                "divlineAlpha": "100",
                "divlineColor": "#999999",
                "divlineThickness": "1",
                "divLineDashed": "1",
                "divLineDashLen": "1",
                "usePlotGradientColor": "0",
                "showplotborder": "0",
                "valueFontColor": "#ffffff",
                "placeValuesInside": "1",
                "showHoverEffect": "1",
                "rotateValues": "1",
                "showXAxisLine": "1",
                "xAxisLineThickness": "1",
                "xAxisLineColor": "#999999",
                "showAlternateHGridColor": "0",
                "legendBgAlpha": "0",
                "legendBorderAlpha": "0",
                "legendShadow": "0",
                "legendItemFontSize": "10",
                "legendItemFontColor": "#666666",
                "formatNumber": "0",
                "formatNumberScale": "0"
            },
            "categories": [
                {
                    "category": [
                        {
                            "label": "Inquiry"
                        },
                        {
                            "label": "Quotation"
                        },
                        {
                            "label": "Booking"
                        }
                    ]
                }
            ],
            "dataset": [
                {
                    "seriesname": "YTD",
                    "data": [
                        {
                            "value": "1"
                        },
                        {
                            "value": "1"
                        },
                        {
                            "value": "1"
                        }
                    ]
                },
                {
                    "seriesname": "MTD",
                    "data": [
                        {
                            "value": "1"
                        },
                        {
                            "value": "1"
                        },
                        {
                            "value": "1"
                        }
                    ]
                }
            ]
        };

        //#endregion

        //#region Barchart

        $scope.barChartData = {
            labels: ["Inquiry", "Quotation", "Booking"],
            datasets: [
                {
                    label: "YTD",
                    fillColor: "rgba(210, 214, 222, 1)",
                    strokeColor: "rgba(210, 214, 222, 1)",
                    pointColor: "rgba(210, 214, 222, 1)",
                    pointStrokeColor: "#c1c7d1",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(220,220,220,1)",
                    data: [0, 0, 0]
                },
                {
                    label: "MTD",
                    fillColor: "rgba(60,141,188,0.9)",
                    strokeColor: "rgba(60,141,188,0.8)",
                    pointColor: "#3b8bba",
                    pointStrokeColor: "rgba(60,141,188,1)",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(60,141,188,1)",
                    data: [0, 0, 0]
                }
            ]
        };

        //#endregion

        //#region Google Maps

        $scope.cities = [];
        $scope.loadGoogleMap = function () {
            $scope.markers = [];
            $scope.map = new google.maps.Map(document.getElementById('map'), {
                mapTypeId: google.maps.MapTypeId.TERRAIN,
                center: new google.maps.LatLng(40.0000, -98.0000),
                zoom: 1
            });
            $scope.infoWindow = new google.maps.InfoWindow({});
            $scope.createMarker = function (info) {
                var geocoder = new google.maps.Geocoder();
                geocoder.geocode({
                    'address': info.city
                },
                    function (results, status) {
                        if (status == google.maps.GeocoderStatus.OK) {
                            var marker = new google.maps.Marker({
                                position: results[0].geometry.location,
                                map: $scope.map,
                                title: info.city,
                                content: '<div class="infoWindowContent">' + info.desc + '</div>'
                            });


                            google.maps.event.addListener(marker, 'click', function () {
                                $scope.infoWindow.setContent('<h2>' + marker.title + '</h2>' + marker.content);
                                $scope.infoWindow.open($scope.map, marker);
                            });
                            $scope.markers.push(marker);
                        }

                    });
            };
            for (var j = 0; j < $scope.cities.length; j++) {
                $scope.createMarker($scope.cities[j]);
            };
        };

        $scope.redirectTo = function (target, id) {

            //if (target == "home") {
            //    $scope.pageTitle = "Home";
            //    $scope.breadcrumbs = ["Home"];
            //    $location.path("/home");
            //}
            if (target == "dashboard") {
                $scope.pageTitle = "Dashboard";
                $scope.breadcrumbs = ["Dashboard"];
                $location.path("/home/dashboard");
            }
            else if (target == "profile") {
                $scope.pageTitle = "My Profile";
                $scope.breadcrumbs = ["My Profile"];
                $location.path("/security/profile");
            }
            else if (target == "enquiry") {
                $scope.$parent.pageTitle = "Enquiry";
                $scope.$parent.breadcrumbs = ["Shipment", "Enquiry"];
                $location.path("/operation/enquiry");
                $window.location.reload();
            }
            else if (target == "quotation") {
                $scope.pageTitle = "Create Quotation";
                $scope.breadcrumbs = ["Shipment", "Quotation", "Create Quotation"];
                $location.path("/operation/quotation");
            }
            else if (target == "booking") {
                $scope.pageTitle = "Approved Quotations";
                $scope.breadcrumbs = ["Shipment", "Booking", "Approved Quotations"];
                $location.path("/operation/booking");
            }
            else if (target == "dockReceipt") {
                $scope.pageTitle = "Dock Receipt";
                $scope.breadcrumbs = ["Document", "Dock Receipt"];
                $location.path("/document/dockReceipt");
            }
        };

        $scope.exportReport = function () {
            debugger;
            var sids = '';
            var opt = $('select[name="selectedSiteId"] option');   

            var frenchieI = "";
            if ($scope.frenchieIdreport.length > 0) {
                frenchieI = $scope.frenchieIdreport.join();
            }
            else {
                frenchieI = $scope.frenchieIdreport;
            }
            sids = frenchieI;
            if ($scope.frenchieIdreport.indexOf("0") !== -1)
            {
                sids = '';
                $('select[name="selectedSiteId"] option').each(function (i, obj) {
                    sids = sids + obj.value.split(':')[1] + ",";
                });
                if (sids != '')
                    sids = sids.substr(0, sids.length - 1);
            }
           
            var reportParams = {               
                SIT_ID: sids 
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
        
        angular.extend(this, new baseController($scope, $timeout, $routeParams, $uibModal, localStorageService, NgTableParams, ngNotifier, entityService));
    };

    controller.$inject = injectParams;

    app.register.controller("corporatedashboardController", controller);

});
