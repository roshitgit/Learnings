$(document).ready(function () {
    try {
        var substr = window.location.search.substring(1).split('&');
        GlobalVars.SOWID = substr[0].split('=')[1];
        GlobalVars.year = substr[1].split('=')[1];
        GlobalVars.ProjectID = substr[2].split('=')[1];
        GlobalVars.Page = substr[3].split('=')[1];
        GlobalVars.GOCCode = substr[4].split('=')[1];
        GlobalVars.GOCName = substr[5].split('=')[1];
        SOWDetails.LoadTemplates();
        SOWDetails.CompileTemplates();
        SOWDetails.GetSOWInfo();
    }
    catch (Err) {
        SOWDetails.SendMailOnException(Err.description);
        return false;
    }
});

var GlobalVars = {
    year: '',
    SOWStartYear: '',
    SOWEndYear: '',
    GOCCode: '',
    GOCName: '',
    SOWID: '',
    ProjectID: '',
    Page: '',
    DoFormat: true,
    symbol: '$',
    positiveFormat: '%s%n',
    negativeFormat: '(%s%n)',
    digitGroupSymbol: ','
};

var SOWDetails = {
    LoadTemplates: function () {
        $.ajax({
            url: '/ClientTemplates/SOWTemplates.htm',
            type: "GET",
            async: false,
            cache: false,
            success: function (data) {
                $('body').append(data);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(jqXHR.responseText);
            }
        });
    },
    CompileTemplates: function () {
        $.template("compiledProjTemplate", $('#AssignedProjectTemplate'));
        $.template("compiledForecastTemplate", $('#FPCForecastTemplate'));
        $.template("compiledTotActForecastTemplate", $('#TotalActualForecastTemplate'));
        $.template("compiledOverUnderTemplate", $('#OverUnderTemplate'));
    },
    GetSOWInfo: function (value) {
        $('#hdnDisplayYear').val(GlobalVars.year);
        if (value != undefined && value != '') {
            if (GlobalVars.SOWEndYear == '' || GlobalVars.SOWStartYear == '') {
                alert("Navigation to next or previous year is disallowed for this SOW");
                return false;
            }
            GlobalVars.year = parseInt($('#lblSOWYear').text()) + value;
        }
        else {
            GlobalVars.year = parseInt($('#hdnDisplayYear').val());
            $('#hdnSOWID').val(GlobalVars.SOWID);
        }

        if (GlobalVars.SOWStartYear != '' &&
            (parseInt(GlobalVars.year) < parseInt(GlobalVars.SOWStartYear))) {
            alert("Financials do not exist for the previous year");
            return false;
        }
        if (GlobalVars.SOWEndYear != '' && (parseInt(GlobalVars.year) > parseInt(GlobalVars.SOWEndYear))) {
            alert("Financials do not exist for the next year");
            return false;
        }
        $("#hdnDisplayYear").val(GlobalVars.year);
        $('#lblSOWYear').text($("#hdnDisplayYear").val());
        this.ClearSOWTables('gvFPCProjects', true);
        this.ClearSOWTables('tblTotal', true);
        this.ClearSOWTables('tblSOW', true);
        this.MakeAjaxCall();

    },
    MakeAjaxCall: function () {
        $.ajax({
            url: "SOWDetails/" + $('#hdnSOWID').val() + "/" + GlobalVars.year,
            type: "GET",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: true,
            cache: false,
            beforeSend: function () {
                SOWDetails.showProgress();
            },
            complete: function () {
                SOWDetails.hideProgress();
            },
            success: function (data) {
                SOWDetails.BindTemplate(data);
                SOWDetails.setCellColor();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                var responseText = jqXHR.responseText;
                if (typeof (responseText) != 'undefined' &&
                            (responseText.toString().toLowerCase().indexOf("siteminder") >= 0
                                    || responseText.toString().toLowerCase().indexOf("html") >= 0)) {
                    SOWDetails.RedirectOnTimeout();
                }
                else if (jqXHR == null || textStatus === 'timeout') {
                    alert('Session expired,Restart Browser');
                    SOWDetails.RedirectOnTimeout();
                    return false;
                }
                else {
                    alert("Request Error /SOWDetail/MakeAjaxCall/. \njqXHR: " + jqXHR + "\n textStatus: " + textStatus + "\n errorThrown: " + errorThrown);
                    SOWDetails.SendMailOnException(errorThrown);
                }
            }
        });
    },
    SendMailOnException: function (errorThrown) {
        if (parent.location.toString().toLowerCase().indexOf('pgprojecttab') >= 0)
            parent.HandleException("SOWDetail.js", "MakeAjaxCall", "MakeAjaxCall function Exception - " + errorThrown);
        else if (parent.opener != undefined && parent.opener.parent != undefined)
            parent.opener.parent.HandleException("SOWDetail.js", "MakeAjaxCall", "MakeAjaxCall function Exception - " + errorThrown);
    },
    RedirectOnTimeout: function () {
        if (parent.location.toString().toLowerCase().indexOf('pgprojecttab') >= 0)
            parent.location.href = '../Common/pgDashboard.aspx';
        else if (parent.opener != undefined && parent.opener.parent != undefined)
            parent.opener.parent.location.href = '../Common/pgDashboard.aspx';
    },
    SetProjectName: function (ProjectID, ProjectName) {
        var name = ProjectID + " - " + ProjectName;
        if (screen.width <= 1024) {
            if (name.toString().length > 20) {
                return name.substr(0, 20) + "..";
            }
            else { return name; }
        }
        else {
            if (name.toString().length > 30) {
                return name.substr(0, 30) + "..";
            }
            else { return name; }
        }
    },
    RedirectToMPF: function () {
        var winl = 5;
        var wint = 5;
        if (screen.width == 1024) {
            var options = "width=" + (screen.width - 30);
            options += "px,height=" + (screen.height - 70) + "px";
            options += ",top=" + wint;
            options += ",left=" + winl;
            options += ",location=no,toolbar=no, menubar=no, scrollbars=1, resizable=1";
        }
        else if (screen.width >= 1280) {
            var options = "width=" + (screen.width - 30);
            options += "px,height=" + (screen.height - 100) + "px";
            options += ",top=" + wint;
            options += ",left=" + winl;
            options += ",location=no,toolbar=no, menubar=no, scrollbars=0, resizable=1";
        }
        else {
            var options = "width=" + (screen.width - 30);
            options += "px,height=" + (screen.height - 70) + "px";
            options += ",top=" + wint;
            options += ",left=" + winl;
            options += ",location=no,toolbar=no, menubar=no, scrollbars=1, resizable=1";
        }
        var SOWID = $('#lblSOWNo').text().split('-')[0];
        var GOCCode = GlobalVars.GOCCode;
        var GOCName = GlobalVars.GOCName;
        var year = GlobalVars.year;
        var url = '../Projects/pgMPFSearchMn.aspx?CallerName=pgMPFAll&SOW_ID=' + SOWID + '&GocCode=' + GOCCode + '&Year=' + year + '&GOC_Name= ' + GOCName;
        window.open(url, '', options);
    },
    RedirectToFinancialPage: function (projectId) {
        if (GlobalVars.Page.toLowerCase() == 'projfin') {
            parent.window.location.href = '../Projects/pgProjectTab.aspx?ProjectId=' + projectId + '&TABID=1';
        }
        else {
            if (window.parent.opener != undefined) {
                var pageName = window.parent.opener.location.toString().toLowerCase();
                if (pageName.indexOf('pgsowdetail') >= 0) {
                    window.parent.opener.parent.location.href = '../Projects/pgProjectTab.aspx?ProjectId=' + projectId + '&TABID=1';
                }
                else if (pageName.indexOf('pgsowexception') >= 0) {
                    window.parent.opener.parent.location.href = '../Projects/pgProjectTab.aspx?ProjectId=' + projectId + '&TABID=1';
                }
                else {
                    window.parent.opener.location.href = '../Projects/pgProjectTab.aspx?ProjectId=' + projectId + '&TABID=1';
                }
            }
            parent.window.close();
        }
    },
    BindTemplate: function (data) {
        //For year navigation
        if (data.SOWLifeCycle.length > 0) {
            var SOWLifeCycle = data.SOWLifeCycle;
            GlobalVars.SOWStartYear = SOWLifeCycle[0].SOWStartYear;
            GlobalVars.SOWEndYear = SOWLifeCycle[0].SOWEndYear;
        }
        var tblForecastInfo = $('#tblSOW tbody');
        var tBodySOWProjects = $('#gvFPCProjects tbody');
        var tblTotal = $('#tblTotal tbody');

        /* bind each template with array */
        if (data.fpcForecast.length > 0) {
            var fpcForecast = data.fpcForecast;
            $.tmpl("compiledForecastTemplate", fpcForecast).appendTo(tblForecastInfo);
            $('#lblSOWNo').text(fpcForecast[0].SOWID);
            $('#lblSOWName').text(fpcForecast[0].SOWName);
            $('#lblSOWManager').text(fpcForecast[0].SOWManager);
            $('#lblGOC').text(fpcForecast[0].GOCCode);
            $('#lblVendor').text(fpcForecast[0].Vendor);
        }
        else {
            $('#NoRecordsTemplate').tmpl({ 'Message': 'No records Found.', 'ColSpan': '13' }).appendTo(tblForecastInfo);
        }
        if (data.projectsAssigned.length > 0) {
            var projectsAssigned = data.projectsAssigned;
            $.tmpl("compiledProjTemplate", projectsAssigned).appendTo(tBodySOWProjects);
            $("#aProjId").live("click", function () {
                selectedItem = $.tmplItem(this);
                var projId = selectedItem.data.ProjectID.split('-')[0];
                SOWDetails.RedirectToFinancialPage(projId);
            });
        }
        else {
            $('#NoRecordsTemplate').tmpl({ 'Message': 'No records Found.', 'ColSpan': '16' }).appendTo(tBodySOWProjects);
        }
        if (data.totalActualForecast.length > 0) {
            var totalActualForecast = data.totalActualForecast;
            $.tmpl("compiledTotActForecastTemplate", totalActualForecast).appendTo(tblTotal);
        }
        if (data.overUnder.length > 0) {
            var overUnder = data.overUnder;
            $.tmpl("compiledOverUnderTemplate", overUnder).appendTo(tblTotal);
            GlobalVars.DoFormat = true;
        }
        else {
            GlobalVars.DoFormat = false;
        }
        $('#tblProjects tr').addClass("gridRowStyleSOW");
        this.HandleRedirectionAndStyles();
        if ($('#tblProjects tr').length > 13)
            $('#divAssignedProjects').addClass("SOWAssignedProjects");
        this.SetCellWidthBasedOnHeader();
    },
    SetCellWidthBasedOnHeader: function () {
        var arr = new Array();
        $('#tblHeader tr th').each(function (i) {
            arr.push($(this).css("width"));
        });

        if ($('#tblSOW tr td').length > 1) {
            $('#tblSOW tr td').each(function (i) {
                $(this).css("width", arr[i].toString());
            });
        }

        if ($('#gvFPCProjects tr td').length > 1) {
            $('#gvFPCProjects tr').each(function () {
                $('td', this).each(function (i) {
                    $(this).css("width", arr[i].toString());
                });
            });
        }

        if ($('#tblTotal tr:first td').length > 0) {
            $('#tblTotal tr:first td:nth-child(2)').css("width", arr[3].toString());
            $('#tblTotal tr:first td:nth-child(3)').css("width", arr[4].toString());
            $('#tblTotal tr:first td:nth-child(4)').css("width", arr[5].toString());
            $('#tblTotal tr:first td:nth-child(5)').css("width", arr[6].toString());
            $('#tblTotal tr:first td:nth-child(6)').css("width", arr[7].toString());
            $('#tblTotal tr:first td:nth-child(7)').css("width", arr[8].toString());
            $('#tblTotal tr:first td:nth-child(8)').css("width", arr[9].toString());
            $('#tblTotal tr:first td:nth-child(9)').css("width", arr[10].toString());
            $('#tblTotal tr:first td:nth-child(10)').css("width", arr[11].toString());
            $('#tblTotal tr:first td:nth-child(11)').css("width", arr[12].toString());
            $('#tblTotal tr:first td:nth-child(12)').css("width", arr[13].toString());
            $('#tblTotal tr:first td:nth-child(13)').css("width", arr[14].toString());
            $('#tblTotal tr:first td:nth-child(14)').css("width", arr[15].toString());
            $('#tblTotal tr:first td:nth-child(15)').css("width", arr[16].toString());
            $('#tblTotal tr:first td:nth-child(16)').css("width", arr[17].toString());
            $('#tblTotal tr:first td:nth-child(17)').css("width", arr[18].toString());
        }
        if ($('#tblTotal tr:last td').length > 0) {
            $('#tblTotal tr:last td:nth-child(2)').css("width", arr[3].toString());
            $('#tblTotal tr:last td:nth-child(3)').css("width", arr[4].toString());
            $('#tblTotal tr:last td:nth-child(4)').css("width", arr[5].toString());
            $('#tblTotal tr:last td:nth-child(5)').css("width", arr[6].toString());
            $('#tblTotal tr:last td:nth-child(6)').css("width", arr[7].toString());
            $('#tblTotal tr:last td:nth-child(7)').css("width", arr[8].toString());
            $('#tblTotal tr:last td:nth-child(8)').css("width", arr[9].toString());
            $('#tblTotal tr:last td:nth-child(9)').css("width", arr[10].toString());
            $('#tblTotal tr:last td:nth-child(10)').css("width", arr[11].toString());
            $('#tblTotal tr:last td:nth-child(11)').css("width", arr[12].toString());
            $('#tblTotal tr:last td:nth-child(12)').css("width", arr[13].toString());
            $('#tblTotal tr:last td:nth-child(13)').css("width", arr[14].toString());
            $('#tblTotal tr:last td:nth-child(14)').css("width", arr[15].toString());
            $('#tblTotal tr:last td:nth-child(15)').css("width", arr[16].toString());
            $('#tblTotal tr:last td:nth-child(16)').css("width", arr[17].toString());
            $('#tblTotal tr:last td:nth-child(17)').css("width", arr[18].toString());
        }
    },
    HandleRedirectionAndStyles: function () {
        if (GlobalVars.Page == 'MPF') {
            $('#divAssignedProjects').css('height', '350px');
            $('#acrMPFLink').css('cursor', 'auto').css('textDecoration', 'none');
            document.getElementById('acrMPFLink').onclick = function () {
                return false;
            }
        }
    },
    formatCurrency: function (num) {
        //num = (isNaN(parseFloat(num))) ? '0.00' : num;
        num = (isNaN(parseFloat(num))) ? '' : num;
        if (num == '') return num;
        num = parseFloat(num).toFixed(2);
        var sign = (num < 0) ? '-' : '';
        var a = num.split('.', 2);
        var d = a[1];
        var i = a[0];                       //modified to display positive/negative nos.
        var isPositive = (i == Math.abs(parseInt(i)));

        var n = new String(i);
        if (n.indexOf('-') >= 0)
            n = n.replace('-', '');
        var b = [];
        while (n.length > 3) {
            var nn = n.substr(n.length - 3);
            b.unshift(nn);
            n = n.substr(0, n.length - 3);
        }
        if (n.length > 0) { b.unshift(n); }
        n = b.join(GlobalVars.digitGroupSymbol);
        num = n;

        // format symbol/negative
        var format = isPositive ? GlobalVars.positiveFormat : GlobalVars.negativeFormat;
        var money = format.replace(/%s/g, GlobalVars.symbol);
        money = money.replace(/%n/g, num).replace('-', '');
        return money;
    },
    setCellColor: function () {
        $('#tblTotal tr:last td').each(function (i) {
            var cell = $(this)[0];
            if (i >= 1 && GlobalVars.DoFormat) {
                var replacedString = cell.innerText.replace(/,/g, '').replace('$', ''); //if -ve, retain black color.
                if (parseInt(replacedString) > 0)
                    $(cell).css('color', 'red');
            }
        });
    },
    hideProgress: function () {
        $("#FPCDetailsLoading").css("display", "none");
    },
    showProgress: function () {
        $("#FPCDetailsLoading").css("display", "block");
    },
    ClearSOWTables: function (id, applyTrigger) {
        $('#' + id + ' tr').remove();
        if (applyTrigger)
            $('#' + id).trigger("update");
    }
};

