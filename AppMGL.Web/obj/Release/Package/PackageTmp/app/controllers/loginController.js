"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$location", "$uibModal", "localStorageService", "ngNotifier", "authService", "ngAuthSettings"];

    var controller = function ($scope, $timeout, $location, $uibModal, localStorageService, ngNotifier, authService, ngAuthSettings) {

        $scope.regexEmail = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
        $scope.cookieData = [];
        $scope.loginData = {
            //userName: "fakir.m@gmail.com",
            //password: "Lg123456!",
            userName: "",
            password: "",
            useRefreshTokens: false
        };
        var getCookie = function () {
            var cookie = decodeURIComponent(document.cookie);
            var userNames = cookie ? cookie.split("userNames=")[1] : [];
            $scope.cookieData = userNames.length ? userNames.toString().split(",") : [];
            if ($scope.cookieData.length == 1) {
                $scope.loginData.userName = $scope.cookieData[0];
            }
            $scope.loginData.useRefreshTokens = $scope.cookieData.length > 0;
        };
        getCookie();
        $scope.login = function () {
            
            if (!$scope.regexEmail.test($scope.loginData.userName))
            {
                ngNotifier.error("Please enter valid Email");
                return;
            }
            authService.login($scope.loginData).then(
                function (output) {

                    var authData = {
                        token: output.data.access_token,
                        userId: output.data.userId,
                        userName: output.data.userName,
                        refreshToken: ($scope.loginData.useRefreshTokens ? output.data.refresh_token : ""),
                        useRefreshTokens: $scope.loginData.useRefreshTokens,
                        aspNetUserId: output.data.aspNetUserId,
                        userSite: JSON.parse(output.data.userSite)
                    };

                    if (eval(output.data.changePassword.toLowerCase())) {
                        callChangePassword(output, authData);
                    }else {
                        if ($scope.loginData.useRefreshTokens && $scope.cookieData.indexOf($scope.loginData.userName) === -1) {
                            $scope.cookieData.push($scope.loginData.userName);
                            var d = new Date();
                            d.setTime(d.getTime() +(30 * 24 * 60*60*1000));
                            var expires = "expires="+d.toUTCString();
                            document.cookie = 'userNames' + "=" + $scope.cookieData.toString() + ";" + expires;
                        }
                        afterLoginCalling(output, authData);
                    }
                },
                function (output) {
                    if (output.data.error && $.inArray(output.data.error, ["2004", "2007"]) != -1) {
                        ngNotifier.error(output.data.error_description);
                    }
                    else {
                        ngNotifier.error(output.statusText);
                    }
                }
            );
        };

        $scope.authExternalProvider = function (provider) {

            var redirectUri = location.protocol + "//" + location.host + "/authcomplete.html";

            var externalProviderUrl = ngAuthSettings.apiServiceBaseUri + "api/userAccount/ExternalLogin?provider=" + provider
                + "&response_type=token&client_id=" + ngAuthSettings.clientId + "&redirect_uri=" + redirectUri;
            window.$windowScope = $scope;

            var oauthWindow = window.open(externalProviderUrl, "Authenticate Account", "location=0,status=0,width=600,height=750");
        };

        $scope.authCompletedCB = function (fragment) {

            $scope.$apply(function () {
                if (fragment.haslocalaccount == "False") {
                    authService.logOut();
                    authService.externalAuthData = {
                        provider: fragment.provider,
                        userName: fragment.external_user_name,
                        externalAccessToken: fragment.external_access_token
                    };
                    $location.path("/associate");
                }
                else {
                    //Obtain access token and redirect to orders
                    var externalData = { provider: fragment.provider, externalAccessToken: fragment.external_access_token };
                    authService.obtainAccessToken(externalData).then(
                        function (output) {
                            $location.path("/home");
                        },
                        function (output) {
                            ngNotifier.error(output.statusText);
                        });
                }
            });
        };

        $scope.callResetPassword = function () {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "md",
                templateUrl: "app/components/security/resetPassword/index.html",
                controller: "resetPasswordController",
                resolve: {
                    requestData: function () {
                        return {
                            loginData: $scope.loginData
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.data && output.resultId == 1001) {
                        ngNotifier.show(output.data);
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        };

        var setAuthentication = function (authData) {

            authService.authentication.isAuth = true;
            authService.authentication.userId = authData.userId;
            authService.authentication.userName = authData.userName;
            authService.authentication.useRefreshTokens = authData.useRefreshTokens;
            authService.authentication.aspNetUserId = authData.aspNetUserId;
            authService.authentication.userSite = authData.userSite;
            authService.authentication.selectedSiteId = authData.selectedSiteId;
        };

        var callChangePassword = function (loginOutput, authData) {

            var modalInstance = $uibModal.open({
                animation: false,
                backdrop: "static",
                keyboard: false,
                size: "md",
                templateUrl: "app/components/security/changePassword/index.html",
                controller: "changePasswordController",
                resolve: {
                    requestData: function () {
                        return {
                            userId: 0,
                            aspNetUserId: authData.aspNetUserId
                        };
                    }
                }
            });

            modalInstance.result.then(
                function (output) {
                    if (output.data && output.resultId == 1001) {
                        //ngNotifier.show(output.data);
                        afterLoginCalling(loginOutput, authData);
                    }
                },
                function (output) {
                    ngNotifier.logError(output);
                });
        }

        var afterLoginCalling = function (output, authData) {

            var siteData = JSON.parse(output.data.userSite)

            if (siteData.length > 1) {

                var modalInstance = $uibModal.open({
                    animation: true,
                    backdrop: "static",
                    keyboard: false,
                    size: "sm",
                    templateUrl: "app/views/shared/selectSite.html",
                    controller: function ($scope, $timeout, $uibModalInstance, requestData) {
                        $scope.userSite = requestData.data;
                        $scope.sitId = requestData.data[0].SitId;
                        $scope.select = function () {
                            $uibModalInstance.close({ data: $scope.sitId, resultId: 1001 });
                        };
                    },
                    resolve: {
                        requestData: function () {
                            return {
                                data: siteData
                            };
                        }
                    }
                });

                modalInstance.result.then(
                    function (output) {
                        $scope.$parent.selectedSiteId = output.data;
                        authData.selectedSiteId = $scope.$parent.selectedSiteId;
                        setAuthentication(authData);
                        localStorageService.set("authData", authData);
                        sessionStorage.setItem('token', authData.token);
                        $scope.$emit("initMainPage", null);

                        $location.path($location.search().returnUrl);
                        $location.search({ returnUrl: null });
                    },
                    function (output) {
                        ngNotifier.logError(output);
                    });
            }
            else {
                if (siteData.length == 0) {
                    ngNotifier.error("There is no Unit to work with. Please check with MGL Adminstrator");
                    return;
                }
                else {
                    $scope.$parent.selectedSiteId = (siteData.length > 0) ? siteData[0].SitId : 0;
                    authData.selectedSiteId = $scope.$parent.selectedSiteId;
                    setAuthentication(authData);
                    localStorageService.set("authData", authData);
                    sessionStorage.setItem('token', authData.token);
                    $scope.$emit("initMainPage", null);

                    $location.path($location.search().returnUrl);
                    $location.search({ returnUrl: null });
                }
            }
        };
    };

    controller.$inject = injectParams;

    app.controller("loginController", controller);

});
