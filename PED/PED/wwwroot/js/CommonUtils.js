function InitControls() {
    var errFlag = false;
    "use strict";
    $(".DateMask").mask("99/99/9999");
    var startDateVal = '';
    var endDateVal = '';
    var today = '';
    var date = new Date();
    today = new Date(date.getFullYear(), date.getMonth(), date.getDate());
    var hrfYear = $('#hfReviewYear').val();
    if (hrfYear !== null && hrfYear != undefined) {
        if (hrfYear !== '') {
            var revYears = hrfYear.split(',');
            var yearArray = [];
            $.each(revYears, function (i) {
                yearArray.push(revYears[i]);
            });

            var first = yearArray[0];
            var last = yearArray[yearArray.length - 1];
            if (date.getFullYear() === last) {
                startDateVal = '01-01-' + first;
                endDateVal = '-1d';
            }
            else {
                startDateVal = '01-01-' + first;
                endDateVal = '12-31-' + last;
            }
        }
    }
    else {
        startDateVal = '01-01-' + date.getFullYear();
        endDateVal = '-1d';
    }

    $('.datepicker-past').datepicker({
        todayHighlight: true,
        autoclose: true,
        drops: "down",
        //endDate: endDateVal,
        //startDate: startDateVal,
        startDate: '01-01-1900',
        clearBtn: true
        //endDate: '3-12-31'
    });

    $('.datepicker-autoClose').datepicker({
        todayHighlight: true,
        format: 'mm/dd/yyyy',
        //endDate: '+0d',
        autoclose: true
    });

    $('.datepicker-current').datepicker({
        todayHighlight: true,
        autoclose: true,
        startDate: '1d'
    });


    function CheckSessionTimeOut(req) {
        var url;
        if (req.status === 403) {
            var resp = $.parseJSON(req.responseText);

            if (resp.type === "SESSION_TIMEOUT") {
                url = $("#hrfUrlSessionExpiry").val();
                window.location.href = url;
            }
        }
        else {

            url = $("#hrfUrlAppError").val();
            window.location.href = url;
        }
    }


    function ScrollToTop() {
        $('html, body').animate({
            scrollTop: $("#page-container").offset().top
        }, 2000);
    }

    function GetCurrentDateTime() {
        now = new Date();
        year = "" + now.getFullYear();
        month = "" + (now.getMonth() + 1); if (month.length === 1) { month = "0" + month; }
        day = "" + now.getDate(); if (day.length === 1) { day = "0" + day; }
        hour = "" + now.getHours(); if (hour.length === 1) { hour = "0" + hour; }
        minute = "" + now.getMinutes(); if (minute.length === 1) { minute = "0" + minute; }
        second = "" + now.getSeconds(); if (second.length === 1) { second = "0" + second; }
        return year + "-" + month + "-" + day + "_" + hour + "" + minute + "" + second;
    }
}