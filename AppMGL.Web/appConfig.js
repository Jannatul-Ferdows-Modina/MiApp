define(['require'], function (r) {
    'use strict';
    var config = {
        "mainLogo": "Images/l6.png",
       // "companyName": "Logistics Global Lines",
        "companyName": "Miami Global Lines",
       // "portalName": "Welcome to Logistics Portal",
        "portalName": "Welcome to MGL Portal",
       // "copyRights": "<strong>Copyright © 2019 Logistics Global Lines</strong>. All rights reserved.",
        "copyRights": "<strong>Copyright © 2019 Miami Global Lines</strong>. All rights reserved.",
        "footer": {
            //"copyRight": "Copyright © 2019 Logistics Global Lines",
            "copyRight": "Copyright © 2019 Miami Global Lines",
            "version": "5.0.0"
        },
        "header": {
           // "logo": "Images/Icons/fulllogo99.jpeg"
            "logo": "Images/Icons/mgl_icon_full.png"
                 },
        "idleTimeOut": 30, //value will consider as mins
        "userWarningTime": 30, //value will consider as secs
        "idleTimeOutHeaderMsg": "You're inactive. So your session will be expired now !",
        "idleTimeOutContentMsg": "Please move the mouse over the applicaiton for keep continue the session"
    };
    return config;
});
