"use strict";

define(["app"], function (app) {

    var injectParams = ["$scope", "$timeout", "$location", "localStorageService", "ngNotifier", "authService", "ngAuthSettings"];

    var controller = function ($scope, $timeout, $location, localStorageService, ngNotifier, authService, ngAuthSettings) {

        $scope.slides = [
            {
                id: 0,
                heading: "MGL Logistic",
                description: "MGL Logistic offers the broadest range of deployment options for on-premise and cloud, and is the industry’s most highly customizable Logistic platform based on open technologies. Only MGL gives users the level of flexibility needed to truly create differentiated, winning Logistic strategies. Whether onsite, in the cloud, or On-Demand, MGL offers the same level of functionality, customization and integration with 3rd party systems and tools. This means starting your Logistic initiative knowing that MGL scales with your business— no matter how large or customized.",
                image: "Images/Slides/slide-crm.png"
            },
            {
                id: 1,
                heading: "MGL Inventory",
                description: "Designed with the individual in mind, MGL Logistic offers the most innovative and intuitive user experience on the market. With MGL modern and immersive interface, every customer-facing employee can effectively engage with customers every time thanks to a consistent MGL experience regardless of your access point or device. Embedded collaboration tools help break down departmental silos and increase engagement and service levels.",
                image: "Images/Slides/slide-inv.png"
            },
            {
                id: 2,
                heading: "MGL Freight",
                description: "MGL Logistic offers the most innovative, flexible and affordable Logistic in the market and delivers the best all-around value of any Logistic solution in the industry. However, the real value of MGL goes far beyond the low total cost of ownership customers have enjoyed for more than a decade. In addition to the robust sales, marketing and support features, businesses can customize and build on the MGL platform without hidden fees or forced upgrades to more costly editions. Additionally, users can make any number of integrations without additional charges or fees. ",
                image: "Images/Slides/slide-lis.png"
            }
        ];

        (function (authService) {
            if (!authService.authentication.isAuth) {
                $location.path("/login");
            }
        })(authService);

    };

    controller.$inject = injectParams;

    app.controller("homeController", controller);

});
