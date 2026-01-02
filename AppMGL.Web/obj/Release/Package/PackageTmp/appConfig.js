define(['require'], function (r) {
    'use strict';
    var config = {
        "mainLogo": "Images/logo04.gif",
        "companyName": "<b>MGL</b> Miami Global Lines",
        "portalName": "Welcome to MGL Portal",
        "copyRights": "<strong>Copyright © 2019 Miami Global Lines</strong>. All rights reserved.",
        "footer": {
            "copyRight": "Copyright © 2019 Miami Global Lines",
            "version": "3.0.0"
        },
        "header": {
            "logo": "Images/Icons/mgl_icon_full.png"
        },
        "idleTimeOut": 30, //value will consider as mins
        "userWarningTime": 30, //value will consider as secs
        "idleTimeOutHeaderMsg": "You're inactive. So your session will be expired now !",
        "idleTimeOutContentMsg": "Please move the mouse over the applicaiton for keep continue the session"
    };
    return config;
});
