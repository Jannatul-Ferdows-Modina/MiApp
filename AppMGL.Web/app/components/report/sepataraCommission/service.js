   "use strict";

define(["app"], function (app) {

    var injectParams = ["$http", "$q", "ngAuthSettings", "Upload"];

    var service = function ($http, $q, ngAuthSettings, Upload) {

        var serviceBase = ngAuthSettings.apiServiceBaseUri + "api/mcsReport/";
        var serviceBase1 = ngAuthSettings.apiServiceBaseUri ;

        this.getMCSList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "GetMCSList_Sepatara/",
                method: "POST",
                data: listParams
            });
        };

        this.GetQBMCSList = function (listParams) {
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "GetQBMCSList/",
                method: "POST",
                data: listParams
            });
        };

       // debugger
        this.test = function (userid,entityid) {
            debugger
          //  alert('Come ' + serviceBase + "testmethod/" + "Userid" + entityid);
            return $http({
                headers: { 'Content-Type': "application/json", 'docnumber': entityid, 'Userid': userid },
                url: serviceBase + "testmethod/" + userid,
               // url: serviceBase + "testmethod/",
                method: "GET"
                
            });
        };


        this.QuckbookInvoiceReceiptnew = function (id, docnumber) {
            // alert('Come ' + serviceBase + "QuckbookInvoiceReceiptnew/" + test + "Userid" + userid);
            //var docnumber;
            //docnumber = id;
            //id = userid;
            return $http({
                headers: { 'Content-Type': "application/json", 'Pragma': "no-cache", 'Cache-Control': "no-cache", 'docnumber': docnumber },
                url: serviceBase + "QuckbookInvoiceReceiptnew/" + id,
                method: "GET"

            });
        };

        this.exportReport = function (reportParams) {
            return $http({
                cache: false,
                headers: { 'Content-Type': "application/json" },
                url: serviceBase + "ExportReport_Sepatara/",
                method: "POST",
                data: reportParams,
                responseType: "arraybuffer"
            });
        };
        this.qbRefreshTokens = function () {
            var t = serviceBase1 + "api/QuickBook/RefreshToken";
            return $http({
                headers: { 'Content-Type': "application/json" },
                url: t,
                method: "POST",
                data: {}
            });
        };

        this.resetinvoice = function (userid, docid) {
            debugger
            //  alert('Come ' + serviceBase + "testmethod/" + "Userid" + entityid);
            return $http({
                headers: { 'Content-Type': "application/json", 'docnumber': docid, 'Userid': userid },
                url: serviceBase + "ResetInvoice/" + userid,
                // url: serviceBase + "testmethod/",
                method: "POST"

            });
        };

        this.performQuickBookStatus = function (docno) {
           
            //  alert('Come ' + serviceBase + "testmethod/" + "Userid" + entityid);
            return $http({
                headers: { 'Content-Type': "application/json", 'docno': docno },
                url: serviceBase + "SearchInvoiceQB" ,
                // url: serviceBase + "testmethod/",
                method: "POST"

            });
        };

        this.addcomm = function (docid, comm) {
            debugger
            //  alert('Come ' + serviceBase + "testmethod/" + "Userid" + entityid);
            return $http({
                headers: { 'Content-Type': "application/json", 'docid': docid, 'comm': comm },
                url: serviceBase + "AddComm",
                // url: serviceBase + "testmethod/",
                method: "POST"

            });
        };


        angular.extend(this, new baseService($http, $q, ngAuthSettings, serviceBase, Upload, "report"));
    };

    service.$inject = injectParams;

    app.register.service("sepataraReportService", service);

});
