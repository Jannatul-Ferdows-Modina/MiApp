"use strict";

define(["angular", "slimscroll"], function (angular) {

    /* --------------------
     * - AdminLTE Options -
     * --------------------
     * Modify these options to suit your implementation
     */
    var options = {
        //Add slimscroll to navbar menus
        //This requires you to load the slimscroll plugin
        //in every page before app.js
        navbarMenuSlimscroll: true,
        navbarMenuSlimscrollWidth: "3px", //The width of the scroll bar
        navbarMenuHeight: "200px", //The height of the inner menu
        //General animation speed for JS animated elements such as box collapse/expand and
        //sidebar treeview slide up/down. This options accepts an integer as milliseconds,
        //'fast', 'normal', or 'slow'
        animationSpeed: 500,
        //Sidebar push menu toggle button selector
        sidebarToggleSelector: "[data-toggle='offcanvas']",
        //Activate sidebar push menu
        sidebarPushMenu: true,
        //Activate sidebar slimscroll if the fixed layout is set (requires SlimScroll Plugin)
        sidebarSlimScroll: true,
        //Enable sidebar expand on hover effect for sidebar mini
        //This option is forced to true if both the fixed layout and sidebar mini
        //are used together
        sidebarExpandOnHover: false,
        //Bootstrap.js tooltip
        enableBSToppltip: true,
        BSTooltipSelector: "[data-toggle='tooltip']",
        //Enable Fast Click. Fastclick.js creates a more
        //native touch experience with touch devices. If you
        //choose to enable the plugin, make sure you load the script
        //before AdminLTE's app.js
        enableFastclick: true,
        //Control Sidebar Options
        enableControlSidebar: true,
        controlSidebarOptions: {
            //Which button should trigger the open/close event
            toggleBtnSelector: "[data-toggle='control-sidebar']",
            //The sidebar selector
            selector: ".control-sidebar",
            //Enable slide over content
            slide: true
        },
        //Direct Chat plugin options
        directChat: {
            //Enable direct chat by default
            enable: true,
            //The button to open and close the chat contacts pane
            contactToggleSelector: '[data-widget="chat-pane-toggle"]'
        },
        //Define the set of colors to use globally around the website
        colors: {
            lightBlue: "#3c8dbc",
            red: "#f56954",
            green: "#00a65a",
            aqua: "#00c0ef",
            yellow: "#f39c12",
            blue: "#0073b7",
            navy: "#001F3F",
            teal: "#39CCCC",
            olive: "#3D9970",
            lime: "#01FF70",
            orange: "#FF851B",
            fuchsia: "#F012BE",
            purple: "#8E24AA",
            maroon: "#D81B60",
            black: "#222222",
            gray: "#d2d6de"
        },
        //The standard screen sizes that bootstrap uses.
        //If you change these in the variables.less file, change
        //them here too.
        screenSizes: {
            xs: 480,
            sm: 768,
            md: 992,
            lg: 1200
        }
    };

    var adminLTE = angular.module("adminLTE", []);

    adminLTE.provider("adminLTEConfig", function () {

        this.options = options;

        this.setOptions = function (key, value) {
            this.options[key] = value;
        }

        this.$get = function () {
            return {
                options: this.options
            };
        };
    });

    /* Layout
     * ======
     * Fixes the layout height in case min-height fails.
     *
     * @type Object
     * @usage $.AdminLTE.layout.activate()
     *        $.AdminLTE.layout.fix()
     *        $.AdminLTE.layout.fixSidebar()
     */
    adminLTE.factory("adminLTE.layout", ["adminLTEConfig", function (adminLTEConfig) {
        var options = adminLTEConfig.options;
        var activate = function () {
            var _this = this;
            _this.fix();
            _this.fixSidebar();
            $(window, ".wrapper").resize(function () {
                _this.fix();
                _this.fixSidebar();
            });
        };
        var fix = function () {
            //Get window height and the wrapper height
            var neg = $('.main-header').outerHeight() + $('.main-footer').outerHeight();
            var window_height = $(window).height();
            var sidebar_height = $(".sidebar").height();
            //Set the min-height of the content and sidebar based on the
            //the height of the document.
            if ($("body").hasClass("fixed")) {
                $(".content-wrapper, .right-side").css('min-height', window_height - $('.main-footer').outerHeight());
            } else {
                var postSetWidth;
                if (window_height >= sidebar_height) {
                    $(".content-wrapper, .right-side").css('min-height', window_height - neg);
                    postSetWidth = window_height - neg;
                } else {
                    $(".content-wrapper, .right-side").css('min-height', sidebar_height);
                    postSetWidth = sidebar_height;
                }

                //Fix for the control sidebar height
                var controlSidebar = $(options.controlSidebarOptions.selector);
                if (typeof controlSidebar !== "undefined") {
                    if (controlSidebar.height() > postSetWidth)
                        $(".content-wrapper, .right-side").css('min-height', controlSidebar.height());
                }

            }
        };
        var fixSidebar = function () {
            //Make sure the body tag has the .fixed class
            if (!$("body").hasClass("fixed")) {
                if (typeof $.fn.slimScroll != 'undefined') {
                    $(".sidebar").slimScroll({ destroy: true }).height("auto");
                }
                return;
            } else if (typeof $.fn.slimScroll == 'undefined' && window.console) {
                window.console.error("Error: the fixed layout requires the slimscroll plugin!");
            }
            //Enable slimscroll for fixed layout
            if (options.sidebarSlimScroll) {
                if (typeof $.fn.slimScroll != 'undefined') {
                    //Destroy if it exists
                    $(".sidebar").slimScroll({ destroy: true }).height("auto");
                    //Add slimscroll
                    $(".sidebar").slimscroll({
                        height: ($(window).height() - $(".main-header").height()) + "px",
                        color: "rgba(0,0,0,0.2)",
                        size: "3px"
                    });
                }
            }
        };
        return {
            activate: activate,
            fix: fix,
            fixSidebar: fixSidebar
        };
    }]);

    /* PushMenu()
     * ==========
     * Adds the push menu functionality to the sidebar.
     *
     * @type Function
     * @usage: $.AdminLTE.pushMenu("[data-toggle='offcanvas']")
     */
    adminLTE.factory("adminLTE.pushMenu", ["adminLTEConfig", function (adminLTEConfig) {
        var options = adminLTEConfig.options;
        var activate = function (toggleBtn) {
            //Get the screen sizes
            var screenSizes = options.screenSizes;

            //Enable sidebar toggle
            $(toggleBtn).on('click', function (e) {
                e.preventDefault();

                //Enable sidebar push menu
                if ($(window).width() > (screenSizes.sm - 1)) {
                    if ($("body").hasClass('sidebar-collapse')) {
                        $("body").removeClass('sidebar-collapse').trigger('expanded.pushMenu');
                    } else {
                        $("body").addClass('sidebar-collapse').trigger('collapsed.pushMenu');
                    }
                }
                    //Handle sidebar push menu for small screens
                else {
                    if ($("body").hasClass('sidebar-open')) {
                        $("body").removeClass('sidebar-open').removeClass('sidebar-collapse').trigger('collapsed.pushMenu');
                    } else {
                        $("body").addClass('sidebar-open').trigger('expanded.pushMenu');
                    }
                }
            });

            $(".content-wrapper").click(function () {
                //Enable hide menu when clicking on the content-wrapper on small screens
                if ($(window).width() <= (screenSizes.sm - 1) && $("body").hasClass("sidebar-open")) {
                    $("body").removeClass('sidebar-open');
                }
            });

            //Enable expand on hover for sidebar mini
            if (options.sidebarExpandOnHover
                    || ($('body').hasClass('fixed')
                            && $('body').hasClass('sidebar-mini'))) {
                this.expandOnHover();
            }
        };
        var expandOnHover = function () {
            var _this = this;
            var screenWidth = options.screenSizes.sm - 1;
            //Expand sidebar on hover
            $('.main-sidebar').hover(function () {
                if ($('body').hasClass('sidebar-mini')
                        && $("body").hasClass('sidebar-collapse')
                        && $(window).width() > screenWidth) {
                    _this.expand();
                }
            }, function () {
                if ($('body').hasClass('sidebar-mini')
                        && $('body').hasClass('sidebar-expanded-on-hover')
                        && $(window).width() > screenWidth) {
                    _this.collapse();
                }
            });
        };
        var expand = function () {
            $("body").removeClass('sidebar-collapse').addClass('sidebar-expanded-on-hover');
        };
        var collapse = function () {
            if ($('body').hasClass('sidebar-expanded-on-hover')) {
                $('body').removeClass('sidebar-expanded-on-hover').addClass('sidebar-collapse');
            }
        };
        return {
            activate: activate,
            expandOnHover: expandOnHover,
            expand: expand,
            collapse: collapse
        };
    }]);

    /* Tree()
     * ======
     * Converts the sidebar into a multilevel
     * tree view menu.
     *
     * @type Function
     * @Usage: $.AdminLTE.tree('.sidebar')
     */
    adminLTE.factory("adminLTE.tree", ["adminLTEConfig", function (adminLTEConfig) {
        var _this = this;
        var animationSpeed = adminLTEConfig.options.animationSpeed;
        var activate = function (menu) {
            $(document).on('click', menu + ' li a', function (e) {
                var $this = $(this);
                //Get the clicked link and the next element
                var checkElement = $this.next();

                //Check if the next element is a menu and is visible
                if ((checkElement.is('.treeview-menu')) && (checkElement.is(':visible'))) {
                    //Close the menu
                    checkElement.slideUp(animationSpeed, function () {
                        checkElement.removeClass('menu-open');
                        //Fix the layout in case the sidebar stretches over the height of the window
                        //_this.layout.fix();
                    });
                    checkElement.parent("li").removeClass("active");
                }
                    //If the menu is not visible
                else if ((checkElement.is('.treeview-menu')) && (!checkElement.is(':visible'))) {
                    //Get the parent menu
                    var parent = $this.parents('ul').first();
                    //Close all open menus within the parent
                    var ul = parent.find('ul:visible').slideUp(animationSpeed);
                    //Remove the menu-open class from the parent
                    ul.removeClass('menu-open');
                    //Get the parent li
                    var parent_li = $this.parent("li");

                    //Open the target menu and add the menu-open class
                    checkElement.slideDown(animationSpeed, function () {
                        //Add the class active to the parent li
                        checkElement.addClass('menu-open');
                        parent.find('li.active').removeClass('active');
                        parent_li.addClass('active');
                        //Fix the layout in case the sidebar stretches over the height of the window
                        //_this.layout.fix();
                    });
                }
                else {
                    //Get the parent menu
                    var parent = $this.parents('ul').first();
                    //Close all open menus within the parent
                    var ul = parent.find('ul:visible').slideUp(animationSpeed);
                    //Remove the menu-open class from the parent
                    ul.removeClass('menu-open');
                    //Get the parent li
                    var parent_li = $this.parent("li");
                    //Add the class active to the parent li
                    parent.find('li.active').removeClass('active');
                    parent_li.addClass('active');
                }

                //if this isn't a link, prevent the page from being redirected
                if (checkElement.is('.treeview-menu')) {
                    if (checkElement.parent().children(":first-child").prop("href") == "") {
                        e.preventDefault();
                    }
                }
            });
        };
        return {
            activate: activate
        };
    }]);

    /* ControlSidebar
     * ==============
     * Adds functionality to the right sidebar
     *
     * @type Object
     * @usage $.AdminLTE.controlSidebar.activate(options)
     */
    adminLTE.factory("adminLTE.controlSidebar", ["adminLTEConfig", function (adminLTEConfig) {
        var options = adminLTEConfig.options;
        var activate = function () {
            //Get the object
            var _this = this;
            //Update options
            var o = options.controlSidebarOptions;
            //Get the sidebar
            var sidebar = $(o.selector);
            //The toggle button
            var btn = $(o.toggleBtnSelector);

            //Listen to the click event
            btn.on('click', function (e) {
                e.preventDefault();
                //If the sidebar is not open
                if (!sidebar.hasClass('control-sidebar-open')
                        && !$('body').hasClass('control-sidebar-open')) {
                    //Open the sidebar
                    _this.open(sidebar, o.slide);
                } else {
                    _this.close(sidebar, o.slide);
                }
            });

            //If the body has a boxed layout, fix the sidebar bg position
            var bg = $(".control-sidebar-bg");
            _this._fix(bg);

            //If the body has a fixed layout, make the control sidebar fixed
            if ($('body').hasClass('fixed')) {
                _this._fixForFixed(sidebar);
            } else {
                //If the content height is less than the sidebar's height, force max height
                if ($('.content-wrapper, .right-side').height() < sidebar.height()) {
                    _this._fixForContent(sidebar);
                }
            }
        };
        var open = function (sidebar, slide) {
            //Slide over content
            if (slide) {
                sidebar.addClass('control-sidebar-open');
            } else {
                //Push the content by adding the open class to the body instead
                //of the sidebar itself
                $('body').addClass('control-sidebar-open');
            }
        };
        var close = function (sidebar, slide) {
            if (slide) {
                sidebar.removeClass('control-sidebar-open');
            } else {
                $('body').removeClass('control-sidebar-open');
            }
        };
        var _fix = function (sidebar) {
            var _this = this;
            if ($("body").hasClass('layout-boxed')) {
                sidebar.css('position', 'absolute');
                sidebar.height($(".wrapper").height());
                $(window).resize(function () {
                    _this._fix(sidebar);
                });
            } else {
                sidebar.css({
                    'position': 'fixed',
                    'height': 'auto'
                });
            }
        };
        var _fixForFixed = function (sidebar) {
            sidebar.css({
                'position': 'fixed',
                'max-height': '100%',
                'overflow': 'auto',
                'padding-bottom': '50px'
            });
        };
        var _fixForContent = function (sidebar) {
            $(".content-wrapper, .right-side").css('min-height', sidebar.height());
        };
        return {
            activate: activate,
            open: open,
            close: close,
            _fix: _fix,
            _fixForFixed: _fixForFixed,
            _fixForContent: _fixForContent
        };
    }]);

    /* ------------------
     * - Implementation -
     * ------------------
     * The next block of code implements AdminLTE's
     * functions and plugins as specified by the
     * options above.
     */
    adminLTE.service("adminLTEService", ["adminLTEConfig", "adminLTE.layout", "adminLTE.pushMenu", "adminLTE.tree", "adminLTE.controlSidebar",
        function (adminLTEConfig, layout, pushMenu, tree, controlSidebar) {
            var options = adminLTEConfig.options;
            var init = function () {

                $("body").removeClass("hold-transition");

                layout.activate();

                //Enable sidebar tree view controls
                tree.activate('.sidebar');

                //Enable control sidebar
                if (options.enableControlSidebar) {
                    controlSidebar.activate();
                }

                //Add slimscroll to navbar dropdown
                if (options.navbarMenuSlimscroll && typeof $.fn.slimscroll != 'undefined') {
                    $(".navbar .menu").slimscroll({
                        height: options.navbarMenuHeight,
                        alwaysVisible: false,
                        size: options.navbarMenuSlimscrollWidth
                    }).css("width", "100%");
                }

                //Activate sidebar push menu
                if (options.sidebarPushMenu) {
                    pushMenu.activate(options.sidebarToggleSelector);
                }

                //Activate Bootstrap tooltip
                if (options.enableBSToppltip) {
                    $('body').tooltip({
                        selector: options.BSTooltipSelector
                    });
                }

                //Activate fast click
                if (options.enableFastclick && typeof FastClick != 'undefined') {
                    FastClick.attach(document.body);
                }

                //Activate direct chat widget
                if (options.directChat.enable) {
                    $(document).on('click', options.directChat.contactToggleSelector, function () {
                        var box = $(this).parents('.direct-chat').first();
                        box.toggleClass('direct-chat-contacts-open');
                    });
                }

                //Initialize Button Toggle
                $('.btn-group[data-toggle="btn-toggle"]').each(function () {
                    var group = $(this);
                    $(this).find(".btn").on('click', function (e) {
                        group.find(".btn.active").removeClass("active");
                        $(this).addClass("active");
                        e.preventDefault();
                    });
                });
            };
            return {
                init: init
            };
        }]);

});
