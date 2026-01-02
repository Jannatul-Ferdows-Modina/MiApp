"use strict";

define(["app", "toastr"], function (app, toastr) {

    var notifier = function ($log, $window, $location, ngAuthSettings) {

        toastr.options = {
            closeButton: true
        };

        return {
            success: function (message, header) {
                if (header == null) header = "Success";
                toastr.success(message, header);
            },
            error: function (message, header) {
                if (header == null) header = "Error";
                toastr.error(message, header);
            },
            info: function (message, header) {
                if (header == null) header = "Information";
                toastr.info(message, header);
            },
            warn: function (message, header) {
                if (header == null) header = "Warning";
                toastr.warning(message, header);
            },
            confirm: function (message, header, okCallback, cancelCallback) {
                if (header == null) header = "Confirmation";
                message = "<div><p>" + message + "</p><div><button type=\"button\" class=\"btn btn-primary\" id=\"btnOk\">OK</button><button type=\"button\" class=\"btn btn-default\" id=\"btnCancel\">Cancel</button></div></div>";
                toastr.options.timeOut = 0;
                toastr.warning(message, header)
                    .on("click", "#btnOk", okCallback)
                    .on("click", "#btnCancel", cancelCallback);
                toastr.options.timeOut = 5000;
            },
            show: function (data) {
                if (data) {
                    if (data.resultId === 1001) {
                        toastr.success(data.messages, "Success");
                    }
                    else if (data.resultId === 2002) {
                        toastr.error(data.messages, "Error");
                    }
                    else {
                        toastr.error(data.messages, "Error");
                    }
                }
                else {
                    toastr.error("", "Error");
                    $.ajax({
                        type: "POST",
                        url: ngAuthSettings.apiServiceBaseUri + "api/logs/error",
                        contentType: "application/json",
                        data: angular.toJson({
                            userName: $("span[name=spanUserName]").text(),
                            url: $window.location.href,
                            browser: Utility.getBrowser(),
                            userAgent: navigator.userAgent,
                            message: Utility.messageEncode(data),
                            type: "error"
                        })
                    });
                }
            },
            showError: function (info, output, header) {
                if (!info.useRefreshTokens) {
                    if (header == null) header = "Error";
                    toastr.error((output.status == 401 ? "Session Expired" : output.data.messages), header);
                }
            },
            logError: function (message) {
                $log.error.apply($log, arguments);
                $.ajax({
                    type: "POST",
                    url: ngAuthSettings.apiServiceBaseUri + "api/logs/error",
                    contentType: "application/json",
                    data: angular.toJson({
                        userName: $("span[name=spanUserName]").text(),
                        url: $window.location.href,
                        browser: Utility.getBrowser(),
                        userAgent: navigator.userAgent,
                        message: Utility.messageEncode(message),
                        type: "error"
                    })
                });
            },
            logWarn: function (message) {
                $log.log.apply($log, arguments);
                $.ajax({
                    type: "POST",
                    url: ngAuthSettings.apiServiceBaseUri + "api/logs/warn",
                    contentType: "application/json",
                    data: angular.toJson({
                        userName: $("span[name=spanUserName]").text(),
                        url: $window.location.href,
                        browser: Utility.getBrowser(),
                        userAgent: navigator.userAgent,
                        message: Utility.messageEncode(message),
                        type: "warn"
                    })
                });
            },
            logInfo: function (message) {
                $log.log.apply($log, arguments);
                $.ajax({
                    type: "POST",
                    url: ngAuthSettings.apiServiceBaseUri + "api/logs/info",
                    contentType: "application/json",
                    data: angular.toJson({
                        userName: $("span[name=spanUserName]").text(),
                        url: $window.location.href,
                        browser: Utility.getBrowser(),
                        userAgent: navigator.userAgent,
                        message: Utility.messageEncode(message),
                        type: "info"
                    })
                });
            }
        };
    };

    app.factory("ngNotifier", notifier);
});
