(function (window) {

    "use strict";
    function define_utility() {

        var Utility = {};

        //#region Browser Detection Properties

        // Opera 8.0+
        Utility.isOpera = (!!window.opr && !!opr.addons) || !!window.opera || navigator.userAgent.indexOf(' OPR/') >= 0;
        // Firefox 1.0+
        Utility.isFirefox = typeof InstallTrigger !== 'undefined';
        // Safari 3.0+ "[object HTMLElementConstructor]" 
        //Utility.isSafari = Object.prototype.toString.call(window.HTMLElement).indexOf('Constructor') > 0 || (function (p) { return p.toString() === "[object SafariRemoteNotification]"; })(!window['safari'] || safari.pushNotification);
        Utility.isSafari = !!navigator.userAgent.match(/Version\/[\d\.]+.*Safari/);
        // Internet Explorer 6-11
        Utility.isIE = /*@cc_on!@*/false || !!document.documentMode;
        // Edge 20+
        Utility.isEdge = !Utility.isIE && !!window.StyleMedia;
        // Chrome 1+
        Utility.isChrome = !!window.chrome && !!window.chrome.webstore;
        // Chrome 18+ (Apple)
        Utility.isChromeApple = !!navigator.userAgent.match('CriOS');
        // Chrome 18+
        Utility.isChromeMobile = !!navigator.userAgent.match(/Chrome\/[\d\.]+.*Safari/);
        // Blink engine detection
        Utility.isBlink = (Utility.isChrome || Utility.isOpera) && !!window.CSS;
        // Find Browser
        Utility.getBrowser = function () {
            if (Utility.isOpera) {
                return "Opera 8.0+";
            }
            else if (Utility.isFirefox) {
                return "Firefox 1.0+";
            }
            else if (Utility.isSafari) {
                return "Safari 3.0+";
            }
            else if (Utility.isIE) {
                return "Internet Explorer 6-11";
            }
            else if (Utility.isEdge) {
                return "Edge 20+";
            }
            else if (Utility.isChrome) {
                return "Chrome 1+";
            }
            else if (Utility.isChromeApple) {
                return "Chrome 18+ (Apple)";
            }
            else if (Utility.isChromeMobile) {
                return "Chrome 18+";
            }
            else if (Utility.isBlink) {
                return "Blink engine detection";
            }
            return "Browser Undetected";
        };

        //#endregion

        //#region Object Methods

        Utility.mergeOjectArray = function (newObject, oldObject, keyField) {

            $.merge(newObject, oldObject);

            var unique = [];

            newObject = $.grep(newObject, function (o) {
                if ($.inArray(o[keyField], unique) !== -1) {
                    return false;
                }
                else {
                    unique.push(o[keyField]);
                    return true;
                }
            });

            return newObject;
        };

        Utility.inArray = function (array, property, value) {

            var k = $.map(array, function (o, j) {
                if (o[property] == value) {
                    return j;
                }
            });

            if (k.length > 0) {
                return k[0];
            }

            return -1;
        };

        Utility.inPropertyArray = function (array, keyName, keyValue, returnName) {

            var output = null;

            array.some(function (item) {
                if (item[keyName] == keyValue) {
                    output = item[returnName];
                    return true;
                }
            });

            return output;
        };

        Utility.sumArray = function (array, field) {

            var total = 0.00;
            array.forEach(function (o) {
                total += Utility.unformat(o[field]);
            });
            return total;
        };

        Utility.sumCostArray = function (array, fieldA, fieldB) {

            var total = 0.00;
            array.forEach(function (o) {
                total += (Utility.unformat(o[fieldA]) * Utility.unformat(o[fieldB]));
            });
            return total;
        };

        Utility.sumUniqueArray = function (array, field, keyField) {

            var unique = [];
            var total = 0.00;

            array.forEach(function (o) {
                if ($.inArray(o[keyField], unique) === -1) {
                    unique.push(o[keyField]);
                    total += Utility.unformat(o[field]);
                }
            });

            return total;
        };

        //#endregion

        //#region String Methods

        Utility.lowerFirstChar = function (input) {

            return input.charAt(0).toLowerCase() + input.slice(1);
        };

        Utility.limitText = function (value, limit, ishtml, tail) {

            if (!value) {
                return "";
            }

            if (ishtml) {
                value = $(value).text();
            }

            limit = parseInt(limit);

            if (!limit) {
                return value;
            }

            if (value.length <= limit) {
                return value;
            }

            value = value.substring(0, limit);

            return value + (tail || " ...");
        };

        Utility.messageEncode = function (input) {

            if (input) {
                return input.replace(/{/g, "{{").replace(/}/g, "}}");
            }
            return "";
        };

        Utility.urlEncode = function (input) {
            var output = input;
            var binVal, thisString;
            var regExp = /(%[^%]{2})/;
            while ((match = regExp.exec(output)) != null && match.length > 1 && match[1] != '') {
                binVal = parseInt(match[1].substr(1), 16);
                thisString = String.fromCharCode(binVal);
                output = output.replace(match[1], thisString);
            }
            return output;
        }

        //#endregion

        //#region Date Methods

        Utility.addDays = function (date, days) {
            var result = date || (new Date());
            result.setDate(result.getDate() + days);
            return result;
        };

        Utility.addWeeks = function (date, weeks) {
            var result = date || (new Date());
            result.setDate(result.getDate() + (weeks * 7));
            return result;
        };

        Utility.addMonths = function (date, months) {
            var result = date || (new Date());
            result.setMonth(result.getMonth() + months);
            return result;
        };

        Utility.addQuarters = function (date, quarters) {
            var result = date || (new Date());
            result.setMonth(result.getMonth() + (quarters * 3));
            return result;
        };

        Utility.addYears = function (date, years) {
            var result = date || (new Date());
            result.setFullYear(result.getFullYear() + years);
            return result;
        };

        Utility.getDateISO = function (date) {
            date = date || (new Date());
            var result = (new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()))).toISOString();
            return result.substring(0, result.lastIndexOf("."));
        };

        Utility.getDateTimeISO = function (date, hours, minutes, seconds) {
            date = date || (new Date());
            var result = (new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds()))).toISOString();
            if (hours != null && minutes != null && seconds != null) {
                result = (new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), hours, minutes, seconds))).toISOString();
            }
            return result.substring(0, result.lastIndexOf("."));
        };

        Utility.getFirstDate = function (date, type, offset) {
            date = date || (new Date());
            offset = offset || 0;
            var result;
            switch (type) {
                case "week":
                    result = Utility.addDays(new Date(date.setDate(date.getDate() - date.getDay())), offset);
                    break;
                case "month":
                    result = new Date(date.getFullYear(), date.getMonth(), 1);
                    break;
                case "quarter":
                    result = new Date(date.getFullYear(), 3 * Math.floor((date.getMonth() + 1) / 3) - 3, 1);
                    break;
                case "year":
                    result = new Date(date.getFullYear(), 0, 1);
                    break;
                default:
                    result = date;
                    break;
            }
            return result;
        };

        Utility.getLastDate = function (date, type, offset) {
            date = date || (new Date());
            offset = offset || 0;
            var result;
            switch (type) {
                case "week":
                    result = Utility.addDays(new Date(date.setDate(date.getDate() - date.getDay() + 6)), offset);
                    break;
                case "month":
                    result = new Date(date.getFullYear(), date.getMonth() + 1, 0);
                    break;
                case "quarter":
                    result = Utility.addDays(new Date(date.getFullYear(), 3 * Math.floor((date.getMonth() + 1) / 3), 1), -1);
                    break;
                case "year":
                    result = new Date(date.getFullYear(), 11, 31);
                    break;
                default:
                    result = date;
                    break;
            }
            return result;
        };

        Utility.isValidDate = function (date) {
            var flag = false;
            if (!isNaN(Date.parse(date))) {
                flag = true;
            }
            return flag;
        };

        //#endregion

        //#region Math Methods

        Utility.getRandomInt = function (min, max) {
            return Math.floor(Math.random() * (max - min + 1)) + min;
        };

        Utility.unformat = function (value, decimal) {

            // Fails silently (need decent errors):
            value = value || 0;

            // Return the value as-is if it's already a number:
            if (typeof value === "number") return value;

            // Default decimal point comes from settings, but could be set to eg. "," in opts:
            decimal = decimal || ".";

            // Build regex to strip out everything except digits, decimal point and minus sign:
            var regex = new RegExp("[^0-9-" + decimal + "]", ["g"]),
                unformatted = parseFloat(
                    ("" + value)
                        .replace(/\((.*)\)/, "-$1") // replace bracketed values with negatives
                        .replace(regex, '')         // strip out any cruft
                        .replace(decimal, '.')      // make sure decimal point is standard
                );

            // This will fail silently which may cause trouble, let's wait and see:
            return !isNaN(unformatted) ? unformatted : 0;
        };

        //#endregion

        //#region Timer Methods

        Utility.sleep = function (milliseconds) {
            var start = new Date().getTime();
            for (var i = 0; i < 1e7; i++) {
                if ((new Date().getTime() - start) > milliseconds) {
                    break;
                }
            }
        };

        //#endregion

        //#region Filter Methods

        Utility.createFilter = function (name, type, fieldName, value, operator, valueT) {
            return {
                name: name,
                type: type,
                fieldName: (fieldName || name),
                value: (value || ""),
                operator: (operator || ""),
                valueT: (valueT || "")
            };
        };

        Utility.stringifyFilter = function (name, type, fieldName, value, operator, valueT) {

            var filter = [];
            filter.push({
                name: name,
                type: type,
                fieldName: (fieldName == null) ? name : fieldName,
                value: value,
                operator: operator,
                valueT: valueT
            });
            return JSON.stringify(filter);
        };

        Utility.onChangeOperation = function (event, source) {

            var name = $(source).prop("name");
            name = name.substring(0, name.length - 2);

            if ($(source).val() == "between") {
                $("input[name='" + name + "T']").show();
            }
            else {
                $("input[name='" + name + "T']").hide().val("");
            }
        };

        //#endregion

        //#region Common Module Methods

        Utility.getName = function (contact) {
            return (contact.prefix ? contact.prefix + " " : "")
                + (contact.firstName ? contact.firstName + " " : "")
                + (contact.middleInitial ? contact.middleInitial + " " : "")
                + (contact.lastName ? contact.lastName : "")
                + (contact.suffix ? ", " + contact.suffix : "");
        };

        //#endregion

        return Utility;
    }

    //define globally if it doesn't already exist
    if (typeof (Utility) === 'undefined') {
        window.Utility = define_utility();
    }
    else {
        console.log("Utility already defined.");
    }

})(window);