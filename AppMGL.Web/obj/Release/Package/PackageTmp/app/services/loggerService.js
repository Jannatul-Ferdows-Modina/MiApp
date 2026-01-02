"use strict";

define(["app", "stacktrace"], function (app, StackTrace) {

    var stacktraceService = function () {
        return ({
            StackTrace: StackTrace
        });
    };

    var errorLoggerService = function ($log, $window, stacktraceService, ngAuthSettings) {
        function error(exception, cause) {
            $log.error.apply($log, arguments);
            try {
                stacktraceService.StackTrace
                    .fromError(exception)
                    .then(function (stackframes) {
                        $.ajax({
                            type: "POST",
                            url: ngAuthSettings.apiServiceBaseUri + "api/logs/error",
                            contentType: "application/json",
                            data: angular.toJson({
                                userName: $("span[name=spanUserName]").text(),
                                url: $window.location.href,
                                browser: Utility.getBrowser(),
                                userAgent: navigator.userAgent,
                                message: Utility.messageEncode(exception.toString()),
                                type: "error",
                                stackTrace: stackframes.join("\r\n              "),
                                cause: (cause || "")
                            })
                        });
                    })
                    .catch(function (err) {
                        $log.warn("Error client-side logging failed");
                        $log.log(err);
                    });
            } catch (loggingError) {
                $log.warn("Error server-side logging failed");
                $log.log(loggingError);
            }
        }
        return (error);
    };

    var clientLogger = angular.module("clientLoggerService", []);

    clientLogger.factory("stacktraceService", stacktraceService);

    clientLogger.factory("errorLoggerService", errorLoggerService);

    clientLogger.provider("$exceptionHandler", {
        $get: function (errorLoggerService) {
            return (errorLoggerService);
        }
    });
});

