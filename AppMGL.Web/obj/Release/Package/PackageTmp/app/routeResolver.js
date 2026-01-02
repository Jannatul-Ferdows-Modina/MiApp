"use strict";

define(["angular"], function (angular) {

    var routeResolver = function () {

        this.$get = function () {
            return this;
        };

        this.routeConfig = function () {

            //var webBaseUri = "";
            var webBaseUri = "/MGL";

            var componentsDirectory = webBaseUri + "/app/components/",
                controllersDirectory = webBaseUri + "/app/controllers/",
                servicesDirectory = webBaseUri + "/app/services/",
                viewsDirectory = webBaseUri + "/app/views/",

            setBaseDirectories = function (componentsDir, controllersDir, servicesDir, viewsDir) {
                componentsDirectory = componentsDir;
                controllersDirectory = controllersDir;
                servicesDirectory = servicesDir;
                viewsDirectory = viewsDir;
            },

            getViewsDirectory = function (flag) {
                if (flag) return componentsDirectory;
                return viewsDirectory;
            },

            getControllersDirectory = function (flag) {
                if (flag) return componentsDirectory;
                return controllersDirectory;
            },

            getServicesDirectory = function (flag) {
                if (flag) return componentsDirectory;
                return servicesDirectory;
            };

            return {
                setBaseDirectories: setBaseDirectories,
                getViewsDirectory: getViewsDirectory,
                getControllersDirectory: getControllersDirectory,
                getServicesDirectory: getServicesDirectory
            };

        }();

        this.route = function (routeConfig) {

            var resolveDependencies = function ($q, $rootScope, dependencies) {

                var defer = $q.defer();

                require(dependencies, function () {
                    $rootScope.$apply(function () {
                        defer.resolve();
                    });
                });

                return defer.promise;
            };

            var resolve = function (rp) {

                var routeDef = {};
                routeDef.templateUrl = appUrl[rp.module].urls.index;
                routeDef.controller = rp.module + "Controller";
                routeDef.secure = rp.hasOwnProperty("secure") ? rp.secure : false;
                routeDef.resolve = {
                    load: ["$q", "$rootScope", "ngAuthSettings", function ($q, $rootScope, ngAuthSettings) {
                        var dependencies = [];
                        $.each(appUrl[rp.module].depend, function (index, d) { dependencies.push("/" + ngAuthSettings.webBaseUri + d); });
                        return resolveDependencies($q, $rootScope, dependencies);
                    }]
                };

                return routeDef;
            };

            return {
                resolve: resolve
            };

        }(this.routeConfig);

    };

    var routeService = angular.module("routeResolverService", []);

    //Must be a provider since it will be injected into module.config()    
    routeService.provider("routeResolver", routeResolver);
});
