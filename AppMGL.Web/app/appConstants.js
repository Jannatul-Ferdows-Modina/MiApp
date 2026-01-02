define(['require'], function (r) {
    'use strict';
    var constants = {
        mainMenus: {
            shipment: {
                'quotation': [{ menu: 'Rate Capture Quotation', url: '/operation/ratecapturequotation', header: 'RCQ ' },
                    { menu: 'Create Quotation', url: '/operation/quotation', header: 'CQ' },
                    { menu: 'Pending Shipper Approval', url: '/operation/quotationApproval', header: 'PSA' },
                    { menu: 'Send for Booking', url: '/operation/quotationBooking', header: 'SB' },
                    { menu: 'Site Change', url: '/operation/sitechange', header: 'SC' }
                ],
                'booking': [{ menu: 'Booking To Do', url: '/operation/booking', header: 'BT' },
                    { menu: 'Waiting for Line Confirmation', url: '/operation/bookingDocument', header: 'CQ' },
                    { menu: 'Booking Conf Ready To Send', url: '/operation/bookingConfReadyToSend', header: 'BC' },
                    { menu: 'Awaited Shipper Confirmation', url: '/operation/bookingShipperConfirmation', header: 'AS' },
                    { menu: 'Edit/Rollover', url: '/operation/bookingSpace', header: 'E/R' },
                    { menu: 'Capture Expenses', url: '/operation/bookingCaptureExpenses', header: 'CE' },
                    { menu: 'Rollover Records', url: '/operation/bookingRollover', header: 'RR' }],
                'trucking': [{ menu: 'Finalize Transporation', url: '/operation/finalizedTransportation', header: 'FT' },
                    { menu: 'Dock Receipt', url: '/document/dockReceipt', header: 'DR' },
                    { menu: 'Pending Movement', url: '/operation/pendingMovement', header: 'PM' },
                    { menu: 'Capture Container & Seal No', url: '/operation/bookingCaptureContainer', header: 'CCS' }],
                'documentation': [{ menu: 'Pending Shipping Instruction', url: '/operation/documentation1', header: 'PSI' },
                    { menu: 'Pending filing of SED', url: '/operation/documentation2', header: 'PF' },
                    { menu: 'Pending B/L instruction Customer', url: '/operation/documentation3', header: 'PIC' },
                    { menu: 'Pending B/L instruction Line', url: '/operation/documentation4', header: 'PIL' },
                    { menu: 'Pending Draft B/L Line', url: '/operation/documentation5', header: 'PDL' },
                    { menu: 'Pending Send Draft B/L Customer', url: '/operation/documentation6', header: 'PSD' },
                    { menu: 'Pending Draft B/L Approval From Customer', url: '/operation/documentation7', header: 'PDA' },
                    { menu: 'Pending to Send Approved Draft B/L to Line', url: '/operation/documentation8', header: 'PSA' },
                    { menu: 'B/L Release Awaited From Line', url: '/operation/documentation9', header: 'RA' },
                    { menu: 'Container Abandonement', url: '/operation/bookingCaptureContainerAbandonement', header: 'CA' }]
            },
            contract: [{ menu: 'Contract Capture', url: '/operation/contractRate', header: 'CC', menuID: 3 },
                    { menu: 'Rate Capture', url: '/operation/rateCapture', header: 'RC', menuID: 3 }],
            document: [{ menu: 'HBL', url: '/document/hbl', header: 'HB', menuID: 5 },
                    { menu: 'MBL', url: '/document/mbl', header: 'MB', menuID: 5 },
                    { menu: 'CO', url: '/document/co', header: 'CO', menuID: 5 }],
            warehouse: [{ menu: 'Warehouse Location', url: 'datamanagement/warehouselocation', header: 'WL', menuID: 9 },
                      { menu: 'Warehouse Mapping', url: 'operation/warehousemapping', header: 'WM', menuID: 9 }],

            report: [{ menu: 'Enquiry', url: '/report/enquiryReport', header: 'E', menuID: 12 },
                    { menu: 'Quotation', url: '/report/quotationReport', header: 'Q', menuID: 12 },
                    { menu: 'Export Register Report', url: '/report/exportRegisterReport', header: 'ER', menuID: 12 },
                    { menu: 'Expense Report', url: '/report/expensesReport', header: 'ER', menuID: 12 },
                    { menu: 'Process Invoice Report', url: '/report/processInvoiceReport', header: 'PI', menuID: 12 },
                    { menu: 'Activity Due Report', url: '/report/activityDueReport', header: 'AD', menuID: 12 },
                    { menu: 'SP MCS', url: '/report/mcsReport', header: 'SP', menuID: 12 },
                    { menu: 'Booking', url: '/report/bookingReport', header: 'B', menuID: 12 },
                    { menu: 'Miami Booking Status', url: '/report/bookingStatusReport', header: 'MB', menuID: 12 },
                    { menu: 'Region Wise Container Report', url: '/report/regionWiseContainerReport', header: 'RW', menuID:12}
            ],
            administration: [{ menu: 'System Entities', header: 'SE', hasChild: true, menuID: 7 },
                    { menu: 'Security Entities', header: 'SE', hasChild: true, menuID: 7 },
                    { menu: 'Unit', url: '/security/site', header: 'U', menuID: 7 },
                    { menu: 'Customer Unit', url: '/security/customerUnit', header: 'CU', menuID: 7 },
                    { menu: 'User', url: '/security/user', header: 'U', menuID: 7 },
                    { menu: 'Vendor', url: '/operation/vendorCustomerContact', header: 'V' },
            { menu: 'Get QB Refresh Token', url: '/setup/qb', header: 'Q' }],
            'system entities': [{ menu: 'Country', url: '/setup/country', header: 'C', menuID: 7 },
                    { menu: 'State', url: '/setup/state', header: 'S', menuID: 7 },
                    { menu: 'Department', url: '/setup/department', header: 'D', menuID: 7 },
                    { menu: 'Ttile', url: '/setup/title', header: 'T', menuID: 7 },
                    { menu: 'Timezone', url: '/setup/timezone', header: 'TZ', menuID: 7 }
            ],
            'security entities': [{ menu: 'Module Type', url: '/security/moduleType', header: 'MT', menuID: 7 },
                    { menu: 'Module', url: '/security/module', header: 'M', menuID: 7 },
                    { menu: 'Action', url: '/security/action', header: 'A', menuID: 7 },
                    { menu: 'Role', url: '/security/role', header: 'R', menuID: 7 }
            ],
            'data management': [{ menu: 'Geographical', header: 'G', menuID: 6, hasChild: true },
                    { menu: 'Configurator', header: 'C', menuID: 6, hasChild: true }
            ],
            geographical: [{ menu: 'Region/Continent', url: '/datamanagement/continent', header: 'RC', menuID: 6 },
                    { menu: 'Country', url: '/datamanagement/siplcountry', header: 'C', menuID: 6 },
                    { menu: 'State', url: '/datamanagement/lgvwstate', header: 'S', menuID: 6 },
                    { menu: 'Port Group', url: '/datamanagement/portgroup', header: 'PG', menuID: 6 },
                    { menu: 'City', url: '/datamanagement/lgvwcity', header: 'C', menuID: 6 },
                    { menu: 'Rail Ramp', url: '/datamanagement/railramp', header: 'RR', menuID: 6 },
                    { menu: 'Destination Terminal', url: '/datamanagement/terminal', header: 'DT', menuID: 6 },
                    { menu: 'Surcharge Group', url: '/datamanagement/surchargegroup', header: 'SG', menuID: 6 },
                    { menu: 'Port', url: '/datamanagement/lgvwport', header: 'P', menuID: 6 },
            ],
            configurator: [{ menu: 'Contact Category', url: '/datamanagement/contactcategory', header: 'CC', menuID: 6 },
                    { menu: 'Commodity', url: '/datamanagement/commodity', header: 'C', menuID: 6 },
                    { menu: 'Container Type', url: '/datamanagement/containertype', header: 'CT', menuID: 6 },
                    { menu: 'Load Type', url: '/datamanagement/loadtype', header: 'LT', menuID: 6 },
                    { menu: 'Trade Service', url: '/datamanagement/tradeservice', header: 'TS', menuID: 6 },
                    { menu: 'Create Alias', url: '/datamanagement/lgvwalias', header: 'CA', menuID: 6 },
                    { menu: 'Account Category', url: '/datamanagement/lgacctcategory', header: 'AC', menuID: 6 },
                    { menu: 'SP FEE Category', url: '/datamanagement/lgspfeecategory', header: 'SP', menuID: 6 },
            ],
            aes: [{ menu: 'AES Submission', url: '/ees/eessub', header: 'AS', menuID: 501 }, { menu: 'Country', url: '/datamanagement/siplcountry', header: 'C', menuID: 502 },
                { menu: 'State', url: '/datamanagement/lgvwstate', header: 'S', menuID: 503 }
                   
            ],
            crm: [{ menu: 'Account Management', url: '/crm/accountmanagement', header: 'AM', menuID: 528 },
                { menu: 'Lead Generation', url: '/crm/leadgeneration', header: 'LG', menuID: 529 }

            ],
        }
    };
    return constants;
});
